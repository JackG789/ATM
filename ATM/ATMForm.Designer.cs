namespace ATM
{
    partial class ATMForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.input = new System.Windows.Forms.TextBox();
            this.enter = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(96, 272);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(173, 26);
            this.input.TabIndex = 1;
            // 
            // enter
            // 
            this.enter.BackColor = System.Drawing.Color.LightGreen;
            this.enter.Location = new System.Drawing.Point(309, 262);
            this.enter.Name = "enter";
            this.enter.Size = new System.Drawing.Size(85, 46);
            this.enter.TabIndex = 2;
            this.enter.Text = "enter";
            this.enter.UseVisualStyleBackColor = false;
            this.enter.Click += new System.EventHandler(this.enter_Click);
            // 
            // output
            // 
            this.output.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.output.DetectUrls = false;
            this.output.Font = new System.Drawing.Font("Gill Sans MT", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.Location = new System.Drawing.Point(51, 45);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.output.Size = new System.Drawing.Size(409, 184);
            this.output.TabIndex = 0;
            this.output.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(413, 275);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ATMForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(509, 358);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.enter);
            this.Controls.Add(this.input);
            this.Controls.Add(this.output);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ATMForm";
            this.Text = "ATM";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Button enter;
        private System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.Button button1;
    }
}

