using Auth.Model.Form;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth
{
    public interface IBaseRepository<T,K> 
    {
        Task<List<T>> FindAll(int PageNumber,int count);
        Task<T> FindById(K k);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(K k);

        void SaveChanges();
    }
    public abstract class BaseRepository<T, TId> : IBaseRepository<T, TId> where T : User
    {
        private readonly Context RepositoryContext;
        protected BaseRepository(Context context)
        {
            RepositoryContext = context;
        }
        public async Task<List<T>> FindAll(int PageNumber, int count)
        {
           return await RepositoryContext.Set<T>().Skip(PageNumber * count).Take(count).ToListAsync();
        }

        public async Task<T> Create(T t)
        {
            await RepositoryContext.Set<T>().AddAsync(t);
            await RepositoryContext.SaveChangesAsync();
            return t;
        }

        public async Task<T> Update(T entity)
        {
            RepositoryContext.Set<T>().Update(entity);
            return entity;
        }

        public async Task<T> Delete(TId id)
        {
            var result = await FindById(id);
            if (result == null) return null;
            RepositoryContext.Remove(result);
            await RepositoryContext.SaveChangesAsync();
            return result;
        }

        public void SaveChanges()
        {
            RepositoryContext.SaveChanges();
        }

        public async Task<T> FindById(TId id) =>
            await RepositoryContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));

       
    }

}
