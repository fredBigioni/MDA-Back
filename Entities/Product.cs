using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class Product
    {
        public Product()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            ProductGroupDetails = new HashSet<ProductGroupDetail>();
            ProductPresentations = new HashSet<ProductPresentation>();
        }

        public int Code { get; set; }

        public string Imscode { get; set; }

        [JsonIgnore]
        public int LaboratoryCode { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public string RawDescription { get; set; }

        [JsonIgnore]
        public DateTime? LaunchingDate { get; set; }

        public virtual Laboratory Laboratory { get; set; }

        public bool? IsModified { get; set; }

        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductGroupDetail> ProductGroupDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<ProductPresentation> ProductPresentations { get; set; }
    }
}
