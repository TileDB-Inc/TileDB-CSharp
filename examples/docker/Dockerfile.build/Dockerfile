FROM ubuntu:21.04

ARG DEBIAN_FRONTEND=noninteractive
ENV TZ=America/New_York

RUN apt-get update && \
  apt-get install -y wget build-essential gdb git python3-pip tzdata && \
  pip install cmake && \
  wget https://packages.microsoft.com/config/ubuntu/21.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
  dpkg -i packages-microsoft-prod.deb && \
  rm packages-microsoft-prod.deb

RUN apt-get update; \
  apt-get install -y apt-transport-https && \
  apt-get update && \
  apt-get install -y dotnet-sdk-5.0

#--

COPY ./ /TileDB-CSharp/

RUN set -x && \
    cd /TileDB-CSharp && \
    cd cpp && \
    mkdir build && cd build && \
    cmake .. && \
    cmake --build . --target install --config Release && \
    cd /TileDB-CSharp/TileDB.CSharp && \
    dotnet build /p:Platform=x64 -c Release && \
    cd /TileDB-CSharp/TileDB.CSharp/benchmark && \
    dotnet build /p:Platform=x64 -c Release

RUN cd /TileDB-CSharp/examples/TileDB.Example && \
    dotnet build /p:Platform=x64 -c Release && \
    ./bin/x64/Release/net5.0/TileDB.Example
