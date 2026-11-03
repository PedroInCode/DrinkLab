using DrinkLab.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkLab.Model;

internal class UsuarioAuth
{
    // Retorna o ID do usuário se o login e a senha baterem, ou 0 se falhar
    public static int Autenticar(string loginDigitado, string senhaDigitada)
    {
        string sql = "SELECT Id, Senha FROM Usuarios WHERE Login = @Login";

        using (var cmd = new MySqlCommand(sql))
        {
            cmd.Parameters.AddWithValue("@Login", loginDigitado);

            using (var reader = Banco.ExecutarComRetornoDeDados(cmd))
            {
                if (reader.Read())
                {
                    int usuarioId = reader.GetInt32("Id");
                    string senhaHashBanco = reader.GetString("Senha");

                    // Valida a senha digitada com o hash BCrypt salvo no banco
                    bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senhaDigitada, senhaHashBanco);

                    if (senhaCorreta)
                    {
                        return usuarioId; // Login bem-sucedido!
                    }
                }
            }
        }
        return 0; // Falha no login
    }
}
