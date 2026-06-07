using GrpcCrudSample.Api.Models;

namespace GrpcCrudSample.Api.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(int id);
    Task<Person> CreateAsync(Person person);
    Task<Person?> UpdateAsync(Person person);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByPersonalCodeAsync(string personalCode, int? excludedId = null);
}
