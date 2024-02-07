using bankingSystem.Service;

namespace bankingSystem;

public class Menu(Account.AccountDto? accountDto)
{
    public void LoadMainMenu()
    {
        int choice = 0;
        do
        {
            Console.WriteLine($"Welcome ~{accountDto?.Username}~ to the Simple Banking System!");
            if (accountDto?.Password != null)
            {
                Console.WriteLine(
                    $"Balance: {AccountService.GetAccount(accountDto?.Username, accountDto?.Password)?.Balance}$");
                Console.WriteLine(
                    $"Number: {AccountService.GetAccount(accountDto?.Username, accountDto?.Password)?.BankNumber}");
                Console.WriteLine("1. Deposit money");
                Console.WriteLine("2. Withdraw money");
                Console.WriteLine("3. Transfer money");
                Console.WriteLine("4. Check account balance");
                Console.WriteLine("5. Exit\n");
                Console.Write("Please enter your choice: ");
                // Read user input and attempt to parse it as an integer
                if (int.TryParse(Console.ReadLine(), out choice))
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter the amount: ");
                            var depositAmount = Convert.ToDecimal(Console.ReadLine());
                            var bankNumber = AccountService.GetAccount(accountDto?.Username, accountDto?.Password)
                                ?.BankNumber;
                            if (bankNumber != null)
                                AccountService.Deposit(
                                    bankNumber,
                                    depositAmount);
                            break;
                        case 2:
                            Console.Write("Enter the amount: ");
                            var amount = Convert.ToDecimal(Console.ReadLine());
                            if (accountDto != null)
                            {
                                var accountNumber = AccountService.GetAccount(accountDto.Username, accountDto.Password)
                                    ?.BankNumber;
                                if (accountNumber != null)
                                    AccountService.Withdraw(
                                        accountNumber,
                                        amount);
                            }

                            break;
                        case 3:
                            Console.Write("Enter bank number destination: ");
                            var destinationBankNumber = Console.ReadLine();
                            if (AccountService.FindBankNumberUserName(destinationBankNumber) != null)
                            {
                                Console.WriteLine(
                                    $"Destination account info:\nName: {AccountService.FindBankNumberUserName(destinationBankNumber)}\nBank number: {destinationBankNumber}");
                                Console.Write("Enter amount to transfer: ");
                                var transferAmount = Convert.ToDecimal(Console.ReadLine());
                                if (AccountService.Transfer(
                                        AccountService.GetAccount(accountDto?.Username, accountDto?.Password)?.BankNumber,
                                        destinationBankNumber, transferAmount))
                                {
                                }
                            }
                            else
                            {
                                Console.WriteLine("User with Bank number are not found!");
                            }

                            break;
                        case 4:
                            Console.WriteLine("You account remain: " +
                                              AccountService.GetAccount(accountDto?.Username, accountDto?.Password)
                                                  ?.Balance +
                                              "$");
                            break;
                    }
                else
                    Console.WriteLine("Invalid input. Please enter a valid integer choice.");
            }
        } while (choice != 5);
    }
}