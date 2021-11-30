using System;

namespace PanoramicData.OData.Client.Tests
{
    public class TypeWithIndexer
    {
        public string Name { get; set; }

        public char this[int index] => Name[index];
    }
}