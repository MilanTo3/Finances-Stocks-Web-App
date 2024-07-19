using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace api.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext Context { get; set; }
        protected DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext cont) {
            Context = cont;
            dbSet = Context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity) {
            try {
                await dbSet.AddAsync(entity);
            }
            catch {
                return false;
            }
            return true;
        }

        public virtual async Task<bool> Delete(long id) {
            var stockmodel = await dbSet.FindAsync(id);
            if(stockmodel == null){
                return false;
            }

            dbSet.Remove(stockmodel);

            return true;
        }

        public virtual async Task<IEnumerable<T>> getAll() {
            return await dbSet.ToListAsync();
        }

        public virtual async Task<T> getById(long id) {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Update(T entity, int id) {
            var stockmodel = await dbSet.FindAsync(id);
            if(stockmodel == null){
                return false;
            }

            dbSet.Update(entity);

            return true;
        }

    }
}