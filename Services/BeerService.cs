using System;
using AutoMapper;
using Backend.Dtos;
using Backend.Models;
using Backend.Repository;

namespace Backend.Services;

public class BeerService : ICommomService<BeerDto, BeerInsertDto, BeerUpdateDto>
{
    private IRepository<Beer> _beerRepository;
    private IMapper _mapper;

    public List<string> Errors {get;}

    public BeerService(IRepository<Beer> beerRepository, IMapper mapper)
    {
        _beerRepository = beerRepository;
        _mapper = mapper;
        Errors = new List<string>();
    }

    public async Task<IEnumerable<BeerDto>> Get()
    {
        var beers = await _beerRepository.Get();

        return beers.Select(beer => _mapper.Map<BeerDto>(beer));
    }
    public async Task<BeerDto> GetById(int id)
    {
        var beer = await _beerRepository.GetById(id);

        if (beer != null)
        {
            var newBeerDto = _mapper.Map<BeerDto>(beer);

            return newBeerDto;
        }

        return null;
    }
    public async Task<BeerDto> Add(BeerInsertDto beerInsertDto)
    {
        // Mapeo
        var beer = _mapper.Map<Beer>(beerInsertDto); // Crea la instancia Beer de la DB

        // Manipulación y guardado
        await _beerRepository.Add(beer);
        await _beerRepository.Save();

        // Retorno
        var newBeerDto = _mapper.Map<BeerDto>(beer);

        return newBeerDto;

    }

    public async Task<BeerDto> Update(int id, BeerUpdateDto beerUpdateDto)
    {
        // Valido si existe, si no retorno un notFound, actualizo y guardo, retorno el beerDto
        var beer = await _beerRepository.GetById(id);

        if (beer != null)
        {
            beer = _mapper.Map<BeerUpdateDto, Beer>(beerUpdateDto, beer);


            _beerRepository.Update(beer);
            await _beerRepository.Save();

            var beerUpdated = _mapper.Map<BeerDto>(beer);

            return beerUpdated;
        }

        return null;
    }
    public async Task<BeerDto> Delete(int id)
    {
        var beer = await _beerRepository.GetById(id);

        if (beer != null)
        {
            var beerDeleted = _mapper.Map<BeerDto>(beer);

            _beerRepository.Delete(beer);
            await _beerRepository.Save();


            return beerDeleted;
        }

        return null;
    }

    public bool Validate(BeerInsertDto dtoInsert)
    {
        if(_beerRepository.Search(b => b.Name == dtoInsert.Name).Count() > 0)
        {
            Errors.Add("No puede existir una cerveza con un nombre ya existente");
            return false;
        }
        return true;
    }


    public bool Validate(BeerUpdateDto dtoUpdate)
    {
        if(_beerRepository.Search(
            b => 
                b.Name == dtoUpdate.Name && dtoUpdate.Id != b.BeerId)
                    .Count() > 0
        )
        {
            Errors.Add("No puede existir una cerveza con un nombre ya existente");
            return false;
        }
        return true;
    }
}
