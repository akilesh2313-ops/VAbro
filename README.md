# Wildhaven — Unity 3D Mobile Game

An original third-person island-survival prototype for Android, built with Unity 2022.3 LTS.

## Included
- Unity project configuration and package manifest
- Third-person character movement and follow camera
- Runtime-generated demo island scene
- Resource pickups and inventory counters
- Shelter upgrade station
- Basic HUD
- GitHub Actions workflow for Android APK builds

## Open the project
1. Install Unity Hub and Unity **2022.3.62f1** with Android Build Support.
2. Clone or download this repository.
3. In Unity Hub, choose **Add > Add project from disk** and select the repository folder.
4. Open the project and let Unity import the assets.
5. The demo scene is generated automatically when an Android build starts. You can also use **Wildhaven > Create or Repair Demo Scene** in the Unity menu.
6. Press Play to test in the Editor.

## Android APK through GitHub Actions
1. Configure a valid Unity license for GameCI and add it to the repository's Actions secrets as `UNITY_LICENSE`. Do not commit license files or credentials.
2. Open the **Actions** tab in GitHub.
3. Select **Build Wildhaven Android APK** and choose **Run workflow**.
4. When the run succeeds, open the run summary and download the `Wildhaven-Android-APK` artifact.

The workflow is a build configuration only. An APK is produced only after the complete project imports successfully and the Unity license secret is configured.

## Character and island tools
- Select **Wildhaven > Generate Island Decorations** to add placeholder trees, rocks, and surrounding ocean to the open demo scene.
- The demo player has a basic outfit color customizer component. Outfit presets are Forest, Ocean, and Sunset; avatar style and display name are exposed through the component for a future character-selection screen.
- These are functional foundations, not finished character models or high-detail environment art.

## Controls
- Editor: WASD / arrow keys to move.
- Android: connect a `MobileJoystick` component to a Canvas joystick to enable touch movement. The current demo scene is a basic starter scene and does not yet include a finished touch-control layout.

## Current limitations
This is an early playable foundation, not a finished AAA game. The demo uses primitive placeholder geometry. Character selection UI, polished touch controls, animation, quests, audio, save/load, art, optimization, and device testing are still future work.

All code and assets are intended to be original or properly licensed. No proprietary game assets are included.
