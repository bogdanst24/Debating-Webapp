﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dezbateriEntities : DbContext
    {
        public dezbateriEntities()
            : base("name=dezbateriEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryDebate> CategoryDebates { get; set; }
        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<DebateInfo> DebateInfoes { get; set; }
        public virtual DbSet<RoundState> RoundStates { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDebate> UserDebates { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }
    }
}
