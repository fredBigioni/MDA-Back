using System;
using System.Collections.Generic;

#nullable disable

namespace WebApi.Entities
{
    public partial class UserPermission
    {
        public int Code { get; set; }
        public int UserId { get; set; }
        public int? LineGroupCode { get; set; }
        public int? LineCode { get; set; }
        public int? CustomMarketCode { get; set; }
        public bool? FullAccess { get; set; }
    }
}
