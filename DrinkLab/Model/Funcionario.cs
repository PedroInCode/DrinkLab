using DrinkLab.Data;
using DrinkLab.Model.Enums;
using MySqlConnector;

namespace DrinkLab.Model;

internal class Funcionario
{
    //Propriedades Privadas ( _nome )
    private int _id;
    private string _nome;
    private string _documento;
    private TipoDocumento _tipoDocumento;
    private string _telefone;
    private string _email;
    private TipoCargo _tipoCargo;
    private decimal _salario;
    private DateTime _dataAdmissao;
    private bool _ativo;
    private int _usuarioId;

    //Propriedades Públicas ( Nome )
    public int Id { get => _id; set => _id = value; }
    public string Nome { get => _nome; set => _nome = value; }
    public string Documento { get => _documento; set => _documento = value; }
    public TipoDocumento TipoDocumento { get => _tipoDocumento; set => _tipoDocumento = value; }
    public string Telefone { get => _telefone; set => _telefone = value; }
    public string Email { get => _email; set => _email = value; }
    public TipoCargo TipoCargo { get => _tipoCargo; set => _tipoCargo = value; }
    public decimal Salario { get => _salario; set => _salario = value; }
    public DateTime DataAdmissao { get => _dataAdmissao; set => _dataAdmissao = value; }
    public bool Ativo { get => _ativo; set => _ativo = value; }
    public int UsuarioId { get => _usuarioId; set => _usuarioId = value; }

    // Construtor para novo cadastro (sem ID)
    public Funcionario(string nome, string documento, TipoDocumento tipoDocumento, string telefone, string email, TipoCargo tipoCargo, decimal salario, DateTime dataAdmissao, int usuarioId)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new Exception("O nome do funcionário não pode ser vazio!");

        this.Nome = nome;
        this.Documento = documento;
        this.TipoDocumento = tipoDocumento;
        this.Telefone = telefone;
        this.Email = email;
        this.TipoCargo = tipoCargo;
        this.Salario = salario;
        this.DataAdmissao = dataAdmissao;
        this.Ativo = true;
        this.UsuarioId = usuarioId;
    }

    // Construtor para registros vindos do banco (com ID)
    public Funcionario(int id, string nome, string documento, TipoDocumento tipoDocumento, string telefone, string email, TipoCargo tipoCargo, decimal salario, DateTime dataAdmissao, bool ativo, int usuarioId)
    {
        this.Id = id;
        this.Nome = nome;
        this.Documento = documento;
        this.TipoDocumento = tipoDocumento;
        this.Telefone = telefone;
        this.Email = email;
        this.TipoCargo = tipoCargo;
        this.Salario = salario;
        this.DataAdmissao = dataAdmissao;
        this.Ativo = ativo;
        this.UsuarioId = usuarioId;
    }

    public static int Inserir(Funcionario funcionario)
    {
        string sql = @"INSERT INTO Funcionarios (`Nome`, `Documento`, `TipoDocumento`, `Telefone`, `Email`, `TipoCargo`, `Salario`, `DataAdmissao`, `Ativo`, `UsuarioId`) 
                       VALUES (@Nome, @Documento, @TipoDocumento, @Telefone, @Email, @TipoCargo, @Salario, @DataAdmissao, @Ativo, @UsuarioId)";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Nome", funcionario.Nome);
            cmd.Parameters.AddWithValue("@Documento", funcionario.Documento);
            cmd.Parameters.AddWithValue("@TipoDocumento", funcionario.TipoDocumento.ToString()); // Converte Enum para String pro ENUM do MySQL
            cmd.Parameters.AddWithValue("@Telefone", funcionario.Telefone);
            cmd.Parameters.AddWithValue("@Email", funcionario.Email);
            cmd.Parameters.AddWithValue("@TipoCargo", funcionario.TipoCargo.ToString()); // Converte Enum para String pro ENUM do MySQL
            cmd.Parameters.AddWithValue("@Salario", funcionario.Salario);
            cmd.Parameters.AddWithValue("@DataAdmissao", funcionario.DataAdmissao);
            cmd.Parameters.AddWithValue("@Ativo", funcionario.Ativo);
            cmd.Parameters.AddWithValue("@UsuarioId", funcionario.UsuarioId);

            return Banco.ExecutarComando(cmd);
        }
    }

    public static int Deletar(Funcionario funcionario)
    {
        string cmdSql = "DELETE FROM `Funcionarios` WHERE `Id` = @Id";

        using (var cmd = new MySqlCommand(cmdSql))
        {
            cmd.Parameters.AddWithValue("@Id", funcionario.Id);
            return Banco.ExecutarComando(cmd);
        }
    }

    public static List<Funcionario> ListarTodosFuncionarios()
    {
        string cmdSql = "SELECT * FROM `Funcionarios`";
        List<Funcionario> listaFuncionarios = new();

        using (var cmd = new MySqlCommand(cmdSql))
        {
            using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string nome = ValidarCampo(reader["Nome"]);
                    string documento = ValidarCampo(reader["Documento"]);

                    Enum.TryParse(reader["TipoDocumento"].ToString(), out TipoDocumento tipoDoc);

                    string telefone = ValidarCampo(reader["Telefone"]);
                    string email = ValidarCampo(reader["Email"]);

                    Enum.TryParse(reader["TipoCargo"].ToString(), out TipoCargo tipoCargo);

                    decimal salario = reader.GetDecimal("Salario");
                    DateTime dataAdmissao = reader.GetDateTime("DataAdmissao");
                    bool ativo = reader.GetBoolean("Ativo");
                    int usuarioId = reader.GetInt32("UsuarioId");

                    Funcionario func = new(id, nome, documento, tipoDoc, telefone, email, tipoCargo, salario, dataAdmissao, ativo, usuarioId);
                    listaFuncionarios.Add(func);
                }
            }

            return listaFuncionarios;
        }
    }

    public static Funcionario? ListarPorId(int id)
    {
        string sql = "SELECT * FROM `Funcionarios` WHERE `Id` = @Id";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
            {
                Funcionario? funcionario = null;
                while (reader.Read())
                {
                    int idBanco = reader.GetInt32("Id");
                    string nome = ValidarCampo(reader["Nome"]);
                    string documento = ValidarCampo(reader["Documento"]);

                    Enum.TryParse(reader["TipoDocumento"].ToString(), out TipoDocumento tipoDoc);

                    string telefone = ValidarCampo(reader["Telefone"]);
                    string email = ValidarCampo(reader["Email"]);

                    Enum.TryParse(reader["TipoCargo"].ToString(), out TipoCargo tipoCargo);

                    decimal salario = reader.GetDecimal("Salario");
                    DateTime dataAdmissao = reader.GetDateTime("DataAdmissao");
                    bool ativo = reader.GetBoolean("Ativo");
                    int usuarioId = reader.GetInt32("UsuarioId");

                    funcionario = new(idBanco, nome, documento, tipoDoc, telefone, email, tipoCargo, salario, dataAdmissao, ativo, usuarioId);
                }
                return funcionario;
            }
        }
    }

    public static int AtualizarFuncionario(Funcionario funcionario)
    {
        string sql = @"UPDATE `Funcionarios` SET `Nome` = @Nome, `Documento` = @Documento, `TipoDocumento` = @TipoDocumento, 
                       `Telefone` = @Telefone, `Email` = @Email, `TipoCargo` = @TipoCargo, `Salario` = @Salario, 
                       `Ativo` = @Ativo WHERE `Id` = @Id";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Nome", funcionario.Nome);
            cmd.Parameters.AddWithValue("@Documento", funcionario.Documento);
            cmd.Parameters.AddWithValue("@TipoDocumento", funcionario.TipoDocumento.ToString());
            cmd.Parameters.AddWithValue("@Telefone", funcionario.Telefone);
            cmd.Parameters.AddWithValue("@Email", funcionario.Email);
            cmd.Parameters.AddWithValue("@TipoCargo", funcionario.TipoCargo.ToString());
            cmd.Parameters.AddWithValue("@Salario", funcionario.Salario);
            cmd.Parameters.AddWithValue("@Ativo", funcionario.Ativo);
            cmd.Parameters.AddWithValue("@Id", funcionario.Id);

            return Banco.ExecutarComando(cmd);
        }
    }

    private static string ValidarCampo(object campo)
    {
        if (campo == DBNull.Value || campo == null)
        {
            return "";
        }
        else
        {
            return campo.ToString()!;
        }
    }
}