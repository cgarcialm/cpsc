from socket import *

if __name__ == "__main__":
  # server_name = 'zhiju.me'
  # server_ip = '54.218.17.199'
  proxy_ip = ''
  proxy_port = 65432

  client_socket = socket(AF_INET, SOCK_STREAM)
  client_socket.connect((proxy_ip, proxy_port))

  msg = input('Input message: ')
  client_socket.send(msg.encode())

  modified_msg = client_socket.recv(1024)
  print(modified_msg.decode())
  print('client_socket ' , client_socket)

  client_socket.close()