using ServiceWithBlazor.Models;

namespace ServiceWithBlazor.Services;

public interface IStudentService
{
    Task<IEnumerable<Student>> ListAsync(string? search = null, int? year = null);
    Task<Student?> GetAsync(Guid id);
    Task<(bool Ok, string? Error, Student? Value)> CreateAsync(Student student);
    Task<(bool Ok, string? Error)> UpdateAsync(Student student);
    Task DeleteAsync(Guid id);
}