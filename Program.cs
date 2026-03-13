using System;
using OnlineBankingAppService;
using OnlineBankingModels;

namespace OnlineBanking_Act1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("ONLINE BANKING");

            OnlineBankAppService appService = new OnlineBankAppService();
            bool isContinue = true;

            Console.Write("ENTER ACCOUNT NUMBER: ");
            int UserAccountNum = Convert.ToInt32(Console.ReadLine());

            do
            {

                bool authenticated = false;

                for (int i = 0; i < 3; i++)
                {
                    Console.Write("ENTER 4-DIGIT CODE: ");
                    int UserPin = Convert.ToInt32(Console.ReadLine());

                    authenticated = appService.Authenticate(UserAccountNum, UserPin);

                    if (authenticated)
                    {
                        Console.WriteLine("Login Successful!");
                        Choices(appService, UserAccountNum); break;
                    }
                    else
                    {
                        Console.WriteLine("You only have " + (2 - i) + " tries left. Incorrect MPIN entered.");
                    }
                }

                Console.Write("Do you want to continue? [Y/N]: ");
                string continueInput = Console.ReadLine();

                if (continueInput.ToUpper() == "Y")
                {
                    isContinue = true;
                }
                else if (continueInput.ToUpper() == "N")
                {
                    Console.WriteLine("Thank you for using our service. Have a nice day!");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid input. System will exit.");
                    Environment.Exit(0);
                }
            } while (isContinue);

            static void Choices(OnlineBankAppService appService, int accountNumber)
            {
                Console.WriteLine("Welcome! What do you want to do today? \n" +
                              "1. BALANCE \n" +
                              "2. DEPOSIT \n" +
                              "3. WITHDRAW \n" +
                              "OTHER OPTIONS ON THE WAY!");

                Console.Write("PLEASE SELECT AN OPTION: ");
                int MenuInput = Convert.ToInt32(Console.ReadLine());

                switch (MenuInput)
                {
                    case 1:
                        Console.WriteLine("Your Balance is: PHP " + appService.GetBalance(accountNumber)); //RETRIEVE
                        break;
                    case 2: // CASH-IN
                        Console.Write("\n DEPOSIT CHOICES: \n" +
                                      "1. BANK CASH-IN [BCI]\n" +
                                      "2. OVER-THE-COUNTER CASH-IN [OTC]\n" +
                                      "PLEASE SELECT AN OPTION [BCI|OTC]: ");
                        string DepositInput = Console.ReadLine().ToUpper();

                        Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP");
                        double deposit = Convert.ToDouble(Console.ReadLine());

                        appService.Deposit(accountNumber, DepositInput, deposit);

                        break;
                        //case 3: //UPDATE
                        //    appService.Withdraw(accountNumber); // CASH-OUT
                        //    break;
                        //default:
                        //    Console.WriteLine("Invalid input. System will exit.");
                        //    break;
                }
            }
        }

    }
}
