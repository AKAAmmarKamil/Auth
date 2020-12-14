using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Repository
{
    public interface IUsersRepository : IBaseRepository<User,Guid>
    {
    }
}
