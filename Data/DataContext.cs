using Microsoft.EntityFrameworkCore;
using WhatsappWeb.Api.Models;

namespace WhatsappWeb.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. הגדרת קשר: שולח ההודעה -> מקושר ל-SentMessages של המשתמש
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages) // חיבור מפורש לרשימת ההודעות שנשלחו
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. הגדרת קשר: מקבל ההודעה -> מקושר ל-ReceivedMessages של המשתמש
            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.ReceivedMessages) // חיבור מפורש לרשימת ההודעות שהתקבלו
                .HasForeignKey(m => m.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. הגדרת אינדקס ייחודי למספר הטלפון (מונע כפילויות ברישום)
            builder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}