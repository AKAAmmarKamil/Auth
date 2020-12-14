using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Data;
using Auth.Repository;
using AutoMapper;

namespace Auth
{
    public interface IRepositoryWrapper : IDisposable
    {
        IUsersRepository User { get; }
        void Save();
    }

    public class RepositoryWrapper : IRepositoryWrapper
    {
        private Context _repoContext;
        private IUsersRepository _user;
        public IUsersRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }
                return _user;
            }
        }

        public IUsersRepository Users => throw new NotImplementedException();

        public RepositoryWrapper(Context repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }

        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }
    }
}
