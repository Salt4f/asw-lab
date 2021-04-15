using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNewsASW.Models
{
    public class Ask : Contribution
    {
        public string Title { get; set; }
        public override string getTitle()
        {
            return Title;
        }
    }
}
