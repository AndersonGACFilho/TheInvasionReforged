# The Invasion: Reforged - Documentation

**Developer:** Anderson Gonçalves  
**Engine:** Unreal Engine 5.7  
**Current Phase:** Week 2 - Art Production  
**Target:** Early Access Launch - February 16, 2026

---

## Quick Start

**New here? Read these in order:**
1. [START_HERE.md](START_HERE.md) - Week 1 tasks
2. [GDD.md](../01_Domain/Design/GDD.md) - What we're building

**The Plan:**
- Launching Early Access in 14 weeks with 3 arenas and 2-phase boss
- Price: $7.99 during EA, $9.99 at full release
- Free content updates every 2-3 weeks after launch

---

## Find What You Need

| Need | Document |
|------|----------|
| **This week's tasks** | [START_HERE.md](START_HERE.md) |
| **Game design** | [GDD.md](../01_Domain/Design/GDD.md) |
| **Timeline** | [Production Schedule](../02_Production/Schedule/MASTER_PRODUCTION_SCHEDULE.md) |
| **Art style guide** | [Low-Poly VHS Guide](../01_Domain/Technical/LOW_POLY_VHS_GUIDE.md) |
| **Hangar hub design** | [Hangar Hub System](../01_Domain/Technical/HANGAR_HUB_DESIGN.md) |
| **Code architecture** | [Unreal Clean Architecture](../01_Domain/Technical/UNREAL_CLEAN_ARCHITECTURE.md) |

---

## Documentation Structure

```
Documents/
├── 00_Core/                    # Start here
│   ├── README.md              # This file
│   └── START_HERE.md          # Weekly tasks
│
├── 01_Domain/                  # Game design & technical
│   ├── Design/
│   │   └── GDD.md             # Game Design Document
│   └── Technical/
│       ├── LOW_POLY_VHS_GUIDE.md
│       ├── UNREAL_CLEAN_ARCHITECTURE.md
│       ├── MOBILE_CONTROLS_DESIGN.md
│       ├── PERFORMANCE_TARGETS.md
│       └── TESTING_STRATEGY.md
│
├── 02_Production/              # Timeline & launch
│   ├── Schedule/
│   │   └── MASTER_PRODUCTION_SCHEDULE.md
│   ├── Launch/
│   │   ├── EARLY_ACCESS_STRATEGY.md
│   │   └── LAUNCH_PLAN.md
│   └── RISK_REGISTER.md
│
└── 03_Features/                # Post-launch features
    └── Leaderboard/
```

---

## If You're Working On...

**Art (Weeks 1-6 - Current):**
- Start with [LOW_POLY_VHS_GUIDE.md](../01_Domain/Technical/LOW_POLY_VHS_GUIDE.md)
- Check GDD Sections 2, 4, 5 for asset specs
- Week 1: Player ship + 3 enemies + VHS filter
- Week 2: Heavy units + Boss (current)
- Week 6: Hangar environment + Alien pilot character

**Programming (Weeks 7+):**
- Start with [UNREAL_CLEAN_ARCHITECTURE.md](../01_Domain/Technical/UNREAL_CLEAN_ARCHITECTURE.md)
- Read [HANGAR_HUB_DESIGN.md](../01_Domain/Technical/HANGAR_HUB_DESIGN.md) for meta-game system
- Follow PERFORMANCE_TARGETS.md and TESTING_STRATEGY.md
- Follow TDD approach

**Game Design:**
- Read [GDD.md](../01_Domain/Design/GDD.md) completely
- Check [HANGAR_HUB_DESIGN.md](../01_Domain/Technical/HANGAR_HUB_DESIGN.md) for meta-game loop
- See [EARLY_ACCESS_STRATEGY.md](../02_Production/Launch/EARLY_ACCESS_STRATEGY.md) for content roadmap


---

## Current Phase Timeline

**Week 1 (Complete):** Player ship, 3 basic enemies, VHS filter prototype  
**Week 2 (Current - Nov 11-17):** Heavy units (Atlas, Raven-IX, Aegis), Prometheus boss  
**Week 3-5:** Materials, arenas, VFX  
**Week 6:** UI + Hangar hub (walk as alien pilot, upgrade ship, launch missions)  
**Week 7-11:** Core gameplay implementation  
**Week 12-14:** Polish and Early Access launch prep  
**Week 14:** Launch Early Access (Feb 16, 2026)  
**Week 15+:** Free content updates every 2-3 weeks

---

## Project Status

**Completed:**
- ✅ Game design document (with hangar hub design)
- ✅ Early Access strategy
- ✅ 14-week production schedule
- ✅ Risk assessment
- ✅ Mobile controls design
- ✅ All technical specs (architecture, hangar system, performance)
- ✅ Week 1 art assets (player ship, 3 basic enemies, VHS filter)

**Current (Week 2 - Nov 11-17):**
- 🔄 Heavy enemy models (Atlas, Raven-IX, Aegis)
- 🔄 Prometheus boss model (modular design)
- 🔄 Model refinement (Week 1 assets)
- 🔄 UV unwrapping all models

**Next Week (Week 3 - Nov 18-24):**
- Materials and color palette
- Flat shading implementation
- VHS filter final tuning
- Color palette documentation

---

That's it. Keep it simple, stay focused, ship the game.
