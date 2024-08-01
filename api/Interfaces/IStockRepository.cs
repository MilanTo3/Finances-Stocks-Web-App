using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.DTOs;

namespace api.Interfaces
{
    public interface IStockRepository: IGenericRepository<Stock>
    {
        Task<IEnumerable<Stock>> getAll(QueryObject qor);
        Task<Stock?> getBySymbolAsync(string symbol);
    }
}