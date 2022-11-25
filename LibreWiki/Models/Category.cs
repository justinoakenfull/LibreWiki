using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LibreWiki.Models
{
    public class Category
    {

        [DisplayName("CategoryID")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
#nullable enable
        public int? ParentCategoryId { get; set; }

        public virtual Category? ParentCategory { get; set; }

        public virtual ICollection<Category>? ChildenCategories { get; set; }

        public virtual ICollection<Product> Products { get; set; }
#nullable disable
    }
}
