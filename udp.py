from socket import *

multicast_port  = 1235
multicast_group = "232.1.1.1"
interface_ip    = "127.0.0.1"

s = socket(AF_INET, SOCK_DGRAM )
s.bind(("", multicast_port ))
mreq = inet_aton(multicast_group) + inet_aton(interface_ip)
s.setsockopt(IPPROTO_IP, IP_ADD_MEMBERSHIP, str(mreq))

while 1:
    print(s.recv(1500))