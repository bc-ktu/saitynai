using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Entities
{
    public class Order
    {
        private int id;
        private string status = string.Empty;
        private DateTime dateCreated = DateTime.Today;
        private DateTime? dateEditted;

        [Key]
        public int Id { get => id; set => id = value; }
        public string Status { get => status; set => status = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public DateTime? DateEditted { get => dateEditted; set => dateEditted = value; }


    }
}