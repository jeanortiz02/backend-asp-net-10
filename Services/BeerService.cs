using System;
using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class BeerService : ICommomService<BeerDto, BeerInsertDto, BeerUpdateDto>
{
    private StoreContext _context;

    public BeerService(StoreContext storeContext)
    {
        _context = storeContext;
    }

    public async Task<IEnumerable<BeerDto>> Get()
    {
        return await _context.Beers.Select(beer => new BeerDto
        {
            Id = beer.BeerId,
            Name = beer.Name,
            BrandID = beer.BrandID,
            Alcohol = beer.Alcohol
        }).ToListAsync();
    }
    public async Task<BeerDto> GetById(int id)
    {
        var beer = await _context.Beers.FindAsync(id);

        if (beer != null)
        {
            var newBeerDto = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                BrandID = beer.BrandID,
                Alcohol = beer.Alcohol
            };

            return newBeerDto;
        }

        return null;
    }
    public async Task<BeerDto> Add(BeerInsertDto beerInsertDto)
    {
        // Modelo de la BBD
        var beer = new Beer
        {
            Name = beerInsertDto.Name,
            Alcohol = beerInsertDto.Alcohol,
            BrandID = beerInsertDto.BrandID
        };

        // Manipulación y guardado
        await _context.AddAsync(beer);
        await _context.SaveChangesAsync();

        // Retorno
        var newBeerDto = new BeerDto
        {
            Id = beer.BeerId,
            Name = beer.Name,
            Alcohol = beer.Alcohol,
        };

        return newBeerDto;

    }

    public async Task<BeerDto> Update(int id, BeerUpdateDto beerUpdateDto)
    {
        // Valido si existe, si no retorno un notFound, actualizo y guardo, retorno el beerDto
        var beer = await _context.Beers.FindAsync(id);

        if (beer != null)
        {
            beer.Name = beerUpdateDto.Name;
            beer.BrandID = beerUpdateDto.BrandID;
            beer.Alcohol = beerUpdateDto.Alcohol;

            await _context.SaveChangesAsync();

            var beerUpdated = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                BrandID = beer.BrandID,
                Alcohol = beer.Alcohol,
            };

            return beerUpdated;
        }

        return null;
    }
    public async Task<BeerDto> Delete(int id)
    {
        var beer = await _context.Beers.FindAsync(id);

        if (beer != null)
        {
            var beerDeleted = new BeerDto
            {
                Id = beer.BeerId,
                Name = beer.Name,
                BrandID = beer.BrandID,
                Alcohol = beer.Alcohol,
            };

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();


            return beerDeleted;
        }

        return null;
    }
}
