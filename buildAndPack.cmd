@if "%1"=="" goto usage

:build
dotnet build PanoramicData.OData.Client.sln -c Release -p:VersionPrefix=%1
@goto pack

:pack
call nuget pack PanoramicData.OData.Client.nuspec	-Symbols -Version %1 -OutputDirectory build\packages -SymbolPackageFormat snupkg
call nuget pack PanoramicData.OData.V3.Client.nuspec -Symbols -Version %1 -OutputDirectory build\packages -SymbolPackageFormat snupkg
call nuget pack PanoramicData.OData.V4.Client.nuspec -Symbols -Version %1 -OutputDirectory build\packages -SymbolPackageFormat snupkg
@goto end

:usage
 echo Usage: buildAndPack version-number
 @goto end

:end