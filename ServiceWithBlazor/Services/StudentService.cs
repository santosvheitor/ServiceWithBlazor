using ServiceWithBlazor.Models;
using ServiceWithBlazor.Repositories;

namespace ServiceWithBlazor.Services;

public class StudentService(IStudentRepository repo) : IStudentService
    {
        // List with optional filters
        public async Task<IEnumerable<Student>> ListAsync(string? search = null, int? year = null)
        {
            var all = await repo.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLowerInvariant();
                all = all.Where(x => x.FirstName.ToLowerInvariant().Contains(s)
                || x.LastName.ToLowerInvariant().Contains(s)
                || x.Email.ToLowerInvariant().Contains(s));
            }
            if (year is >= 1 and <= 6)
                all = all.Where(x => x.Year == year);
            return all.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
        }


        public Task<Student?> GetAsync(Guid id) => repo.GetByIdAsync(id);


        public async Task<(bool Ok, string? Error, Student? Value)> CreateAsync(Student student)
        {
            var error = Validate(student);
            if (error is not null) return (false, error, null);


            var exists = await repo.GetByEmailAsync(student.Email);
            if (exists is not null)
                return (false, "Email already in use.", null);


            var created = await repo.AddAsync(student);
            return (true, null, created);
        }


        public async Task<(bool Ok, string? Error)> UpdateAsync(Student student)
        {
            if (await repo.GetByIdAsync(student.Id) is null)
                return (false, "Student not found.");


            var error = Validate(student);
            if (error is not null) return (false, error);


            var emailOwner = await repo.GetByEmailAsync(student.Email);
            if (emailOwner is not null && emailOwner.Id != student.Id)
                return (false, "Email already in use by another student.");


            await repo.UpdateAsync(student);
            return (true, null);
        }


        public Task DeleteAsync(Guid id) => repo.DeleteAsync(id);


        private static string? Validate(Student s)
        {
            if (string.IsNullOrWhiteSpace(s.FirstName)) return "First name is required.";
            if (string.IsNullOrWhiteSpace(s.LastName)) return "Last name is required.";
            if (string.IsNullOrWhiteSpace(s.Email) || !s.Email.Contains('@')) return "Valid email is required.";
            if (s.Year is < 1 or > 6) return "Year must be between 1 and 6.";
            return null;
        }
    }