using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsASW.Models
{
    public class Comment : Contribution
    {
        public Contribution Commented { get; set; }
    }
}
