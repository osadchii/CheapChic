using CheapChic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CheapChic.Data;

public sealed class CheapChicContext : DbContext
{
    public CheapChicContext(DbContextOptions<CheapChicContext> options) : base(options)
    {
        ChangeTracker.StateChanged += UpdateAuditFields;
        ChangeTracker.Tracked += UpdateAuditFields;
    }

    public DbSet<TelegramBotEntity> TelegramBots { get; set; }
    public DbSet<TelegramUserEntity> TelegramUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramUserEntity>(entity =>
        {
            entity.HasIndex(p => p.ChatId)
                .IsUnique();
        });

        modelBuilder.Entity<TelegramBotEntity>(entity =>
        {
            entity.HasIndex(p => p.Token)
                .IsUnique();

            entity.HasIndex(p => p.OwnerId);
        });

        base.OnModelCreating(modelBuilder);
    }

    private static void UpdateAuditFields(object sender, EntityEntryEventArgs e)
    {
        var entry = e.Entry;

        if (entry.Entity is not BaseEntity entity)
        {
            return;
        }

        var now = DateTime.UtcNow;

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (entry.State)
        {
            case EntityState.Modified:
                entity.ChangedOn = now;
                entry.Properties.First(x => x.Metadata.Name == nameof(BaseEntity.ChangedOn))
                    .IsModified = true;
                break;
            case EntityState.Added:
                entity.ChangedOn = now;
                entity.CreatedOn = now;
                break;
        }
    }
}