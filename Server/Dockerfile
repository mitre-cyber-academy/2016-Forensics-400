FROM phusion/baseimage
RUN apt-get update 
RUN apt-get install -y build-essential
RUN apt install -y mono-mcs

COPY ServerPart1.cs /bin
COPY ServerPart2.cs /bin

RUN mcs /bin/ServerPart1.cs
RUN mcs /bin/ServerPart2.cs

RUN chmod 755 /bin/ServerPart1.exe
RUN chmod 755 /bin/ServerPart2.exe

ADD start_serv /etc/service/
RUN chmod 755 /etc/service/

EXPOSE 21

CMD ["/sbin/my_init"]