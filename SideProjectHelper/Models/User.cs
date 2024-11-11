using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SideProjectHelper.Models;

public class User : IdentityUser
{
   public List<Project>? Projects { get; set; }
}