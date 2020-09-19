using System;
using System.Collections.Generic;
using System.Linq;

namespace ISUCorp.Services.Resources.Responses
{
    public class PagedResponse<T> : DataResponse<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedResponse() : base()
        {

        }

        public PagedResponse(string errorMessage, int pageNumber, int pageSize) : base(errorMessage)
        {
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalCount = 0;
            TotalPages = 0;
        }

        public PagedResponse(T items, int count, int pageNumber, int pageSize) : base(items)
        {
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public static PagedResponse<List<T>> ToPagedResponse(
            IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResponse<List<T>>(items, count, pageNumber, pageSize);
        }
    }
}
