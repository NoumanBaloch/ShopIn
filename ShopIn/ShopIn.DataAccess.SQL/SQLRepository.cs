using ShopIn.Core.Contracts;
using ShopIn.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopIn.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _dbSet;

        public SQLRepository(DataContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }
        public IEnumerable<T> Collection()
        {
            return _dbSet;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var item = Find(Id);
            if (_context.Entry(item).State == EntityState.Detached)
                _dbSet.Attach(item);

            _dbSet.Remove(item);
        }

        public T Find(string Id)
        {
           return _dbSet.Find(Id);
        }

        public void Insert(T item)
        {
            _dbSet.Add(item);
        }

        public void Update(T item)
        {
            _dbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            
        }
    }
}
