using Microsoft.EntityFrameworkCore;
using OAuthWithGoogle.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OAuthWithGoogle.Repository
{
    public class BaseRepository : IBaseRepository
    {
        /// <summary>
        /// The _context
        /// </summary>
        ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        public BaseRepository(ApplicationDbContext db_context)
        {
            _context = db_context;
        }
        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete<T>(T instance) where T : class
        {

            _context.Set<T>().Remove(instance);
            _context.SaveChanges();
        }

        /// <summary>
        /// Executes the SQL command.
        /// </summary>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int ExecuteSqlCommand(string sqlCommand, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// Predicate.
        /// </returns>
        public IList<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Finds the top n.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<T> FindTopN<T>(Expression<Func<T, bool>> predicate, int count = 0) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// All Data.
        /// </returns>
        public IList<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().ToList();

        }



        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includes">The includes.</param>
        /// <returns>
        /// All Data.
        /// </returns>
        public IList<T> GetAll<T>(params string[] includes) where T : class
        {
            var results = _context.Set<T>();
            return includes.Aggregate<string, DbSet<T>>(results, (current, inc) => (DbSet<T>)current.Include(inc)).ToList();
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="sorts">The sorts.</param>
        /// <param name="filters">The filters.</param>
        /// <returns>
        /// All Data.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<T> GetAll<T>(int page, int pageSize, List<string> sorts, List<KeyValuePair<string, string>> filters) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> GetAllQuery<T>() where T : class
        {
            return _context.Set<T>();
        }

        public IQueryable<T> GetAllQuery<T>(string table) where T : class
        {
            return _context.Set<T>().Include(table);
        }

        public IQueryable<T> GetAllQuery<T>(string table, string table2) where T : class
        {
            return _context.Set<T>().Include(table).Include(table2);
        }

        /// <summary>
        /// Gets all for given page.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>
        /// All Data.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<T> GetAllToPage<T>(int take, int skip, int page, int pageSize) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// Instance.
        /// </returns>
        public T Insert<T>(T instance) where T : class
        {
            _context.Set<T>().Add(instance);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return instance;
        }

        /// <summary>
        /// Inserts the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<T> Insert<T>(IList<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                _context.Set<T>().Add(instance);
                _context.SaveChanges();
            }
            return instances;
        }

        /// <summary>
        /// Takes the specified no of item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="noOfItem">The no of item.</param>
        /// <param name="isDesc">if set to <c>true</c> [is desc].</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<T> Take<T>(int noOfItem, bool isDesc) where T : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="commit"></param>
        /// <returns>
        /// Instance.
        /// </returns>
        public T Update<T>(T instance, bool commit = true) where T : class
        {
            _context.Entry(instance).State = EntityState.Modified;
            _context.SaveChanges();
            return instance;
        }


        public IList<T> Update<T>(IList<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                _context.Entry(instance).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return instances;
        }

        public T PopulateAuditFields<T>(T entity, int? userId, bool isInsert = true)
        {
            //var obj = entity as ITrackedModel;
            //if (isInsert)
            //{
            //    obj.CreatedUtc = DateTime.UtcNow;
            //    obj.CreatedById = userId;
            //}
            //obj.ModifiedUtc = DateTime.UtcNow;
            //obj.ModifiedById = userId;
            //return (T)obj;
            throw new NotImplementedException();
        }

        public void Delete<T>(IList<T> instances) where T : class
        {
            foreach (var instance in instances)
            {
                _context.Set<T>().Remove(instance);
                _context.SaveChanges();
            }
        }
    }
}
