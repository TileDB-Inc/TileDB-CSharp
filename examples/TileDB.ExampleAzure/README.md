This directory contains an example creating, writing, and reading a TileDB
array stored in Azure Blob Storage.

### Usage

Fill in the configuration variables ("vfs.azure.*") in `ExampleAzure.cs`
before running `dotnet run`. Alternatively, these may be set as
environment variables as documented in the [Azure CLI documentation](https://docs.microsoft.com/en-us/azure/storage/blobs/authorize-data-operations-cli#set-environment-variables-for-authorization-parameters)
(AZURE_STORAGE_ACCOUNT, AZURE_STORAGE_KEY, AZURE_STORAGE_SAS_TOKEN).
