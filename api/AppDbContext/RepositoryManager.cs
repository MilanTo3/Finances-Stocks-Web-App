using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Repositories;

namespace api.AppDbContext
{
    public class RepositoryManager : IRepositoryManager
    {
        
        private readonly Lazy<IStockRepository> stockRepository;
        private readonly Lazy<ICommentRepository> commentRepository;
        private readonly Lazy<IUnitOfWork> unitofWork;

        public RepositoryManager(ApplicationDbContext dbContext){
            stockRepository = new Lazy<IStockRepository>(() => new StockRepository(dbContext));
            commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(dbContext));
            unitofWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));

        }

        public IStockRepository stockRepo => stockRepository.Value;

        public ICommentRepository commentRepo => commentRepository.Value;

        public IUnitOfWork unitOfWork => unitofWork.Value;
    }
}