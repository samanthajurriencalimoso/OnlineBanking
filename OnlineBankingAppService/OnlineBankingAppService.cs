using System.Collections.Generic;
using OnlineBankingDataService;
using OnlineBankingDataModel;
using System;

namespace OnlineBankingAppService
{
    public class OnlineBankAppService
    {
        OnlineBankDataService dataService = new OnlineBankDataService();

       
        //static double Fee = 0.0;
        //static double balance = 0.0;
        //static double deposit = 0.0;
        //static double withdraw = 0.0;
        //static double amount = 0.0;

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

        public string Deposit(int accountNumber, string SectionInput, string BankInput, double amount) // CASH-IN
        {
            var account = dataService.GetAccNum(accountNumber);
            if (account == null) return "Account not found.";

            double Fee = 0.0;

            switch (SectionInput)
            {
                case "BCI":
                    Console.WriteLine("BANK CASH-IN OPTIONS: \n" +
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
                    Console.WriteLine("BANK OVER-THE-COUNTER OPTIONS: \n" +
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
                    } break;
                case "PO":
                    Console.WriteLine("PARTNER OUTLET OPTIONS: \n" +
                                      "1. SM  \n" +
                                      "2. PUREGOLD \n" +
                                      "3. PALAWAN PAWNSHOP \n" +
                                      "ENTER PARTNER OUTLET CASH-OUT: ");
                    BankInput = Console.ReadLine().ToUpper().Trim();
                    switch (BankInput)
                    {
                        case "SM":
                        case "PUREGOLD":
                        case "PALAWAN":
                            Fee = 10.00;
                            break;
                    }
                            break;
                default:
                    Console.WriteLine("WE REGRET TO INFORM YOU THAT BANK IS NOT INCLUDED. WE WILL REVISIT THIS FEATURE LATER."); return;
            }

            account.balance += amount - Fee;
            BankAccount accountUpdate = new BankAccount();

            dataService.Update(account);

            Console.WriteLine("THE AMOUNT DEPOSITED WITH THE BANK FEE IS: PHP" + account.balance);
        }

        public void Withdraw(int accountNumber, string SectionInput2, double amount) // CASH-IN
        {
            var account = dataService.GetAccNum(accountNumber);
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            double Fee = 0.0;

            switch (SectionInput2)
            {
                case "BT":
                    Console.WriteLine("BANK TRANSFER OPTIONS: \n" +
                                      "1. BPI \n" +
                                      "2. BDO \n" +
                                      "3. LANDBANK \n" +
                                      "ENTER BANK TRANSFER [BT] BANK: ");
                    string BankInput = Console.ReadLine().ToUpper();
                    switch (BankInput)
                    {
                        case "BPI":
                        case "BDO":
                        case "LANDBANK":
                            Fee = 20.00;
                            break;
                    }
                    break;
                case "OTC":
                    Console.WriteLine("OVER-THE-COUNTER CASH-OUT OPTIONS: \n" +
                                      "1. PALAWAN \n" +
                                      "2. CEBUANA \n" +
                                      "3. VILLARICA \n" +
                                      "ENTER OTC CASH-OUT: ");
                    BankInput = Console.ReadLine().ToUpper();
                    switch (BankInput)
                    {
                        case "PALAWAN":
                        case "CEBUANA":
                        case "VILLARICA":
                            Fee = 15.00;
                            break;
                        case "7-ELEVEN":
                            Fee = 0.02;
                            break;
                    }
                    break;

                case "PO":
                    Console.WriteLine("PARTNER OUTLET OPTIONS: \n" +
                                      "1. 7-ELEVEN \n" +
                                      "2. SM \n" +
                                      "3. PUREGOLD \n" +
                                      "ENTER PARTNER OUTLET CASH-OUT: ");
                    BankInput = Console.ReadLine().ToUpper();
                    switch (BankInput)
                    {
                        case "7-ELEVEN":
                            Fee = 0.02;
                            break;
                        case "SM":
                        case "PUREGOLD":
                            Fee = 10.00;
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("WE REGRET TO INFORM YOU THAT BANK IS NOT INCLUDED. WE WILL REVISIT THIS FEATURE LATER."); return;
            }
            account.balance -= amount + Fee;
            BankAccount accountUpdate = new BankAccount();

            dataService.Update(account);

            Console.WriteLine("THE AMOUNT DEPOSITED WITH THE BANK FEE IS: PHP" + account.balance);
        }

        //public void Withdraw(int accountNumber, object withdrawAmount)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Deposit(int accountNumber, object damount)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
