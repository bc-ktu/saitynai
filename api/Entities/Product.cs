using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Product
    {
        private int id;
        private decimal? price;
        private string title = string.Empty;
        private string type = string.Empty;
        private string description = string.Empty;
        private int quantity = 1;
        private bool isNew = false;

        public Array<> MyProperty { get; set; }

        // add author, isArchived

        [Key]
        public int Id { get => id; set => id = value;}

        [Required(ErrorMessage = "Required field")]
        [MaxLength(30, ErrorMessage = "This field must contain between 3 and 30 characters")]
        [MinLength(3, ErrorMessage = "This field must contain between 3 and 30 characters")]
        public string Title { get => title; set => title = value; }

        [Required(ErrorMessage = "Required field")]
        [MaxLength(30, ErrorMessage = "This field must contain between 3 and 30 characters")]
        [MinLength(3, ErrorMessage = "This field must contain between 3 and 30 characters")]
        public string Type { get => type; set => type = value; }

        [Required(ErrorMessage = "Required field")]
        [MaxLength(256, ErrorMessage = "This field must contain between 3 and 30 characters")]
        [MinLength(3, ErrorMessage = "This field must contain between 3 and 30 characters")]
        public string Description { get => description; set => description = value; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public int Quantity { get => quantity; set => quantity = value; }

        [Range(1, int.MaxValue, ErrorMessage = "The price must be greater than zero")]
        public decimal? Price { get => price; set => price = value; }

        public bool IsNew { get => isNew; set => isNew = value; }
    }
}