using System.ComponentModel.DataAnnotations;

namespace SkillForge.Data.Entities;

public class Category
{
    [Key]
    public int ID {get;set;}

    public string Name {get;set;}
}