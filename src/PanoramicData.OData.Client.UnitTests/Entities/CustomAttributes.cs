using System;

namespace PanoramicData.OData.Client.Tests
{
    public class NotMappedAttribute : Attribute
    {
    }

    public class DataAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class ColumnAttribute : DataAttribute
    {
    }
}