using DrinkLab.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrinkLab.Service;

internal class AuthService
{
    public static bool RealizarLogin()
    {
        Console.Clear();
        Console.WriteLine("===== AUTENTICAÇÃO - ADEGA DRINKLAB =====");
        Console.Write("Login: ");
        string login = Console.ReadLine()!;

        Console.Write("Senha: ");
        string senha = LerSenhaOculta();

        int usuarioId = UsuarioAuth.Autenticar(login, senha);

        if (usuarioId > 0)
        {
            Console.WriteLine("\n\nLogin efetuado com sucesso! Pressione qualquer tecla para entrar...");
            Console.ReadKey();
            return true;
        }
        else
        {
            Console.WriteLine("\n\nLogin ou senha incorretos! Pressione qualquer tecla para tentar novamente...");
            Console.ReadKey();
            return false;
        }
    }

    private static string LerSenhaOculta()
    {
        string senha = "";
        while (true)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(true);

            if (tecla.Key == ConsoleKey.Enter) break;
            else if (tecla.Key == ConsoleKey.Backspace)
            {
                if (senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
            }
            else if (tecla.KeyChar != '\u0000')
            {
                senha += tecla.KeyChar;
                Console.Write("*");
            }
        }
        return senha;
    }
}
