﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MlaWebApi.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MlaDatabaseSNSEntities : DbContext
    {
        public MlaDatabaseSNSEntities()
            : base("name=MlaDatabaseSNSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<group_keys> group_keys { get; set; }
        public virtual DbSet<group_status> group_status { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<user_groups> user_groups { get; set; }
        public virtual DbSet<user_public_keys> user_public_keys { get; set; }
        public virtual DbSet<join_requests> join_requests { get; set; }
    }
}
