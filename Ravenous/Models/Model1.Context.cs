﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ravenous.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ravenousDBEntities : DbContext
    {
        public ravenousDBEntities()
            : base("name=ravenousDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<country> countries { get; set; }
        public virtual DbSet<login> logins { get; set; }
        public virtual DbSet<mealCategory> mealCategories { get; set; }
        public virtual DbSet<meal> meals { get; set; }
        public virtual DbSet<ownerRestaurant> ownerRestaurants { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
    }
}
