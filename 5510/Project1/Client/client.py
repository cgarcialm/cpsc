from socket import *

if __name__ == "__main__":
  proxy_ip = ''
  proxy_port = 65432

  client_socket = socket(AF_INET, SOCK_STREAM)
  client_socket.connect((proxy_ip, proxy_port))

  # msg = input('Input message: ')
  msg = 'GET http://zhiju.me/networks/valid.html HTTP/1.1'
  # msg = 'GET http://example.com/index.html HTTP/1.1'
  client_socket.send(msg.encode())

  modified_msg = client_socket.recv(1024)
  print(modified_msg.decode())
  print('client_socket ' , client_socket)

  client_socket.close()