# Wildhaven — Unity 3D Mobile Game

An original third-person island-survival prototype for Android, built with Unity 2022.3 LTS.

## Included
- Third-person movement and follow camera
- Runtime-generated demo island scene
- Resource pickups, inventory counters, and shelter upgrade station
- On-screen inventory and shelter HUD
- Runtime character setup panel with a custom display name
- Explorer and Adventurer style selection
- Forest, Ocean, and Sunset outfit color presets
- PlayerPrefs persistence for the selected name and presets
- Optional island decoration generator
- GitHub Actions Android APK workflow

## Open the project
1. Install Unity Hub and Unity **2022.3.62f1** with Android Build Support.
2. Clone or download this repository and open it through Unity Hub.
3. Let Unity import the project.
4. Use **Wildhaven > Create or Repair Demo Scene** to generate the demo scene.
5. Press Play to test in the Editor.

The scene generator also runs before a build and adds the demo scene to Build Settings.

## Character setup
The generated demo scene opens with a character setup panel. Choose Explorer or Adventurer, select an outfit palette, enter a display name, then press **SAVE & PLAY**. The selections are stored locally with PlayerPrefs.

These are gameplay style presets on the same capsule placeholder. They are not separate male/female 3D models, finished costumes, or a production character creator. Real avatar models, rigging, animation, and model swapping still need to be added.

## Island decorations
Use **Wildhaven > Generate Island Decorations** to add placeholder palm-style trees, rocks, and surrounding ocean to the open demo scene. These are primitive prototype decorations, not final art.

## Controls
- Editor: WASD / arrow keys to move.
- Android: the current demo is not yet equipped with a finished virtual joystick and touch interaction layout.

## Android APK through GitHub Actions
1. Configure a valid Unity license for GameCI and add it to the repository's Actions secrets as `UNITY_LICENSE`. Never commit license files or credentials.
2. Open the **Actions** tab.
3. Select **Build Wildhaven Android APK** and choose **Run workflow**.
4. Download the artifact only after the workflow completes successfully.

The workflow is build automation, not a prebuilt APK. No APK has been compiled or device-tested as part of these repository edits. A successful build still depends on Unity project import, the license secret, and the build environment.

## Current scope
Wildhaven is an early playable foundation, not a finished AAA game. Production-quality 3D art, separate avatar models, animation, mobile touch controls, quests, audio, robust save/load, optimization, QA, and device testing remain future development.

All code and assets are intended to be original or properly licensed. No proprietary game assets are included.
