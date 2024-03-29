﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Ravenous.Models
{
    [MetadataType(typeof(mealMetaData))]
    public partial class meal
    {
        //Add new properties or methodth
    }
    public class mealMetaData
    {
        // Edit properties

        [Required(ErrorMessage = "* رقـم الـمـطـعـم مـطـلـوب")]
        [Display(Name = "رقـم المـطـعـم")]
        public int restaurantId { get; set; }




        [Required(ErrorMessage = "* رقـم الـوجـبـة مـطـلـوب")]
        [Display(Name = "رقـم الوجـبـة")]
        public int Id { get; set; }








        [Required(ErrorMessage ="* إسـم الـوجـبـة مـطـلـوب")]
        [Display(Name = "اسـم الوجـبـة")]
        public string mealName { get; set; }







        [Required(ErrorMessage = "* سـعـر الـوجـبـة مـطـلـوب")]
        [Display(Name = "سـعـر الـوجـبـة")]
        [DisplayFormat(DataFormatString = "{0:N} شـيـكـل", ApplyFormatInEditMode =false)]
        [Range(1,500,ErrorMessage ="السعر يجب ان يكون 1-500 شيكل")]
        public double mealPrice { get; set; }







        [Required(ErrorMessage = "* نـوع الـوجـبـة مـطـلـوب")]
        [Display(Name = "نـوع الـوجـبـة")]
        public int category { get; set; }







       
        [Display(Name ="صـورة الـوجـبـة")]
        public string mealImage { get; set; }








        [Display(Name = "مـتـاحـة")]
        public bool available { get; set; }





        public virtual mealCategory mealCategory { get; set; }
        public virtual ownerRestaurant ownerRestaurant { get; set; }
    }
}