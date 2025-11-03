# The Invasion: Reforged

**What:** 3D space shooter roguelite where you're the alien  
**Engine:** Unity 2022.3 LTS  
**Developer:** Anderson Gonçalves  
**Timeline:** 14 weeks to Early Access (Feb 16, 2026)  
**Status:** Week 1 - Building the art foundation

---

## What is This?

Remake of my college game, but actually done right this time.

You pilot an alien ship against endless waves of human fleets. Collect XP, level up mid-fight, die, spend your scrap on permanent upgrades, get stronger, go again. Low-poly retro aesthetic with VHS filter because the 80s had the best arcade games.

Launching Early Access with 3 arenas and a 2-phase boss at $7.99, then adding content every 2-3 weeks until full release at $9.99.

---

## Where We're At

**Week 1** (Nov 4-10): Modeling player ship, 3 enemies, and VHS filter prototype

The code was reset. I'm doing this right: art first (weeks 1-6), then implementation (weeks 7-14). No more building on shaky foundations.

**Why art first?**
- Need polished visuals for marketing 2-3 weeks before launch
- Art defines scope (can't add "just one more" if models don't exist)
- Gives me time to plan the architecture properly
- Actually looks like a real game when I start coding

---

## The Plan

**Weeks 1-6:** Make all the art (ships, enemies, arenas, effects)  
**Weeks 7-11:** Build the game (movement, combat, progression, boss)  
**Week 11:** Feature freeze - no new stuff, just polish  
**Weeks 12-14:** Marketing prep, trailer, Steam page  
**Week 14:** Launch Early Access ($7.99)

**After EA Launch:**
- Week 15-16: Add 2 more arenas
- Week 17-19: Add 3rd boss phase and new enemies
- Week 20-24: Add leaderboards and endless mode
- Week 25+: Exit Early Access at $9.99

**Why Early Access?**
- Ship 5 weeks sooner than waiting for "full version"
- Start making money at week 14 instead of week 19
- Get player feedback while building
- Multiple launch moments = more visibility

**EA Content:**
- 3 planetary arenas with different effects
- 2-phase boss fight
- 3 enemy types with unique behaviors
- Full progression system (temporary + permanent upgrades)
- 10+ hours of gameplay

---

## Technical Approach

Building this with clean architecture so I don't hate myself in week 10:

- **SOLID principles** - Interfaces, dependency injection, single responsibility
- **Test-driven** - Write tests first, then implementation
- **Data-driven** - Add enemies/weapons without touching code
- **Multiplayer-ready** - Event-driven design, even if I never ship co-op

The goal: change one thing, break nothing.

Full details in [UNITY_CLEAN_ARCHITECTURE.md](Assets/Documents/01_Domain/Technical/UNITY_CLEAN_ARCHITECTURE.md)

---

## How Data-Driven Works

Once the code is done (week 11), adding content should be drag-and-drop:

**New enemy?**
1. Create stats ScriptableObject (health, speed, damage)
2. Pick movement pattern from dropdown
3. Assign attack behaviors
4. Done - no code needed

**New weapon?**
1. Create attack definition
2. Add to weapon kit
3. Done

This is the whole point of good architecture - add content without breaking stuff.

---

## Documentation

Everything's documented in `Assets/Documents/`:

- **START_HERE.md** - What to do this week
- **GDD.md** - Full game design
- **MASTER_PRODUCTION_SCHEDULE.md** - Week-by-week timeline
- **LOW_POLY_VHS_GUIDE.md** - Art style guide
- **UNITY_CLEAN_ARCHITECTURE.md** - Code patterns
- **EARLY_ACCESS_STRATEGY.md** - Launch plan

Start with [Assets/Documents/00_Core/README.md](Assets/Documents/00_Core/README.md)

---

## Tech Stack

- Unity 2022.3 LTS (URP)
- C# with SOLID principles
- Unity Test Framework for TDD
- New Input System for mobile + PC
- Shader Graph for VHS effects

---

## What I'm Doing This Week

**Week 1 tasks:**
- [ ] Player ship model (12h)
- [ ] 3 enemy models (20h)
- [ ] VHS filter prototype (8h)
- [ ] Unity project setup (6h)

See [START_HERE.md](Assets/Documents/00_Core/START_HERE.md) for daily breakdown.

**Week 11 checkpoint:** Feature freeze. No new content after this, only polish.  
**Week 14:** Launch on Steam Early Access and mobile app stores.

---

## Contributing

Want to help? Read [CONTRIBUTING.md](CONTRIBUTING.md) first.

This project uses SOLID principles and test-driven development. All contributions must maintain code quality.

---

## License

Proprietary - All Rights Reserved  
© 2025 Anderson Gonçalves

---

**Current:** Week 1 art production  
**Next:** Week 2 - heavy enemies and boss model  
**Goal:** Ship Early Access in 14 weeks
