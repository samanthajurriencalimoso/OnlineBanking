using System;
using OnlineBankingAppService;
using OnlineBankingDataModel;

namespace OnlineBanking_Act1
{
    internal class Program
    {
        static OnlineBankAppService appService = new OnlineBankAppService();
        static int UserAccountNum;
        static void Main(string[] args)
        {
            Console.WriteLine("ONLINE BANKING");
            Start();
        }

        static void Start()
        {
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
                        Console.WriteLine("\nLogin Successful!");
                        Choices(UserAccountNum); break;
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
        }
            static void Choices(int accountNumber)
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
                    case 1: //RETRIEVE
                    Console.WriteLine("Your Balance is: PHP " + appService.GetBalance(accountNumber)); 
                        break;
                    case 2: // CASH-IN
                        Console.Write("\n DEPOSIT CHOICES: \n" +
                                      "1. BANK CASH-IN [BCI]\n" +
                                      "2. OVER-THE-COUNTER CASH-IN [OTC]\n" +
                                      "PLEASE SELECT AN OPTION [BCI|OTC]: ");
                        string SectionInput = Console.ReadLine().ToUpper();

                        string BankInput = "";

                    switch (SectionInput)
                    {
                        case "BCI":
                            Console.Write("\nBANK CASH-IN OPTIONS: \n" +
                                              "1. BPI \n" +
                                              "2. BDO \n" +
                                              "3. LANDBANK \n" +
                                              "ENTER BANK CASH-IN [BCI] BANK: ");
                            BankInput = Console.ReadLine().ToUpper();
                            break;
                        case "OTC":
                            Console.Write("\nBANK OVER-THE-COUNTER OPTIONS: \n" +
                                              "1. ROBINSONS \n" +
                                              "2. HANDYMAN \n" +
                                              "3. 7-ELEVEN \n" +
                                              "ENTER OVER-THE-COUNTER CASH-IN [OTC] BANK: ");
                            BankInput = Console.ReadLine().ToUpper();
                            break;
                        case "PO":
                            Console.Write("\nPARTNER OUTLET OPTIONS: \n" +
                                              "1. 7-ELEVEN \n" +
                                              "2. SM \n" +
                                              "3. PUREGOLD \n" +
                                              "ENTER PARTNER OUTLET CASH-OUT: ");
                            BankInput = Console.ReadLine().ToUpper();
                            break;

                        default:
                            Console.WriteLine("Invalid deposit option.");
                            return;
                    }

                    Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP ");
                    double amount = Convert.ToDouble(Console.ReadLine());

                    string result = appService.Deposit(accountNumber, SectionInput, BankInput, amount);
                    Console.WriteLine(result);
                    break;

                    case 3: // CASH-IN
                        Console.Write("\n WITHDRAW CHOICES: \n" +
                                      "1. BANK TRANSFER [BT]\n" +
                                      "2. OVER-THE-COUNTER CASH-OUT [OTC]\n" +
                                      "3. PARTNER OUTLET CASH-OUT [PO]\n" +
                                      "PLEASE SELECT AN OPTION [BCI|OTC|PO]: ");
                        string SectionInput2 = Console.ReadLine().ToUpper();

                        string BankInput2 = "";

                    switch (SectionInput2)
                    {
                        case "BT":
                            Console.Write("\nBANK TRANSFER OPTIONS: \n" +
                                              "1. BPI \n" +
                                              "2. BDO \n" +
                                              "3. LANDBANK \n" +
                                              "ENTER BANK TRANSFER [BT]: ");
                            BankInput2 = Console.ReadLine().ToUpper();
                            break;

                        case "OTC":
                            Console.Write("\nOVER-THE-COUNTER CASH-OUT OPTIONS: \n" +
                                              "1. PALAWAN \n" +
                                              "2. CEBUANA \n" +
                                              "3. VILLARICA \n" +
                                              "ENTER OVER-THE-COUNTER CASH-IN [OTC] BANK: ");
                            BankInput2 = Console.ReadLine().ToUpper();
                            break;

                        case "PO":
                            Console.Write("\nPARTNER OUTLET CASH-OUT OPTIONS: \n" +
                                              "1. 7-ELEVEN \n" +
                                              "2. SM \n" +
                                              "3. PUREGOLD \n" +
                                              "ENTER PARTNER-OUTLET CASH-IN [PO]: ");
                            BankInput2 = Console.ReadLine().ToUpper();
                            break;

                        default:
                            Console.WriteLine("Invalid withdraw option.");
                            return;
                    }

                    Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP");
                        double Wamount = Convert.ToDouble(Console.ReadLine());

                    string withdrawResult = appService.Withdraw(accountNumber, SectionInput2, BankInput2, Wamount);
                    Console.WriteLine(withdrawResult);
                    break;

            }
        }
    }

}

