using Backend.Dtos;
using Backend.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {

        private StoreContext _storeContext;
        private IValidator<BeerInsertDto> _beerInsertValidator;

        public BeerController(StoreContext context, IValidator<BeerInsertDto> beerInsertValidator)
        {
            _storeContext = context;
            _beerInsertValidator = beerInsertValidator;
        }

        [HttpGet]
        public async Task<IEnumerable<BeerDto>> Get()
        {
            return await _storeContext.Beers.Select(beer => new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                BrandID = beer.BrandID,
                Alcohol = beer.Alcohol
            }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDto>> GetById(int id)
        {
            var beer = await _storeContext.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            var newBeerDto = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                BrandID = beer.BrandID,
                Alcohol = beer.Alcohol
            };

            return Ok(newBeerDto);
        }

        [HttpPost]
        public async Task<ActionResult<BeerDto>> Add(BeerInsertDto beerInsertDto)
        {
            // Validar
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInsertDto);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            // Modelo de la BBD
            var beer = new Beer
            {
                Name = beerInsertDto.Name,
                Alcohol = beerInsertDto.Alcohol,
                BrandID = beerInsertDto.BrandID
            };

            // Manipulación y guardado
            await _storeContext.AddAsync(beer);
            await _storeContext.SaveChangesAsync();

            // Retorno
            var beerDto = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                Alcohol = beer.Alcohol,
            };

            return CreatedAtAction(nameof(GetById), new { id = beer.BeerId }, beerDto); // Crea el elemento, retorna el objeto y dice donde esta disponible
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BeerDto>> Update(int id, BeerUpdateDto beerUpdateDto)
        {
            // Valido si existe, si no retorno un notFound, actualizo y guardo, retorno el beerDto
            var beer = await _storeContext.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            beer.Name = beerUpdateDto.Name;
            beer.BrandID = beerUpdateDto.BrandID;
            beer.Alcohol = beerUpdateDto.Alcohol;

            await _storeContext.SaveChangesAsync();

            var beerUpdated = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                BrandID = beer.BrandID,
                Alcohol = beer.Alcohol,
            };

            return Ok(beerUpdated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var beer = await _storeContext.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound();
            }

            _storeContext.Beers.Remove(beer);
            await _storeContext.SaveChangesAsync();

            return Ok();

        }
    }
}
