# **WIPE.SH IS A DANGEROUS FILE.  THE FIRST LINE STOPS THE PROGRAM FROM DELETING THE FILE SYSTEM BUT PLEASE BE CAREFUL.  REMOVE THE FIRST LINE ONLY WHEN YOU HAVE PUT THE FILE INTO THE VM**  #

**Not Complete - Server needs static IP which will be put in all C# programs**

Malware Analysis requires competitors to stop a rogue program on their VM from destroying the VM.  However they also have to feed the program information in order to find the flag.

# Server Installation #

run the following command


```
#!bash

docker build .
```

# Client Installation #

Client must not have a password or password must be put into wipe.sh

copy ClientPart1.cs, wipe.sh, and rc.local to the VM

```
#!bash

sudo apt-get update

sudo apt install mono-mcs

mcs ClientPart1.cs

sudo cp ClientPart1.exe /bin

sudo cp wipe.sh /bin

sudo cp rc.local /etc

sudo chmod 755 /bin/wipe.sh

sudo chmod 755 /bin/ClientPart1.exe

sudo chmod 755 /etc/rc.local
```



# Solution #
There are many ways to solve this challenge.  One of the easiest ways is to intercept the connection Client makes with Server.  Intercept Server’s first message to Client and send your own message.  Let Server and Client continue to communicate.  Eventually Client will connect to another address on the Server.  Hit the new address with a POST request and it will send the flag.  

The flag is MCA-7190CE16.