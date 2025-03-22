namespace MedSync.Infrastructure.Repositories.Scripts;

public static class PessoaScripts
{
    internal static readonly string SelectBase =
        @"
            SELECT 
                Id,
                Nome,
                CPF,
                RG, 
                DataNascimento,
                Email,
                Sexo,
                CriadoEm
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            FROM
                pessoas
            WHERE
                ExcluidoEm IS NULL
        ";

    internal static readonly string Insert =
        @"
            INSERT INTO pessoas(
                Id,
                Nome,
                CPF,
                RG, 
                DataNascimento,
                Email,
                Sexo,
                CriadoEm
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUE(
                @Id,
                @Nome,
                @CPF,
                @RG, 
                @DataNascimento,
                @Email,
                @Sexo,
                @CriadoEm
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
            )
        ";
    internal static readonly string Existe =
        @"
            SELECT
                COUNT(*)
            FROM
                pessoas
            WHERE
                Id = @Id
                
        ";
    
    internal static readonly string CPFExiste =
        @"
            SELECT
                COUNT(*)
            FROM
                pessoas
            WHERE
                CPF = @CPF
                
        ";
}
