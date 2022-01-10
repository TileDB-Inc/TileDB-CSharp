FROM mcr.microsoft.com/dotnet/sdk:5.0

RUN cd /home && \
  git clone https://github.com/TileDB-Inc/TileDB-CSharp.git -b bd/ch13349-metadata-example && \
  cd TileDB.CSharp/examples/TileDB.Metadata && \
  dotnet publish "metadata.csproj" -c Release -o /app/example_metadata
WORKDIR /app 
RUN cd /app/example_metadata

 