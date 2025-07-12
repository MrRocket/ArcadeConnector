namespace ArcadeConnector
{
    partial class UserStatusForm
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
            this.pbProfile = new System.Windows.Forms.PictureBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblOnlineTime = new System.Windows.Forms.Label();
            this.lblAfkTime = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnChangePicture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // pbProfile
            // 
            this.pbProfile.Location = new System.Drawing.Point(12, 12);
            this.pbProfile.Name = "pbProfile";
            this.pbProfile.Size = new System.Drawing.Size(120, 105);
            this.pbProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbProfile.TabIndex = 0;
            this.pbProfile.TabStop = false;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(161, 21);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(35, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "label1";
            // 
            // lblOnlineTime
            // 
            this.lblOnlineTime.AutoSize = true;
            this.lblOnlineTime.Location = new System.Drawing.Point(161, 56);
            this.lblOnlineTime.Name = "lblOnlineTime";
            this.lblOnlineTime.Size = new System.Drawing.Size(35, 13);
            this.lblOnlineTime.TabIndex = 2;
            this.lblOnlineTime.Text = "label2";
            // 
            // lblAfkTime
            // 
            this.lblAfkTime.AutoSize = true;
            this.lblAfkTime.Location = new System.Drawing.Point(161, 92);
            this.lblAfkTime.Name = "lblAfkTime";
            this.lblAfkTime.Size = new System.Drawing.Size(35, 13);
            this.lblAfkTime.TabIndex = 3;
            this.lblAfkTime.Text = "label3";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(164, 123);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 32);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Whatever";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnChangePicture
            // 
            this.btnChangePicture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnChangePicture.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnChangePicture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePicture.Location = new System.Drawing.Point(12, 123);
            this.btnChangePicture.Name = "btnChangePicture";
            this.btnChangePicture.Size = new System.Drawing.Size(120, 32);
            this.btnChangePicture.TabIndex = 5;
            this.btnChangePicture.Text = "Change Profile Image";
            this.btnChangePicture.UseVisualStyleBackColor = false;
            this.btnChangePicture.Click += new System.EventHandler(this.btnChangePicture_Click);
            // 
            // UserStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(283, 174);
            this.ControlBox = false;
            this.Controls.Add(this.btnChangePicture);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblAfkTime);
            this.Controls.Add(this.lblOnlineTime);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.pbProfile);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Information";
            this.Load += new System.EventHandler(this.UserStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbProfile;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblOnlineTime;
        private System.Windows.Forms.Label lblAfkTime;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnChangePicture;
    }
}