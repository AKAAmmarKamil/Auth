using System;
using System.Collections.Generic;
using Auth.Repository;
namespace Auth.Data
{
    public class UserRepository : BaseRepository<User,Guid>, IUsersRepository
    {
        private readonly Context _context;
        
        public UserRepository(Context repositoryContext):base(repositoryContext)
        {
            _context = repositoryContext;
        }
    }
}
