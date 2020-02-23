using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPWebService.Persistence
{
    public class AppDbOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public short Port { get; set; }

        public bool IsNull()
            => string.IsNullOrEmpty(UserName)
            || string.IsNullOrEmpty(Password)
            || string.IsNullOrEmpty(Host)
            || Port <= 0;
    }
}