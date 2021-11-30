namespace PanoramicData.OData.Client.V4.Adapter.Extensions;

public sealed class DynamicAggregationFunction
{
    internal DynamicAggregationFunction()
    {
    }

	public (string, ODataExpression) Average(ODataExpression expression) => (nameof(Average), expression);

	public (string, ODataExpression) Sum(ODataExpression expression) => (nameof(Sum), expression);

	public (string, ODataExpression) Min(ODataExpression expression) => (nameof(Min), expression);

	public (string, ODataExpression) Max(ODataExpression expression) => (nameof(Max), expression);

	public (string, ODataExpression) Count() => (nameof(Count), null);

	public (string, ODataExpression) CountDistinct(ODataExpression expression) => (nameof(CountDistinct), expression);
}
