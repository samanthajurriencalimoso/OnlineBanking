using System.Collections.Generic;
using OnlineBankingDataService;
using OnlineBankingDataModel;
using System;

namespace OnlineBankingAppService
{
    public class OnlineBankAppService
    {
        IOnlineBankingDataService dataService = new OnlineBankingDBData();

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

            if (amount <= 0) return "Invalid deposit amount.";

            switch (SectionInput)
            {
                case "BCI":
                    switch (BankInput)
                    {
                        case "BPI":
                        case "BDO":
                        case "LANDBANK":
                            Fee = 15.00;
                            break;
                        default:
                            return "Invalid bank for Bank Cash-In.";
                    }
                    break;
                case "OTC":
                    switch (BankInput)
                    {
                        case "ROBINSONS":
                        case "HANDYMAN":
                            Fee = 15.00;
                            break;
                        case "7-ELEVEN":
                            Fee = 0.02;
                            break;
                        default:
                            return "Invalid bank/provider for Over-the-counter options.";
                    }
                    break;
                case "PO":
                    switch (BankInput) {
                        case "7-ELEVEN":
                            Fee = 0.02;
                            break;
                        case "SM":
                        case "PUREGOLD":
                            Fee = 10.00;
                            break;
                        default:
                            return "Invalid provider for Pay-Online options.";
                    } break;
                default:
                    return "Invalid deposit section.";
        }

            account.balance += amount - Fee;
            dataService.Update(account);

            return $"Deposit successful. Fee: PHP {Fee}. New balance: PHP {account.balance}";
        }

        public string Withdraw(int accountNumber, string SectionInput, string BankInput, double amount) // CASH-IN
        {
            var account = dataService.GetAccNum(accountNumber);
            if (account == null)
            {
                return "Account not found.";
            }

            double Fee = 0.0;

           if (amount <= 0)
            {
                return "Invalid withdrawal amount.";
            }

            switch (SectionInput)
            {
                case "BT":
                    switch (BankInput)
                    {
                        case "BPI":
                        case "BDO":
                        case "LANDBANK":
                            Fee = 20.00;
                            break;
                        default:
                            return "Invalid bank for BT.";
                    }
                    break;

                case "OTC":
                    switch (BankInput)
                    {
                        case "PALAWAN":
                        case "CEBUANA":
                        case "VILLARICA":
                            Fee = 15.00;
                            break;
                        default:
                            return "Invalid provider for OTC.";
                    }
                    break;
                case "PO":
                    switch (BankInput)
                    {
                        case "7-ELEVEN":
                            Fee = 0.02;
                            break;
                        case "SM":
                        case "PUREGOLD":
                            Fee = 10.00;
                            break;
                        default:
                            return "Invalid provider for PO.";
                    }
                    break;

                default:
                    return "Invalid withdrawal section.";
            }
            if (account.balance < amount + Fee)
                return "Insufficient balance.";

            account.balance -= amount + Fee;
            dataService.Update(account);

            return $"Withdrawal successful. Fee: PHP {Fee}. New balance: PHP {account.balance}";
        }
    }
}
