from socket import *
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


if __name__ == "__main__":
  print('Number of arguments:', len(sys.argv), 'arguments.')
  print('Argument List:', str(sys.argv))

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
        server_msg = 'Message read in 127.0.0.1 {}'.format(sys.argv[1]).encode()
    
    # Send back msg
    conn_socket.send(server_msg)

    # Close connection socket
    conn_socket.close()