using System.Windows.Forms;

namespace ArcadeConnector
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.WadName = new System.Windows.Forms.TextBox();
            this.lblProcessStatus = new System.Windows.Forms.Label();
            this.lblGameIsHosted = new System.Windows.Forms.Label();
            this.TabMain = new MetroFramework.Controls.MetroTabControl();
            this.tabIRC = new MetroFramework.Controls.MetroTabPage();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.lvUsers = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtNick = new MetroFramework.Controls.MetroTextBox();
            this.btnDisconnect = new MetroFramework.Controls.MetroButton();
            this.btnSendIRC = new MetroFramework.Controls.MetroButton();
            this.txtInput = new MetroFramework.Controls.MetroTextBox();
            this.btnIRCConnect = new MetroFramework.Controls.MetroButton();
            this.tabTest = new MetroFramework.Controls.MetroTabPage();
            this.btnROMsLocation = new MetroFramework.Controls.MetroButton();
            this.txtSaveFilesLocation = new MetroFramework.Controls.MetroTextBox();
            this.lblServerName = new MetroFramework.Controls.MetroLabel();
            this.txtRomsDefaultPath = new MetroFramework.Controls.MetroTextBox();
            this.btnSaveFiles = new MetroFramework.Controls.MetroButton();
            this.txtServerName = new MetroFramework.Controls.MetroTextBox();
            this.btnCSUMELocation = new MetroFramework.Controls.MetroButton();
            this.txtRomPath = new MetroFramework.Controls.MetroTextBox();
            this.btnBrowseRom = new MetroFramework.Controls.MetroButton();
            this.txtCSUMELocation = new MetroFramework.Controls.MetroTextBox();
            this.cmbEngineSelector = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.cmbEngine = new MetroFramework.Controls.MetroComboBox();
            this.txtCMDParameters = new MetroFramework.Controls.MetroTextBox();
            this.btnLaunch = new MetroFramework.Controls.MetroButton();
            this.chkHostTest = new MetroFramework.Controls.MetroCheckBox();
            this.tabHostedServers = new MetroFramework.Controls.MetroTabPage();
            this.dgHostedServer = new MetroFramework.Controls.MetroGrid();
            this.txtChannel = new MetroFramework.Controls.MetroTextBox();
            this.txtPort = new MetroFramework.Controls.MetroTextBox();
            this.txtServer = new MetroFramework.Controls.MetroTextBox();
            this.lblDownloadProgress = new System.Windows.Forms.Label();
            this.contextMenuUser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.readyUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTestIcons = new System.Windows.Forms.Button();
            this.Engine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Addons = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkIRCAutoConnect = new MetroFramework.Controls.MetroCheckBox();
            this.TabMain.SuspendLayout();
            this.tabIRC.SuspendLayout();
            this.tabTest.SuspendLayout();
            this.tabHostedServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgHostedServer)).BeginInit();
            this.contextMenuUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // WadName
            // 
            this.WadName.Location = new System.Drawing.Point(154, 95);
            this.WadName.Multiline = true;
            this.WadName.Name = "WadName";
            this.WadName.Size = new System.Drawing.Size(16, 10);
            this.WadName.TabIndex = 31;
            this.WadName.Visible = false;
            // 
            // lblProcessStatus
            // 
            this.lblProcessStatus.AutoSize = true;
            this.lblProcessStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProcessStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblProcessStatus.Location = new System.Drawing.Point(16, 494);
            this.lblProcessStatus.Name = "lblProcessStatus";
            this.lblProcessStatus.Size = new System.Drawing.Size(117, 13);
            this.lblProcessStatus.TabIndex = 84;
            this.lblProcessStatus.Text = "Process Status info";
            // 
            // lblGameIsHosted
            // 
            this.lblGameIsHosted.AutoSize = true;
            this.lblGameIsHosted.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGameIsHosted.ForeColor = System.Drawing.Color.Teal;
            this.lblGameIsHosted.Location = new System.Drawing.Point(16, 453);
            this.lblGameIsHosted.Name = "lblGameIsHosted";
            this.lblGameIsHosted.Size = new System.Drawing.Size(72, 12);
            this.lblGameIsHosted.TabIndex = 89;
            this.lblGameIsHosted.Text = "HostStatus";
            // 
            // TabMain
            // 
            this.TabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TabMain.Controls.Add(this.tabIRC);
            this.TabMain.Controls.Add(this.tabTest);
            this.TabMain.Controls.Add(this.tabHostedServers);
            this.TabMain.FontWeight = MetroFramework.MetroTabControlWeight.Bold;
            this.TabMain.Location = new System.Drawing.Point(12, 21);
            this.TabMain.Name = "TabMain";
            this.TabMain.SelectedIndex = 0;
            this.TabMain.Size = new System.Drawing.Size(380, 414);
            this.TabMain.Style = MetroFramework.MetroColorStyle.Brown;
            this.TabMain.TabIndex = 99;
            this.TabMain.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.TabMain.UseSelectable = true;
            // 
            // tabIRC
            // 
            this.tabIRC.Controls.Add(this.chkIRCAutoConnect);
            this.tabIRC.Controls.Add(this.txtChat);
            this.tabIRC.Controls.Add(this.lvUsers);
            this.tabIRC.Controls.Add(this.txtNick);
            this.tabIRC.Controls.Add(this.btnDisconnect);
            this.tabIRC.Controls.Add(this.btnSendIRC);
            this.tabIRC.Controls.Add(this.txtInput);
            this.tabIRC.Controls.Add(this.btnIRCConnect);
            this.tabIRC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabIRC.HorizontalScrollbarBarColor = true;
            this.tabIRC.HorizontalScrollbarHighlightOnWheel = false;
            this.tabIRC.HorizontalScrollbarSize = 10;
            this.tabIRC.Location = new System.Drawing.Point(4, 41);
            this.tabIRC.Name = "tabIRC";
            this.tabIRC.Size = new System.Drawing.Size(372, 369);
            this.tabIRC.TabIndex = 5;
            this.tabIRC.Text = "IRC Lobby";
            this.tabIRC.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabIRC.VerticalScrollbarBarColor = true;
            this.tabIRC.VerticalScrollbarHighlightOnWheel = false;
            this.tabIRC.VerticalScrollbarSize = 10;
            // 
            // txtChat
            // 
            this.txtChat.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChat.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChat.ForeColor = System.Drawing.SystemColors.Menu;
            this.txtChat.Location = new System.Drawing.Point(125, 47);
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(242, 289);
            this.txtChat.TabIndex = 141;
            this.txtChat.Text = "";
            this.txtChat.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtChat_MouseDown);
            // 
            // lvUsers
            // 
            this.lvUsers.BackColor = System.Drawing.SystemColors.InfoText;
            this.lvUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvUsers.ForeColor = System.Drawing.Color.MintCream;
            this.lvUsers.FullRowSelect = true;
            this.lvUsers.HideSelection = false;
            this.lvUsers.HoverSelection = true;
            this.lvUsers.Location = new System.Drawing.Point(4, 47);
            this.lvUsers.MultiSelect = false;
            this.lvUsers.Name = "lvUsers";
            this.lvUsers.Size = new System.Drawing.Size(115, 319);
            this.lvUsers.SmallImageList = this.imageList1;
            this.lvUsers.TabIndex = 140;
            this.lvUsers.UseCompatibleStateImageBehavior = false;
            this.lvUsers.View = System.Windows.Forms.View.List;
            this.lvUsers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvUsers_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "online.png");
            this.imageList1.Images.SetKeyName(1, "snooze.png");
            this.imageList1.Images.SetKeyName(2, "ingame.png");
            this.imageList1.Images.SetKeyName(3, "is_moderator.png");
            // 
            // txtNick
            // 
            // 
            // 
            // 
            this.txtNick.CustomButton.Image = null;
            this.txtNick.CustomButton.Location = new System.Drawing.Point(93, 1);
            this.txtNick.CustomButton.Name = "";
            this.txtNick.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtNick.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtNick.CustomButton.TabIndex = 1;
            this.txtNick.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtNick.CustomButton.UseSelectable = true;
            this.txtNick.CustomButton.Visible = false;
            this.txtNick.Lines = new string[0];
            this.txtNick.Location = new System.Drawing.Point(4, 18);
            this.txtNick.MaxLength = 8;
            this.txtNick.Name = "txtNick";
            this.txtNick.PasswordChar = '\0';
            this.txtNick.PromptText = "IRC User Name";
            this.txtNick.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNick.SelectedText = "";
            this.txtNick.SelectionLength = 0;
            this.txtNick.SelectionStart = 0;
            this.txtNick.ShortcutsEnabled = true;
            this.txtNick.Size = new System.Drawing.Size(115, 23);
            this.txtNick.TabIndex = 139;
            this.txtNick.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtNick.UseSelectable = true;
            this.txtNick.WaterMark = "IRC User Name";
            this.txtNick.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtNick.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(295, 17);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(77, 24);
            this.btnDisconnect.TabIndex = 138;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnDisconnect.UseSelectable = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnSendIRC
            // 
            this.btnSendIRC.Location = new System.Drawing.Point(330, 342);
            this.btnSendIRC.Name = "btnSendIRC";
            this.btnSendIRC.Size = new System.Drawing.Size(37, 24);
            this.btnSendIRC.TabIndex = 132;
            this.btnSendIRC.Text = "Send";
            this.btnSendIRC.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSendIRC.UseSelectable = true;
            this.btnSendIRC.Click += new System.EventHandler(this.btnSendIRC_Click);
            // 
            // txtInput
            // 
            this.txtInput.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            // 
            // 
            // 
            this.txtInput.CustomButton.Image = null;
            this.txtInput.CustomButton.Location = new System.Drawing.Point(183, 2);
            this.txtInput.CustomButton.Name = "";
            this.txtInput.CustomButton.Size = new System.Drawing.Size(17, 17);
            this.txtInput.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtInput.CustomButton.TabIndex = 1;
            this.txtInput.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtInput.CustomButton.UseSelectable = true;
            this.txtInput.CustomButton.Visible = false;
            this.txtInput.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.txtInput.Lines = new string[0];
            this.txtInput.Location = new System.Drawing.Point(125, 344);
            this.txtInput.MaxLength = 90000;
            this.txtInput.Name = "txtInput";
            this.txtInput.PasswordChar = '\0';
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtInput.SelectedText = "";
            this.txtInput.SelectionLength = 0;
            this.txtInput.SelectionStart = 0;
            this.txtInput.ShortcutsEnabled = true;
            this.txtInput.Size = new System.Drawing.Size(203, 22);
            this.txtInput.Style = MetroFramework.MetroColorStyle.Silver;
            this.txtInput.TabIndex = 131;
            this.txtInput.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtInput.UseCustomBackColor = true;
            this.txtInput.UseCustomForeColor = true;
            this.txtInput.UseSelectable = true;
            this.txtInput.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtInput.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnIRCConnect
            // 
            this.btnIRCConnect.Location = new System.Drawing.Point(125, 17);
            this.btnIRCConnect.Name = "btnIRCConnect";
            this.btnIRCConnect.Size = new System.Drawing.Size(71, 24);
            this.btnIRCConnect.TabIndex = 117;
            this.btnIRCConnect.Text = "Connect";
            this.btnIRCConnect.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnIRCConnect.UseSelectable = true;
            this.btnIRCConnect.Click += new System.EventHandler(this.btnIRCConnect_Click);
            // 
            // tabTest
            // 
            this.tabTest.Controls.Add(this.btnROMsLocation);
            this.tabTest.Controls.Add(this.txtSaveFilesLocation);
            this.tabTest.Controls.Add(this.lblServerName);
            this.tabTest.Controls.Add(this.txtRomsDefaultPath);
            this.tabTest.Controls.Add(this.btnSaveFiles);
            this.tabTest.Controls.Add(this.txtServerName);
            this.tabTest.Controls.Add(this.btnCSUMELocation);
            this.tabTest.Controls.Add(this.txtRomPath);
            this.tabTest.Controls.Add(this.btnBrowseRom);
            this.tabTest.Controls.Add(this.txtCSUMELocation);
            this.tabTest.Controls.Add(this.cmbEngineSelector);
            this.tabTest.Controls.Add(this.metroLabel2);
            this.tabTest.Controls.Add(this.cmbEngine);
            this.tabTest.Controls.Add(this.WadName);
            this.tabTest.Controls.Add(this.txtCMDParameters);
            this.tabTest.Controls.Add(this.btnLaunch);
            this.tabTest.Controls.Add(this.chkHostTest);
            this.tabTest.HorizontalScrollbarBarColor = true;
            this.tabTest.HorizontalScrollbarHighlightOnWheel = false;
            this.tabTest.HorizontalScrollbarSize = 10;
            this.tabTest.Location = new System.Drawing.Point(4, 41);
            this.tabTest.Name = "tabTest";
            this.tabTest.Size = new System.Drawing.Size(372, 369);
            this.tabTest.TabIndex = 2;
            this.tabTest.Text = "Setup|Launch";
            this.tabTest.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabTest.VerticalScrollbarBarColor = true;
            this.tabTest.VerticalScrollbarHighlightOnWheel = false;
            this.tabTest.VerticalScrollbarSize = 10;
            // 
            // btnROMsLocation
            // 
            this.btnROMsLocation.Location = new System.Drawing.Point(208, 44);
            this.btnROMsLocation.Name = "btnROMsLocation";
            this.btnROMsLocation.Size = new System.Drawing.Size(161, 23);
            this.btnROMsLocation.TabIndex = 124;
            this.btnROMsLocation.Text = "ROMs Location";
            this.btnROMsLocation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnROMsLocation.UseSelectable = true;
            this.btnROMsLocation.Click += new System.EventHandler(this.btnROMsLocation_Click);
            // 
            // txtSaveFilesLocation
            // 
            // 
            // 
            // 
            this.txtSaveFilesLocation.CustomButton.Image = null;
            this.txtSaveFilesLocation.CustomButton.Location = new System.Drawing.Point(162, 1);
            this.txtSaveFilesLocation.CustomButton.Name = "";
            this.txtSaveFilesLocation.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtSaveFilesLocation.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtSaveFilesLocation.CustomButton.TabIndex = 1;
            this.txtSaveFilesLocation.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtSaveFilesLocation.CustomButton.UseSelectable = true;
            this.txtSaveFilesLocation.CustomButton.Visible = false;
            this.txtSaveFilesLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtSaveFilesLocation.Lines = new string[0];
            this.txtSaveFilesLocation.Location = new System.Drawing.Point(2, 343);
            this.txtSaveFilesLocation.MaxLength = 32767;
            this.txtSaveFilesLocation.Name = "txtSaveFilesLocation";
            this.txtSaveFilesLocation.PasswordChar = '\0';
            this.txtSaveFilesLocation.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSaveFilesLocation.SelectedText = "";
            this.txtSaveFilesLocation.SelectionLength = 0;
            this.txtSaveFilesLocation.SelectionStart = 0;
            this.txtSaveFilesLocation.ShortcutsEnabled = true;
            this.txtSaveFilesLocation.Size = new System.Drawing.Size(184, 23);
            this.txtSaveFilesLocation.Style = MetroFramework.MetroColorStyle.Green;
            this.txtSaveFilesLocation.TabIndex = 119;
            this.txtSaveFilesLocation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtSaveFilesLocation.UseCustomForeColor = true;
            this.txtSaveFilesLocation.UseSelectable = true;
            this.txtSaveFilesLocation.Visible = false;
            this.txtSaveFilesLocation.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtSaveFilesLocation.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(3, 238);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(90, 19);
            this.lblServerName.TabIndex = 132;
            this.lblServerName.Text = "Server Name:";
            this.lblServerName.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // txtRomsDefaultPath
            // 
            // 
            // 
            // 
            this.txtRomsDefaultPath.CustomButton.Image = null;
            this.txtRomsDefaultPath.CustomButton.Location = new System.Drawing.Point(169, 1);
            this.txtRomsDefaultPath.CustomButton.Name = "";
            this.txtRomsDefaultPath.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtRomsDefaultPath.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtRomsDefaultPath.CustomButton.TabIndex = 1;
            this.txtRomsDefaultPath.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtRomsDefaultPath.CustomButton.UseSelectable = true;
            this.txtRomsDefaultPath.CustomButton.Visible = false;
            this.txtRomsDefaultPath.Lines = new string[0];
            this.txtRomsDefaultPath.Location = new System.Drawing.Point(3, 44);
            this.txtRomsDefaultPath.MaxLength = 32767;
            this.txtRomsDefaultPath.Name = "txtRomsDefaultPath";
            this.txtRomsDefaultPath.PasswordChar = '\0';
            this.txtRomsDefaultPath.PromptText = "ROMs Location";
            this.txtRomsDefaultPath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtRomsDefaultPath.SelectedText = "";
            this.txtRomsDefaultPath.SelectionLength = 0;
            this.txtRomsDefaultPath.SelectionStart = 0;
            this.txtRomsDefaultPath.ShortcutsEnabled = true;
            this.txtRomsDefaultPath.Size = new System.Drawing.Size(191, 23);
            this.txtRomsDefaultPath.Style = MetroFramework.MetroColorStyle.Orange;
            this.txtRomsDefaultPath.TabIndex = 123;
            this.txtRomsDefaultPath.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtRomsDefaultPath.UseSelectable = true;
            this.txtRomsDefaultPath.WaterMark = "ROMs Location";
            this.txtRomsDefaultPath.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtRomsDefaultPath.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnSaveFiles
            // 
            this.btnSaveFiles.Location = new System.Drawing.Point(199, 343);
            this.btnSaveFiles.Name = "btnSaveFiles";
            this.btnSaveFiles.Size = new System.Drawing.Size(161, 23);
            this.btnSaveFiles.TabIndex = 118;
            this.btnSaveFiles.Text = "File Download Location";
            this.btnSaveFiles.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSaveFiles.UseSelectable = true;
            this.btnSaveFiles.Visible = false;
            this.btnSaveFiles.Click += new System.EventHandler(this.btnSaveFiles_Click);
            // 
            // txtServerName
            // 
            this.txtServerName.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            // 
            // 
            // 
            this.txtServerName.CustomButton.Image = null;
            this.txtServerName.CustomButton.Location = new System.Drawing.Point(180, 1);
            this.txtServerName.CustomButton.Name = "";
            this.txtServerName.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtServerName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtServerName.CustomButton.TabIndex = 1;
            this.txtServerName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtServerName.CustomButton.UseSelectable = true;
            this.txtServerName.CustomButton.Visible = false;
            this.txtServerName.ForeColor = System.Drawing.Color.Gray;
            this.txtServerName.Lines = new string[0];
            this.txtServerName.Location = new System.Drawing.Point(3, 260);
            this.txtServerName.MaxLength = 20;
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.PasswordChar = '\0';
            this.txtServerName.PromptText = "Max 32 Characters";
            this.txtServerName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtServerName.SelectedText = "";
            this.txtServerName.SelectionLength = 0;
            this.txtServerName.SelectionStart = 0;
            this.txtServerName.ShortcutsEnabled = true;
            this.txtServerName.Size = new System.Drawing.Size(202, 23);
            this.txtServerName.Style = MetroFramework.MetroColorStyle.Green;
            this.txtServerName.TabIndex = 131;
            this.txtServerName.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtServerName.UseCustomBackColor = true;
            this.txtServerName.UseCustomForeColor = true;
            this.txtServerName.UseSelectable = true;
            this.txtServerName.WaterMark = "Max 32 Characters";
            this.txtServerName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtServerName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnCSUMELocation
            // 
            this.btnCSUMELocation.Location = new System.Drawing.Point(208, 15);
            this.btnCSUMELocation.Name = "btnCSUMELocation";
            this.btnCSUMELocation.Size = new System.Drawing.Size(161, 23);
            this.btnCSUMELocation.TabIndex = 122;
            this.btnCSUMELocation.Text = "CSUME Location";
            this.btnCSUMELocation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnCSUMELocation.UseSelectable = true;
            this.btnCSUMELocation.Click += new System.EventHandler(this.btnCSUMELocation_Click);
            // 
            // txtRomPath
            // 
            this.txtRomPath.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            // 
            // 
            // 
            this.txtRomPath.CustomButton.Image = null;
            this.txtRomPath.CustomButton.Location = new System.Drawing.Point(180, 1);
            this.txtRomPath.CustomButton.Name = "";
            this.txtRomPath.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtRomPath.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtRomPath.CustomButton.TabIndex = 1;
            this.txtRomPath.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtRomPath.CustomButton.UseSelectable = true;
            this.txtRomPath.CustomButton.Visible = false;
            this.txtRomPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtRomPath.Lines = new string[0];
            this.txtRomPath.Location = new System.Drawing.Point(3, 215);
            this.txtRomPath.MaxLength = 32767;
            this.txtRomPath.Name = "txtRomPath";
            this.txtRomPath.PasswordChar = '\0';
            this.txtRomPath.PromptText = "Add ROM File";
            this.txtRomPath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtRomPath.SelectedText = "";
            this.txtRomPath.SelectionLength = 0;
            this.txtRomPath.SelectionStart = 0;
            this.txtRomPath.ShortcutsEnabled = true;
            this.txtRomPath.Size = new System.Drawing.Size(202, 23);
            this.txtRomPath.Style = MetroFramework.MetroColorStyle.Green;
            this.txtRomPath.TabIndex = 130;
            this.txtRomPath.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtRomPath.UseCustomForeColor = true;
            this.txtRomPath.UseSelectable = true;
            this.txtRomPath.WaterMark = "Add ROM File";
            this.txtRomPath.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtRomPath.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnBrowseRom
            // 
            this.btnBrowseRom.Location = new System.Drawing.Point(223, 215);
            this.btnBrowseRom.Name = "btnBrowseRom";
            this.btnBrowseRom.Size = new System.Drawing.Size(146, 23);
            this.btnBrowseRom.TabIndex = 129;
            this.btnBrowseRom.Text = "Load ROM File";
            this.btnBrowseRom.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnBrowseRom.UseSelectable = true;
            this.btnBrowseRom.Click += new System.EventHandler(this.btnBrowseRom_Click);
            // 
            // txtCSUMELocation
            // 
            // 
            // 
            // 
            this.txtCSUMELocation.CustomButton.Image = null;
            this.txtCSUMELocation.CustomButton.Location = new System.Drawing.Point(169, 1);
            this.txtCSUMELocation.CustomButton.Name = "";
            this.txtCSUMELocation.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtCSUMELocation.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCSUMELocation.CustomButton.TabIndex = 1;
            this.txtCSUMELocation.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCSUMELocation.CustomButton.UseSelectable = true;
            this.txtCSUMELocation.CustomButton.Visible = false;
            this.txtCSUMELocation.Lines = new string[0];
            this.txtCSUMELocation.Location = new System.Drawing.Point(3, 15);
            this.txtCSUMELocation.MaxLength = 32767;
            this.txtCSUMELocation.Name = "txtCSUMELocation";
            this.txtCSUMELocation.PasswordChar = '\0';
            this.txtCSUMELocation.PromptText = "CSUME Engine Location";
            this.txtCSUMELocation.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCSUMELocation.SelectedText = "";
            this.txtCSUMELocation.SelectionLength = 0;
            this.txtCSUMELocation.SelectionStart = 0;
            this.txtCSUMELocation.ShortcutsEnabled = true;
            this.txtCSUMELocation.Size = new System.Drawing.Size(191, 23);
            this.txtCSUMELocation.Style = MetroFramework.MetroColorStyle.Orange;
            this.txtCSUMELocation.TabIndex = 121;
            this.txtCSUMELocation.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtCSUMELocation.UseSelectable = true;
            this.txtCSUMELocation.WaterMark = "CSUME Engine Location";
            this.txtCSUMELocation.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCSUMELocation.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // cmbEngineSelector
            // 
            this.cmbEngineSelector.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cmbEngineSelector.FormattingEnabled = true;
            this.cmbEngineSelector.ItemHeight = 19;
            this.cmbEngineSelector.Items.AddRange(new object[] {
            "CSUME"});
            this.cmbEngineSelector.Location = new System.Drawing.Point(200, 84);
            this.cmbEngineSelector.Name = "cmbEngineSelector";
            this.cmbEngineSelector.Size = new System.Drawing.Size(108, 25);
            this.cmbEngineSelector.Style = MetroFramework.MetroColorStyle.Orange;
            this.cmbEngineSelector.TabIndex = 120;
            this.cmbEngineSelector.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cmbEngineSelector.UseSelectable = true;
            this.cmbEngineSelector.Visible = false;
            this.cmbEngineSelector.SelectedIndexChanged += new System.EventHandler(this.cmbEngineSelector_SelectedIndexChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(3, 95);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(126, 19);
            this.metroLabel2.TabIndex = 121;
            this.metroLabel2.Text = "Current Parameters:";
            this.metroLabel2.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // cmbEngine
            // 
            this.cmbEngine.FontSize = MetroFramework.MetroComboBoxSize.Small;
            this.cmbEngine.FormattingEnabled = true;
            this.cmbEngine.ItemHeight = 19;
            this.cmbEngine.Items.AddRange(new object[] {
            "CSUME"});
            this.cmbEngine.Location = new System.Drawing.Point(317, 84);
            this.cmbEngine.Name = "cmbEngine";
            this.cmbEngine.Size = new System.Drawing.Size(43, 25);
            this.cmbEngine.Style = MetroFramework.MetroColorStyle.Orange;
            this.cmbEngine.TabIndex = 115;
            this.cmbEngine.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.cmbEngine.UseSelectable = true;
            this.cmbEngine.Visible = false;
            // 
            // txtCMDParameters
            // 
            // 
            // 
            // 
            this.txtCMDParameters.CustomButton.Image = null;
            this.txtCMDParameters.CustomButton.Location = new System.Drawing.Point(286, 1);
            this.txtCMDParameters.CustomButton.Name = "";
            this.txtCMDParameters.CustomButton.Size = new System.Drawing.Size(79, 79);
            this.txtCMDParameters.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtCMDParameters.CustomButton.TabIndex = 1;
            this.txtCMDParameters.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtCMDParameters.CustomButton.UseSelectable = true;
            this.txtCMDParameters.CustomButton.Visible = false;
            this.txtCMDParameters.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtCMDParameters.Lines = new string[0];
            this.txtCMDParameters.Location = new System.Drawing.Point(3, 117);
            this.txtCMDParameters.MaxLength = 32767;
            this.txtCMDParameters.Multiline = true;
            this.txtCMDParameters.Name = "txtCMDParameters";
            this.txtCMDParameters.PasswordChar = '\0';
            this.txtCMDParameters.PromptText = "...";
            this.txtCMDParameters.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCMDParameters.SelectedText = "";
            this.txtCMDParameters.SelectionLength = 0;
            this.txtCMDParameters.SelectionStart = 0;
            this.txtCMDParameters.ShortcutsEnabled = true;
            this.txtCMDParameters.Size = new System.Drawing.Size(366, 81);
            this.txtCMDParameters.Style = MetroFramework.MetroColorStyle.Brown;
            this.txtCMDParameters.TabIndex = 120;
            this.txtCMDParameters.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.txtCMDParameters.UseSelectable = true;
            this.txtCMDParameters.WaterMark = "...";
            this.txtCMDParameters.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtCMDParameters.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // btnLaunch
            // 
            this.btnLaunch.Location = new System.Drawing.Point(223, 289);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(146, 48);
            this.btnLaunch.TabIndex = 112;
            this.btnLaunch.Text = "Launch";
            this.btnLaunch.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnLaunch.UseSelectable = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // chkHostTest
            // 
            this.chkHostTest.AutoSize = true;
            this.chkHostTest.Location = new System.Drawing.Point(223, 268);
            this.chkHostTest.Name = "chkHostTest";
            this.chkHostTest.Size = new System.Drawing.Size(82, 15);
            this.chkHostTest.TabIndex = 114;
            this.chkHostTest.Text = "Host Game";
            this.chkHostTest.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkHostTest.UseSelectable = true;
            this.chkHostTest.CheckedChanged += new System.EventHandler(this.chkHostTest_CheckedChanged);
            // 
            // tabHostedServers
            // 
            this.tabHostedServers.Controls.Add(this.dgHostedServer);
            this.tabHostedServers.HorizontalScrollbarBarColor = true;
            this.tabHostedServers.HorizontalScrollbarHighlightOnWheel = false;
            this.tabHostedServers.HorizontalScrollbarSize = 10;
            this.tabHostedServers.Location = new System.Drawing.Point(4, 41);
            this.tabHostedServers.Name = "tabHostedServers";
            this.tabHostedServers.Size = new System.Drawing.Size(372, 369);
            this.tabHostedServers.TabIndex = 3;
            this.tabHostedServers.Text = "Hosted Servers";
            this.tabHostedServers.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabHostedServers.VerticalScrollbarBarColor = true;
            this.tabHostedServers.VerticalScrollbarHighlightOnWheel = false;
            this.tabHostedServers.VerticalScrollbarSize = 10;
            // 
            // dgHostedServer
            // 
            this.dgHostedServer.AllowUserToResizeRows = false;
            this.dgHostedServer.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.dgHostedServer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgHostedServer.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgHostedServer.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(81)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgHostedServer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgHostedServer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgHostedServer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Engine,
            this.ServerName,
            this.IPAddress,
            this.Port,
            this.Addons});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgHostedServer.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgHostedServer.EnableHeadersVisualStyles = false;
            this.dgHostedServer.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgHostedServer.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.dgHostedServer.Location = new System.Drawing.Point(3, 12);
            this.dgHostedServer.Name = "dgHostedServer";
            this.dgHostedServer.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(81)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgHostedServer.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgHostedServer.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgHostedServer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgHostedServer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgHostedServer.Size = new System.Drawing.Size(367, 297);
            this.dgHostedServer.Style = MetroFramework.MetroColorStyle.Brown;
            this.dgHostedServer.TabIndex = 129;
            this.dgHostedServer.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.dgHostedServer.UseCustomBackColor = true;
            this.dgHostedServer.UseCustomForeColor = true;
            this.dgHostedServer.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgHostedServer_CellDoubleClick);
            // 
            // txtChannel
            // 
            // 
            // 
            // 
            this.txtChannel.CustomButton.Image = null;
            this.txtChannel.CustomButton.Location = new System.Drawing.Point(-8, 1);
            this.txtChannel.CustomButton.Name = "";
            this.txtChannel.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtChannel.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtChannel.CustomButton.TabIndex = 1;
            this.txtChannel.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtChannel.CustomButton.UseSelectable = true;
            this.txtChannel.CustomButton.Visible = false;
            this.txtChannel.Lines = new string[] {
        "#ArcadeConnector"};
            this.txtChannel.Location = new System.Drawing.Point(129, 481);
            this.txtChannel.MaxLength = 32767;
            this.txtChannel.Name = "txtChannel";
            this.txtChannel.PasswordChar = '\0';
            this.txtChannel.PromptText = "Channel";
            this.txtChannel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtChannel.SelectedText = "";
            this.txtChannel.SelectionLength = 0;
            this.txtChannel.SelectionStart = 0;
            this.txtChannel.ShortcutsEnabled = true;
            this.txtChannel.Size = new System.Drawing.Size(14, 23);
            this.txtChannel.TabIndex = 137;
            this.txtChannel.Text = "#ArcadeConnector";
            this.txtChannel.UseSelectable = true;
            this.txtChannel.Visible = false;
            this.txtChannel.WaterMark = "Channel";
            this.txtChannel.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtChannel.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtPort
            // 
            // 
            // 
            // 
            this.txtPort.CustomButton.Image = null;
            this.txtPort.CustomButton.Location = new System.Drawing.Point(-3, 1);
            this.txtPort.CustomButton.Name = "";
            this.txtPort.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtPort.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPort.CustomButton.TabIndex = 1;
            this.txtPort.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPort.CustomButton.UseSelectable = true;
            this.txtPort.CustomButton.Visible = false;
            this.txtPort.Lines = new string[] {
        "6667"};
            this.txtPort.Location = new System.Drawing.Point(104, 481);
            this.txtPort.MaxLength = 32767;
            this.txtPort.Name = "txtPort";
            this.txtPort.PasswordChar = '\0';
            this.txtPort.PromptText = "Port";
            this.txtPort.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPort.SelectedText = "";
            this.txtPort.SelectionLength = 0;
            this.txtPort.SelectionStart = 0;
            this.txtPort.ShortcutsEnabled = true;
            this.txtPort.Size = new System.Drawing.Size(19, 23);
            this.txtPort.TabIndex = 136;
            this.txtPort.Text = "6667";
            this.txtPort.UseSelectable = true;
            this.txtPort.Visible = false;
            this.txtPort.WaterMark = "Port";
            this.txtPort.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPort.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtServer
            // 
            // 
            // 
            // 
            this.txtServer.CustomButton.Image = null;
            this.txtServer.CustomButton.Location = new System.Drawing.Point(64, 1);
            this.txtServer.CustomButton.Name = "";
            this.txtServer.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtServer.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtServer.CustomButton.TabIndex = 1;
            this.txtServer.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtServer.CustomButton.UseSelectable = true;
            this.txtServer.CustomButton.Visible = false;
            this.txtServer.Lines = new string[] {
        "irc.libera.chat"};
            this.txtServer.Location = new System.Drawing.Point(12, 481);
            this.txtServer.MaxLength = 32767;
            this.txtServer.Name = "txtServer";
            this.txtServer.PasswordChar = '\0';
            this.txtServer.PromptText = "Server";
            this.txtServer.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtServer.SelectedText = "";
            this.txtServer.SelectionLength = 0;
            this.txtServer.SelectionStart = 0;
            this.txtServer.ShortcutsEnabled = true;
            this.txtServer.Size = new System.Drawing.Size(86, 23);
            this.txtServer.TabIndex = 135;
            this.txtServer.Text = "irc.libera.chat";
            this.txtServer.UseSelectable = true;
            this.txtServer.Visible = false;
            this.txtServer.WaterMark = "Server";
            this.txtServer.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtServer.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // lblDownloadProgress
            // 
            this.lblDownloadProgress.AutoSize = true;
            this.lblDownloadProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloadProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblDownloadProgress.Location = new System.Drawing.Point(16, 481);
            this.lblDownloadProgress.Name = "lblDownloadProgress";
            this.lblDownloadProgress.Size = new System.Drawing.Size(43, 13);
            this.lblDownloadProgress.TabIndex = 100;
            this.lblDownloadProgress.Text = "Status";
            // 
            // contextMenuUser
            // 
            this.contextMenuUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.contextMenuUser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.readyUpToolStripMenuItem});
            this.contextMenuUser.Name = "contextMenuUser";
            this.contextMenuUser.Size = new System.Drawing.Size(135, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItem1.Text = "View Status";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.viewStatusToolStripMenuItem_Click);
            // 
            // readyUpToolStripMenuItem
            // 
            this.readyUpToolStripMenuItem.CheckOnClick = true;
            this.readyUpToolStripMenuItem.Image = global::ArcadeConnector.Properties.Resources.ready;
            this.readyUpToolStripMenuItem.Name = "readyUpToolStripMenuItem";
            this.readyUpToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.readyUpToolStripMenuItem.Text = "Ready Up";
            this.readyUpToolStripMenuItem.Click += new System.EventHandler(this.readyUpToolStripMenuItem_Click);
            // 
            // btnTestIcons
            // 
            this.btnTestIcons.Location = new System.Drawing.Point(315, 442);
            this.btnTestIcons.Name = "btnTestIcons";
            this.btnTestIcons.Size = new System.Drawing.Size(70, 23);
            this.btnTestIcons.TabIndex = 101;
            this.btnTestIcons.Text = "test icons";
            this.btnTestIcons.UseVisualStyleBackColor = true;
            this.btnTestIcons.Visible = false;
            this.btnTestIcons.Click += new System.EventHandler(this.btnTestIcons_Click);
            // 
            // Engine
            // 
            this.Engine.FillWeight = 80F;
            this.Engine.HeaderText = "Engine";
            this.Engine.Name = "Engine";
            this.Engine.Width = 80;
            // 
            // ServerName
            // 
            this.ServerName.HeaderText = "Server Name";
            this.ServerName.Name = "ServerName";
            this.ServerName.Width = 180;
            // 
            // IPAddress
            // 
            this.IPAddress.HeaderText = "IP";
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Visible = false;
            this.IPAddress.Width = 80;
            // 
            // Port
            // 
            this.Port.HeaderText = "Port";
            this.Port.Name = "Port";
            this.Port.Visible = false;
            this.Port.Width = 60;
            // 
            // Addons
            // 
            this.Addons.HeaderText = "ROM";
            this.Addons.Name = "Addons";
            this.Addons.Width = 80;
            // 
            // chkIRCAutoConnect
            // 
            this.chkIRCAutoConnect.AutoSize = true;
            this.chkIRCAutoConnect.Location = new System.Drawing.Point(4, 0);
            this.chkIRCAutoConnect.Name = "chkIRCAutoConnect";
            this.chkIRCAutoConnect.Size = new System.Drawing.Size(100, 15);
            this.chkIRCAutoConnect.TabIndex = 142;
            this.chkIRCAutoConnect.Text = "Auto Connect ";
            this.chkIRCAutoConnect.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.chkIRCAutoConnect.UseSelectable = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.ClientSize = new System.Drawing.Size(402, 485);
            this.Controls.Add(this.btnTestIcons);
            this.Controls.Add(this.lblDownloadProgress);
            this.Controls.Add(this.TabMain);
            this.Controls.Add(this.lblGameIsHosted);
            this.Controls.Add(this.lblProcessStatus);
            this.Controls.Add(this.txtChannel);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.txtPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Arcade Connector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TabMain.ResumeLayout(false);
            this.tabIRC.ResumeLayout(false);
            this.tabIRC.PerformLayout();
            this.tabTest.ResumeLayout(false);
            this.tabTest.PerformLayout();
            this.tabHostedServers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgHostedServer)).EndInit();
            this.contextMenuUser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox WadName;
        private System.Windows.Forms.Label lblProcessStatus;
        private System.Windows.Forms.Label lblGameIsHosted;
        private MetroFramework.Controls.MetroTabControl TabMain;
        private MetroFramework.Controls.MetroComboBox cmbEngine;
        private MetroFramework.Controls.MetroButton btnLaunch;
        private MetroFramework.Controls.MetroCheckBox chkHostTest;
        private MetroFramework.Controls.MetroTextBox txtCMDParameters;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTabPage tabTest;
        private MetroFramework.Controls.MetroGrid dgHostedServer;
        private MetroFramework.Controls.MetroTabPage tabHostedServers;
        private MetroFramework.Controls.MetroComboBox cmbEngineSelector;
        private System.Windows.Forms.Label lblDownloadProgress;
        private MetroFramework.Controls.MetroTabPage tabIRC;
        private MetroFramework.Controls.MetroButton btnIRCConnect;
        private MetroFramework.Controls.MetroTextBox txtInput;
        private MetroFramework.Controls.MetroButton btnSendIRC;
        private MetroFramework.Controls.MetroTextBox txtPort;
        private MetroFramework.Controls.MetroTextBox txtServer;
        private MetroFramework.Controls.MetroTextBox txtChannel;
        private MetroFramework.Controls.MetroButton btnDisconnect;
        private MetroFramework.Controls.MetroTextBox txtNick;
        private System.Windows.Forms.ListView lvUsers;
        private System.Windows.Forms.RichTextBox txtChat;
        private ImageList imageList1;
        private ContextMenuStrip contextMenuUser;
        private ToolStripMenuItem toolStripMenuItem1;
        private Button btnTestIcons;
        private MetroFramework.Controls.MetroButton btnCSUMELocation;
        private MetroFramework.Controls.MetroTextBox txtCSUMELocation;
        private MetroFramework.Controls.MetroTextBox txtRomPath;
        private MetroFramework.Controls.MetroButton btnBrowseRom;
        private MetroFramework.Controls.MetroButton btnROMsLocation;
        private MetroFramework.Controls.MetroTextBox txtRomsDefaultPath;
        private MetroFramework.Controls.MetroTextBox txtServerName;
        private MetroFramework.Controls.MetroLabel lblServerName;
        private MetroFramework.Controls.MetroTextBox txtSaveFilesLocation;
        private MetroFramework.Controls.MetroButton btnSaveFiles;
        private ToolStripMenuItem readyUpToolStripMenuItem;
        private DataGridViewTextBoxColumn Engine;
        private DataGridViewTextBoxColumn ServerName;
        private DataGridViewTextBoxColumn IPAddress;
        private DataGridViewTextBoxColumn Port;
        private DataGridViewTextBoxColumn Addons;
        private MetroFramework.Controls.MetroCheckBox chkIRCAutoConnect;
    }
}

