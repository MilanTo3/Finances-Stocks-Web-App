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
    }
}