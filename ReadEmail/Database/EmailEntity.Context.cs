﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReadEmail.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PRAEntities : DbContext
    {
        public PRAEntities()
            : base("name=PRAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }      
        public DbSet<EmailWatcherConfig> EmailWatcherConfigs { get; set; }
        public DbSet<SupplierEmailConfig> SupplierEmailConfigs { get; set; }
        public DbSet<EmailData> EmailDatas { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<DataLoaderConfig> DataLoaderConfigs { get; set; }
         
    }
}
