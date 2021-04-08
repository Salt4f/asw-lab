using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerNewsASW.Models
{
    public abstract class Contribution
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        //variables
        public int Upvotes { get; set; }
        
        public string Content { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        //[InverseProperty("UsuariId")]
        public User Author { get; set; }

        public ICollection<Contribution> Comments { get; set; }

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