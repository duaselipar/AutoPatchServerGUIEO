# AutoPatchServerGUI

**AutoPatchServerGUI** is a modern Windows Forms application that acts as a secure version handshake server for Eudemons Online private servers.  
It provides a user-friendly interface to configure patch versions, monitor client connections, and manage patch delivery—all in real time.

---

## ✨ Features

- Graphical UI (WinForms) for managing patch server settings
- TCP-based version handshake on custom port
- Smart patch delivery (client version → next patch → next until up-to-date)
- Input validation to block malformed or suspicious packets
- INI configuration support (`ServerConfig.ini`) with autosave
- Strict version validation (only numbers between 1000–99999 are accepted)
- Optional AutoStart feature on application load
- Live uptime indicator and connection logs
- Colored log console for visual clarity
- Real-time log writing to daily files (`log/log-YYYY-MM-DD.txt`)

---

## ⚙️ Configuration File

The app reads from and saves to a `ServerConfig.ini` file in the following format:

```ini
[config]
latest_patch=1000
listen_port=9528
web_hostname=yourserver.com
web_port=80
web_path=patches
autorun=false
```

✅ The GUI will automatically fill in fields based on this config and save any changes when the server is started.

---

## 📁 Sample Log Output

```text
[20:12:01] [INFO] Autopatch Server started on port 9528
[20:12:01] [INFO] Server IP/Host : yourserver.com | 111.11.111.111
[20:12:01] [INFO] Latest Version : 1000
[20:12:01] [INFO] Patch folder : patches
[20:12:01] [INFO] Client will patch from http://yourserver.com:80/patches/1000.exe
```
---

## 🛡️ License

MIT — free to use and modify for your server projects.
