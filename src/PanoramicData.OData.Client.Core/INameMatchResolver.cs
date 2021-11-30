namespace PanoramicData.OData.Client;

public interface INameMatchResolver
{
    bool IsMatch(string actualName, string requestedName);
}
