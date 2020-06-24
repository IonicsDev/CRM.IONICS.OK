using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Common.ContextDomain;
using CRM.Common.RepositoryDomain;

namespace CRM.Common.EntityDomain
{
    public abstract class DelRepository<TEntity, TId> : Repository<TEntity, TId>
        where TEntity : class,IEntityTypedId<TId>, IDelEntity, new()
    {
        public DelRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public override void Add(TEntity item, int userId)
        {
            item.Estado = true;
            item.FechaCreacion = DateTime.Now;
            item.FechaActualizacion = DateTime.Now;
            item.idUsuario = userId;
       
            base.Add(item, userId);
            base.Context.Commit();
        }

        public override void Modify(TEntity item, int userId)
        {
            item.FechaActualizacion = DateTime.Now;
            item.Estado = item.Estado;
            item.idUsuario = userId;
            base.Modify(item,userId);
            base.Context.Commit();
        }

        public override void Modif2(TEntity item, int userId)
        {
            item.FechaActualizacion = Convert.ToDateTime(item.FechaActualizacion);
            item.FechaCreacion = Convert.ToDateTime(item.FechaCreacion);
            item.idUsuario = userId;
            item.Estado = true;
            base.Modif2(item, userId);
            base.Context.Commit();
        }

        public override IQueryable<TEntity> Get(TId id, params Expression<Func<TEntity, object>>[] includes)
        {
            return base.Get(id, includes);
        }


        public override IQueryable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var q = base.GetFiltered(filter, includes);
            return q;
        }

        public new virtual IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return base.GetAll(includes);
        }

        public override void Remove(TEntity item, int userId)
        {
            item.Estado = !item.Estado;
            this.Modify(item,userId);
            base.Context.Commit();
        }

        public void RemoveFromDataBase(TEntity item, int userId)
        {
            base.Remove(item, userId);
            base.Context.Commit();
        }


    }
}
