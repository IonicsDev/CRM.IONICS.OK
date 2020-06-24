using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using VXERP.Common.ContextDomain;

namespace VXERP.Common.EntityDomain
{
    public class UnitOfWork
        : DbContext, IQueryableUnitOfWork
    {
        public UnitOfWork()
            : base("EntitiesConnectionString")
        {
           Database.SetInitializer<UnitOfWork>(null);
          base.Configuration.LazyLoadingEnabled = false;
        }

        #region IDbSet Members

       public IDbSet<Usuario> Usuarios { get { return base.Set<Usuarios>(); } }
       //public IDbSet<Actividades> Actividades { get { return base.Set<Actividades>(); } }
       //public IDbSet<Proveedores> Proveedores { get { return base.Set<Proveedores>(); } }
       //public IDbSet<Divisiones> Divisiones { get { return base.Set<Divisiones>(); } }
       //public IDbSet<Cantones> Cantones { get { return base.Set<Cantones>(); } }

        #endregion

       #region IQueryableUnitOfWork Members

       public DbSet<TEntity> CreateSet<TEntity>()
           where TEntity : class
       {
           return base.Set<TEntity>();
       }

       public IEntityTypedId<TId> GetContextEntity<TEntity, TId>(TEntity item) where TEntity : class, IEntityTypedId<TId>
       {
           try
           {
               var original = base.Set<TEntity>().Find(item.Id);
               return original;
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
       }

       public TEntity Attach<TEntity, TId>(TEntity item)
           where TEntity : class, IEntityTypedId<TId>
       {
           if (base.Entry<TEntity>(item).State == EntityState.Detached)
           {
               item = base.Set<TEntity>().Find(item.Id);
           }
           //attach and set as unchanged
           base.Entry<TEntity>(item).State =EntityState.Unchanged;
           return item;
       }

       public void Attach<TEntity>(TEntity item)
           where TEntity : class
       {
           //attach and set as unchanged
           base.Entry<TEntity>(item).State = EntityState.Unchanged;
       }

       public void SetModified<TEntity, TId>(TEntity item)
           where TEntity : class, IEntityTypedId<TId>, new()
       {
           if (base.Entry<TEntity>(item).State == EntityState.Detached)
           {
               var oldItem = base.Set<TEntity>().Find(item.Id);
               ApplyCurrentValues<TEntity>(oldItem, item);
           }
           else
           {
               //this operation also attach item in object state manager
               base.Entry<TEntity>(item).State = EntityState.Modified;
           }
       }

       public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
           where TEntity : class
       {
           //if it is not attached, attach original and set current values
           base.Entry<TEntity>(original).CurrentValues.SetValues(current);
       }

       public void Commit()
       {
           base.SaveChanges();
       }

       public void CommitAndRefreshChanges()
       {
           bool saveFailed = false;

           do
           {
               try
               {
                   base.SaveChanges();

                   saveFailed = false;

               }
               catch (DbUpdateConcurrencyException ex)
               {
                   saveFailed = true;

                   ex.Entries.ToList()
                             .ForEach(entry =>
                             {
                                 entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                             });

               }
           } while (saveFailed);

       }

       public void RollbackChanges()
       {
           base.ChangeTracker.Entries()
                             .ToList()
                             .ForEach(entry => entry.State = EntityState.Unchanged);
       }

       #region ISql Members

       public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
       {
           return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
       }

       public int ExecuteCommand(string sqlCommand, params object[] parameters)
       {
           return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
       }

       #endregion

       #endregion

        #region DbContext Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>().ToTable("Usuarios");
          //  modelBuilder.Entity<Actividades>().HasRequired(p => p.Proveedores).WithOptional();
            modelBuilder.Entity<Cantones>().HasRequired(p => p.Proveedores).WithOptional();
        

          //  modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Configurations.Add(new MenuEntityTypeConfiguration());
            //modelBuilder.Configurations.Add(new PersonEntityTypeConfiguration());
            //modelBuilder.Configurations.Add(new CountryEntityTypeConfiguration());
            //modelBuilder.Configurations.Add(new UserEntityTypeConfiguration());
            //modelBuilder.Configurations.Add(new RoleEntityTypeConfiguration());
        }
        #endregion


        public DbContext DBContext
        {
            get
            {
                return this;
            }
    
        }
    }
}
