using System.ComponentModel.DataAnnotations;

namespace RangoAgil.Api.Models;

public class RangoParaCriacaoDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; }
}