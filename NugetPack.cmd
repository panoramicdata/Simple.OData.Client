call nuget pack PanoramicData.OData.Client.nuspec	-Symbols -Version %1 -OutputDirectory build\packages -SymbolPackageFormat snupkg
call nuget pack PanoramicData.OData.V3.Client.nuspec -Symbols -Version %1 -OutputDirectory build\packages -SymbolPackageFormat snupkg
call nuget pack PanoramicData.OData.V4.Client.nuspec -Symbols -Version %1 -OutputDirectory build\packages -SymbolPackageFormat snupkg
