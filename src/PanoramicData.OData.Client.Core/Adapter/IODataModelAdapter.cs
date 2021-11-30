namespace PanoramicData.OData.Client;

public interface IODataModelAdapter
{
    AdapterVersion AdapterVersion { get; }
    string ProtocolVersion { get; set; }
    object Model { get; set; }
}
