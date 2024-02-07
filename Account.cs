namespace bankingSystem;

public class Account
{
    public record AccountDto(string BankNumber, string? Username, string? Password, decimal Balance);
}