dotnet build -c Release
dotnet pack -c Release --output ./output

$packagePath = Get-ChildItem -Path ./output -Filter *.nupkg -Recurse | Select-Object -First 1
dotnet nuget push $packagePath.FullName --api-key YourApiKey --source https://api.nuget.org/v3/index.json
