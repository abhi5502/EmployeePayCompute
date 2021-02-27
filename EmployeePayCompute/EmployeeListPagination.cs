using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePayCompute
{
    public class EmployeeListPagination<T>:List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPage { get; set; }

        public  EmployeeListPagination (List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPage = (int)Math.Ceiling(count/(double)pageSize);
            this.AddRange(items);
        }

        //Enable or Disable our paging button 
        public bool IsPeriviousPageAvailable => PageIndex > 1;

        public bool IsNextPageAvailable => PageIndex < TotalPage;

        public static EmployeeListPagination <T> Create(IList<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new EmployeeListPagination<T>(items,count, pageIndex, pageSize);
        }

    }
}
