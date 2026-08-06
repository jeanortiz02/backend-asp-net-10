using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class Beer
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BeerId {get; set;}

    public string Name {get; set;}

    [Column(TypeName = "decimal(18,2)")]
    public decimal Alcohol {get; set;}

    public int BrandID {get; set;}

    [ForeignKey("BrandID")]
    public virtual Brand Brand {get; set;} // Esta es la tabla que hace referencia a la relación
}
