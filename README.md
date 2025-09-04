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

## AI Prompts

#### Prompt 1 — Scaffold WPF MVVM skeleton (frontend)
Generate a minimal WPF MVVM project skeleton in C# for .NET 8 that includes:
- App.xaml/App.xaml.cs wiring dependency injection
- MainWindow that hosts views via ContentControl
- SignInView + NodeListView XAML & code-behind
- ViewModels: SignInViewModel, NodeListViewModel, BaseViewModel, RelayCommand
- Services: IAuthService, INodeService, IVpnConnectionService and a VpnConnectionSimulator
Keep code short, runnable, and show exact file names and folder layout.

#### Prompt 2 — Create mock backend (Express)
Create a small Node.js Express mock backend for local development:
- Expose POST /api/v1/auth/login (mocked single user)
- Expose GET /api/v1/nodes (return JSON from data/nodes.json)
- Provide a simple health check GET /health
- Provide package.json and a start script
Keep implementation minimal and easy to run with `npm install and npm start`.

#### Prompt 3 — Help debug XAML/UX issue 
I'm getting 'Set property UIElement.Effect threw an exception' in SignInView.xaml.
Suggest a safe, portable replacement for using DynamicResource SystemParameters.DropShadowKey 
so the SignIn card uses a drop shadow without runtime exceptions.
Also produce the fully updated SignInView.xaml using that fix.

## AI bugs + fixes
Broken AI-generated snippet (given earlier) — and debug explanation
Broken snippet (AI forgot to await the HTTP call)

This is the exact broken code the AI gave earlier (it looks plausible but is incorrect):

```csharp
public async Task<bool> LoginAsync(string email, string password)
{
    // AI forgot to await the HTTP call and returns true always when status code is 200
    var response = httpClient.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
    if (response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        return true; // but response is Task<HttpResponseMessage> -> accessing StatusCode is invalid
    }
    return false;
}
```
Why it’s broken (explanation)

PostAsJsonAsync(...) returns a Task<HttpResponseMessage>. The code assigns that task to response but never awaits it.

Accessing response.StatusCode is attempting to access a property on the Task object, not on the HttpResponseMessage result — this is a type error and causes runtime/compile issues.

Also, the method returns true or false without reading or validating the response body (token parsing missing).

Fixed version (corrected code):
```csharp
public async Task<bool> LoginAsync(string email, string password)
{
    var response = await httpClient.PostAsJsonAsync("/api/v1/auth/login", new { email, password });
    if (!response.IsSuccessStatusCode) return false;

    var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
    // store token temporarily (demo only)
    return body != null && !string.IsNullOrEmpty(body.Token);
}

```
Notes about the fix

Always await asynchronous HTTP calls before inspecting the result.

Prefer response.IsSuccessStatusCode (covers 200–299).

Parse the JSON body to get the token and persist it if needed.


## Extra broken item you encountered & short fix

While not a pure **AI-code bug**, one runtime error we hit was:

```
Set property 'UIElement.Effect' threw an exception
```

This came from using:

```xml
Effect="{DynamicResource {x:Static SystemParameters.DropShadowKey}}"
```

On some machines that dynamic resource is not available or invalid. The safe fix is to replace that with an explicit `DropShadowEffect` (see the `SignInView.xaml` above) or remove the `Effect` entirely.


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
