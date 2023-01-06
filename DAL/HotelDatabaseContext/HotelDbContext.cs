using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HotelDatabaseContext
{
    public class HotelDbContext : DbContext, IHotelDbContext, IDisposable
    {
        public HotelDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomHistory> RoomHistories { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<RoomImages> RoomImages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(HotelDbContext).Assembly);
        }

        public class CustomerTypeConfiguration : IEntityTypeConfiguration<Customer>
        {
            public void Configure(EntityTypeBuilder<Customer> builder)
            {
                builder
                    .HasKey(x => x.Id);

                builder
                    .HasMany(x => x.Books)
                    .WithOne(x => x.Customer)
                    .HasForeignKey(x => x.CustomerId);
            }
        }

        public class RoomTypeConfiguration : IEntityTypeConfiguration<Room>
        {
            public void Configure(EntityTypeBuilder<Room> builder)
            {
                builder
                    .HasKey(x => x.Id);

                builder
                    .HasMany(x => x.Histories)
                    .WithOne(x => x.Room)
                    .HasForeignKey(x => x.RoomId);

                builder
                    .HasMany(x=>x.RoomImages)
                    .WithOne()
                    .HasForeignKey(x=>x.RoomId);

            }
        }

        public class RoomHistoryTypeConfiguration : IEntityTypeConfiguration<RoomHistory>
        {
            public void Configure(EntityTypeBuilder<RoomHistory> builder)
            {
                builder
                    .HasKey(x => x.Id);


            }
        }

        public class BookTypeConfiguration : IEntityTypeConfiguration<Book>
        {
            public void Configure(EntityTypeBuilder<Book> builder)
            {
                builder
                    .HasKey(x => x.Id);

                builder
                    .HasOne(x => x.Room)
                    .WithMany()
                    .HasForeignKey(x => x.RoomId);
            }
        }
        public class RoomImagesTypeConfiguration : IEntityTypeConfiguration<RoomImages>
        {
            public void Configure(EntityTypeBuilder<RoomImages> builder)
            {
                builder
                    .HasKey(x => x.Id);
            }
        }
    }
}
