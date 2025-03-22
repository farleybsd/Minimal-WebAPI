using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RangoAgil.Api.Entities;

public class Ingrediente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nome { get; set; }

    public ICollection<Rango> Rangos { get; set; } = [];

    public Ingrediente()
    {
    }

    [SetsRequiredMembers]
    public Ingrediente(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}