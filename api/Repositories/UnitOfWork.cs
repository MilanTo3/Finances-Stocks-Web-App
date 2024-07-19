using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.AppDbContext;
using api.Interfaces;

namespace api.Repositories
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

        public Task<int> Complete() =>
            _dbContext.SaveChangesAsync();

        public void Dispose() {
            _dbContext.Dispose();
    }
    }
}