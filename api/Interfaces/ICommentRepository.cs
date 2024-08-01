using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository: IGenericRepository<Comment>
    {
        Task<bool> Add(int id, Comment dto);
        Task<Comment> getById(int id);
    }
}