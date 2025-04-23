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
    public static string CaractereInvalido =
        "O campo {PropertyName} deve possuir somente um caracter.";
    public static string AgendamentoPeriodo =
        "Não há disponibilidade de agendamento para o período informado.";
    public static string SigaInvalida =
       "O campo {PropertyName} deve ter 2 caracteres.";
    public static string PeriodoInvalido =
        "O período informado existe em nossa base de dados!."; 
    public static string HorarioInvalido =
        "O horário informado não está disponível para agendamento.";
    public static string DataInvalida =
        "Data não pode se menor que o valor da  data atual.";
    public static string HoraIntervaloInvalido =
        "Horário deve ter 20 minutos de diferença do horário anterior.";

}
