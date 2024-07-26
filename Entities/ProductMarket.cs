using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class ProductMarket
    {
        public int? ProductCode { get; set; }
        public int? ProductPresentationGroupCode { get; set; }
        public int MarketCode { get; set; }
    }
}
