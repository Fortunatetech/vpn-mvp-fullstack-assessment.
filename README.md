# VPN MVP (Windows)

A small native Windows MVP that demonstrates:

* Mocked user sign-in (email/password)
* Fetching VPN nodes from a mock REST API
* Simulated connect / disconnect to a node (UI only — no real VPN)
---

## Prerequisites (install these first)

* Windows 10/11
* Node.js (LTS, e.g. v22.x) — [https://nodejs.org/](https://nodejs.org/)
* .NET SDK 8 (or 7) — [https://dotnet.microsoft.com/](https://dotnet.microsoft.com/)
* VS Code, Git and Github

Verify installs in a fresh terminal:

```powershell
node -v
npm -v
dotnet --info
```

---

## Quick start — run the app (two terminals)

### 1) Start the mock backend

Open Terminal A, then:

```bash
cd Your-Project-Folder-Path\vpn_mvp_windows\backend-mock
npm install
npm start
```

* Server runs at: `http://localhost:4000`
* Check health: `http://localhost:4000/health` → `{"status":"ok"}`

> Note: If you visit `http://localhost:4000/` you may see `Cannot GET /` — this is expected (we only expose API endpoints).

### 2) Start the Windows client (WPF)

Open Terminal B, then:

```powershell
cd Your-Project-Folder-Path\vpn_mvp_windows\client-windows\VPNMvp
dotnet restore
dotnet build
dotnet run
```

A window appears with a centered **Sign in** card.

---

## Test credentials

Use these to sign in:

* **Email:** `test@example.com`
* **Password:** `password123`

After signing in you will see the node list. Click **Connect** to simulate a connection. The button will toggle to **Disconnect** and the UI status will update.

---

## What the project contains (short)

vpn_mvp_windows/
├─ backend-mock/                                # Mock backend (Node.js + Express)
│ ├─ package.json                               # Backend dependencies
│ ├─ server.js                                  # Express server + API routes
│ ├─ data/
│ │ └─ nodes.json                               # Static mock node data
│ └─ README.md                                  # Info about backend
│
├─ client-windows/                              # Windows WPF client
│ ├─ VPNMvp/                                    # Main WPF project
│ │ ├─ VPNMvp.csproj                            # .NET project config
│ │ ├─ App.xaml                                 # App entry (XAML)
│ │ ├─ App.xaml.cs                              # Startup + DI setup
│ │ ├─ MainWindow.xaml                          # Host window for navigation
│ │ ├─ MainWindow.xaml.cs                       # Hosts SignInView initially
│ │ ├─ Views/                                   # UI screens
│ │ │ ├─ SignInView.xaml                        # Sign-in UI
│ │ │ ├─ SignInView.xaml.cs                     # Code-behind for sign-in
│ │ │ ├─ NodeListView.xaml                      # Node list + connect UI
│ │ │ └─ NodeListView.xaml.cs                   # Code-behind for node list
│ │ ├─ ViewModels/                              # MVVM logic
│ │ │ ├─ BaseViewModel.cs                       # Common base class
│ │ │ ├─ SignInViewModel.cs                     # Sign-in logic (auth)
│ │ │ ├─ NodeListViewModel.cs                   # Node list + connect/disconnect
│ │ │ └─ RelayCommand.cs                        # ICommand implementation
│ │ ├─ Models/
│ │ │ └─ Node.cs                                # Node model
│ │ ├─ Services/                                # Business logic + API calls
│ │ │ ├─ IAuthService.cs                        # Auth service interface
│ │ │ ├─ AuthService.cs                         # Mock login implementation
│ │ │ ├─ INodeService.cs                        # Node service interface
│ │ │ ├─ NodeService.cs                         # Fetches nodes from API
│ │ │ ├─ IVpnConnectionService.cs               # VPN connection interface
│ │ │ └─ VpnConnectionSimulator.cs              # Fake connect/disconnect
│ │ ├─ Utils/
│ │ │ └─ NotificationHelper.cs                  # Helper for showing popups
│ └─ README.md                                  # Project instructional Guide 
│
└─ demo_video.mp4                               # Short project functionality demo video 

```

---

## How it works (simple)

* The **backend** provides `POST /api/v1/auth/login` and `GET /api/v1/nodes`. These are mocked and return static JSON.
* The **client** is a WPF app (MVVM):

  * `SignInView` lets user input credentials (mock checked against backend).
  * `NodeListView` shows nodes from `/api/v1/nodes`.
  * `VpnConnectionSimulator` fakes connecting/disconnecting (no real network tunnel).

---

## One AI bug we fixed (example)

AI scaffold produced a snippet that forgot `await`:

**Broken:**

```csharp
var response = httpClient.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
// response is a Task<HttpResponseMessage> — this is wrong
```

**Fixed:**

```csharp
var response = await httpClient.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
```

Keep an eye for missing `await` or wrong types when using AI-generated code.

## Tips & common issues

* If you see `Cannot GET /` on the backend, that’s normal — use `/health` or `/api/v1/nodes`.
* If the client does not show a window, ensure you run `dotnet run` in the WPF project (`VPNMvp.csproj`) and that `<OutputType>WinExe</OutputType>` and `<UseWPF>true</UseWPF>` are in the `.csproj`.
* If build gives warnings about SDK, the app still runs; changing the `.csproj` Sdk to `Microsoft.NET.Sdk` can silence one SDK warning (optional).

---
