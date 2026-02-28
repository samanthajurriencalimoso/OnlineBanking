using System;

namespace OnlineBanking_Act1
{
    internal class Program
    {
        static double deposit = 0.0, balance = 0.0, withdraw = 0.0;
        static string OptionCon = "";
        static int correctPin = 0000, Fee = 15, ServiceFee;
        static void Main(string[] args)
        {
            Console.WriteLine("ONLINE BANKING");

            for (int i = 0; i < 3; i++)
            {
                Console.Write("Good day! \n" +
                              "ENTER 4-DIGIT CODE: ");
                int UserPin = Convert.ToInt32(Console.ReadLine());

                if (UserPin == correctPin)
                {
                    Choices(); break;
                }
                else
                {
                    Console.WriteLine("You only have " + (2 - i) + " tries left. Incorrect MPIN entered.");
                }
            }

            static void Choices()
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
                        Balance(); //RETRIEVE
                        break;
                    case 2: //UPDATE
                        Deposit(); // CASH-IN
                        break;
                    case 3: //UPDATE
                        Withdraw(); // CASH-OUT
                        break;
                    default:
                        Console.WriteLine("Invalid input. System will exit.");
                        break;
                }
            }


            static void Balance()
            {
                Console.WriteLine("CURRENT BALANCE IS PHP" + balance);
            }

            static void Deposit() // CASH-IN
            {
                Console.Write("\n DEPOSIT CHOICES: \n" +
                              "1. BANK CASH-IN \n" +
                              "2. OVER-THE-COUNTER CASH-IN\n" +
                              "PLEASE SELECT AN OPTION: ");
                int DepositInput = Convert.ToInt32(Console.ReadLine());

                switch (DepositInput)
                {
                    case 1:
                        Console.Write("ENTER BANK CASH-IN [BPI|BDO|LANDBANK]: ");
                        string Bank = Console.ReadLine();

                        string BankInput = Bank.ToUpper();

                        if (BankInput == "BPI" || BankInput == "BDO" || BankInput == "LANDBANK")
                        {   
                            Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP");
                            deposit = Convert.ToDouble(Console.ReadLine());

                            balance = balance + (deposit - Fee);

                            Console.WriteLine("THE AMOUNT DEPOSITED WITH THE BANK FEE IS: PHP" + deposit +
                                              ". UPDATED BALANCE PHP" + balance);

                        } break;
                    case 2:
                        Console.Write("ENTER BANK CASH-IN [7-ELEVEN|ROBINSONS|HANDYMAN]: ");
                        Bank = Console.ReadLine();

                        BankInput = Bank.ToUpper();

                        if (BankInput == "ROBINSONS" || BankInput == "HANDYMAN")
                        {
                            Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP");
                            deposit = Convert.ToDouble(Console.ReadLine());

                            balance = balance + (deposit - Fee);

                            Console.WriteLine("THE AMOUNT DEPOSITED WITH THE BANK FEE IS: PHP" + deposit +
                                              ". UPDATED BALANCE PHP" + balance);
                        }
                        else if (BankInput == "7-ELEVEN")
                        {
                            Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP");
                            deposit = Convert.ToDouble(Console.ReadLine());

                            balance = balance + (deposit - ServiceFee);

                            Console.WriteLine("THE AMOUNT DEPOSITED WITH THE BANK FEE IS: PHP" + deposit +
                                              ". UPDATED BALANCE PHP" + balance);
                        } break;
                    default:
                        Console.WriteLine("WE REGRET TO INFORM YOU THAT BANK IS NOT INCLUDED. WE WILL REVISIT THIS FEATURE LATER."); break;
                }
            }

            static void Withdraw() //CASH-OUT
            {
                Console.Write("WITHDRAW CHOICES: \n" +
                              "1. WITHDRAWAL \n" +
                              "2. BANK TRANSFER \n" +
                              "PLEASE SELECT AN OPTION: ");
                int WithdrawInput = Convert.ToInt32(Console.ReadLine());

                if (WithdrawInput == 1)
                {
                    Console.Write("ENTER THE AMOUNT TO WITHDRAW: PHP");
                    withdraw = Convert.ToDouble(Console.ReadLine());
                    balance = balance - withdraw;
                }

                else if (WithdrawInput == 2)
                {
                    Console.Write("ENTER THE AMOUNT TO DEPOSIT: PHP");
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



                //Console.WriteLine("UPDATE TRANSACTION HISTORY: PHP" + amount +
                //          ". UPDATED BALANCE PHP" + balance);
            }
        }
    }
}
