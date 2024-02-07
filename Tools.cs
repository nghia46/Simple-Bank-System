namespace bankingSystem;

public static class Tools
{
    private static readonly Random random = new();

    public static string RandomAccountNumber()
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}