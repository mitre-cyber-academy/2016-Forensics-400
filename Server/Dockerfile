# escape=`

FROM ubuntu:14.04
RUN apt-get update && apt-get install -y build-essential
RUN apt install -y mono-mcs

COPY ServerPart1.cs /bin
COPY ServerPart2.cs /bin
COPY rc.local /etc

RUN mcs /bin/ServerPart1.cs
RUN mcs /bin/ServerPart2.cs

RUN chmod 755 /bin/ServerPart1.exe
RUN chmod 755 /bin/ServerPart2.exe
RUN chmod 755 /etc/rc.local