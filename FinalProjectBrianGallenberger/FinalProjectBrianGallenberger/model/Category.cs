using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectBrianGallenberger.model
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please enter a name!")]
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
