namespace MedSync.Application.Responses;

public class Response
{
    public string? Status { get; set; }
    public bool Error { get; set; }

    public Response GerarErro(string status, bool error)
    {
        Status = status;
        Error = error;
        return this;
    }

    public Response GerarErro(List<string?> notificacoes, bool error)
    {
        Status = string.Join("\n", notificacoes);
        Error = error;
        return this;
    }
}
