﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLChatThai.Models.QLCTModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QLCTModel : DbContext
    {
        public QLCTModel()
            : base("name=QLCTModel")
        {
            Database.SetInitializer<QLCTModel>(null);
        }
    
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    throw new UnintentionalCodeFirstException();
        //}
    
        public DbSet<Chuyencho> Chuyenchoes { get; set; }
        public DbSet<CTNHap> CTNHaps { get; set; }
        public DbSet<DM_CT> DM_CT { get; set; }
        public DbSet<DMCap1> DMCap1 { get; set; }
        public DbSet<DMCap2> DMCap2 { get; set; }
        public DbSet<DMKHO> DMKHOes { get; set; }
        public DbSet<Khachhang> Khachhangs { get; set; }
        public DbSet<Khuvuc> Khuvucs { get; set; }
        public DbSet<Nhanvien> Nhanviens { get; set; }
        public DbSet<Nhap> Nhaps { get; set; }
        //public DbSet<PPXuly> PPXulies { get; set; }
        public DbSet<Tinhthanh> Tinhthanhs { get; set; }
        public DbSet<DMXe> DMXes { get; set; }
        public DbSet<Xuly> Xulies { get; set; }
    }
}
