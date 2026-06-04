# 🧠 NeuroAR

<div align="center">
  <img src="Assets/neuroar_logo.png" alt="NeuroAR Logo" width="200"/>
  <br/>
  <p><i>An immersive Augmented Reality application for anatomical brain education.</i></p>
</div>

---

## 📖 Overview

**NeuroAR** is an interactive Augmented Reality (AR) educational application built with **Unity 3D** and **AR Foundation**. It allows users to project a detailed 3D model of the human brain into their real-world environment. 

By leveraging spatial computing, users can intuitively interact with various anatomical lobes, learning about their functions through a multi-modal experience encompassing spatial visual feedback, detailed text, and narrated audio descriptions.

## ✨ Key Features

- 📍 **Surface Detection & AR Placement**: Seamlessly scan your physical environment to detect flat surfaces (planes) and accurately anchor the 3D brain model.
- 🖐️ **Interactive Anatomy**: Tap on specific parts of the brain (Cerebellum, Brain Stem, Left/Right Hemisphere, Corpus Callosum, Pituitary Gland) to isolate and highlight them.
- 🎓 **Multi-Modal Learning Experience**: 
    - **Visual**: Selected areas illuminate with custom glowing materials and emit spark particle effects.
    - **Auditory**: Dedicated voiceover audio descriptions (`.mp3`) for every selectable brain section.
    - **Textual**: Dynamic 2D UI panels displaying the nomenclature and neurological function of the selected lobe.
- 🔄 **Intuitive Object Manipulation**: Inspect the model closely with natural touch gestures:
    - **1-Finger Swipe**: Rotate the brain on its Y-axis.
    - **2-Finger Pinch**: Scale the brain model up or down.
- 🎭 **Polished UI Flow**: Smooth user interface transitions with custom fade, pop, and pulse animations guiding the user from the Welcome screen, through the AR Scanning phase, to the Interaction dashboard.
- 📊 **Interaction Analytics**: Silent background data logging that records normalized touch coordinates to a local CSV file, acting as a heatmap generator to study user engagement and UX friction.

---

## 🛠️ Prerequisites & Requirements

To open and build this project, ensure you have the following installed:

- **Unity Engine**: `2022.3 LTS` or higher (Recommended).
- **Target Platforms**: 
  - Android (Requires ARCore supported device).
  - iOS (Requires ARKit supported device, A9 chip or newer).
- **Unity Packages** (Install via Package Manager):
  - `AR Foundation`
  - `Apple ARKit XR Plugin` (for iOS builds)
  - `Google ARCore XR Plugin` (for Android builds)
  - `Input System`
  - `TextMeshPro`

---

## 🚀 Installation & Setup

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/YourUsername/NeuroAR.git
   ```
2. **Open in Unity:**
   Open the `NeuroAR` folder via the Unity Hub.
3. **Configure Build Settings:**
   - Go to `File > Build Settings`.
   - Switch your platform to **iOS** or **Android**.
   - Navigate to `Edit > Project Settings > XR Plug-in Management` and ensure the appropriate plugin provider (ARKit or ARCore) is checked for your target platform.
   - For Android, ensure `Minimum API Level` is set to Android 7.0 (API Level 24) or higher.
   - For iOS, ensure `Requires ARKit` is checked and a target minimum iOS version of 11.0 is set.
4. **Build and Run:**
   Connect your mobile device and click `Build and Run`.

---

## 📂 Architecture & Core Scripts

The core logic of the application resides within the `Assets/` directory. 

| Script | Responsibility |
| :--- | :--- |
| **`AppFlowManager.cs`** | The central state machine managing UI transitions (Welcome ➔ Scanning ➔ Interaction). Controls AR plane detection states and mediates UI button triggers. |
| **`ARPlacement.cs`** | Handles AR spawning logic. Uses `ARRaycastManager` to cast rays from screen taps onto AR planes. Instantiates the `Brain_Parent.prefab` upon a valid hit. |
| **`BrainTapManager.cs`** | Manages 3D object interaction post-placement. Casts physics rays to detect touch intersections with `LobeData` components, managing selection/deselection states. |
| **`LobeData.cs`** | Attached to individual parts of the 3D brain. A data container holding names, descriptions, and media (Audio/Particles). Manages local visual state changes (color tinting). |
| **`ObjectManipulator.cs`** | Processes touch gestures for the 3D model. Calculates distance deltas for pinch-to-scale zooming and 1-finger swipe deltas for object rotation. |
| **`UIAnimator.cs`** | A Singleton utility class providing smooth, coroutine-based animations (`FadeIn`, `FadeOut`, `PopIn`, `StartPulse`) without requiring Unity's heavy Animator component. |
| **`HeatmapLogger.cs`** | Analytics tool logging normalized X/Y touch coordinates and timestamps asynchronously to `heatmap_data.csv` in the device's persistent data path. |
| **`GameManager.cs`** | Bootstrapper script initializing `EnhancedTouchSupport` and clamping the target frame rate to 45 FPS to balance smooth rendering and battery life. |

---

## 📱 Application Flow Diagram

1. **Welcome State**: Intro UI and logo. AR placement locked.
2. **Scanning State**: `ARPlaneManager` activates. Pulsing UI prompts user to scan the room. `ARPlacement` listens for taps.
3. **Placement**: User taps a detected plane. Model spawns. Plane detection halts to save battery.
4. **Interaction State**: `BrainTapManager` unlocks. User can rotate/scale the model, and tap specific lobes to trigger educational text, audio voiceovers, and visual feedback.