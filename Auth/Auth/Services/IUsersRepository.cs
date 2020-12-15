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
}
