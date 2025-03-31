namespace MedSync.Infrastructure.Repositories.Scripts;

public class MedicoScripts
{
    internal static readonly string SelectBase =
        @"
            SELECT
                m.Id, 
                m.PessoaId,
                m.CRM,
                m.Especialidade,
                m.CriadoEm,
                m.CriadoPor,
                m.ModificadoEm,
                m.ModificadoPor,
                p.Id,
                p.Nome,
                p.CPF,
                p.RG,
                p.Sexo,
                p.DataNascimento,
                p.Email,
                p.CriadoEm,
                p.CriadoPor,
                p.ModificadoEm,
                p.ModificadoPor,
                t.Id,
                t.PacienteId,
                t.MedicoId,
                t.Numero,
                t.Tipo,
                t.CriadoEm,
                t.CriadoPor,
                t.ModificadoEm,
                t.ModificadoPor
            FROM
                medicos m INNER JOIN
                pessoas p ON p.Id = m.PessoaId
                    AND p.ExcluidoEm IS NULL INNER JOIN
                telefones t ON t.MedicoId = m.Id
                    AND t.ExcluidoEm IS NULL     
            WHERE
                m.ExcluidoEm IS NULL
        ";

    internal static readonly string Insert =
        @"
            INSERT INTO medicos(
                Id, 
                PessoaId,
                CRM,
                Especialidade,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id, 
                @PessoaId,
                @CRM,
                @Especialidade,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
            )
        ";

    internal static readonly string Update =
        @"
            UPDATE medicos SET
                Id = @Id, 
                CRM = @CRM,
                Especialidade = @Especialidade,
                ModificadoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Delete =
        @"
            UPDATE medicos SET
                ModificadoEm = @ModificadoEm,
                ExcluidoEm = @ModificadoEm
            WHERE 
                Id = @Id
                AND ExcluidoEm IS NULL
        ";
    
    internal static readonly string Existe =
        @"
            SELECT
                COUNT(*)
            FROM 
                medicos
            WHERE 
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string CRMExiste =
        @"
            SELECT
                COUNT(*)
            FROM 
                medicos
            WHERE 
                CRM = @CRM
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string WhereId =
        @"
            AND m.Id = @Id
        ";
    
    internal static readonly string WhereCRM =
        @"
            AND m.CRM = @CRM
        ";
}
