using System;
using System.Collections.Generic;
using System.Text;

namespace TdsHelper.Models
{
    public class UserMapOptions
    {
        public string PgAuthId { get; set; }
        public SqlCredential MapFor { get; set; }
    }
}
