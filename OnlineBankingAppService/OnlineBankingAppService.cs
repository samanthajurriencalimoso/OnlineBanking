using System.Collections.Generic;
using OnlineBankingDataService;
using OnlineBankingDataModel;
using System;

namespace OnlineBankingAppService
{
    public class OnlineBankAppService
    {
        OnlineBankDataService dataService = new OnlineBankDataService();

       
        static double Fee = 0.0;
        static double balance = 0.0;
        static double deposit = 0.0;
        static double withdraw = 0.0;
        static double amount = 0.0;

        public bool Authenticate(int accountNumber, int pincode)
        {
            var account = dataService.GetAccNum(accountNumber);
            return account != null && account.Pincode == pincode;

        }


        public double GetBalance(int accountNumber)
        {
            var account = dataService.GetAccNum(accountNumber);
            return account != null ? account.balance : 0.0;
        }

        public void Deposit(int accountNumber, string DepositInput, double amount) // CASH-IN
        {
            var account = dataService.GetAccNum(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            switch (DepositInput)
            {
                case "BCI":
                    Console.WriteLine("BANK CASH-IN OPTIONS:" +
                                      "1. BPI \n" +
                                      "2. BDO \n" +
                                      "3. LANDBANK \n" +
                                      "ENTER BANK CASH-IN [BCI] BANK: ");
                    string BankInput = Console.ReadLine().ToUpper();
                    switch (BankInput)
                    {
                        case "BPI":
                        case "BDO":
                        case "LANDBANK":
                            Fee = 15.00;
                            break;
                    } break;
                case "OTC":
                    Console.WriteLine("BANK OVER-THE-COUNTER OPTIONS:" +
                                      "1. ROBINSONS \n" + 
                                      "2. HANDYMAN \n" +
                                      "3. 7-ELEVEN \n" +
                                      "ENTER OVER-THE-COUNTER CASH-IN [OTC] BANK: ");
                    BankInput = Console.ReadLine().ToUpper();
                    switch (BankInput)
                    {
                        case "ROBINSONS":
                        case "HANDYMAN":
                            Fee = 15.00;
                            break;
                        case "7-ELEVEN":
                            Fee = 0.02;
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("WE REGRET TO INFORM YOU THAT BANK IS NOT INCLUDED. WE WILL REVISIT THIS FEATURE LATER."); return;
            }
            balance = balance + (amount - Fee);
            BankAccount accountUpdate = new BankAccount();
            accountUpdate.balance = balance;
            

            dataService.Update(account);

            Console.WriteLine("THE AMOUNT DEPOSITED WITH THE BANK FEE IS: PHP" + balance);
        }
        public void Withdraw(int accountNumber, double amount) //CASH-OUT
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
                amount = balance - withdraw;
            }

            else if (WithdrawInput == 2)
            {
                Console.Write("ENTER THE AMOUNT TO WITHDRAW: PHP");
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

        public void Withdraw(int accountNumber, object withdrawAmount)
        {
            throw new NotImplementedException();
        }

        public void Deposit(int accountNumber, object damount)
        {
            throw new NotImplementedException();
        }
    }
}
