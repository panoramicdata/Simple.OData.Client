nuget pack ./PanoramicData.OData.Client.nuspec -Symbols -Version $args[0] -OutputDirectory build/packages -SymbolPackageFormat snupkg
nuget pack ./PanoramicData.OData.V3.Client.nuspec -Symbols -Version $args[0] -OutputDirectory build/packages -SymbolPackageFormat snupkg
nuget pack ./PanoramicData.OData.V4.Client.nuspec -Symbols -Version $args[0] -OutputDirectory build/packages -SymbolPackageFormat snupkg
