using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApi.Entities
{
    public partial class DrugGroupDetail
    {
        [JsonIgnore]
        public int DrugGroupCode { get; set; }
        [JsonIgnore]
        public int DrugCode { get; set; }

        public virtual Drug Drug { get; set; }
        [JsonIgnore]
        public virtual DrugGroup DrugGroup { get; set; }
    }
}