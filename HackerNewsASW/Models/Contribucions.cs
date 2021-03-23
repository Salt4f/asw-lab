using System;

namespace HackerNewsASW.Models
{
    public class Contribucions
    {
        //variables
        private string url { get; set; }

        private int points { get; set; }

        private string author { get; set; }

        private DateTime date { get; set; }

        private string title { get; set; }

        //constructora
        public Contribucions (string link, int punts, string autor, DateTime data, string titol)
        {
            url = link;
            points = punts;
            author = autor;
            date = data;
            title = titol;
        }


        //funcions que criden al controlador i es el controlador el que executa merdes.

    }
}