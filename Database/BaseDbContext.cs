using System;
using System.Collections.Generic;
using System.Linq;
using Karma.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Karma.Database
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region UserSeed    
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User { Id = 1, Username = "First", Email = "first@email.com", FirstName = "First", LastName = "Test", Password = "First" },
                    new User { Id = 2, Username = "Second", Email = "second@email.com", FirstName = "Second", LastName = "Test", Password = "Second" },
                    new User { Id = 3, Username = "Third", Email = "third@email.com", FirstName = "John", LastName = "Smith", Password = "Third" },
                    new User { Id = 4, Username = "Fourth", Email = "fourth@email.com", FirstName = "Anna", LastName = "Smith", Password = "Fourth" }
                );
            #endregion

            #region ListingSeed
            modelBuilder
                .Entity<Listing>()
                .HasData(
                    new Listing { Id = 1, UserId = 1, Status = Status.Reserved, recipientId = 2, Name = "First Listing", Description = "", Quantity = 1, LocationJson = "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\u0160iauliai\",\"RadiusKM\":5}", Category = "Vehicles", DatePublished = System.DateTime.Parse("2021-12-01 16:27:12.2587492"), ImagePath = "images/default.png", Condition = 0, GardenPlant = "Tree", GardenX = -2, GardenZ = 2 },
                    new Listing { Id = 2, UserId = 3, Status = Status.Created, recipientId = null, Name = "Second Listing", Description = "", Quantity = 1, LocationJson = "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\u0160iauliai\",\"RadiusKM\":5}", Category = "Vehicles", DatePublished = System.DateTime.Parse("2021-12-02 13:30:36.9708905"), ImagePath = "images/default.png", Condition = 0, GardenPlant = "Flower", GardenX = 4, GardenZ = -1 },
                    new Listing { Id = 3, UserId = 4, Status = Status.Reserved, recipientId = 1, Name = "Third Listing", Description = "", Quantity = 1, LocationJson = "{\"Country\":\"Lithuania\",\"District\":\"Zemaitija\",\"City\":\"\u0160iauliai\",\"RadiusKM\":5}", Category = "Vehicles", DatePublished = System.DateTime.Parse("2021-12-02 13:30:43.4599796"), ImagePath = "images/default.png", Condition = 0, GardenPlant = "Tree", GardenX = 0, GardenZ = -3 }
                );
            #endregion

            #region ConversationSeed
            modelBuilder
                .Entity<Conversation>()
                .HasData(
                    new Conversation { Id = 1, UserOneId = 1, UserTwoId = 2, ListingId = 1, GroupId = "3e888732f3a04974b3679967f92e1aff" },
                    new Conversation { Id = 2, UserOneId = 4, UserTwoId = 1, ListingId = 3, GroupId = "2b33bd58fe314cf694f848a593396208" }
                );
            #endregion

            #region ListingUserSeed
            modelBuilder
                .Entity<User>()
                .HasMany<Listing>(u => u.RequestedListings)
                .WithMany(l => l.Requestees)
                .UsingEntity(j => j.ToTable("ListingUser")
                    .HasData(
                    new { RequestedListingsId = 1, RequesteesId = 2 },
                    new { RequestedListingsId = 3, RequesteesId = 1 }
                    )
                );
            #endregion

            modelBuilder
                .Entity<User>()
                .HasMany<Listing>(u => u.Listings)
                .WithOne(l => l.User);
            
            modelBuilder
                .Entity<Conversation>()
                .HasOne(c => c.UserOne)
                .WithMany(u => u.StartedConversations)
                .HasForeignKey(c => c.UserOneId);
            modelBuilder
                .Entity<Conversation>()
                .HasOne(c => c.UserTwo)
                .WithMany(u => u.ParticipatingConversations)
                .HasForeignKey(c => c.UserTwoId);
            modelBuilder
                .Entity<Conversation>()
                .HasAlternateKey(c => c.GroupId);
            modelBuilder
                .Entity<Conversation>()
                .HasMany<Message>(c => c.Messages)
                .WithOne(m => m.Conversation);
            
            modelBuilder
                .Entity<Message>()
                .HasOne<Conversation>(m => m.Conversation)
                .WithMany(c => c.Messages);
            modelBuilder
                .Entity<Message>()
                .HasOne<User>(m => m.FromUser)
                .WithMany(u => u.Messages);
        }
    }
}