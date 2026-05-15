using System;
using System.Collections.Generic;
using System.Globalization;

public interface IPizza
{
    string Nome { get; }
    decimal GetPreco();
}

public class Mozarela : IPizza
{
    public string Nome => "mozarela";
    public decimal GetPreco() => 32.90m;
}

public class Calabresa : IPizza
{
    public string Nome => "calabresa";
    public decimal GetPreco() => 35.50m;
}

public class Portuguesa : IPizza
{
    public string Nome => "portuguesa";
    public decimal GetPreco() => 41.75m;
}

public class FrangoCatupiry : IPizza
{
    public string Nome => "frango catupiry";
    public decimal GetPreco() => 39.90m;
}

public class QuatroQueijos : IPizza
{
    public string Nome => "quatro queijos";
    public decimal GetPreco() => 43.60m;
}

public class Marguerita : IPizza
{
    public string Nome => "marguerita";
    public decimal GetPreco() => 36.40m;
}

public static class Pizzaria
{
    private static readonly Dictionary<string, Func<IPizza>> Fabrica = new(StringComparer.OrdinalIgnoreCase)
    {
        ["mozarela"] = () => new Mozarela(),
        ["calabresa"] = () => new Calabresa(),
        ["portuguesa"] = () => new Portuguesa(),
        ["frango catupiry"] = () => new FrangoCatupiry(),
        ["quatro queijos"] = () => new QuatroQueijos(),
        ["marguerita"] = () => new Marguerita()
    };

    public static IReadOnlyCollection<string> Sabores => Fabrica.Keys;

    public static IPizza FabricaPizza(string sabor)
    {
        if (string.IsNullOrWhiteSpace(sabor))
        {
            throw new ArgumentException("O sabor da pizza não pode ser vazio.", nameof(sabor));
        }

        var chave = sabor.Trim();

        if (Fabrica.TryGetValue(chave, out var criarPizza))
        {
            return criarPizza();
        }

        throw new ArgumentException($"'{sabor}' não existe no cardápio!");
    }
}

public static class Program
{
    public static void Main()
    {
        var soma = 0m;

        Console.WriteLine("=== Pizzaria (Factory em C#) ===");
        Console.WriteLine("Digite o nome da pizza e pressione Enter.");
        Console.WriteLine("Deixe vazio para finalizar o pedido.\n");
        Console.WriteLine("Sabores disponíveis:");

        foreach (var sabor in Pizzaria.Sabores)
        {
            Console.WriteLine($"- {sabor}");
        }

        Console.WriteLine();

        while (true)
        {
            Console.Write("Nome da pizza: ");
            var entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                break;
            }

            try
            {
                var pizza = Pizzaria.FabricaPizza(entrada);
                var preco = pizza.GetPreco();
                soma += preco;

                Console.WriteLine($"Pizza: {pizza.Nome,-16} | Preço: R$ {preco.ToString("F2", CultureInfo.GetCultureInfo("pt-BR"))}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        Console.WriteLine($"\nTotal a pagar: R$ {soma.ToString("F2", CultureInfo.GetCultureInfo("pt-BR"))}");
    }
}
