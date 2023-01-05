using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NattiDigestBot.Data.Models;

public class Category
{
    public int CategoryId { get; set; }
    public long AccountId { get; set; }
    public string Keyword { get; set; }
    public string Description { get; set; }
}