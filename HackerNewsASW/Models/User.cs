using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackerNewsASW.Models
{
    public class User
    {
        public string UserId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Email { get; set; }

        public string About { get; set; }

        public DateTime DateCreated { get; set; }

        //[InverseProperty("Author")]
        public ICollection<Contribution> Contributions { get; set; }

        public ICollection<Contribution> Upvoted { get; set; }

    }
}