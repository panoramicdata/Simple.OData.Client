call nuget push build\packages\PanoramicData.OData.Client.%1.nupkg	-Source https://api.nuget.org/v3/index.json -apikey %2
call nuget push build\packages\PanoramicData.OData.V3.Client.%1.nupkg -Source https://api.nuget.org/v3/index.json -apikey %2
call nuget push build\packages\PanoramicData.OData.V4.Client.%1.nupkg -Source https://api.nuget.org/v3/index.json -apikey %2