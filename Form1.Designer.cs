namespace Contentsafety_app
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtInput = new TextBox();
            rtbChatHistory = new RichTextBox();
            SuspendLayout();
            // 
            // txtInput
            // 
            txtInput.Location = new Point(87, 48);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(1866, 65);
            txtInput.TabIndex = 0;
            txtInput.KeyDown += textBoxInput_KeyDown;
            // 
            // rtbChatHistory
            // 
            rtbChatHistory.Location = new Point(87, 190);
            rtbChatHistory.Name = "rtbChatHistory";
            rtbChatHistory.Size = new Size(1866, 974);
            rtbChatHistory.TabIndex = 5;
            rtbChatHistory.Text = "";
            rtbChatHistory.TextChanged += rtbChatHistory_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(24F, 59F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2013, 1239);
            Controls.Add(rtbChatHistory);
            Controls.Add(txtInput);
            Font = new Font("Segoe UI", 16.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(6);
            Name = "Form1";
            Text = "ContentSafety";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtInput;
        private RichTextBox rtbChatHistory;
    }
}
