# MilotaConnect4Demo
Custom Connect 4 Unity Game made by me
Currently targeted to PC (Windows) and Android builds.  Will support iOS at some point [hopefully once I get an iOS device to test on...]

# Release notes

***Sunday, January 24th, 2021 @ 2:21pm PST***

* Uploaded MilotaConnect4Demo build under Unity 2019.4.15f
* Make sure you select Connect4Scene when starting for the first time.
* Game requires a mouse to play.  You can't use touch [unless your machine simulates a mouse click] 

***Sunday, January 24th, 2021 @ 3:07pm PST***

* Upgrade from Unity 2019.4.15f1 to Unity 2020.2.2f1

***Sunday, January 24th, 2021 @ 6:40pm PST***

* Built and run on Android.  Must be run in landscape mode.  Game play works [touch turns into mouse clicks].  However there's a bug from Title screen and Quit button.  It goes to Thanks for Playing screen but then returns to Title screen for some reason.  Need to investigate.

***Monday, January 25th, 2021 @ 9:47am PST***

* Fixed Android quit/restart issue.  
* Removed all Input.GetMouseButtonDown(0) checks and instead replaced by full screen button click callbacks.
* Updated state machine to support RestartOrQuit button and Fullscreen button clicks, so you can do different stuff based on context.
* Few UI tweaks.  Made footer blink on Title and Game Over screens for instance.

# Improvements that can be done
* Need to test for various screen resolutions/aspect ratios
* Fix aspect ratio issues [aka it looks bad in portrait mode...need to force landscape mode or come up with some strategy for that]
* Fullscreen/windows toggle
* Sound (there is none)
* Support touch [though touches on my Android device appear to create mouse moves/clicks...need to test on other devices]
* Prettier menus/buttons
* More optimized AI [currently brute force and not smart.  Would like to see places that you can go and determine to block you if that could get you a win]
* Use Checker class for grid entry pieces [rather than managing that seperately]
* Use Checker class for player select [rather than managing that seperately]
* Localization
* More dynamic camera
* Better art
* Score keeping
* iOS builds?
* Steam builds?
* More Game Modes: AI vs AI?
* Better AI/Ability to change AI play modes on the fly
* More Game Modes: Player vs Player via split screen?
* More Game Modes: Player vs Player via network?
* More Game Modes: Multiple player support?
* More Game Modes: Real time play as fast as possible game mode?
