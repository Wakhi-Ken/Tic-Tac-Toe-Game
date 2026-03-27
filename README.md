# Tic-Tac-Toe Game
![alt text](image.png)
A cross-platform Tic-Tac-Toe game built with Unity. Play against a friend or challenge the AI!

## Game Features
- Player vs Player mode
- Player vs AI mode (Easy & Hard difficulty)
- Sound control with volume slider
- Settings persist between sessions
- Works on PC, Android, and WebGL

## Class Structure
<img width="1451" height="862" alt="image" src="https://github.com/user-attachments/assets/f7509f54-0792-4959-8fe4-25764ecd4f2a" />

GameLogic (Pure C# - No Unity)
Class	Purpose
BoardState	Manages game board array, checks empty cells, resets board
WinChecker	Detects winner (3 in a row), finds winning pattern, checks draw
GameEvents	Observer pattern - notifies UI when game state changes
AI (Strategy Pattern)
Class	Purpose
IAIStrategy	Interface for AI algorithms (allows swapping)
RuleBasedAIStrategy	AI logic: ① Win ② Block ③ Random

 UI (Unity MonoBehaviour)
Class	Purpose
GameManager	Main controller - handles turns, UI updates, mobile orientation
SettingsMenu	Settings panel - game mode, difficulty, volume
MainMenu	Navigation - Play, Settings, Quit buttons
ResetAllSettings	Factory reset - clears all saved settings

 Audio
Class	Purpose
AudioManager	Persistent volume control across scenes

Tests
Class	Purpose
GameLogicTests	NUnit tests for win detection, AI logic, board state

text

## How AI Works

The AI uses a simple 3-step decision process:

1. **WIN** - Check if AI can win in the next move
2. **BLOCK** - Check if player can win and block them
3. **RANDOM** - If no win or block, pick a random empty cell

**Easy Mode**: Only uses step 3 (random moves)
**Hard Mode**: Uses all 3 steps (smart AI)

## How to Run the Game

### Option 1: Play the Builds
- **PC**: Download and run the .exe file [https://www.mediafire.com/file/6rlnwbilchq78il/TicTacToe.zip/file]
- **Android**: Install the APK on your device [https://www.mediafire.com/file/wys9kobxyh5i1sk/Tic_Tac_Toe.apk/file]
- **WebGL**: Play in browser at [https://play.unity.com/en/games/27669612-67e4-4673-8f82-b7dfc05afccb/tic-tac-toe]

### Option 2: Run in Unity Editor

1. Open project in Unity (2020.3 or newer)
2. Open scene: `Assets/Scenes/Start Menu.unity`
3. Click Play button

### Option 3: Build Yourself

1. File → Build Settings
2. Select target platform (PC, Android, iOS, WebGL)
3. Click Build

##  How to Play

1. **Main Menu**: Choose Play to start or Settings to adjust options
2. **Game Modes**:
   - PvP: Two players take turns on same device
   - PvAI: Play against computer
3. **Gameplay**:
   - Tap/Click any empty cell to place X or O
   - First to get 3 in a row wins!
   - Press Reset to start over

##  Platform Notes

| Platform | Orientation | Input |
|----------|------------|-------|
| PC | Any | Mouse Click |
| Android | Landscape | Touch |
| WebGL | Any | Mouse Click |

##  Requirements

- Unity 2020.3 or newer
- TextMeshPro (included with Unity)

---

**Made with Unity** 
