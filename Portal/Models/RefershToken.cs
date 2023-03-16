using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class RefershToken
    {
        [Key]
        public int RefreshId { get; set; }
        public string Jwtid { get; set; }
        public string Token { get; set; }
        public Boolean IsRevoked { get; set; }
        public DateTime DateAded { get; set; }
        public DateTime DateExpires { get; set; }

        public string Id { get; set; }
        [ForeignKey(nameof(Id))]
        public IdentityUser User { get; set; }

    }
}
