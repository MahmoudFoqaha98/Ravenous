using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ravenous.Models
{
    [MetadataType(typeof(mealCategoryMetaData))]
    public partial class mealCategory
    {
        // Add new properties or methods
    }
    public class mealCategoryMetaData
    {
        // Edit properties



        [Required(ErrorMessage ="* رقـم الـوجـبـة مـطـلـوب")]
        [Display(Name ="رقـم الـوجـبـة")]
        public int Id { get; set; }




        [Required(ErrorMessage ="* نـوع الـوجـبـة مـطـلـوب")]
        [Display(Name = "نـوع الـوجـبـة")]
        public string type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meal> meals { get; set; }
    }
}