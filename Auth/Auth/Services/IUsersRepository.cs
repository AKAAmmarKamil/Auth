using Auth.Model.Form;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Repository
{
    public interface IUsersRepository : IBaseRepository<User,Guid>
    {
        Task<User> Authintication(LoginForm login);

    }
    public class UsersRepository : BaseRepository<User, Guid>, IUsersRepository
    {
        private readonly Context _db;

        public UsersRepository(Context context) : base(context)
        {
            _db = context;
        }
        public async Task<User> Authintication(LoginForm login) =>
             await _db.User.Where(x => x.UserName == login.Username)
                 .FirstOrDefaultAsync();

        public Task<User> Create(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> Delete(Guid k)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> FindAll(int PageNumber, int count)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindById(Guid k)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<User> Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
