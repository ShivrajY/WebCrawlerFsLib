namespace WebCrawler
{
    partial class MainForm
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            this.fluentDarkTheme1 = new Telerik.WinControls.Themes.FluentDarkTheme();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.totalLinksLabel = new Telerik.WinControls.UI.RadLabelElement();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.radLabelElement2 = new Telerik.WinControls.UI.RadLabelElement();
            this.checkedLinksLabel = new Telerik.WinControls.UI.RadLabelElement();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.logListControl = new Telerik.WinControls.UI.RadListControl();
            this.urlTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.startButton = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.stopButton = new Telerik.WinControls.UI.RadButton();
            this.openFolderButton = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logListControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.urlTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.openFolderButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusStrip1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "Links in the Queue:";
            this.radLabelElement1.TextWrap = true;
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radLabelElement1,
            this.totalLinksLabel,
            this.commandBarSeparator1,
            this.radLabelElement2,
            this.checkedLinksLabel,
            this.radLabelElement1,
            this.totalLinksLabel,
            this.commandBarSeparator1,
            this.radLabelElement2,
            this.checkedLinksLabel});
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 321);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(507, 25);
            this.radStatusStrip1.TabIndex = 0;
            this.radStatusStrip1.ThemeName = "FluentDark";
            // 
            // totalLinksLabel
            // 
            this.totalLinksLabel.Name = "totalLinksLabel";
            this.radStatusStrip1.SetSpring(this.totalLinksLabel, false);
            this.totalLinksLabel.Text = "0";
            this.totalLinksLabel.TextWrap = true;
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.radStatusStrip1.SetSpring(this.commandBarSeparator1, false);
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // radLabelElement2
            // 
            this.radLabelElement2.Name = "radLabelElement2";
            this.radStatusStrip1.SetSpring(this.radLabelElement2, false);
            this.radLabelElement2.Text = "Checked Links:";
            this.radLabelElement2.TextWrap = true;
            // 
            // checkedLinksLabel
            // 
            this.checkedLinksLabel.Name = "checkedLinksLabel";
            this.radStatusStrip1.SetSpring(this.checkedLinksLabel, false);
            this.checkedLinksLabel.Text = "0";
            this.checkedLinksLabel.TextWrap = true;
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGroupBox1.Controls.Add(this.logListControl);
            this.radGroupBox1.HeaderMargin = new System.Windows.Forms.Padding(1);
            this.radGroupBox1.HeaderText = "Log";
            this.radGroupBox1.Location = new System.Drawing.Point(0, 71);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(507, 244);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.Text = "Log";
            this.radGroupBox1.ThemeName = "FluentDark";
            // 
            // logListControl
            // 
            this.logListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListControl.ItemHeight = 24;
            this.logListControl.Items.Add(radListDataItem1);
            this.logListControl.Location = new System.Drawing.Point(2, 18);
            this.logListControl.Name = "logListControl";
            this.logListControl.Size = new System.Drawing.Size(503, 224);
            this.logListControl.TabIndex = 0;
            this.logListControl.ThemeName = "FluentDark";
            this.logListControl.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.logListControl_SelectedIndexChanged);
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(89, 11);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(416, 24);
            this.urlTextBox.TabIndex = 2;
            this.urlTextBox.Text = "https://eclipse2024.org";
            this.urlTextBox.ThemeName = "FluentDark";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(89, 41);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 24);
            this.startButton.TabIndex = 3;
            this.startButton.Text = "Start";
            this.startButton.ThemeName = "FluentDark";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(71, 18);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "Starting Link:";
            this.radLabel1.ThemeName = "FluentDark";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(170, 41);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 24);
            this.stopButton.TabIndex = 3;
            this.stopButton.Text = "Stop";
            this.stopButton.ThemeName = "FluentDark";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // openFolderButton
            // 
            this.openFolderButton.Location = new System.Drawing.Point(371, 41);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(124, 24);
            this.openFolderButton.TabIndex = 3;
            this.openFolderButton.Text = "Open Output Folder";
            this.openFolderButton.ThemeName = "FluentDark";
            this.openFolderButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 346);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.openFolderButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.radStatusStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "WebCrawler Link Checker 1.0";
            this.ThemeName = "FluentDark";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logListControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.urlTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.openFolderButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.Themes.FluentDarkTheme fluentDarkTheme1;
        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.UI.RadLabelElement totalLinksLabel;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement2;
        private Telerik.WinControls.UI.RadLabelElement checkedLinksLabel;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadListControl logListControl;
        private Telerik.WinControls.UI.RadTextBox urlTextBox;
        private Telerik.WinControls.UI.RadButton startButton;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton stopButton;
        private Telerik.WinControls.UI.RadButton openFolderButton;
    }
}