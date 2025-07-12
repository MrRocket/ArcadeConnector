using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcadeConnector
{
    public partial class UserStatusForm : Form
    {
        public event Action<string, string> ProfilePictureChanged;


        public UserStatusForm(string nick, string onlineStr, string afkStr, Image profileImage = null, string localUser = null)
        {
            InitializeComponent();

            lblUsername.Text = $"User: {nick}";
            lblOnlineTime.Text = $"Online: {onlineStr}";
            lblAfkTime.Text = $"AFK: {afkStr}";
            pbProfile.Image = profileImage ?? Properties.Resources.DefaultProfile;

            // 🛡️ Only show the change button if the viewer == profile owner
            if (!string.Equals(nick, localUser, StringComparison.OrdinalIgnoreCase))
            {
                btnChangePicture.Visible = false;
            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void btnChangePicture_Click(object sender, EventArgs e)
        {
            string picDir = Path.Combine(Application.StartupPath, "ProfilePic");
            Directory.CreateDirectory(picDir);

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = picDir;
                dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the image safely without locking the file
                        Image selectedImage;
                        using (var fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                        using (var ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            ms.Position = 0;
                            selectedImage = Image.FromStream(ms);
                        }

                        // Resize for consistency
                        Image resized = new Bitmap(selectedImage, new Size(128, 128));
                        pbProfile.Image = resized;

                        // Save locally
                        string nick = lblUsername.Text.Replace("User: ", "").Trim();
                        string savePath = Path.Combine(picDir, nick + ".png");
                        resized.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

                        // 🔁 Notify main form to broadcast this profile picture
                        ProfilePictureChanged?.Invoke(nick, savePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load or save image.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UserStatusForm_Load(object sender, EventArgs e)
        {

        }





        ////private void BroadcastProfilePicture(string nick, string imagePath)
        ////{
        ////    if (!File.Exists(imagePath))
        ////        return;

        ////    byte[] imageData = File.ReadAllBytes(imagePath);

        ////    // TODO: Send over custom socket protocol — below is pseudocode.
        ////    var packet = new CustomPacket
        ////    {
        ////        Type = "PROFILE_PIC",
        ////        Nickname = nick,
        ////        Data = imageData
        ////    };

        ////    SendToConnectedClients(packet); // Your custom method
        ////}


    }
}
