using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CRM.Business.Contexts.ModelBuilderConfiguration;
using CRM.Business.Entities;
using CRM.Common.ContextDomain;
using CRM.Common.EntityDomain;

namespace CRM.Business.Contexts
{
	public class ConfigurationContext : DbContext, IQueryableUnitOfWork
	{
		public ConfigurationContext()
			: base("EntitiesConnectionString")
		{
			Database.SetInitializer<ConfigurationContext>(null);
           //  Database.Log = (s) => System.Diagnostics.Debug.Write(s);

           
			Configuration.LazyLoadingEnabled = false;
			base.Configuration.ProxyCreationEnabled = true;
		}

		#region DbContext Overrides

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add<Usuario>(new UsuarioEntityTypeConfiguration());
           // modelBuilder.Configurations.Add<NoConformidad>(new NoConformidadEntityTypeConfiguration());

            //	modelBuilder.Configurations.Add<Provincia>(new ProvinciaEntityTypeConfiguration());
            //modelBuilder.Configurations.Add<Pais>(new PaisEntityTypeConfiguration());

            //modelBuilder.Configurations.Add<Ciudad>(new CiudadEntityTypeConfiguration());

            modelBuilder.Entity<Tipos_NoConformidades>().ToTable("Tipos_NoConformidades");
            modelBuilder.Entity<Origen_NoConformidades>().ToTable("Origen_NoConformidades");
            modelBuilder.Entity<Identificacion_NoConformidades>().ToTable("Identificacion_NoConformidades");


            modelBuilder.Configurations.Add<RolEmpresa>(new RolesEmpresaEntityTypeConfiguration());
		
			modelBuilder.Configurations.Add<Rol>(new RolEntityTypeConfiguration());

            modelBuilder.Configurations.Add<Parametro>(new ParametroEntityTypeConfiguration());
		
			

			base.OnModelCreating(modelBuilder);
		}
        #endregion

        #region IDbSet Members



        public IDbSet<Acontecimiento> Acontemiento { get { return base.Set<Acontecimiento>(); } }
        public IDbSet<AcontecimientoDetalle> AcontemientoDetalle { get { return base.Set<AcontecimientoDetalle>(); } }
        public IDbSet<TipoAcontecimiento> TipoAcontemiento { get { return base.Set<TipoAcontecimiento>(); } }

        public IDbSet<ModuloPermiso> ModuloPermiso { get { return base.Set<ModuloPermiso>(); } }
		public IDbSet<Usuario> Usuarios { get { return base.Set<Usuario>(); } }
        public IDbSet<NoConformidad> NoConformidad { get { return base.Set<NoConformidad>(); } }
        public IDbSet<RolEmpresa> RolesEmpresa { get { return base.Set<RolEmpresa>(); } }
		public IDbSet<Rol> Roles { get { return base.Set<Rol>(); } }
		public IDbSet<Modulo> Modulos { get { return base.Set<Modulo>(); } }
      
        public IDbSet<ArchivoModulo> ArchivoModulo { get { return base.Set<ArchivoModulo>(); } }
        public IDbSet<UsuarioRolCliente> UsuarioRolCliente { get { return base.Set<UsuarioRolCliente>(); } }

        public IDbSet<Novedad> Novedad { get { return base.Set<Novedad>(); } }
        public IDbSet<NovedadUsuarios> NovedadUsuarios { get { return base.Set<NovedadUsuarios>(); } }
        public IDbSet<TipoNovedad> TipoNovedad { get { return base.Set<TipoNovedad>(); } }

        public IDbSet<Listado> Listado { get { return base.Set<Listado>(); } }
        public IDbSet<PropiedadNavegacionListado> PropiedadNavegacionListado { get { return base.Set<PropiedadNavegacionListado>(); } }

        public IDbSet<Parametro> Parametro { get { return base.Set<Parametro>(); } }

        public IDbSet<Pedido> Pedido { get { return base.Set<Pedido>(); } }
        public IDbSet<PedidoDetalle> PedidoDetalle { get { return base.Set<PedidoDetalle>(); } }
        public IDbSet<PedidoEstado> PedidoEstado { get { return base.Set<PedidoEstado>(); } }
        public IDbSet<ColorEstado> ColorEstado { get { return base.Set<ColorEstado>(); } }

        public IDbSet<Mail> Mail { get { return base.Set<Mail>(); } }
        public IDbSet<MailDestinatario> MailDestinatario { get { return base.Set<MailDestinatario>(); } }

        public IDbSet<Eventualidad> Eventualidad { get { return base.Set<Eventualidad>(); } }
        public IDbSet<TipoEventualidad> TipoEventualidad { get { return base.Set<TipoEventualidad>(); } }
        public IDbSet<SubTipoEventualidad> SubTipoEventualidad { get { return base.Set<SubTipoEventualidad>(); } }

        public IDbSet<EventualidadUsuario> EventualidadUsuario { get { return base.Set<EventualidadUsuario>(); } }
        public IDbSet<EventualidadContacto> EventualidadContacto { get { return base.Set<EventualidadContacto>(); } }
		public IDbSet<Origen> Origen { get { return base.Set<Origen>(); } }

		#endregion

		#region IQueryableUnitOfWork Members
		public DbContext DBContext
		{
			get { return this; }
		}

		public DbSet<TEntity> CreateSet<TEntity>()
			where TEntity : class
		{
			return base.Set<TEntity>();
		}

		public IEntityTypedId<TId> GetContextEntity<TEntity, TId>(TEntity item) where TEntity : class, IEntityTypedId<TId>
		{
			try {
				var original = base.Set<TEntity>().Find(item.Id);
				return original;
			} catch (Exception ex) {
				throw new Exception(ex.Message);

			}
		}

		public TEntity Attach<TEntity, TId>(TEntity item)
			where TEntity : class, IEntityTypedId<TId>
		{
			if (base.Entry<TEntity>(item).State == EntityState.Detached) {
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
			if (base.Entry<TEntity>(item).State == EntityState.Detached) {
				var oldItem = base.Set<TEntity>().Find(item.Id);
				ApplyCurrentValues<TEntity>(oldItem, item);
			} else {
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
            try
            {
                base.SaveChanges();
        } catch (DbEntityValidationException dbEx) {
				foreach (var validationErrors in dbEx.EntityValidationErrors) {
					foreach (var validationError in validationErrors.ValidationErrors) {
						Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
					}
}
			} catch (Exception ex) {
				throw new Exception(ex.Message);
			}
		}

		public void CommitAndRefreshChanges()
		{
			bool saveFailed = false;

			do {
				try {
					base.SaveChanges();

					saveFailed = false;

				} catch (DbUpdateConcurrencyException ex) {
					saveFailed = true;

					ex.Entries.ToList()
							  .ForEach(entry => {
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





	}

}
