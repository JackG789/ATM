using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public static class Global
    {
        //this is the semaphore which will control the access to the bank funds
        //Will only allow one thread in at a time and starts with one space open
        public static Semaphore _access = new Semaphore(1, 1);
    }


    public class Account
    {
        //the attributes for the account
        private int balance;
        private int pin;
        private int accountNum;

        // a constructor that takes initial values for each of the attributes (balance, pin, accountNumber)
        public Account(int balance, int pin, int accountNum)
        {
            this.balance = balance;
            this.pin = pin;
            this.accountNum = accountNum;
        }

        //getter and setter functions for balance
        public int getBalance()
        {
            return balance;
        }
        public void setBalance(int newBalance)
        {
            this.balance = newBalance;
        }

        /*
         *   This funciton allows us to decrement the balance of an account
         *   it perfomes a simple check to ensure the balance is greater than
         *   the amount being debeted
         *   
         *   reurns:
         *   true if the transactions if possible
         *   false if there are insufficent funds in the account
         */
        public Boolean decrementBalance(int amount)
        {
            if (this.balance > amount)
            {
                //After the balance is checked, we will manually wait 2 seconds to allow
                //The user to create a race condition themselves
                //(e.g. withdrawing £500 from an 
                //account at two atms where that balance is only £750 - results in £-250)
                Thread.Sleep(2000);
                balance -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
         * This funciton checks the account pin against the argument passed to it
         *
         * returns:
         * true if they match
         * false if they do not
         */
        public Boolean checkPin(int pinEntered)
        {
            if (pinEntered == pin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int getAccountNum()
        {
            return accountNum;
        }

    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]


        public static void Main()
        {
            //Create 3 accounts
            Account[] ac = new Account[3];
            ac[0] = new Account(300, 1111, 111111);
            ac[1] = new Account(750, 2222, 222222);
            ac[2] = new Account(3000, 3333, 333333);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Create 4 ATM windows and create a thread for each
            for (int i=0; i<4; i++)
            {
                Thread _thread = new Thread(() =>
                {
                    Application.Run(new ATMForm(ac));
                });
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start();

            }
        }
    }


}
