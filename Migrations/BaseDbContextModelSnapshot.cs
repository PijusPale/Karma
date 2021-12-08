﻿// <auto-generated />
using System;
using Karma.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karma.Migrations
{
    [DbContext(typeof(BaseDbContext))]
    partial class BaseDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.12");

            modelBuilder.Entity("Karma.Models.Conversation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ListingId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserOneId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserTwoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Conversations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GroupId = "3e888732f3a04974b3679967f92e1aff",
                            ListingId = 1,
                            UserOneId = 1,
                            UserTwoId = 2
                        },
                        new
                        {
                            Id = 2,
                            GroupId = "2b33bd58fe314cf694f848a593396208",
                            ListingId = 3,
                            UserOneId = 4,
                            UserTwoId = 1
                        });
                });

            modelBuilder.Entity("Karma.Models.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Condition")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePublished")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("LocationJson")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isReserved")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("recipientId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Listings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Vehicles",
                            Condition = 0,
                            DatePublished = new DateTime(2021, 12, 1, 16, 27, 12, 258, DateTimeKind.Unspecified).AddTicks(7492),
                            Description = "",
                            ImagePath = "images/default.png",
                            LocationJson = "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\\u0160iauliai\",\"RadiusKM\":5}",
                            Name = "First Listing",
                            Quantity = 1,
                            UserId = 1,
                            isReserved = true,
                            recipientId = 2
                        },
                        new
                        {
                            Id = 2,
                            Category = "Vehicles",
                            Condition = 0,
                            DatePublished = new DateTime(2021, 12, 2, 13, 30, 36, 970, DateTimeKind.Unspecified).AddTicks(8905),
                            Description = "",
                            ImagePath = "images/default.png",
                            LocationJson = "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\\u0160iauliai\",\"RadiusKM\":5}",
                            Name = "Second Listing",
                            Quantity = 1,
                            UserId = 3,
                            isReserved = false
                        },
                        new
                        {
                            Id = 3,
                            Category = "Vehicles",
                            Condition = 0,
                            DatePublished = new DateTime(2021, 12, 2, 13, 30, 43, 459, DateTimeKind.Unspecified).AddTicks(9796),
                            Description = "",
                            ImagePath = "images/default.png",
                            LocationJson = "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\\u0160iauliai\",\"RadiusKM\":5}",
                            Name = "Third Listing",
                            Quantity = 1,
                            UserId = 4,
                            isReserved = true,
                            recipientId = 1
                        });
                });

            modelBuilder.Entity("Karma.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("TEXT");

                    b.Property<int>("FromId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Karma.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarPath")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastActive")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "First",
                            LastName = "Test",
                            Username = "First"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Second",
                            LastName = "Test",
                            Username = "Second"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "John",
                            LastName = "Smith",
                            Username = "Third"
                        },
                        new
                        {
                            Id = 4,
                            FirstName = "Anna",
                            LastName = "Smith",
                            Username = "Fourth"
                        });
                });

            modelBuilder.Entity("ListingUser", b =>
                {
                    b.Property<int>("RequestedListingsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequesteesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RequestedListingsId", "RequesteesId");

                    b.HasIndex("RequesteesId");

                    b.ToTable("ListingUser");

                    b.HasData(
                        new
                        {
                            RequestedListingsId = 1,
                            RequesteesId = 2
                        },
                        new
                        {
                            RequestedListingsId = 3,
                            RequesteesId = 1
                        });
                });

            modelBuilder.Entity("Karma.Models.Listing", b =>
                {
                    b.HasOne("Karma.Models.User", "User")
                        .WithMany("Listings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ListingUser", b =>
                {
                    b.HasOne("Karma.Models.Listing", null)
                        .WithMany()
                        .HasForeignKey("RequestedListingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Karma.Models.User", null)
                        .WithMany()
                        .HasForeignKey("RequesteesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Karma.Models.User", b =>
                {
                    b.Navigation("Listings");
                });
#pragma warning restore 612, 618
        }
    }
}
