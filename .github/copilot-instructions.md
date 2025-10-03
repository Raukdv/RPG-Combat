# Copilot Instructions for RPG-Combat

## Project Overview
- This is a Unity-based RPG combat game. The main code is in `Assets/Scripts/` and related subfolders.
- The project uses Unity's Input System (`UnityEngine.InputSystem`) and Animator for player and enemy motion/animation.
- Scene, asset, and prefab files are in `Assets/Scenes/`, `Assets/Enemies/`, etc.

## Key Components
- **PlayerMotion.cs**: Handles player movement, input, and animation. Uses Rigidbody for physics and Animator for state changes.
- **Enemies/**: Contains enemy logic/scripts. Each enemy type may have its own script and prefab.
- **Scenes/**: Game scenes, likely with scene-specific scripts and setup.

## Developer Workflows
- **Build**: Use Unity Editor's build menu. No custom build scripts detected.
- **Run/Debug**: Play mode in Unity Editor. Attach debugger to Unity process for C# debugging.
- **Input**: Uses Unity Input System (`PlayerInput.inputactions`). Update input actions via Unity Editor.
- **Animation**: Animator parameters are set in scripts (e.g., `isMove`, `Moving`, `MoveX`, `MoveY`).

## Coding Patterns & Conventions
- **Movement**: Player movement is calculated using camera-relative vectors:
  ```csharp
  vec3Move = cam.forward * vec2Move.y;
  vec3Move += cam.right * vec2Move.x;
  vec3Move.Normalize();
  ```
- **Input Handling**: Use `OnMove(InputValue value)` for movement input. Set Rigidbody velocity to zero when not moving.
- **Animation Triggers**: Animator parameters are set based on movement vector magnitude and direction.
- **Physics**: Rigidbody is used for movement and collision. Always get Rigidbody via `GetComponent<Rigidbody>()` in `Awake()`.

## External Dependencies
- Unity Input System
- Unity Visual Scripting (some references, but not core to gameplay)

## Integration Points
- Scripts interact with Unity components (Rigidbody, Animator, Transform).
- Input actions are defined in `.inputactions` assets and referenced in scripts.

## Recommendations for AI Agents
- Always update both movement and animation states when handling player input.
- Respect Unity lifecycle methods (`Awake`, `Start`, `FixedUpdate`).
- When adding new gameplay features, place scripts in the appropriate subfolder under `Assets/Scripts/`.
- Use Unity Editor for asset and prefab management; do not attempt to edit `.meta` files directly.
- For new input actions, update the `.inputactions` asset and reference it in scripts.

## Key Files & Directories
- `Assets/Scripts/PlayerMotion.cs`: Player movement and input logic
- `Assets/Enemies/`: Enemy scripts and prefabs
- `Assets/Scenes/`: Game scenes
- `Assets/PlayerInput.inputactions`: Input action definitions

---

If any section is unclear or missing important project-specific details, please provide feedback to improve these instructions.