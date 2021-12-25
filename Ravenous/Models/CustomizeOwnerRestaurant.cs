using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ravenous.Models
{
    [MetadataType(typeof(ownerRestaurantMetaData))]
    public partial class ownerRestaurant
    {
        // add new methods or properties
    }

    class ownerRestaurantMetaData
    {
        // Edit properties


        [Required(ErrorMessage = "* رقم المطعم مطلوب")]
        [Display(Name ="رقم المطعم")]
        public int Id { get; set; }






        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$",ErrorMessage ="* الرجاء ادخال بريد الكتروني صالح")]
        [MaxLength(50,ErrorMessage ="* البريد الالكتروني يجب أن لا يتجاوز 50 حرف")]
        [Required(ErrorMessage ="* البريد الالكتروني مطلوب")]
        [Display(Name = "البريد الالكتروني")]
        public string email { get; set; }





        [MaxLength(100,ErrorMessage ="* اسم المطعم يجب أن لا يتجاوز عن 100 حرف")]
        [Required(ErrorMessage ="* اسم المطعم مطلوب")]
        [Display(Name = "اسم المطعم")]
        public string restaurantName { get; set; }





        [RegularExpression(@"^0(59[987542]|56[982]|42)\d{6}$", ErrorMessage ="* الرجاء ادخال رقم هاتف صالح")]
        [Required(ErrorMessage = "* رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string restaurantPhone { get; set; }








        [Required(ErrorMessage = "* المدينة مطلوبة")]
        [Display(Name = "المدينة")]
        public int city { get; set; }






        [MaxLength(100, ErrorMessage = "* العنوان يجب أن لا يتجاوز عن 100 حرف")]
        [Required(ErrorMessage = "* العنوان مطلوب")]
        [Display(Name = "العنوان")]
        public string location { get; set; }







        [Required]
        [Display(Name = "متاح للمناسبات")]
        public bool isAvailableForOccasion { get; set; }







        [Required]
        [Display(Name = "متاح للأطفال")]
        public bool isAvailableForKids { get; set; }






        [Required(ErrorMessage = "* ساعة بدء العمل طلوبة")]
        [Display(Name = "ساعة بدء العمل")]
        public int startTime { get; set; }







        [Required(ErrorMessage = "* ساعة انتهاء العمل مطلوبة")]
        [Display(Name = "ساعة انتهاء العمل")]
        public int endTime { get; set; }






       
        [Display(Name = "صورة المطعم")]
        public string image { get; set; }






        [Required]
        [Display(Name = "معتمد")]
        public bool isApproved { get; set; }






      
        public virtual city city1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<meal> meals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table> Tables { get; set; }
    }
}