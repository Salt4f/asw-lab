using System;
using System.ComponentModel.DataAnnotations;

namespace HackerNewsASW.Models
{
    public class Contribucio
    {
        //variables
        public string Url { get; set; }

        public int Points { get; set; }

        public string Author { get; set; }

        public DateTime Date { get; set; }
        
        public string Title { get; set; }

        [Key]
        public ulong Id { get; set; }

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