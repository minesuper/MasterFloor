﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MasterFloor.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MasterFloorDBEntities : DbContext
    {
        private static MasterFloorDBEntities _instance;
        public MasterFloorDBEntities()
            : base("name=MasterFloorDBEntities")
        {
        }

        public static MasterFloorDBEntities GetContext()
        {
            if (_instance == null)
            {
                _instance = new MasterFloorDBEntities();
            }
            return _instance;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<MaterialTypes> MaterialTypes { get; set; }
        public virtual DbSet<PartnerProducts> PartnerProducts { get; set; }
        public virtual DbSet<Partners> Partners { get; set; }
        public virtual DbSet<PartnersTypes> PartnersTypes { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductTypes> ProductTypes { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<Streets> Streets { get; set; }
    }
}
