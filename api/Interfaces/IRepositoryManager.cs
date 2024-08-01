using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;

namespace api.Interfaces
{
    public interface IRepositoryManager
    {
        IStockRepository stockRepo{get;}
        ICommentRepository commentRepo{get;}
        IPortfolioRepository portfolioRepo{get;}
        IUnitOfWork unitOfWork{get;}
    }
}