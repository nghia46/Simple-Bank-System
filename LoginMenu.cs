using bankingSystem.Service;

namespace bankingSystem;

public class LoginMenu
{
    public void LoadLoginMenu()
    {
        int choice;
        do
        {
            Console.WriteLine("\nWelcome to the Simple Banking System!");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Create account");
            Console.WriteLine("3. Exit");
            Console.Write("Please enter your choice: ");

            // Read user input and attempt to parse it as an integer
            if (int.TryParse(Console.ReadLine(), out choice))
                switch (choice)
                {
                    case 1:
                        var usernameLogin = Tools.ValidateString("Username: ",0);
                        var passwordLogin = Tools.ValidateString("Password: ",0);
                        if (AccountService.Login(usernameLogin, passwordLogin))
                        {
                            var mainMenu = new Menu(AccountService.GetAccount(usernameLogin, passwordLogin));
                            mainMenu.LoadMainMenu();
                        }
                        break;  
                    case 2:
                        do
                        {
                            var username = Tools.ValidateString("Username: ",8);
                            var password = Tools.ValidateString("Password: ",8);
                            if (AccountService.CreateAccount(username, password))
                                break;
                        } while (true);
                        break;
                    case 3:
                        Console.WriteLine("Exiting the application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            else
                Console.WriteLine("Invalid input. Please enter a valid integer choice.");
        } while (choice != 3);
    }
}