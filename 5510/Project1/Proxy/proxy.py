from socket import *
from urllib.parse import urlparse
import sys
from pathlib import Path

class ProxyServer:
    def __init__(self, port):
        self.server_socket = self.create_server_socket(port)
        self.server_port = 80
        self.buf_size = 1024

    def create_server_socket(self, port):
        try:
            server_socket = socket(AF_INET, SOCK_STREAM)
            server_socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
            server_socket.bind(('', port))
            server_socket.listen()
            return server_socket
        except socket.error as err:
            raise Exception("Socket creation failed with error %s".format(err))

    def is_msg_length_valid(self, msg):
        return len(msg.split()) == 3

    def parse_http_msg(self, msg):
        msg_components = msg.split()
        method = msg_components[0]
        url = msg_components[1]
        version = msg_components[2]
        return method, url, version

    def is_http_version_correct(self, version):
        return version == 'HTTP/1.1'

    def is_method_get(self, method):
        return method == 'GET'

    def get_cache_path(self, url):
        parsed_url = urlparse(url)
        return Path('./cache/' + parsed_url.hostname + parsed_url.path)

    def cache_exists(self, url):
        return self.get_cache_path(url).exists()

    def read_cache(self, url):
        path = self.get_cache_path(url)
        file = path.open()
        return file.read()

    def write_cache(self, url, msg):
        path = self.get_cache_path(url)
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(msg)

    def create_msg_to_server(self, url):
        path = urlparse(url).path
        host = urlparse(url).hostname
        return "GET {} HTTP/1.1\r\nHost: {}\r\nConnection: close\r\n\r\n".format(path, host)

    def get_response_status(self, msg):
        try:
            version, status, reason = msg.split(None, 2)
            return status
        except ValueError:
            return -1

    def process_server_message(self, url, msg):
        version = self.get_response_status(msg)
        if version == "200":
            print("Response received from the server, and status code is 200! Writing to cache for future use...")
            self.write_cache(url, msg)
        if version not in ["200", "404"]:
            return "500 Internal Server Error\n"
        return msg

    def run(self):
        while True:
            print('\n\n\n****************************** Ready to serve... ******************************')
            conn_socket, addr = self.server_socket.accept()
            client_ip, client_port = conn_socket.getpeername()
            print("Received a client connection from: (\'{}\', {})".format(client_ip, client_port))
            client_msg = conn_socket.recv(self.buf_size).decode()
            print("Received a message from this client: b\'{}\'".format(client_msg))

            if not self.is_msg_length_valid(client_msg):
                server_msg = "Message length incorrect. Should be 3."
            else:
                method, url, version = self.parse_http_msg(client_msg)
                if not self.is_method_get(method):
                    server_msg = "Method incorrect. Should be GET."
                elif not self.is_http_version_correct(version):
                    server_msg = "HTTP version incorrect. Should be HTTP/1.1."
                else:
                    if self.cache_exists(url):
                        print("Yeah! The requested file is in the cache and is about to be sent to the client!")
                        server_msg = self.read_cache(url)
                    else:
                        print("Oops! No cache hit! Requesting origin server for the file...")
                        client_socket = socket(AF_INET, SOCK_STREAM)
                        msg_to_server = self.create_msg_to_server(url)
                        client_socket.connect((urlparse(url).hostname, self.server_port))
                        print("Sending the following message from proxy to server:\n", msg_to_server)
                        client_socket.send(msg_to_server.encode())

                        server_msg = client_socket.recv(self.buf_size).decode()
                        server_msg = self.process_server_message(url, server_msg)
                        
                        client_socket.close()
                        print("Now responding to the client...")

            conn_socket.send(server_msg.encode())
            conn_socket.close()
            print("All done! Closing socket...")

if __name__ == "__main__":
    if len(sys.argv) <= 1:
        print('Usage : "python proxy.py port_number"\n[port_number] : It is the Port Number Of Proxy Server')
        sys.exit(2)

    port_number = int(sys.argv[1])
    proxy_server = ProxyServer(port_number)
    proxy_server.run()
