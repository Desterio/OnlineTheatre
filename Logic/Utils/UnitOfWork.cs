using System;
using System.Data;
using System.Linq;
using NHibernate;

namespace Logic.Utils
{
    public class UnitOfWork : IDisposable
    {
        private readonly ISession _sesion;
        private readonly ITransaction _transaction;
        private bool _isAlive = true;
        private bool _isCommitted;

        public UnitOfWork(SessionFactory sessionFactory)
        {
            _sesion = sessionFactory.OpenSession();
            _transaction = _sesion.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        
        public void Dispose()
        {
            if (!_isAlive) return;

            _isAlive = false;

            try
            {
                if (_isCommitted)
                    _transaction.Commit();
            }
            finally
            {
               _transaction.Dispose();
               _sesion.Dispose();
            }
        }

        public void Commit()
        {
            if (!_isAlive) return;

            _isCommitted = true;
        }

        internal T Get<T>(long id) where T : class
        {
            return _sesion.Get<T>(id);
        }

        internal void SaveOrUpdate<T>(T entity)
        {
            _sesion.SaveOrUpdate(entity);
        }

        internal void Delete<T>(T entity)
        {
            _sesion.Delete(entity);
        }

        public IQueryable<T> Query<T>()
        {
            return _sesion.Query<T>();
        }

        public ISQLQuery CreateSqlQuery(string query)
        {
            return _sesion.CreateSQLQuery(query);
        }
    }
}