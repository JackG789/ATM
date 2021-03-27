using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public partial class ATMForm : Form
    {
        private Account[] ac;
        private Account activeAccount = null;

        //signal to wait for enter button to be clicked
        SemaphoreSlim buttonSignal = new SemaphoreSlim(0, 1);
        //Task main;

        bool canceled;

        //Constructor
        public ATMForm(Account[] ac)
        {
            InitializeComponent();
            input.KeyPress += input_KeyPress;
            input.KeyDown += input_KeyDown;

            this.ac = ac;

            run();
        }

        //loops forever to allow program to run
        private async void run()
        {

            activeAccount = null;

            while (true)
            {
                canceled = false;

                activeAccount = await this.findAccount();

                if (activeAccount != null)
                {
                    //if the account is found check the pin 
                    if (activeAccount.checkPin(await this.promptForPin()))
                    {
                        //if the pin is a match give the options to do stuff to the account (take money out, view balance, exit)
                        await dispOptions();
                    }
                }
                else
                {   //if the account number entered is not found let the user know!
                    write("no matching account found.");
                    await buttonSignal.WaitAsync();
                    clear();
                }
            }
        }

        private async Task<Account> findAccount()
        {
            write("Please enter your account number:");

            int input_ = await recieveInput();

            if (canceled)
                return null;

            clear();

            for (int i = 0; i < this.ac.Length; i++)
            {
                Debug.WriteLine(ac[i].getAccountNum());
                if (ac[i].getAccountNum() == input_)
                {
                    return ac[i];
                }
            }

            return null;
        }

        private async Task<int> promptForPin()
        {
            write("enter pin:");

            int pinNumEntered = await recieveInput();

            if (canceled)
                return 0;

            clear();
            return pinNumEntered;
        }

        private async Task dispOptions()
        {
            write("1> take out cash");
            write("2> balance");
            write("3> exit");

            int choice = await recieveInput();

            if (canceled)
                return;

            clear();

            if (choice == 1)
            {
                await dispWithdraw();
            }
            else if (choice == 2)
            {
                await dispBalance();
            }
            else if (choice == 3)
            {


            }
            else
            {

            }


        }

        private async Task dispWithdraw()
        {
            write("1> 10");
            write("2> 50");
            write("3> 500");

            int input = await recieveInput();

            if (canceled)
                return;

            clear();

            if (input > 0 && input < 4)
            {

                //opiton one is entered by the user
                if (input == 1)
                {

                    //attempt to decrement account by 10 punds
                    if (activeAccount.decrementBalance(10))
                    {
                        //if this is possible display new balance and await key press
                        write("new balance " + activeAccount.getBalance());
                        write("(prese enter to continue)");
                        await buttonSignal.WaitAsync();
                    }
                    else
                    {
                        //if this is not possible inform user and await key press
                        write("insufficent funds" + "\n(prese enter to continue)");
                        write(" (prese enter to continue)");
                        await buttonSignal.WaitAsync();
                    }
                }
                else if (input == 2)
                {
                    if (activeAccount.decrementBalance(50))
                    {
                        write("new balance " + activeAccount.getBalance());
                        write(" (prese enter to continue)");
                        await buttonSignal.WaitAsync();
                    }
                    else
                    {
                        write("insufficent funds");
                        write(" (prese enter to continue)");
                        await buttonSignal.WaitAsync();
                    }
                }
                else if (input == 3)
                {
                    if (activeAccount.decrementBalance(500))
                    {
                        write("new balance " + activeAccount.getBalance());
                        write(" (prese enter to continue)");
                        await buttonSignal.WaitAsync();
                    }
                    else
                    {
                        write("insufficent funds");
                        write(" (prese enter to continue)");
                        await buttonSignal.WaitAsync();
                    }
                }

                clear();
            }
        }

        private async Task dispBalance()
        {
            if (this.activeAccount != null)
            {
                //TODO
                write(" your current balance is : " + activeAccount.getBalance());
                write(" (prese enter to continue)");
                await buttonSignal.WaitAsync();

                if (canceled)
                    return;

                clear();
            }
        }

        delegate void SetTextCallback(string text);

        private async Task<int> recieveInput()
        {
            await buttonSignal.WaitAsync();



            if (input.Text.Length == 0)
            {
                return 0;
            }

            try
            {
                int input_ = Convert.ToInt32(input.Text);

                input.Text = "";

                return input_;
            }
            catch (OverflowException)
            {
                input.Text = "";

                return 0;
            }
        }

        //write to output text box
        private void write(string line)
        {
            output.Text = output.Text + "\n" + line;
        }

        //clear output textbox
        private void clear()
        {
            output.Text = "";
        }

        //release button signal on button press
        private async void enter_Click(object sender, EventArgs e)
        {
            try
            {
                buttonSignal.Release();
            }
            catch (ObjectDisposedException)
            {

            }
        }

        //allow only numbers in input
        //https://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers
        private void input_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //enter key also releases button signal
        private async void input_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSignal.Release();

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            canceled = true;

            buttonSignal.Release();

            clear();
        }
    }
}


