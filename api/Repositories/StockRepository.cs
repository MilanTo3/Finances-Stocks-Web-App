using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.AppDbContext;
using api.Interfaces;
using api.Models;
using api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDbContext cont) : base(cont)
        {
        }

        public async Task<IEnumerable<Stock>> getAll(QueryObject qor)
        {

            var retVal = await Context.Stocks.Include(x => x.Comments).ThenInclude(x => x.AppUser).ToListAsync();
            if(qor.Symbol != null){
                retVal = (List<Stock>)retVal.Select(x => x.Symbol == qor.Symbol);
            }else if(qor.CompanyName != null){
                retVal = (List<Stock>)retVal.Select(x => x.CompanyName == qor.CompanyName);
            }else if(qor.SortBy != null){
                if(qor.SortBy.ToLower() == "symbol"){
                    retVal = (qor.ascending ? retVal.OrderBy(x => x.Symbol) : retVal.OrderByDescending(x => x.Symbol)).ToList();
                }
            }

            var skipNumber = (qor.pageNumber - 1) * qor.pageSize;

            return retVal.Skip(skipNumber).Take(qor.pageSize);
        }

        public override async Task<Stock> getById(long id)
        {
            return await Context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> getBySymbolAsync(string symbol)
        {
            return await Context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }
}