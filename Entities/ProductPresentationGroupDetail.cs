using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductPresentationGroupDetail
    {
        [JsonIgnore]
        public int ProductPresentationGroupCode { get; set; }
        [JsonIgnore]
        public int ProductPresentationCode { get; set; }

        public virtual ProductPresentation ProductPresentation { get; set; }
        [JsonIgnore]
        public virtual ProductPresentationGroup ProductPresentationGroup { get; set; }
    }
}
