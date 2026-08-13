using Backend.Dtos;
using Backend.Models;
using Backend.Services;
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
        private IValidator<BeerInsertDto> _beerInsertValidator;
        private IValidator<BeerUpdateDto> _beerUpdateValidator;
        private ICommomService<BeerDto, BeerInsertDto, BeerUpdateDto> _beerService;

        public BeerController(
            IValidator<BeerInsertDto> beerInsertValidator,
            IValidator<BeerUpdateDto> beerUpdateValidator,
            [FromKeyedServices("beerService")] ICommomService<BeerDto, BeerInsertDto, BeerUpdateDto> beerService
            )
        {
            _beerInsertValidator = beerInsertValidator;
            _beerUpdateValidator = beerUpdateValidator;
            _beerService = beerService;
        }

        [HttpGet]
        public async Task<IEnumerable<BeerDto>> Get()
        {
            return await _beerService.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BeerDto>> GetById(int id)
        {
            var beer = await _beerService.GetById(id);

            return beer == null ? NotFound() : Ok(beer);
        }

        [HttpPost]
        public async Task<ActionResult<BeerDto>> Add(BeerInsertDto beerInsertDto)
        {
            // Validar
            var validationResult = await _beerInsertValidator.ValidateAsync(beerInsertDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Validation For Services
            if (!_beerService.Validate(beerInsertDto))
            {
                return BadRequest(_beerService.Errors);
            }
            var beer = await _beerService.Add(beerInsertDto);

            return CreatedAtAction(nameof(GetById), new { id = beer.Id }, beer); // Crea el elemento, retorna el objeto y dice donde esta disponible
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BeerDto>> Update(int id, BeerUpdateDto beerUpdateDto)
        {
            // Validar
            var validResult = await _beerUpdateValidator.ValidateAsync(beerUpdateDto);
            if (!validResult.IsValid)
            {
                return BadRequest(validResult.Errors);
            }
            // Validation For Services
            if (!_beerService.Validate(beerUpdateDto))
            {
                return BadRequest(_beerService.Errors);
            }

            var beerUpdated = await _beerService.Update(id, beerUpdateDto);

            return beerUpdated == null ? NotFound() : Ok(beerUpdated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BeerDto>> Delete(int id)
        {
            var beer = await _beerService.Delete(id);

            return beer == null ? NotFound() : Ok(beer);

        }
    }
}
