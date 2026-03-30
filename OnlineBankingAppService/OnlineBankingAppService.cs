using OnlineBankingDataModel;
using OnlineBankingDataService;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace OnlineBankingAppService
{
    public class OnlineBankAppService
    {
        BankingDataService dataService = new BankingDataService(new OnlineBankingDBData());

        public bool Authenticate(int accountNumber, int pincode)
        {
            var account = dataService.GetAccNum(accountNumber);
            return account != null && account.Pincode == pincode;

        }

        public string CreateAccount(int age, int pin, int securityCode)
        {

            if (pin != securityCode)
            {
                return "SECURITY CODE DOES NOT MATCH THE PIN.";
            }

            int newAccNo = dataService.GenerateNewAccountNumber();
            var newAccount = new BankAccount
            {
                AccountNumber = newAccNo,
                Pincode = pin,
                balance = 0
            };

            dataService.Add(newAccount);

            return "-------------------------------------\n" +
                   $"YOUR ACCOUNT HAS BEEN REGISTERED SUCCESSFULLY!\n" +
                   $"YOUR ACCOUNT NUMBER IS: {newAccNo}\n" +
                   $"INITIAL BALANCE: PHP {newAccount.balance}\n" +
                   $"PLEASE KEEP YOUR PIN SECURE.";
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
                            return "Invalid provider for Pay-Online options.";
                    }
                    break;
                default:
                    return "Invalid deposit section.";
            }

            account.balance += amount - Fee;
            dataService.Update(account);
            account.Transactions.Add($"DEPOSIT PHP {amount} via {BankInput}");

            return $"Deposit successful. Fee: PHP {Fee}. New balance: PHP {account.balance}";
        }

        public string SendMoney(int SenderAccNo, string ReceiverAccInput, double amount)
        {
            int receiverAccNo;
            if (!int.TryParse(ReceiverAccInput, out receiverAccNo))
            {
                return "Invalid Receiver Account Number.";
            }

            var sender = dataService.GetAccNum(SenderAccNo);
            var receiver = dataService.GetAccNum(receiverAccNo);

            if (sender == null || receiver == null)
            {
                return "Account not found.";
            }

            if (sender.balance < amount)
            {
                return "Insufficient balance.";
            }

            sender.balance -= amount;
            receiver.balance += amount;

            dataService.Update(sender);
            dataService.Update(receiver);
            sender.Transactions.Add($"SEND MONEY PHP {amount} TO ACCOUNT {receiverAccNo}");

            return $"Transfer successful! \n" +
                   $"PHP {amount} has been transferred to account number {receiverAccNo}." +
                   $" New balance: PHP {sender.balance}";
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
                case "SM":
                    return SendMoney(accountNumber, BankInput, amount);
                    break;

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
            account.Transactions.Add($"WITHDRAW PHP {amount} via {BankInput}");

            return $"Withdrawal successful. Fee: PHP {Fee}. New balance: PHP {account.balance}";
        }

        public string PrintReceipt(int accountNumber)
        {
            var account = dataService.GetAccNum(accountNumber);

            var receipt = "\n-----------DIGITAL RECEIPT------------\n" +
                         $"ACCOUNT: {account.AccountNumber}\n" +
                          "TRANSACTIONS: \n";
            foreach (var transaction in account.Transactions)
            {
                receipt += $"  -  {transaction} \n";
            }

            receipt +=  $"CURRENT BALANCE: PHP {account.balance}\n" +
                        $"DATE: {DateTime.Now: dd-MMM-yyyy}\n" +
                        "-------------------------------------";
            return receipt;
        }
    }
}
