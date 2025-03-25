namespace MedSync.Application.Validation;

public static class MessagesValidation
{
    public static string CampoObrigatorio =
        "O campo {PropertyName} precisa ser fornecido.";
    public static string CPFInvalido =
        "CPF inválido.";
    public static string PessoaExiste =
        "Pessoa já cadastrada em nossa base de dados.";
    public static string CPFCadastrado =
        "CPF já cadastrado em nossa base de dados.";
    public static string NomeInvalido =
        "O campo {PropertyName} deve ter no mínimo 3 caracteres.";
    public static string EmailInvalido =
        "Email inválido.";
    public static string NaoEncontrado =
        "{PropertyName} não foi encontrado.";
}
