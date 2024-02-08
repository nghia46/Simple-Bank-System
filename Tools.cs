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
    public static string? ValidateString(string mgs,int inputLength)
    {
        string? input;
        do
        {
            Console.Write(mgs);
            input = Console.ReadLine();
            if (input?.Trim().Length < inputLength)
            {
                Console.WriteLine($"Input is too short. Please enter a string with at least {inputLength} characters:");
            }
        } while (input?.Trim().Length < inputLength);
        return input;
    }
    public static decimal ValidateDecimal(string message)
    {
        decimal amount;
        do
        {
            Console.Write(message);
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, out amount))
            {
                // Successful conversion, check if the amount is positive
                if (amount >= 0)
                {
                    return amount;
                }
                else
                {
                    Console.WriteLine("Amount cannot be negative. Please enter a non-negative number:");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid decimal number:");
            }
        } while (true);
    }
}