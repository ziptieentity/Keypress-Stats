namespace KeyPressStats
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            SaveInterval = new System.Windows.Forms.Timer(components);
            Tray = new NotifyIcon(components);
            UpdateTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // SaveInterval
            // 
            SaveInterval.Interval = 30000;
            SaveInterval.Tick += SaveTimerTick;
            // 
            // Tray
            // 
            Tray.BalloonTipIcon = ToolTipIcon.Info;
            Tray.Icon = (Icon)resources.GetObject("Tray.Icon");
            Tray.Text = "Tray";
            Tray.Visible = true;
            // 
            // UpdateTimer
            // 
            UpdateTimer.Interval = 1000;
            UpdateTimer.Tick += UpdateTimerTick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(139, 88);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            FormClosing += OnExit;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer SaveInterval;
        private NotifyIcon Tray;
        private System.Windows.Forms.Timer UpdateTimer;
    }
}