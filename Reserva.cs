
using System; // <-
using System.Collections.Generic;
using System.Linq;

class Reserva
{
    public string NomeSolicitante { get; set; }
    public List<string> Equipamentos { get; set; } = new List<string>();
    public int HoraInicio { get; set; }
    public int HoraFim { get; set; }
}

class Program
{
    static void MostrarMenu()
    {
        Console.WriteLine("=== Sistema de Reserva de Equipamentos Audiovisuais ===");
        Console.WriteLine("1 - Realizar nova reserva");
        Console.WriteLine("2 - Ver reservas realizadas");
        Console.WriteLine("3 - Sair");
        Console.Write("Escolha uma opção: ");
    }

    static void ListarEquipamentos()
    {
        Console.WriteLine("\nEscolha os equipamentos separados por vírgula (ex: 1,3,7):");
        Console.WriteLine("1 - Datashow");
        Console.WriteLine("2 - TV com VCR");
        Console.WriteLine("3 - TV com DVD");
        Console.WriteLine("4 - Projetor de Slides");
        Console.WriteLine("5 - Sistema de Áudio/Microfone");
        Console.WriteLine("6 - Caixa Amplificada");
        Console.WriteLine("7 - Notebook");
        Console.WriteLine("8 - Kit Multimídia");
        Console.Write("Opções: ");
    }

    static string ObterEquipamento(int opcao)
    {
        switch (opcao)
        {
            case 1: return "Datashow";
            case 2: return "TV com VCR";
            case 3: return "TV com DVD";
            case 4: return "Projetor de Slides";
            case 5: return "Sistema de Áudio/Microfone";
            case 6: return "Caixa Amplificada";
            case 7: return "Notebook";
            case 8: return "Kit Multimídia";
            default: return "";
        }
    }

    static bool VerificarConflito(List<Reserva> reservas, Reserva novaReserva)
    {
        bool conflitoEncontrado = false;

        foreach (Reserva r in reservas)
        {
            bool horarioConflitante = !(novaReserva.HoraFim <= r.HoraInicio || novaReserva.HoraInicio >= r.HoraFim);
            if (horarioConflitante)
            {
                foreach (string eqNovo in novaReserva.Equipamentos)
                {
                    foreach (string eqExistente in r.Equipamentos)
                    {
                        if (eqNovo == eqExistente)
                        {
                            conflitoEncontrado = true;
                            Console.WriteLine("\n⚠ Equipamento já reservado no mesmo horário!");
                            Console.WriteLine("📦 Equipamento: " + eqNovo);
                            Console.WriteLine("⏰ Horário reservado: " + r.HoraInicio + "h às " + r.HoraFim + "h");
                            Console.WriteLine("👤 Reservado por: " + r.NomeSolicitante);
                        }
                    }
                }
            }
        }

        return conflitoEncontrado;
    }

    static void RealizarReserva(List<Reserva> reservas)
    {
        Reserva nova = new Reserva();

        Console.Write("\nNome do solicitante: ");
        nova.NomeSolicitante = Console.ReadLine();

        ListarEquipamentos();
        string entradaEquipamentos = Console.ReadLine();
        string[] codigosStr = entradaEquipamentos.Split(',');
        foreach (string codStr in codigosStr)
        {
            int codigo;
            if (int.TryParse(codStr.Trim(), out codigo))
            {
                string nomeEquip = ObterEquipamento(codigo);
                if (nomeEquip != "")
                {
                    nova.Equipamentos.Add(nomeEquip);
                }
            }
        }

        Console.Write("Informe a hora de início (7-16): ");
        nova.HoraInicio = int.Parse(Console.ReadLine());

        Console.Write("Informe a hora de término (máximo 2 horas depois): ");
        nova.HoraFim = int.Parse(Console.ReadLine());

        int duracao = nova.HoraFim - nova.HoraInicio;

        if (nova.HoraInicio < 7 || nova.HoraFim > 16 || duracao <= 0 || duracao > 2)
        {
            Console.WriteLine("⚠ Tempo inválido! Certifique-se de informar horários entre 7h e 16h e com duração máxima de 2 horas.\n");
            return;
        }

        if (VerificarConflito(reservas, nova))
        {
            Console.WriteLine("⚠ Não foi possível concluir a reserva devido a conflito de equipamento/horário.");
            return;
        }

        reservas.Add(nova);
        Console.WriteLine("\n✅ Sua reserva foi realizada com sucesso, " + nova.NomeSolicitante + ".");
        Console.WriteLine("⏰ Sua reserva será das " + nova.HoraInicio + "h às " + nova.HoraFim + "h com os seguintes equipamentos:");
        foreach (string eq in nova.Equipamentos)
        {
            Console.WriteLine("- " + eq);
        }
        Console.WriteLine();
    }

    static void MostrarReservas(List<Reserva> reservas)
    {
        Console.WriteLine("\n=== Reservas Realizadas ===");
        if (reservas.Count == 0)
        {
            Console.WriteLine("Nenhuma reserva encontrada.");
        }
        else
        {
            for (int i = 0; i < reservas.Count; i++)
            {
                Reserva r = reservas[i];
                Console.WriteLine((i + 1) + ". Solicitante: " + r.NomeSolicitante + " | Horário: " + r.HoraInicio + "h às " + r.HoraFim + "h | Equipamentos: " + string.Join(", ", r.Equipamentos));
            }
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        List<Reserva> reservas = new List<Reserva>();
        bool executando = true;

        while (executando)
        {
            MostrarMenu();
            string entrada = Console.ReadLine();
            int opcao;
            if (int.TryParse(entrada, out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        RealizarReserva(reservas);
                        break;
                    case 2:
                        MostrarReservas(reservas);
                        break;
                    case 3:
                        executando = false;
                        Console.WriteLine("Saindo do programa...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida.");
            }
        }
    }
}
