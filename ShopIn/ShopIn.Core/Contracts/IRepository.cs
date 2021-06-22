using System.Collections.Generic;
using ShopIn.Core.Models;

namespace ShopIn.Core.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> Collection();
        void Commit();
        void Delete(string Id);
        T Find(string Id);
        void Insert(T item);
        void Update(T item);
    }
}