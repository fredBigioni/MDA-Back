using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductGroup
    {

       public ProductGroup()
        {
            CustomMarketDetails = new HashSet<CustomMarketDetail>();
            ProductGroupDetails = new HashSet<ProductGroupDetail>();
        }
        public int Code { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomMarketDetail> CustomMarketDetails { get; set; }

        public virtual ICollection<ProductGroupDetail> ProductGroupDetails { get; set; }
    }
}
