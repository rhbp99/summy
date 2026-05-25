# Summy

[![Made with MonoGame](https://img.shields.io/badge/Made%20with-MonoGame%203.8.1-blue.svg)](https://www.monogame.net/)
[![VB.NET](https://img.shields.io/badge/Language-VB.NET-blue.svg)](https://docs.microsoft.com/en-us/dotnet/visual-basic/)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Status: Complete](https://img.shields.io/badge/Status-Complete-green.svg)]()

## 📖 Overview

Summy is a mathematical board game inspired by Scrabble, developed as a digital adaptation of Corné van Moorsel's original concept. Players create mathematical expressions using numbers and arithmetic operators instead of words, combining strategic thinking with mathematical skills.

The project was originally built with XNA 4.0 and has since been migrated to **MonoGame 3.8.1** targeting **.NET 8.0**, making it buildable with modern tooling without any legacy XNA installation.

### 🎮 Game Features

- **1 vs CPU**: Play against a CPU opponent powered by a greedy AI algorithm
- **Mathematical Gameplay**: Create valid expressions using:
  - Addition (`+`)
  - Subtraction (`-`)
  - Multiplication (`x`)
  - Division (`:`)
- **25×25 Board**: Tile-based board where expressions are placed horizontally or vertically
- **Scoring System**: Points are calculated from the sum of digit values in valid expressions
- **Swap & Skip**: Each player has limited swap and skip chances
- **Pause Menu**: Pause mid-game with options to resume, restart, or return to main menu
- **Help Screen**: Multi-page in-game tutorial with slide navigation
- **About Screen**: Version info and credits
- **FPS Counter**: Built-in debug FPS display

## 📸 Screenshots

![Main Menu](https://bitbucket.org/repo/GjRnop/images/1257416750-MainMenu.png)
*Main Menu*

![Help Screen](https://bitbucket.org/repo/GjRnop/images/3493031165-Bantuan.png)
*Tutorial Screen*

![Gameplay](https://bitbucket.org/repo/GjRnop/images/807773152-Gameplay.png)
*Gameplay*

## 🎮 Game Controls

### Mouse Controls
- **Left Click + Drag**: Pick up and place a tile
- **Left Click on Menu Button**: Confirm action (Swap / Clear / Done)
- **Left Click on Exit Button**: Toggle pause menu

### Keyboard Controls
- **Esc**: Toggle pause menu during gameplay / close overlay in main menu
- **Left / Right Arrow**: Navigate help pages in the tutorial screen

## 🚀 Getting Started

### System Requirements

- Windows 10 or later
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- DirectX 11 compatible GPU

> No XNA Framework or Visual Studio required. The project builds entirely with the standard `dotnet` CLI.

### Quick Start (CLI)

```bash
git clone <repository-url>
cd summy/Summy/Summy

# Restore the local mgcb content tool (only needed once)
dotnet tool restore

# Build and run
dotnet run
```

> **Important:** all `dotnet` commands must be run from `Summy/Summy/` (the directory containing `Summy.vbproj` and `dotnet-tools.json`), not from the repo root or the `Summy/` solution folder.

### What happens during build

1. `dotnet build` restores NuGet packages (`MonoGame.Framework.WindowsDX` and `MonoGame.Content.Builder.Task`)
2. `MonoGame.Content.Builder.Task` invokes the local `mgcb` tool to compile all assets declared in `Content/Content.mgcb`
3. Raw assets are read from `Summy/SummyContent/` (PNG textures + `Debug.spritefont`) and compiled to `bin/Windows/Debug/Content/`
4. The compiled binary is placed in `bin/Debug/net8.0-windows/Summy.exe`

### Build for Release

```bash
dotnet publish -c Release
```

Output will be in `bin/Release/net8.0-windows/publish/`.

## 💻 Development Setup

### Recommended Tools
- Visual Studio 2022 with the **Visual Basic** workload, or VS Code with the **C# Dev Kit** extension
- .NET 8.0 SDK

### Opening the Project in Visual Studio 2022

> ⚠️ Do **not** open `Summy.sln` — it is a legacy VS2015 solution file that still references the old `SummyContent.contentproj` and will not build correctly. Open the project file directly instead.

1. Clone the repository
2. In Visual Studio 2022: **File → Open → Project/Solution** → select `Summy/Summy/Summy.vbproj`
3. Open a terminal in the `Summy/Summy/` directory and run `dotnet tool restore` once to install the local `mgcb` tool
4. Press **F5** to build and run

### Opening the Project in VS Code

1. Install the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension
2. Open the `Summy/Summy/` folder (not the repo root)
3. Run `dotnet tool restore` in the integrated terminal
4. Use `dotnet run` to build and launch

### Project Configuration

| Property | Value |
|---|---|
| Target Framework | `net8.0-windows` |
| Platform | `AnyCPU` |
| Output Type | `WinExe` |
| Resolution | Fixed 1366×768 |
| VSync | Enabled (fixed time step) |
| Content Pipeline | MonoGame MGCB 3.8.1.303 |
| Content Profile | Reach |

### Content Pipeline Notes

Raw assets live in `Summy/SummyContent/` and are referenced by `Content/Content.mgcb` using relative paths (`../../SummyContent/...`). The `mgcb` tool compiles them to `bin/Windows/$(Configuration)/Content/` at build time. You do not need to run `mgcb` manually — it is invoked automatically by `MonoGame.Content.Builder.Task` during `dotnet build`.

If you add new assets:
1. Place the file in `Summy/SummyContent/gfx/` (or the root for fonts)
2. Add a corresponding `/build` entry in `Content/Content.mgcb`
3. Load it in code via `ContentManager.Load(Of Texture2D)("gfx/your-asset")`

### Troubleshooting

| Problem | Solution |
|---|---|
| `mgcb: command not found` | Run `dotnet tool restore` from `Summy/Summy/` |
| Content not found at runtime | Ensure the asset is listed in `Content.mgcb` and the build succeeded without errors |
| Build fails with `net8.0-windows` target | Confirm you are on Windows and have the .NET 8 SDK installed (`dotnet --version`) |
| VS2022 shows errors after opening `.sln` | Open `Summy.vbproj` directly instead of the solution file |

## 🎯 How to Play

1. **Start the Game** — Launch and click **Main** from the main menu
2. **Turn Order** — Turn order between Human and CPU is randomized at the start
3. **Place Tiles** — Drag tiles from your hand onto the board to form a valid mathematical expression (e.g. `3+4=7`)
4. **Valid Expressions** — Expressions must follow the form `A op B = C` and be mathematically correct. Operator precedence (`x`, `:` before `+`, `-`) is respected
5. **Confirm Move** — Click the **Done** button to submit your move and score points
6. **Swap** — Use the **Swap** button to exchange your tiles (limited uses)
7. **Clear** — Use the **Clear** button to take back tiles you placed this turn
8. **Game End** — The game ends when the tile stack is empty and both players have exhausted their skip chances. The player with the highest score wins

### Scoring
Points for a move equal the sum of all digit values in the expression placed (e.g. `3+4=7` scores 3+4+7 = 14).

## 🤖 CPU AI

The CPU uses a **greedy algorithm** (`GreedyProcess.vb`) that runs on a background thread using `Parallel.ForEach`. It:
- Generates permutations of tiles in the CPU's hand
- Tests each permutation against all four operators
- Validates placement against existing tiles on the board
- Places the first valid solution found within a configurable time limit (default: 5 minutes)

## 🛠️ Technical Details

### Project Structure
```
summy/
├── Summy/
│   └── Summy/
│       ├── Base/               # Interfaces and base classes (IBaseScreen, TileBoard, TileMenu, etc.)
│       ├── Content/            # MonoGame content pipeline (Content.mgcb, compiled assets)
│       ├── Main/               # Entry point, GlobalProperty module, MainGame class
│       ├── Screen/             # Game screens (MenuUtamaScreen, GamePlayScreen, BackgroundGamePlayScreen)
│       ├── Script/             # Game logic (Bahasa, GreedyProcess, FPSCounter, CekSemuaKotak)
│       ├── My Project/         # Assembly info
│       └── Summy.vbproj        # Project file
├── SummyContent/               # Raw content assets (textures, fonts)
└── README.md
```

### Key Components

| File | Description |
|---|---|
| `Main/MainGame.vb` | Main `Game` class; manages screen stack and game loop |
| `Main/GlobalProperty.vb` | Shared global state (input, timing, CPU flags) |
| `Base/TileBoard.vb` | Board tile model and rendering |
| `Base/TileMenu.vb` | Action menu tile (Swap, Clear, Done) |
| `Base/Screens.vb` | Abstract base class for all screens |
| `Screen/GamePlayScreen.vb` | Core gameplay logic, player/CPU turn management, board validation |
| `Screen/MenuUtamaScreen.vb` | Main menu, help, and about screens |
| `Script/Bahasa.vb` | Expression parser and validator (syntax + arithmetic correctness) |
| `Script/GreedyProcess.vb` | CPU AI — permutation-based greedy solver |
| `Script/FPSCounter.vb` | Debug FPS overlay |

### Built With
- [MonoGame 3.8.1](https://www.monogame.net/) (WindowsDX backend)
- VB.NET / .NET 8.0
- MonoGame Content Builder (`mgcb`) for asset pipeline

## 👥 Contributing

1. Fork the repository
2. Create a branch: `git checkout -b feature/your-feature`
3. Make your changes and build: `dotnet build`
4. Submit a pull request with a clear description of the change

## 📝 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## 📧 Contact

- **Developer**: [Reza Hidayat Bayu Prabowo](https://www.linkedin.com/in/reza-hidayat-bayu-prabowo/)
- **Email**: rh.bayu.prabowo@outlook.com

## 🙏 Acknowledgments

- Original Game Concept: [Corné van Moorsel](http://www.cwali.nl/summy/summy.htm)
- USU Computer Science Department & Project Supervisors
- Font: **PixelDust** — Copyright © Andreas Nylin
- Icons: **Material Design** — Copyright © Google

## 📌 Project Status

Completed as a final requirement for the Computer Science Bachelor's Degree program at Universitas Sumatera Utara (USU). The codebase has been migrated from XNA 4.0 to MonoGame 3.8.1 / .NET 8.0. Community contributions for improvements and bug fixes are welcome.
