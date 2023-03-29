using box.infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace box.infrastructure.Data.Repositories
{
    internal class GenericRepository<T> where T : class, IEntity
    {
        protected readonly DbContext m_DbContext;
        protected readonly DbSet<T> m_DbSet;

        public GenericRepository(DbContext p_DbContext, ILogger p_Logger)
        {
            if (p_DbContext == null)
                throw new ArgumentNullException(nameof(p_DbContext), "DbContext cannot be null");

            this.m_DbContext = p_DbContext;
            this.m_DbSet = m_DbContext.Set<T>();
            this.Logger = p_Logger;
        }

        protected ILogger Logger { get; set; }

        /// <summary>
        /// Get the current <see cref="DbContext"/>
        /// </summary>
        public DbContext DbContext => this.m_DbContext;

        public Task AddAsync(T p_Entity)
        {
            if (p_Entity == null)
                throw new ArgumentNullException(nameof(p_Entity), "Entity cannot be null");

            return AddInternalAsync(p_Entity);
        }

        public Task AddAsync(IEnumerable<T> p_Entities)
        {
            if (p_Entities == null || !p_Entities.Any())
                throw new ArgumentNullException(nameof(p_Entities), "Entity cannot be null or empty");

            return AddInternalAsync(p_Entities);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> p_Predicate = null)
        {
            return p_Predicate == null ? await this.m_DbSet.LongCountAsync() : await this.m_DbSet.LongCountAsync(p_Predicate);
        }

        public Task DeleteAsync(object p_Id)
        {
            if (p_Id == null)
                throw new ArgumentNullException(nameof(p_Id), "Id cannot be null");

            return this.DeleteInternalAsync(p_Id);
        }

        public Task DeleteAsync(T p_Entity)
        {
            if (p_Entity == null)
                throw new ArgumentNullException(nameof(p_Entity), "Entity cannot be null");

            return this.DeleteInternal(p_Entity);
        }

        public Task DeleteAsync(IEnumerable<T> p_Entities)
        {
            if (p_Entities == null)
                throw new ArgumentNullException(nameof(p_Entities), "Entity cannot be null");
            return DeleteInternal(p_Entities);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool p_Disposing)
        {
            // Cleanup
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> p_Predicate)
        {
            if (p_Predicate == null)
                throw new ArgumentNullException(nameof(p_Predicate), "Predicate cannot be null or empty");

            return ExistsInternalAsync(p_Predicate);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> p_Predicate, Func<IQueryable<T>, IOrderedQueryable<T>> p_OrderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> p_Include = null, int? p_Skip = null, int? p_Take = null, bool p_DisableTracking = true)
        {
            IQueryable<T> v_Query = this.m_DbSet;
            if (p_DisableTracking)
                v_Query = v_Query.AsNoTracking<T>();

            if (p_Include != null)
                v_Query = p_Include(v_Query);

            if (p_Predicate != null)
                v_Query = v_Query.Where(p_Predicate);

            if (p_OrderBy != null)
                v_Query = p_OrderBy(v_Query);

            if (p_Skip.HasValue)
                v_Query = v_Query.Skip(p_Skip.Value);

            if (p_Take.HasValue)
                v_Query = v_Query.Take(p_Take.Value);

            return v_Query;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> p_Include = null, Func<IQueryable<T>, IOrderedQueryable<T>> p_OrderBy = null, int? p_Skip = null, int? p_Take = null, bool p_DisableTracking = true)
        {
            return await GetAllInternal(p_Include, p_OrderBy, p_Skip, p_Take, p_DisableTracking).ToListAsync();
        }

        public Task<T> GetAsync(object p_Id)
        {
            if (p_Id == null)
                throw new ArgumentNullException(nameof(p_Id), "Id cannot be null");

            return this.GetInternalAsync(p_Id);
        }

        public Task UpdateAsync(T p_Entity)
        {
            if (p_Entity == null)
                throw new ArgumentNullException(nameof(p_Entity), "Entity cannot be null");

            return this.UpdateInternal(p_Entity);
        }

        public Task UpdateAsync(IEnumerable<T> p_Entities)
        {
            if (p_Entities == null || !p_Entities.Any())
                throw new ArgumentNullException(nameof(p_Entities), "Entity cannot be null or empty");

            return this.UpdateInternal(p_Entities);
        }

        private async Task AddInternalAsync(T p_Entity)
        {
            await this.m_DbSet.AddAsync(p_Entity);
        }

        private async Task AddInternalAsync(IEnumerable<T> p_Entities)
        {
            await this.m_DbSet.AddRangeAsync(p_Entities);
        }

        private async Task DeleteInternalAsync(object p_Id)
        {
            T v_Entity = await this.m_DbSet.FindAsync(p_Id);
            await this.DeleteAsync(v_Entity);
        }

        private Task DeleteInternal(T p_Entity)
        {
            if (this.m_DbContext.Entry(p_Entity).State == EntityState.Detached)
            {
                this.m_DbSet.Attach(p_Entity);
            }
            this.m_DbSet.Remove(p_Entity);
            return Task.CompletedTask;
        }

        private Task DeleteInternal(IEnumerable<T> p_Entities)
        {
            this.m_DbSet.RemoveRange(p_Entities);
            return Task.CompletedTask;
        }

        private async Task<bool> ExistsInternalAsync(Expression<Func<T, bool>> p_Predicate)
        {
            return await this.m_DbSet.AnyAsync(p_Predicate);
        }

        private IQueryable<T> GetAllInternal(Func<IQueryable<T>, IIncludableQueryable<T, object>> p_Include = null, Func<IQueryable<T>, IOrderedQueryable<T>> p_OrderBy = null, int? p_Skip = null, int? p_Take = null, bool p_DisableTracking = true)
        {
            return this.FindBy(null, p_OrderBy, p_Include, p_Skip, p_Take, p_DisableTracking);
        }

        private async Task<T> GetInternalAsync(object p_Id)
        {
            return await this.m_DbSet.FindAsync(p_Id);
        }

        private Task UpdateInternal(T p_Entity)
        {
            if (((int)m_DbContext.Entry<T>(p_Entity).State) < 2)
                m_DbContext.Entry<T>(p_Entity).State = EntityState.Modified;
            this.m_DbSet.Update(p_Entity);
            return Task.CompletedTask;
        }

        private Task UpdateInternal(IEnumerable<T> p_Entities)
        {
            this.m_DbSet.UpdateRange(p_Entities);
            return Task.CompletedTask;
        }
    }
}