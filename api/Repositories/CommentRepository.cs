using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.AppDbContext;
using api.Interfaces;
using api.Models;

namespace api.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationDbContext cont) : base(cont)
        {
        }

        public async Task<bool> Add(int id, Comment dto){

            try {
                dto.StockId = id;
                await dbSet.AddAsync(dto);
            }
            catch {
                return false;
            }
            return true;
        }
    }
}