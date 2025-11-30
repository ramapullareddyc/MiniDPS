using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DocumentProcessor.Web.Models;

namespace DocumentProcessor.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    static AppDbContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Document>(entity =>
        {
            // Apply table mapping with schema
            entity.ToTable("documents", "public");
            
            // Apply column mappings for all properties
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FileName).HasColumnName("file_name");
            entity.Property(e => e.OriginalFileName).HasColumnName("original_file_name");
            entity.Property(e => e.FileExtension).HasColumnName("file_extension");
            entity.Property(e => e.FileSize).HasColumnName("file_size");
            entity.Property(e => e.ContentType).HasColumnName("content_type");
            entity.Property(e => e.StoragePath).HasColumnName("storage_path");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.UploadedBy).HasColumnName("uploaded_by");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            
            // EF Core boolean to int conversion for PostgreSQL compatibility
            entity.Property(e => e.IsDeleted).HasConversion<int>();
            
            // Preserve existing query filter
            entity.HasQueryFilter(d => !d.IsDeleted);
        });
    }
}
