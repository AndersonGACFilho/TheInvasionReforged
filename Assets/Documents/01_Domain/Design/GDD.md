# The Invasion: Reforged - Game Design Document

**Genre:** Roguelite space shooter (Vampire Survivors-like)  
**Platform:** PC, iOS, Android  
**Developer:** Anderson Gonçalves (solo)  
**Target:** Players who like arcade action with progression

---

## Game Concept

### The Pitch
You're an alien pilot defending your species from invading humans. Each run is a fight to survive, evolve, and get stronger for the next attempt.

### How It Works
- Start with a basic ship
- Kill enemies and collect scrap
- Level up during runs and pick temporary upgrades
- Survive waves and boss fights
- Die and return to your Hangar
- Spend scrap on permanent upgrades
- Go again, but stronger

### Main Features
- Auto-fire combat with active abilities
- Roguelite progression (temporary + permanent)
- 5 planetary arenas with environmental effects
- Entropy Matrix system for better loot
- Clean architecture for easy updates

---

## Setting & Atmosphere

### The World
Space battles around different planets. Each arena has a planet in the background to make it feel big and lonely.

### Planetary Arenas

#### Early Access Launch (v0.8 - 3 Arenas)
| Planet | Visual Identity | Gameplay Effect |
|--------|----------------|-----------------|
| **Aureon Prime** | Orange cosmic dust | Reduced visibility |
| **Cryovex** | Blue fog with floating ice | Human projectiles move slower |
| **Voltra-9** | Intense red sun | Gradual damage if player remains stationary |

#### Content Update 1 (Weeks 15-16 - +2 Arenas)
| Planet | Visual Identity | Gameplay Effect |
|--------|----------------|-----------------|
| **Zerion Nexus** | Human metallic structures | Stationary turrets and radar interference |
| **Ethereal Void** | Distorted space | Unstable gravity and curved projectile paths |

### The Feel
You're alone. Last ship of your species fighting off humans who think you're the invaders. It's survival, not heroics.

---

## Core Gameplay

### Movement
- **PC:** WASD to move, Space to dash
- **Mobile:** Left joystick to move, right button to dash
- Physics feel like zero-gravity - momentum and drift
- Ship auto-rotates on mobile

### Combat

**Auto Weapons**
Your ship fires automatically. No aiming needed - focus on positioning and staying alive.

**Active Abilities**
Special powers on cooldown that you trigger manually:
- **Singularity Ray:** Laser beam that pierces through enemies
- **Reflect Field:** Shield that bounces projectiles back
- **Gravitational Burst:** Area damage pulse

### Enemy Waves
Human fleets spawn from the edges and come after you. Waves get denser over time.

---

## Weapons & Abilities

### Automatic Weapons
| Type | Name | Description |
|------|------|-------------|
| **Plasma Beam** | Pulse Beam | Auto-targeting shots that seek nearby enemies |
| **Defensive Drone** | Orbital Sentinel | Continuously fires while orbiting your ship |
| **Ion Cannon** | Ion Burst | Circular explosion of pure energy |

### Active Abilities (Cooldown-based)
| Type | Name | Description | Cooldown | Mobile Control |
|------|------|-------------|----------|----------------|
| **Focused Laser** | Singularity Ray | Continuous piercing beam | 8 seconds | Tap icon (right side) |
| **Reactive Shield** | Reflect Field | Reflects projectiles for 3 seconds | 12 seconds | Tap icon (right side) |
| **Gravity Dash** | Void Step | Teleport short distance, leaving gravity well | 6 seconds | Tap icon (right side) |

### Upgrade Synergies
Weapons can evolve and combine during runs. Example: Pulse Beam + Ion Burst = Plasma Nova (periodic automatic explosions around the ship).

---

## Enemy Types

### Basic Enemies
| Type | Name | Behavior | Health | Threat Level |
|------|------|----------|--------|--------------|
| **Light Fighter** | Falcon Mk I | Attacks in groups, orbiting the player. Fragile but numerous. | Low | ⭐ |
| **Orbital Drone** | Sentry-03 | Maintains distance, fires guided missiles | Medium | ⭐⭐ |
| **Heavy Bomber** | Atlas | Advances slowly, drops energy mines. High resistance. | High | ⭐⭐⭐ |
| **Defense Turret** | Aegis Node | Stationary, fires linear beams | Medium | ⭐⭐ |
| **Kamikaze Interceptor** | Raven-IX | Suicide ships that accelerate toward player | Low | ⭐⭐⭐ |

### Raven-IX - Detailed Mechanics
**Visual Design:**
- Glowing engines
- Compact body with vibrating blades
- Red flashing light before detonation

**Audio:**
- Short siren warning
- Growing energy charge sound
- Heavy metallic explosion with spatial distortion effect on impact

**Gameplay Function:**
- Forces constant movement and tactical prioritization
- Creates dangerous "denial zones" in the arena
- Punishes static positioning

### Boss Unit
**Prometheus - Capital Ship**

#### Early Access Launch (v0.8 - 2 Phases)
- **Phase 1:** Deploys auxiliary drones while firing spread patterns
- **Phase 2:** Activates shield generators that must be destroyed
- **Reward:** Massive scrap payout and guaranteed epic upgrade

#### Content Update 2 (Weeks 17-19 - +3rd Phase)
- **Phase 3:** Direct assault with devastating beam weapons
- **Additional Mechanics:** Enraged state with faster attacks and new patterns

---

## Progression System

### Alien Scrap (Main Resource)
**How It Works:**
- Metal fragments drop from destroyed enemies
- Blue light pulse when collected
- Metallic echo sound
- Counter shows on HUD
- Keep 70% when you die

### Upgrade Menu - The Hangar
**What It Looks Like:**
- Dark industrial space
- Repair drones floating around
- Energy in the pipes
- Holographic upgrade panels

**Animation:** Scrap turns to blue liquid and gets absorbed into your ship.

---

## Permanent Upgrades

### Xenonic Integrity Core
**What It Does:** Stronger hull, more shields, faster base speed

**Cost:** 100-500 scrap per level  
**Max:** 10 levels

### Primary Combat Protocols
**What It Does:** Unlocks new auto-weapons and makes them stronger

**Cost:** 250-1000 scrap  
**Effect:** New weapon types and better damage

### Advanced Tactical Subsystems
**What It Does:** Makes your active abilities better

**Cost:** 150-800 scrap per level  
**Effect:** Faster cooldowns, longer duration, more power

### Quantum Entropy Matrix
**What It Does:** Better loot drops during runs

**Cost:** 300-1000 scrap per level  
**See details below**

### Adaptive Gravitational Field
**What It Does:** Collect scrap from further away

**Cost:** 100-400 scrap per level  
**Max:** 5 levels

### Biomechanical Evolution Synthesis
**What It Does:** Changes your ship's biology for better stats

**Cost:** 500-1500 scrap per level  
**Max:** 8 levels  
**Effect:** More damage, faster healing, better fire rate

---

## Quantum Entropy Matrix

### The Idea
*"Humanity believes in chance. We program it."*

Ancient alien tech that bends probability. Makes rare upgrades show up more often during runs.

### How It Looks
- Purple glowing core
- Floats in distorted gravity
- Quantum particle effects

### How It Sounds
- Temporal fold sound on rare drops
- Deep harmonic hum when active

### Upgrade Tiers
| Level | Effect | Cost |
|-------|--------|------|
| I | +5% Rare | 300 scrap |
| II | +10% Rare / +3% Epic | 500 scrap |
| III | +15% Rare / +7% Epic | 700 scrap |
| IV | +20% Rare / +10% Epic | 1000 scrap |

**Strategy:** Upgrading this early makes later runs way more powerful.

---

## Visual & Audio Design

### Graphics
**Art Style:**
- 3D with neon and particles
- Blues, purples, greens on black space
- Huge planets in background, floating debris

**UI:**
- Holographic menus
- Alien symbols
- Glowing indicators
- Everything has subtle glow

**Ships:**
- Organic curves + tech
- Glowing energy in hull
- Visual changes as you upgrade

### Sound
**Music:**
- Spatial synthwave
- Gets intense with combat
- Ambient space drones when calm

**Effects:**
| Event | Sound |
|-------|-------|
| Scrap pickup | Metallic ping |
| Level up | Rising pulse |
| Explosions | Heavy impact |
| Rare upgrade | Temporal distortion |
| Ship damage | Biomech strain |
| Active ability | Energy release |

---

## Architecture (SOLID Principles)

### Single Responsibility
Each system does one thing:
- `MovementSystem`: Ship movement
- `WeaponSystem`: Firing and projectiles
- `EnemyAISystem`: Enemy behavior
- `CollectionSystem`: Resource gathering
- `UpgradeSystem`: Stat changes
- `LeaderboardSystem`: Scores (post-launch)

### Open/Closed
Add new stuff without changing old code:
- Abstract base classes for weapons and enemies
- Data-driven configurations in JSON/ScriptableObjects
- Plugin architecture for new upgrade types

### Liskov Substitution Principle (L)
All game objects follow common contracts:
- All enemies implement `IEnemy` interface
- All weapons implement `IWeapon` interface
- All collectibles implement `ICollectable` interface

### Interface Segregation Principle (I)
Small, specific interfaces prevent unnecessary dependencies:
- `IDamageable`: Health and damage handling
- `ICollectable`: Pickup and collection logic
- `IPoolable`: Object pooling for performance
- `ITargetable`: Enemy targeting system
- `ILeaderboardEntry`: Score submission and validation (post-launch)
- `ILeaderboardProvider`: Platform-specific leaderboard integration (post-launch)

### Dependency Inversion Principle (D)
Core systems depend on abstractions, not concrete implementations:
- Systems reference interfaces, not specific classes
- Dependency injection for service location
- Event-driven architecture for loose coupling

### Benefits for Solo Development
Even as a one-person project, SOLID principles ensure:
- **Maintainability:** Easy to find and fix bugs
- **Scalability:** Simple to add new content
- **Testability:** Individual systems can be tested in isolation
- **Clarity:** Code remains understandable months later

---

## 11. DEVELOPMENT TIMELINE (Early Access Strategy)

### Phase 1: Art Production (6 Weeks)
**Weeks 1-6 (Nov 4 - Dec 15, 2025)**

**Deliverables:**
- All visual assets (ships, enemies, boss, UI, VFX)
- 3 planetary arena skyboxes (Aureon, Cryovex, Voltra-9)
- VHS filter and post-processing
- Low-poly models with retro aesthetic
- Complete art asset library

### Phase 2: Core Gameplay Implementation (5 Weeks)
**Weeks 7-11 (Dec 16 - Jan 20, 2026)**

**Week 7: Core Systems**
- Functional movement with inertia physics
- Basic automatic firing system
- Scrap collection with visual/audio feedback
- 3 enemy types: Falcon, Sentry, Atlas

**Week 8: Enemy AI & Combat**
- Enemy AI state machines
- Wave spawning system
- Combat systems and damage calculation
- Particle effects for weapons and explosions

**Week 9: Progression Systems**
- Level-up system with XP
- Temporary upgrade system (8-10 upgrades)
- Upgrade selection UI during runs
- Visual/audio feedback for progression

**Week 10: Meta-Progression & Boss**
- Hangar scene with upgrade menu
- 6 permanent upgrade categories
- Quantum Entropy Matrix implementation
- Prometheus boss (2 phases)
- Save/load system

**Week 11: Feature Freeze & Polish**
- 3 planetary arenas fully functional
- Balance pass on all systems
- Bug fixing and optimization
- **FEATURE FREEZE** - No new content after this point

### Phase 3: Early Access Launch (3 Weeks)
**Weeks 12-14 (Jan 27 - Feb 16, 2026)**

**Week 12: Polish & Testing**
- Sound design polish
- Final VFX pass
- Performance optimization
- Playtesting and bug fixes

**Week 13: Marketing Preparation**
- Steam Early Access page setup
- Gameplay trailer (60-90 seconds)
- Demo build creation
- DevLog 1: "Why Early Access?"

**Week 14: Early Access Launch**
- Final build configuration (PC, iOS, Android)
- Platform submission (Steam, App Store, Google Play)
- 🚀 **EARLY ACCESS LAUNCH** (Feb 16, 2026)
- Community monitoring and hotfixes

### Phase 4: Content Update 1 (2 Weeks)
**Weeks 15-16 (Feb 17 - Mar 2, 2026)**

**Deliverables:**
- 2 new planetary arenas (Zerion Nexus, Ethereal Void)
- Arena-specific environmental effects
- Balance adjustments based on player feedback
- DevLog 2: "Arena Expansion Update"

### Phase 5: Content Update 2 (3 Weeks)
**Weeks 17-19 (Mar 3 - Mar 23, 2026)**

**Deliverables:**
- Prometheus boss 3rd phase
- Raven-IX kamikaze enemy (if not in EA launch)
- 2-3 additional enemy variants
- Additional permanent upgrades
- DevLog 3: "Boss Fight 2.0"

### Phase 6: Content Update 3 (5 Weeks)
**Weeks 20-24 (Mar 24 - Apr 27, 2026)**

**Deliverables:**
- Global leaderboards (all categories)
- Daily and weekly challenge systems
- Endless survival mode
- Friends leaderboard integration
- DevLog 4: "Competitive Update"

### Phase 7: Full Release v1.0 (Week 25+)
**Week 25+ (Apr 28, 2026 onward)**

**Deliverables:**
- Exit Early Access on all platforms
- "Full Release" marketing campaign
- Price increase ($7.99 → $9.99 on PC)
- Post-launch support and community engagement

---

## 12. TECHNICAL SPECIFICATIONS

### PC Minimum Requirements
- **OS:** Windows 10 64-bit
- **Processor:** Intel Core i5-6600K / AMD Ryzen 3 1300X
- **Memory:** 8 GB RAM
- **Graphics:** NVIDIA GTX 960 / AMD Radeon R9 380
- **Storage:** 2 GB available space

### PC Recommended Requirements
- **OS:** Windows 10/11 64-bit
- **Processor:** Intel Core i7-8700K / AMD Ryzen 5 3600
- **Memory:** 16 GB RAM
- **Graphics:** NVIDIA GTX 1070 / AMD Radeon RX 5700
- **Storage:** 2 GB available space (SSD recommended)

### Mobile Requirements (iOS)
- **Minimum:** iPhone 8 / iPad (6th gen) or newer
- **OS:** iOS 14.0 or later
- **Recommended:** iPhone 12 or newer for 60 FPS
- **Storage:** 500 MB available space

### Mobile Requirements (Android)
- **Minimum:** Android 8.0 (Oreo) or higher
- **Processor:** Snapdragon 660 / Exynos 7884 or equivalent
- **Memory:** 3 GB RAM minimum, 4 GB recommended
- **Graphics:** Adreno 512 / Mali-G71 or better
- **Storage:** 500 MB available space

### Performance Targets
- **PC Frame Rate:** 60 FPS minimum, 144 FPS supported
- **Mobile Frame Rate:** 30 FPS minimum, 60 FPS on high-end devices
- **Resolution:**
  - PC: 1080p native, 1440p and 4K upscaling
  - Mobile: Adaptive resolution scaling (720p-1080p)
- **Load Times:** Under 5 seconds between runs (PC), under 8 seconds (mobile)
- **Battery Optimization:** Maximum 2 hours continuous play on mobile

### Mobile-Specific Optimizations
- **Dynamic Resolution Scaling:** Automatically adjusts quality for stable framerate
- **Reduced Particle Density:** 50% fewer particles on mobile without sacrificing visual appeal
- **Texture Compression:** Platform-specific formats (ASTC for mobile)
- **Simplified Shaders:** Mobile-friendly shader variants
- **Cloud Saves:** Cross-platform progression via cloud sync

---

## 13. MONETIZATION & POST-LAUNCH

### Early Access Launch - Premium Model
**PC (Steam):**
- **Early Access Price:** $7.99 USD
- **Full Release Price:** $9.99 USD (after exiting EA)
- **Early Adopter Benefit:** Exclusive "Vanguard" ship skin for EA buyers
- **No ads, no microtransactions**

**Mobile (iOS/Android):**
- **Early Access Price:** $4.99 USD
- **Full Release Price:** $4.99 USD (no price increase on mobile)
- **No ads, no microtransactions, no IAP**
- Complete premium experience - One-time purchase, lifetime access

**Early Access Duration:** 3-4 months (February → May/June 2026)

**What's Included in Early Access (v0.8):**
- ✅ Complete core gameplay loop (movement, combat, progression)
- ✅ 3 planetary arenas with unique effects
- ✅ 2-phase boss fight (Prometheus)
- ✅ Full meta-progression system (Hangar upgrades)
- ✅ 8-10 temporary upgrades
- ✅ 6 permanent upgrade categories
- ✅ Save/load with cloud sync
- ✅ 10+ hours of content

**Coming During Early Access (Free Updates):**
- 🔄 2 additional arenas (Update 1 - Weeks 15-16)
- 🔄 Boss 3rd phase (Update 2 - Weeks 17-19)
- 🔄 Leaderboards & Endless mode (Update 3 - Weeks 20-24)

### Post-Launch Content (All Free Updates)
- **Content Update 1 (Weeks 15-16):** Arena Expansion
  - +2 planetary arenas (Zerion Nexus, Ethereal Void)
  - Balance adjustments based on player feedback

- **Content Update 2 (Weeks 17-19):** Boss Enhancement
  - Prometheus 3rd phase
  - 2+ new enemy variants
  - Additional permanent upgrades

- **Content Update 3 (Weeks 20-24):** Competitive Features
  - Global leaderboards (survival time, kills, scrap, etc.)
  - Daily/weekly challenge runs
  - Endless survival mode
  - Friends leaderboard integration
  - Achievement system expansion

- **Full Release v1.0 (Week 25+):** Exit Early Access
  - All content complete
  - PC price increase to $9.99 (mobile stays $4.99)
  - "Full Release" marketing push
  - Potential console ports exploration

**Cross-Platform Strategy:**
- Unified progression via cloud saves
- No feature differences between platforms
- Early Access status applies to all platforms simultaneously

### Optional Future DLC (Post-v1.0)
- **"Human Perspective"** - Play as human forces defending Earth
  - Price: $2.99 PC / $1.99 mobile
  - New campaign, ships, abilities
  - Optional expansion, not required for core experience

**All core updates remain free - DLC is strictly optional bonus content**

### Community Features
- Steam Workshop support for custom arena modifications (v1.1)
- Global leaderboards with multiple categories (Update 3)
- Achievement system (25+ achievements, expandable)

---

## 13.5. LEADERBOARD SYSTEM

> **⚠️ POST-LAUNCH FEATURE (Content Update 3)**  
> **Implementation Timeline:** Weeks 17-19 (after initial release)  
> **Rationale:** Due to system complexity (backend infrastructure, anti-cheat, cross-platform sync), leaderboards will be developed and deployed as a free content update after the core game is stable and launched. This allows focus on core gameplay polish for initial release.

### Overview
The leaderboard system provides competitive metrics and social engagement across all platforms, encouraging replayability through multiple scoring categories and time-limited challenges.

### Leaderboard Categories

#### 1. Global Rankings (Persistent)
**Highest Survival Time**
- **Metric:** Total time survived in a single run
- **Display:** Minutes:Seconds format
- **Tiebreaker:** Enemies defeated

**Maximum Enemies Defeated**
- **Metric:** Total enemy kills in a single run
- **Display:** Kill count
- **Tiebreaker:** Survival time

**Highest Scrap Collected**
- **Metric:** Total scrap gathered in a single run (before death penalty)
- **Display:** Scrap amount with alien fragment icon
- **Tiebreaker:** Survival time

**Fastest Boss Kill**
- **Metric:** Time to defeat Prometheus Capital Ship
- **Display:** Minutes:Seconds format
- **Unlocked After:** First boss encounter completion

**Longest Streak**
- **Metric:** Consecutive runs completed without dying before wave 10
- **Display:** Number of successful runs
- **Reset Condition:** Death before wave 10

#### 2. Challenge Leaderboards (Time-Limited)
**Daily Challenge**
- **Duration:** 24 hours, resets at midnight UTC
- **Format:** Fixed seed run with predetermined arena and starting upgrades
- **Scoring:** Composite score based on survival time (40%), enemies defeated (40%), scrap collected (20%)
- **Rewards:** Top 100 players receive unique cosmetic ship skin
- **Display:** Player rank, score, and percentage ranking

**Weekly Challenge**
- **Duration:** 7 days, resets Monday 00:00 UTC
- **Format:** Specific gameplay modifier (e.g., "Double Enemy Speed", "No Active Abilities", "Boss Rush")
- **Scoring:** Challenge-specific metric
- **Rewards:** Top 10 players featured in in-game "Hall of Champions" terminal
- **Display:** Top 1000 players shown, with regional filters

#### 3. Friends Leaderboard
- **Platform Integration:** Steam Friends, Game Center (iOS), Google Play Games (Android)
- **Categories:** All global categories filtered to friends list
- **Display:** Friend avatar, username, rank, and score
- **Notifications:** Alert when friend beats your score

### Visual Design

#### Leaderboard UI
**Main Screen:**
- **Location:** Accessible from Hangar via holographic terminal
- **Visual Style:** Holographic data streams with alien glyphs
- **Animation:** Entries fade in with particle effects
- **Color Coding:**
  - Top 10: Gold glow
  - Top 100: Silver glow
  - Top 1000: Bronze glow
  - Player entry: Cyan highlight

**Entry Information:**
| Column | Data |
|--------|------|
| Rank | Position (with arrow indicating movement since last update) |
| Player Name | Username with platform icon (Steam/iOS/Android) |
| Score/Time | Primary metric for the category |
| Secondary Stat | Tiebreaker or additional context |
| Date | When score was achieved |

#### In-Game Integration
**Post-Run Screen:**
- **Score Breakdown:** Shows performance metrics
- **Rank Achievement:** "You ranked #247 globally in Survival Time!" with celebratory particle burst
- **Personal Best Indicator:** Gold star icon if run set new personal record
- **Comparison Widget:** "You beat 73% of your friends in this category"

### Technical Implementation

#### Backend Architecture
**Platform-Agnostic Service:**
- **Primary:** Custom REST API backend (Firebase/AWS GameLift/PlayFab)
- **Fallback:** Platform-specific leaderboards (Steam Leaderboards, Game Center, Google Play Games Services)
- **Sync Strategy:** Submit scores to both custom API and platform services

**Anti-Cheat Measures:**
- **Server-side validation:** Cross-reference survival time with expected enemy spawn rates
- **Anomaly detection:** Flag scores that deviate >3 standard deviations from mean
- **Replay verification:** Store compressed run data for top 100 entries
- **Checksum validation:** Hash critical gameplay variables during run
- **Manual review:** Top 10 entries undergo human verification for major tournaments

**Data Structure (Score Submission):**
```json
{
  "playerId": "unique_player_id",
  "platform": "Steam|iOS|Android",
  "timestamp": "ISO8601_datetime",
  "category": "survival_time|enemies_defeated|scrap_collected",
  "score": 1234,
  "secondaryMetric": 567,
  "runChecksum": "hash_value",
  "gameVersion": "1.2.3",
  "permanentUpgrades": { "hull": 5, "weapons": 3, ... }
}
```

#### Performance Optimization
- **Caching:** Local cache of top 1000 entries, refreshed every 5 minutes
- **Pagination:** Load 50 entries at a time
- **Regional Servers:** Separate leaderboards for NA/EU/ASIA/OCEANIA with "Global" aggregate view
- **Compression:** Gzip compression for leaderboard data transfer

### Privacy & Settings

**Player Options:**
- **Visibility Toggle:** Opt out of global leaderboards (still tracks personal bests)
- **Username Display:** Use Steam/platform name or custom alias
- **Ghost Mode:** Hide from friends leaderboard but still participate globally
- **Score History:** View past 50 submitted scores with trends graph

**GDPR Compliance:**
- Player data deletion removes all leaderboard entries
- Leaderboard data retention: 12 months, then anonymized
- No personal information displayed beyond chosen username

### Rewards & Incentives

#### Achievement Integration
- **Leaderboard Achievements:**
  - "Top Pilot" - Reach top 1000 in any global category (20G)
  - "Elite Squadron" - Reach top 100 in any global category (50G)
  - "Champion of the Void" - Reach top 10 in any global category (100G)
  - "Daily Warrior" - Complete 7 daily challenges (30G)
  - "Friend Rivalry" - Beat all friends in at least one category (25G)

#### Cosmetic Rewards
- **Rank-Based Ship Skins:**
  - Top 1000: "Chromatic Fade" ship pattern
  - Top 100: "Void Crystal" hull effect
  - Top 10: "Ethereal Champion" animated trail effect

- **Challenge Skins:**
  - Daily Challenge Top 100: Rotating daily skin set (24h exclusive)
  - Weekly Challenge Top 10: Permanent exclusive "Hall of Champions" emblem

### Mobile Considerations

**Separate Mobile Leaderboards:**
- **Rationale:** Control scheme differences create unequal playing field
- **Implementation:** Mobile platforms compete in separate pools
- **Display:** Show both mobile and PC rankings with toggle
- **Combined View Option:** "All Platforms" view for interested players

**Mobile-Specific Categories:**
- **Touch Precision Award:** Bonus points for avoiding damage on mobile
- **One-Handed Hero:** Special leaderboard for players using one-handed mode

### Post-Launch Expansion

**Seasonal Leaderboards (v1.1):**
- **Duration:** 3-month seasons
- **Reset:** Full leaderboard wipe with Hall of Fame archive
- **Rewards:** Season-exclusive cosmetics for top performers
- **Progression:** Season pass track with leaderboard milestone rewards

**Clan/Team Leaderboards (v1.2):**
- **Formation:** Players join clans (max 50 members)
- **Scoring:** Aggregate of top 10 clan members' best scores
- **Clan Wars:** Monthly events where clans compete for territory control
- **Rewards:** Clan-wide bonuses and exclusive clan ship skins

**Tournament Mode (v1.3):**
- **Format:** Bracket-style elimination tournaments
- **Entry:** Free weekly tournaments, premium monthly tournaments ($5 entry, prize pool)
- **Spectator Mode:** Live viewing of tournament matches
- **Prizes:** Cash prizes for top 3 in premium tournaments (70%/20%/10% split)

---

## 14. MARKETING & COMMUNITY

### Key Selling Points
1. **Reverse the narrative:** Play as the alien defender
2. **Deep progression:** Permanent upgrades that meaningfully change gameplay
3. **SOLID engineering:** Quality code architecture even in solo development
4. **Vampire Survivors meets Space:** Familiar formula in fresh setting

### Target Communities
- Roguelite enthusiasts (r/roguelites, r/roguelike)
- Vampire Survivors fans
- Space game communities
- Indie game supporters
- **Premium mobile gaming community** (r/iosgaming, r/AndroidGaming - "premium games" advocates)
- **TouchArcade forums** (influential premium mobile gaming site)

### Marketing Materials Needed
- **Gameplay trailer** (60-90 seconds)
- **GIF collection** showing key moments and upgrades
- **Press kit** with screenshots and fact sheet
- **Steam page** with compelling description and imagery

---

## 15. SUCCESS METRICS

### Critical Success Factors
- **Gameplay Loop:** Players completing multiple runs in single session
- **Meta-Progression:** 80%+ of players engaging with Hangar upgrades
- **Difficulty Balance:** Average run length of 15-20 minutes
- **Technical Performance:** 95%+ of players maintaining 60+ FPS

### Launch Targets
**PC (Steam) - Week 1:**
- **Sales:** 1,000+ copies
- **Reviews:** "Very Positive" rating (80%+ positive)
- **Playtime:** Average 10+ hours per player
- **Retention:** 40%+ of players returning for second session

**Mobile (iOS/Android) - Month 1:**
- **Downloads:** 5,000+ combined (App Store + Google Play)
- **Rating:** 4.5+ stars average
- **Reviews:** Focus on "premium quality, no ads" in positive reviews
- **Retention:** 35%+ day-7 retention (typical for premium mobile)
- **Refund Rate:** Under 5% (low refunds indicate quality perception)

**Cross-Platform:**
- **Cloud Save Usage:** 20%+ of players syncing between devices
- **Cross-Purchase Rate:** 15%+ of mobile players buying PC version

---

## 16. RISKS & MITIGATION

### Technical Risks
| Risk | Impact | Mitigation |
|------|--------|------------|
| Performance issues with particle effects | High | Implement object pooling, LOD system |
| Save corruption | High | Multiple save slots, cloud backup |
| Physics bugs in zero-G movement | Medium | Extensive playtesting, clamping values |

### Design Risks
| Risk | Impact | Mitigation |
|------|--------|------------|
| Progression feels too slow | High | Frequent playtesting, adjustable scrap rates |
| Enemy patterns become predictable | Medium | Randomized spawn patterns, enemy mixing |
| Upgrade balance issues | Medium | Data-driven balance, easy iteration |

### Development Risks
| Risk | Impact | Mitigation |
|------|--------|------------|
| Scope creep | High | Strict feature freeze after week 3 |
| Solo burnout | High | Regular breaks, realistic daily goals |
| Missing deadline | Medium | Core features prioritized, polish optional |

---

## 17. CONCLUSION

**The Invasion Reforged** combines the addictive gameplay loop of Vampire Survivors with the atmospheric tension of space combat, all wrapped in a unique narrative perspective where the player defends against human expansion.

Through SOLID architecture principles and disciplined solo development, the project maintains professional code quality while delivering a complete, polished experience in a one-month development cycle.

**Core Philosophy:**
> "Scrap is the blood of evolution. Entropy is your ally."

The game respects player time with quick run cycles, meaningful progression, and constantly evolving strategies. Each death teaches, each run strengthens, and each upgrade brings you closer to mastering the human invasion.

---

## APPENDIX A: GLOSSARY

**Roguelite:** Game genre with permanent progression between runs

**Auto-Shooter:** Combat system where weapons fire automatically

**Meta-Progression:** Permanent upgrades that persist after death

**SOLID:** Software design principles for maintainable code

**Skybox:** 3D background environment visible from any point

**Object Pooling:** Performance optimization technique for reusable objects

---

## APPENDIX B: REFERENCES & INSPIRATION

**Games:**
- Vampire Survivors (auto-combat, upgrade systems)
- Everspace (space combat, roguelite structure)
- Risk of Rain (progression design)
- Hades (permanent upgrade philosophy)

**Visual References:**
- Guardians of the Galaxy (space aesthetics)
- Tron Legacy (neon UI design)
- Interstellar (planetary scale)

---

**Document Version:** 1.0

**Last Updated:** November 2025

**Author:** Solo Developer

**Project Status:** Pre-Production Complete

---

*End of Game Design Document*

