using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppPoc.Models;

[MultiTenant]
[PrimaryKey(nameof(Id))]
public class ToDo
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public bool IsCompleted { get; set; }
    
    public DateTimeOffset? CompletedDateTimeStamp { get; set; }
}