using System;
using Backend.Dtos;

namespace Backend.Services;

// T = General, TI = Insert, TU = Update
public interface ICommomService<T, TI, TU>
{
    Task<IEnumerable<T>> Get();
    Task<T> GetById(int id);
    Task<T> Add(TI beerInsertDto);
    Task<T> Update(int id, TU beerUpdateDto);
    Task<T> Delete(int id);

}
