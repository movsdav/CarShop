﻿using CarShop.Cloud;
using CarShop.Models.Accounts;
using CarShop.Models.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    private readonly FileService _fileService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, FileService fileService)
        : base(options)
    {
        _fileService = fileService;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
            .Entity<AppUser>()
            .Property(au => au.Balance)
            .HasColumnType("money")
            .HasDefaultValue(0);

        builder
            .Entity<CarBrand>()
            .HasIndex(cb => cb.Name)
            .IsUnique();

        builder
            .Entity<CarModel>()
            .Property(cm => cm.Price)
            .HasColumnType("money");

        builder
            .Entity<WishCart>()
            .HasOne(cart => cart.AppUser)
            .WithMany(user => user.WishCartItems)
            .HasForeignKey(f => f.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Entity<WishCart>()
            .HasOne(cart => cart.CarModel)
            .WithMany()
            .HasForeignKey(f => f.CarModelId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .Entity<WishCart>()
            .HasIndex(cart => new { cart.AppUserId, cart.CarModelId })
            .IsUnique();

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Deleted)
            {
                if (entry.Entity is CarModel carModel)
                {
                    if (carModel.ImgFileName != null)
                    {
                        _fileService.Delete(carModel.ImgFileName);
                    }
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }


    public DbSet<CarBrand> CarBrands { get; set; } = default!;
    public DbSet<CarModel> CarModels { get; set; } = default!;
    public DbSet<WishCart> WishCarts { get; set; } = default!;
}
