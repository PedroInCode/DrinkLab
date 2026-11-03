using DrinkLab.Data;
using DrinkLab.Model.Enums;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkLab.Model;

public class Cliente
{
    //Propriedades Privadas ( _nome )
    private int _id;
    private string _nome;
    private string _documento;
    private TipoDocumento _tipoDocumento;
    private string _telefone;
    private string _email;
    private string _endereco;
    private int _pontos;

    //Propriedades Públicas ( Nome )
    public int Id { get => _id; set => _id = value; }
    public string Nome { get => _nome; set => _nome = value; }
    public string Documento { get => _documento; set => _documento = value; }
    public TipoDocumento TipoDocumento { get => _tipoDocumento; set => _tipoDocumento = value; }
    public string Telefone { get => _telefone; set => _telefone = value; }
    public string Email { get => _email; set => _email = value; }
    public string Endereco { get => _endereco; set => _endereco = value; }
    public int Pontos { get => _pontos; set => _pontos = value; }

    // Construtor para novos cadastros (sem ID)
    public Cliente(string nome, string documento, TipoDocumento tipoDocumento, string telefone, string email, string endereco, int pontos)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new Exception("O nome do cliente não pode ser vazio!");

        this.Nome = nome;
        this.Documento = documento;
        this.TipoDocumento = tipoDocumento;
        this.Telefone = telefone;
        this.Email = email;
        this.Endereco = endereco;
        this.Pontos = pontos;
    }

    // Construtor para registros vindos do banco (com ID)
    public Cliente(int id, string nome, string documento, TipoDocumento tipoDocumento, string telefone, string email, string endereco, int pontos)
    {
        this.Id = id;
        this.Nome = nome;
        this.Documento = documento;
        this.TipoDocumento = tipoDocumento;
        this.Telefone = telefone;
        this.Email = email;
        this.Endereco = endereco;
        this.Pontos = pontos;
    }

    public void AdicionarPontos(int pontos)
    {
        this.Pontos += pontos;
    }

    public static int Inserir(Cliente cliente)
    {
        string sql = "INSERT INTO Cliente (`Nome`, `Documento`, `TipoDocumento`, `Telefone`, `Email`, `Endereco`, `Pontos`) VALUES (@Nome, @Documento, @TipoDocumento, @Telefone, @Email, @Endereco, @Pontos)";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@Documento", cliente.Documento);
            cmd.Parameters.AddWithValue("@TipoDocumento", cliente.TipoDocumento.ToString()); // Converte Enum para String pro MySQL
            cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@Pontos", cliente.Pontos);

            return Banco.ExecutarComando(cmd);
        }
    }

    public static int Deletar(Cliente cliente)
    {
        string cmdSql = "DELETE FROM `Cliente` WHERE `Id` = @Id";

        using (var cmd = new MySqlCommand(cmdSql))
        {
            cmd.Parameters.AddWithValue("@Id", cliente._id);

            return Banco.ExecutarComando(cmd);
        }
    }

    public static List<Cliente> ListarTodos()
    {
        string cmdSql = "SELECT * FROM `Cliente`";
        List<Cliente> listaClientes = new();

        using (var cmd = new MySqlCommand(cmdSql))
        {
            using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32("Id");
                    string nome = ValidarCampo(reader["Nome"]);
                    string documento = ValidarCampo(reader["Documento"]);

                    // Converte a string do ENUM do banco de volta para o Enum do C#
                    Enum.TryParse(reader["TipoDocumento"].ToString(), out TipoDocumento tipoDoc);

                    string telefone = ValidarCampo(reader["Telefone"]);
                    string email = ValidarCampo(reader["Email"]);
                    string endereco = ValidarCampo(reader["Endereco"]);
                    int pontos = reader.GetInt32("Pontos");

                    Cliente cliente = new(id, nome, documento, tipoDoc, telefone, email, endereco, pontos);
                    listaClientes.Add(cliente);
                }
            }

            return listaClientes;
        }
    }

    public static Cliente? ListarPorId(int id)
    {
        string sql = "SELECT * FROM `Cliente` WHERE `Id` = @Id";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Id", id);

            using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
            {
                Cliente? cliente = null;
                while (reader.Read())
                {
                    int idBanco = reader.GetInt32("Id");
                    string nome = ValidarCampo(reader["Nome"]);
                    string documento = ValidarCampo(reader["Documento"]);

                    Enum.TryParse(reader["TipoDocumento"].ToString(), out TipoDocumento tipoDoc);

                    string telefone = ValidarCampo(reader["Telefone"]);
                    string email = ValidarCampo(reader["Email"]);
                    string endereco = ValidarCampo(reader["Endereco"]);
                    int pontos = reader.GetInt32("Pontos");

                    cliente = new(idBanco, nome, documento, tipoDoc, telefone, email, endereco, pontos);
                }
                return cliente;
            }
        }
    }

    public static int AtualizarCliente(Cliente cliente)
    {
        string sql = "UPDATE `Cliente` SET `Nome` = @Nome, `Documento` = @Documento, `TipoDocumento` = @TipoDocumento, `Telefone` = @Telefone, `Email` = @Email, `Endereco` = @Endereco, `Pontos` = @Pontos WHERE `Id` = @Id";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@Documento", cliente.Documento);
            cmd.Parameters.AddWithValue("@TipoDocumento", cliente.TipoDocumento.ToString());
            cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
            cmd.Parameters.AddWithValue("@Email", cliente.Email);
            cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
            cmd.Parameters.AddWithValue("@Pontos", cliente.Pontos);
            cmd.Parameters.AddWithValue("@Id", cliente.Id);

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
