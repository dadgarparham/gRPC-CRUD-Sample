using GrpcCrudSample.Api.Models;
using System.Collections.Concurrent;

namespace GrpcCrudSample.Api.Repositories;

public class InMemoryPersonRepository : IPersonRepository
{
    private readonly ConcurrentDictionary<int, Person> _persons = new();
    private int _currentId = 0;

    public InMemoryPersonRepository()
    {
        Seed();
    }

    public Task<List<Person>> GetAllAsync()
    {
        var result = _persons.Values
            .OrderBy(x => x.Id)
            .Select(Clone)
            .ToList();

        return Task.FromResult(result);
    }

    public Task<Person?> GetByIdAsync(int id)
    {
        _persons.TryGetValue(id, out var person);
        return Task.FromResult(person is null ? null : Clone(person));
    }

    public Task<Person> CreateAsync(Person person)
    {
        var id = Interlocked.Increment(ref _currentId);
        person.Id = id;
        var clone = Clone(person);
        _persons[id] = clone;
        return Task.FromResult(Clone(clone));
    }

    public Task<Person?> UpdateAsync(Person person)
    {
        if (!_persons.ContainsKey(person.Id))
            return Task.FromResult<Person?>(null);

        var clone = Clone(person);
        _persons[person.Id] = clone;
        return Task.FromResult<Person?>(Clone(clone));
    }

    public Task<bool> DeleteAsync(int id)
    {
        var result = _persons.TryRemove(id, out _);
        return Task.FromResult(result);
    }

    public Task<bool> ExistsByPersonalCodeAsync(string personalCode, int? excludedId = null)
    {
        var exists = _persons.Values.Any(x =>
            x.PersonalCode.Equals(personalCode, StringComparison.OrdinalIgnoreCase) &&
            (!excludedId.HasValue || x.Id != excludedId.Value));

        return Task.FromResult(exists);
    }

    private void Seed()
    {
        var persons = new[]
        {
            new Person
            {
                Id = 1,
                Name = "Ali",
                LastName = "Ahmadi",
                PersonalCode = "0011223344",
                BirthDay = new DateTime(1995, 5, 21)
            },
            new Person
            {
                Id = 2,
                Name = "Sara",
                LastName = "Mohammadi",
                PersonalCode = "0055667788",
                BirthDay = new DateTime(1998, 11, 2)
            }
        };

        foreach (var person in persons)
        {
            _persons[person.Id] = Clone(person);
        }

        _currentId = persons.Max(x => x.Id);
    }

    private static Person Clone(Person person) => new()
    {
        Id = person.Id,
        Name = person.Name,
        LastName = person.LastName,
        PersonalCode = person.PersonalCode,
        BirthDay = person.BirthDay
    };
}
