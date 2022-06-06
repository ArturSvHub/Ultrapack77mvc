using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using UpakDataAccessLibrary.DataContext;
using UpakDataAccessLibrary.Repository.IRepository;

namespace UpakDataAccessLibrary.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly MssqlContext _context;
		internal DbSet<T> dbSet;

		public Repository(MssqlContext context)
		{
			_context = context;
			this.dbSet = _context.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await dbSet.AddAsync(entity);
		}

		public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (includeProperties != null)
			{
				foreach (var incProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(incProp);
				}
			}
			if (!isTracking)
			{
				query = query.AsNoTracking();
			}
			return query.FirstOrDefault();
		}

		public async Task<T> FindAsync(int id)
		{
			return await dbSet.FindAsync(id);
		}

		public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, bool isTracking = true)
		{
			IQueryable<T> query = dbSet;
				if(filter!=null)
			{
				query = query.Where(filter);
			}
				if(includeProperties!=null)
			{
				foreach(var incProp in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(incProp);
				}
			}
				if(orderBy!=null)
			{
				query = orderBy(query);
			}
				if(!isTracking)
			{
				query = query.AsNoTracking();
			}
			return query.ToList();
 		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public async Task SaveAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}
