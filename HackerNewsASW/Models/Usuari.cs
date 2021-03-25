using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerNewsASW.Models
{
    public class Usuari
    {
        //variables 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UsuariId { get; set; }

        public string Email { get; set; }

        public DateTime DateCreated { get; set; }

        //constructora

       /* public Usuari(string userId, string mail, DateTime dataCreacio)
        {
            UsuariId = userId;
            Email = mail;
            DateCreated = dataCreacio;
        }*/

        //funcions que cridaran als controladors i ells faran merdes
    }
}