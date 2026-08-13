using System;
using Backend.Dtos;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class BeerRepository : IRepository<Beer>
{
    private StoreContext _context;

    public BeerRepository(StoreContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Beer>> Get() => await _context.Beers.ToListAsync();


    public async Task<Beer> GetById(int id) =>  await _context.Beers.FindAsync(id);


    public async Task Add(Beer beer) => await _context.AddAsync(beer);



    public async void Update(Beer beer)
    {
        _context.Beers.Attach(beer); // Rastrea el cambio de beer y lo llena
        _context.Beers.Entry(beer).State = EntityState.Modified; // Notifica el cambio
    }
    public void Delete(Beer beer) => _context.Beers.Remove(beer);
    
    public async Task Save() => await _context.SaveChangesAsync();

    public IEnumerable<Beer> Search(Func<Beer, bool> filter) => _context.Beers.Where(filter).ToList();
}
