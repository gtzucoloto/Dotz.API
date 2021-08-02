using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotz.API
{
    public static class Settings
    {
        private const string KEY = "MY_BIG_SECURE_KEY_ytyYTFYTFYTytf65ffyTFTY#$FTRD7#@@@!6f563wdqu()*)qy8&yd6878egdguagyg";
        public static byte[] ASCII_KEY { get => Encoding.ASCII.GetBytes(KEY); }
    }
}
