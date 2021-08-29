using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesData.Logic
{
    public class Pagination<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public Pagination(List<T> items, int pageIndex, int pageSize, int count)
        {
            PageIndex = pageIndex;
            TotalPages = count / pageSize;

            this.AddRange(items);

        }

        public static Pagination<T> Display(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var item = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new Pagination<T>(item, pageIndex, pageSize, count);
            
        }

    }
}
