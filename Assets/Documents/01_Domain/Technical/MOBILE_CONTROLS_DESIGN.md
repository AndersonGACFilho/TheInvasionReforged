# 📱 Mobile Controls Design

**Project:** The Invasion: Reforged  
**Platform:** iOS + Android  
**Control Scheme:** Touch-based virtual controls  
**Last Updated:** November 3, 2025  
**Status:** DESIGN PHASE - Needs finalization before Week 2 prototype

---

## 🎯 Design Goals

1. **Intuitive:** New players understand controls within 10 seconds
2. **Comfortable:** Playable for 30+ minutes without hand fatigue
3. **Responsive:** <50ms input latency
4. **Accessible:** Support for different hand sizes and grip styles
5. **Flexible:** Works on phones (4-7") and tablets (8-12")

---

## 📐 Button Layout

### Portrait Mode (Phone - Primary)

```
┌────────────────────────────┐
│         Health/XP          │ ← Top HUD
│                            │
│           🎮               │ ← Arena (playable space)
│        [Enemy]             │
│                            │
│     [You]                  │
│                            │
│    🔵         ⚡  🛡️  🚀   │ ← Bottom UI
│  Joystick   Ability Buttons│
│                            │
│   Scrap: 142    Wave: 3    │ ← Bottom stats
└────────────────────────────┘

Left Side:
- Virtual joystick (movement)
- Position: Bottom-left corner
- Size: 120px diameter (dynamic sizing)
- Opacity: 50% when not touched

Right Side:
- 3 ability buttons (active skills)
- Position: Bottom-right corner
- Size: 80px diameter each
- Layout: Horizontal row
- Cooldown overlay when on CD
```

### Landscape Mode (Tablet - Secondary)

```
┌──────────────────────────────────────────────┐
│  HP  ━━━━━━━━━  [Arena]  Wave: 3  Scrap: 142│
│                                               │
│                  [Enemy]                      │
│                                               │
│              [You]                       ⚡   │
│                                          🛡️   │
│  🔵                                      🚀   │
│                                               │
└──────────────────────────────────────────────┘

Left Side: Joystick (same as portrait)
Right Side: Ability buttons (vertical column)
```

---

## 👍 Thumb Zone Analysis

### Safe Zones (Easy to Reach)

**Portrait Phone (Right-handed):**
```
┌────────────────────────────┐
│          ❌ Hard            │ ← Top (unreachable)
│                            │
│       ⚠️ Awkward           │ ← Upper-middle
│                            │
│                            │
│     ✅ Comfortable          │ ← Middle-lower
│                            │
│  ✅ Easy      ✅ Easy       │ ← Bottom corners
└────────────────────────────┘

Left thumb: Bottom-left 25% of screen
Right thumb: Bottom-right 25% of screen
```

**Design Principle:** All interactive elements MUST be in bottom 40% of screen.

---

## 🎮 Control Schemes

### Scheme A: Virtual Joystick (Recommended)

**Movement:**
- Virtual joystick on left side
- Appears on first touch (doesn't need fixed position)
- Drag to move, release to stop
- Visual feedback: Circle expands in drag direction

**Abilities:**
- 3 fixed buttons on right side
- Tap to activate
- Cooldown overlay shows remaining time
- Pulsing glow when ready

**Dash:**
- Integrated into joystick (swipe outward = dash in that direction)
- Alternative: Dedicated 4th button on right side

**Pros:**
- ✅ Familiar to mobile gamers
- ✅ Precise directional control
- ✅ Works well with auto-fire combat

**Cons:**
- ❌ Requires two hands
- ❌ Thumbs cover ~15% of screen

---

### Scheme B: Tap-to-Move (Alternative)

**Movement:**
- Tap anywhere on left 50% of screen
- Ship moves toward tap point
- Hold to continuously move

**Abilities:**
- Same as Scheme A (right side buttons)

**Pros:**
- ✅ Less screen occlusion
- ✅ Easier for casual players

**Cons:**
- ❌ Less precise movement
- ❌ Harder to dodge quickly
- ❌ Not standard for action games

**Decision:** Use Scheme A (Virtual Joystick) unless Week 2 prototype reveals issues.

---

## ♿ Accessibility Features

### Hand Size Adaptation

**Button Scaling:**
- Small hands: Buttons 20% larger, closer to corners
- Large hands: Buttons normal size, slightly inward
- Detection: User setting in Options menu

### One-Handed Mode (Optional - Decide by Week 2)

**Concept:** All controls on one side (left OR right)

```
ONE-HANDED MODE (Left Hand):
┌────────────────────────────┐
│                            │
│                            │
│        [Arena]             │
│                            │
│  🔵                        │
│  ⚡                        │ ← All controls
│  🛡️                        │   on one side
│  🚀                        │
└────────────────────────────┘
```

**Pros:**
- ✅ Play on bus/train (one hand free)
- ✅ Accessibility for players with limited hand mobility

**Cons:**
- ❌ More screen occlusion (one thumb area)
- ❌ Less precise (joystick + buttons close together)
- ❌ Extra UI code complexity

**Decision:** **Defer to post-launch** unless Week 2 playtesting shows strong demand.

---

## 🎯 Auto-Aim Assistance

**Problem:** Touch controls less precise than gamepad/mouse.

### Option 1: No Assist (Pure Player Skill)
- Weapons fire in movement direction
- Player must position correctly
- **Pros:** Skill-based, rewarding
- **Cons:** Frustrating on mobile, may feel unfair

### Option 2: Soft Lock-On (Recommended)
- Weapons aim toward nearest enemy within 30° cone
- Player still needs to be "close enough"
- Visual indicator (subtle arrow) shows lock-on target
- **Pros:** Feels good, not "cheating"
- **Cons:** May prioritize wrong enemy sometimes

### Option 3: Hard Lock-On (Too Easy)
- Weapons auto-target nearest enemy (any direction)
- No player aiming needed
- **Pros:** Super accessible
- **Cons:** Removes skill, boring

**Decision:** **Implement Option 2 (Soft Lock-On)** - Test in Week 2 prototype.

---

## 🧪 Testing Plan

### Week 2: Control Prototype
**Goal:** Validate core movement + ability activation feel good

**Test Scene:**
- Empty arena
- Virtual joystick + 1 test button
- 3 stationary target dummies
- Metrics: Can tester hit all 3 targets within 20 seconds?

**Success Criteria:**
- [ ] Joystick feels responsive (<50ms latency)
- [ ] Button taps register reliably (no missed inputs)
- [ ] No thumb fatigue after 5 minutes of testing
- [ ] Tester says "this feels good"

**If Failed:** Adjust button sizes, positions, or consider Scheme B (tap-to-move).

---

### Week 9: First Mobile Build
**Goal:** Validate controls in real gameplay with enemies

**Test Scenario:**
- Full combat: player vs 5 enemies
- One arena, 3-minute session
- Test on 2 devices: iPhone (small) + Android tablet (large)

**Success Criteria:**
- [ ] Player can dodge enemy attacks reliably
- [ ] Ability buttons accessible without looking down
- [ ] No accidental button presses
- [ ] 60 FPS maintained during combat

---

### Week 13: Full Mobile QA Pass
**Goal:** Test complete game on multiple device sizes

**Test Devices (Minimum 3):**
- Small phone: iPhone SE / Galaxy A32 (4.7-5.5")
- Large phone: iPhone 14 / Galaxy S23 (6-6.7")
- Tablet: iPad / Galaxy Tab (9-11")

**Test Cases:**
- [ ] Complete run start-to-finish (15+ minutes)
- [ ] All 3 abilities used in combat
- [ ] No UI overlap or clipping
- [ ] Buttons accessible in landscape + portrait
- [ ] Works with phone case (adds thickness)

---

### Week 16: Device Compatibility Testing
**Goal:** Catch edge cases on unusual devices

**Test Matrix:**
| Device | Screen Size | Aspect Ratio | Notes |
|--------|-------------|--------------|-------|
| iPhone 8 | 4.7" | 16:9 | Minimum spec |
| iPhone 14 Pro Max | 6.7" | 19.5:9 | Notch handling |
| Galaxy S23 Ultra | 6.8" | 20:9 | Punch-hole camera |
| iPad 9th Gen | 10.2" | 4:3 | Tablet landscape |
| Galaxy Z Fold | 7.6" | 21.6:18 | Foldable (weird ratio) |

---

## 📊 Technical Implementation Notes

### Unity Input System (New Input System)

**Input Action Asset:**
```
Actions/PlayerControls:
- Movement (Vector2) → Touch Position 0
- Ability1 (Button) → Touch Position 1
- Ability2 (Button) → Touch Position 2
- Ability3 (Button) → Touch Position 3
```

**Touch Handling:**
```csharp
// Mobile input implementation
public class MobilePlayerInput : IPlayerInput {
    private VirtualJoystick _joystick;
    private TouchButton[] _abilityButtons;
    
    public Vector2 GetMovementInput() {
        return _joystick.GetDelta(); // Returns -1 to 1 range
    }
    
    public bool GetAbility1Pressed() {
        return _abilityButtons[0].WasPressedThisFrame;
    }
    
    // ... etc
}
```

### UI Canvas Settings

**Canvas Scaler:**
- Mode: Scale With Screen Size
- Reference Resolution: 1920×1080
- Match: 0.5 (balance width/height)
- Reason: Handles wide range of aspect ratios

**Virtual Joystick:**
- Prefab: UI → EventSystem → Virtual Joystick
- Anchors: Bottom-left corner
- Pivot: (0, 0)
- Size Delta: (240, 240) at 1920×1080 reference

---

## 🎨 Visual Design

### Joystick Appearance
- Outer ring: Semi-transparent white circle (30% opacity)
- Inner knob: Solid cyan circle (#00FFCC)
- Drag feedback: Outer ring pulses when touched
- Movement trail: Subtle particle effect in drag direction

### Ability Buttons
- Shape: Circles (not squares—easier to hit on touch)
- Background: Dark gray (#222222, 80% opacity)
- Icon: White symbols (⚡ 🛡️ 🚀)
- Active state: Cyan glow (#00FFCC)
- Cooldown state: Gray overlay with timer text

### Touch Feedback
- Visual: Button scales to 110% on press, back to 100% on release
- Haptic: Light haptic pulse on button press (iOS/Android vibration API)
- Audio: Subtle "click" sound (50% volume)

---

## ⚙️ Settings & Customization

### Control Settings Menu

**Adjustable Parameters:**
- [ ] Joystick size (80% / 100% / 120%)
- [ ] Button size (80% / 100% / 120%)
- [ ] Joystick opacity (30% / 50% / 70%)
- [ ] Haptic feedback (On / Off)
- [ ] Button layout (Default / Swapped for left-handed)

**Presets:**
- Default (100% size, 50% opacity)
- Large (120% size, 70% opacity) — For small phones
- Minimal (80% size, 30% opacity) — For tablets

---

## 🚨 Known Issues & Mitigations

### Issue 1: Thumb Drift
**Problem:** Joystick thumb slips off during long sessions  
**Mitigation:** Joystick "recenters" to current thumb position if dragged >150px from origin

### Issue 2: Accidental Button Presses
**Problem:** Palm/thumb accidentally hits ability buttons  
**Mitigation:** Buttons require 100ms press duration (not instant tap)

### Issue 3: Screen Occlusion
**Problem:** Thumbs cover 10-15% of playable area  
**Mitigation:** Camera slightly zoomed out on mobile (10% more view distance)

### Issue 4: Aspect Ratio Extremes
**Problem:** 21:9 phones (Galaxy S23) have very wide screens  
**Mitigation:** Canvas Scaler Match = 0.5 (scales proportionally)

---

## ✅ Checklist - Mobile Controls Complete

**Week 2 Deliverables:**
- [ ] Virtual joystick prototype functional
- [ ] 1 ability button functional
- [ ] Test scene with 3 target dummies
- [ ] Playtest with 3 people (get feedback)
- [ ] Document feedback in Git commit

**Week 9 Deliverables:**
- [ ] All 3 ability buttons functional
- [ ] Joystick + buttons integrated into main game
- [ ] First mobile build tested on 2 devices
- [ ] No critical control issues found

**Week 13 Deliverables:**
- [ ] Control settings menu implemented
- [ ] Tested on 3+ device sizes
- [ ] Haptic feedback working
- [ ] UI scales correctly on all devices

**Week 16 Deliverables:**
- [ ] Edge case devices tested (foldables, notches)
- [ ] No UI clipping or overlap
- [ ] 60 FPS maintained with touch input

---

## 🎯 Decision Summary

| Decision | Choice | Rationale |
|----------|--------|-----------|
| **Control Scheme** | Virtual Joystick (Scheme A) | Industry standard, precise |
| **Auto-Aim** | Soft Lock-On (Option 2) | Balances skill + accessibility |
| **One-Handed Mode** | Defer to post-launch | Complexity vs demand unknown |
| **Dash Input** | Joystick swipe outward | Reduces button clutter |
| **Haptic Feedback** | Yes (optional in settings) | Improves tactile feel |

---

**Next Steps:**
1. Week 2: Build joystick prototype (Unity test scene)
2. Week 2: Playtest with 3 people, gather feedback
3. Week 9: Full integration into main game
4. Week 13: Device compatibility testing

---

*Last Updated: November 3, 2025*  
*Status: Design phase complete, pending Week 2 prototype*

