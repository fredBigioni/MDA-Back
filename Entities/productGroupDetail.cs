using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductGroupDetail
    {
        [JsonIgnore]
        public int ProductGroupCode { get; set; }
        [JsonIgnore]
        public int ProductCode { get; set; }

        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual ProductGroup ProductGroup { get; set; }
    }
}
