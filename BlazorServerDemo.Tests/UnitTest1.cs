
using FluentAssertions;
using ServiceWithBlazor.Models;
using ServiceWithBlazor.Repositories;

namespace BlazorServerDemo.Tests;

public class InMemoryStudentRepositoryTests
{
    private static InMemoryStudentRepository NewRepo() => new();


    [Fact]
    public async Task GetAllAsync_should_return_seeded_students()
    {
        var repo = NewRepo();


        var all = await repo.GetAllAsync();


        all.Should().HaveCountGreaterThanOrEqualTo(2);
        all.Select(s => s.Email).Should().Contain(new[] { "ada@uni.edu", "alan@uni.edu" });
    }


    [Fact]
    public async Task AddAsync_should_add_and_return_entity_with_id()
    {
        var repo = NewRepo();
        var s = new Student { FirstName = "Grace", LastName = "Hopper", Email = "grace@uni.edu", Year = 4 };


        var created = await repo.AddAsync(s);


        created.Id.Should().NotBe(Guid.Empty);
        (await repo.GetByIdAsync(created.Id)).Should().NotBeNull();
    }


    [Fact]
    public async Task GetByIdAsync_should_return_null_when_not_found()
    {
        var repo = NewRepo();
        var result = await repo.GetByIdAsync(Guid.NewGuid());
        result.Should().BeNull();
    }


    [Theory]
    [InlineData("ADA@UNI.EDU")]
    [InlineData("alan@UNI.edu")]
    public async Task GetByEmailAsync_should_be_case_insensitive(string emailVariant)
    {
        var repo = NewRepo();
        var student = await repo.GetByEmailAsync(emailVariant);
        student.Should().NotBeNull();
    }


    [Fact]
    public async Task UpdateAsync_should_replace_entity_values()
    {
        var repo = NewRepo();
        var added = await repo.AddAsync(new Student
        {
            FirstName = "Linus",
            LastName = "Torvalds",
            Email = "linus@uni.edu",
            Year = 2
        });


        added.LastName = "T.";
        added.Year = 3;
        await repo.UpdateAsync(added);


        var fetched = await repo.GetByIdAsync(added.Id);
        fetched!.LastName.Should().Be("T.");
        fetched.Year.Should().Be(3);
    }


    [Fact]
    public async Task DeleteAsync_should_remove_entity()
    {
        var repo = NewRepo();
        var s = await repo.AddAsync(new Student
            { FirstName = "Katherine", LastName = "Johnson", Email = "kj@uni.edu", Year = 1 });


        await repo.DeleteAsync(s.Id);


        (await repo.GetByIdAsync(s.Id)).Should().BeNull();
    }


    [Fact]
    public async Task FindAsync_should_filter_by_predicate()
    {
        var repo = NewRepo();
        await repo.AddAsync(new Student { FirstName = "Ada", LastName = "L.", Email = "ada2@uni.edu", Year = 3 });


        var thirds = await repo.FindAsync(s => s.Year == 3);


        thirds.Should().OnlyContain(s => s.Year == 3);
    }


    [Fact]
    public async Task AddAsync_should_throw_if_duplicate_id_is_added()
    {
        var repo = NewRepo();
        var s = new Student
            { Id = Guid.NewGuid(), FirstName = "Dup", LastName = "One", Email = "dup1@uni.edu", Year = 1 };
        await repo.AddAsync(s);


        var duplicate = new Student
            { Id = s.Id, FirstName = "Dup", LastName = "Two", Email = "dup2@uni.edu", Year = 2 };
        var act = async () => await repo.AddAsync(duplicate);


        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}