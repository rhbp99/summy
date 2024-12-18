# Summy

[![Made with XNA](https://img.shields.io/badge/Made%20with-XNA%204.0-blue.svg)](https://en.wikipedia.org/wiki/Microsoft_XNA)
[![VS2015](https://img.shields.io/badge/Visual%20Studio-2015-purple.svg)](https://visualstudio.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Status: Complete](https://img.shields.io/badge/Status-Complete-green.svg)]()

## 📖 Overview

Summy is an innovative mathematical board game inspired by Scrabble, developed as a digital adaptation of Corné van Moorsel's original concept. In this engaging multiplayer game, players create mathematical expressions using numbers and arithmetic operators instead of words, offering a unique blend of strategic thinking and mathematical skills.

### 🎮 Game Features

- **Multiplayer Experience**: Support for 2 or more players
- **Mathematical Gameplay**: Create expressions using:
  - Addition (+)
  - Subtraction (-)
  - Multiplication (×)
  - Division (÷)
- **Scoring System**: Points based on the numbers used in valid expressions
- **Interactive Board**: Classic board game style interface
- **Tutorial System**: Built-in help and game rules
- **Visual Feedback**: Clear indication of valid moves and scores

## 📸 Screenshots

![Main Menu](https://bitbucket.org/repo/GjRnop/images/1257416750-MainMenu.png)
*Main Menu - Start your mathematical adventure*

![Help Screen](https://bitbucket.org/repo/GjRnop/images/3493031165-Bantuan.png)
*Tutorial Screen - Learn game mechanics and rules*

![Gameplay](https://bitbucket.org/repo/GjRnop/images/807773152-Gameplay.png)
*Gameplay - Create mathematical expressions to score points*

## 🚀 Getting Started

### System Requirements

- Windows 7 or later
- .NET Framework 4.0 or higher
- DirectX 9.0c or higher
- 2GB RAM minimum
- Graphics card supporting Shader Model 2.0

### Prerequisites

Before running the game, ensure you have installed:
1. Visual Studio 2015
2. DirectX End-User Runtime
3. XNA Framework 4.0 Redistributable
4. XNA Game Studio 4.0 Platform Tools
5. XNA Game Studio 4.0 Shared
6. XNA Game Studio 4.0.vsix

### Installation Steps

1. Clone the repository:
   ```bash
   git clone [repository-url]
   ```
2. Install XNA Framework for Visual Studio 2015:
   - Download from [XNA For Visual Studio 2015](https://flatredball.com/visual-studio-2019-xna-setup/)
   - Follow the installation wizard
3. Open `Summy.sln` in Visual Studio 2015
4. Build the solution (F5 or Ctrl+F5)

## 🎯 How to Play

1. **Starting the Game**
   - Launch the game
   - Select number of players
   - Each player gets a set of number tiles and operators

2. **Gameplay Rules**
   - Players take turns placing numbers and operators on the board
   - Create valid mathematical expressions
   - Expressions can be formed horizontally or vertically
   - All connected numbers and operators must form valid equations

3. **Scoring**
   - Points are awarded based on the numbers used in expressions
   - Bonus points for complex expressions
   - Invalid expressions are not counted

4. **Winning**
   - Game ends when no more valid moves are possible
   - Player with the highest score wins

## 🛠️ Technical Details

### Project Structure
```
summy/
├── Summy/              # Main game project
├── SummyContent/       # Game assets and content
├── Summy.sln          # Solution file
└── XnaForVS2019/      # XNA Framework support files
```

### Built With
- Visual Studio 2015
- XNA Framework 4.0
- DirectX
- C# Programming Language

## 👥 Contributing

We welcome contributions! Here's how you can help:

1. **Fork the Repository**
2. **Create a Branch**
   ```bash
   git checkout -b feature/YourFeature
   ```
3. **Make Changes**
4. **Test**
   - Ensure your changes don't break existing functionality
   - Add new tests if necessary
5. **Submit Pull Request**
   - Describe your changes in detail
   - Reference any related issues

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact & Support

- **Developer**: [Reza Hidayat Bayu Prabowo](https://www.linkedin.com/in/reza-hidayat-bayu-prabowo/)
- **Issues**: Create an issue in this repository
- **Email**: Contact through LinkedIn

## 🙏 Acknowledgments

- Original Game Concept: [Corné van Moorsel](http://www.cwali.nl/summy/summy.htm)
- USU Computer Science Department
- Project Supervisors
- Microsoft XNA Community
- All contributors and testers

## 📌 Project Status

This project was completed as a final requirement for the Computer Science Bachelor's Degree program at Universitas Sumatera Utara (USU). While the main development is complete, we welcome community contributions for improvements and bug fixes.