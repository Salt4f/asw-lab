using System;

namespace HackerNewsASW.Models
{
    public class Usuaris
    {
        //variables 
    
        private string usuariId { get; set; }

        private string email { get; set; }

        private DateTime dateCreated { get; set; }

        //constructora

        public Usuaris(string userId, string mail, DateTime dataCreacio)
        {
            usuariId = userId;
            email = mail;
            dateCreated = dataCreacio;
        }

        //funcions que cridaran als controladors i ells faran merdes
    }
}