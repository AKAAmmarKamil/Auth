using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Model.Form;
using Auth.Repository;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Auth.Data
{
    public class UserRepository : BaseRepository<User,Guid>, IUsersRepository
    {
        private readonly Context _db;

        public UserRepository(Context context) : base(context)
        {
            _db = context;
        }
        public async Task<User> Authintication(LoginForm login) =>
             await _db.User.Where(x => x.UserName == login.Username && x.Password==login.Password)
                 .FirstOrDefaultAsync();

        public async Task<List<User>> FindAll(int PageNumber, int count)
        {
            return await _db.Set<User>().Skip(PageNumber * count).Take(count).ToListAsync();
        }

        public async Task<User> Create(User t)
        {
            await _db.Set<User>().AddAsync(t);
            await _db.SaveChangesAsync();
            return t;
        }

        public async Task<User> Update(User entity)
        {
            _db.Set<User>().Update(entity);
            return entity;
        }

        public async Task<User> Delete(Guid id)
        {
            var result = await FindById(id);
            if (result == null) return null;
            _db.Remove(result);
            await _db.SaveChangesAsync();
            return result;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public async Task<User> FindById(Guid id) =>
            await _db.Set<User>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }
}
