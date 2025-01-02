using Shopping.Shared.Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Core.Paging
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
           
                var count = source.Count();
                var items = source
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize).ToList();

                return new PagedList<T>(items, count, pageNumber, pageSize);
            

        }
    }
}