using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsASW.Models
{
    public class Tupla
    {
        public Comment Parent { get; set; }
        public HashSet<Tupla> Children { get; set; }

    }
}
