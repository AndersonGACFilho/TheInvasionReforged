# Hangar Hub System - Technical Design

**Purpose:** The hangar serves as the meta-game hub, replacing traditional menus with an immersive 3D environment where the player walks as their alien pilot character.

**Key Concept:** No loading screens between hangar and menu systems. Everything is spatial and interactive.

---

## Overview

### What It Is
The hangar is a 3D space where:
- Player controls the alien pilot (third-person)
- Ship sits on a central repair platform being worked on
- Upgrade terminals are scattered around the perimeter
- Entering the ship's cockpit launches combat missions
- Returning from missions plays a landing sequence

### Design Goals
1. **Immersion** - Feel like a real pilot preparing for battle
2. **Visual Progression** - Ship visually evolves as you upgrade
3. **No Traditional Menus** - All interactions are spatial/diegetic
4. **Atmosphere** - Industrial alien facility that feels alive
5. **Fast Iteration** - Quick in-and-out for experienced players

---

## Scene Layout

### Hangar Dimensions
- **Size:** 40m x 40m x 15m (height)
- **Platform:** Central, circular, 12m diameter
- **Walkable area:** Ring around platform (3m wide walkway)
- **Terminals:** 6 stations at 60° intervals around the ring
- **Camera bounds:** Keep player within visible area

### Spatial Organization

```
                    [Launch Bay Door]
                           ↑
                           |
        [Terminal 3] ← Ship Platform → [Terminal 4]
                   (Repair Drones)
                           |
   [Terminal 2]    [Alien Pilot]     [Terminal 5]
                    (Player Start)
                           |
        [Terminal 1] ← Entrance → [Terminal 6]
                   (Quantum Matrix)
```

### Terminal Placement
| Terminal | Location | Color | Upgrade Type |
|----------|----------|-------|--------------|
| 1 (Front-Left) | Cyan | Xenonic Integrity Core (Hull/Shields) |
| 2 (Mid-Left) | Orange | Primary Combat Protocols (Weapons) |
| 3 (Back-Left) | Purple | Advanced Tactical (Abilities) |
| 4 (Back-Right) | Green | Gravitational Field (Pickup Range) |
| 5 (Mid-Right) | Red | Biomechanical Evolution (Bio-Stats) |
| 6 (Center) | Purple | Quantum Entropy Matrix (Loot Luck) |

---

## Player Character - Alien Pilot

### Model Specifications
- **Poly Count:** 800-1200 triangles
- **Height:** 1.8m (humanoid proportions)
- **Design:** Sleek, alien silhouette with bioluminescent details
- **Color:** Dark gray/blue body with cyan glowing accents
- **Style:** Matches ship aesthetic (organic + tech hybrid)

### Animations
| Animation | Type | Duration | Usage |
|-----------|------|----------|-------|
| **Idle** | Loop | 3-5s | Standing still |
| **Idle_LookShip** | Loop | 4s | Occasionally looks at ship |
| **Walk** | Loop | 1s | WASD movement |
| **Interact** | One-shot | 1.5s | Pressing button/entering ship |
| **CockpitEnter** | One-shot | 2s | Climbing into ship |
| **CockpitExit** | One-shot | 2s | Climbing out of ship |

### Character Controller
**Movement:**
- **Type:** Third-person, ground-based
- **Speed:** 4 m/s walk speed
- **Input:**
  - PC: WASD for movement
  - Mobile: Virtual joystick (left side of screen)
- **Camera:** Fixed-angle third-person (slight overhead)
- **Rotation:** Character faces movement direction
- **Collision:** Capsule collider (prevent walking through objects)

**Interaction System:**
- **Range:** 2.5m from terminal/ship
- **Indicator:** Floating prompt "Press E to Interact" (PC) / Button (Mobile)
- **Raycast:** From character forward to detect interactable objects
- **Interface:** `IInteractable` for terminals and ship cockpit

---

## Ship Repair Platform

### Base Platform
- **Model:** Circular metallic platform, 12m diameter
- **Poly Count:** 500-800 tris
- **Materials:** Brushed alien metal with energy conduit inlays
- **Lighting:** Up-lights from below creating dramatic shadows
- **Particle Effects:** Subtle energy pulses along conduits

### Ship Placement
- **Position:** Center of platform, elevated 0.5m
- **Orientation:** Nose facing launch bay (north)
- **Scale:** Ship model at 1:1 scale (same as combat scenes)
- **Dynamic LOD:** Full detail model (no LOD needed, static scene)

### Ship Visual States
Ship model swaps/updates based on upgrade levels:

**State 0 - Starting Ship (Damaged):**
- Missing hull panels
- Exposed internals
- Dim engine glow
- No weapon hardpoints

**State 1 - Basic Upgrades (25% progression):**
- Some hull panels repaired
- Basic weapon mounts appear
- Engine glow brightens

**State 2 - Mid Upgrades (50% progression):**
- Most hull repaired
- Multiple weapon systems visible
- Shield emitters appear on wings
- Engine color shifts (cyan → magenta gradient)

**State 3 - Advanced Upgrades (75% progression):**
- Full hull integrity
- Heavy weapon systems
- Bio-mechanical armor plates
- Pulsing bioluminescent veins

**State 4 - Fully Upgraded (100%):**
- Pristine condition
- All weapon systems visible
- Full bio-tech integration
- Intense engine glow
- Legendary appearance

**Implementation:**
- Use multiple Static Mesh Components as children of ship actor
- Enable/disable components based on upgrade flags  
- Smooth transitions using material parameters (1-2 second lerp)

---

## Repair Drones (NPCs)

### Model Specifications
- **Poly Count:** 200-400 tris each
- **Design:** Geometric spheres with tool arms
- **Count:** 3-5 drones active at once
- **Behavior:** Simple waypoint movement around ship

### Movement Patterns
- **Waypoints:** Pre-defined points around ship (wings, engines, hull)
- **Speed:** Slow float (1-2 m/s)
- **Animation:** Rotate slowly while moving, tools extend near ship
- **Audio:** Subtle beeping every 3-5 seconds

### Visual Effects
- **Welding Sparks:** When near ship surface (particle burst every 2s)
- **Scanning Beam:** Cyan laser line from drone to ship
- **Eye Light:** Single glowing "eye" that blinks occasionally

**Purpose:** Create sense of ongoing repair work, make hangar feel alive

---

## Upgrade Terminals

### Terminal Models
Each terminal is a holographic pillar with unique color:

**Shared Design:**
- **Base:** 0.5m x 0.5m x 2m tall pillar
- **Poly Count:** 300-500 tris
- **Material:** Translucent holographic shader
- **Particle Effect:** Gentle upward energy stream

**Color Coding:**
| Terminal | Base Color | Particle Color | Glow Intensity |
|----------|-----------|----------------|----------------|
| Integrity Core | Cyan (#00FFCC) | Light blue | Medium |
| Combat Protocols | Orange (#FF6600) | Bright orange | High |
| Tactical Systems | Purple (#AA00FF) | Violet | Medium |
| Gravitational Field | Green (#00FF66) | Emerald | Low |
| Biomechanical | Red (#FF0033) | Crimson | High |
| Quantum Matrix | Purple (#CC00FF) | Multi-color shimmer | Very High |

### Terminal Interaction Flow

1. **Player Approaches Terminal**
   - Prompt appears: "Press E - View Upgrades"
   - Terminal glow intensifies
   - Soft activation hum plays

2. **Player Activates Terminal**
   - Camera zooms to terminal (smooth lerp over 0.5s)
   - Holographic UI panel fades in above terminal
   - Character plays "interact" animation
   - Background blur (depth of field effect)

3. **Upgrade UI Displayed**
   - Show available upgrades for this category
   - Display current level, cost, next level stats
   - Highlight affordable upgrades (green) vs locked (red)
   - "Purchase" and "Cancel" buttons

4. **Player Purchases Upgrade**
   - Scrap counter decreases (animated count-down)
   - Energy surge VFX from terminal to ship
   - Blue liquid flows along ground toward ship
   - Ship model updates (if visual change required)
   - Success sound effect
   - UI updates to show new stats

5. **Player Exits Terminal**
   - Press "Cancel" or "Back"
   - Camera returns to player (smooth lerp over 0.5s)
   - UI fades out
   - Terminal glow returns to idle state

---

## Ship Cockpit Interaction

### Cockpit Entry Point
- **Position:** Front-center of ship, near nose
- **Indicator:** Glowing holographic arrow + "Enter Ship" prompt
- **Interaction Range:** 3m from cockpit

### Launch Sequence

**Phase 1: Enter Cockpit (2 seconds)**
1. Player presses interact
2. Character walks to cockpit (if not already there)
3. "CockpitEnter" animation plays
4. Camera follows character

**Phase 2: Startup Sequence (3 seconds)**
5. Camera transitions into cockpit (first-person view)
6. Holographic HUD flickers on
7. Engine startup sound (rising pitch)
8. Ship begins to hover (platform vibrates)
9. Hangar ambient music fades out
10. Launch music begins

**Phase 3: Launch (2 seconds)**
11. Hangar bay doors open (animation)
12. Ship accelerates forward
13. Camera switches to top-down view
14. Fade to black

**Phase 4: Arena Load (1 second)**
15. Load combat scene in background
16. Fade in to arena
17. Combat music fully active
18. Gameplay begins

**Total Transition Time:** ~8 seconds (feels epic, not tedious)

### Skip Option
- **PC:** Hold Space to skip to Phase 4 (instant)
- **Mobile:** "Skip" button appears after 1 second
- **First-time players:** Force full sequence (tutorial)
- **Returning players:** Skip available immediately

---

## Return to Hangar Sequence

### Landing Sequence

**Phase 1: Combat End**
1. Boss defeated OR player ship destroyed
2. Combat music fades out
3. Victory/defeat sound plays
4. Fade to black (1 second)

**Phase 2: Return Flight (3 seconds)**
5. Fade in to hangar exterior (ship approaching)
6. Ship flies through bay doors
7. Doors close behind ship
8. Ship descends to platform
9. Return music plays (somber or triumphant)

**Phase 3: Landing (2 seconds)**
10. Ship settles on platform
11. Engine powers down (descending pitch)
12. Camera pulls back from ship
13. Cockpit opens

**Phase 4: Exit Cockpit (2 seconds)**
14. Character "CockpitExit" animation plays
15. Character walks onto platform
16. Camera returns to third-person follow
17. Hangar ambient music resumes

**Phase 5: Results Display (3 seconds)**
18. UI panel appears: "Mission Complete" / "Mission Failed"
19. Scrap summary: "Scrap Collected: XXX"
20. "Scrap Retained: YYY (70%)" on death
21. Player regains control

**Total Transition Time:** ~10 seconds (can be skipped after first death)

---

## Lighting & Atmosphere

### Light Sources
| Type | Color | Intensity | Purpose |
|------|-------|-----------|---------|
| **Platform Up-Lights** | Cyan | High | Dramatic ship lighting |
| **Terminal Glows** | Varies | Medium | Terminal identification |
| **Ambient Fill** | Dark blue | Low | Overall scene visibility |
| **Repair Drone Lights** | White | Low | Moving accents |
| **Energy Conduits** | Cyan pulse | Low | Wall detail |
| **Skybox** | Black space | N/A | Background |

### Particle Systems
| Effect | Particle Count | Spawn Rate | Purpose |
|--------|---------------|------------|---------|
| **Platform Energy** | 20-30 | 5/sec | Platform detail |
| **Welding Sparks** | 10-15 | Burst | Repair work |
| **Terminal Streams** | 15-20 | 3/sec | Terminal activity |
| **Ambient Dust** | 50-100 | 2/sec | Atmosphere |
| **Ship Engine Idle** | 30-40 | 10/sec | Ship is alive |

### Post-Processing
- **Bloom:** Moderate (enhances glows)
- **Color Grading:** Slight blue tint
- **Vignette:** Subtle (focus on center)
- **Depth of Field:** Only during terminal interactions
- **VHS Filter:** DISABLED in hangar (clean look contrasts with combat)

---

## Audio Design

### Ambient Sound Loop
- **Base Layer:** Low mechanical hum (looping)
- **Mid Layer:** Repair drone beeping (random intervals 3-8s)
- **Detail Layer:** Energy crackling, distant machinery
- **Volume:** -15 dB (non-intrusive background)

### Music States
| State | Track | Transition |
|-------|-------|------------|
| **Idle** | Ambient pads, slow tempo | Loops seamlessly |
| **Near Ship** | Music swells, adds bass pulse | Crossfade 2s |
| **Launch Sequence** | Dramatic build, rising intensity | Hard cut on launch |

### Sound Effects
| Action | Sound | Volume |
|--------|-------|--------|
| Footsteps | Metallic clank | -18 dB |
| Terminal Activate | Holographic chime | -12 dB |
| Upgrade Purchase | Energy surge + scrap flow | -10 dB |
| Ship Interaction | Cockpit unlock, systems online | -8 dB |
| Drone Movement | Soft hover hum | -20 dB |
| Scrap Counter | Digital counting beeps | -14 dB |

---

## Technical Implementation

### Scene Structure
```
HangarLevel/
├── Environment/
│   ├── BP_Floor (Static Mesh Actor)
│   ├── BP_Walls (Static Mesh Actor)
│   ├── BP_Platform (Static Mesh Actor)
│   ├── BP_ShipBayDoors (Blueprint with animation)
│   └── Props/
│       ├── BP_EnergyCables
│       ├── BP_StorageContainers
│       └── BP_Machinery
├── Ship/
│   ├── BP_PlayerShipDisplay (swappable mesh based on upgrades)
│   ├── UpgradeModules/ (child components)
│   └── EngineParticles (Niagara System)
├── RepairDrones/
│   ├── BP_RepairDrone_01 (waypoint AI)
│   ├── BP_RepairDrone_02
│   └── BP_RepairDrone_03
├── UpgradeTerminals/
│   ├── BP_Terminal_Integrity (cyan)
│   ├── BP_Terminal_Combat (orange)
│   ├── BP_Terminal_Tactical (purple)
│   ├── BP_Terminal_Gravity (green)
│   ├── BP_Terminal_Biomech (red)
│   └── BP_Terminal_QuantumMatrix (purple - center)
├── AlienPilot (Player Character)
│   ├── CharacterMovementComponent
│   ├── InteractionComponent
│   └── SkeletalMeshComponent
├── Camera/
│   ├── CameraActor (follows player)
│   └── CineCamera (for smooth transitions)
├── Lighting/
│   ├── PlatformLights (Point Lights x6)
│   ├── TerminalLights (Spot Lights x6)
│   ├── SkyLight
│   └── DirectionalLight (ambient fill)
├── PostProcessVolume/
│   └── HangarPostProcess (Bloom, Color Grading)
└── Audio/
    ├── AmbientSoundActor (Ambient Attenuation)
    ├── MusicAudioComponent
    └── SFXPool (Audio Components)
```

### Key Systems (Code Architecture)

**AHangarGameMode** (C++ GameMode)
- Manages hangar state (idle, interacting, launching)
- Handles level transitions (to/from combat)
- Controls ship visual state updates
- Manages scrap currency via Game Instance

**AAlienPilotCharacter** (C++ Character)
- Character movement (WASD / virtual joystick via Enhanced Input)
- Interaction system using line traces
- Animation Blueprint integration
- Spring Arm + Camera Component for follow

**AUpgradeTerminal** (C++ Actor, implements IInteractable interface)
- Terminal activation/deactivation
- UMG Widget display (specific upgrade category)
- Purchase logic (check scrap, deduct, apply upgrade)
- Visual feedback (dynamic materials, Niagara particles)

**AShipInteraction** (C++ Actor, implements IInteractable interface)
- Cockpit entry/exit logic
- Launch sequence orchestration using Sequencer
- Return/landing sequence
- Skip functionality via input binding

**ARepairDroneAI** (C++ Pawn with AIController)
- Simple waypoint navigation using AI Perception
- Procedural animations via Animation Blueprint
- Particle trigger at waypoints (Niagara)

**UShipVisualSubsystem** (C++ GameInstanceSubsystem)
- Listens for upgrade events via delegates
- Enables/disables ship mesh components
- Handles smooth material transitions using timelines

### Data Persistence
**UHangarSaveGame** (C++ SaveGame class)
```cpp
UCLASS()
class UHangarSaveGame : public USaveGame {
    GENERATED_BODY()
    
public:
    UPROPERTY(VisibleAnywhere, Category="Progression")
    int32 TotalScrap;
    
    UPROPERTY(VisibleAnywhere, Category="Progression")
    TMap<EUpgradeType, int32> UpgradeLevels;
    
    UPROPERTY(VisibleAnywhere, Category="Visual")
    EShipVisualState CurrentShipState;
    
    UPROPERTY(VisibleAnywhere, Category="Tutorial")
    bool bIsFirstLaunch;
};
```

### Performance Targets
- **FPS:** 60+ on min-spec hardware
- **Load Time:** <3 seconds (hangar → combat)
- **Memory:** <200 MB for hangar scene
- **Draw Calls:** <150 (batched materials)

---

## Mobile Adaptations

### Controls
- **Movement:** Virtual joystick (left side, bottom third)
- **Interaction:** Large button (right side) appears near interactables
- **Camera:** Auto-rotates to face movement direction (no manual camera)
- **UI Scaling:** Larger buttons, touch-optimized terminal panels

### Performance Optimizations
- **Particle Count:** Reduce by 50% (5 particles/sec → 2.5/sec)
- **Drone Count:** 3 drones max (instead of 5)
- **Lighting:** Baked lightmaps for static objects
- **Shadows:** Disabled on mobile (use contact shadows only)
- **Post-Processing:** Bloom only, no DOF

---

## Tutorial Integration

### First Hangar Visit (After First Death)

**Step 1:** Player returns to hangar for first time
- UI Prompt: "Welcome back to your hangar"
- Highlight ship with arrow
- Text: "Your ship is being repaired"

**Step 2:** Walk to nearest terminal
- Arrow points to cyan terminal
- Text: "Approach upgrade terminals to spend scrap"

**Step 3:** Activate terminal
- Forced interaction
- UI tutorial: "This is the Xenonic Integrity Core"
- Explanation: "Upgrades here are permanent - they persist across all runs"

**Step 4:** Purchase first upgrade
- If player has 100+ scrap, prompt to buy first hull upgrade
- If not, text: "Collect more scrap in combat to afford upgrades"

**Step 5:** Return to ship
- Arrow points to ship cockpit
- Text: "Enter your ship when ready to launch again"

**Step 6:** Launch mission
- Full launch sequence (no skip option on first launch)
- Tutorial complete

**Subsequent Visits:**
- No tutorial prompts
- Player knows the loop

---

## Future Enhancements (Post-Launch)

**Content Update Ideas:**
1. **Hangar Customization**
   - Unlockable cosmetic props (trophy displays)
   - Color theme options (change terminal colors)
   - Ship paint jobs (cosmetic)

2. **NPC Crew Members**
   - 2-3 alien NPCs walking around
   - Dialogue snippets on interaction (lore drops)
   - Vendor for rare upgrades?

3. **Mission Briefings**
   - Holographic table with planetary intel
   - Shows next arena before launch
   - Arena-specific modifiers (optional challenges)

4. **Photo Mode**
   - Pause and free-cam around ship
   - Community sharing (Steam Workshop)

---

## Implementation Checklist

### Week 6 (Art Production)
- [ ] Hangar environment models
- [ ] Alien pilot character (rigged, animated)
- [ ] 6 upgrade terminal models
- [ ] 3 repair drone models
- [ ] Ship visual state variants (5 levels)
- [ ] Props (cables, containers, tools)
- [ ] Particle effects (sparks, energy streams)
- [ ] Lighting setup

### Week 10 (Programming)
- [ ] AlienPilotController (movement, interaction)
- [ ] HangarManager (state management)
- [ ] UpgradeTerminal system (UI integration)
- [ ] ShipInteraction (launch/return sequences)
- [ ] RepairDroneAI (waypoint movement)
- [ ] ShipVisualController (dynamic model swapping)
- [ ] Scene transition system (hangar ↔ combat)
- [ ] Save/load integration
- [ ] Tutorial system (first-time flow)

### Week 12 (Polish)
- [ ] Audio implementation (ambient, music, SFX)
- [ ] VFX polish (timing, colors)
- [ ] Camera transitions (smooth lerping)
- [ ] UI/UX refinement (button placement, readability)
- [ ] Performance optimization (batching, LODs)
- [ ] Mobile control testing

---

**Document Status:** Complete  
**Last Updated:** Nov 13, 2025  
**Owner:** Anderson Gonçalves

