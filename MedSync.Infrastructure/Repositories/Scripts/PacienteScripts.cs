namespace MedSync.Infrastructure.Repositories.Scripts;

public class PacienteScripts
{
    internal static readonly string SelectBase =
       @"
           SELECT
            p.Id, 
            p.PessoaId,
            pe.Id,
            pe.Nome,
            pe.CPF,
            pe.RG,
            pe.Sexo,
            pe.DataNascimento,
            pe.Email,
            pe.CriadoEm,
            pe.CriadoPor,
            pe.ModificadoEm,
            pe.ModificadoPor,
            e.Id,
            e.PacienteId,
            e.Logradouro,
            e.Numero,
            e.complemento,
            e.Bairro,
            e.Cidade,
            e.Estado,
            e.CEP,
            e.CriadoEm,
            e.CriadoPor,
            e.ModificadoEm,
            e.ModificadoPor,
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
            pacientes p INNER JOIN
            pessoas pe ON pe.Id = p.PessoaId
                AND pe.ExcluidoEm IS NULL INNER JOIN
	        enderecos e ON e.PacienteId = p.Id
		        AND e.ExcluidoEm IS NULL INNER JOIN
            telefones t ON t.PacienteId = p.Id
                AND t.ExcluidoEm IS NULL     
        WHERE
            p.ExcluidoEm IS NULL
        ";

    internal static readonly string Insert =
        @"
            INSERT INTO pacientes(
                Id, 
                PessoaId,
                CriadoEm,
                CriadoPor,
                ModificadoEm,
                ModificadoPor
            )VALUES(
                @Id, 
                @PessoaId,
                @CriadoEm,
                @CriadoPor,
                @ModificadoEm,
                @ModificadoPor
            )
        ";

    internal static readonly string Update =
        @"
            UPDATE pacientes SET
                ModificadoEm = @ModificadoEm
            WHERE
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string Delete =
        @"
            UPDATE pacientes SET
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
                pacientes
            WHERE 
                Id = @Id
                AND ExcluidoEm IS NULL
        ";

    internal static readonly string WhereId =
        @"
            AND p.Id = @Id
        ";
}
