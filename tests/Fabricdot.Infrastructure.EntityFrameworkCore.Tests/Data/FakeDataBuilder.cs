using System;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Entities;

namespace Fabricdot.Infrastructure.EntityFrameworkCore.Tests.Data;

public class FakeDataBuilder
{
    private readonly FakeDbContext _dbContext;
    public static int DeletedAuthorId => 2;
    public static string BookWithTagsId => "f00015fe-e5a7-419a-a235-a897a5f7df8c";
    public static string DeletedBookTag => "DeletedTag";
    public static Guid TenantId => new("86b2b1b1-ef3d-46e2-a4c6-5a1df6f694d4");

    public FakeDataBuilder(FakeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BuildAsync()
    {
        await AddBooks();
        await AddAuthors();
        await AddEmployeesAsync();
    }

    private async Task AddBooks()
    {
        var books = new[]
        {
            new Book("1", "CSharp"),
            new Book("2", "Java"),
            new Book("3", "Typescript"),
            new Book("4", "Rust"),
            new Book("5", "CPP"),
            new Book(
            BookWithTagsId,
            "BookWithTags",
            new[] { "Tag1", "Tag2", "Tag3" })
            {
                Contents = new BookContents("Introduce something.")
            }
        };
        books.Single(v => v.Id == BookWithTagsId).AddTag(DeletedBookTag, true);
        foreach (var book in books)
            await _dbContext.AddAsync(book);
        _dbContext.SaveChanges();
    }

    private async Task AddAuthors()
    {
        var authors = new[]
        {
            new Author(1, "Anders", "Hejlsberg"),
            new Author(2, "James", "Gosling"),
            new Author(3, "Bjarne", "Stroustrup"),
            new Author(4, "Graydon", "Hoare")
        };
        authors.Single(v => v.Id == DeletedAuthorId).MarkDeleted();

        foreach (var author in authors)
            await _dbContext.AddAsync(author);
        _dbContext.SaveChanges();
    }

    private async Task AddEmployeesAsync()
    {
        var employees = new[]
        {
            new Employee(Guid.NewGuid(),"Name1",TenantId),
            new Employee(Guid.NewGuid(),"Name2",TenantId),
            new Employee(Guid.NewGuid(),"Name3",TenantId),
            new Employee(Guid.NewGuid(),"Name4"),
        };

        foreach (var employee in employees)
            await _dbContext.AddAsync(employee);
        _dbContext.SaveChanges();
    }
}