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

RUN chmod 755 /etc/service/

RUN mkdir /etc/service/serverpart1
ADD serverpart1.sh /etc/service/serverpart1/run
RUN chmod +x /etc/service/serverpart1/run


RUN mkdir /etc/service/serverpart2
ADD serverpart2.sh /etc/service/serverpart2/run
RUN chmod +x /etc/service/serverpart2/run

EXPOSE 21

CMD ["/sbin/my_init"]