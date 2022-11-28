namespace MangaStore.Web.Models.Pagination;

public class Pager
{
    public int TotalItens { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int StartPage { get; private set; }
    public int EndPage { get; private set; }

    public Pager()
    {
        
    }

    public Pager(int totalItens, int page, int pageSize = 12)
    {
        int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalItens) / Convert.ToDecimal(pageSize)));
        int currentPage = page;
        
        int startPage = currentPage - 2;
        int endPage = currentPage + 2;
        
        if(startPage <= 0)
        {
            endPage = endPage - (startPage - 1);
            startPage = 1;
        }

        if (endPage > totalPages)
        {
            endPage = totalPages;

            if (endPage > 10)
            {
                startPage = endPage - 9;
            }
        }

        TotalItens = totalItens;
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        StartPage = startPage;
        EndPage = endPage;
    }
}