using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM
{
    public partial class ATMMenu : Form
    {
        private Account[] ac = new Account[3];

        public ATMMenu()
        {
            InitializeComponent();
            //Create 3 accounts
            
            ac[0] = new Account(300, 1111, 111111);
            ac[1] = new Account(750, 2222, 222222);
            ac[2] = new Account(3000, 3333, 333333);
        }

        private void ATMMenu_Update(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("ATM Simulator by Struan Robertson, Jack Glen and Khaelem Watt. Assignment 3: ATM Simulator, 2021", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void howToStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Choose one of the 3 options\n\n1: Race Condition " +
                "- This is the ATM simulator that will simulate a race condition. " +
                "The accessing of the account's balance has been slowed down to give the user time to withdraw money from the same account from two " +
                "different ATM's at the same time. When withdrawing an amount that is more than half the users balance, " +
                "it will go into the negatives (i.e. if you withdraw £500 twice from an account that has £750, then their balance will end up £-250.\n\n" +
                "2: Semaphore Fix - This ATM fixes the race condition mentioned above by using a semaphore to restrict access to the account's balance.\n\n" +
                "3: Lock Fix - This ATM uses a Lock to prevent access to the account's balance", "How to", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int mode = 0;
            string strMode = modeList.GetItemText(modeList.SelectedItem);
            if (strMode.Equals(""))
            {
                DialogResult result;
                result = MessageBox.Show("You must select an ATM Mode to start ", "Selection Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (strMode.Equals("Race Condition"))
                    mode = 0;
                if (strMode.Equals("Semaphore"))
                    mode = 1;

                //Create 2 ATM windows and create a thread for each
                for (int i = 0; i < 2; i++)
                {
                    Thread _thread = new Thread(() =>
                    {
                        Application.Run(new ATMForm(ac, mode));
                    });
                    _thread.SetApartmentState(ApartmentState.STA);
                    _thread.Start();
                }
            }
        }        
    }
}
