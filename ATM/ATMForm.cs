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

        //Checks if the cancel button has been pressed - if it has, return to get account number screen
        bool canceled;
        
        //Constructor
        public ATMForm(Account[] ac)
        {
            InitializeComponent();
            input.KeyPress += input_KeyPress;
            input.KeyDown += input_KeyDown;

            //We will add the keypad_Click event to each numebrical keypad button to make code 
            //neater as we wont need 10 button(num)_Click events that practically do the same thing
            button0.Click += new EventHandler(this.keypad_Click);
            button1.Click += new EventHandler(this.keypad_Click);
            button2.Click += new EventHandler(this.keypad_Click);
            button3.Click += new EventHandler(this.keypad_Click);
            button4.Click += new EventHandler(this.keypad_Click);
            button5.Click += new EventHandler(this.keypad_Click);
            button6.Click += new EventHandler(this.keypad_Click);
            button7.Click += new EventHandler(this.keypad_Click);
            button8.Click += new EventHandler(this.keypad_Click);
            button9.Click += new EventHandler(this.keypad_Click);

            this.ac = ac;

            run();
            
        }

        //loops forever to allow program to run
        private async Task run()
        {
            while (true)
            {
                canceled = false;

                //Get the account number
                activeAccount = await this.findAccount();

                //Check if account is found
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
            output.Text += Environment.NewLine;

            //Waits to recieve input from the user
            int input_ = await recieveInput();

            //Checks if the user's input is the canceled button
            if (canceled)
                return null;

            clear();

            //Check input against all known account numbers to try find a match
            for (int i = 0; i < this.ac.Length; i++)
            {
                if (ac[i].getAccountNum() == input_)
                {
                    return ac[i];
                }
            }

            //no match found
            return null;
        }

        //Prompts the user to enter their PIN
        private async Task<int> promptForPin()
        {
            write("enter pin: ");
            output.Text += Environment.NewLine + Environment.NewLine;
            
            //Wait for user input
            int pinNumEntered = await recieveInput();

            //Checks if cancel button was pressed
            if (canceled)
                return 0;

            clear();
            return pinNumEntered;
        }

        //Displays the meun options to the user
        private async Task dispOptions()
        {
            write("> take out cash");
            write("> balance");
            write("> exit");

            //Wait for user input
            int choice = await recieveInput();

            //Checks if cancel button was pressed
            if (canceled)
                return;

            clear();

            //Checks to see if user input was a listed choice
            if (choice == 1)
                await dispWithdraw();
            else if (choice == 2)
                await dispBalance();
            else if (choice == 3) 
                return;
            else
                return;        
        }

        //Displays the withdrawal amounts to the user
        private async Task dispWithdraw()
        {
            write("> 10");
            write("> 50");
            write("> 500");

            //Wait for user input
            int input = await recieveInput();

            //Checks if user pressed the cancel button
            if (canceled)
                return;

            clear();

            //Makes sure user input is between 1 and 3 (inclusive)
            if (input > 0 && input < 4)
            {
                //opiton one is entered by the user
                if (input == 1)
                {
                    //Wait to get access from semaphore to ensure no race conditions happen
                    Global._access.WaitOne();
                    //attempt to decrement account by 10 punds
                    if (activeAccount.decrementBalance(10))
                    {
                        //if this is possible display new balance and await key press
                        write("new balance " + activeAccount.getBalance());
                        write("(prese enter to continue)");
                        //Now that we have finished any operations involving the banks balance, we can release the semaphore
                        Global._access.Release();
                        await buttonSignal.WaitAsync();
                    }
                    else
                    {
                        //if this is not possible inform user and await key press
                        write("insufficent funds"+ "\n(prese enter to continue)");
                        write(" (prese enter to continue)");
                        //Now that we have finished any operations involving the banks balance, we can release the semaphore
                        Global._access.Release();
                        await buttonSignal.WaitAsync();
                    }                    

                }
                else if (input == 2)
                {
                    //Wait to get access from semaphore to ensure no race conditions happen
                    Global._access.WaitOne();
                    if (activeAccount.decrementBalance(50))
                    {
                        write("new balance " + activeAccount.getBalance());
                        write(" (prese enter to continue)");
                        //Now that we have finished any operations involving the banks balance, we can release the semaphore
                        Global._access.Release();
                        await buttonSignal.WaitAsync();
                    }
                    else
                    {
                        write("insufficent funds");
                        write(" (prese enter to continue)");
                        //Now that we have finished any operations involving the banks balance, we can release the semaphore
                        Global._access.Release();
                        await buttonSignal.WaitAsync();
                    }
                }
                else if (input == 3)
                {
                    //Wait to get access from semaphore to ensure no race conditions happen
                    Global._access.WaitOne();
                    if (activeAccount.decrementBalance(500))
                    {
                        write("new balance " + activeAccount.getBalance());
                        write(" (prese enter to continue)");
                        //Now that we have finished any operations involving the banks balance, we can release the semaphore
                        Global._access.Release();
                        await buttonSignal.WaitAsync();
                    }
                    else
                    {
                        write("insufficent funds");
                        write(" (prese enter to continue)");
                        //Now that we have finished any operations involving the banks balance, we can release the semaphore
                        Global._access.Release();
                        await buttonSignal.WaitAsync();
                    }
                }
                clear();
            }
        }

        //Displays the balance to the user
        private async Task dispBalance()
        {
            //Checks we have a valid account
            if (this.activeAccount != null)
            {
                //TODO
                write(" your current balance is : " + activeAccount.getBalance());
                write(" (prese enter to continue)");
                //Waits for the user to press a button
                await buttonSignal.WaitAsync();

                //Checks if user pressed the cancel button
                if (canceled)
                    return;

                clear();
            }
        }

        //Gets input from the user
        private async Task<int> recieveInput()
        {
            input.Text = "";
            //Waits for the enter button to be pressed
            await buttonSignal.WaitAsync();

            //Checks the hidden input text box isnt empty
            if (input.TextLength == 0)
            {
                return 0;
            }

            //Check if the user input can be parsed as an integer
            if (int.TryParse(input.Text, out int result))
            {
                //Convert it to an int then return the value
                int input_ = Convert.ToInt32(input.Text);
                input.Text = "";
                return input_;
            }
            else
            {
                //User input was not a valid integer
                input.Text = "";
                return 0;
            } 
        }

        //write to output text box
        private void write(string line)
        {
            output.Text += line + "\n";
        }

        //clear output textbox
        private void clear()
        {
            output.Text = "";
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
        private void input_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                buttonSignal.Release();

        }

        private void writeUserInput(string num)
        {
            input.Text += num;
            //This will check if we are on the "enter pin" screen and will
            //Display an asterisk instead of the users pin
            if(output.Text.Substring(0, 5).Equals("enter"))
            {
                output.Text += "*";
            }
            else
            {
                output.Text += num;
            }
        }

        private void ATMForm_Load(object sender, EventArgs e)
        {

        }

        private void keypad_Click(object sender, EventArgs e)
        {
            writeUserInput((sender as Button).Text);
        }

        //These are the side buttons shown next to the screen
        //These are an alternative quick select for menu options
        private void side1_Click(object sender, EventArgs e)
        {
            input.Text = "1";
            enter_Click(sender, new EventArgs());
        }

        private void side2_Click(object sender, EventArgs e)
        {
            input.Text = "2";
            enter_Click(sender, new EventArgs());
        }

        private void side3_Click(object sender, EventArgs e)
        {
            input.Text = "3";
            enter_Click(sender, new EventArgs());
        }

        //Control buttons for the atm interface (enter, clear, cancel)

        //release button signal semaphore on button press
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

        //Removes the last entered number (if there is any)
        private void clearButton_Click(object sender, EventArgs e)
        {
            if(input.Text.Length > 0)
            {
                input.Text = input.Text.Remove(input.Text.Length - 1);
                output.Text = output.Text.Remove(output.Text.Length - 1);
            }
        }

        //Cancel button is used to cease all operations and return back to the main "enter account
        //number" screen
        private void cancelButton_Click(object sender, EventArgs e)
        {
            canceled = true;
            buttonSignal.Release();

            clear();
        }

        private void side4_Click(object sender, EventArgs e)
        {

        }
    }

}
