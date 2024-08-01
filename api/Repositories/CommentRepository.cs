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

        public override async Task<IEnumerable<Comment>> getAll(){
            return await Context.Comments.Include(x => x.AppUser).ToListAsync();
        }

        public async Task<Comment> getById(int id){
            return await Context.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}