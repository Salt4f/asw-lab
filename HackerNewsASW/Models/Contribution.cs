using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerNewsASW.Models
{
    public abstract class Contribution
    {
        public DateTime DateCreated { get; set; }

        //variables
        public int Upvotes { get; set; }
        
        public string Content { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        //[InverseProperty("Contributions")]
        public User Author { get; set; }

        //[InverseProperty("Upvoted")]
        public ICollection<User> Upvoters { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public abstract string getTitle();

        //constructora
        /*public Contribucio (string link, int punts, string autor, DateTime data, string titol)
        {
            Url = link;
            Points = punts;
            Author = autor;
            Date = data;
            Title = titol;
        }*/


        //funcions que criden al controlador i es el controlador el que executa merdes.

    }
}
