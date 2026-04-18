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

        public BankAccount GetAccNum(int accountNumber)
        {
            return dataService.GetAccNum(accountNumber);
        }

        public bool Authenticate(int accountNumber, int pincode)
        {
            var account = dataService.GetAccNum(accountNumber);
            return account != null && account.Pincode == pincode;
        }

        public BankAccount CreateAccount(int age, int pin, int securityCode)
        {
            int newAccNo = dataService.GenerateNewAccountNumber();
            var newAccount = new BankAccount
            {
                AccountNumber = newAccNo,
                Pincode = pin,
                balance = 0,
                Transactions = new List<string>()
            };

            dataService.Add(newAccount);
            return newAccount;
        }

        public double GetBalance(int accountNumber)
        {
            var account = dataService.GetAccNum(accountNumber);
            return account != null ? account.balance : 0.0;
        }

        public (bool success, double fee, double newBalance) Deposit(int accountNumber, int SectionInput, int BankInput, string BankOption, double amount) // CASH-IN
        {
            var account = dataService.GetAccNum(accountNumber);
            if (account == null || amount <= 0)
            {
                return (false, 0.0, 0.0);
            }

            double Fee = 0.0;

            switch (SectionInput)
            {
                case 1:
                    switch (BankInput)
                    {
                        case 1:
                        case 2:
                        case 3:
                            Fee = 15.00;
                            break;
                        default:
                            return (false, 0.0, 0.0);
                    }
                    break;
                case 2:
                    switch (BankInput)
                    {
                        case 1:
                        case 2:
                            Fee = 15.00;
                            break;
                        case 3:
                            Fee = 0.02;
                            break;
                        default:
                            return (false, 0.0, 0.0);
                    }
                    break;
                case 3:
                    switch (BankInput)
                    {
                        case 1:
                            Fee = 0.02;
                            break;
                        case 2:
                        case 3:
                            Fee = 10.00;
                            break;
                        default:
                            return (false, 0.0, 0.0);
                    }
                    break;
                default:
                    return (false, 0.0, 0.0);
            }

            account.balance += amount - Fee;
            account.Transactions.Add($"DEPOSIT PHP {amount} via {BankOption}");
            dataService.Update(account);

            return (true, Fee, account.balance);
        }

        public (bool success, double newBalance) SendMoney(int SenderAccNo, int ReceiverAccInput, string BankOption, double amount)
        {

            var sender = dataService.GetAccNum(SenderAccNo);
            var receiver = dataService.GetAccNum(ReceiverAccInput);

            if (sender == null || receiver == null)
                return (false, 0);

            if (amount <= 0)
                return (false, 0.0);

            if (sender.balance < amount)
                return (false, 0.0);

            sender.balance -= amount;
            receiver.balance += amount;

            sender.Transactions.Add($"SEND MONEY PHP {amount} TO ACCOUNT {ReceiverAccInput}");
            receiver.Transactions.Add($"RECEIVED PHP {amount} FROM ACCOUNT {SenderAccNo}");

            dataService.Update(sender);
            dataService.Update(receiver);

            return (true, sender.balance);
        }

        public (bool success, double fee, double newBalance) Withdraw(int accountNumber, int SectionInput, int BankInput, string BankOption2, double amount) // CASH-IN
        {
            var account = dataService.GetAccNum(accountNumber);
            if (account == null)
                return (false, 0.0, 0.0);

            if (amount <= 0)
                return (false, 0.0, 0.0);

            double Fee = 0.0;

            switch (SectionInput)
            {
                case 1: // SEND MONEY
                    {
                        var result = SendMoney(accountNumber, BankInput, BankOption2, amount);
                        return (result.success, 0.0, result.newBalance);
                    }
                    break;

                case 2: // BANK TRANSFER
                    switch (BankInput)
                    {
                        case 1:
                        case 2:
                        case 3:
                            Fee = 20.00;
                            break;
                        default:
                            return (false, 0, 0);
                    }
                    break;

                case 3: // OVER-THE-COUNTER
                    switch (BankInput)
                    {
                        case 1:
                        case 2:
                        case 3:
                            Fee = 15.00;
                            break;
                        default:
                            return (false, 0.0, 0.0);
                    }
                    break;
                case 4: // PARTNER OUTLET
                    switch (BankInput)
                    {
                        case 1:
                            Fee = 0.02;
                            break;
                        case 2:
                        case 3:
                            Fee = 10.00;
                            break;
                        default:
                            return (false, 0.0, 0.0);
                    }
                    break;

                default:
                    return (false, 0.0, 0.0);
            }
            if (account.balance < amount + Fee)
                return (false, 0.0, 0.0);

            account.balance -= amount + Fee;
            account.Transactions.Add($"WITHDRAW PHP {amount} via {BankOption2}");

            dataService.Update(account);

            return (true, Fee, account.balance);
        }

        public (int accountNumber, List<string> transactions, double balance, DateTime date) PrintReceipt(int accountNumber)
        {
            var account = dataService.GetAccNum(accountNumber);

            if (account == null)
                return (0, new List<string>(), 0.0, DateTime.Now);

            return (account.AccountNumber, account.Transactions, account.balance, DateTime.Now);
        }
    }
}
