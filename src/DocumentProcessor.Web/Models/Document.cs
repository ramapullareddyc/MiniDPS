using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentProcessor.Web.Models;

public enum DocumentStatus { Pending, Processing, Processed, Failed }

[Table("Documents", Schema = "public")]
public class Document
{
    [Key]
    [Column("Id")]
    public Guid Id { get; set; }
    
    [Column("FileName")]
    public string FileName { get; set; } = string.Empty;
    
    [Column("OriginalFileName")]
    public string OriginalFileName { get; set; } = string.Empty;
    
    [Column("FileExtension")]
    public string FileExtension { get; set; } = string.Empty;
    
    [Column("FileSize")]
    public long FileSize { get; set; }
    
    [Column("ContentType")]
    public string ContentType { get; set; } = string.Empty;
    
    [Column("StoragePath")]
    public string StoragePath { get; set; } = string.Empty;
    
    [Column("Status")]
    public DocumentStatus Status { get; set; }
    
    [Column("Summary")]
    public string? Summary { get; set; }
    
    [Column("UploadedBy")]
    public string UploadedBy { get; set; } = string.Empty;
    
    [Column("IsDeleted")]
    public bool IsDeleted { get; set; }
}
