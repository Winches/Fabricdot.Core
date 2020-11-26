using System.Threading.Tasks;
using Fabricdot.Infrastructure.Core.Data;
using IntegrationTests.Data.Entities;
using IntegrationTests.Data.Repositories;

namespace IntegrationTests.Data
{
    public class FakeDataBuilder
    {
        private readonly FakeDbContext _dbContext;
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FakeDataBuilder(
            FakeDbContext dbContext,
            IBookRepository bookRepository,
            IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task BuildAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            await AddBooks();
        }

        private async Task AddBooks()
        {
            await _bookRepository.AddAsync(new Book("5175207C-5641-4AF2-9D2A-60B0F55D2DB4", "CSharp"));
            await _bookRepository.AddAsync(new Book("EBA44B30-1665-4AA5-B389-8F153B987EF0", "Java"));
            await _bookRepository.AddAsync(new Book("3C7A1815-7FD0-4353-8110-6E4B10B43BBE", "Typescript"));
            await _bookRepository.AddAsync(new Book("263DCFF9-DB16-4FA4-ADC0-9E45865A0E50", "Rust"));
            await _bookRepository.AddAsync(new Book("920A6F9C-A96B-4E4E-A6C9-BE99CD5547F3", "CPP"));
            await _unitOfWork.CommitChangesAsync();
        }
    }
}