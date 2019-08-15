using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required (ErrorMessage="Category {0} is required")]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
    }
}