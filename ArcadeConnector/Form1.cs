using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using IrcErrorEventArgs = Meebey.SmartIrc4net.ErrorEventArgs;

// ===========================================================================

// ==  Arcade Connector was created by Mr.Rocket aka Ron Goode ~ 7-11-2025  ==

// ===========================================================================



namespace ArcadeConnector
{
    public partial class Form1 : Form
    {
    
        private System.Windows.Forms.Timer _hostBroadcastTimer;
        private Process _serverProcess;
        private bool _isHosting = false;
        private System.Windows.Forms.Timer _hostPresenceTimer;
        private string _currentHostSessionId = null; // On the client
        private string _hostSessionId = null;        // On the host
        private string _endedHostSessionId = null;
        private string _lastHostedEngine = "";

        //IRC client 
        private readonly IrcClient _irc = new IrcClient();
        private Thread _listenThread;
        private readonly SoundPlayer _msgPlayer; // login sound
        private SoundPlayer _cheerPlayer;
        private SoundPlayer _msgDisconnect;
        private SoundPlayer _msgHostSound;
        private readonly List<string> _messageHistory = new List<string>();
        private int _historyIndex = -1;

        private Dictionary<string, SoundPlayer> _customSounds = new Dictionary<string, SoundPlayer>(StringComparer.OrdinalIgnoreCase);
   //     private bool _isAfk = false;
        private readonly HashSet<string> _afkUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private System.Timers.Timer _idleTimer;
        private TimeSpan _afkTimeout = TimeSpan.FromMinutes(10);
        private readonly Dictionary<string, DateTime> _userJoinTimes = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, DateTime> _userAfkTimestamps = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
   //     private bool _userInitiatedDisconnect = false;
        private System.Windows.Forms.Timer _connectionCheckTimer;
        private System.Windows.Forms.Timer _pingTimer;
   //     private System.Windows.Forms.Timer _ghostCheckTimer;
        private readonly Dictionary<string, string> _userStatus = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // anti-spam system

        private readonly Dictionary<string, Queue<DateTime>> _userMessageTimestamps = new Dictionary<string, Queue<DateTime>>();
        private readonly HashSet<string> _warnedUsers = new HashSet<string>();
        private const int SpamThreshold = 5;
        private readonly TimeSpan SpamWindow = TimeSpan.FromSeconds(10);

        // auto join
        private HashSet<string> _readyUsers = new HashSet<string>();
        private string _lastJoinLink = null;

        // end IRC 

        private List<HostedServerEntry> _hostedServerList = new List<HostedServerEntry>();

        class HostedServerEntry
        {
            public string IP;
            public int Port;
            public string Engine;
            public List<string> Addons;
            public DateTime LastSeen; 
        }


        public Form1()
        {
            InitializeComponent();

            // IRC
            _irc.OnConnected += Irc_OnConnected;
            _irc.OnChannelMessage += Irc_OnChannelMessage;
            _irc.OnError += Irc_OnError;

            // extra events
            _irc.OnJoin += Irc_OnJoin;
            _irc.OnPart += Irc_OnPart;
            _irc.OnNames += Irc_OnNames;

            _irc.OnQuit += Irc_OnQuit;
            _irc.OnNickChange += Irc_OnNickChange;

            this.AcceptButton = btnSendIRC;
            txtInput.KeyDown += txtInput_KeyDown;

            // sound events 
            var soundPath = Path.Combine(Application.StartupPath, "Sounds", "pop.wav");
            if (!File.Exists(soundPath))
            {
                AppendChatLog($"[ERROR] Sound file not found at: {soundPath}", Color.Red);
            }
            else
            {
                try
                {
                    _msgPlayer = new SoundPlayer(soundPath);
                    _msgPlayer.Load(); // synchronous load to catch bad formats
                                          
                }
                catch (Exception ex)
                {
                    AppendChatLog($"[ERROR] Failed to load sound: {ex.Message}", Color.Red);
                }
            }

            string cheerPath = Path.Combine(Application.StartupPath, "Sounds", "cheer.wav");
            if (File.Exists(cheerPath))
            {
                _cheerPlayer = new SoundPlayer(cheerPath);
                _cheerPlayer.Load(); // preload it
            }

            string DiscoPath = Path.Combine(Application.StartupPath, "Sounds", "beback.wav");
            if (File.Exists(DiscoPath))
            {
                _msgDisconnect = new SoundPlayer(DiscoPath);
                _msgDisconnect.Load(); 
            }

            string HostSound = Path.Combine(Application.StartupPath, "Sounds", "beep.wav");
            if (File.Exists(HostSound))
            {
                _msgHostSound = new SoundPlayer(HostSound);
                _msgHostSound.Load(); 
            }
            // ~

        }



        private void StaleServerCleanup_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            var stale = _hostedServerList
                .Where(entry => (now - entry.LastSeen).TotalSeconds > 15)
                .ToList();

            foreach (var entry in stale)
            {
                string key = entry.IP + ":" + entry.Port;

                _hostedServerList.Remove(entry); // Remove from memory
                RemoveServerFromDataGrid(entry); // Remove from UI
                UpdateHostedServerGridEnabled(); // Refresh controls
            }
        }


        private void RemoveServerFromDataGrid(HostedServerEntry entry)
        {
            foreach (DataGridViewRow row in dgHostedServer.Rows)
            {
                string ip = row.Cells["IPAddress"].Value?.ToString();
                string port = row.Cells["Port"].Value?.ToString();

                if (ip == entry.IP && port == entry.Port.ToString())
                {
                    dgHostedServer.Rows.Remove(row);
                    break;
                }
            }
        }


        private void UpdateCommandLine()
        {
            string enginePath = "";
            if (cmbEngine.SelectedItem != null)
            {
                switch (cmbEngine.SelectedItem.ToString())
                {
                    case "CSUME":
                        enginePath = txtCSUMELocation.Text;
                        break;

                }
            }

            string host = "";
            if (chkHostTest.Checked == true)
            {
                host = " -server ";
            }

            if (chkHostTest.Checked == false)
            {
                host = "";
            }

            string romfile = "";
            if(txtRomPath.Text != null)
            {
                romfile = txtRomPath.Text;
            }



            txtCMDParameters.Text = $"{enginePath} {host} {romfile}".Trim();
        }


        private void HandleServerExited()
        {
            string nick = txtNick.Text.Trim();

            // Send BACK IRC message so other clients can update
            if (_irc != null && _irc.IsConnected)
            {
                string backMsg = $"[BACK:{nick}]";
                _irc.RfcPrivmsg(txtChannel.Text, backMsg);

                // Also process it locally so host updates his own view
                ParseIrcMessageLocally($"[BACK:{nick}]");

            }

            _isHosting = false;
            _userStatus[nick] = "online";
            SetUserInGame(nick, false);  // ✅ Sets icon to online.png
            SetUserAfk(nick, false);     // In case AFK was triggered

            this.Invoke((MethodInvoker)(() =>
            {
                _hostBroadcastTimer?.Stop();
                _hostBroadcastTimer?.Dispose();
                _hostBroadcastTimer = null;

                lblGameIsHosted.Text = "";
                UpdateUserIcon(nick); // ✅ Local icon (redundant but safe)

                // Remove own server from grid if still there
                for (int i = dgHostedServer.Rows.Count - 1; i >= 0; i--)
                {
                    var row = dgHostedServer.Rows[i];
                    string engineVal = row.Cells["Engine"].Value?.ToString();
                    string portVal = row.Cells["Port"].Value?.ToString();
                    string ipVal = row.Cells["IPAddress"].Value?.ToString();

                    if (engineVal == "CSUME" && portVal == "5029" && ipVal == GetLocalIPAddress())
                    {
                        dgHostedServer.Rows.RemoveAt(i);
                        break;
                    }
                }

                // Clear server from other clients
                BroadcastHostInfo("", new List<string>(), "");
            }));
        }

        private void ParseIrcMessageLocally(string msg)
        {
            string nick = txtNick.Text;

            if (msg.StartsWith("[BACK:", StringComparison.OrdinalIgnoreCase))
            {
                string backUser = msg.Substring(6, msg.Length - 7);
                string backKey = backUser.ToLowerInvariant();

                _userAfkTimestamps.Remove(backKey);
                _afkUsers.Remove(backUser);
                SetUserAfk(backUser, false);
                SetUserInGame(backUser, false);
                UpdateUserIcon(backUser);
                return;
            }

            // ~ this could be expanded..
        }


        private void btnLaunch_Click(object sender, EventArgs e)
        {
            string gameExecutable = "";

            if (cmbEngine.SelectedItem.ToString() == "CSUME")
            {
                gameExecutable = txtCSUMELocation.Text;
            }

            if (string.IsNullOrWhiteSpace(gameExecutable) || !File.Exists(gameExecutable))
            {
                MessageBox.Show("Please select a valid game executable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Gather hosted file
            string servername = txtServerName.Text.Trim();
            if (string.IsNullOrWhiteSpace(servername))
            {
                txtServerName.Text = "Hosted from [Arcade Connector!]";
                servername = txtServerName.Text;
            }

            string Host = "", engineName = cmbEngine.SelectedItem.ToString();

            string cmdParams = txtCMDParameters.Text.Trim();
            if (!string.IsNullOrEmpty(gameExecutable) && cmdParams.StartsWith(gameExecutable, StringComparison.OrdinalIgnoreCase))
                cmdParams = cmdParams.Substring(gameExecutable.Length).TrimStart();

            string GameArgs = ""; 

            string port = (engineName == "CSUME") ? "5029" : "";

            string serverArgs = $"{Host} {GameArgs}";


            if (chkHostTest.Checked == true)
            {
                _isHosting = true;
                string nick = txtNick.Text.Trim();
                _userStatus[nick] = "ingame";
                UpdateUserIcon(nick);
                UpdateLaunchButtonEnabled();

                if (_hostBroadcastTimer != null)
                {
                    _hostBroadcastTimer.Stop();
                    _hostBroadcastTimer.Dispose();
                }

                _hostBroadcastTimer = null;
                _hostSessionId = Guid.NewGuid().ToString();

                // === CSUME Launch ===
                if (engineName == "CSUME")
                {
                    string romPath = txtRomPath.Text.Trim();
                    if (string.IsNullOrEmpty(romPath) || !File.Exists(romPath) || !romPath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Please select a valid ROM (.zip) file for CSUME.", "ROM Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string romFileName = Path.GetFileName(romPath);
                    serverArgs = $"\"{romPath}\" -server -port {port}";
                    string workingDir = Path.GetDirectoryName(gameExecutable);

                    var serverPsi = new ProcessStartInfo(gameExecutable, serverArgs)
                    {
                        UseShellExecute = true,
                        WorkingDirectory = workingDir,
                        WindowStyle = ProcessWindowStyle.Normal
                    };

                    Debug.WriteLine("Launching CSUME server with:");
                    Debug.WriteLine("Executable: " + gameExecutable);
                    Debug.WriteLine("Arguments : " + serverArgs);
                    Debug.WriteLine("Working Dir: " + workingDir);

                    try
                    {
                        _serverProcess = Process.Start(serverPsi);
                        TabMain.SelectedTab = tabHostedServers;

                        _isHosting = true;
                        _userStatus[nick] = "ingame";
                        SetUserInGame(nick, true);
                        UpdateUserIcon(nick);
                        UpdateLaunchButtonEnabled();

                        string gameStatus = $"Heartbeat detected a {engineName} game hosted!";
                        lblGameIsHosted.Text = gameStatus;

                        // Send to all clients via IRC
                        if (_irc != null && _irc.IsConnected)
                        {
                            _irc.RfcPrivmsg(txtChannel.Text, $"[BROADCAST:{gameStatus}]");
                        }

                        // IRC Announce !
                        if (_irc != null && _irc.IsConnected)
                        {
                            string ip = GetLocalIPAddress();
                            string joinLink = $"join://{ip}:{port}|rom={romFileName}";
                            string msg = $"[CSUME] [{nick}] is hosting a CSUME game! Click link to Join!  {joinLink}";

                            _irc.RfcPrivmsg(txtChannel.Text, $"[INGAME:{nick}]");
                            _irc.RfcPrivmsg(txtChannel.Text, msg);
                            AppendChatLog(msg, Color.OrangeRed);

                            // Autojoin broadcast for users marked as ready
                            foreach (string readyUser in _readyUsers.ToList())
                            {
                                string command = $"[AUTOJOIN:{readyUser}]";
                                _irc.RfcPrivmsg(txtChannel.Text, command);
                            }
                        }

                        // Broadcast Timer
                        _hostBroadcastTimer = new System.Windows.Forms.Timer();
                        _hostBroadcastTimer.Interval = 3000;
                        _hostBroadcastTimer.Tick += (s, e2) =>
                        {
                            BroadcastHostInfo("CSUME", new List<string> { romFileName }, txtServerName.Text);
                        };
                        _hostBroadcastTimer.Start();

                        // Process Exit Cleanup
                        _serverProcess.EnableRaisingEvents = true;
                        _serverProcess.Exited += (s, e3) =>
                        {
                            HandleServerExited();

                            this.Invoke((MethodInvoker)(() =>
                            {
                                _isHosting = false;
                                _userStatus[nick] = "online";
                                chkHostTest.Checked = false;
                                SetUserInGame(nick, false);
                                UpdateUserIcon(nick);
                                UpdateLaunchButtonEnabled();
                                lblGameIsHosted.Text = "";

                                // Remove stale server from grid
                                for (int i = dgHostedServer.Rows.Count - 1; i >= 0; i--)
                                {
                                    var row = dgHostedServer.Rows[i];
                                    string engineVal = row.Cells["Engine"].Value?.ToString();
                                    string portVal = row.Cells["Port"].Value?.ToString();
                                    string fileVal = row.Cells["Addons"].Value?.ToString();

                                    if (engineVal == "CSUME" && portVal == "5029" && fileVal == romFileName)
                                    {
                                        dgHostedServer.Rows.RemoveAt(i);
                                        break;
                                    }
                                }
                            }));

                            // Stop broadcasting
                            _hostBroadcastTimer?.Stop();
                            _hostBroadcastTimer?.Dispose();
                            _hostBroadcastTimer = null;

                            // Clear from client list
                            Task.Run(() =>
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    BroadcastHostInfo("", new List<string>(), "");
                                    Thread.Sleep(500);
                                }
                            });

                            // IRC: Returned from host
                            if (_irc != null && _irc.IsConnected)
                            {
                                string returnMsg = $"[{nick}] returned from hosting the game.";
                                _irc.RfcPrivmsg(txtChannel.Text, returnMsg);
                                AppendChatLog(returnMsg, Color.Brown);
                            }
                        };

                        //  ! Initial heartbeat to make it visible immediately
                        Thread.Sleep(1500);
                        BroadcastHostInfo("CSUME", new List<string> { romFileName }, txtServerName.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error launching CSUME Server: " + ex.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    return;
                }


            }
            if (chkHostTest.Checked == false)
            {
                // === Single player fallback ===

                // Must be wrapped in quotes incase the path has spaces in it
                string allArgs = "\""+txtRomPath.Text+"\"";
                string workingDir = Path.GetDirectoryName(gameExecutable);
                try
                {
                    Debug.WriteLine("Launching a normal CSUME game with:");
                    Debug.WriteLine("Executable: " + gameExecutable);
                    Debug.WriteLine("Arguments : " + allArgs);
                    Debug.WriteLine("Working Dir: " + workingDir);
                   btnLaunch.Enabled = false;

                    var psi = new ProcessStartInfo
                    {
                        FileName = gameExecutable,
                        Arguments = allArgs,
                        UseShellExecute = false,
                        WorkingDirectory = workingDir,
                        WindowStyle = ProcessWindowStyle.Normal,
                    };


                    var proc = Process.Start(psi);
                    if (proc != null)
                    {
                        proc.EnableRaisingEvents = true;
                        proc.Exited += (s, _) =>
                        {
                            this.Invoke((MethodInvoker)(() =>
                            {
                                btnLaunch.Enabled = true;
                            }));
                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error launching game: " + ex.Message, "Launch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void UpdateLaunchButtonEnabled()
        {
            btnLaunch.Enabled = !_isHosting;
        }


        private void BroadcastHostInfo(string engine, List<string> hostedFiles, string serverName = "")
        {
            string hostIp = GetLocalIPAddress();
            string fileNames = string.Join(";", hostedFiles.Select(Path.GetFileName));
            string sessionId = _hostSessionId ?? "";

            serverName = serverName.Replace("|", ""); // sanitize

            string nick = txtNick.Text.Trim();
            string profilePicFilename = ""; // will remain empty unless we find an image
            string profilePicPath = Path.Combine(Application.StartupPath, "ProfilePic", nick + ".png");

            if (File.Exists(profilePicPath))
            {
                profilePicFilename = Path.GetFileName(profilePicPath);
            }

            // Final broadcast message ~ I may add a profile field for this later
            string message = $"ArcadeConnector|{engine}|{hostIp}|{fileNames}|{sessionId}|{serverName}";
            if (!string.IsNullOrEmpty(profilePicFilename))
                message += $"|profile={profilePicFilename}"; //~

            using (var udp = new System.Net.Sockets.UdpClient())
            {
                udp.EnableBroadcast = true;

                // 1. Send standard host info
                //~ This needs updated..
                // TODO, handle unknown requests.

                byte[] data = Encoding.UTF8.GetBytes(message);
                udp.Send(data, data.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Broadcast, 45291));

                // 2. Send raw profile image separately (optional binary packet)
                if (File.Exists(profilePicPath))
                {
                    byte[] imgData = File.ReadAllBytes(profilePicPath);

                    using (MemoryStream ms = new MemoryStream())
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        writer.Write("PROFILE_PIC");           // Tag
                        writer.Write(nick);                    // Username
                        writer.Write(imgData.Length);          // Image size
                        writer.Write(imgData);                 // Raw image bytes

                        byte[] picPacket = ms.ToArray();
                        udp.Send(picPacket, picPacket.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Broadcast, 45291));
                    }
                }
            }

            // Update LastSeen for our own hosted server entry    
            string port = (engine == "CSUME") ? "5029" : "";
            string key = hostIp + ":" + port;

            var entry = _hostedServerList.FirstOrDefault(h => h.IP + ":" + h.Port == key);
            if (entry != null)
            {
                entry.LastSeen = DateTime.Now;
            }

            if(engine == "CSUME")
            {
                 hostIp = GetLocalIPAddress();
                string fileList = string.Join(";", hostedFiles);
                sessionId = _hostSessionId ?? "";


                string msg = $"ArcadeConnector|{engine}|{hostIp}|{fileList}|{sessionId}|{serverName}";

                Debug.WriteLine("[BROADCAST] " + msg); 

                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                using (UdpClient udp = new UdpClient())
                {
                    udp.EnableBroadcast = true;
                    udp.Send(bytes, bytes.Length, new System.Net.IPEndPoint(System.Net.IPAddress.Broadcast, 45291));
                }
            }
        }

        // Add the hosted server to the data grid
        private void AddOrUpdateHostedServer(string engine, string servername, string ip, string port, string addons)
        {
            foreach (DataGridViewRow row in dgHostedServer.Rows)
            {
                if (row.Cells["IPAddress"].Value?.ToString() == ip && row.Cells["Port"].Value?.ToString() == port)
                {
                    row.Cells["Engine"].Value = engine;
                    row.Cells["ServerName"].Value = servername;
                    row.Cells["Addons"].Value = addons;

                    // Set row colors
                    row.DefaultCellStyle.BackColor = Color.Cyan;
                    row.DefaultCellStyle.ForeColor = Color.White;

                    UpdateHostedServerGridEnabled();
                    return;
                }
            }

            // Add new row
            int newIndex = dgHostedServer.Rows.Add(engine, servername, ip, port, addons);
            var newRow = dgHostedServer.Rows[newIndex];
            newRow.DefaultCellStyle.BackColor = Color.Cyan;
            newRow.DefaultCellStyle.ForeColor = Color.White;

            UpdateHostedServerGridEnabled();

        }


        private void ConnectToSelectedServer()
        {
            string nick = txtNick.Text.Trim();

            if (_isHosting || (_userStatus.TryGetValue(nick, out string status) && status == "ingame"))
            {
                AppendChat("[INFO] You're already in a game. Return before joining another.");
                return;
            }

            if (dgHostedServer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a server to connect to.");
                return;
            }

            var row = dgHostedServer.SelectedRows[0];
            string engine = row.Cells["Engine"].Value?.ToString();
            string ip = row.Cells["IPAddress"].Value?.ToString();
            string port = row.Cells["Port"].Value?.ToString();
            string addons = row.Cells["Addons"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(engine) || string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(port))
            {
                if (!row.IsNewRow)
                    dgHostedServer.Rows.Remove(row);
                UpdateHostedServerGridEnabled();
                return;
            }

            var hostedFiles = !string.IsNullOrWhiteSpace(addons)
                ? addons.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : new List<string>();

            if (!string.IsNullOrEmpty(engine))
                cmbEngine.SelectedItem = engine;

            string gameExecutable = "";
            switch (engine)
            {
                case "CSUME": gameExecutable = txtCSUMELocation.Text; break;
            }

            if (string.IsNullOrWhiteSpace(gameExecutable) || !File.Exists(gameExecutable))
            {
                MessageBox.Show("Please select a valid game executable.", "Error");
                return;
            }

            // Client Handling
            if (engine == "CSUME")
            {
                if (hostedFiles.Count == 0)
                {
                    MessageBox.Show("Host did not advertise a ROM file. Cannot proceed.", "Missing ROM Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string romFileName = Path.GetFileName(hostedFiles[0]); 
                string romFolder = txtRomsDefaultPath.Text.Trim();
                string romPath = Path.Combine(romFolder, romFileName);

                if (!File.Exists(romPath))
                {
                    MessageBox.Show($"ROM file not found: {romPath}\nPlease place it in your ROMs folder and try again.", "ROM Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string args = $"{romFileName} -client -port 5029 -hostname {ip}";
                string workingDir = Path.GetDirectoryName(gameExecutable);
                Debug.WriteLine("Launching CSUME client with:");
                Debug.WriteLine("Executable: " + gameExecutable);
                Debug.WriteLine("Arguments : " + args);
                Debug.WriteLine("Working Dir: " + workingDir);

                var psi = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    WorkingDirectory = workingDir,
                    FileName = gameExecutable,
                    Arguments = args,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                _userStatus[nick] = "ingame";
                SetUserInGame(nick, true);
                UpdateUserIcon(nick);

                if (_irc?.IsConnected == true)
                {
                    _irc.RfcPrivmsg(txtChannel.Text, $"[INGAME:{nick}]");
                }


                // Set the chat to Read-Only when hosting and connecting
                txtChat.ReadOnly = true;
                txtChat.BackColor = Color.FromArgb(30, 30, 30);
                txtChat.ForeColor = Color.White;
                dgHostedServer.Enabled = false;

                try
                {
                    if (_irc?.IsConnected == true)
                    {
                        _irc.RfcPrivmsg(txtChannel.Text, $"[{nick}] joined the game.");
                        AppendChatLog($"[{nick}] joined the game.", Color.LimeGreen);
                    }

                    var proc = Process.Start(psi);
                    if (proc != null)
                    {
                        proc.EnableRaisingEvents = true;
                        proc.Exited += (s, e) =>
                        {
                            _userStatus[nick] = "online";
                            SetUserInGame(nick, false);
                            SetUserAfk(nick, false); // just in case
                            UpdateUserIcon(nick);

                            // Clear Ready Up state
                            ClearReadyStatusForUser(nick);

                            // Restart idle timer for AFK detection
                            _idleTimer?.Stop();
                            _idleTimer?.Dispose();
                            _idleTimer = new System.Timers.Timer(_afkTimeout.TotalMilliseconds);
                            _idleTimer.Elapsed += IdleTimerElapsed;
                            _idleTimer.AutoReset = false;
                            _idleTimer.Start();


                            this.Invoke((MethodInvoker)(() =>
                            {
                                txtChat.Enabled = true;
                                dgHostedServer.Enabled = true;
                                UpdateHostedServerGridEnabled();
                            }));

                            if (_irc?.IsConnected == true)
                            {
                                _irc.RfcPrivmsg(txtChannel.Text, $"[{nick}] returned from game.");
                                AppendChatLog($"[{nick}] returned from game.", Color.Green);
                            }
                        };

                    }
                    else
                    {
                        txtChat.ReadOnly = false;
                        txtChat.BackColor = Color.FromArgb(30, 30, 30);
                        dgHostedServer.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    txtChat.ReadOnly = false;
                    txtChat.BackColor = Color.FromArgb(30, 30, 30);
                    dgHostedServer.Enabled = true;
                    MessageBox.Show("Error launching CSUME game: " + ex.Message);
                }

                return;
            }
        }



        private void UpdateHostedServerGridEnabled()
        {
            dgHostedServer.Enabled = !_isHosting && dgHostedServer.Rows.Count > 0;
        }

        private bool IsPrintableText(byte[] data)
        {
            return data.All(b => b == 9 || b == 10 || b == 13 || (b >= 32 && b <= 126));
        }

        //new System.Net.IPEndPoint(System.Net.IPAddress.Any, 45291);
        private void StartHostListener()
        {
            Task.Run(() =>
            {
                try
                {
                    var endpoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 45291);

                    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    sock.ExclusiveAddressUse = false;
                    sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    sock.Bind(endpoint);

                    using (var udp = new UdpClient { Client = sock })
                    {
                        while (true)
                        {
                            var remoteEP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 45291);
                            var data = udp.Receive(ref remoteEP);

                            if (data == null || data.Length < 12)
                                continue;

                            if (IsPrintableText(data))
                            {
                                string msg = Encoding.UTF8.GetString(data);

                                if (msg.StartsWith("ArcadeConnector|"))
                                {
                                    var parts = msg.Split('|');
                                    if (parts.Length >= 3)
                                    {
                                        string engine = parts[1];
                                        string hostIp = parts[2];

                                        // ✅ Handle shutdown broadcast
                                        if (string.IsNullOrWhiteSpace(engine))
                                        {
                                            this.Invoke((MethodInvoker)(() =>
                                            {
                                                lblGameIsHosted.Text = "";
    

                                                _hostPresenceTimer?.Stop();

                                                // ✅ Remove ALL hosted servers with this IP
                                                for (int i = dgHostedServer.Rows.Count - 1; i >= 0; i--)
                                                {
                                                    var row = dgHostedServer.Rows[i];
                                                    string ip = row.Cells["IPAddress"].Value?.ToString();
                                                    if (ip == hostIp)
                                                        dgHostedServer.Rows.RemoveAt(i);
                                                }
                                            }));
                                            continue;
                                        }

                                        //  Proceed only if enough parts for full server info
                                        if (parts.Length >= 6)
                                        {
                                            string hostedFile = parts[3];
                                            string sessionId = parts[4];
                                            string servername = parts[5];

                                            this.Invoke((MethodInvoker)delegate
                                            {
                                                if (!string.IsNullOrEmpty(_endedHostSessionId) && sessionId == _endedHostSessionId)
                                                    return;

                                                if (!string.IsNullOrEmpty(sessionId) && sessionId != _currentHostSessionId)
                                                {
                                                    _currentHostSessionId = sessionId;
                                                    _endedHostSessionId = null;
                                                }

                                                if (_hostPresenceTimer == null)
                                                {
                                                    _hostPresenceTimer = new System.Windows.Forms.Timer();
                                                    _hostPresenceTimer.Interval = 5000;
                                                    _hostPresenceTimer.Tick += (s, e) =>
                                                    {
                                                        lblGameIsHosted.Text = "";
 
                                                        _hostPresenceTimer.Stop();
                                                    };
                                                }

                                                _hostPresenceTimer.Stop();
                                                _hostPresenceTimer.Start();

                                                string port = (engine == "CSUME") ? "5029" : "";
                                                string addons = hostedFile;

                                                _lastHostedEngine = engine;

                                                // ✅ Always show label on client when game is hosted
                                                lblGameIsHosted.Text = $"Heartbeat detected a {engine} game hosted!\nView [Hosted Servers] for more info...";
                                                AddOrUpdateHostedServer(engine, servername, hostIp, port, addons);
                                            });
                                        }
                                    }
                                }
                            }
                            // Profile picture handling
                            else
                            {
                                try
                                {
                                    using (var ms = new MemoryStream(data))
                                    using (var reader = new BinaryReader(ms))
                                    {
                                        string tag = reader.ReadString();
                                        if (tag == "PROFILE_PIC")
                                        {
                                            string senderNick = reader.ReadString();
                                            int imgLength = reader.ReadInt32();
                                            if (imgLength > 0 && imgLength < 1_000_000)
                                            {
                                                byte[] imgBytes = reader.ReadBytes(imgLength);
                                                string savePath = Path.Combine(Application.StartupPath, "ProfilePic", senderNick + ".png");
                                                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                                                File.WriteAllBytes(savePath, imgBytes);
                                                Console.WriteLine($"[PROFILE] Saved profile image for {senderNick}");
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("[PROFILE] Error receiving image: " + ex.Message);
                                }
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show($"Failed to bind host listener socket: {ex.Message}", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }


        // Get the systems local ip address for hosting a game
        private string GetLocalIPAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            // in not found we assume it's offline
            return "127.0.0.1";
        }

   

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hostBroadcastTimer != null)
            {
                _hostBroadcastTimer.Stop();
                _hostBroadcastTimer.Dispose();
                _hostBroadcastTimer = null;
            }

            base.OnFormClosing(e);
        }


        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            lblProcessStatus.Visible = !lblProcessStatus.Visible;
        }

 

        private void Timer_Tick(object sender, EventArgs e)
        {
            var timer = sender as System.Windows.Forms.Timer;
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            btnCloseInfo.Visible = false;
            rtbInfo.Visible = false;

            rtbWelcome.Text = "\n\n\n        Thank you for using Arcade Connector!\n\n       Please select a ROM file to get started.";

            // Toggle visibility based on whether a ROM was previously selected
            if (!string.IsNullOrEmpty(Properties.Settings.Default.RomName))
                rtbWelcome.Visible = false;
            else
                rtbWelcome.Visible = true;


            // Get the defualt CSUME paths
            string appRoot = Application.StartupPath;
            txtCSUMELocation.Text = Path.Combine(appRoot, "csume", "csume.exe");
            txtRomsDefaultPath.Text = Path.Combine(appRoot, "csume", "roms");

            string defaultPath = Path.Combine(Application.StartupPath, "csume", "artwork", "default.png");
            if (File.Exists(defaultPath))
                pbSnap.Image = Image.FromFile(defaultPath);
            else
                pbSnap.Image = null;

            string romName = Path.GetFileNameWithoutExtension(Properties.Settings.Default.RomName);
            UpdateSnapPreviewFromRom(romName);

            //    txtCSUMELocation.ReadOnly = true;
            //    txtRomsDefaultPath.ReadOnly = true;
            ////btnCSUMELocation.Enabled = false;
            ////btnROMsLocation.Enabled = false;


            // Start the program up in the right corner of the screen
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int formWidth = this.Width;
            this.Location = new Point(screenWidth - formWidth, 0);
     
            // This program broadcasts if a game is hosted with it
            // Start the host listener method, listen for hosted games.
            StartHostListener();

            cmbEngine.SelectedIndex = 0;
            cmbEngineSelector.SelectedIndex = 0;

            dgHostedServer.CellFormatting += dgHostedServer_CellFormatting;

            imageList1.Images.Add("online", Image.FromFile(Path.Combine(Application.StartupPath, "Images", "online.png")));
            imageList1.Images.Add("snooze", Image.FromFile(Path.Combine(Application.StartupPath, "Images", "snooze.png")));
            imageList1.Images.Add("ingame", Image.FromFile(Path.Combine(Application.StartupPath, "Images", "ac_icon.png")));
            imageList1.Images.Add("user_ready", Image.FromFile(Path.Combine(Application.StartupPath, "Images", "user_ready.png")));
            imageList1.Images.Add("is_moderator", Image.FromFile(Path.Combine(Application.StartupPath, "Images", "is_moderator.png")));

            lvUsers.SmallImageList = imageList1;
            lvUsers.ContextMenuStrip = contextMenuUser;
            lvUsers.MouseDown += lvUsers_MouseDown;

            _connectionCheckTimer = new System.Windows.Forms.Timer();
            _connectionCheckTimer.Interval = 10000; // 10 seconds
            _connectionCheckTimer.Tick += CheckIrcConnectionAlive;
            _connectionCheckTimer.Start();

            txtChat.ReadOnly = true;
            txtChat.Enabled = true;

            txtChat.LinkClicked += txtChat_LinkClicked;

            this.txtChat.MouseDown += new MouseEventHandler(this.txtChat_MouseDown);
            this.txtChat.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtChat_MouseMove);

            dgHostedServer.Enabled = false;
            lblDownloadProgress.Text = "";
            lblServerName.Visible = false;
            txtServerName.Visible = false;
            UpdateLaunchButtonEnabled();

            lblGameIsHosted.Text = "";
            lblProcessStatus.Text = "";
      
            ////txtCSUMELocation.Text = Properties.Settings.Default.CSUMELocation;
            ////txtRomsDefaultPath.Text = Properties.Settings.Default.RomsDefaultPath;
            txtRomPath.Text = Properties.Settings.Default.RomPath;
            lblLoadedROM.Text = Properties.Settings.Default.RomName;
            cmbEngine.SelectedItem = Properties.Settings.Default.SelectedEngine;

            //IRC USERNAME
            txtNick.Text = Properties.Settings.Default.IRCUserName;

            //IRC Auto Connect
            chkIRCAutoConnect.Checked = Properties.Settings.Default.IRCAutoConnect;

            //Call UpdateCommandLine on form load
            UpdateCommandLine();

            // Update CMDParameters
            txtCMDParameters.Text = Properties.Settings.Default.CMDParameters;

            cmbEngineSelector.SelectedIndex = cmbEngine.SelectedIndex;

            // IRC Clickable link 
            txtChat.LinkClicked += txtChat_LinkClicked;


            // Autoconnect to IRC--------------------------------------------
            if (chkIRCAutoConnect.Checked == true)
            {
                // disable txtNick so it can't be changed while connected. 
                txtNick.Enabled = false;

                // disable the Connect button
                btnIRCConnect.Enabled = false;

                AppendChat("[STATUS] Looking up server...");

                // profile start up:

                StartUdpProfileListener();
                BroadcastProfilePicture();


                AppendChat("[STATUS] Connecting…");
                try
                {
                    string server = txtServer.Text.Trim();
                    int port = int.Parse(txtPort.Text);
                    string nick = string.IsNullOrWhiteSpace(txtNick.Text)
                                    ? $"User{new Random().Next(1000, 9999)}"
                                    : txtNick.Text.Trim();
                    txtNick.Text = nick;

                    // 1) TCP connect
                    _irc.Connect(server, port);

                    // 2) Handshake
                    _irc.Login(nick, nick);

                    // 3) JOIN right after handshake
                    string channel = txtChannel.Text.Trim();
                    _irc.RfcJoin(channel);
                    _irc.RfcPrivmsg(channel, $"entered the channel!");
                    AppendChatLog($"Joining Channel {nick}!", Color.Green);

                    AppendChat($"[STATUS] JOIN {channel}...");

                    // Record join time
                    _userJoinTimes[nick] = DateTime.Now;
                    if (_pingTimer == null)
                    {
                        _pingTimer = new System.Windows.Forms.Timer();
                        _pingTimer.Interval = 10000; // every 10 seconds
                        _pingTimer.Tick += CheckRealConnection;
                    }
                    _pingTimer.Start();

                    // Add self to lvUsers with default icon
                    if (!lvUsers.Items.Cast<ListViewItem>().Any(i => i.Text.Equals(nick, StringComparison.OrdinalIgnoreCase)))
                    {
                        ListViewItem selfItem = new ListViewItem(nick);
                        selfItem.ImageKey = "online";
                        lvUsers.Items.Add(selfItem);

                    }

                    // Apply AFK icon to all users already marked as AFK
                    foreach (ListViewItem item in lvUsers.Items)
                    {
                        string u = item.Text;
                        if (_afkUsers.Contains(u))
                            item.ImageKey = "snooze";
                        else
                            item.ImageKey = "online";
                    }

                    // Start idle timer
                    _idleTimer = new System.Timers.Timer(_afkTimeout.TotalMilliseconds);
                    _idleTimer.Elapsed += IdleTimerElapsed;
                    _idleTimer.AutoReset = false;
                    _idleTimer.Start();

                    // 4) Start background IRC listening
                    _listenThread = new Thread(() => _irc.Listen())
                    {
                        IsBackground = true
                    };
                    _listenThread.Start();
                }
                catch (Exception ex)
                {
                    AppendChat($"[EXCEPTION] {ex.Message}");
                }

                // Save current nickname setting
                Properties.Settings.Default.IRCUserName = txtNick.Text;
                Properties.Settings.Default.Save();

                //Auto IRC Connect end ---------------------------------
            }

        }

        private void dgHostedServer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgHostedServer.Rows[e.RowIndex].IsNewRow)
                return;

            // Color all rows cyan background, white text
            // This doesn't always work, it might stay as a dark orange bg sometimes
            dgHostedServer.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkCyan;
            dgHostedServer.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
        }


        private void btnSaveFiles_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder to save downloaded files";
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    txtSaveFilesLocation.Text = fbd.SelectedPath;
                    Properties.Settings.Default.SaveFilesLocation = fbd.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
        }



        private void dgHostedServer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string localNick = txtNick.Text.Trim();
            if (_isHosting || (_userStatus.ContainsKey(localNick) && _userStatus[localNick] == "ingame"))
            {
                MessageBox.Show("You're already in a game. Return before joining another.", "In-Game", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ConnectToSelectedServer();
        }


        // This actually won't do anything atm, its meant to allow other engine in the combobox
        private void cmbEngineSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCMDParameters.Text = string.Empty; 

            if (cmbEngine.SelectedIndex == -1)
            {
                // No selection made, do nothing
                return;
            }

            Properties.Settings.Default.SelectedEngine = cmbEngine.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            UpdateCommandLine();

            txtCMDParameters.Text = txtCMDParameters.Text.Trim();
            cmbEngine.SelectedIndex = cmbEngineSelector.SelectedIndex;
        }


        private void chkHostTest_CheckedChanged(object sender, EventArgs e)
        {
            string nick = txtNick.Text.Trim();

            if (chkHostTest.Checked)
            {
                lblServerName.Visible = true;
                txtServerName.Visible = true;
                btnLaunch.Text = "Host Game";

                // Set icon to as moderator
                SetUserIcon(nick, "is_moderator");

                // Broadcast to others
                if (_irc != null && _irc.IsConnected)
                {
                    _irc.RfcPrivmsg(txtChannel.Text, $"[MODERATOR:{nick}]");
                    string msg = $"[CSUME] [{nick}] is prepared to host a game.";
                    _irc.RfcPrivmsg(txtChannel.Text, msg);
                    AppendChatLog(msg, Color.OrangeRed);
                }
            }
            else
            {
                lblServerName.Visible = false;
                txtServerName.Visible = false;
                btnLaunch.Text = "Launch";

                // Reset icon to online
                SetUserIcon(nick, "online");

                // Broadcast to others
                if (_irc != null && _irc.IsConnected)
                {
                    _irc.RfcPrivmsg(txtChannel.Text, $"[UNMODERATOR:{nick}]");
                }
            }

            UpdateCommandLine();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save user preferences first
            Properties.Settings.Default.CMDParameters = txtCMDParameters.Text;
            Properties.Settings.Default.IRCAutoConnect = chkIRCAutoConnect.Checked;
            Properties.Settings.Default.Save();

            string nick = txtNick.Text.Trim();
            string channel = txtChannel.Text.Trim();

            // Cleanly disconnect from IRC in a background task
            Task.Run(() =>
            {
                try
                {
                    if (_irc != null && _irc.IsConnected)
                    {
                        _irc.RfcPrivmsg(channel, $"[{nick}] left the channel.");
                        _irc.RfcQuit("Bye!");
                        _irc.Disconnect();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[IRC Disconnect ERROR] " + ex.Message);
                }
            });

            // Abort listener thread cleanly if alive
            try
            {
                if (_listenThread != null && _listenThread.IsAlive)
                    _listenThread.Join(300); // allow it to finish


            }
            catch (Exception ex)
            {
                Debug.WriteLine("[THREAD JOIN ERROR] " + ex.Message);
            }

            // Clear state — no UI updates!
            try
            {
                _afkUsers.Clear();
                _userStatus.Clear();
                _readyUsers.Clear();

            }
            catch { }

            // Play disconnect sound asynchronously
            Task.Run(() =>
            {
                try
                {
                    _msgDisconnect?.Play();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[SOUND ERROR] " + ex.Message);
                }
            });

          
        }




        private UdpClient _udpProfileClient;
        private Thread _udpProfileThread;
        private bool _udpProfileRunning = false;


        // Begin IRC -----------------------------------------------------------

        // IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Any, 0);

        private void StartUdpProfileListener()
        {
            try
            {
                _udpProfileClient = new UdpClient(23470);
                _udpProfileRunning = true;

                _udpProfileThread = new Thread(() =>
                {
                    try
                    {
                        IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Any, 0);

                        while (_udpProfileRunning)
                        {
                            if (_udpProfileClient.Available > 0)
                            {
                                byte[] data = _udpProfileClient.Receive(ref remoteEP);
                                // TODO: handle incoming profile data
                            }
                            else
                            {
                                Thread.Sleep(100); // avoid tight loop
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("Profile UDP socket error: " + ex.Message);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Expected when closing socket
                    }
                });

                _udpProfileThread.IsBackground = true;
                _udpProfileThread.Start();
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Could not start profile listener: {ex.Message}", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopUdpProfileListener()
        {
            _udpProfileRunning = false;

            try
            {
                _udpProfileClient?.Close();
                _udpProfileClient = null;
            }
            catch { }

            try
            {
                if (_udpProfileThread != null && _udpProfileThread.IsAlive)
                    _udpProfileThread.Join(500);
            }
            catch { }

            _udpProfileThread = null;
        }


        private void BroadcastProfilePicture()
        {
            string nick = txtNick.Text.Trim();
            string path = Path.Combine(Application.StartupPath, "ProfilePic", nick + ".png");
            BroadcastProfilePicture(nick, path); // Calls the new method you defined
        }

        private void btnIRCConnect_Click(object sender, EventArgs e)
        {
            btnIRCConnect.Enabled = false;
            AppendChat("[STATUS] Looking up server...");

            // Profile start up:

            StartUdpProfileListener();
            BroadcastProfilePicture();

            txtNick.Enabled = false;

            //-------------------------------------------


            AppendChat("[STATUS] Connecting…");
            try
            {
                string server = txtServer.Text.Trim();
                int port = int.Parse(txtPort.Text);
                string nick = string.IsNullOrWhiteSpace(txtNick.Text)
                                ? $"User{new Random().Next(1000, 9999)}"
                                : txtNick.Text.Trim();
                txtNick.Text = nick;

                // 1) TCP connect
                _irc.Connect(server, port);

                // 2) Handshake
                _irc.Login(nick, nick);

                // 3) JOIN right after handshake
                string channel = txtChannel.Text.Trim();
                _irc.RfcJoin(channel);
                _irc.RfcPrivmsg(channel, $"entered the channel!");
                AppendChatLog($"Joining Channel {nick}!", Color.Green);

                AppendChat($"[STATUS] JOIN {channel}...");

                // Record join time
                _userJoinTimes[nick] = DateTime.Now;
                if (_pingTimer == null)
                {
                    _pingTimer = new System.Windows.Forms.Timer();
                    _pingTimer.Interval = 10000; // every 10 seconds
                    _pingTimer.Tick += CheckRealConnection;
                }
                _pingTimer.Start();

                // Add self to lvUsers with default icon
                if (!lvUsers.Items.Cast<ListViewItem>().Any(i => i.Text.Equals(nick, StringComparison.OrdinalIgnoreCase)))
                {
                    ListViewItem selfItem = new ListViewItem(nick);
                    selfItem.ImageKey = "online";
                    lvUsers.Items.Add(selfItem);

                }

                // Apply correct AFK icon to all users already marked as AFK
                foreach (ListViewItem item in lvUsers.Items)
                {
                    string u = item.Text;
                    if (_afkUsers.Contains(u))
                        item.ImageKey = "snooze";
                    else
                        item.ImageKey = "online";
                }

                // Start idle timer
                _idleTimer = new System.Timers.Timer(_afkTimeout.TotalMilliseconds);
                _idleTimer.Elapsed += IdleTimerElapsed;
                _idleTimer.AutoReset = false;
                _idleTimer.Start();

                // 4) Start background IRC listening
                _listenThread = new Thread(() => _irc.Listen())
                {
                    IsBackground = true
                };
                _listenThread.Start();
            }
            catch (Exception ex)
            {
                AppendChat($"[EXCEPTION] {ex.Message}");
            }

            Properties.Settings.Default.IRCUserName = txtNick.Text;
            Properties.Settings.Default.Save();
        }

        private void IdleTimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                string nick = txtNick.Text.Trim();
                string channel = txtChannel.Text.Trim();

                if (!_afkUsers.Contains(nick))
                {
                    _afkUsers.Add(nick);
                    _userAfkTimestamps[nick.ToLowerInvariant()] = DateTime.Now;

                    SetUserAfk(nick, true); // Updates _userStatus and calls UpdateUserIcon()
                    _irc.RfcPrivmsg(channel, $"[AFK:{nick}]");

                    AppendChatLog($"[AUTO] Marked as AFK due to inactivity", Color.SandyBrown);
                    Console.WriteLine($"[DEBUG] Auto-set AFK after {Math.Round(_afkTimeout.TotalMinutes)} min inactivity");
                }
            }));
        }



        // fired once TCP + IRC handshake is complete
        private void Irc_OnConnected(object sender, EventArgs e)
        {
            AppendChat("[STATUS] Connected to server...");
            var channel = txtChannel.Text.Trim();
            _irc.RfcJoin(channel);

            if (txtInput.InvokeRequired)
                txtInput.Invoke((MethodInvoker)(() => txtInput.Focus()));
            else
                txtInput.Focus();
        }

        private void UpdateUserIcon(string nick)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke(new Action(() => UpdateUserIcon(nick)));
                return;
            }

            string status = "online";
            if (_userStatus.TryGetValue(nick, out string s))
                status = s;

            foreach (ListViewItem item in lvUsers.Items)
            {
                if (item.Text.Equals(nick, StringComparison.OrdinalIgnoreCase))
                {
                    // Match status string to image keys
                    if (status == "afk")
                        item.ImageKey = "snooze";
                    else if (status == "ingame")
                        item.ImageKey = "ingame";
                    else
                        item.ImageKey = "online";

                    break;
                }
            }
        }

        private void SetUserInGame(string username, bool ingame)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)(() => SetUserInGame(username, ingame)));
                return;
            }

            string key = username.ToLowerInvariant();
            _userStatus[key] = ingame ? "ingame" : "online";
            UpdateUserIcon(username);
        }


        private void SetUserAfk(string nick, bool isAfk)
        {
            _userStatus[nick] = isAfk ? "afk" : "online";
            UpdateUserIcon(nick);
        }


        private void btnTestIcons_Click(object sender, EventArgs e)
        {
            TestAddUsersWithIcons();
        }
        private void TestAddUsersWithIcons()
        {
            lvUsers.Items.Clear();

            ListViewItem user1 = new ListViewItem("PlayerOne") { ImageKey = "online" };
            ListViewItem user2 = new ListViewItem("PlayerTwo") { ImageKey = "ingame" };
            lvUsers.Items.AddRange(new[] { user1, user2 });
        }


        // channel chat lines
        private void Irc_OnChannelMessage(object sender, IrcEventArgs e)
        {
            string msg = e.Data.Message;
            string nick = e.Data.Nick;
            Console.WriteLine($"[IRC DEBUG] From {e.Data.Nick}: {e.Data.Message}");
            Console.WriteLine($"[IRC_MSG] From {nick}: {msg}");

            try
            {
                // [BACK:]
                if (msg.StartsWith("[BACK:", StringComparison.OrdinalIgnoreCase))
                {
                    string backUser = msg.Substring(6, msg.Length - 7);
                    string localUser = txtNick.Text.Trim();

                    SetUserInGame(backUser, false);
                    SetUserAfk(backUser, false);

                    // Remove from ready list if they were in it
                    if (_readyUsers.Contains(backUser))
                        _readyUsers.Remove(backUser);

                    // Reset user icon to "online"
                    SetUserIcon(backUser, "online");

                    // If it's the local user, update ListView item as well
                    if (backUser.Equals(localUser, StringComparison.OrdinalIgnoreCase))
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            foreach (ListViewItem item in lvUsers.Items)
                            {
                                if (item.Text == backUser)
                                {
                                    item.ImageKey = "online";

                                    // checkmark toggle
                                    readyUpToolStripMenuItem.Checked = false;

                                    break;
                                }
                            }
                        }));
                    }

                    return;
                }

                if (msg.StartsWith("[MODERATOR:", StringComparison.OrdinalIgnoreCase))
                {
                    string user = msg.Substring(11, msg.Length - 12);
                    SetUserIcon(user, "is_moderator");
                    return;
                }

                if (msg.StartsWith("[UNMODERATOR:", StringComparison.OrdinalIgnoreCase))
                {
                    string user = msg.Substring(13, msg.Length - 14);
                    SetUserIcon(user, "online");
                    return;
                }


                // SAFELY HANDLE [AUTOJOIN:]
                if (msg.StartsWith("[AUTOJOIN:", StringComparison.OrdinalIgnoreCase))
                {
                    int startIdx = "[AUTOJOIN:".Length;
                    int endIdx = msg.IndexOf(']', startIdx);

                    if (endIdx > startIdx)
                    {
                        string autoJoinTarget = msg.Substring(startIdx, endIdx - startIdx);
                        string localUser = txtNick.Text.Trim();

                        if (autoJoinTarget.Equals(localUser, StringComparison.OrdinalIgnoreCase))
                        {
                            AppendChatLog("[AUTOJOIN] Joining game automatically...", Color.Orange);

                            this.Invoke((MethodInvoker)(() =>
                            {
                                ClearReadyStatusForUser(localUser); // Clear ready state
                                PerformAutoJoinFromLastJoinLink();  // Trigger ConnectToSelectedServer
                                                                        
                            }));
                        }
                    }
                    else
                    {
                        AppendChatLog("[AUTOJOIN] Invalid format. Ignored.", Color.Red);
                    }

                    return;
                }


                if (msg.StartsWith("[READY:", StringComparison.OrdinalIgnoreCase))
                {
                    string user = msg.Substring(7, msg.Length - 8);
                    _readyUsers.Add(user);
                    SetUserIcon(user, "user_ready");
                    return;
                }

                if (msg.StartsWith("[UNREADY:", StringComparison.OrdinalIgnoreCase))
                {
                    string user = msg.Substring(9, msg.Length - 10);
                    _readyUsers.Remove(user);
                    SetUserIcon(user, "online");
                    return;
                }

                // Anti-spam logic
                if (!_userMessageTimestamps.ContainsKey(nick))
                    _userMessageTimestamps[nick] = new Queue<DateTime>();

                var timestamps = _userMessageTimestamps[nick];
                DateTime now = DateTime.Now;

                while (timestamps.Count > 0 && (now - timestamps.Peek()) > SpamWindow)
                    timestamps.Dequeue();

                timestamps.Enqueue(now);

                if (timestamps.Count > SpamThreshold)
                {
                    if (!_warnedUsers.Contains(nick))
                    {
                        AppendChatLog($"[WARN] {nick}, you're sending messages too fast. Please slow down.", Color.Orange);
                        _warnedUsers.Add(nick);
                    }
                }
                else
                {
                    if (_warnedUsers.Contains(nick))
                        _warnedUsers.Remove(nick);
                }

                // [AFK:]
                if (msg.StartsWith("[AFK:", StringComparison.OrdinalIgnoreCase))
                {
                    string afkUser = msg.Substring(5, msg.Length - 6);
                    string afkKey = afkUser.ToLowerInvariant();

                    _userAfkTimestamps[afkKey] = DateTime.Now;
                    _afkUsers.Add(afkUser);
                    SetUserAfk(afkUser, true);
                    return;
                }

                // [INGAME:]
                if (msg.StartsWith("[INGAME:", StringComparison.OrdinalIgnoreCase))
                {
                    string ingameUser = msg.Substring(8, msg.Length - 9);
                    string key = ingameUser.ToLowerInvariant();

                    _userStatus[key] = "ingame";
                    SetUserInGame(ingameUser, true);
                    UpdateUserIcon(ingameUser);
                    AppendChatLog($"{ingameUser} is now in-game.", Color.Yellow);
                    return;
                }

                // joined the game
                if (msg.StartsWith("[") && msg.Contains("] joined the game."))
                {
                    string joinedUser = msg.Substring(1, msg.IndexOf(']') - 1).Trim();
                    SetUserInGame(joinedUser, true);
                    AppendChatLog(msg, Color.LimeGreen);
                    return;
                }

                // returned from game
                if (msg.StartsWith("[") && msg.Contains("] returned from game."))
                {
                    string returningUser = msg.Substring(1, msg.IndexOf(']') - 1).Trim();
                    SetUserInGame(returningUser, false);
                    AppendChatLog(msg, Color.Green);
                    return;
                }

                if (msg.EndsWith("returned from hosting the game."))
                {
                    AppendChatLog($"{msg}", Color.OrangeRed);
                    return;
                }

                if (msg.Contains("is hosting a") && msg.Contains("join://"))
                {
                    AppendChatLog($"{msg}", Color.Goldenrod);
                    try
                    {
                        var match = Regex.Match(msg, @"join://[^\s]+");
                        if (match.Success)
                        {
                            _lastJoinLink = match.Value;
                        }

                        _msgHostSound?.Play();
                    }
                    catch (Exception ex)
                    {
                        AppendChat($"[SOUND ERROR] {ex.Message}");
                    }

                    return;
                }


                if (msg.Contains("[STATUS]")) { AppendChatLog($"{nick}: {msg}", Color.Lime); return; }
                if (msg.Contains("[WARN]")) { AppendChatLog($"{nick}: {msg}", Color.Brown); return; }
                if (msg.Contains("[INFO]")) { AppendChatLog($"{nick}: {msg}", Color.LimeGreen); return; }
                if (msg.Contains("[EXCEPTION]")) { AppendChatLog($"{nick}: {msg}", Color.HotPink); return; }

                if (msg.Contains("joined the game.") || msg.Contains("returned from game."))
                {
                    AppendChatLog(msg, msg.Contains("returned") ? Color.Green : Color.LimeGreen);
                }
                else
                {
                    AppendChatLog($"{nick}: {msg}", Color.LightBlue);
                }

                if (!Regex.IsMatch(msg, @"\/sound\s+\w+", RegexOptions.IgnoreCase))
                {
                    try { _msgPlayer?.Play(); } catch { }
                }

                if (msg.EndsWith("entered the channel.") || msg.EndsWith("entered the channel!"))
                {
                    PlaySound("cheer.wav");
                }

                if (msg.EndsWith("left the channel."))
                {
                    PlaySound("beback.wav");
                }

                if (_warnedUsers.Contains(nick) && timestamps.Count <= SpamThreshold / 2)
                {
                    _warnedUsers.Remove(nick);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[IRC] ERROR: " + ex.Message);
            }
        }


        private void PerformAutoJoinFromLastJoinLink()
        {
            if (string.IsNullOrEmpty(_lastJoinLink))
            {
                AppendChatLog("[AUTOJOIN ERROR] No valid join link was received.", Color.Red);
                return;
            }

            try
            {
                HandleJoinLink(_lastJoinLink); 
            }
            catch (Exception ex)
            {
                AppendChatLog("[AUTOJOIN ERROR] " + ex.Message, Color.Red);
            }
        }



        private void SetUserIcon(string user, string imageKey)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke((MethodInvoker)(() => SetUserIcon(user, imageKey)));
                return;
            }

            foreach (ListViewItem item in lvUsers.Items)
            {
                if (item.Text.Equals(user, StringComparison.OrdinalIgnoreCase))
                {
                    item.ImageKey = imageKey;
                    break;
                }
            }
        }

        private void SetUserAfkIcon(string nick, bool isAfk)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke(new Action(() => SetUserAfkIcon(nick, isAfk)));
                return;
            }

            foreach (ListViewItem item in lvUsers.Items)
            {
                if (item.Text.Equals(nick, StringComparison.OrdinalIgnoreCase))
                {
                    item.ImageKey = isAfk ? "snooze" : "online"; 
                    break;
                }
            }
        }

        private void PlaySound(string filename)
        {
            string path = Path.Combine(Application.StartupPath, "Sounds", filename);
            if (File.Exists(path))
            {
                try
                {
                    using (SoundPlayer s = new SoundPlayer(path))
                        s.Play();
                }
                catch (Exception ex)
                {
                    AppendChat($"[SOUND ERROR] {ex.Message}");
                }
            }
        }


        private void AppendChatLog(string text, Color color)
        {
            // Skip if control is disposed or handle not ready
            if (txtChat == null || txtChat.IsDisposed || !txtChat.IsHandleCreated)
                return;

            if (txtChat.InvokeRequired)
            {
                txtChat.Invoke(new Action(() => AppendChatLog(text, color)));
                return;
            }

            try
            {
                // Handle all /sound commands (multiple in a line)
                text = Regex.Replace(text, @"\/sound\s+(\w+)", match =>
                {
                    string soundName = match.Groups[1].Value;
                    string soundPath = Path.Combine(Application.StartupPath, "Sounds", soundName + ".wav");

                    if (File.Exists(soundPath))
                    {
                        try
                        {
                            if (!_customSounds.ContainsKey(soundPath))
                            {
                                SoundPlayer sp = new SoundPlayer(soundPath);
                                sp.Load(); // Preload
                                _customSounds[soundPath] = sp;
                            }

                            _customSounds[soundPath].Play();
                            return $" (played {soundName}.wav)";
                        }
                        catch (Exception ex)
                        {
                            color = Color.Red;
                            return $"[SOUND ERROR] {ex.Message}";
                        }
                    }
                    else
                    {
                        color = Color.Brown;
                        return $"[WARN] Sound not found: {soundName}.wav";
                    }
                }, RegexOptions.IgnoreCase);

                // Define image keywords and paths
                var imageTokens = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "/soulsphere", Path.Combine(Application.StartupPath, "Images", "soulsphere.png") },
            { "/beer", Path.Combine(Application.StartupPath, "Images", "beer.png") },
            { "/cheers", Path.Combine(Application.StartupPath, "Images", "cheers.png") },
            { "/snooze", Path.Combine(Application.StartupPath, "Images", "snooze.png") }
            // TODO add more image commands
        };

                txtChat.SelectionStart = txtChat.TextLength;
                txtChat.SelectionLength = 0;
                txtChat.SelectionColor = color;

                // Split into tokens and insert inline images
                string[] tokens = text.Split(' ');
                foreach (string token in tokens)
                {
                    if (imageTokens.ContainsKey(token))
                    {
                        string imgPath = imageTokens[token];
                        if (File.Exists(imgPath))
                        {
                            try
                            {
                                using (Image img = Image.FromFile(imgPath))
                                {
                                    Clipboard.SetImage(img);
                                    bool wasReadOnly = txtChat.ReadOnly;
                                    txtChat.ReadOnly = false;
                                    txtChat.Paste();
                                    txtChat.ReadOnly = wasReadOnly;
                                    txtChat.AppendText(" ");
                                }
                            }
                            catch
                            {
                                txtChat.AppendText("[ImageError] ");
                            }
                        }
                        else
                        {
                            txtChat.AppendText("[MissingImage] ");
                        }
                    }
                    else
                    {
                        txtChat.AppendText(token + " ");
                    }
                }

                txtChat.AppendText(Environment.NewLine);
                txtChat.SelectionColor = txtChat.ForeColor;

                // Highlight join:// links
                MatchCollection matches = Regex.Matches(text, @"join://[^\s]+");
                foreach (Match match in matches)
                {
                    int linkStart = txtChat.Text.LastIndexOf(match.Value);
                    if (linkStart >= 0)
                    {
                        txtChat.Select(linkStart, match.Length);
                        txtChat.SelectionColor = Color.BlueViolet;
                        txtChat.SelectionFont = new Font(txtChat.Font, FontStyle.Underline);
                    }
                }

                // Scroll to bottom 
                txtChat.SelectionStart = txtChat.Text.Length;
                txtChat.ScrollToCaret();

                // Save plain text (not images) for logging
                LogChatLine(text);
            }
            catch (ObjectDisposedException)
            {
                // disposed — ignore
            }
            catch (Exception ex)
            {
                // debugging
                Debug.WriteLine("[AppendChatLog ERROR] " + ex.Message);
            }
        }




        private void LogChatLine(string line)
        {
            try
            {
                string logsDir = Path.Combine(Application.StartupPath, "IRCLogs");
                if (!Directory.Exists(logsDir))
                    Directory.CreateDirectory(logsDir);

                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string logFile = Path.Combine(logsDir, $"IRCLog_{date}.txt");

                File.AppendAllText(logFile, $"[{DateTime.Now:HH:mm:ss}] {line}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                // silently ignore or log 
                Debug.WriteLine($"[Log ERROR] {ex.Message}");
            }
        }




        private void Irc_OnError(object sender, IrcErrorEventArgs e)
        {
            AppendChat($"<ERROR> {e.Data}");
        }
        // Initial NAMES list when you join a channel
        private void Irc_OnNames(object sender, NamesEventArgs e)
        {
            UpdateUserList(e.UserList);

            // Play cheer.wav when joined
            if (_cheerPlayer != null)
            {
                try
                {
                    _cheerPlayer.Play();
                }
                catch (Exception ex)
                {
                    AppendChat($"[SOUND ERROR] {ex.Message}");
                }
            }

            // Connected to IRC and joining channel welcome message
            //
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string nick = txtNick.Text.Trim();
            AppendChatLog($"Joined channel..", Color.LimeGreen);
            AppendChatLog($"On: {date}", Color.Green);
            AppendChatLog($"", Color.White);
            AppendChatLog($"Welcome {nick}!", Color.Cyan);
            AppendChatLog($"________________________", Color.DarkCyan);
            AppendChatLog($"", Color.White);
        }


        // Someone joins—add them to the user list
        private void Irc_OnJoin(object sender, JoinEventArgs e)
        {
            string msg = e.Data.Message;
            string nick = e.Data.Nick;

            Console.WriteLine($"[IRC_MSG] From {nick}: {msg}");

            AddUser(e.Who);

            // Only set join time if we haven't already (prevents 1ms bug on hover)
            if (!_userJoinTimes.ContainsKey(e.Who))
                _userJoinTimes[e.Who] = DateTime.Now;

            string localNick = txtNick.Text.Trim();
            string joinedUser = e.Who;
            string channel = txtChannel.Text.Trim();

            Console.WriteLine($"[DEBUG] User joined: {joinedUser}");
            Console.WriteLine($"[DEBUG] Local nick: {localNick}");
            Console.WriteLine($"[DEBUG] Are we AFK? {_afkUsers.Contains(localNick)}");

            // If the user who joined isn't us, and we're AFK, re-broadcast our AFK status
            if (!joinedUser.Equals(localNick, StringComparison.OrdinalIgnoreCase) &&
                _afkUsers.Contains(localNick))
            {
                Console.WriteLine($"[DEBUG] Scheduling AFK re-broadcast in 200ms...");

                Task.Delay(200).ContinueWith(_ =>
                {
                    try
                    {
                        _irc.RfcPrivmsg(channel, $"[AFK:{localNick}]");
                        AppendChatLog($"[SYNC] Re-broadcasted AFK status for {localNick}", Color.Gray);
                        SetUserAfk(nick, true);
                        Console.WriteLine($"[DEBUG] Broadcasted: [AFK:{localNick}]");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Failed to send AFK message: {ex.Message}");
                    }
                });
            }
        }


        // Someone leaves the channel
        private void Irc_OnPart(object sender, PartEventArgs e)
        {
            RemoveUser(e.Who);
        }

        // Someone quits the network (remove from *all* channels)
        private void Irc_OnQuit(object sender, QuitEventArgs e)
        {
            RemoveUser(e.Who);
        }

        // nick‐change—update the existing item
        private void Irc_OnNickChange(object sender, NickChangeEventArgs e)
        {
            RenameUser(e.OldNickname, e.NewNickname);
        }


        // helper: replace the entire list
        private void UpdateUserList(IEnumerable<string> users)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke(new Action(() => UpdateUserList(users)));
                return;
            }

            lvUsers.BeginUpdate();
            lvUsers.Items.Clear();

            foreach (var u in users.OrderBy(n => n))
            {
                string imageKey = "online";

                if (_userStatus.TryGetValue(u, out string status))
                {
                    imageKey = status;
                }
                else if (_afkUsers.Contains(u))
                {
                    imageKey = "afk";
                }

                ListViewItem item = new ListViewItem(u)
                {
                    ImageKey = imageKey
                };
                lvUsers.Items.Add(item);
            }

            lvUsers.EndUpdate();
        }


        // add a single user
        private void AddUser(string nick)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke(new Action(() => AddUser(nick)));
                return;
            }
            // avoid dupes
            if (lvUsers.Items.Cast<ListViewItem>()
                       .Any(i => i.Text.Equals(nick, StringComparison.OrdinalIgnoreCase)))
                return;

            ListViewItem item = new ListViewItem(nick);
            item.ImageKey = "online";
            lvUsers.Items.Add(item);

        }

        // UI‐helper: remove a single user
        private void RemoveUser(string nick)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke(new Action(() => RemoveUser(nick)));
                return;
            }
            var item = lvUsers.Items.Cast<ListViewItem>()
                          .FirstOrDefault(i => i.Text.Equals(nick, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                lvUsers.Items.Remove(item);
        }

        // UI‐helper: rename a user
        private void RenameUser(string oldNick, string newNick)
        {
            if (lvUsers.InvokeRequired)
            {
                lvUsers.Invoke(new Action(() => RenameUser(oldNick, newNick)));
                return;
            }
            var item = lvUsers.Items.Cast<ListViewItem>()
                          .FirstOrDefault(i => i.Text.Equals(oldNick, StringComparison.OrdinalIgnoreCase));
            if (item != null)
                item.Text = newNick;
        }

        // Send the chat message
        private void btnSendIRC_Click(object sender, EventArgs e)
        {
            var msg = txtInput.Text.Trim();
            var channel = txtChannel.Text.Trim();
            var nick = txtNick.Text.Trim();
            var nickKey = nick.ToLowerInvariant();

            if (msg.Length == 0)
            {
                AppendChat("[WARN] Cannot send empty message.");
                return;
            }
            if (!_irc.IsConnected)
            {
                AppendChat("[WARN] Not connected—click Connect first.");
                return;
            }
            if (!channel.StartsWith("#"))
            {
                AppendChat("[WARN] Channel must start with ‘#’.");
                return;
            }

            // how long has the user been online?
            // ~ ensure we have an online timestamp for this user
            if (!_userJoinTimes.ContainsKey(nickKey))
            {
                _userJoinTimes[nickKey] = DateTime.Now;
            }

            try
            {
                _irc.RfcPrivmsg(channel, msg);
                // Add to history (avoid duplicates)
                if (!string.IsNullOrWhiteSpace(msg) &&
                    (_messageHistory.Count == 0 || _messageHistory[_messageHistory.Count - 1] != msg))
                {
                    _messageHistory.Add(msg);
                    if (_messageHistory.Count > 50)
                        _messageHistory.RemoveAt(0); // Keep only last 50 messages
                }

                _historyIndex = _messageHistory.Count; // Reset history navigation


                AppendChatLog($"{nick}: {msg}", Color.LightBlue);
                txtInput.Clear();

                if (!Regex.IsMatch(msg, @"\/sound\s+\w+", RegexOptions.IgnoreCase) && _msgPlayer != null)
                {
                    try { _msgPlayer.Play(); }
                    catch (Exception ex) { AppendChat($"[SOUND ERROR] {ex.Message}"); }
                }

            }
            catch (Exception ex)
            {
                AppendChat($"[SEND ERROR] {ex.Message}");
            }

            // Handle /afk command
            if (msg.Equals("/afk", StringComparison.OrdinalIgnoreCase))
            {
                SetLocalUserAfkIcon(true);
                _afkUsers.Add(nick);
                _userAfkTimestamps[nickKey] = DateTime.Now;
                _irc.RfcPrivmsg(channel, $"[AFK:{nick}]");

                return;
            }

            // Handle /back command
            if (msg.Equals("/back", StringComparison.OrdinalIgnoreCase))
            {
                SetLocalUserAfkIcon(false);
                _afkUsers.Remove(nick);
                _userAfkTimestamps.Remove(nickKey);
                _irc.RfcPrivmsg(channel, $"[BACK:{nick}]");
                return;
            }

            // Auto-clear AFK if user types *any* message (except /back)
            if (_userAfkTimestamps.ContainsKey(nickKey) &&
                !msg.Equals("/back", StringComparison.OrdinalIgnoreCase))
            {
                SetLocalUserAfkIcon(false);
                _afkUsers.Remove(nick);
                _userAfkTimestamps.Remove(nickKey);
                _irc.RfcPrivmsg(channel, $"[BACK:{nick}]");
                Console.WriteLine($"[DEBUG] Auto-removed AFK due to user activity.");
            }
        }



        private void SetLocalUserAfkIcon(bool isAfk)
        {
            string localNick = txtNick.Text.Trim();

            foreach (ListViewItem item in lvUsers.Items)
            {
                if (item.Text.Equals(localNick, StringComparison.OrdinalIgnoreCase))
                {
                    item.ImageKey = isAfk ? "snooze" : "online";
                    break;
                }
            }
        }


        private void btnDisconnect_Click(object sender, EventArgs e)
        {

            StopUdpProfileListener();

            btnIRCConnect.Enabled = true;
            txtNick.Enabled = true;
            string nick = txtNick.Text.Trim();
            string channel = txtChannel.Text.Trim();

            if (_irc.IsConnected)
            {
                // Send visible "left the channel" message to all clients
                _irc.RfcPrivmsg(channel, $"[{nick}] left the channel.");
                _irc.RfcQuit("Bye!");
                _irc.Disconnect();
            }

            // Wait for listener thread to shut down
            _listenThread?.Join(500);

            lvUsers.Items.Clear(); // Clear the user list
            _afkUsers.Clear(); //  Reset all AFK states on disconnect



            if (_msgHostSound != null)
            {
                try
                {
                    _msgHostSound.Play();
                }
                catch (Exception ex)
                {
                    AppendChat($"[SOUND ERROR] {ex.Message}");
                }
            }

            // Play local disconnect sound 
            if (_msgDisconnect != null)
            {
                try
                {
                    _msgDisconnect.Play();
                }
                catch (Exception ex)
                {
                    AppendChat($"[SOUND ERROR] {ex.Message}");
                }
            }

            AppendChatLog("[STATUS] Disconnected...", Color.IndianRed);

        }


        private void AppendChat(string line)
        {
            Color color = GetColorForTag(line);
            AppendChatLog(line, color);
        }

        private Color GetColorForTag(string msg)
        {
            if (msg.Contains("[STATUS]")) return Color.Lime;
            if (msg.Contains("[WARN]")) return Color.Brown;
            if (msg.Contains("[INFO]")) return Color.LimeGreen;
            if (msg.Contains("[ERROR]")) return Color.Red;
            return txtChat.ForeColor; // default ~ light blue?
        }

        // This handles if a link is clicked in chat but already in game
        private void txtChat_MouseDown(object sender, MouseEventArgs e)
        {

            int index = txtChat.GetCharIndexFromPosition(e.Location);
            if (index < 0 || index >= txtChat.Text.Length)
                return;

            string fullText = txtChat.Text;

            int start = index;
            while (start > 0 && !char.IsWhiteSpace(fullText[start - 1]) && fullText[start - 1] != '\n')
                start--;

            int end = index;
            while (end < fullText.Length && !char.IsWhiteSpace(fullText[end]) && fullText[end] != '\n')
                end++;

            string word = fullText.Substring(start, end - start);
            System.Diagnostics.Debug.WriteLine("Clicked word: " + word);

            if (word.StartsWith("join://"))
            {
                HandleJoinLink(word);
            }
        }


        // Join link:
        private void HandleJoinLink(string link)
        {
            if (_isHosting || (_userStatus.TryGetValue(txtNick.Text.Trim(), out string status) && status == "ingame"))
            {
                AppendChatLog("[INFO] You're already in a game! * Return before joining another.", Color.Red);
                return;
            }

            try
            {
                string[] parts = link.Substring("join://".Length).Split('|');
                string[] ipPort = parts[0].Split(':');
                string ip = ipPort[0];
                int port = int.Parse(ipPort[1]);

          
                List<string> pwads = new List<string>();
                string rom = "";

                // Parse the remaining parts
                for (int i = 1; i < parts.Length; i++)
                {

                    if (parts[i].StartsWith("rom=", StringComparison.OrdinalIgnoreCase))
                    {
                        rom = parts[i].Substring("rom=".Length).Trim();
                    }
                }

                // Handle CSUME join
                if (!string.IsNullOrEmpty(rom))
                {
                    string romsFolder = txtRomsDefaultPath.Text.Trim();
                    if (string.IsNullOrEmpty(romsFolder) || !Directory.Exists(romsFolder))
                    {
                        AppendChatLog("[ERROR] ROMs folder is not set or doesn't exist.", Color.Red);
                        return;
                    }

                    string fullRomPath = Path.Combine(romsFolder, rom);
                    if (!File.Exists(fullRomPath))
                    {
                        AppendChatLog($"[ERROR] ROM file '{rom}' not found in ROMs folder.", Color.Red);
                        return;
                    }

                    string csumeExe = txtCSUMELocation.Text.Trim();
                    if (string.IsNullOrEmpty(csumeExe) || !File.Exists(csumeExe))
                    {
                        AppendChatLog("[ERROR] CSUME executable is not set or missing.", Color.Red);
                        return;
                    }

                    string args = $"\"{fullRomPath}\" -client -port {port} -hostname {ip}";
                    string workingDir = Path.GetDirectoryName(csumeExe);

                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = csumeExe,
                            Arguments = args,
                            WorkingDirectory = workingDir,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Normal
                        };

                        var proc = Process.Start(psi);
                        if (proc != null)
                        {
                            _irc.RfcPrivmsg(txtChannel.Text, $"[INGAME:{txtNick.Text}]");
                          
                            string nick = txtNick.Text.Trim();
                            _userStatus[nick] = "ingame";
                            SetUserInGame(nick, true);
                            UpdateUserIcon(nick);

                            // Apply dark read-only theme when in-game
                            txtChat.ReadOnly = true;
                            txtChat.BackColor = Color.FromArgb(30, 30, 30);
                            dgHostedServer.Enabled = false;


                            proc.EnableRaisingEvents = true;
                            proc.Exited += (s, e) =>
                            {
                                this.Invoke((MethodInvoker)(() =>
                                {
             
                                    _userStatus[nick] = "online";
                                    SetUserInGame(nick, false);
                                    UpdateUserIcon(nick);  

                                    txtChat.ReadOnly = false;
                                    txtChat.BackColor = Color.Black;
                                 //   txtChat.ForeColor = Color.White;
                                    dgHostedServer.Enabled = true;
                                }));

                                if (_irc?.IsConnected == true)
                                {
                                    _irc.RfcPrivmsg(txtChannel.Text, $"[{nick}] returned from game.");
                                    AppendChatLog($"[{nick}] returned from game.", Color.Green);
                                }
                            };
                        }

                        AppendChatLog($"[CSUME] Connecting to {ip}:{port} with ROM '{rom}'", Color.LightBlue);
                    }
                    catch (Exception ex)
                    {
                        AppendChatLog("[ERROR] Failed to launch CSUME: " + ex.Message, Color.Red);
                    }

                    return;
                }

 

                string gameExecutable = GetSelectedEngineExecutable();
                if (string.IsNullOrEmpty(gameExecutable) || !File.Exists(gameExecutable))
                {
                    AppendChatLog("[ERROR] Game executable not found.", Color.Red);
                    return;
                }

      
            }
            catch (Exception ex)
            {
                AppendChat($"[JOIN ERROR] {ex.Message}");
            }
        }


        private void txtChat_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            string localNick = txtNick.Text.Trim();

            // Prevent joining if already in-game or hosting
            if (_isHosting || (_userStatus.ContainsKey(localNick) && _userStatus[localNick] == "ingame"))
            {
                AppendChatLog("[INFO] You're already in a game. Return before joining another.", Color.Red);
                return;
            }

            string link = e.LinkText;

            // Handle join:// links (game join)
            if (link.StartsWith("join://", StringComparison.OrdinalIgnoreCase))
            {
                HandleJoinLink(link); // or HandleJoinLink(link); based on your setup
                return;
            }

            // Handle external URLs (http, https, ftp)
            if (link.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                link.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                link.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    System.Diagnostics.Process.Start(link);
                }
                catch (Exception ex)
                {
                    AppendChat($"[ERROR] Could not open link: {ex.Message}");
                }
            }
        }


        private void txtChat_MouseMove(object sender, MouseEventArgs e)
        {
            int index = txtChat.GetCharIndexFromPosition(e.Location);

            if (index < 0 || index >= txtChat.Text.Length)
            {
                txtChat.Cursor = Cursors.Default;
                return;
            }

            // Extract full word at mouse location
            int start = index;
            string text = txtChat.Text;

            while (start > 0 && !char.IsWhiteSpace(text[start - 1])) start--;

            int end = index;
            while (end < text.Length && !char.IsWhiteSpace(text[end])) end++;

            string word = text.Substring(start, end - start);

            if (word.StartsWith("join://"))
                txtChat.Cursor = Cursors.Hand;
            else
                txtChat.Cursor = Cursors.Default;
        }


        // User online or AFK time duration
        private string FormatDuration(TimeSpan span)
        {
            if (span.TotalHours >= 1)
                return $"{(int)span.TotalHours}h {span.Minutes}m";
            else if (span.TotalMinutes >= 1)
                return $"{(int)span.TotalMinutes}m {span.Seconds}s";
            else
                return $"{span.Seconds}s";
        }

        private void Irc_OnDisconnected(object sender, EventArgs e)
        {
            AppendChat("[NOTICE] Disconnected from server.");

            string nick = txtNick.Text.Trim();
            string channel = txtChannel.Text.Trim();

            AppendChat($"[AUTO] Attempting to reconnect as {nick}...");

            Task.Run(() =>
            {
                Thread.Sleep(3000); // brief delay
                TryReconnect(channel, nick);
            });
        }

        private void TryReconnect(string channel, string nick)
        {
            const int maxAttempts = 3;

            for (int i = 1; i <= maxAttempts; i++)
            {
                try
                {
                    AppendChat($"[RECONNECT] Attempt {i} of {maxAttempts}...");
                    _irc.Connect(txtServer.Text.Trim(), int.Parse(txtPort.Text));
                    _irc.Login(nick, nick);
                    _irc.RfcJoin(channel);
                    _irc.RfcPrivmsg(channel, $"[{nick}] rejoined after disconnect.");
                    AppendChat($"[RECONNECT] Successfully rejoined {channel}.");
                    return;
                }
                catch (Exception ex)
                {
                    AppendChat($"[RECONNECT ERROR] {ex.Message}");
                    Thread.Sleep(3000); // retry delay
                }
            }

            AppendChat("[RECONNECT] All attempts failed. Manual reconnection required.");
        }


        private void Irc_OnUserQuit(object sender, QuitEventArgs e)
        {
            string nick = e.Who;
            AppendChat($"[INFO] {nick} has disconnected.");
        }
        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (_messageHistory.Count > 0 && _historyIndex > 0)
                {
                    _historyIndex--;
                    txtInput.Text = _messageHistory[_historyIndex];
                    txtInput.SelectionStart = txtInput.Text.Length;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (_messageHistory.Count > 0 && _historyIndex < _messageHistory.Count - 1)
                {
                    _historyIndex++;
                    txtInput.Text = _messageHistory[_historyIndex];
                    txtInput.SelectionStart = txtInput.Text.Length;
                }
                else
                {
                    _historyIndex = _messageHistory.Count;
                    txtInput.Clear();
                }
                e.Handled = true;
            }
        }


        private void lvUsers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = lvUsers.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        // Right click get user info..
        private void viewStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count == 0)
                return;

            string nick = lvUsers.SelectedItems[0].Text;
            string localNick = txtNick.Text.Trim(); // Local logged-in nickname

            string onlineStr = _userJoinTimes.ContainsKey(nick)
                ? FormatDuration(DateTime.Now - _userJoinTimes[nick])
                : "Unknown";

            string afkStr = _afkUsers.Contains(nick)
                ? (_userAfkTimestamps.ContainsKey(nick)
                    ? FormatDuration(DateTime.Now - _userAfkTimestamps[nick])
                    : "Currently AFK")
                : "No";

            string profilePath = Path.Combine(Application.StartupPath, "ProfilePic", nick + ".png");

            // Load image safely without locking the image file
            Image profileImage = null;
            if (File.Exists(profilePath))
            {
                try
                {
                    using (FileStream fs = new FileStream(profilePath, FileMode.Open, FileAccess.Read))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        profileImage = Image.FromStream(ms);
                    }
                }
                catch
                {
                    profileImage = null;
                }
            }

            // Pass localNick to compare inside the form
            var statusForm = new UserStatusForm(nick, onlineStr, afkStr, profileImage, localNick);
            statusForm.ProfilePictureChanged += (changedNick, imagePath) =>
            {
                BroadcastProfilePicture(changedNick, imagePath);
            };
            statusForm.Show();
        }



        // Is the users connection actually still active?
        private void CheckIrcConnectionAlive(object sender, EventArgs e)
        {
            if (_irc == null || !_irc.IsConnected)
            {
                _connectionCheckTimer?.Stop();

                AppendChatLog("[STATUS] Connection to IRC lost.", Color.OrangeRed);

                string nick = txtNick.Text.Trim();
                string channel = txtChannel.Text.Trim();

                if (!string.IsNullOrWhiteSpace(nick) && !string.IsNullOrWhiteSpace(channel))
                {
                    // Local notice to room 
                    AppendChatLog($"[{nick}] has lost connection to IRC.", Color.DarkOrange);

                    // Optionally attempt visual feedback in UI
                    foreach (ListViewItem item in lvUsers.Items)
                    {
                        if (item.Text.Equals(nick, StringComparison.OrdinalIgnoreCase))
                        {
                            item.ImageKey = "snooze";
                            break;
                        }
                    }
                }
            }
        }

        // User might have lost connection?
        private void CheckRealConnection(object sender, EventArgs e)
        {
            try
            {
                if (_irc == null || !_irc.IsConnected)
                {
                    _pingTimer?.Stop();
                    AppendChatLog("[STATUS] IRC connection is no longer alive.", Color.OrangeRed);

                    // Broadcast to channel that user disconnected (if channel and nick are available)
                    string nick = txtNick.Text.Trim();
                    string channel = txtChannel.Text.Trim();
                    if (!string.IsNullOrWhiteSpace(nick) && !string.IsNullOrWhiteSpace(channel))
                    {
                        try
                        {
                            _irc.RfcPrivmsg(channel, $"[{nick}] has disconnected unexpectedly.");
                        }
                        catch { /* ignore if sending fails */ }
                    }

                    return;
                }

                // Send a harmless ping to force a socket check
                _irc.WriteLine("PING check", Priority.Low);
            }
            catch (Exception ex)
            {
                _pingTimer?.Stop();
                AppendChatLog($"[PING FAIL] Lost connection: {ex.Message}", Color.Red);
            }
        }

        private void SendStatusMessage(string message)
        {
            if (!_irc.IsConnected) return;

            _irc.RfcPrivmsg(txtChannel.Text, message);
        }

      
        private void BroadcastProfilePicture(string nick, string imagePath)
        {
            try
            {
                if (!File.Exists(imagePath)) return;

                byte[] imageData = File.ReadAllBytes(imagePath);

                using (var udp = new UdpClient())
                {
                    udp.EnableBroadcast = true;

                    using (var ms = new MemoryStream())
                    using (var writer = new BinaryWriter(ms))
                    {
                        writer.Write("PROFILE_PIC");
                        writer.Write(nick);
                        writer.Write(imageData.Length);
                        writer.Write(imageData);

                        byte[] payload = ms.ToArray();
                        udp.Send(payload, payload.Length, new IPEndPoint(System.Net.IPAddress.Broadcast, 45291));
                    }
                }
            }
            catch (Exception ex)
            {
                AppendChat($"[ERROR] Failed to broadcast profile picture: {ex.Message}");
            }
        }


        private void ClearReadyStatusForUser(string user)
        {
            if (_readyUsers.Contains(user))
                _readyUsers.Remove(user);

            this.Invoke((MethodInvoker)(() =>
            {
                foreach (ListViewItem item in lvUsers.Items)
                {
                    if (item.Text == user)
                    {
                        item.ImageKey = "online";
                        break;
                    }
                }

                if (user == txtNick.Text.Trim())
                    readyUpToolStripMenuItem.Checked = false;
            }));

            if (_irc?.IsConnected == true)
                _irc.RfcPrivmsg(txtChannel.Text, $"[UNREADY:{user}]");
        }

        // End IRC --------------------------------------------------------------





        // Game Engine Setup ----------------------------------------------------

        // Get selected game, if not there, there's a error message
        // This was actually setup if more game engines are added, maybe later.
        private string GetSelectedEngineExecutable()
        {
            if (cmbEngine.SelectedItem == null)
                return "";

            switch (cmbEngine.SelectedItem.ToString())
            {
                case "CSUME": return txtCSUMELocation.Text;
                default: return "";
            }
        }
        ////private void btnCSUMELocation_Click(object sender, EventArgs e)
        ////{
        ////    OpenFileDialog openFileDialog = new OpenFileDialog();
        ////    openFileDialog.Filter = "CSUME Executable|CSUME.exe";
        ////    if (openFileDialog.ShowDialog() == DialogResult.OK)
        ////    {
        ////        txtCSUMELocation.Text = openFileDialog.FileName;

        ////        Properties.Settings.Default.CSUMELocation = txtCSUMELocation.Text;
        ////        Properties.Settings.Default.Save(); 

        ////    }

        ////    if (string.IsNullOrWhiteSpace(txtCSUMELocation.Text))
        ////    {
        ////        MessageBox.Show("Please enter a path to your CSUME Engine.");
        ////    }
        ////    else
        ////    {
        ////        cmbEngine.Text = "CSUME";

        ////    }
        ////    if (cmbEngine.Text == "CSUME")
        ////    {

        ////        cmbEngine.SelectedIndex = 0; 
        ////        txtCSUMELocation.Text = openFileDialog.FileName;
        ////    }
        ////}

        private void btnBrowseRom_Click(object sender, EventArgs e)
        {


            // Set default ROMs folder path (always inside application root)
            string romsPath = Path.Combine(Application.StartupPath, "csume", "roms");

            // Ensure the folder exists (optional, but good practice)
            if (!Directory.Exists(romsPath))
                Directory.CreateDirectory(romsPath);

            // Open file dialog to select ROM
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.InitialDirectory = romsPath; // ✅ Set default directory
                dlg.Filter = "ROM ZIP Files (*.zip)|*.zip|All Files (*.*)|*.*";
                dlg.Title = "Select CSUME ROM";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRomPath.Text = dlg.FileName;

                    // 👇 Get only the filename (e.g., "joustwr.zip")
                    string romFileName = Path.GetFileName(dlg.FileName);

                    // 👇 Display it in the label
                    lblLoadedROM.Text = $"{romFileName}";

                    Properties.Settings.Default.RomName = lblLoadedROM.Text;
                    Properties.Settings.Default.RomPath = txtRomPath.Text;
                    Properties.Settings.Default.Save();

                    // if no ROM has ever been loaded, we display a welcome message:
                    rtbWelcome.Text = "\n\n\n        Thank you for using Arcade Connector!\n\n       Please select a ROM file to get started.";

                    // Toggle visibility based on whether a ROM was previously selected
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.RomName))
                        rtbWelcome.Visible = false;
                    else
                        rtbWelcome.Visible = true;

                    //...


                    UpdateSnapPreviewFromRom(dlg.FileName); // 🔁 Load matching preview

                    UpdateCommandLine();
                }
            }
        }





        private void UpdateSnapPreviewFromRom(string romFilePath)
        {
            string romFileName = Path.GetFileNameWithoutExtension(romFilePath); // e.g., "dkong"
            string snapPath = Path.Combine(Application.StartupPath, "csume", "artwork", romFileName + ".png");
            string altPath = Path.Combine(Application.StartupPath, "csume", "artpreview", romFileName + ".png");

            if (File.Exists(snapPath))
            {
                pbSnap.Image = Image.FromFile(snapPath);
            }
            else if (File.Exists(altPath))
            {
                pbSnap.Image = Image.FromFile(altPath);
            }
            else
            {
                string defaultPath = Path.Combine(Application.StartupPath, "csume", "artwork", "default.png");
                if (File.Exists(defaultPath))
                    pbSnap.Image = Image.FromFile(defaultPath);
                else
                    pbSnap.Image = null;
            }
        }




        ////private void btnROMsLocation_Click(object sender, EventArgs e)
        ////{
        ////    FolderBrowserDialog fbd = new FolderBrowserDialog();
        ////    fbd.Description = "Select your ROMs Location..";
        ////    if (fbd.ShowDialog() == DialogResult.OK && fbd.SelectedPath.Length > 0)
        ////    {
        ////        txtRomsDefaultPath.Text = fbd.SelectedPath.ToString();
        ////        Properties.Settings.Default.RomsDefaultPath = txtRomsDefaultPath.Text; 
        ////        Properties.Settings.Default.Save(); 

        ////    }
        ////}

        private void readyUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count == 0) return;

            var selectedItem = lvUsers.SelectedItems[0];
            string user = selectedItem.Text;

            // Toggle state
            bool isReady = selectedItem.ImageKey == "user_ready";

            if (isReady)
            {
                selectedItem.ImageKey = "online";
                _readyUsers.Remove(user);

                if (_irc != null && _irc.IsConnected && user == txtNick.Text)
                    _irc.RfcPrivmsg(txtChannel.Text, $"[UNREADY:{user}]");
            }
            else
            {
                selectedItem.ImageKey = "user_ready";
                _readyUsers.Add(user);

                if (_irc != null && _irc.IsConnected && user == txtNick.Text)
                    _irc.RfcPrivmsg(txtChannel.Text, $"[READY:{user}]");
            }
        }

        private void btnShowInfo_Click(object sender, EventArgs e)
        {
            rtbInfo.Visible = true;
            btnShowInfo.Visible = false;
            btnCloseInfo.Visible = true;
            // Show a message box with game instructions
           rtbInfo.Text = "\n   After entering a game, press [5] to insert a coin.\n\n" +
                "   Press [1] or [2] for a 1 or 2 player game.\n\n   If in a Network game:\n" +
                "   Player 2 needs to select [1] to start a 2 player game.\n\n" +
                "   Press [N] to toggle network info.\n\n" +
                "   Most Controls are [Ctrl], [Alt], and [Arrow keys].\n" +
                "   This, however depends on the game.\n\n" +
                "   Select [Tab] to setup input controls\n" +
                "   or other in-game configurations if needed.\n\n" +
                "   *Note: Some games are designed to only allow\n" +
                "   a 2 player game on the same machine.  ";
        }

        private void btnCloseInfo_Click(object sender, EventArgs e)
        {
            btnShowInfo.Visible = true;
            btnCloseInfo.Visible = false;
            rtbInfo.Visible = false;
        }



        // End Game Engine Setup ---------------------------------------------------

    }
}