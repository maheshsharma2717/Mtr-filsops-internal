using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Models;

public partial class MtrContext : DbContext
{
    public MtrContext()
    {
    }
    public MtrContext(DbContextOptions<MtrContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<Fieldo_LoginDetails> Fieldo_LoginDetails { get; set; }

    public virtual DbSet<Fieldo_UserDetails> Fieldo_UserDetails { get; set; }
    public virtual DbSet<Fieldo_Role> Fieldo_Roles { get; set; }
    public virtual DbSet<Fieldo_Task> Fieldo_Task { get; set; }
    public virtual DbSet<Fieldo_RequestCategory> Fieldo_RequestCategory { get; set; }
    public virtual DbSet<Fieldo_WorkerTasks> Fieldo_WorkerTasks { get; set; }
    public virtual DbSet<Fieldo_WorkerBankDetails> Fieldo_WorkerBankDetails { get; set; }
    public virtual DbSet<Fieldo_Banks> Fieldo_Banks { get; set; }
    public virtual DbSet<Fieldo_Address> Fieldo_Address { get; set; }
    public virtual DbSet<Fieldo_Review> Fieldo_Review { get; set; }
    public virtual DbSet<Fieldo_TaskStatus> Fieldo_TaskStatus { get; set; }
    public virtual DbSet<Fieldo_UserReview> Fieldo_UserReview { get; set; }
    public virtual DbSet<Fieldo_Payments> Fieldo_Payments { get; set; }
    public virtual DbSet<Fieldo_TaskAttachment> Fieldo_TaskAttachments { get; set; }
    public virtual DbSet<Fieldo_Notification> Fieldo_Notifications { get; set; }
    public virtual DbSet<Fieldo_Message> Fieldo_Messages { get; set; }
    public virtual DbSet<Fieldo_EmailTemplate> Fieldo_EmailTemplate { get; set; }
    public virtual DbSet<Fieldo_GenericSetting> Fieldo_GenericSetting { get; set; }
    public virtual DbSet<Fieldo_Wallet> Fieldo_Wallet { get; set; }
    
    public virtual DbSet<Fieldo_Log> Fieldo_Log { get; set; }
    public virtual DbSet<Fieldo_Customer> Fieldo_Customer { get; set; }

     public virtual DbSet<Fieldo_WalletTransaction> Fieldo_WalletTransaction { get; set; }


    public virtual DbSet<Fieldo_DeviceToken> Fieldo_DeviceToken { get; set; }
    public virtual DbSet<FieldOps_AdminDeviceToken> FieldOps_AdminDeviceToken { get; set; }
    public virtual DbSet<taxi_employee> Taxi_Employees { get; set; }
    public virtual DbSet<taxi_user> Taxi_User { get; set; }
    public virtual DbSet<taxi_domains> Taxi_Domains { get; set; }
    

    protected override void OnConfiguring
                  (DbContextOptionsBuilder OptionsBuilder)
    {
        base.OnConfiguring(OptionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<taxi_employee>()
           .ToTable("taxi_employees");
        modelBuilder.Entity<taxi_user>()
         .ToTable("taxi_user");

        modelBuilder.Entity<taxi_domains>()
           .ToTable("taxi_domains");
    }

}
