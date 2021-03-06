﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using PaymentMethodService.Models;
using System;

namespace PaymentMethodService.Migrations
{
    [DbContext(typeof(ShoppingCartDbContext))]
    partial class ShoppingCartDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PaymentMethodService.Models.PaymentMethod", b =>
                {
                    b.Property<Guid>("PaymentMethodId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PaymentMethodId");

                    b.ToTable("PaymentMethods");
                });
#pragma warning restore 612, 618
        }
    }
}
