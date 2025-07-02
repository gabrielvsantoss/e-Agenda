using e_Agenda.Dominio.ModuloContato;
using Microsoft.Data.SqlClient;

namespace eAgenda.Infraestrutura.SqlServer;

public class RepositorioContatoEmSql : IRepositorioContato
{
    private readonly string connectionString = 
 "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaDb;Integrated Security=True";

    public void CadastrarRegistro(Contato novoRegistro)
    {
        var sqlInserir =
            @"INSERT INTO [TBContato]
            (
                [ID], 
                [NOME], 
                [EMAIL], 
                [TELEFONE], 
                [EMPRESA], 
                [CARGO] )

            VALUES
            (
                @ID, 
                @NOME, 
                @EMAIL, 
                @TELEFONE, 
                @EMPRESA, 
                @CARGO);     
            SELECT SCOPE_IDENTITY()";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);
        SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

        ConfigurarParametrosContato(novoRegistro, comandoInsercao);
        conexaoComBanco.Open();
        comandoInsercao.ExecuteNonQuery();  
        conexaoComBanco.Close();


    }

    public bool EditarRegistro(Guid idRegistro, Contato registroEditado)
    {
        var sqlEditarRegistro  = 
            @"UPDATE [TBContato]
            SET 
                [NOME] = @NOME, 
                [EMAIL] = @EMAIL, 
                [TELEFONE] = @TELEFONE, 
                [CARGO] = @CARGO, 
                [EMPRESA] = @EMPRESA
            WHERE 
                [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);
        SqlCommand comandoEdicao = new SqlCommand(sqlEditarRegistro, conexaoComBanco);

        registroEditado.Id = idRegistro;

        ConfigurarParametrosContato(registroEditado, comandoEdicao);

        conexaoComBanco.Open();

        var linhasAfetadas = comandoEdicao.ExecuteNonQuery();
        conexaoComBanco.Close();
        return linhasAfetadas > 0;

    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var sqlExcluirRegistro =
            @"DELETE FROM [TBContato]
            WHERE 
                [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);
        SqlCommand comandoExclusao = new SqlCommand(sqlExcluirRegistro, conexaoComBanco);

        comandoExclusao.Parameters.AddWithValue("ID", idRegistro);
        conexaoComBanco.Open();

        var linhasAfetadas = comandoExclusao.ExecuteNonQuery();
        conexaoComBanco.Close();
        return linhasAfetadas > 0;
    }

    public Contato SelecionarRegistroPorId(Guid idRegistro)
    {
        var sqlSelecionarPorId =
            @"SELECT 
            [ID], 
            [NOME], 
            [EMAIL], 
            [TELEFONE], 
            [CARGO], 
            [EMPRESA]
        FROM
               [TBCONTATO]
        WHERE 
            [ID] = @ID";

        SqlConnection conexaoComBanco = new SqlConnection(connectionString);
        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorId, conexaoComBanco);

        comandoSelecao.Parameters.AddWithValue("ID", idRegistro);

        conexaoComBanco.Open();

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        Contato? contato = null;

        if(leitor.Read())
        {
            contato = ConverterParaContato(leitor);
        }
        conexaoComBanco.Close();
        return contato;
    }

    public List<Contato> SelecionarRegistros()
    {
        var sqlSelecionarTodos = @"SELECT 
            [Id], 
            [Nome], 
            [Email], 
            [Telefone], 
            [Cargo], 
            [Empresa] 
        FROM 
            TBContato";

         SqlConnection conexaoComBanco = new SqlConnection(connectionString);

        conexaoComBanco.Open();

        SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

        SqlDataReader leitor = comandoSelecao.ExecuteReader();

        var contatos = new List<Contato>();

        while(leitor.Read())
        {
            var contato = ConverterParaContato(leitor);
            contatos.Add(contato);
        }
        conexaoComBanco.Close();
        return contatos;
    }
    private Contato ConverterParaContato(SqlDataReader leitor)
    {
        var contato = new Contato(
            Convert.ToString(leitor["NOME"])!,
            Convert.ToString(leitor["EMAIL"])!,
            Convert.ToString(leitor["TELEFONE"])!,
            Convert.ToString(leitor["CARGO"])!,
            Convert.ToString(leitor["EMPRESA"])!
        );

        contato.Id = Guid.Parse(leitor["ID"].ToString()!);
        

        return contato;
    }
    private void ConfigurarParametrosContato(Contato contato, SqlCommand comando)
    {
        comando.Parameters.AddWithValue("ID", contato.Id);
        comando.Parameters.AddWithValue("NOME", contato.Nome);
        comando.Parameters.AddWithValue("EMAIL", contato.Email);
        comando.Parameters.AddWithValue("TELEFONE", contato.Telefone);
        comando.Parameters.AddWithValue("CARGO", string.IsNullOrEmpty(contato.Cargo) ? DBNull.Value : contato.Cargo);
        comando.Parameters.AddWithValue("EMPRESA", string.IsNullOrEmpty(contato.Empresa) ? DBNull.Value : contato.Empresa);
    }
}