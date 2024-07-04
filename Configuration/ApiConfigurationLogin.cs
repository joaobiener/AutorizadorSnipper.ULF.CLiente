using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutorizadorSnipper.ULF.Cliente.Configuration
{
    public class ApiConfigurationLogin
    {
        public  string? BaseAddress { get; set; }
        public int Timeout { get; set; } = 2;
    }
}
