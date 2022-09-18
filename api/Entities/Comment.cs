using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Entities
{
    public class Comment
    {
        private int id;
        private DateTime dateCreated = DateTime.Today;
        private string title = string.Empty;
        private string text = string.Empty;
        private bool isEditted = false;
        private DateTime? dateEditted;
        private bool isFeatured = false;

        //todo: add buyerId to indicate who is commenting
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Key]
        public int Id { get => id; set => id = value; }
        public DateTime DateCreated { get => dateCreated; set => dateCreated = value; }
        public string Title { get => title; set => title = value; }
        public string Text { get => text; set => text = value; }
        public bool IsEditted { get => isEditted; set => isEditted = value; }
        public DateTime? DateEditted { get => dateEditted; set => dateEditted = value; }
        public bool IsFeatured { get => isFeatured; set => isFeatured = value; }

    }
}