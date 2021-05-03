using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public string Token { get; set; }

        [InverseProperty("Author")]
        public ICollection<Contribution> Contributions { get; set; }

        [InverseProperty("Upvoters")]
        public ICollection<Contribution> Upvoted { get; set; }

    }
}
