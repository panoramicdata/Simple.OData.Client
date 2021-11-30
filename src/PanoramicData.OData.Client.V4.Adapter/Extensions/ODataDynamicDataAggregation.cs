using PanoramicData.OData.Client.V4.Adapter.Extensions;

namespace PanoramicData.OData.Client;

public static class ODataDynamicDataAggregation
{
    public static DynamicDataAggregation Builder => new();

    public static DynamicAggregationFunction AggregationFunction => new();
}
