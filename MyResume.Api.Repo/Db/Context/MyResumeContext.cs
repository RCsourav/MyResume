using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyResume.Api.Repo.Db.Entity;

namespace MyResume.Api.Repo.Db.Context;

public partial class MyResumeContext : DbContext
{
    public MyResumeContext()
    {
    }

    public MyResumeContext(DbContextOptions<MyResumeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LoginChatHistory> LoginChatHistories { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserIpAddress> UserIpAddresses { get; set; }

    public virtual DbSet<UserLoginDetail> UserLoginDetails { get; set; }

    public virtual DbSet<UserLoginHistory> UserLoginHistories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoginChatHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PkLoginChatHistory");

            entity.ToTable("LoginChatHistory");

            entity.HasIndex(e => e.UserLoginHistoryId, "IXLoginChatHistoryUserLoginHistoryId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())", "DfLoginChatHistoryCreatedAt")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestMessage).IsUnicode(false);
            entity.Property(e => e.ResponseMessage).IsUnicode(false);

            entity.HasOne(d => d.UserLoginHistory).WithMany(p => p.LoginChatHistories)
                .HasForeignKey(d => d.UserLoginHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkLoginChatHistoryUserLoginHistory");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PkUserDetails");

            entity.HasIndex(e => e.EmailId, "IXUserDetailsEmailId");

            entity.HasIndex(e => e.UserName, "IXUserDetailsUserName");

            entity.HasIndex(e => new { e.UserName, e.EmailId }, "UXUserDetailsUserNameEmailId").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())", "DfUserDetailsCreatedAt")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())", "DfUserDetailsUpdatedAt")
                .HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserIpAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PkUserIpAddress");

            entity.ToTable("UserIpAddress");

            entity.HasIndex(e => e.IpAddress, "IXUserIpAddressIpAddress");

            entity.HasIndex(e => e.IpAddress, "UXUserIpAddressIpAddress").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())", "DfUserIpAddressCreatedAt")
                .HasColumnType("datetime");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getutcdate())", "DfUserIpAddressUpdatedAt")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserLoginDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UserLoginDetails");

            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.LastActivityTime).HasColumnType("datetime");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserLoginHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PkUserLoginHistory");

            entity.ToTable("UserLoginHistory");

            entity.HasIndex(e => e.IpAddressId, "IXUserLoginHistoryIpAddressId");

            entity.HasIndex(e => e.UserId, "IXUserLoginHistoryUserId");

            entity.Property(e => e.IsActive).HasDefaultValue(true, "DfUserLoginHistoryIsActive");
            entity.Property(e => e.LastActivityTime).HasColumnType("datetime");
            entity.Property(e => e.LoginTime)
                .HasDefaultValueSql("(getutcdate())", "DfUserLoginHistoryLoginTime")
                .HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");

            entity.HasOne(d => d.IpAddress).WithMany(p => p.UserLoginHistories)
                .HasForeignKey(d => d.IpAddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkUserLoginHistoryUserIpAddress");

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkUserLoginHistoryUserDetails");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
