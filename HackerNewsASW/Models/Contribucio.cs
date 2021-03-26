using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerNewsASW.Models
{
    public class Contribucio
    {
        //variables
        public string Url { get; set; }

        public int Points { get; set; }

        //[InverseProperty("UsuariId")]
        public Usuari Author { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }
        
        public string Title { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

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