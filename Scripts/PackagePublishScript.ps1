dotnet build -c Release
dotnet pack -c Release --output ./output
dotnet nuget push ./output/*.nupkg --api-key YourApiKey --source https://api.nuget.org/v3/index.json