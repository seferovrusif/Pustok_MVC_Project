using System.Collections;

namespace Pustok_project.ViewModels.Common
{
    public class LoadMoreVM<T> where T : IEnumerable
    {
        public int LastPage { get; }
        public int CurrentPage { get; }
        public bool HasLoadMore { get; }
        public T Items { get; }
        public LoadMoreVM(int totalCount, int currentPage, int lastPage, T items)
        {
            CurrentPage = currentPage;
            LastPage = lastPage;
            Items = items;
            if (currentPage < lastPage)
            {
                HasLoadMore = true;
            }
            if (currentPage>=lastPage)
            {
                HasLoadMore = false;
            }

        }
    }
}