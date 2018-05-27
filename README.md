# Skeletons Vs. Zombies (Unity Clone)
A multiplayer online battle arena game (MOBA) where you play as a skeleton mage with basic Unity networking capabilities.

**Tutorial: https://www.udemy.com/devslopes-unity3d/**

# Controls
Button | Description
------ | -----------
RIGHT CLICK/ALT BUTTON | Walk Around/Attack

# What Was Learned From This Game?
- 3D Character Animations (On the Network)
- Adding 3D Model Weapon To Character In Unity's Editor
- Developing a Functional Game
- 3D Collision Detection
- Pathfinding With NavMesh
- OnStartLocalPlayer
- Local Player vs. Server
- NetworkServer.Spawn
- Deactivating Non-Local Player's Cameras
- SyncVars
- Assertions
- Implementing Game Logic
    - Camera Follows Character
    - Move With Right Click (Point and Click)
    - Attack Enemy With Right Click
- UI
    - Player Health Slider (On the Network)
        - Health Slider Always Faces Camera (CameraFacingBillboard)
- Prefabs
    - Player/Enemy
    - Zombie
    - Skill Effects
- Particle System Effects
    - Attack
- Syncing To The Network (NetworkBehaviour)
    - Player Movement (NetworkTransform)
    - Player Health Slider
    - Player Animations (NetworkAnimator)
    - Player Attacks (NetworkAnimator Through Code Because It Is A Trigger)
- Exporting To PC

# Screenshots
Host | Client
:--: | -----:
<img src="/Screenshots/Host.png"> | <img src="/Screenshots/Client.png">
