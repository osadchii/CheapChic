using CheapChic.Data.Entities;
using CheapChic.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
    public DbSet<TelegramChannelEntity> TelegramChannels { get; set; }
    public DbSet<TelegramMessageEntity> TelegramMessages { get; set; }
    public DbSet<TelegramUserStateEntity> TelegramUserStates { get; set; }
    public DbSet<TelegramBotChannelMappingEntity> TelegramBotChannelMappings { get; set; }
    public DbSet<AdEntity> Ads { get; set; }
    public DbSet<AdPhotoEntity> AdPhotos { get; set; }
    public DbSet<PhotoEntity> Photos { get; set; }

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

        modelBuilder.Entity<TelegramChannelEntity>(entity =>
        {
            entity.HasIndex(p => p.ChatId)
                .IsUnique();
        });

        modelBuilder.Entity<TelegramMessageEntity>(entity =>
        {
            entity.HasIndex(p => new
                {
                    p.UserId,
                    p.ChannelId,
                    p.MessageId
                })
                .IsUnique();

            entity.HasIndex(p => p.ChannelId);
            entity.HasIndex(p => p.UserId);
            entity.HasIndex(p => p.BotId);

            entity.Property(p => p.Type)
                .HasConversion(new EnumToStringConverter<TelegramMessageEntity.TelegramMessageType>());
        });

        modelBuilder.Entity<TelegramUserStateEntity>(entity =>
        {
            entity.HasIndex(p => new
                {
                    p.UserId,
                    p.BotId
                })
                .IsUnique();

            entity.Property(p => p.State)
                .HasConversion(new EnumToStringConverter<State>());
        });

        modelBuilder.Entity<TelegramBotChannelMappingEntity>(entity =>
        {
            entity.HasIndex(p => new
                {
                    p.BotId,
                    p.ChannelId
                })
                .IsUnique();
        });

        modelBuilder.Entity<AdEntity>(entity =>
        {
            entity.HasIndex(p => p.BotId);
            entity.HasIndex(p => p.UserId);
            entity.HasIndex(p => p.DateOfLastPublication);
        });

        modelBuilder.Entity<AdPhotoEntity>(entity =>
        {
            entity.HasIndex(p => p.AdId);
        });

        modelBuilder.Entity<PhotoEntity>(entity =>
        {
            entity.HasIndex(p => p.Hash)
                .IsUnique();
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