namespace AlunoNota;

class Program
{
    public static List<Aluno> alunos = null;

    static void Main(string[] args)
    {
        Console.Clear();
        //Console.Beep();

        Console.WriteLine("*************************************************************");
        Console.WriteLine("************ Sistema de alunos e notas **********************");
        Console.WriteLine("*************************************************************");

        alunos = new List<Aluno>();
        Aluno aluno;
        do
        {
            aluno = new Aluno();

            Console.Write("\nInforme o nome do aluno: ");
            aluno.Nome = Console.ReadLine();            

            PreencherNotas(aluno);

            alunos.Add(aluno);

            Console.WriteLine();

        } while (ValidarSimNao());

        Console.WriteLine("");
        Console.WriteLine("REPORT");
        Console.WriteLine("");
        Console.WriteLine("****" + nameof(AprovarReprovarAlunos));
        AprovarReprovarAlunos(alunos);

        Console.WriteLine("EscreveListaAlunos");

        //Calcular Nota Geral
        CalculoMediaGeral(alunos);

        EscreveListaAlunos(alunos);

        // Calcular menor nota
        CaluloMenorCR(alunos);

        Console.Write("Pressione uma tecla para sair");
        Console.ReadKey();
    }

    public static void PreencherNotas(Aluno aluno)
    {
        foreach (Disciplinas disciplina in (Disciplinas[])Enum.GetValues(typeof(Disciplinas)))
        {
            string notaTemp = string.Empty;
            float nota = 0f;
            do
            {
                Console.Write($"Informe a nota do {aluno.Nome} para a disciplina {disciplina}: ");

                notaTemp = Console.ReadLine();

            } while (float.TryParse(notaTemp, out nota) == false);

            aluno.NotaPorDisciplina.Add(disciplina, nota);
        }
    }

    private static bool ValidarSimNao()
    {
        bool respostaValida;
        char resposta;

        do
        {
            Console.Write("");
            Console.Write("Deseja cadastrar outro aluno? (S/N)");
            char entrada = Console.ReadKey().KeyChar;
            resposta = Char.ToUpper(entrada);

            if (resposta == 'S')
            {
                return true;
            }
            else if (resposta == 'N')
            {
                return false;
            }
            else
            {
                Console.WriteLine("\n\nEntrada inválida. Por favor, digite S ou N.");
            }

        } while (true); // O loop vai continuar até o return ser executado
    }

    private static void AprovarReprovarAlunos(List<Aluno> alunos)
    {
        Console.WriteLine("/n *** AprovarReprovarAlunos");


        foreach (var item in alunos)
        {
            var itemRecuperado = (item.NotaPorDisciplina.Sum(i => i.Value) / Enum.GetValues(typeof(Disciplinas)).Length);

            if (itemRecuperado > 7)
            {
                item.Status = Status.Aprovado;
            }
            else
            {
                item.Status = Status.Reprovado;
            }
        }

        Console.WriteLine("");
    }

    private static void EscreveListaAlunos(List<Aluno> alunos)
    {
        Console.WriteLine("/n *** EscreveListaAlunos");

        for (int i = 0; i < alunos.Count; i++)
        {
            string msg = $" **** \n{i + 1} º aluno(a): {alunos[i].Nome}" +
                $"\nstatus: {alunos[i].Status}" +
                "\nData da avaliação: " + alunos[i].Avaliacao + 
                $"\nnotas por disciplina";

            foreach (KeyValuePair<Disciplinas, float> keyValuePair in alunos[i].NotaPorDisciplina)
            {
                msg += $" \n  - {keyValuePair.Key}: {keyValuePair.Value} \n";
            }

            msg += $"media {alunos[i].MediaGeral} \n";

            Console.Write(msg);
        }

        Console.WriteLine("");
    }

    #region Métodos   

    private static void CaluloMenorCR(List<Aluno> alunos)
    {
        Aluno? alunoNotaMaxima = alunos.MinBy(p => p.MediaGeral);

        if (alunoNotaMaxima != null)
        {
            Console.WriteLine("A MENOR CR é do " + alunoNotaMaxima.Nome + " com a média " + alunoNotaMaxima.MediaGeral);
        }
        else
        {
            Console.WriteLine("ERR0043 - Não foi possível calcular verificar a menor nota");
        }
    }

    #endregion

    private static void CalculoMediaGeral(List<Aluno> alunos)
    {        
        foreach (var itemAluno in alunos)
        {
            float notaFinal = 0f;

            foreach (KeyValuePair<Disciplinas, float> itemNotaPorDisciplina in itemAluno.NotaPorDisciplina)
            {
                notaFinal += itemNotaPorDisciplina.Value;
            }

            itemAluno.MediaGeral = notaFinal / Enum.GetValues(typeof(Disciplinas)).Length;
        }
    }
}

//Objetos | Estado & Comportamento
//Estado = propriedades | variaveis
//Comportamento = métodos
public class Aluno
{
    public string Nome { get; set; }

    public char PrimeiraLetra { get; set; }

    private float _mediaGeral { get; set; }

    public float MediaGeral
    {
        get { return _mediaGeral; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Nota não pode ser menor que zero.");
            }
            _mediaGeral = value;
        }
    }

    public Status Status;

    public Dictionary<Disciplinas, float> NotaPorDisciplina { get; set; } = new();

    public bool Ativo { get; set; }

    public DateTime Avaliacao { get; set; }

    public void SetNome(string nome)
    {
        Nome = nome;
    }
}

[Flags]
public enum Status
{
    None = 0,
    Aprovado = 1,
    Reprovado = 2
}

public enum Disciplinas
{
    História,
    Matemática,
    Física,
    Geografia
}