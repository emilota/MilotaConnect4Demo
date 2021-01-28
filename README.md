# MilotaConnect4Demo
Custom Connect 4 Unity Game made by me.
You need Unity 2019.4.15f for initial commit in main branch.
Currently needs Unity 2020.2.2f1 for latest in main branch.

# Platforms
* Supported/Verified: Windows 10 PC
* Supported/Verified: Android builds (need to verify which versions)
* Future Maybe: Mac
* Future Maybe: Steam
* Future Maybe: iOS (ipad + iphone)

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

***Tuesday, January 26th, 2021 @ 7:07pm PST***

* Replaces MainScene with SplashScene, GameScene, and ExitScene.  Added a SceneManager to help with transitions between scenes.
* Added code to allow state changes to force scene changes.  Added code in each scene that forces Initial scene if you start the app not in the one you want.
* Converted Controller to App class.  Removed singletons.  Renamed GameUI to GameSceneMB (GameSceneMonoBehavior), and added SplashSceneMB and ExitSceneMB.
* Tweaks to UI for each scene (specially how clicks on buttons or full screens work)
* Removed Wait To Quit state.  QuitProgram now will just go to TITLE_SCREEN state if on Android (as well as minimize the app)

***Thursday, January 28th, 2021 @ 7:40am PST***

* Add ScreenOrientationManager and have it force us into landscape mode (either direction).  I wrote it in such a way that it checks periodically and forces it again as I've seen some devices that don't listen to Unity's value so setting it multiple times seems to fix that.
* Still have OnePlus 6t Quit/Restart button bug though, so this orientation fix didn't fix that, unfortunately

# Known Bugs
* One of my android devices runs games just fine [Huawei MediaPad M5] but my other one [OnePlus 6t] has issues with Quit/Restart button [you can't click it]

# Improvements that can be done
* Need to test for various screen resolutions/aspect ratios
* Fix aspect ratio issues [issues with ultra wide landscape perhaps?]
* Fullscreen/windows toggle (on PCs)
* Sound (there is none)
* Support touch [though touches on my Android device appear to create mouse moves/clicks...need to test on other devices]
* Prettier menus/buttons
* More optimized AI [currently brute force and not smart.  Would like to see places that you can go and determine to block you if that could get you a win]
* Use Checker class for grid entry pieces [rather than managing that seperately]
* Use Checker class for player select [rather than managing that seperately]
* Localization
* More dynamic camera
* Better art/lighting + scene background/world
* Score keeping
* iOS builds?
* Steam builds?
* Mac builds?
* More Game Modes: AI vs AI?
* Better AI/Ability to change AI play modes on the fly
* More Game Modes: Player vs Player via split screen?
* More Game Modes: Player vs Player via network?
* More Game Modes: Multiple player support?
* More Game Modes: Real time play as fast as possible game mode?
