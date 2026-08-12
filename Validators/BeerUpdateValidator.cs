using System;
using Backend.Dtos;
using FluentValidation;

namespace Backend.Validators;

public class BeerUpdateValidator : AbstractValidator<BeerUpdateDto>
{
    public BeerUpdateValidator()
    {
        RuleFor(x => x.Id).NotNull().WithMessage("El ID es obligatorio");
        RuleFor(x => x.Name).NotEmpty().WithMessage("El name no debe estar vacío");
        RuleFor(x => x.Name).Length(2, 20).WithMessage("El nombre debe ser entre 2 y 20 caracteres");
        RuleFor(x => x.BrandID).NotNull().WithMessage("La marca es obligatoria");
        RuleFor(x => x.BrandID).GreaterThan(0).WithMessage("Error con el valor enviado de la marca");
        RuleFor(x => x.Alcohol).GreaterThan(0).WithMessage("El {PropertyName} debe ser mayor a cero");
    }
}
