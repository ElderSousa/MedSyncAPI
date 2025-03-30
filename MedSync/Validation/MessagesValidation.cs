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
    public static string CEPInvalido =
        "CEP inválido. Formato esperado: 12345-678 ou 12345678.";
    public static string NumeroInvalido =
        "Número inválido. Formato esperado: xx-xxxxx-xxxx."; 
   public static string CRMInvalido =
        "CRM inválido. O formato esperado é '123456/SP' ou apenas números como 1234 ou 123456."; 
    public static string CRMExiste =
        "CRM encontra-se cadastrado em nossa base de dados.";

}
