using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Web.Models;

public enum DocumentStatus { Pending, Processing, Processed, Failed }

[Table("documents", Schema = "public")]
public class Document
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("file_name")]
    public string FileName { get; set; } = string.Empty;
    
    [Column("original_file_name")]
    public string OriginalFileName { get; set; } = string.Empty;
    
    [Column("file_extension")]
    public string FileExtension { get; set; } = string.Empty;
    
    [Column("file_size")]
    public long FileSize { get; set; }
    
    [Column("content_type")]
    public string ContentType { get; set; } = string.Empty;
    
    [Column("storage_path")]
    public string StoragePath { get; set; } = string.Empty;
    
    [Column("status")]
    public DocumentStatus Status { get; set; }
    
    [Column("summary")]
    public string? Summary { get; set; }
    
    [Column("uploaded_by")]
    public string UploadedBy { get; set; } = string.Empty;
    
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
}
