using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.AppDbContext;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDbContext cont) : base(cont)
        {
        }

        public override async Task<IEnumerable<Stock>> getAll()
        {
            
            return await base.Context.Stocks.Include(x => x.Comments).ToListAsync();
        }

        public override Task<Stock> getById(long id)
        {

            return base.Context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}