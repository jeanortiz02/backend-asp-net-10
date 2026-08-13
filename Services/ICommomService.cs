using System;
using Backend.Dtos;

namespace Backend.Services;

// T = General, TI = Insert, TU = Update
public interface ICommomService<T, TI, TU>
{
    public List<string> Errors {get;}
    Task<IEnumerable<T>> Get();
    Task<T> GetById(int id);
    Task<T> Add(TI beerInsertDto);
    Task<T> Update(int id, TU beerUpdateDto);
    Task<T> Delete(int id);
    bool Validate(TI dtoInsert);
    bool Validate(TU dtoUpdate);


}
