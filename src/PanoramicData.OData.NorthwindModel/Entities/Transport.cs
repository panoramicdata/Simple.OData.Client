using System.ComponentModel.DataAnnotations;

namespace PanoramicData.OData.NorthwindModel.Entities
{
    public abstract class Transport
    {
        [Key]
        public int TransportID { get; set; }
        [Required]
        public int TransportType { get; set; }
    }
}