using bankingSystem.Service;

namespace bankingSystem;

public class Menu(Account.AccountDto? accountDto)
{
    public void LoadMainMenu()
    {
        int choice = 0;
        do
        {
            Console.WriteLine($"\nWelcome ~{accountDto?.Username}~ to the Simple Banking System!");
            if (accountDto?.Password != null)
            {
                Console.WriteLine(
                    $"Balance: {AccountService.GetAccount(accountDto?.Username, accountDto?.Password)?.Balance}$");
                Console.WriteLine(
                    $"Number: {AccountService.GetAccount(accountDto?.Username, accountDto?.Password)?.BankNumber}");
                System.Console.WriteLine("~~~~~~~~Menu~~~~~~~~");
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
                            var depositAmount = Tools.ValidateDecimal("Enter the amount: ");
                            var bankNumber = AccountService.GetAccount(accountDto?.Username, accountDto?.Password)
                                ?.BankNumber;
                            if (bankNumber != null)
                                AccountService.Deposit(
                                    bankNumber,
                                    depositAmount);
                            break;
                        case 2:
                            var amount = Tools.ValidateDecimal("Enter the amount: ");
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
                            var destinationBankNumber = Tools.ValidateString("Enter bank number destination: ", 12);
                            if (AccountService.FindBankNumberUserName(destinationBankNumber) != null)
                            {
                                Console.WriteLine(
                                    $"Destination account info:\nName: {AccountService.FindBankNumberUserName(destinationBankNumber)}\nBank number: {destinationBankNumber}");
                                var transferAmount = Tools.ValidateDecimal("Enter amount to transfer: ");

                                Console.WriteLine($"Do you want to transfer {transferAmount} to {AccountService.FindBankNumberUserName(destinationBankNumber)}?");
                                System.Console.WriteLine("1. Yes");
                                System.Console.WriteLine("2. No");
                                Console.Write("Please enter your choice: ");
                                int transferConfirm = Convert.ToInt16(Console.ReadLine());
                                if (transferConfirm != 1)
                                {
                                    System.Console.WriteLine("Canceled transfer!");
                                }
                                else if (AccountService.Transfer(
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