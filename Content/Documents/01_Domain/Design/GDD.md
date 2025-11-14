# Game Design Document: The Invasion Reforged

**Genre:** Roguelite Space Shooter  
**Platform:** PC (Steam), iOS, Android  
**Engine:** Unreal Engine 5.7  
**Timeline:** 14 weeks to Early Access  
**Price:** $7.99 (EA) / $9.99 (Full) PC, $4.99 mobile

---

## Core Concept

Wave-based space combat roguelite with dual-loop progression. Player controls alien defender against human invasion forces. Meta loop: walk alien hangar hub, upgrade ship between runs. Combat loop: auto-fire space battles with manual abilities, 15-25 minute sessions.

**Key Differentiator:** Physical hangar hub replaces traditional menus - walk as pilot, interact with terminals, see ship visually evolve with upgrades.

---

## Gameplay Systems

### Combat (Core Loop)

**Movement:**
- WASD/left stick + dash ability
- Physics-based drift, zero-G feel
- Enhanced Input System (PC + mobile)

**Weapons:**
- Auto-targeting primary weapons (Plasma Beam, Orbital Sentinel, Ion Cannon)
- 3 active abilities on cooldown: Singularity Ray, Reflect Field, Void Step
- Weapon evolution through temporary upgrades during runs

**Enemy Types (EA Launch):**
| Enemy | Role | Behavior |
|-------|------|----------|
| Falcon Mk I | Light fighter | Swarm, orbit player |
| Sentry-03 | Orbital drone | Distance, guided missiles |
| Atlas | Heavy bomber | Slow advance, energy mines |
| Aegis Node | Turret | Stationary, linear beams |
| Raven-IX | Kamikaze | Suicide rush |

**Boss:**
- Prometheus Capital Ship
- Phase 1: Drone deployment, spread patterns (EA)
- Phase 2: Shield generators (EA)
- Phase 3: Direct assault (Update 2, Week 17-19)

### Progression

**Temporary (In-Run):**
- XP from kills, level up every 2-3 minutes
- Choose 1 of 3 random upgrades
- Lost on death/completion
- 8-10 upgrade types, stackable synergies

**Permanent (Meta):**
- Alien Scrap currency (70% retained on death)
- 6 upgrade categories via hangar terminals:
  - Xenonic Integrity Core (hull/shields/speed)
  - Primary Combat Protocols (weapons)
  - Advanced Tactical Subsystems (abilities)
  - Quantum Entropy Matrix (loot quality)
  - Adaptive Gravitational Field (pickup range)
  - Biomechanical Evolution (damage/fire rate/healing)
- Visual ship changes reflect upgrades
- 10+ hours to unlock all

### Hangar Hub (Meta Game)

**Function:** Replaces traditional menus with 3D environment.

**Layout:**
- Central repair platform with player ship
- 6 holographic upgrade terminals (color-coded by type)
- Alien pilot character (third-person walk)
- Ambient repair drones, welding effects
- Ship cockpit interaction to launch missions

**Flow:**
1. Walk hangar, spend scrap at terminals
2. Observe ship visual changes
3. Enter cockpit → launch sequence → combat
4. Return to hangar after death/victory
5. Scrap summary displayed

**Ship Evolution:**
- Hull upgrades: repaired panels
- Weapons: visible hardpoints
- Engines: thruster glow changes
- Shields: emitter modules
- Biomech: organic armor, bioluminescence

---

## Arenas & Environments

**Early Access (3 Arenas):**
| Arena | Color | Effect |
|-------|-------|--------|
| Aureon Prime | Orange | Reduced visibility |
| Cryovex | Cyan | Slower projectiles |
| Voltra-9 | Red | Heat damage when static |

**Update 1 (Week 15-16, +2 Arenas):**
- Zerion Nexus (Gray): Stationary turrets, radar interference
- Ethereal Void (Purple): Gravity distortion, curved projectiles

**Visual:** Planetary skyboxes, floating debris, low-poly aesthetic with VHS post-processing.

---

## Art Direction

**Style:**
- Low-poly 3D: 500-3000 triangles per model
- Flat shading (hard normals)
- 80s arcade neon palette: cyan, magenta, orange, purple
- VHS filter: scanlines, chromatic aberration, CRT curvature

**Hangar:**
- Dark alien industrial
- Cyan/blue energy lighting
- Brushed metals, organic curves
- Repair sparks, holographic glows

**Combat:**
- Massive planet backgrounds
- Heavy particle trails (neon)
- Minimal HUD (holographic)
- Alien glyphs in UI

---

## Audio Design

**Music:**
- Hangar: Ambient synth, contemplative
- Combat: Synthwave, intensity scales with waves
- Boss: Cinematic synthwave
- Transitions: Seamless crossfades

**SFX:**
- Hangar: Metallic footsteps, repair drones, terminal chimes
- Combat: Spatial enemy audio, distinct weapon sounds
- Scrap: Metallic ping
- Level up: Digital pulse
- Quantum drops: Temporal distortion

---

## Technical Architecture

**Core Principles:**
- Component-based actors (UE5 best practices)
- Interface-driven systems
- Data Assets for configuration
- Event-based communication
- Automation Framework testing

**Key Systems:**
- Movement: Enhanced Input, physics drift
- Combat: Modular weapon components
- AI: Behavior Trees
- Progression: Component-based XP/upgrades
- Meta: Interactive hangar terminals

**Performance Targets:**
- PC: 60 FPS (GTX 1060), 144 FPS supported
- Mobile: 30-60 FPS adaptive, dynamic resolution
- Load times: <5s PC, <8s mobile

---

## Production Timeline

**Weeks 1-6:** Art production (all assets)  
**Weeks 7-11:** Implementation (feature freeze Week 11)  
**Weeks 12-14:** Polish, marketing, EA launch  
**Week 14:** Early Access launch (Feb 16, 2026)  

**Post-EA:**
- Week 15-16: +2 arenas
- Week 17-19: Boss phase 3, new enemies
- Week 20-24: Leaderboards, endless mode
- Week 25+: Full release v1.0, $9.99 price

---

## Early Access Content

**Included (v0.8):**
- 3 arenas, 5 enemy types, 2-phase boss
- Full progression systems (temporary + permanent)
- Hangar hub with interactive upgrade terminals
- 10+ hours content
- PC, iOS, Android

**Coming (Free Updates):**
- +2 arenas (Update 1)
- +Boss phase 3 (Update 2)
- +Leaderboards, endless mode (Update 3)

---

## Monetization

**Premium Model (No MTX, No Ads):**
- PC: $7.99 EA → $9.99 full release
- Mobile: $4.99 permanent
- Early adopter bonus: Exclusive ship skin
- All updates free

**Optional Future DLC (Post-v1.0):**
- "Human Perspective" campaign: $2.99 PC / $1.99 mobile
- Completely optional, core game remains complete

---

## Success Metrics

**Week 1 PC:**
- 1,000+ sales
- "Very Positive" Steam rating
- 10+ hours average playtime
- 40%+ session return rate

**Month 1 Mobile:**
- 5,000+ downloads combined
- 4.5+ star rating
- 35%+ day-7 retention
- <5% refund rate

---

## Leaderboard System (Post-Launch Update 3)

**Categories:**
- Highest survival time
- Maximum enemies defeated
- Highest scrap collected
- Fastest boss kill
- Daily/weekly challenges

**Features:**
- Global rankings + friends filter
- Platform-specific pools (PC vs mobile)
- Anti-cheat validation
- Cloud-synced scores
- Cosmetic rewards for top performers

**Timeline:** Weeks 20-24 (after core game stable)

---

## Risk Mitigation

**Technical:**
- Feature freeze Week 11 prevents scope creep
- Art-first locks visual scope
- Component architecture enables isolated testing
- Mobile performance: dynamic resolution scaling

**Business:**
- EA launch 5 weeks earlier reduces cash flow risk
- Premium model avoids F2P monetization complexity
- Multiple marketing moments (EA + 3 updates + full release)
- Cross-platform increases addressable market

**Design:**
- Proven roguelite formula reduces risk
- Vampire Survivors inspiration provides reference
- Dual progression ensures variety
- 10+ hour content prevents "too short" criticism

---

**Last Updated:** November 13, 2025  
**Document Version:** 2.0

