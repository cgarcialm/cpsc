from socket import *
from urllib.parse import urlparse
import sys


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

def get_cache_path(url):
  parsed_url = urlparse(url)
  path = './cache/' + parsed_url.hostname + parsed_url.path
  file = open(path, "r")

  return file.read()

def create_msg_to_server(url):
  path = get_cache_path(url)

  return "GET {} HTTP/1.1\r\nHost: zhiju.me\r\nConnection: close\r\n\r\n".format(path)


if __name__ == "__main__":

  server_name = 'zhiju.me'
  server_port = 80

  if len(sys.argv) <= 1:
    print('Usage : "python proxy.py server_ip"\n[server_ip : It is the IP Address Of Proxy Server')
    sys.exit(2)

  # Create a server socket, bind it to a port and start listening
  server_socket = socket(AF_INET, SOCK_STREAM)
  server_socket.setsockopt(SOL_SOCKET, SO_REUSEADDR, 1)
  server_socket.bind(('', int(sys.argv[1])))
  server_socket.listen()

  print('The proxy server is ready to receive...')
  while True:
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
        msg_to_server = create_msg_to_server(url)

        client_socket = socket(AF_INET, SOCK_STREAM)
        client_socket.connect((server_name, server_port))
        # client_socket.send(msg_to_server.encode())
        client_socket.send(b"GET /networks/valid.html HTTP/1.1\r\nHost: zhiju.me\r\nConnection: close\r\n\r\n")
        server_msg = client_socket.recv(1024)
        print(server_msg.decode())
        print('client_socket ' , client_socket)
    
    # Send back msg
    conn_socket.send(server_msg)

    # Close connection socket
    conn_socket.close()