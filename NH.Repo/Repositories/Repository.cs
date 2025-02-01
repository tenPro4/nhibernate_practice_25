using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace NH.Repo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ISession _session;

        public Repository(ISession session)
        {
            _session = session;
        }

        public virtual T GetById(int id)
        {
            return _session.Get<T>(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _session.Query<T>();
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _session.Query<T>().Where(predicate);
        }

        public virtual void Add(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _session.Save(entity);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public virtual void Update(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _session.Update(entity);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public virtual void Delete(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _session.Delete(entity);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public virtual void DeleteById(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _session.Query<T>().Any(predicate);
        }
    }
}