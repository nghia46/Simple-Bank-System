namespace bankingSystem;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        LoginMenu menu = new();
        menu.LoadLoginMenu();
        Console.ReadKey();
    }
}