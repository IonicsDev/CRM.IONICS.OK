using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CRM.Common.EntityDomain;

namespace CRM.Common.RepositoryDomain
{
    public interface IRepositoryService<TDTOEntity, TId>
       where TDTOEntity : class, IEntityTypedId<TId>
    {
        /// <summary>
        /// Add item into repository
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(TDTOEntity item);

        /// <summary>
        /// Delete item 
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Delete(TDTOEntity item);

        /// <summary>
        /// Set item as modified
        /// </summary>
        /// <param name="item">Item to modify</param>
        void Update(TDTOEntity item);

        /// <summary>
        /// Get element by entity key
        /// </summary>
        /// <param name="id">Entity key value</param>
        /// <returns></returns>
        TDTOEntity Get(TId id, params Expression<Func<TDTOEntity, object>>[] includes);

        /// <summary>
        /// Get all elements of type TEntity in repository
        /// </summary>
        /// <returns>List of selected elements</returns>
        IList<TDTOEntity> GetAll(params Expression<Func<TDTOEntity, object>>[] includes);

        /// <summary>
        /// Get all elements of type TEntity in repository
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageCount">Number of elements in each page</param>
        /// <param name="orderByExpression">Order by expression for this query</param>
        /// <param name="ascending">Specify if order is ascending</param>
        /// <returns>List of selected elements</returns>
        IList<TDTOEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TDTOEntity, KProperty>> orderByExpression, bool ascending, params Expression<Func<TDTOEntity, object>>[] includes);

        /// <summary>
        /// Get  elements of type TEntity in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IList<TDTOEntity> GetFiltered(Expression<Func<TDTOEntity, bool>> filter, params Expression<Func<TDTOEntity, object>>[] includes);
    }
}
