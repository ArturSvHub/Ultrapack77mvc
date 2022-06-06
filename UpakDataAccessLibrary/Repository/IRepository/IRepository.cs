
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UpakDataAccessLibrary.Repository.IRepository
{
	public interface IRepository<T> where T:class
	{
		Task<T> FindAsync(int id);
		IEnumerable<T> GetAll(
			Expression<Func<T, bool>> filter = null,
			Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
			string includeProperties = null,
			bool isTracking = true
			);


		T FirstOrDefault(
			Expression<Func<T, bool>> filter = null,
			string includeProperties = null,
			bool isTracking = true
			);

		Task AddAsync(T entity);
		void Remove(T entity);
		Task SaveAsync();
	}
}
