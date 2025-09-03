namespace ServiceWithBlazor.Models;

public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Year { get; set; } // 1–4


    public string FullName => $"{FirstName} {LastName}".Trim();
}