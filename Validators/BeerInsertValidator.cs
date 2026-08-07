using System;
using Backend.Dtos;
using FluentValidation;

namespace Backend.Validators;

public class BeerInsertValidator : AbstractValidator<BeerInsertDto>
{
    public BeerInsertValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("El name no debe estar vacío");
    }
}
