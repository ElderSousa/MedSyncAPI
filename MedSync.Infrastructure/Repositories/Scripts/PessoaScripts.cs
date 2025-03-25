using System.Runtime.CompilerServices;

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
                CriadoEm,
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
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id,
                @Nome,
                @CPF,
                @RG, 
                @DataNascimento,
                @Email,
                @Sexo,
                @CriadoEm,
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
                AND ExcluidoEm IS NULL
                
        ";
    
    internal static readonly string CPFExiste =
        @"
            SELECT
                COUNT(*)
            FROM
                pessoas
            WHERE
                CPF = @CPF
                AND ExcluidoEm IS NULL
                
        ";
    
    internal static readonly string WhereId =
        @"
            AND Id = @Id              
        "; 
    
    internal static readonly string WhereCPF =
        @"
            AND CPF = @CPF      
        ";

    internal static readonly string Delete =
        @"
            UPDATE SET pessoas
                 ModifiadoEm = @ModificadoEm,
                 ModificadoPor = @ModificadoPor,
                 ExcluidoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";
    internal static readonly string Update =
         @"
            UPDATE SET pessoas
                Nome = @Nome,
                CPF = @CPF,
                RG = @RG,
                DataNascimento = @DataNascimento,
                Email = @Email,
                Sexo = @Sexo,
                ModifiadoEm = @ModificadoEm,
                ModificadoPor = @ModificadoPor,
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";


}
