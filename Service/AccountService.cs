using Newtonsoft.Json;

namespace bankingSystem.Service;

public static class AccountService
{
    private static List<Account.AccountDto>? _accounts = new();

    public static Account.AccountDto? GetAccount(string? username, string? password)
    {
        LoadAccountsFromDatabase();
        if (_accounts != null)
        {
            var account = _accounts.FirstOrDefault(acc => acc.Username == username && acc.Password == password);
            if (account != null)
                return new Account.AccountDto(account.BankNumber, account.Username, account.Password, account.Balance);
        }

        Console.WriteLine("You don't have permission to access to this Get out now!");
        return null;
    }

    public static bool Deposit(string bankNumber, decimal amount)
    {
        LoadAccountsFromDatabase();
        if (_accounts != null)
        {
            var account = _accounts.FirstOrDefault(acc => acc.BankNumber == bankNumber);
            if (account != null)
            {
                Console.WriteLine($"Successfully deposit : +{amount}$");
                var updatedAccount = account with { Balance = account.Balance + amount };
                var index = _accounts.IndexOf(account);
                _accounts[index] = updatedAccount;
                SaveAccountsToDatabase();
                return true;
            }
        }
        return false;
    }

    public static bool Withdraw(string accountNumber, decimal amount)
    {
        LoadAccountsFromDatabase();
        if (_accounts != null)
        {
            var account = _accounts.FirstOrDefault(acc => acc.BankNumber == accountNumber);
            if (account != null)
            {
                // Check if the account has sufficient balance for the withdrawal
                if (account.Balance >= amount)
                {
                    Console.WriteLine($"Successfully withdrawn -{amount}$");

                    // Create a new AccountDto with the updated balance after withdrawal
                    var updatedAccount = account with { Balance = account.Balance - amount };

                    // Update the account in the list with the new balance
                    var index = _accounts.IndexOf(account);
                    _accounts[index] = updatedAccount;

                    // Save the updated accounts to the database
                    SaveAccountsToDatabase();

                    return true;
                }

                Console.WriteLine("You don't have enough balance!");
                return false;
            }
        }

        Console.WriteLine("Account not found");
        return false;
    }


    public static bool Login(string? username, string? password)
    {
        LoadAccountsFromDatabase();
        if (_accounts != null && _accounts.Exists(acc => acc.Username == username && acc.Password == password))
        {
            Console.WriteLine("Login successful");
            return true;
        }

        Console.WriteLine("Login failed");
        return false;
    }

    public static bool CreateAccount(string? username, string? password)
    {
        LoadAccountsFromDatabase();
        // Check if the username already exists
        if (_accounts != null && _accounts.Exists(acc => acc.Username == username))
        {
            Console.WriteLine("Username already exists. Please choose a different one.");
            return false;
        }

        // Create a new account with a random GUID

        if (username != null)
        {
            if (password != null)
            {
                var newAccount = new Account.AccountDto(Tools.RandomAccountNumber(), username, password, 0);

                // Add the new account to the list of accounts
                _accounts?.Add(newAccount);
            }
        }


        // Save the updated list of accounts to the database
        if (SaveAccountsToDatabase())
        {
            Console.WriteLine("Account created successfully!");
            return true;
        }

        Console.WriteLine("Failed to create account!");
        return false;
    }

    private static bool SaveAccountsToDatabase()
    {
        try
        {
            // Get the base directory where the application is running
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the base directory with the file name
            var filePath = Path.Combine(baseDirectory, "bank-accounts.json");

            // Serialize the list of accounts to JSON
            var json = JsonConvert.SerializeObject(_accounts, Formatting.Indented);

            // Write the JSON string to the file
            File.WriteAllText(filePath, json);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving accounts: {ex.Message}");
            return false;
        }
    }

    private static void LoadAccountsFromDatabase()
    {
        try
        {
            // Get the base directory where the application is running
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Combine the base directory with the file name
            var filePath = Path.Combine(baseDirectory, "bank-accounts.json");

            // Check if the JSON file exists
            if (File.Exists(filePath))
            {
                // Read the JSON string from the file
                var json = File.ReadAllText(filePath);

                // Deserialize the JSON string to a list of accounts
                _accounts = JsonConvert.DeserializeObject<List<Account.AccountDto>>(json);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading accounts: {ex.Message}");
        }
    }

    public static string? FindBankNumberUserName(string? bankNumber)
    {
        LoadAccountsFromDatabase();
        if (_accounts != null)
        {
            var account = _accounts.FirstOrDefault(acc => acc.BankNumber == bankNumber);
            if (account == null) return null;
            return account.Username;
        }else
        {
            System.Console.WriteLine($"Account with {bankNumber} not found!");
        }

        return null;
    }

    internal static bool Transfer(string? baseNumber, string? destinationBankNumber, decimal transferAmount)
    {
        LoadAccountsFromDatabase();

        // Logic for Base Account
        if (_accounts != null)
        {
            var baseAccount = _accounts.FirstOrDefault(acc => acc.BankNumber == baseNumber);
            if (baseAccount == null) return false;

            if (baseAccount.Balance >= transferAmount)
            {
                var baseAccountAfterWithdrawal = baseAccount with { Balance = baseAccount.Balance - transferAmount };
                var baseAccountIndex = _accounts.IndexOf(baseAccount);
                _accounts[baseAccountIndex] = baseAccountAfterWithdrawal;
            }
            else
            {
                Console.WriteLine("Insufficient balance for transfer!");
                return false;
            }
        }

        // Logic for Destination Account
        if (_accounts != null)
        {
            var destinationAccount = _accounts.FirstOrDefault(acc => acc.BankNumber == destinationBankNumber);
            if (destinationAccount == null) return false;

            var destinationAccountAfterDeposit =
                destinationAccount with { Balance = destinationAccount.Balance + transferAmount };
            var destinationAccountIndex = _accounts.IndexOf(destinationAccount);
            _accounts[destinationAccountIndex] = destinationAccountAfterDeposit;
        }else
        {
            Console.WriteLine("The destination account not found!");
        }

        SaveAccountsToDatabase();
        System.Console.WriteLine($"Successfully transfers -{transferAmount}$");
        return true;
    }
}