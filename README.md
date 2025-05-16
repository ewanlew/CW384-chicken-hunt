# Coursework for CSC384: Introduction to Video Games Programming
## Overview
**Chicken Hunt** is a Unity game based on Nintendo's [Duck Hunt (1984)](https://en.wikipedia.org/wiki/Duck_Hunt)—with all shown features fully implemented. Its gameplay loop is the same as Duck Hunt, except this time, with chickens!

<p align="center"><img src="https://github.com/user-attachments/assets/15c313ad-ee43-4aad-b728-a7e2ca7bb057" height="500"/></p>

## Gameplay
### Objective
The core objective is, of course, clicking on chickens! It's an arcade-style game, where you aim for constantly topping your own high scores. There's a dedicated (local) leaderboard, as well as a live-view of your leaderboard standings in-game. You start with two lives, and you lose one life for every chicken that falls off the bottom of the screen, and a half-life if you (narrowly) miss clicking on the chicken. There's a slim chance of a chicken spawning as a **golden chicken**—granting the user an opportunity to receive one of three unique items: an extra life, slow-time (10s), and 2x points (10s).

### Options
You can adjust the main volume track, as well as the SFX and music subtracks.
You can adjust the resolution, fullscreen value, and framerate options (lock & view).
You can adjust the particle amount, and screenshake boolean.
All controls are remappable through the options menu, but the deafult controls are as follows:
 - Cycle left in inventory: **A**
 - Cycle right in inventory: **D**
 - Use item: **SPACE**
 - Discard item: **F**

## How to Install
| Note: The game is currently only supported on Windows. There are no plans to support Linux or MacOS.

In the [releases](https://github.com/ewanlew/CW384-chicken-hunt/releases), download & extract "Build.zip". It should work out of the box—if there are any issues, please feel free to email me at 2105531 (at) swansea (dot) ac (dot) uk.
