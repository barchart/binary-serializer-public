dotnet build -c Release
dotnet pack -c Release --output ./output

#example of packagePath - ./output/binary-serialization-net.1.0.0.nupkg
$packagePath = Get-ChildItem -Path ./output -Filter *.nupkg -Recurse | Select-Object -First 1
dotnet nuget push $packagePath.FullName --api-key YourApiKey --source https://api.nuget.org/v3/index.json