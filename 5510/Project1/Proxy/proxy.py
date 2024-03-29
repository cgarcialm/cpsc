"""
Author: Cecilia Garcia Lopez de Munain
Version: 1.0

This script defines two classes, ProxyClient and ProxyServer, for a basic HTTP 
proxy server.

ProxyClient handles communication with the origin server, generating requests, 
getting and accumulating responses.

ProxyServer manages client connections, serving cached responses or forwarding 
requests to the origin server through a ProxyClient and caching valid responses.

Usage:
The main entry of the program accepts a port number via the command line for 
server configuration. It instantiates and runs a proxy server, executing it with
the desired port number. Only if html not in cache, a proxy client is created
to request the origin server:
```
python proxy.py port_number
```
where 'port_number' is the port for the proxy server.
"""

from socket import *
from urllib.parse import urlparse
import sys
from pathlib import Path

class URL:
    """
    A class to parse an URL into its components.
    """
    DEFAULT_HTTP_PORT = 80

    def __init__(self, url):
        """
        Initialize a URL instance.

        :param str url: The URL.
        """
        self.parsed_url = urlparse(url)
        self.host = self.parsed_url.hostname
        self.port = self.parsed_url.port or self.DEFAULT_HTTP_PORT  # Default port for HTTP
        self.path = self.parsed_url.path

class ProxyClient:
    """
    ProxyClient is responsible for handling communication with the origin 
    server.
    """

    BUF_SIZE = 1024  # The buffer size for receiving data from the server

    def __init__(self, url, headers=""):
        """
        Initialize a ProxyClient instance.

        :param str url: The URL of the origin server.
        :param str headers: (Optional) Additional HTTP headers to include in 
        the request.
        """
        self.url = url
        self.headers = headers

    def create_http_request_to_server(self):
        """
        Create an HTTP request message to be sent to the origin server.

        :return: The HTTP request message.
        :rtype: str
        """
        request_msg = (
            "GET {} HTTP/1.1\r\n"
            "Host: {}\r\n"
            "Connection: close\r\n"
            .format(self.url.path, self.url.host)
        )
        if self.headers:
            request_msg += self.headers
        request_msg += "\r\n"

        return request_msg

    def receive_http_response(self, client_socket):
        """
        Receive an HTTP response from the origin server, accumulating both 
        headers and body.

        :param client_socket: The socket connected to the origin server.
        :type client_socket: socket.socket
        :return: The accumulated HTTP response data.
        :rtype: bytes
        :raises Exception: If the HTTP response exceeds the maximum allowed 
        size.
        """
        MAX_RESPONSE_SIZE = 16 * 1024 * 1024  # 16MB
        received_data = b""

        while True:
            chunk = client_socket.recv(self.BUF_SIZE)
            if not chunk:
                break

            received_data += chunk

            if len(received_data) >= MAX_RESPONSE_SIZE:
                raise Exception(
                    "HTTP response exceeds the maximum allowed size"
                )

        return received_data

    def get_and_process_server_msg(self):
        """
        Get and process the HTTP response from the origin server based on the 
        client request.

        :return: The HTTP response message to send back to the client.
        :rtype: str
        """
        client_socket = socket(AF_INET, SOCK_STREAM)
        client_socket.connect((self.url.host, self.url.port))
        msg_to_server = self.create_http_request_to_server()
        print(
            "Sending the following message from proxy to"
            " server:\r\n",
            msg_to_server,
        )
        client_socket.send(msg_to_server.encode())

        # Receive and process the response from the origin server
        server_msg = self.receive_http_response(client_socket).decode()

        client_socket.close()

        return server_msg

class ProxyServer:
    """
    ProxyServer is a simple HTTP proxy server that handles client requests and
    caches responses.
    """

    BUF_SIZE = 1024  # The buffer size for receiving data from clients and 
                     # from server

    def __init__(self, port):
        """
        Initialize the ProxyServer with the given port number.

        :param int port: Port number for the proxy server.
        """
        self.port = port
        self.server_socket = self.create_server_socket()

    def create_server_socket(self):
        """
        Create and configure the server socket.

        :param int port: Port number for the server socket.
        :return: The created server socket.
        :rtype: socket.socket
        :raises Exception: If socket creation fails.
        """
        try:
            server_socket = socket(AF_INET, SOCK_STREAM)
            server_socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
            server_socket.bind(("", self.port))
            server_socket.listen()
            return server_socket
        except socket.error as err:
            raise Exception("Socket creation failed with error %s".format(err))

    def is_valid_http_message_length(self, msg):
        """
        Check if the length of the received message is valid (should be 3).

        :param str msg: The received message.
        :return: True if the message length is valid, False otherwise.
        :rtype: bool
        """

        return len(msg.split()) >= 3

    def parse_http_request(self, msg):
        """
        Parse an HTTP message into its components.

        :param str msg: The HTTP message.
        :return: A tuple containing the method, URL, version, and headers.
        :rtype: tuple
        """

        msg_components = msg.split()
        method = msg_components[0]
        url = URL(msg_components[1])
        version = msg_components[2]
        try:
            headers = " ".join(msg_components[3:])
        except:
            headers = ""
        return method, url, version, headers

    def is_valid_http_version(self, version):
        """
        Check if the HTTP version is correct (should be 'HTTP/1.1').

        :param str version: The HTTP version.
        :return: True if the version is correct, False otherwise.
        :rtype: bool
        """
        return version == "HTTP/1.1"

    def is_http_get_method(self, method):
        """
        Check if the HTTP method is GET.

        :param str method: The HTTP method.
        :return: True if the method is GET, False otherwise.
        :rtype: bool
        """

        return method == "GET"

    def is_http_url_valid(self, url):
        """
        Check if the URL is well-formed (correctly structured).

        :param str url: The URL.
        :return: True if URL is valid, False otherwise.
        :rtype: bool
        """
        try:
            return all([url.parsed_url.scheme, url.parsed_url.netloc])
        except:
            return False

    def get_cache_file_path(self, url):
        """
        Get the cache file path for a given URL.

        :param str url: The URL.
        :return: The cache file path.
        :rtype: pathlib.Path
        """
        return Path("./cache/{}/{}{}".format(url.host, url.port, url.path))

    def cache_exists(self, url):
        """
        Check if a cache file exists for a given URL.

        :param str url: The URL.
        :return: True if a cache file exists, False otherwise.
        :rtype: bool
        """
        return self.get_cache_file_path(url).exists()

    def read_cache_file_contents(self, url):
        """
        Read the contents of a cache file for a given URL.

        :param str url: The URL.
        :return: The contents of the cache file.
        :rtype: str
        """
        path = self.get_cache_file_path(url)
        file = path.open()
        return file.read()

    def write_to_cache_file(self, url, msg):
        """
        Write an HTTP response to a cache file for a given URL.

        :param str url: The URL.
        :param str msg: The HTTP response message.
        """
        path = self.get_cache_file_path(url)
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(msg)

    def get_http_response_status(self, msg):
        """
        Get the HTTP response status code.

        :param str msg: The HTTP response message.
        :return: The HTTP response status code.
        :rtype: str
        """
        try:
            version, status, reason = msg.split(None, 2)
            return status
        except ValueError:
            return -1

    def process_http_response_from_server(self, url, msg):
        """
        Process the HTTP response message from the origin server.

        :param str url: The URL.
        :param str msg: The HTTP response message.
        :return: The processed HTTP response message.
        :rtype: str
        """
        version = self.get_http_response_status(msg)

        if len(msg) > 0 and version in ["200", "404"]:
            # Extract the response body after headers
            msg = msg[msg.find("\r\n\r\n") :]

            if version == "200":
                # Cache the response and modify cache headers for a 200 response
                print(
                    "Response received from the server, and status code is"
                    " 200! Writing to cache for future use..."
                )
                self.write_to_cache_file(
                    url, "HTTP/1.1 200 OK\r\nCache Hit: 1\r\n" + msg
                )
                msg = "HTTP/1.1 200 OK\r\nCache Hit: 0\n" + msg
            else:
                # Modify cache headers for a 404 response
                msg = "HTTP/1.1 404 Not Found\r\nCache Hit: 0\r\n" + msg

            return msg

        # Handle unexpected response
        return "HTTP/1.1 500 Internal Server Error\r\nCache Hit: 0"

    def validate_client_request(self, client_msg):
        """
        Validate the client's HTTP request and return corresponding response 
        if error found. Otherwise, None.

        :param str client_msg: The client's HTTP request message.
        :return: The server's response message.
        :rtype: str
        """
        if not self.is_valid_http_message_length(client_msg):
            return "Message length incorrect. Should be >= 3."

        method, url, version, _ = self.parse_http_request(client_msg)

        if not self.is_http_get_method(method):
            return "Method incorrect. Should be GET."

        if not self.is_http_url_valid(url):
            return "Invalid URL."

        if not self.is_valid_http_version(version):
            return "HTTP version incorrect. Should be HTTP/1.1."

        return None  # No validation issues found

    def run(self):
        """
        Run the proxy server to handle client requests.

        The method continuously listens for client connections and serves 
        multiple clients concurrently.

        This method does not return and runs indefinitely until manually 
        terminated.
        """
        while True:
            print(
                "\r\n\r\n\r\n****************************** Ready to serve..."
                " ******************************"
            )
            conn_socket, _ = self.server_socket.accept()
            client_ip, client_port = conn_socket.getpeername()
            print(
                "Received a client connection from: ('{}', {})".format(
                    client_ip, client_port
                )
            )
            client_msg = conn_socket.recv(self.BUF_SIZE)
            print("Received a message from this client: {}".format(client_msg))

            # Get and process the server response based on the client request
            client_msg = client_msg.decode()

            validation_result = self.validate_client_request(client_msg)
            if validation_result is not None:
                # Handle validation issues
                server_msg = validation_result
            else:
                _, url, _, headers = self.parse_http_request(
                    client_msg
                    )
                if self.cache_exists(url) and not headers:
                    # Serve from cache if the requested file is present
                    print(
                        "Yeah! The requested file is in the cache and is"
                        " about to be sent to the client!"
                    )
                    server_msg = self.read_cache_file_contents(url)
                else:
                    # Request the file from the origin server
                    print(
                        "Oops! No cache hit! Requesting origin server for"
                        " the file..."
                    )
                    # Use the ProxyClient class to handle communication with the
                    # origin server
                    proxy_client = ProxyClient(url, headers)
                    server_msg = proxy_client.get_and_process_server_msg()
                    server_msg = self.process_http_response_from_server(
                        url, server_msg
                        )

                    print("Now responding to the client...")

            # Send the response to the client
            server_msg += "\r\n\r\n"
            conn_socket.send(server_msg.encode())
            conn_socket.close()
            print("All done! Closing socket...")

if __name__ == "__main__":
    if len(sys.argv) <= 1:
        print(
            "Usage: \"python proxy.py port_number\"\r\n"
            "[port_number]: It is the port number of proxy server"
        )
        sys.exit(2)

    port_number = int(sys.argv[1])
    proxy_server = ProxyServer(port_number)
    proxy_server.run()
