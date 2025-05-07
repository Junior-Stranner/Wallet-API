using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SenderWallet)
            .WithMany(w => w.SentTransactions)
            .HasForeignKey(t => t.SenderWalletId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ReceiverWallet)
            .WithMany(w => w.ReceivedTransactions)
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        base.OnModelCreating(modelBuilder);
    }
}