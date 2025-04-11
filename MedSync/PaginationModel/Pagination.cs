namespace MedSync.Application.PaginationModel;

public class Pagination<T>
{
    public int CurrentPage { get; set; }
    public int QuantityOfPages { get; set; }
    public int TotalItens { get; set; }
    public List<T> Itens { get; set; } = new();
}
