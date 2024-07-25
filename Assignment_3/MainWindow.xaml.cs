using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ATMApp
{
    public class Account  //account class creation
    {
        public int AccountNumber { get; private set; }
        public double Balance { get; private set; }
        public double InterestRate { get; private set; }
        public string AccountHolderName { get; private set; }
        public List<string> Transactions { get; private set; }

        public Account(int accountNumber, double initialBalance, double interestRate, string accountHolderName)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            InterestRate = interestRate;
            AccountHolderName = accountHolderName;
            Transactions = new List<string>();
        }

        public void Deposit(double amount) //method for depositing money in account
        {
            Balance += amount;
            Transactions.Add($"Deposited: {amount}");
        }

        public void Withdraw(double amount)  // method for withdrawing money from account
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                Transactions.Add($"Withdrew: {amount}");
            }
            else
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
        }

        public override string ToString()
        {
            return $"Account Holder: {AccountHolderName}, Account Number: {AccountNumber}, Balance: {Balance}, Interest Rate: {InterestRate}%";
        }
    }

    public class Bank  //bank class creation
    {
        private List<Account> accounts; //list for showing all accounts

        public Bank()
        {
            accounts = new List<Account>();
            // Create default accounts
            for (int i = 100; i < 110; i++)
            {
                accounts.Add(new Account(i, 100, 3, $"Default User {i}"));
            }
        }

        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

        public Account RetrieveAccount(int accountNumber)
        {
            return accounts.FirstOrDefault(acc => acc.AccountNumber == accountNumber);
        }

        public List<Account> GetAllAccounts()
        {
            return accounts;
        }
    }

    public partial class AtmApplication : Window  //inherting class
    {
        private Bank bank;
        private Account selectedAccount;

        public AtmApplication()
        {
            InitializeComponent();
            bank = new Bank();
            DisplayMainMenu();
        }

        private void DisplayMainMenu() //method for displaying all operation cases
        {
            while (true)
            {
                string choice = Prompt.ShowDialog("1. Create Account\n2. Select Account\n3. Exit", "ATM Main Menu");

                switch (choice)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        SelectAccount();
                        break;
                    case "3":
                        return;
                    default:
                        MessageBox.Show("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void CreateAccount()  //method for creating account by entering all the details
        {
            int accountNumber = int.Parse(Prompt.ShowDialog("Enter Account Number:", "Create Account"));
            double initialBalance = double.Parse(Prompt.ShowDialog("Enter Initial Balance:", "Create Account"));
            double interestRate = double.Parse(Prompt.ShowDialog("Enter Interest Rate:", "Create Account"));
            string accountHolderName = Prompt.ShowDialog("Enter Account Holder's Name:", "Create Account");

            Account newAccount = new Account(accountNumber, initialBalance, interestRate, accountHolderName);
            bank.AddAccount(newAccount);

            MessageBox.Show("Account created successfully.");
        }

        private void SelectAccount()  //method for selecting account
        {
            int accountNumber = int.Parse(Prompt.ShowDialog("Enter Account Number:", "Select Account"));
            selectedAccount = bank.RetrieveAccount(accountNumber);

            if (selectedAccount != null)
            {
                DisplayAccountMenu();
            }
            else
            {
                MessageBox.Show("Account not found. Please try again.");
            }
        }

        private void DisplayAccountMenu()  //method for displaying menu
        {
            while (true)
            {
                string choice = Prompt.ShowDialog("1. Check Balance\n2. Deposit\n3. Withdraw\n4. Display Transactions\n5. Exit Account", "Account Menu");

                switch (choice)
                {
                    case "1":
                        MessageBox.Show($"Balance: {selectedAccount.Balance}");
                        break;
                    case "2":
                        Deposit();
                        break;
                    case "3":
                        Withdraw();
                        break;
                    case "4":
                        DisplayTransactions();
                        break;
                    case "5":
                        return;
                    default:
                        MessageBox.Show("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void Deposit() // method for depositing money
        {
            double amount = double.Parse(Prompt.ShowDialog("Enter amount to deposit:", "Deposit"));
            selectedAccount.Deposit(amount);
            MessageBox.Show("Deposit successful.");
        }

        private void Withdraw() //method for withdrawing money from account
        {
            double amount = double.Parse(Prompt.ShowDialog("Enter amount to withdraw:", "Withdraw"));
            try
            {
                selectedAccount.Withdraw(amount);
                MessageBox.Show("Withdrawal successful.");
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayTransactions()  //method for displaying transactions
        {
            string transactions = string.Join("\n", selectedAccount.Transactions);
            MessageBox.Show(transactions);
        }
    }

    public static class Prompt  // class for prompting output window
    {
        public static string ShowDialog(string text, string caption)
        {
            Window prompt = new Window()
            {
                Width = 500,  //width of the window that will show output
                Height = 350, //height of the window that will show output
                Title = caption,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            StackPanel stackPanel = new StackPanel();

            TextBlock textBlock = new TextBlock()
            {
                Text = text,
                Margin = new Thickness(10)
            };
            stackPanel.Children.Add(textBlock);

            TextBox textBox = new TextBox() { Margin = new Thickness(10) };
            stackPanel.Children.Add(textBox);

            Button button = new Button()
            {
                Content = "OK",
                Width = 70,
                Height = 30,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            button.Click += (sender, e) => { prompt.Close(); };
            stackPanel.Children.Add(button);

            prompt.Content = stackPanel;
            prompt.ShowDialog();

            return textBox.Text;
        }
    }
}
