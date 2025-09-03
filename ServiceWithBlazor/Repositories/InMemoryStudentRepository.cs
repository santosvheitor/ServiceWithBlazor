using ServiceWithBlazor.Models;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using ServiceWithBlazor.Repositories;


namespace ServiceWithBlazor.Repositories;

   public class InMemoryStudentRepository : IStudentRepository
    {
        // Thread‑safe in‑memory store
        private readonly ConcurrentDictionary<Guid, Student> _store = new();


        public InMemoryStudentRepository()
        {
            // Seed a little data
            var s1 = new Student { FirstName = "Ada", LastName = "Lovelace", Email = "ada@uni.edu", Year = 3 };
            var s2 = new Student { FirstName = "Alan", LastName = "Turing", Email = "alan@uni.edu", Year = 4 };
            _store.TryAdd(s1.Id, s1);
            _store.TryAdd(s2.Id, s2);
        }


        public Task<Student> AddAsync(Student entity)
        {
            entity.Id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            if (!_store.TryAdd(entity.Id, entity))
                throw new InvalidOperationException("Could not add student.");
            return Task.FromResult(entity);
        }


        public Task DeleteAsync(Guid id)
        {
            _store.TryRemove(id, out _);
            return Task.CompletedTask;
        }


        public Task<IEnumerable<Student>> FindAsync(Expression<Func<Student, bool>> predicate)
        {
            var query = _store.Values.AsQueryable().Where(predicate);
            return Task.FromResult(query.AsEnumerable());
        }


        public Task<IEnumerable<Student>> GetAllAsync()
        => Task.FromResult(_store.Values.OrderBy(s => s.LastName).AsEnumerable());


        public Task<Student?> GetByEmailAsync(string email)
        {
            var student = _store.Values.FirstOrDefault(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(student);
        }


        public Task<Student?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var student);
            return Task.FromResult(student);
        }


        public Task UpdateAsync(Student entity)
        {
            _store.AddOrUpdate(entity.Id, entity, (id, existing) => entity);
            return Task.CompletedTask;
        }
    }
