using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AspNetCoreTodo.Models
{
    public class TodoItemAddViewModel
    {
        public Guid Id { get; set; }
        public bool IsDone { get; set; }
        [Required]
        public string Title { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public string UserId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public List<Category> Categories { get; set; }
    }
}