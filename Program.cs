using OnlineBankingAppService;
using OnlineBankingDataModel;
using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace OnlineBanking_Act1
{
    internal class Program
    {
        static OnlineBankAppService appService = new OnlineBankAppService();
        static void Main(string[] args)
        {
            Console.Write("WELCOME TO ONLINE BANKING \n");
            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Write("-------------------------\n");
                Console.Write("1. CREATE ACCOUNT \n" +
                              "2. LOGIN (BALANCE, DEPOSIT, WITHDRAW) \n" +
                              "3. EXIT \n");
                Console.Write("-------------------------\n");
                Console.Write("PLEASE SELECT AN OPTION: ");
                int MenuInput;

                if (!int.TryParse(Console.ReadLine(), out MenuInput))
                {
                    Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                    continue;
                }

                switch (MenuInput)
                {
                    case 1:
                        Register();
                        break;
                    case 2:
                        LOGIN();
                        break;
                    case 3:
                        Console.WriteLine("THANK YOU FOR USING ONLINE BANKING!");
                        return;
                        break;
                    default:
                        Console.WriteLine("INVALID OPTION. PLEASE TRY AGAIN.");
                        continue;
                }
            }
        }
        static void Register()
        {
            /*
             * Register Requirements:
             * User must be over 18, Enter 4 digit pin, Confirm 4 digit pin (Security code)
             */

            bool success = false;

            do
            {
                Console.Write("-------------------------\n");
                Console.Write("ACCOUNT REGISTRATION\n" +
                              "AGE VALIDATION: \n" +
                              "ENTER YOUR AGE: ");
                int age;

                //Age Validation
                if (!int.TryParse(Console.ReadLine(), out age))
                {
                    Console.Write("INVALID AGE INPUT.");
                    continue;
                }

                if (age < 18 || age > 116)
                {
                    Console.WriteLine(age < 18
                        ? "SORRY, YOU MUST BE AT LEAST 18 YEARS OLD TO CREATE AN ACCOUNT."
                        : "SORRY, YOU MUST ENTER A VALID AGE.");
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("REGISTRATION FAILED");
                    Console.Write("DO YOU WANT TO REGISTER AGAIN? [Y|N]: ");
                    string retry = Console.ReadLine().ToUpper();
                
                    if (retry != "Y")
                    {
                        success = true;
                        Console.WriteLine("-------------------------\n");
                        Console.WriteLine("RETURNING TO THE MAIN MENU...");
                        MainMenu();
                        return;
                    }
                    else if (retry == "Y")
                    {
                        continue;
                    }
                }

                //Enter 4-Digit Pin
                    Console.Write("ENTER A 4-DIGIT PIN: ");
                    string pinCode = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(pinCode) || pinCode.Length != 4 || !int.TryParse(pinCode, out int pinInt))
                    {
                        Console.WriteLine("INVALID PIN. PLEASE ENTER A 4-DIGIT PIN.");
                        continue;
                    }

                //Confirm 4-Digit Pin (Security Code)
                        Console.Write("CONFIRM YOUR 4-DIGIT PIN: ");
                        string securityCode = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(securityCode) || securityCode.Length != 4 || !int.TryParse(securityCode, out int ConfirmPin))
                        {
                            Console.WriteLine("INVALID CONFIRMATION PIN.");
                            continue;
                        }

                        if (pinInt != ConfirmPin)
                        {
                            Console.WriteLine("SECURITY CODE DOES NOT MATCH THE PIN.");
                            continue;
                        }

                            BankAccount newAccount = appService.CreateAccount(age, pinInt, ConfirmPin);

                        //Display new account information
                        if (newAccount != null)
                        {
                            success = true;
                            Console.WriteLine("-------------------------------------");
                            Console.WriteLine("YOUR ACCOUNT HAS BEEN REGISTERED SUCCESSFULLY!");
                            Console.WriteLine($"YOUR ACCOUNT NUMBER IS: {newAccount.AccountNumber}");
                            Console.WriteLine($"INITIAL BALANCE: PHP {newAccount.balance}");
                            Console.WriteLine("PLEASE KEEP YOUR PIN SECURE.");

                            Console.WriteLine("-------------------------------------");
                            Console.WriteLine("YOU MAY NOW LOG IN TO START THE TRANSACTIONS.");
                            LOGIN();
                            return;
                            }
                        } while (!success);
        }

        static void LOGIN()
        {
            bool isContinue = true;

            do
            {
                Console.WriteLine("-------------------------");
                Console.Write("ENTER ACCOUNT NUMBER: ");
                int UserAccountNum;

                if(!int.TryParse(Console.ReadLine(), out UserAccountNum))
                {
                    Console.WriteLine("YOU MAY HAVE TYPED YOUR ACCOUNT NUMBER WRONG. PLEASE TRY AGAIN.");
                    continue;
                }

                //Account Validation
                var acc = appService.GetAccNum(UserAccountNum);

                if(acc == null)
                {
                    Console.WriteLine("THE ACCOUNT NUMBER YOU HAVE ENTERED DOES NOT EXIST IN OUR SYSTEM.");
                    continue;
                }

                bool authenticated = false;

                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("-------------------------");
                    Console.Write("ENTER 4-DIGIT CODE: ");
                    int UserPin;

                    if (!int.TryParse(Console.ReadLine(), out UserPin))
                    {
                        Console.WriteLine("YOU MAY HAVE TYPED YOUR PIN NUMBER WRONG. PLEASE TRY AGAIN.");
                        continue;
                    }

                    authenticated = appService.Authenticate(UserAccountNum, UserPin);

                    if (authenticated)
                    {
                        Console.WriteLine("-------------------------");
                        Console.WriteLine("-------------------------");
                        Console.WriteLine("Login Successful!");
                        Choices(UserAccountNum); 
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You only have " + (2 - i) + " tries left. Incorrect MPIN entered.");
                    }
                }

                Console.WriteLine("-------------------------");
                Console.Write("Do you want to continue? [Y/N]: ");
                string continueInput = Console.ReadLine();

                if (continueInput.ToUpper() == "Y")
                {
                    isContinue = true;
                }
                else if (continueInput.ToUpper() == "N")
                {
                    Console.WriteLine("RETURNING TO MAIN MENU...");
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
            bool stayLoggedIn = true;

            while (stayLoggedIn)
            { 
            Console.WriteLine("-------------------------");
            Console.WriteLine("Welcome! What do you want to do today? \n" +
                              "1. BALANCE \n" +
                              "2. DEPOSIT \n" +
                              "3. WITHDRAW \n" +
                              "4. EXIT \n" +
                              "OTHER OPTIONS ON THE WAY!");

            Console.Write("PLEASE SELECT AN OPTION: ");
            int MenuInput;

            if (!int.TryParse(Console.ReadLine(), out MenuInput))
            {
                Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                continue;
            }

                switch (MenuInput)
                {
                    case 1: //RETRIEVE
                        Console.WriteLine("-------------------------");
                        Console.WriteLine("Your Balance is: PHP " + appService.GetBalance(accountNumber));
                        break;
                    case 2: // CASH-IN
                        Console.WriteLine("-------------------------");
                        Console.Write("DEPOSIT CHOICES: \n" +
                                      "1. BANK CASH-IN\n" +
                                      "2. OVER-THE-COUNTER CASH-IN\n" +
                                      "3. PARTNER OUTLET CASH-IN\n" +
                                      "PLEASE SELECT AN OPTION: ");
                        int SectionInput;

                        if (!int.TryParse(Console.ReadLine(), out SectionInput))
                        {
                            Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                            continue;
                        }

                        int BankInput = 0;
                        string BankOption = string.Empty;

                        switch (SectionInput)
                        {
                            case 1:
                                Console.WriteLine("-------------------------");
                                Console.Write("BANK CASH-IN OPTIONS: \n" +
                                                  "1. BPI \n" +
                                                  "2. BDO \n" +
                                                  "3. LANDBANK \n" +
                                                  "ENTER BANK CASH-IN BANK: ");
                                if (!int.TryParse(Console.ReadLine(), out BankInput))
                                {
                                    Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                                    continue;
                                }

                                switch (BankInput)
                                {
                                    case 1: 
                                        BankOption = "BPI";
                                        break;
                                    case 2:
                                        BankOption = "BDO";
                                        break;
                                    case 3:
                                        BankOption = "LANDBANK";
                                        break;
                                    default:
                                        Console.WriteLine("INVALID BANK CASH-IN OPTION.");
                                        continue;
                                }

                                break;
                            case 2:
                                Console.WriteLine("-------------------------");
                                Console.Write("BANK OVER-THE-COUNTER OPTIONS: \n" +
                                                  "1. ROBINSONS \n" +
                                                  "2. HANDYMAN \n" +
                                                  "3. 7-ELEVEN \n" +
                                                  "ENTER OVER-THE-COUNTER CASH-IN BANK: ");
                                if (!int.TryParse(Console.ReadLine(), out BankInput))
                                {
                                    Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                                    continue;
                                }

                                switch (BankInput)
                                {
                                    case 1:
                                        BankOption = "ROBINSONS";
                                        break;
                                    case 2:
                                        BankOption = "HANDYMAN";
                                        break;
                                    case 3:
                                        BankOption = "7-ELEVEN";
                                        break;
                                    default:
                                        Console.WriteLine("INVALID OVER-THE-COUNTER OPTION.");
                                        continue;
                                }

                                break;
                            case 3:
                                Console.WriteLine("-------------------------");
                                Console.Write("PARTNER OUTLET OPTIONS: \n" +
                                                  "1. 7-ELEVEN \n" +
                                                  "2. SM \n" +
                                                  "3. PUREGOLD \n" +
                                                  "ENTER PARTNER OUTLET CASH-OUT: ");
                                if (!int.TryParse(Console.ReadLine(), out BankInput))
                                {
                                    Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                                    continue;
                                }

                                switch (BankInput)
                                {
                                    case 1:
                                        BankOption = "7-ELEVEN";
                                        break;
                                    case 2:
                                        BankOption = "SM";
                                        break;
                                    case 3:
                                        BankOption = "PUREGOLD";
                                        break;
                                    default:
                                        Console.WriteLine("INVALID PARTNER OUTLET OPTION.");
                                        continue;
                                }

                                break;
                            default:
                                Console.WriteLine("INVALID DEPOSIT OPTION.");
                                continue;
                        }

                        Console.WriteLine("-------------------------");
                        Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP ");
                        double amount;

                        if (!double.TryParse(Console.ReadLine(), out amount))
                        {
                            Console.WriteLine("INVALID AMOUNT. PLEASE ENTER A NUMBER.");
                            break;
                        }

                        double currentBalance = appService.GetBalance(accountNumber);

                        if (amount < 100)
                        {
                            Console.WriteLine("MINIMUM DEPOSIT AMOUNT IS PHP 100. PLEASE ENTER A VALID AMOUNT.");
                            continue;
                        }

                        if (amount > 100000)
                        {
                            Console.WriteLine("MAXIMUM DEPOSIT AMOUNT IS PHP 100,000. PLEASE ENTER A VALID AMOUNT.");
                            continue;
                        }

                        if (currentBalance + amount > 1000000)
                        {
                            Console.WriteLine("DEPOSIT FAILED. YOUR ACCOUNT BALANCE CANNOT EXCEED PHP 1,000,000.");
                            continue;
                        }

                        var result = appService.Deposit(accountNumber, SectionInput, BankInput, BankOption, amount);

                        if (result.success)
                        {
                            Console.WriteLine($"Fee: {result.fee}");
                            Console.WriteLine($"Balance: {result.newBalance}");
                        }
                        else
                        {
                            Console.WriteLine("DEPOSIT FAILED. PLEASE CHECK INPUTS.");
                        }
                        break;

                    case 3: // CASH-OUT
                        Console.WriteLine("-------------------------");
                        Console.Write("WITHDRAW / TRANSFER CHOICES: \n" +
                                      "1. SEND MONEY (INTERNAL TRANSFER)\n" +
                                      "2. BANK TRANSFER\n" +
                                      "3. OVER-THE-COUNTER CASH-OUT\n" +
                                      "4. PARTNER OUTLET CASH-OUT\n" +
                                      "PLEASE SELECT AN OPTION: ");
                        int SectionInput2;

                        if (!int.TryParse(Console.ReadLine(), out SectionInput2))
                        {
                            Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                            continue;
                        }

                        int BankInput2 = 0;
                        string BankOption2 = string.Empty;

                        switch (SectionInput2)
                        {
                            case 1:
                                Console.WriteLine("-------------------------");
                                Console.Write("SEND MONEY (INTERNAL TRANSFER) \n");
                                Console.Write("ENTER RECEIVER ACCOUNT NUMBER [EX. 1000]: ");

                                if (!int.TryParse(Console.ReadLine(), out BankInput2))
                                {
                                    Console.WriteLine("INVALID AMOUNT. PLEASE ENTER A NUMBER.");
                                }

                                BankOption2 = "SEND MONEY";
                                break;

                            case 2:
                                Console.WriteLine("-------------------------");
                                Console.Write("BANK TRANSFER OPTIONS: \n" +
                                              "1. BPI \n" +
                                              "2. BDO \n" +
                                              "3. LANDBANK \n" +
                                              "ENTER BANK TRANSFER: ");
                                if (!int.TryParse(Console.ReadLine(), out BankInput2))
                                {
                                    Console.WriteLine("INVALID AMOUNT. PLEASE ENTER A NUMBER.");
                                }

                                switch (BankInput2)
                                {
                                    case 1:
                                        BankOption2 = "BPI";
                                        break;
                                    case 2:
                                        BankOption2 = "BDO";
                                        break;
                                    case 3:
                                        BankOption2 = "LANDBANK";
                                        break;
                                    default:
                                        Console.WriteLine("INVALID BANK TRANSFER OPTION.");
                                        continue;
                                }
                                break;
                            case 3:
                                Console.WriteLine("-------------------------");
                                Console.Write("OVER-THE-COUNTER CASH-OUT OPTIONS: \n" +
                                              "1. PALAWAN \n" +
                                              "2. CEBUANA \n" +
                                              "3. VILLARICA \n" +
                                              "ENTER OVER-THE-COUNTER CASH-OUT: ");
                                if (!int.TryParse(Console.ReadLine(), out BankInput2))
                                {
                                    Console.WriteLine("INVALID AMOUNT. PLEASE ENTER A NUMBER.");
                                }

                                switch (BankInput2)
                                {
                                    case 1:
                                        BankOption2 = "PALAWAN";
                                        break;
                                    case 2:
                                        BankOption2 = "CEBUANA";
                                        break;
                                    case 3:
                                        BankOption2 = "VILLARICA";
                                        break;
                                    default:
                                        Console.WriteLine("INVALID OVER-THE-COUNTER OPTION.");
                                        continue;
                                }
                                break;
                            case 4:
                                Console.WriteLine("-------------------------");
                                Console.Write("PARTNER OUTLET CASH-OUT OPTIONS: \n" +
                                              "1. 7-ELEVEN \n" +
                                              "2. SM \n" +
                                              "3. PUREGOLD \n" +
                                              "ENTER PARTNER OUTLET CASH-OUT: ");
                                if (!int.TryParse(Console.ReadLine(), out BankInput2))
                                {
                                    Console.WriteLine("INVALID AMOUNT. PLEASE ENTER A NUMBER.");
                                }

                                switch(BankInput2)
                                {
                                    case 1:
                                        BankOption2 = "7-ELEVEN";
                                        break;
                                    case 2:
                                        BankOption2 = "SM";
                                        break;
                                    case 3:
                                        BankOption2 = "PUREGOLD";
                                        break;
                                    default:
                                        Console.WriteLine("INVALID PARTNER OUTLET OPTION.");
                                        continue;
                                    }
                                    break;
                            default:
                                Console.WriteLine("INVALID WITHDRAW OPTION.");
                                return;
                        }

                        Console.WriteLine("-------------------------");
                        Console.Write("ENTER THE AMOUNT TO WITHDRAW: PHP ");
                        double Wamount;

                        if (!double.TryParse(Console.ReadLine(), out Wamount))
                        {
                            Console.WriteLine("INVALID INPUT. PLEASE ENTER A NUMBER.");
                            return;
                        }

                        double currentBalance2 = appService.GetBalance(accountNumber);

                        if (Wamount < 100)
                        {
                            Console.WriteLine("MINIMUM WITHDRAWAL AMOUNT IS PHP 100. PLEASE ENTER A VALID AMOUNT.");
                            continue;
                        }

                        if (Wamount > 100000)
                        {
                            Console.WriteLine("MAXIMUM WITHDRAWAL AMOUNT IS PHP 100,000. PLEASE ENTER A VALID AMOUNT.");
                            continue;
                        }

                        if(Wamount > currentBalance2)
                        {
                            Console.WriteLine("WITHDRAWAL FAILED. INSUFFICIENT FUNDS.");
                            continue;
                        }

                        if (SectionInput2 == 1)
                        {
                            var transferResult = appService.SendMoney(accountNumber, BankInput2, BankOption2, Wamount);

                            if (transferResult.success)
                            {
                                Console.WriteLine("TRANSFER SUCCESSFUL.");
                                Console.WriteLine($"Balance: {transferResult.newBalance}");
                            }
                            else
                            {
                                Console.WriteLine("TRANSFER FAILED. PLEASE CHECK INPUTS.");
                            }
                        }

                        else
                        {
                            var withdrawResult = appService.Withdraw(accountNumber, SectionInput2, BankInput2, BankOption2, Wamount);

                            if (withdrawResult.success)
                            {
                                Console.WriteLine("WITHDRAWAL SUCCESSFUL.");
                                Console.WriteLine($"Fee: {withdrawResult.fee}");
                                Console.WriteLine($"Balance: {withdrawResult.newBalance}");
                            }
                            else
                            {
                                Console.WriteLine("WITHDRAWAL FAILED. PLEASE CHECK INPUTS.");
                            }
                        }
                        break;

                    case 4: //EXIT
                        var receipt = appService.PrintReceipt(accountNumber);

                        Console.WriteLine("-----------DIGITAL RECEIPT------------");
                        Console.WriteLine($"ACCOUNT: {receipt.accountNumber}");
                        Console.WriteLine("TRANSACTIONS:");

                        foreach (var t in receipt.transactions)
                        {
                            Console.WriteLine($"  - {t}");
                        }

                        Console.WriteLine($"CURRENT BALANCE: PHP {receipt.balance}");
                        Console.WriteLine($"DATE: {receipt.date:dd-MMM-yyyy}");
                        Console.WriteLine("-------------------------------------");
                        stayLoggedIn = false;
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("INVALID OPTION. PLEASE TRY AGAIN.");
                        break;
                }
            }
        }
    }
}

