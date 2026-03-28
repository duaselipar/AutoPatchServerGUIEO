using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoPatchServerGUI
{
    public partial class Form1 : Form
    {
        private const int WmNclButtonDown = 0xA1;
        private const int HtCaption = 0x2;
        private const string ConfigFileName = "autopatchconfig.ini";
        private const string LogFolderName = "log";
        private const string BrandName = "DuaSelipar";
        private const string BrandUrl = "https://www.facebook.com/profile.php?id=61554036273018";

        private readonly Stopwatch _uptimeTimer = new();
        private readonly System.Windows.Forms.Timer _displayTimer = new();
        private TcpListener? _listener;
        private Thread? _listenerThread;
        private bool _serverRunning;
        private CancellationTokenSource? _cancelSource;
        private bool _isDarkMode = true;
        private bool _isClosing;
        private int _brandLogLinkStart = -1;
        private int _brandLogLinkLength;

        private string ConfigFilePath => Path.Combine(AppContext.BaseDirectory, ConfigFileName);

        private string LogDirectoryPath => Path.Combine(AppContext.BaseDirectory, LogFolderName);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern nint SendMessage(nint hWnd, int msg, nint wParam, nint lParam);

        public Form1()
        {
            InitializeComponent();
            ConfigureUi();
            FormClosing += Form1_FormClosing;
        }

        private void ConfigureUi()
        {
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox1.Text = "Settings";
            groupBox2.Text = "Logs";
            consoleBox.Font = new Font("Cascadia Code", 9F, FontStyle.Regular, GraphicsUnit.Point);
            titleLabel.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);

            groupBox2.Padding = new Padding(8);
            consoleBox.Dock = DockStyle.None;
            consoleBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            consoleBox.BorderStyle = BorderStyle.None;
            consoleBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            consoleBox.DetectUrls = false;
            consoleBox.MouseMove += ConsoleBox_MouseMove;
            consoleBox.MouseUp += ConsoleBox_MouseUp;

            StyleButton(startBtn, Color.FromArgb(46, 125, 50), Color.White);
            StyleButton(button1, Color.FromArgb(96, 125, 139), Color.White);
            StyleTitleBarButton(themeBtn, "☀", new Font("Segoe UI Symbol", 11F, FontStyle.Bold, GraphicsUnit.Point));
            StyleTitleBarButton(minimizeBtn, "—", new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point));
            StyleTitleBarButton(closeBtn, "✕", new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point));

            InitDisplayTimer();
            UpdateServerStateUi();
            ApplyTheme();
        }

        private void InitDisplayTimer()
        {
            _displayTimer.Interval = 1000;
            _displayTimer.Tick += DisplayTimer_Tick;
            _displayTimer.Start();
            UpdateTimeDisplays();
        }

        private void DisplayTimer_Tick(object? sender, EventArgs e)
        {
            UpdateTimeDisplays();
        }

        private void UpdateTimeDisplays()
        {
            UpdateServerTime();
            UpdateUptimeDisplay();
        }

        private void UpdateServerTime()
        {
            serverTimeValueLabel.Text = $"Server Time (24-H): {DateTime.Now:HH:mm:ss}";
        }

        private void UpdateUptimeDisplay()
        {
            if (_uptimeTimer.IsRunning)
            {
                var uptime = _uptimeTimer.Elapsed;
                label6.Text = uptime.Days > 0
                    ? $"Online Server: {uptime.Days}d {uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}"
                    : $"Online Server: {uptime.Hours:D2}:{uptime.Minutes:D2}:{uptime.Seconds:D2}";
                return;
            }

            label6.Text = "Online Server: -";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LogCreatedBy();
                EnsureConfigExists();
                var config = LoadConfig();
                ApplyConfig(config);

                Log($"[INFO] Loaded {ConfigFileName}", Color.MediumSeaGreen);

                if (autosBox.Checked)
                {
                    Log("[INFO] AutoStart enabled. Starting server...", Color.MediumSeaGreen);
                    startBtn.PerformClick();
                }
            }
            catch (Exception ex)
            {
                Log($"[ERROR] Failed to load config: {ex.Message}", Color.IndianRed);
            }
        }

        private void EnsureConfigExists()
        {
            if (File.Exists(ConfigFilePath))
            {
                return;
            }

            WriteDefaultConfig();
            Log($"[INFO] Config file not found. Created default {ConfigFileName}", Color.Goldenrod);
        }

        private void WriteDefaultConfig()
        {
            var lines = new[]
            {
                "[config]",
                "latest_patch=1000",
                "listen_port=9528",
                "web_hostname=127.0.0.1",
                "web_port=80",
                "web_path=patches",
                "autorun=false",
                "dark_mode=true"
            };

            File.WriteAllLines(ConfigFilePath, lines, new UTF8Encoding(false));
        }

        private Dictionary<string, string> LoadConfig()
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var rawLine in File.ReadAllLines(ConfigFilePath))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith('[') || line.StartsWith(';') || line.StartsWith("//"))
                {
                    continue;
                }

                var parts = line.Split('=', 2);
                if (parts.Length != 2)
                {
                    continue;
                }

                var key = parts[0].Trim();
                var value = parts[1].Split(new[] { "//" }, StringSplitOptions.None)[0].Trim();
                values[key] = value;
            }

            return values;
        }

        private void ApplyConfig(IReadOnlyDictionary<string, string> config)
        {
            if (config.TryGetValue("latest_patch", out var latestPatch) && int.TryParse(latestPatch, out var latestVersion))
            {
                verNumeric.Value = Math.Clamp(latestVersion, (int)verNumeric.Minimum, (int)verNumeric.Maximum);
            }

            if (config.TryGetValue("listen_port", out var listenPort) && int.TryParse(listenPort, out var patchPort))
            {
                autoNumeric.Value = Math.Clamp(patchPort, (int)autoNumeric.Minimum, (int)autoNumeric.Maximum);
            }

            if (config.TryGetValue("web_port", out var webPortText) && int.TryParse(webPortText, out var webPort))
            {
                webNumeric.Value = Math.Clamp(webPort, (int)webNumeric.Minimum, (int)webNumeric.Maximum);
            }

            if (config.TryGetValue("web_hostname", out var hostName))
            {
                hostBox.Text = hostName;
            }

            if (config.TryGetValue("web_path", out var webPath))
            {
                pathBox.Text = webPath;
            }

            if (config.TryGetValue("autorun", out var autorun))
            {
                autosBox.Checked = string.Equals(autorun, bool.TrueString, StringComparison.OrdinalIgnoreCase);
            }

            if (config.TryGetValue("dark_mode", out var darkMode))
            {
                _isDarkMode = string.Equals(darkMode, bool.TrueString, StringComparison.OrdinalIgnoreCase);
            }

            ApplyTheme();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (_serverRunning)
            {
                StopServer();
                return;
            }

            SaveConfig();
            SetInputControlsEnabled(false);

            var latestVersion = verNumeric.Value.ToString();
            var port = (int)autoNumeric.Value;
            var hostname = hostBox.Text.Trim();
            var webPort = webNumeric.Value.ToString();
            var path = pathBox.Text.Trim();

            _cancelSource = new CancellationTokenSource();
            var token = _cancelSource.Token;

            _listenerThread = new Thread(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        _listener = new TcpListener(IPAddress.Any, port);
                        _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        _listener.Start();

                        _serverRunning = true;
                        _uptimeTimer.Restart();
                        RunOnUiThread(UpdateServerStateUi);

                        Log($"[INFO] Autopatch Server started on port {port}", Color.MediumSeaGreen);
                        var resolvedIp = ResolveHostIp(hostname);
                        var hostDisplay = hostname == resolvedIp ? resolvedIp : $"{hostname} | {resolvedIp}";
                        Log($"[INFO] Server IP/Host : {hostDisplay}", Color.MediumSeaGreen);
                        Log($"[INFO] Latest Version : {latestVersion}", Color.MediumSeaGreen);
                        Log($"[INFO] Patch folder : {path}", Color.MediumSeaGreen);
                        Log($"[INFO] Client will patch from http://{hostname}:{webPort}/{path}/{latestVersion}.exe", Color.MediumSeaGreen);

                        while (!token.IsCancellationRequested)
                        {
                            if (!_listener.Pending())
                            {
                                Thread.Sleep(100);
                                continue;
                            }

                            TcpClient? client = null;

                            try
                            {
                                client = _listener.AcceptTcpClient();
                                client.ReceiveTimeout = 5000;
                                client.SendTimeout = 5000;

                                using var stream = client.GetStream();
                                var remote = client.Client.RemoteEndPoint as IPEndPoint;
                                var buffer = new byte[1024];
                                var read = stream.Read(buffer, 0, buffer.Length);
                                if (read <= 0)
                                {
                                    continue;
                                }

                                var raw = Encoding.ASCII.GetString(buffer, 0, read);
                                var clientVersion = raw.Replace("\r", " ").Replace("\n", " ").Trim('\0', ' ');

                                if (!IsValidVersion(clientVersion))
                                {
                                    Log($"[DROP] {remote?.Address}:{remote?.Port} invalid version: \"{clientVersion}\"", Color.IndianRed);
                                    continue;
                                }

                                Log($"[CLIENT] {remote?.Address}:{remote?.Port} version: {clientVersion}", Color.DeepSkyBlue);

                                var clientVersionNumber = int.Parse(clientVersion);
                                var serverVersionNumber = int.Parse(latestVersion);
                                var nextPatch = clientVersionNumber + 1;

                                if (nextPatch > serverVersionNumber)
                                {
                                    SafeSend(stream, "READY");
                                }
                                else
                                {
                                    var updateHost = webPort == "80" ? hostname : $"{hostname}:{webPort}";
                                    var updateMessage = $"UPDATE {updateHost} {path}/{nextPatch}.exe";
                                    SafeSend(stream, updateMessage);
                                }
                            }
                            catch (IOException ex)
                            {
                                Log($"[WARN] Client IO error: {ex.Message}", Color.Goldenrod);
                            }
                            catch (SocketException ex)
                            {
                                Log($"[WARN] Client socket error: {ex.Message}", Color.Goldenrod);
                            }
                            catch (Exception ex)
                            {
                                Log($"[WARN] Client handler error: {ex.Message}", Color.Goldenrod);
                            }
                            finally
                            {
                                try
                                {
                                    client?.Close();
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log($"[ERROR] Listener error: {ex.Message}", Color.IndianRed);
                    }
                    finally
                    {
                        try
                        {
                            _listener?.Stop();
                        }
                        catch
                        {
                        }

                        _serverRunning = false;
                        _uptimeTimer.Stop();
                        RunOnUiThread(UpdateServerStateUi);
                    }

                    if (!token.IsCancellationRequested)
                    {
                        Log("[INFO] Restarting server in 3s ...", Color.Goldenrod);
                        for (var index = 0; index < 30 && !token.IsCancellationRequested; index++)
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            })
            {
                IsBackground = true
            };

            _listenerThread.Start();
        }

        private void StopServer()
        {
            try
            {
                _cancelSource?.Cancel();
                _serverRunning = false;

                try
                {
                    _listener?.Stop();
                }
                catch
                {
                }

                if (_listenerThread is not null && _listenerThread.IsAlive)
                {
                    _listenerThread.Join(1000);
                }

                _uptimeTimer.Stop();

                if (!_isClosing)
                {
                    UpdateServerStateUi();
                    Log("[INFO] Server stopped.", Color.Goldenrod);
                }
            }
            catch (Exception ex)
            {
                if (!_isClosing)
                {
                    Log($"[ERROR] {ex.Message}", Color.IndianRed);
                }
            }
        }

        private void UpdateServerStateUi()
        {
            SetInputControlsEnabled(!_serverRunning);
            UpdateStartButtonAppearance();
            UpdateStatusLabel();
            UpdateTimeDisplays();
        }

        private void SetInputControlsEnabled(bool enabled)
        {
            verNumeric.Enabled = enabled;
            autoNumeric.Enabled = enabled;
            hostBox.Enabled = enabled;
            webNumeric.Enabled = enabled;
            pathBox.Enabled = enabled;
            autosBox.Enabled = enabled;
        }

        private void UpdateStatusLabel()
        {
            statusValueLabel.Text = _serverRunning ? "Status : ONLINE" : "Status : OFFLINE";
            statusValueLabel.ForeColor = _serverRunning
                ? Color.FromArgb(46, 125, 50)
                : Color.FromArgb(211, 47, 47);
            statusValueLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        }

        private void UpdateStartButtonAppearance()
        {
            startBtn.Text = _serverRunning ? "Stop" : "Start";
            startBtn.BackColor = _serverRunning
                ? Color.FromArgb(198, 40, 40)
                : Color.FromArgb(46, 125, 50);
            startBtn.ForeColor = Color.White;
            startBtn.FlatAppearance.MouseOverBackColor = _serverRunning
                ? Color.FromArgb(183, 28, 28)
                : Color.FromArgb(27, 94, 32);
        }

        private void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
            SaveConfig();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var formBackColor = _isDarkMode ? Color.FromArgb(18, 18, 18) : Color.FromArgb(245, 247, 250);
            var panelBackColor = _isDarkMode ? Color.FromArgb(24, 24, 24) : Color.FromArgb(245, 247, 250);
            var inputBackColor = _isDarkMode ? Color.FromArgb(45, 45, 48) : Color.FromArgb(250, 250, 250);
            var foreColor = _isDarkMode ? Color.FromArgb(232, 234, 237) : Color.FromArgb(38, 50, 56);
            var mutedColor = _isDarkMode ? Color.FromArgb(156, 163, 175) : Color.FromArgb(96, 125, 139);
            var logGroupBackColor = formBackColor;
            var logBackColor = _isDarkMode ? Color.FromArgb(32, 33, 36) : Color.FromArgb(248, 249, 250);
            var logForeColor = foreColor;
            var titleBarBackColor = _isDarkMode ? Color.FromArgb(24, 24, 27) : Color.FromArgb(230, 236, 243);

            BackColor = formBackColor;
            ForeColor = foreColor;
            titleBarPanel.BackColor = titleBarBackColor;
            titleLabel.ForeColor = foreColor;
            themeBtn.BackColor = titleBarBackColor;
            minimizeBtn.BackColor = titleBarBackColor;
            closeBtn.BackColor = titleBarBackColor;
            themeBtn.ForeColor = foreColor;
            minimizeBtn.ForeColor = foreColor;
            closeBtn.ForeColor = foreColor;
            themeBtn.Text = _isDarkMode ? "☀" : "☾";

            StyleGroupBox(groupBox1, panelBackColor, foreColor);
            StyleGroupBox(groupBox2, logGroupBackColor, foreColor);
            StyleTextBox(hostBox, inputBackColor, foreColor);
            StyleTextBox(pathBox, inputBackColor, foreColor);
            StyleNumericUpDown(verNumeric, inputBackColor, foreColor);
            StyleNumericUpDown(autoNumeric, inputBackColor, foreColor);
            StyleNumericUpDown(webNumeric, inputBackColor, foreColor);
            ApplyControlTheme(this, foreColor);

            autosBox.BackColor = Color.Transparent;
            consoleBox.BackColor = logBackColor;
            consoleBox.BorderStyle = BorderStyle.None;
            consoleBox.ForeColor = logForeColor;
            consoleBox.Margin = new Padding(0);
            serverTimeValueLabel.ForeColor = mutedColor;
            label6.ForeColor = mutedColor;
            ApplyBrandLogLinkStyle();

            themeBtn.FlatAppearance.MouseOverBackColor = _isDarkMode ? Color.FromArgb(52, 52, 56) : Color.FromArgb(214, 222, 230);
            minimizeBtn.FlatAppearance.MouseOverBackColor = _isDarkMode ? Color.FromArgb(52, 52, 56) : Color.FromArgb(214, 222, 230);
            closeBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(232, 17, 35);
            closeBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 0, 20);

            UpdateStartButtonAppearance();
            UpdateStatusLabel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearLogs();
        }

        private void ClearLogs()
        {
            _brandLogLinkStart = -1;
            _brandLogLinkLength = 0;
            consoleBox.Clear();
        }

        private void LogCreatedBy()
        {
            var message = $"[INFO] Created by {BrandName}";
            var timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";

            if (_isClosing || consoleBox.IsDisposed || consoleBox.Disposing)
            {
                WriteLogFile(timestamped);
                return;
            }

            if (consoleBox.InvokeRequired)
            {
                try
                {
                    consoleBox.Invoke(new Action(LogCreatedBy));
                }
                catch (ObjectDisposedException)
                {
                    WriteLogFile(timestamped);
                }
                catch (InvalidOperationException)
                {
                    WriteLogFile(timestamped);
                }

                return;
            }

            var prefix = $"[{DateTime.Now:HH:mm:ss}] [INFO] Created by ";
            consoleBox.SelectionStart = consoleBox.TextLength;
            consoleBox.SelectionLength = 0;
            consoleBox.AppendText(prefix);

            _brandLogLinkStart = consoleBox.TextLength;
            _brandLogLinkLength = BrandName.Length;
            consoleBox.AppendText(BrandName);
            consoleBox.AppendText(Environment.NewLine);
            ApplyBrandLogLinkStyle();
            consoleBox.ScrollToCaret();

            WriteLogFile(timestamped);
        }

        private void ConsoleBox_MouseMove(object? sender, MouseEventArgs e)
        {
            consoleBox.Cursor = IsOverBrandLogLink(e.Location) ? Cursors.Hand : Cursors.IBeam;
        }

        private void ConsoleBox_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && IsOverBrandLogLink(e.Location))
            {
                OpenBrandPage();
            }
        }

        private bool IsOverBrandLogLink(Point location)
        {
            if (_brandLogLinkStart < 0 || _brandLogLinkLength <= 0)
            {
                return false;
            }

            var charIndex = consoleBox.GetCharIndexFromPosition(location);
            return charIndex >= _brandLogLinkStart && charIndex < _brandLogLinkStart + _brandLogLinkLength;
        }

        private void ApplyBrandLogLinkStyle()
        {
            if (_brandLogLinkStart < 0 || _brandLogLinkLength <= 0 || consoleBox.TextLength < _brandLogLinkStart + _brandLogLinkLength)
            {
                return;
            }

            var originalStart = consoleBox.SelectionStart;
            var originalLength = consoleBox.SelectionLength;

            consoleBox.Select(_brandLogLinkStart, _brandLogLinkLength);
            consoleBox.SelectionColor = consoleBox.ForeColor;
            consoleBox.SelectionFont = consoleBox.Font;
            consoleBox.Select(originalStart, originalLength);
            consoleBox.SelectionColor = consoleBox.ForeColor;
        }

        private void OpenBrandPage()
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = BrandUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Log($"[WARN] Unable to open author page: {ex.Message}", Color.Goldenrod);
            }
        }

        private static void StyleButton(Button button, Color backColor, Color foreColor)
        {
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
        }

        private static void StyleGroupBox(GroupBox groupBox, Color backColor, Color foreColor)
        {
            groupBox.BackColor = backColor;
            groupBox.ForeColor = foreColor;
            groupBox.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
        }

        private static void StyleTextBox(TextBox textBox, Color backColor, Color foreColor)
        {
            textBox.BackColor = backColor;
            textBox.ForeColor = foreColor;
            textBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleNumericUpDown(NumericUpDown numericUpDown, Color backColor, Color foreColor)
        {
            numericUpDown.BackColor = backColor;
            numericUpDown.ForeColor = foreColor;
            numericUpDown.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleTitleBarButton(Button button, string text, Font font)
        {
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Cursor = Cursors.Hand;
            button.Font = font;
            button.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static void ApplyControlTheme(Control parent, Color foreColor)
        {
            foreach (Control control in parent.Controls)
            {
                switch (control)
                {
                    case Label:
                    case CheckBox:
                        control.ForeColor = foreColor;
                        control.BackColor = Color.Transparent;
                        break;
                    case GroupBox:
                    case Button:
                    case RichTextBox:
                    case TextBox:
                    case NumericUpDown:
                        break;
                }

                if (control.HasChildren)
                {
                    ApplyControlTheme(control, foreColor);
                }
            }
        }

        private void themeBtn_Click(object sender, EventArgs e)
        {
            ToggleTheme();
        }

        private void minimizeBtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WmNclButtonDown, HtCaption, 0);
        }

        private void SaveConfig()
        {
            try
            {
                var lines = new[]
                {
                    "[config]",
                    $"latest_patch={verNumeric.Value}",
                    $"listen_port={autoNumeric.Value}",
                    $"web_hostname={hostBox.Text.Trim()}",
                    $"web_port={webNumeric.Value}",
                    $"web_path={pathBox.Text.Trim()}",
                    $"autorun={autosBox.Checked.ToString().ToLowerInvariant()}",
                    $"dark_mode={_isDarkMode.ToString().ToLowerInvariant()}"
                };

                File.WriteAllLines(ConfigFilePath, lines, new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                Log($"[ERROR] Failed to save config: {ex.Message}", Color.IndianRed);
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            _isClosing = true;
            SaveConfig();

            if (_serverRunning)
            {
                StopServer();
            }
        }

        private static bool IsValidVersion(string input)
        {
            if (!int.TryParse(input, out var version))
            {
                return false;
            }

            if (version is < 1000 or > 99999)
            {
                return false;
            }

            foreach (var character in input)
            {
                if (character < 32 || character > 126)
                {
                    return false;
                }
            }

            return true;
        }

        private void Log(string message, Color color)
        {
            var cleanMessage = SanitizeLogMessage(message);
            var timestamped = $"[{DateTime.Now:HH:mm:ss}] {cleanMessage}";

            if (_isClosing || consoleBox.IsDisposed || consoleBox.Disposing)
            {
                WriteLogFile(timestamped);
                return;
            }

            if (consoleBox.InvokeRequired)
            {
                try
                {
                    consoleBox.Invoke(new Action(() => Log(cleanMessage, color)));
                }
                catch (ObjectDisposedException)
                {
                    WriteLogFile(timestamped);
                }
                catch (InvalidOperationException)
                {
                    WriteLogFile(timestamped);
                }

                return;
            }

            consoleBox.SelectionStart = consoleBox.TextLength;
            consoleBox.SelectionLength = 0;
            consoleBox.AppendText(timestamped + Environment.NewLine);
            consoleBox.SelectionColor = consoleBox.ForeColor;
            consoleBox.ScrollToCaret();

            WriteLogFile(timestamped);
        }

        private static string SanitizeLogMessage(string message)
        {
            var builder = new StringBuilder(message.Length);

            foreach (var character in message)
            {
                if (character is '\r' or '\n' or '\t')
                {
                    builder.Append(' ');
                    continue;
                }

                if (char.IsControl(character))
                {
                    builder.Append($"\\x{(int)character:X2}");
                    continue;
                }

                builder.Append(character);
            }

            return builder.ToString();
        }

        private void WriteLogFile(string line)
        {
            try
            {
                Directory.CreateDirectory(LogDirectoryPath);
                var logFilePath = Path.Combine(LogDirectoryPath, $"log-{DateTime.Now:yyyy-MM-dd}.txt");
                File.AppendAllText(logFilePath, line + Environment.NewLine);
            }
            catch
            {
            }
        }

        private string ResolveHostIp(string host)
        {
            try
            {
                var addresses = Dns.GetHostAddresses(host);
                foreach (var ipAddress in addresses)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ipAddress.ToString();
                    }
                }
            }
            catch
            {
            }

            return host;
        }

        private void SafeSend(NetworkStream stream, string message)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(message + "\0");
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Log($"[WARN] Send failed: {ex.Message}", Color.Goldenrod);
            }
        }

        private void RunOnUiThread(Action action)
        {
            if (_isClosing || IsDisposed || Disposing || !IsHandleCreated)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    Invoke(action);
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }

                return;
            }

            action();
        }
    }
}