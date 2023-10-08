from socket import *
from urllib.parse import urlparse
import sys
from pathlib import Path


def is_msg_length_valid(msg):
    return len(msg.split()) == 3

def parse_http_msg(msg):

  msg_components = msg.split()
  
  method = msg_components[0]
  url = msg_components[1]
  version = msg_components[2]

  return method, url, version

def is_http_version_correct(version):
  return version == 'HTTP/1.1'

def is_method_get(method):
  return method == 'GET'

def cache_exists(url):
  parsed_url = urlparse(url)
  path = Path('./cache/' + parsed_url.hostname + parsed_url.path)

  return path.exists()

def read_cache(url):
  parsed_url = urlparse(url)
  path = Path('./cache/' + parsed_url.hostname + parsed_url.path)
  file = path.open()

  return file.read()

def create_msg_to_server(url):
  path = urlparse(url).path
  host = urlparse(url).hostname
  return "GET {} HTTP/1.1\r\nHost: {}\r\nConnection: close\r\n\r\n".format(path, host)


if __name__ == "__main__":

  if len(sys.argv) <= 1:
    print('Usage : "python proxy.py port_number"\n[port_number] : It is the Port Number Of Proxy Server')
    sys.exit(2)

  # Create a server socket
  try:
    server_socket = socket(AF_INET, SOCK_STREAM)
  except socket.error as err:
    raise Exception("Socket creation failed with error %s".format(err))
  
  # Bind it to the given port and start listening
  server_socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
  server_socket.bind(('', int(sys.argv[1])))
  server_socket.listen()

  print('The proxy server is ready to receive...')
  while True:

    server_port = 80
    buf_size = 1024

    # Accept incoming request and create new socket for client
    conn_socket, addr = server_socket.accept()
    print('con_socket: {} | addr: {}'.format(str(conn_socket), str(addr)))

    # Read message from the socket
    client_msg = conn_socket.recv(1024).decode()
    print('client msg: {}'.format(client_msg))

    if not is_msg_length_valid(client_msg):
      server_msg = 'Message length incorrect. Should be 3.'.encode()
    else:
      method, url, version = parse_http_msg(client_msg)
      if not is_method_get(method):
        server_msg = 'Method incorrect. Should be GET.'.encode()
      elif not is_http_version_correct(version):
        server_msg = 'HTTP version incorrect. Should be HTTP/1.1.'.encode()
      else:

        if cache_exists(url):
          server_msg = read_cache(url).encode()
        else:
          client_socket = socket(AF_INET, SOCK_STREAM)
          print('client_socket ' , client_socket)
          msg_to_server = create_msg_to_server(url)
          client_socket.connect((urlparse(url).hostname, server_port))
          client_socket.send(msg_to_server.encode())
          # client_socket.send("GET /networks/valid.html HTTP/1.1\r\nHost: zhiju.me\r\nConnection: close\r\n\r\n".encode())
          server_msg = client_socket.recv(buf_size)
          print(server_msg.decode())
          client_socket.close()
    
    # Send back msg
    conn_socket.send(server_msg)

    # Close connection socket
    conn_socket.close()