using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerNewsASW.Models
{
    public class User
    {
        //variables 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

        //[InverseProperty("Author")]
        public ICollection<Contribution> Contributions { get; set; }

        

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