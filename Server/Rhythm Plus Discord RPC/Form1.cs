using System;
using System.Windows.Forms;
using DiscordRPC;
using DiscordRPC.Logging;
using Fleck;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Linq;

namespace Rhythm_Plus_Discord_RPC
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;



        private WebSocketServer server;
        private IWebSocketConnection currentSocket;
        private bool running = false;

        public DiscordRpcClient client;
        public bool failedRpConnection = false;

        public bool forceUpdate = false;
        public bool firstBoot = false;
        public bool enableRPC = false;
        public bool hasLostConnection = true;
        public string prevDataRP = "";

        public DiscordRPC.Button playButton = new DiscordRPC.Button();
        public DateTime startRPC;
        private DateTime lastMessageTime = DateTime.MinValue;

        public SaveManager saveManager = new SaveManager();

        public float currentAccuracy = 0.0f;
        public string currentScore = "";
        public float currentTime = 0.0f;

        public string resultAccuracy = "";
        public string resultRank = "";
        public string resultMaxCombo = "";
        public string resultScore = "";
        public string resultFC = "";

        public string selectedSongName = "";
        public string selectedSongAuthor = "";
        public string selectedSongCharter = "";
        public string selectedSongTitle = "";

        public string currentUri = "";
        public string currentTitle = "";

        public int port = 55256;
        public int timeOut = 3;

        public bool forceQuit = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void setUpRPC()
        {
            client = new DiscordRpcClient("1331684607199936552");

            client.Logger = new ConsoleLogger() { Level = DiscordRPC.Logging.LogLevel.Warning };

            client.OnReady += (sender, e) =>
            {
                Logging.logString("Received Ready from user " + e.User.Username);
                failedRpConnection = false;
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                if (e.Presence != null)
                {
                    Logging.logString("Received Update - " + e.Presence.Details);
                }
            };

            client.OnConnectionFailed += (sender, e) =>
            {
                Logging.logString("Failed to connect to discord - " + e.Type.ToString());

                saveManager.saveData();
                enableRPC = false;

                client.Dispose();
            };

            client.OnClose += (sender, e) =>
            {
                Logging.logString("Connection closed to discord - " + e.Reason);
            };

            client.Initialize();

            startRPC = DateTime.UtcNow;

            playButton = new DiscordRPC.Button();
            playButton.Label = "Play Rhythm Plus";
            playButton.Url = "https://rhythm-plus.com";

            setPresence();
        }

        public void setPresence()
        {
            try
            {
                if (hasLostConnection)
                {
                    hasLostConnection = false;
                    startRPC = DateTime.UtcNow;
                }

                string point = "Playing Rhythm Plus";
                string uri = currentUri;
                bool forceUpdate = false;

                if (uri.Equals("https://rhythm-plus.com"))
                {
                    point = "On the into screen";
                }
                else if (uri.StartsWith("https://rhythm-plus.com/menu/"))
                {
                    point = "Looking at songs";
                }
                else if (uri.Equals("https://rhythm-plus.com/studio/") || uri.StartsWith("https://rhythm-plus.com/editor/"))
                {
                    point = "Creating a chart";
                }
                else if (uri.Equals("https://rhythm-plus.com/account/"))
                {
                    point = "Changing settings";
                }
                else if (uri.Equals("https://rhythm-plus.com/tutorial/"))
                {
                    point = "Playing the tutorial";
                }
                else if (uri.StartsWith("https://rhythm-plus.com/result/"))
                {
                    point = "Looking at results";
                    forceUpdate = true;
                }
                else if (uri.StartsWith("https://rhythm-plus.com/game-over/"))
                {
                    point = "Failed a chart";
                }
                else if (uri.StartsWith("https://rhythm-plus.com/game/"))
                {
                    string songName = currentTitle.Split(new string[] { " - Rhythm+ Music" }, StringSplitOptions.None)[0];
                    if (songName == "Game")
                    {
                        point = "Loading a song";
                    }
                    else
                    {
                        if (showCurrentChartCheckbox.Checked)
                        {
                            if (songName == selectedSongTitle)
                            {
                                point = $"Playing '{selectedSongName} -by- {selectedSongAuthor}' [{selectedSongCharter}]";
                            }
                            else
                            {
                                point = $"Playing '{songName}'";
                            }
                        }
                        else
                        {
                            point = "Playing a chart";
                        }

                            forceUpdate = true;
                    }
                }

                if (prevDataRP != point || forceUpdate)
                {
                    string state = "";
                    if (uri.StartsWith("https://rhythm-plus.com/game/") && showCurrentStatsCheckbox.Checked)
                    {
                        if (currentScore != "")
                        {
                            string rank = "F";
                            if (currentAccuracy == 0)
                            {
                                rank = "?";
                            }
                            else if (currentAccuracy > 99f)
                            {
                                rank = "S+";
                            }
                            else if (currentAccuracy > 97f)
                            {
                                rank = "S";
                            }
                            else if (currentAccuracy > 94f)
                            {
                                rank = "A";
                            }
                            else if (currentAccuracy > 90f)
                            {
                                rank = "B";
                            }
                            else if (currentAccuracy > 80f)
                            {
                                rank = "C";
                            }
                            else if (currentAccuracy > 60f)
                            {
                                rank = "D";
                            }

                            state = $" - Score: {currentScore} - Acc: {Math.Round(currentAccuracy * 10) / 10}% - Rank: ~{rank} - Point: {Math.Round(currentTime * 10) / 10}%";
                        }
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/result/") && showCurrentStatsCheckbox.Checked)
                    {
                        if (resultScore != "")
                        {
                            if (resultFC == "Full Combo")
                            {
                                state = $" - [FC] - Score: {resultScore} - Acc: {resultAccuracy}% - Rank: {resultRank} - Max Combo: {resultMaxCombo}";
                            }
                            else
                            {
                                state = $" - Score: {resultScore} - Acc: {resultAccuracy}% - Rank: {resultRank} - Max Combo: {resultMaxCombo}";
                            }
                        }
                    }

                    client.SetPresence(new RichPresence()
                    {
                        Details = point,
                        Timestamps = new Timestamps()
                        {
                            Start = startRPC
                        },
                        Assets = new Assets()
                        {
                            LargeImageKey = "logo",
                            LargeImageText = "Rhythm Plus - Discord RPC",
                            SmallImageKey = "icon",
                            SmallImageText = "App by Splamei"
                        },
                        Buttons = new DiscordRPC.Button[]
                        {
                                playButton
                        },
                        State = state
                    });

                    prevDataRP = point;

                    forceUpdate = false;
                }
            }
            catch (Exception ex)
            {
                Logging.logString("Error! - " + ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (running) return;

            var exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;
            if (exists)
            {
                MessageBox.Show("You seem to have the server app already running in a seperate instance. If you want to close it or configure settings for the app, find it's icon in the system tray, right click and press 'Open'.", "Rhythm Plus Discord RPC", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                forceQuit = true;
                this.Close();
                return;
            }

            saveManager.loadData();

            timeOut = saveManager.getInt("timeout");
            port = saveManager.getInt("port");

            if (timeOut >= 8 || timeOut <= 3) { timeOut = 3; }
            if (port >= 65535 || port <= 49152) { port = 55256; }


            timeoutUpDown.Value = timeOut;
            portUpDown.Value = port;

            if (saveManager.getInt("hideCurrentStats") == 0)
            {
                showCurrentStatsCheckbox.Checked = true;
            }

            if (saveManager.getInt("hideCurrentChart") == 0)
            {
                showCurrentChartCheckbox.Checked = true;
            }

            if (timeOut == 0)
            {
                timeOut = 4000;
                saveManager.setInt("timeout", 4);
            }

            timer1.Interval = timeOut * 1000;

            try
            {
                server = new WebSocketServer("ws://0.0.0.0:" + port);
                server.Start(socket =>
                {
                    currentSocket = socket;

                    socket.OnOpen = () =>
                    {
                        lastMessageTime = DateTime.Now;
                        Console.WriteLine("Connected to the extension and ready");
                    };

                    socket.OnMessage = message =>
                    {
                        try
                        {
                            lastMessageTime = DateTime.Now;

                            var data = JsonConvert.DeserializeObject<PageData>(message);
                            if (data == null) return;

                            this.Invoke(new Action(() =>
                            {
                                lblStatus.Text = "Connected and showing Rich Presence";
                            }));

                            currentUri = data.url;
                            currentTitle = data.title;

                            if (float.TryParse(data.currentAccuracy, out currentAccuracy)) { }
                            currentScore = data.currentScore;
                            if (float.TryParse(data.currentTime.Replace("%", ""), out currentTime)) { }

                            resultAccuracy = data.resultAccuracy;
                            resultRank = data.resultRank;
                            resultMaxCombo = data.resultMaxCombo;
                            resultScore = data.resultScore;
                            resultFC = data.resultFC;

                            if (!String.IsNullOrEmpty(data.selectedSongName))
                            {
                                selectedSongName = data.selectedSongName;
                                selectedSongAuthor = data.selectedSongAuthor;
                                selectedSongCharter = data.selectedSongCharter;
                                selectedSongTitle = data.selectedSongTitle;
                            }

                            if (client == null)
                            {
                                setUpRPC();
                            }
                            else
                            {
                                setPresence();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.logString($"Error setting rich presence! - " + ex);
                            this.Invoke(new Action(() =>
                            {
                                lblStatus.Text = "Something went wrong setting RPC";
                            }));
                        }
                    };
                });

                lblStatus.Text = "Server running and ready for extension";
                running = true;
            }
            catch (Exception ex)
            {
                Logging.logString($"Error starting! - " + ex);
                lblStatus.Text = "Something went wrong starting up";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stopServices()
        {
            if (!running) return;

            try
            {
                if (currentSocket != null)
                    currentSocket.Close();

                if (client != null)
                {
                    client.ClearPresence();
                    client.Dispose();
                    client = null;
                }

                server = null;
                lblStatus.Text = "Stopped";

                saveManager.setInt("port", int.Parse(portUpDown.Value.ToString()));
                saveManager.setInt("timeout", int.Parse(timeoutUpDown.Value.ToString()));

                if (showCurrentStatsCheckbox.Checked)
                {
                    saveManager.setInt("hideCurrentStats", 0);
                }
                else
                {
                    saveManager.setInt("hideCurrentStats", 1);
                }

                if (showCurrentChartCheckbox.Checked)
                {
                    saveManager.setInt("hideCurrentChart", 0);
                }
                else
                {
                    saveManager.setInt("hideCurrentChart", 1);
                }

                saveManager.saveData();

                Logging.logString("Stopped the socket");
            }
            catch (Exception ex)
            {
                Logging.logString($"Error stopping! - " + ex);
                lblStatus.Text = "Something went wrong while stopping";
            }

            running = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lastMessageTime != DateTime.MinValue && (DateTime.Now - lastMessageTime).TotalSeconds > 6)
            {
                if (client != null)
                {
                    client.ClearPresence();
                }

                this.Invoke(new Action(() =>
                {
                    lblStatus.Text = "Lost game connection. Not showing RPC";
                    hasLostConnection = true;
                }));

                lastMessageTime = DateTime.MinValue;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (forceQuit)
            {
                stopServices();
            }
            else
            {
                if (MessageBox.Show("Would you like the server window to be hidden so it's out of your way while still allowing for Rich Presence to work or would you like to close out of the app fully and stop RPC?", "Hide or close?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    e.Cancel = true;
                    this.Opacity = 0;
                    this.Hide();

                    notifyIcon1.BalloonTipTitle = "We're now hiding in the background";
                    notifyIcon1.BalloonTipText = "We're now running in the background to allow RPC to keep running. We'll be in the system tray if you need anything";
                    notifyIcon1.ShowBalloonTip(10);
                }
                else
                {
                    stopServices();
                }
            }
        }

        public static void takeFocusOtherInstance()
        {
            var existingProcess = System.Diagnostics.Process.GetProcessesByName("Rhythm Plus DiscordRPC").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);
            if (existingProcess != null)
            {
                IntPtr handle = existingProcess.MainWindowHandle;
                ShowWindow(handle, SW_RESTORE);
                SetForegroundWindow(handle);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Licences licences = new Licences())
            {
                licences.ShowDialog();
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Opacity = 1;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
            this.Opacity = 0;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            forceQuit = true;
            this.Close();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Opacity = 1;
        }
    }

    public class PageData
    {
        public string title { get; set; }
        public string url { get; set; }

        public string selectedSongName { get; set; }
        public string selectedSongAuthor { get; set; }
        public string selectedSongCharter { get; set; }
        public string selectedSongTitle { get; set; }

        public string resultRank { get; set; }
        public string resultAccuracy { get; set; }
        public string resultScore { get; set; }
        public string resultMaxCombo { get; set; }
        public string resultFC { get; set; }

        public string currentAccuracy { get; set; }
        public string currentScore { get; set; }
        public string currentTime { get; set; }
    }
}
