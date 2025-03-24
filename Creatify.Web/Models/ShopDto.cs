using System.ComponentModel.DataAnnotations;

namespace Creatify.Web.Models;
public class ShopDto
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string OwnerEmail { get; set; }
    [Required]
    public string ContactEmail { get; set; }
    [Required]
    public string PhoneNumber { get; set; }

    public string? Location { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    public string? ImageUrl { get; set; }
    public string? ImageLocalPath { get; set; }
}

