using Microsoft.EntityFrameworkCore;
using YourNamespace.Models; // ajuste o namespace conforme sua estrutura

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações adicionais de relacionamento (opcional)
        modelBuilder.Entity<User>()
            .HasOne<Wallet>()
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SenderWallet)
            .WithMany()
            .HasForeignKey(t => t.SenderWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ReceiverWallet)
            .WithMany()
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
