FROM mcr.microsoft.com/dotnet/sdk:5.0.302-alpine3.13-amd64

ENV LD_LIBRARY_PATH /usr/local/lib64

RUN apk add --no-cache gcc g++ zlib zlib-dev cmake make bzip2 bzip2-dev zstd-dev zstd curl-dev curl libcurl lz4 lz4-dev spdlog git openssl-dev autoconf patch linux-headers

RUN cd /tmp && \
  git clone https://github.com/TileDB-Inc/TileDB.git -b 2.3.3 && \
  cd TileDB && \
  mkdir build && \
  cd build && \
  cmake .. -DTILEDB_SERIALIZATION=ON -DTILEDB_S3=ON -DTILEDB_AZURE=ON && \
  make -j$(nproc) && \
  make install-tiledb && \
  rm -rf /tmp/TileDB

ADD . TileDB-CSharp
WORKDIR TileDB-CSharp

RUN cmake -B cpp/build -S cpp && \
  cmake --build cpp/build --target install

RUN dotnet build TileDB.CSharp && \
  dotnet build TileDB.CSharp/benchmark && \
  dotnet build examples/TileDB.Example && \
  dotnet build test
