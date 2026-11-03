
using DrinkLab.Model;
using DrinkLab.Model.Enums;

namespace DrinkLab.Service;

public class FuncionarioService
{
    public static void MenuFuncionario()
    {
        bool sair = false;
        while (!sair)
        {
            Console.Clear();
            Console.WriteLine("===== GESTÃO DE FUNCIONARIOS ===== ");
            Console.WriteLine("1 - Cadastrar Funcionario");
            Console.WriteLine("2 - Listar Todos os Funcionarios");
            Console.WriteLine("3 - Buscar Funcionario por ID");
            Console.WriteLine("4 - Atualizar Funcionario");
            Console.WriteLine("5 - Excluir Funcionario");
            Console.WriteLine("0 - Voltar ao Menu Principal");

            Console.WriteLine(" =============================");
            Console.Write("Digite uma opção: ");

            string opcao = Console.ReadLine()!;
            Console.Clear();

            try
            {
                switch (opcao)
                {
                    case "1":
                        CadastrarFuncionario(); break;
                    case "2":
                        ListarFuncionarios(); break;
                    case "3":
                        ListarFuncionarioPorId(); break;
                    case "4":
                        AtualizarFuncionario(); break;
                    case "5":
                        DeletarFuncionario(); break;
                    case "0":
                        sair = true;
                        Console.WriteLine("Retornando ao menu principal..."); break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        PressionarParaTentarNovamente();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                PressionarParaTentarNovamente();
            }
        }
    }

    private static void CadastrarFuncionario()
    {
        Console.WriteLine("-=-=-=-=-=- CADASTRAR FUNCIONÁRIO -=-=-=-=-=-");

        // Credenciais de Acesso (Tabela Usuarios com BCrypt)
        Console.Write("Digite o Login de acesso ao sistema: ");
        string login = Console.ReadLine()!;
        Console.Write("Digite a senha de acesso: ");
        string senha = Console.ReadLine()!;

        // Insere o usuário e pega o ID gerado
        Usuario novoUsuario = new Usuario(login, senha);
        int usuarioId = Usuario.Inserir(novoUsuario);

        if (usuarioId <= 0)
        {
            Console.WriteLine("\n[ERRO] Falha ao criar o usuário de acesso do funcionário!");
            VoltarParaOMenu();
            return;
        }

        // Dados Pessoais e Profissionais
        Console.Write("Digite o nome do Funcionário: ");
        string nomeFuncionario = Console.ReadLine()!;

        Console.WriteLine("Escolha o Tipo de Documento: 1 - CPF | 2 - CNPJ");
        string opDoc = Console.ReadLine()!;
        TipoDocumento tipoDoc = (opDoc == "2") ? TipoDocumento.CNPJ : TipoDocumento.CPF;

        Console.Write("Digite o número do documento: ");
        string documentoFuncionario = Console.ReadLine()!;

        Console.Write("Digite o telefone: ");
        string telefone = Console.ReadLine()!;

        Console.Write("Digite o e-mail: ");
        string email = Console.ReadLine()!;

        Console.WriteLine("Escolha o Cargo: 1 - Gerente | 2 - Caixa | 3 - Motorista");
        string opCargo = Console.ReadLine()!;
        TipoCargo cargo = TipoCargo.Caixa;
        if (opCargo == "1") cargo = TipoCargo.Gerente;
        else if (opCargo == "3") cargo = TipoCargo.Motorista;

        Console.Write("Digite o salário (ex: 2500.00): ");
        decimal salario = decimal.Parse(Console.ReadLine()!);

        DateTime dataAdmissao = DateTime.Now;

        Funcionario novoFuncionario = new Funcionario(
            nome: nomeFuncionario,
            documento: documentoFuncionario,
            tipoDocumento: tipoDoc,
            telefone: telefone,
            email: email,
            tipoCargo: cargo,
            salario: salario,
            dataAdmissao: dataAdmissao,
            usuarioId: usuarioId
        );

        int resultadoInsert = Funcionario.Inserir(novoFuncionario);
        Console.Clear();

        if (resultadoInsert > 0)
            Console.WriteLine("Funcionário cadastrado com sucesso com senha protegida por BCrypt!");
        else
            Console.WriteLine("Erro ao cadastrar o funcionário no banco de dados.");

        VoltarParaOMenu();
    }

    private static void ListarFuncionarios()
    {
        Console.WriteLine("------ LISTA DE FUNCIONARIOS ------");

        List<Funcionario> lista = Funcionario.ListarTodosFuncionarios();

        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum funcionário cadastrado no sistema!");
        }
        else
        {
            foreach (var f in lista)
            {
                Console.WriteLine($"ID: {f.Id} | Nome: {f.Nome} | Cargo: {f.TipoCargo} | Salário: R$ {f.Salario} | Status: {(f.Ativo ? "Ativo" : "Inativo")}");
            }
        }

        VoltarParaOMenu();
    }

    private static void ListarFuncionarioPorId()
    {
        int idFunc;
        Funcionario? funcionario = null;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("------ BUSCAR FUNCIONARIO POR ID ------");
            Console.Write("\nDigite o [ID] (ou 0 para sair): ");

            if (!int.TryParse(Console.ReadLine(), out idFunc))
            {
                Console.WriteLine("\n[ERRO] ID inválido! Digite apenas números.");
                PressionarParaTentarNovamente();
                continue;
            }

            if (idFunc == 0)
            {
                VoltarParaOMenu();
                return;
            }

            funcionario = Funcionario.ListarPorId(idFunc);

            if (funcionario == null)
            {
                Console.WriteLine("\n[ERRO] Nenhum funcionário encontrado com esse ID no sistema!");
                PressionarParaTentarNovamente();
                continue;
            }

            Console.Clear();
            Console.WriteLine("------ FUNCIONÁRIO ENCONTRADO ------");
            Console.WriteLine($"ID: {funcionario.Id}");
            Console.WriteLine($"Nome: {funcionario.Nome}");
            Console.WriteLine($"Tipo Doc: {funcionario.TipoDocumento} - Doc: {funcionario.Documento}");
            Console.WriteLine($"Telefone: {funcionario.Telefone} | E-mail: {funcionario.Email}");
            Console.WriteLine($"Cargo: {funcionario.TipoCargo}");
            Console.WriteLine($"Salário: R$ {funcionario.Salario}");
            Console.WriteLine($"Data de Admissão: {funcionario.DataAdmissao:dd/MM/yyyy}");
            Console.WriteLine($"Ativo: {(funcionario.Ativo ? "Sim" : "Não")}");

            Console.WriteLine("\n--------------------------------");
            Console.WriteLine("1 - Buscar outro funcionário");
            Console.WriteLine("0 - Voltar ao menu de funcionários");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine()!;

            if (opcao == "0")
            {
                VoltarParaOMenu();
                return;
            }
        }
    }

    private static void AtualizarFuncionario()
    {
        int idFunc;
        Funcionario? funcionario = null;

        while (funcionario == null)
        {
            Console.Clear();
            Console.WriteLine("------ ATUALIZAR FUNCIONARIO ------");
            Console.Write("Digite o [ID] (ou 0 para sair): ");

            if (!int.TryParse(Console.ReadLine(), out idFunc))
            {
                Console.WriteLine("\n[ERRO] ID inválido! Digite apenas números.");
                PressionarParaTentarNovamente();
                continue;
            }

            if (idFunc == 0)
            {
                VoltarParaOMenu();
                return;
            }

            funcionario = Funcionario.ListarPorId(idFunc);

            if (funcionario == null)
            {
                Console.WriteLine("\n[ERRO] Nenhum funcionário encontrado com esse ID no sistema!");
                PressionarParaTentarNovamente();
            }
        }

        Console.Clear();
        Console.WriteLine($"Funcionário Selecionado: {funcionario.Nome} (ID: {funcionario.Id})");
        Console.WriteLine("\n-- O QUE VOCÊ DESEJA ALTERAR? --");
        Console.WriteLine("1 - Nome");
        Console.WriteLine("2 - Documento e Tipo");
        Console.WriteLine("3 - Cargo");
        Console.WriteLine("4 - Salário");
        Console.WriteLine("5 - Telefone e E-mail");
        Console.WriteLine("0 - Cancelar");
        Console.Write("Escolha uma opção: ");

        string opcao = Console.ReadLine()!;
        bool alterado = true;

        switch (opcao)
        {
            case "1":
                Console.Write("Novo Nome: ");
                funcionario.Nome = Console.ReadLine()!;
                break;

            case "2":
                Console.WriteLine("Tipo de Documento: 1 - CPF | 2 - CNPJ");
                string tipoDoc = Console.ReadLine()!;
                funcionario.TipoDocumento = (tipoDoc == "2") ? TipoDocumento.CNPJ : TipoDocumento.CPF;
                Console.Write("Novo Número do Documento: ");
                funcionario.Documento = Console.ReadLine()!;
                break;

            case "3":
                Console.WriteLine("Novo Cargo: 1 - Gerente | 2 - Caixa | 3 - Motorista");
                string cargoFuncionario = Console.ReadLine()!;
                if (cargoFuncionario == "1") funcionario.TipoCargo = TipoCargo.Gerente;
                else if (cargoFuncionario == "3") funcionario.TipoCargo = TipoCargo.Motorista;
                else funcionario.TipoCargo = TipoCargo.Caixa;
                break;

            case "4":
                Console.Write("Novo Salário: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal novoSalario))
                {
                    funcionario.Salario = novoSalario;
                }
                else
                {
                    Console.WriteLine("Salário inválido!");
                    alterado = false;
                }
                break;

            case "5":
                Console.Write("Novo Telefone: ");
                funcionario.Telefone = Console.ReadLine()!;
                Console.Write("Novo E-mail: ");
                funcionario.Email = Console.ReadLine()!;
                break;

            case "0":
                Console.WriteLine("Operação cancelada.");
                alterado = false;
                break;

            default:
                Console.WriteLine("Opção inválida!");
                alterado = false;
                break;
        }

        if (alterado)
        {
            int resultado = Funcionario.AtualizarFuncionario(funcionario);

            if (resultado > 0)
                Console.WriteLine("\nFuncionário atualizado com sucesso!");
            else
                Console.WriteLine("\nErro ao atualizar o funcionário no banco de dados.");
        }

        VoltarParaOMenu();
    }

    private static void DeletarFuncionario()
    {
        int idFunc;
        Funcionario? funcionario = null;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("------ EXCLUINDO FUNCIONARIO ------");
            Console.Write("\nDigite o [ID] (ou 0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out idFunc))
            {
                Console.WriteLine("\n[ERRO] ID inválido! Digite apenas números.");
                PressionarParaTentarNovamente();
                continue;
            }

            if (idFunc == 0)
            {
                VoltarParaOMenu();
                return;
            }

            funcionario = Funcionario.ListarPorId(idFunc);

            if (funcionario == null)
            {
                Console.WriteLine("\n[ERRO] Nenhum funcionário encontrado no sistema!");
                PressionarParaTentarNovamente();
                continue;
            }

            Console.WriteLine($"\nFuncionário encontrado: ID: {funcionario.Id} | Nome: {funcionario.Nome}");
            Console.Write("\nTem certeza que deseja excluir este funcionário? (S/N): ");
            string confirmacao = Console.ReadLine()!.Trim().ToUpper();

            if (confirmacao == "S")
            {
                Funcionario.Deletar(funcionario);
                Console.WriteLine("\nFuncionário excluído com sucesso!");
                break;
            }
            else
            {
                Console.WriteLine("\nExclusão cancelada pelo usuário.");
                PressionarParaTentarNovamente();
            }
        }

        VoltarParaOMenu();
    }

    private static void VoltarParaOMenu()
    {
        Console.Write("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }

    private static void PressionarParaTentarNovamente()
    {
        Console.Write("\nPressione qualquer tecla para tentar novamente...");
        Console.ReadKey();
    }
}
