using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoPatchServerGUI
{
    public partial class Form1 : Form
    {
        private System.Diagnostics.Stopwatch uptimeTimer = new();
        private System.Windows.Forms.Timer uptimeDisplayTimer = new();
        private Label statusLabel;
        private TcpListener listener;
        private Thread listenerThread;
        private bool serverRunning = false;
        private CancellationTokenSource cancelSource;

        public Form1()
        {
            InitializeComponent();
            InitStatusLabel();
            InitUptimeDisplay();

        }

        private void InitStatusLabel()
        {
            statusLabel = new Label();
            statusLabel.Name = "statusLabel";
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(506, 80);
            statusLabel.Text = "🔴 Server Offline";
            statusLabel.ForeColor = Color.Red;
            groupBox1.Controls.Add(statusLabel);
        }

        private void InitUptimeDisplay()
        {
            uptimeDisplayTimer.Interval = 1000;
            uptimeDisplayTimer.Tick += UptimeDisplayTimer_Tick;
            uptimeDisplayTimer.Start();
        }

        private void UptimeDisplayTimer_Tick(object sender, EventArgs e)
        {
            if (uptimeTimer.IsRunning)
            {
                TimeSpan uptime = uptimeTimer.Elapsed;
                label6.Text = $"Online Server: {uptime:hh\\:mm\\:ss}";
            }
            else
            {
                label6.Text = "Online Server: -";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string configPath = "ServerConfig.ini";

            if (!File.Exists(configPath))
            {
                Log("[INFO] Config file not found. Creating default ServerConfig.ini...", Color.Orange);

                try
                {
                    using StreamWriter writer = new StreamWriter(configPath);
                    writer.WriteLine("[config]");
                    writer.WriteLine("latest_patch=1000");
                    writer.WriteLine("listen_port=9528");
                    writer.WriteLine("web_hostname=yourserver.com");
                    writer.WriteLine("web_port=80");
                    writer.WriteLine("web_path=patches");
                    writer.WriteLine("autorun=false");
                }
                catch (Exception ex)
                {
                    Log($"[ERROR] Failed to create config: {ex.Message}", Color.Red);
                    return;
                }
            }

            Dictionary<string, string> config = new();

            foreach (string line in File.ReadAllLines(configPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("[") || trimmed.StartsWith(";") || trimmed == "" || trimmed.StartsWith("//"))
                    continue;

                string[] parts = trimmed.Split('=', 2);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
                    config[key] = value;
                }
            }

            if (config.ContainsKey("latest_patch") && int.TryParse(config["latest_patch"], out int latestVer))
                verNumeric.Value = Math.Clamp(latestVer, verNumeric.Minimum, verNumeric.Maximum);

            if (config.ContainsKey("listen_port") && int.TryParse(config["listen_port"], out int patchPort))
                autoNumeric.Value = Math.Clamp(patchPort, autoNumeric.Minimum, autoNumeric.Maximum);

            if (config.ContainsKey("web_port") && int.TryParse(config["web_port"], out int webPort))
                webNumeric.Value = Math.Clamp(webPort, webNumeric.Minimum, webNumeric.Maximum);

            if (config.ContainsKey("web_hostname"))
                hostBox.Text = config["web_hostname"];

            if (config.ContainsKey("web_path"))
                pathBox.Text = config["web_path"];

            Log("[INFO] Loaded ServerConfig.ini", Color.Green);

            if (config.TryGetValue("autorun", out string autoVal) && autoVal.ToLower() == "true")
            {
                autosBox.Checked = true;
                Log("[INFO] AutoStart enabled. Starting server...", Color.Green);
                startBtn.PerformClick();
            }
        }


        private void startBtn_Click(object sender, EventArgs e)
        {
            if (serverRunning)
            {
                StopServer();
                return;
            }

            SetControlsEnabled(false);
            SaveConfig();

            string latestVersion = verNumeric.Value.ToString();
            int port = (int)autoNumeric.Value;
            string hostname = hostBox.Text.Trim();
            string webPort = webNumeric.Value.ToString();
            string path = pathBox.Text.Trim();

            cancelSource = new CancellationTokenSource();
            CancellationToken token = cancelSource.Token;

            listenerThread = new Thread(() =>
            {
                try
                {
                    listener = new TcpListener(IPAddress.Any, port);
                    listener.Start();
                    serverRunning = true;
                    uptimeTimer.Restart();

                    Invoke(() =>
                    {
                        startBtn.Text = "Stop";
                        statusLabel.Text = "🟢 Server Online";
                        statusLabel.ForeColor = Color.Green;
                    });

                    Log($"[INFO] Autopatch Server started on port {port}", Color.Green);
                    string resolvedIP = ResolveHostIP(hostname);
                    string hostDisplay = (hostname == resolvedIP) ? resolvedIP : $"{hostname} | {resolvedIP}";
                    Log($"[INFO] Server IP/Host : {hostDisplay}", Color.Green);
                    Log($"[INFO] Latest Version : {latestVersion}", Color.Green);
                    Log($"[INFO] Patch folder : {path}", Color.Green);
                    Log($"[INFO] Client will patch from http://{hostname}:{webPort}/{path}/{latestVersion}.exe", Color.Green);

                    try
                    {
                        while (!token.IsCancellationRequested)
                        {
                            if (listener == null || listener.Server == null || !listener.Server.IsBound)
                                break;

                            if (!listener.Pending())
                            {
                                Thread.Sleep(100);
                                continue;
                            }

                            TcpClient client = listener.AcceptTcpClient();
                            using NetworkStream stream = client.GetStream();

                            IPEndPoint remoteEP = client.Client.RemoteEndPoint as IPEndPoint;

                            byte[] buffer = new byte[1024];
                            int read = stream.Read(buffer, 0, buffer.Length);
                            if (read <= 0) continue;

                            string clientVersion = Encoding.ASCII.GetString(buffer, 0, read).Trim('\0', '\n', '\r', ' ');

                            if (!IsValidVersion(clientVersion))
                            {
                                Log($"[DROP] {remoteEP?.Address}:{remoteEP?.Port} invalid version: \"{clientVersion}\"", Color.Red);
                                continue;
                            }

                            Log($"[CLIENT] {remoteEP?.Address}:{remoteEP?.Port} version: {clientVersion}", Color.Teal);

                            int clientVer = int.Parse(clientVersion);
                            int serverVer = int.Parse(latestVersion);
                            int nextPatch = clientVer + 1;

                            if (nextPatch > serverVer)
                            {
                                SendMessage(stream, "READY");
                            }
                            else
                            {
                                string updateHost = (webPort == "80") ? hostname : $"{hostname}:{webPort}";
                                string updateMsg = $"UPDATE {updateHost} {path}/{nextPatch}.exe";
                                SendMessage(stream, updateMsg);
                            }
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // Safe exit
                    }
                }
                catch (Exception ex)
                {
                    Log($"[ERROR] {ex.Message}", Color.Red);
                }
            });

            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        private void StopServer()
        {
            try
            {
                serverRunning = false;
                listener?.Stop();
                cancelSource?.Cancel();
                uptimeTimer.Stop();

                Invoke(() =>
                {
                    startBtn.Text = "Start";
                    statusLabel.Text = "🔴 Server Offline";
                    statusLabel.ForeColor = Color.Red;
                });

                SetControlsEnabled(true);
                Log("[INFO] Server stopped.", Color.Orange);
            }
            catch (Exception ex)
            {
                Log($"[ERROR] {ex.Message}", Color.Red);
            }
        }

        private void SendMessage(NetworkStream stream, string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg + "\0");
            stream.Write(data, 0, data.Length);
        }

        private bool IsValidVersion(string input)
        {
            if (!int.TryParse(input, out int ver)) return false;
            if (ver < 1000 || ver > 99999) return false;

            foreach (char c in input)
            {
                if (c < 32 || c > 126) return false;
            }

            return true;
        }

        private void Log(string message, Color color)
        {
            string timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";

            if (consoleBox.InvokeRequired)
            {
                consoleBox.Invoke(new Action(() => Log(message, color)));
                return;
            }

            consoleBox.SelectionStart = consoleBox.TextLength;
            consoleBox.SelectionLength = 0;
            consoleBox.SelectionColor = color;
            consoleBox.AppendText(timestamped + "\r\n");
            consoleBox.SelectionColor = consoleBox.ForeColor;

            WriteLogFile(timestamped);
        }

        private void WriteLogFile(string line)
        {
            try
            {
                string logFolder = "log";
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                string logFile = Path.Combine(logFolder, $"log-{DateTime.Now:yyyy-MM-dd}.txt");
                File.AppendAllText(logFile, line + Environment.NewLine);
            }
            catch
            {
                // fail silently
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            // Reserved for live server display
        }

        private void SetControlsEnabled(bool enabled)
        {
            verNumeric.Enabled = enabled;
            autoNumeric.Enabled = enabled;
            hostBox.Enabled = enabled;
            webNumeric.Enabled = enabled;
            pathBox.Enabled = enabled;
            autosBox.Enabled = enabled;
        }

        private void SaveConfig()
        {
            try
            {
                using StreamWriter writer = new StreamWriter("ServerConfig.ini");
                writer.WriteLine("[config]");
                writer.WriteLine($"latest_patch={verNumeric.Value}");
                writer.WriteLine($"listen_port={autoNumeric.Value}");
                writer.WriteLine($"web_hostname={hostBox.Text.Trim()}");
                writer.WriteLine($"web_port={webNumeric.Value}");
                writer.WriteLine($"web_path={pathBox.Text.Trim()}");
                writer.WriteLine($"autorun={autosBox.Checked.ToString()}");
                Log("[INFO] Saved ServerConfig.ini", Color.Green);
            }
            catch (Exception ex)
            {
                Log($"[ERROR] Failed to save config: {ex.Message}", Color.Red);
            }
        }


        private void fbLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.facebook.com/profile.php?id=61554036273018",
                UseShellExecute = true
            });
        }

        private string ResolveHostIP(string host)
        {
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(host);
                foreach (var ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        return ip.ToString();
                }
            }
            catch { }

            return host; // fallback
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}