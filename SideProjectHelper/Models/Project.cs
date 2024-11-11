using System.ComponentModel.DataAnnotations;

namespace SideProjectHelper.Models;

public class Project
{
    public int ProjectId { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }

    [Required] public string Title { get; set; }
    public string Description { get; set; }
    public string? Photo { get; set; }
}