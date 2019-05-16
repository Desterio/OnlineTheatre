using Logic.Utils;

namespace Logic.Common
{
    public abstract class Repository<T> where T : Entity
    {
        protected readonly UnitOfWork UnitOfWork;

        protected Repository(UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public T GetById(long id)
        {
            return UnitOfWork.Get<T>(id);
        }

        public void Add(T entity)
        {
            UnitOfWork.SaveOrUpdate(entity);
        }
    }
}