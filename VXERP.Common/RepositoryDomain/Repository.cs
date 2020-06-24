using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using CRM.Common.ContextDomain;
using CRM.Common.EntityDomain;
using CRM.Framework.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace CRM.Common.RepositoryDomain
{
    /// <summary>
    /// Repository class
    /// </summary>
    /// <typeparam name="TEntity">The type of underlying entity in this repository</typeparam>
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IEntityTypedId<TId>, new()
    {
        #region Members

        IQueryableUnitOfWork _UnitOfWork;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork");

            _UnitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        public IUnitOfWork Context
        {
            get
            {
                return _UnitOfWork;
            }
        }

        public virtual void Add(TEntity item, int userId)
        {
            if (item != (TEntity)null)
            {
                //reference tablolar context'deki degerler ile degistiriliyor. Bu islem yapilmazsa yeni kayitta referans tablolar icin de kayit ekleniyor. 
                //Person eklemek istediginde Country tablosundan var olan kayidin id bilgisi yerine yeni kayit ekleyip onun id'sini aliyor.
                var type = typeof(IEntityTypedId<>);
                var referenceItems = item.GetType().GetProperties().Where(pr => pr.PropertyType.GetInterface(type.FullName) != null);
                foreach (System.Reflection.PropertyInfo referenceItem in referenceItems)
                {
                    var referenceValue = (IEntityTypedId<TId>)referenceItem.GetValue(item, null);
                    if (referenceValue != null)
                    {
                        MethodInfo method = _UnitOfWork.GetType().GetMethod("GetContextEntity");
                        MethodInfo generic = method.MakeGenericMethod(referenceItem.PropertyType, referenceItem.PropertyType.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public).PropertyType);
                        var original = generic.Invoke(_UnitOfWork, new[] { referenceValue });
                        item.GetType().GetProperty(referenceItem.Name, BindingFlags.Instance | BindingFlags.Public).SetValue(item, original, null);
                    }
                }

                GetSet().Add(item); // add new item in this set
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("Messages.info_CannotAddNullEntity", typeof(TEntity).ToString());
            }
        }

        public virtual void Remove(TEntity item, int userId)
        {
            if (item != (TEntity)null)
            {
                item = _UnitOfWork.Attach<TEntity, TId>(item);
                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }

        public virtual void TrackItem(TEntity item, int userId)
        {
            if (item != (TEntity)null)
                _UnitOfWork.Attach<TEntity>(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }

        public virtual void Modify(TEntity item, int userId)
        {
           if (item != (TEntity)null)
            {

                _UnitOfWork.SetModified<TEntity, TId>(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }

        public virtual void Modif2(TEntity item, int userId)
        {
            if (item != (TEntity)null)
            {

                _UnitOfWork.SetModified<TEntity, TId>(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("Messages.info_CannotRemoveNullEntity", typeof(TEntity).ToString());
            }
        }

        public virtual IQueryable<TEntity> Get(TId id, params Expression<Func<TEntity, object>>[] includes)
        {
            if (id != null)
            {
                var itemParameter = Expression.Parameter(typeof(TEntity));
                var whereExpression = Expression.Lambda<Func<TEntity, bool>>
                    (
                    Expression.Equal(
                        Expression.Property(
                            itemParameter,
                            "ID"
                            ),
                        Expression.Constant(id)
                        ),
                    new[] { itemParameter }
                    );
                return GetSetAsQueryable(includes).Where(whereExpression);
            }
            else
                return null;
        }

        public virtual IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSetAsQueryable(includes);
        }

    

        public virtual IQueryable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, System.Linq.Expressions.Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, params Expression<Func<TEntity, object>>[] includes)
        {
            var set = GetSetAsQueryable();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }

        public virtual IQueryable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSetAsQueryable(includes).Where(filter);
        }

        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _UnitOfWork.ApplyCurrentValues(persisted, current);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_UnitOfWork != null)
                _UnitOfWork.Dispose();
        }

        #endregion

        #region Private Methods

        IDbSet<TEntity> GetSet()
        {
            var ent = _UnitOfWork.CreateSet<TEntity>();
            return ent;
        }

        IQueryable<TEntity> GetSetAsQueryable(params Expression<Func<TEntity, object>>[] includes)
        {
            var result = GetSet().AsQueryable();
            if (includes == null) return result;

            foreach (var includeItem in includes)
            {
                result = result.Include(includeItem);
            }
            return result;
        }
        #endregion

    }

    public static class ObjectQueryExtensions
    {
        //public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> selector) where T : class
        //{
        //    string propertyPath = GetPropertyPath(selector);
        //    var result = query.Include(propertyPath);
        //    if (selector.Body.Type.GetInterface(typeof(Trul.Domain.Core.IDelEntity).Name) != null)
        //    {
        //        ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
        //        var navigationProperty = typeof(T).GetProperty(propertyPath);
        //        var isDeleted = typeof(IDelEntity).GetProperty("IsDeleted");
        //        var navigationPropertyAccess = Expression.MakeMemberAccess(parameterExpression, navigationProperty);
        //        var expIsDeleted = Expression.MakeMemberAccess(navigationPropertyAccess, isDeleted);
        //        ConstantExpression constantExpression = Expression.Constant(false, typeof(bool));
        //        BinaryExpression binaryExpression = Expression.Equal(expIsDeleted, constantExpression);
        //        Expression<Func<T, bool>> expression = Expression.Lambda<Func<T, bool>>(binaryExpression, new ParameterExpression[] { parameterExpression });
        //        result = result.Where(expression);
        //    }
        //    return result;
        //}

        public static string GetPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> selector)
        {
            StringBuilder sb = new StringBuilder();
            MemberExpression memberExpr = selector.Body as MemberExpression;
            while (memberExpr != null)
            {
                string name = memberExpr.Member.Name;
                if (sb.Length > 0)
                    name = name + ".";
                sb.Insert(0, name);
                if (memberExpr.Expression is ParameterExpression)
                    return sb.ToString();
                memberExpr = memberExpr.Expression as MemberExpression;
            }
            throw new ArgumentException("The expression must be a MemberExpression", "selector");
        }



        

        public static Task ForEachAsync<TSource, TResult>(this IEnumerable<TSource> source,
                  Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor)
        {
            var oneAtATime = new SemaphoreSlim(initialCount: 1, maxCount: 1);
            return Task.WhenAll(
                from item in source
                select ProcessAsync(item, taskSelector, resultProcessor, oneAtATime));
        }

        private static async Task ProcessAsync<TSource, TResult>(TSource item,
            Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor,
            SemaphoreSlim oneAtATime)
        {
            TResult result = await taskSelector(item);
            await oneAtATime.WaitAsync();
            try { resultProcessor(item, result); }
            finally { oneAtATime.Release(); }
        }
    }
}
