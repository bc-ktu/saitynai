using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Authorization.Model;
using api.Data.Entities;
using api.Models;

namespace api.Entities
{
    public class Comment : IUserOwnedResource
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "Privalomas laukas")]
        [MaxLength(30, ErrorMessage = "Šis laukas turi nuo 3 iki 30 simbolių")]
        [MinLength(3, ErrorMessage = "Šis laukas turi nuo 3 iki 30 simbolių")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Privalomas laukas")]
        [MaxLength(180, ErrorMessage = "Šis laukas turi nuo 3 iki 180 simbolių")]
        [MinLength(3, ErrorMessage = "Šis laukas turi nuo 3 iki 180 simbolių")]
        public string Text { get; set; }
        public bool IsEditted { get; set; }
        public DateTime? DateEditted { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsDeleted { get; set; }
        public RegisteredUser? Author { get; set; } // keisti į ID
        public string? AuthorId { get { return UserId; } set { UserId = value; } } // keisti į ID
        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        public int ProductId { get; set; }

    }
}