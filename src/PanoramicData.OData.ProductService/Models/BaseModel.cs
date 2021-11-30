using System;

// ReSharper disable CheckNamespace
namespace PanoramicData.OData.ProductService.Models
// ReSharper restore CheckNamespace
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }
    }
}
