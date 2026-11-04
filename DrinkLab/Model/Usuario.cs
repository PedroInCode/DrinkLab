using MySqlConnector;
using DrinkLab.Data; 

namespace DrinkLab.Model;

internal class Usuario
{
    private int _id;
    private string _login;
    private string _senha;

    public int Id { get => _id; set => _id = value; }
    public string Login { get => _login; set => _login = value; }
    public string Senha { get => _senha; set => _senha = value; }

    // Construtor que recebe texto limpo e aplica o BCrypt na senha
    public Usuario(string login, string senhaTextoLimpo)
    {
        if (string.IsNullOrWhiteSpace(login)) throw new Exception("O login não pode ser vazio!");
        if (string.IsNullOrWhiteSpace(senhaTextoLimpo)) throw new Exception("A senha não pode ser vazia!");

        this.Login = login;
        this.Senha = BCrypt.Net.BCrypt.HashPassword(senhaTextoLimpo);
    }

    // Construtor para buscar do banco
    public Usuario(int id, string login, string senhaHash)
    {
        this.Id = id;
        this.Login = login;
        this.Senha = senhaHash;
    }

    // Método estático para inserir no banco e retornar o ID gerado
    public static int Inserir(Usuario usuario)
    {
        string sql = "INSERT INTO Usuarios (Login, Senha) VALUES (@Login, @Senha); SELECT LAST_INSERT_ID();";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Login", usuario.Login);
            cmd.Parameters.AddWithValue("@Senha", usuario.Senha);

            // Certifique-se de que sua classe Banco possui esse método para retornar o ID gerado no INSERT.
            return Banco.ExecutarComandoComRetornoId(cmd);
        }
    }
}