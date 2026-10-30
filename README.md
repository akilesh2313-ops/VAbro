# Wildhaven — Unity 3D Mobile Game

An original third-person island-survival game prototype for Android, built with Unity.

## Prototype scope
- Third-person exploration on an original island setting
- Character selection and display-name foundation
- Mobile movement and interaction controls
- Resource pickups and shelter upgrades
- A simple runtime HUD

## Getting started
1. Install Unity 2022.3 LTS or newer with Android Build Support.
2. Create a Unity **3D (Core)** project.
3. Copy the `Assets/Wildhaven` folder from this repository into the Unity project's `Assets` folder.
4. Open the project and press Play.
5. Select Android in Unity Build Profiles to build an APK.

## Status
This is an early source prototype, not a finished AAA game or compiled APK. Placeholder geometry is used for the island and character. Realistic art, animation, sound, save/load, quests, optimization, and device testing remain future work.

## Controls
- Editor: WASD / arrow keys to move; Space to interact.
- Android: on-screen movement pad and Action button.

All game code and assets in this project are intended to be original or properly licensed. No proprietary game assets are included.


## Scene setup checklist
- Add a GameObject named `GameManager` with `WildhavenGameManager`.
- Create a Player object with a `CharacterController`, tag it `Player`, and add `PlayerMotor`.
- Add a camera with `FollowCamera`; assign the Player as its target and assign the camera transform to `PlayerMotor`.
- Create a Canvas with a joystick background and handle; add `MobileJoystick` and assign both RectTransforms.
- Add trigger colliders to resource prefabs and attach `WorldPickup`.
- Add a shelter object with `ShelterStation`; assign its staged upgrade visuals.
- Add HUD Text elements and connect them to `RuntimeHUD`.

The movement script uses Unity's legacy Input axes as its editor fallback. If your project is configured for the new Input System only, enable **Both** under Player Settings > Active Input Handling, or replace that fallback with Input System actions.
