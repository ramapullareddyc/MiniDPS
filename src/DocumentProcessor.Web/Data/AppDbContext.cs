using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using DocumentProcessor.Web.Models;

namespace DocumentProcessor.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Document>(entity =>
        {
            // Apply table mapping with schema
            entity.ToTable("Documents", "public");
            
            // Apply column mappings for all properties
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.FileName).HasColumnName("FileName");
            entity.Property(e => e.OriginalFileName).HasColumnName("OriginalFileName");
            entity.Property(e => e.FileExtension).HasColumnName("FileExtension");
            entity.Property(e => e.FileSize).HasColumnName("FileSize");
            entity.Property(e => e.ContentType).HasColumnName("ContentType");
            entity.Property(e => e.StoragePath).HasColumnName("StoragePath");
            entity.Property(e => e.Status).HasColumnName("Status");
            entity.Property(e => e.Summary).HasColumnName("Summary");
            entity.Property(e => e.UploadedBy).HasColumnName("UploadedBy");
            entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
            
            // Apply boolean to integer conversion for PostgreSQL compatibility
            entity.Property(e => e.IsDeleted).HasConversion<int>();
            
            // Preserve existing query filter
            entity.HasQueryFilter(d => !d.IsDeleted);
        });
    }
}
