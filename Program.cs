using System;

namespace OnlineBanking_Act1
{
    internal class Program
    {
        static double deposit = 0.0, balance = 0.0, amount = 0.0, transfer = 0.0, withdraw = 0.0;
        static string OptionCon = "";
        static void Main(string[] args)
        {
            Console.WriteLine("ONLINE BANKING");
            bool isContinue = false;
            do { 
            Console.WriteLine("Welcome! What do you want to do today? \n" +
                              "1. BALANCE \n" +
                              "2. DEPOSIT \n" +
                              "3. WITHDRAW \n" +
                              "4. TRANSFER \n" +
                              "OTHER OPTIONS ON THE WAY!");

            Console.Write("PLEASE SELECT AN OPTION: ");
            int MenuInput = Convert.ToInt32(Console.ReadLine());

            switch (MenuInput)
            {
                case 1:
                    Balance(); //RETRIEVE
                    break;
                case 2: //UPDATE
                    Deposit();
                    break;
                case 3: //UPDATE
                    Withdraw(); break;
                case 4:
                    Transfer(); break;
                default:
                    Console.WriteLine("Invalid input. System will exit.");
                    Environment.Exit(0);
                    break;
            }
                Console.Write("Do you want to Continue? (Y|N): ");
                OptionCon = Console.ReadLine().ToUpper();

                if (OptionCon == "Y")
                {
                    isContinue = true;
                }
                else if (OptionCon == "N")
                {
                    Console.WriteLine("Thank you for using our service! See you again!");
                    isContinue = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. System will exit.");
                }
            } while (isContinue);
        }
        static void Balance()
        {
            Console.WriteLine("CURRENT BALANCE IS ₱" + balance);
        }
        static void Deposit()
        {

            Console.Write("ENTER THE AMOUNT TO DEPOSIT: ₱");
            deposit = Convert.ToDouble(Console.ReadLine());

            balance = balance + deposit;

        }
        static void Withdraw()
        {
            Console.Write("WITHDRAW CHOICES: \n" +
                          "1. WITHDRAWAL \n" +
                          "2. BANK TRANSFER \n" +
                          "PLEASE SELECT AN OPTION: ");
            int WithdrawInput = Convert.ToInt32(Console.ReadLine());

            if (WithdrawInput == 1)
            {
                Console.Write("ENTER THE AMOUNT TO WITHDRAW: ₱");
                withdraw = Convert.ToDouble(Console.ReadLine());
                balance = balance - withdraw;
            }

            else if (WithdrawInput == 2)
            {
                Console.Write("ENTER THE AMOUNT TO DEPOSIT: ₱");
                withdraw = Convert.ToDouble(Console.ReadLine());
                /* Insert transfer FEE to bank account 
                 * BPI | BDO | METROBANK | ETC
                 * SECURITY BANK | UNION BANK | PNB 
                 * CHINA BANK | RCBC | ETC
                 
                   balance = balance - withdraw; */
            }

            else
            {
                Console.WriteLine("Invalid input. System will exit.");
                Environment.Exit(0);
            }

                Console.WriteLine("UPDATE TRANSACTION HISTORY: " + amount +
                          ". UPDATED BALANCE ₱" + balance);
        }

            static void Transfer()
        {
            Console.Write("TRANSFER CHOICES: \n" +
                          "1. CASH IN \n" +
                          "2. BANK TRANSFER \n" +
                          "PLEASE SELECT AN OPTION: ");

            int TransferInput = Convert.ToInt32(Console.ReadLine());

            if (TransferInput == 1)
            {

                Console.Write("ENTER THE AMOUNT TO TRANSFER: ₱");
                transfer = Convert.ToDouble(Console.ReadLine());

                balance = balance + transfer;

            }
            else if (TransferInput == 2)
            {
                Console.Write("ENTER THE AMOUNT TO TRANSFER: ₱");
                transfer = Convert.ToDouble(Console.ReadLine());

                /* Insert transfer FEE to bank account 
                 * BPI | BDO | METROBANK | ETC
                 * SECURITY BANK | UNION BANK | PNB 
                 * CHINA BANK | RCBC | ETC
                 */
            }
            else
            {
                Console.WriteLine("Invalid input. System will exit.");
                Environment.Exit(0);
            }

            Console.WriteLine("UPDATE TRANSACTION HISTORY: " +amount+
                          ". UPDATED BALANCE ₱" +balance);
        }
    }
}