# 📚 The Invasion: Reforged - Documentation Hub

**Project:** The Invasion: Reforged  
**Engine:** Unreal Engine 5.7  
**Developer:** Anderson Gonçalves (solo)  
**Strategy:** Early Access Launch → Full Release  
**Timeline:** 14 weeks to Early Access (Feb 16, 2026)

---

## 🚀 Quick Start

**New to the project? Start here:**

1. **[00_Core/START_HERE.md](00_Core/START_HERE.md)** - Current week's tasks
2. **[00_Core/README.md](00_Core/README.md)** - Documentation navigation
3. **[01_Domain/Design/GDD.md](01_Domain/Design/GDD.md)** - Game design overview

---

## 📁 Documentation Structure

```
Documents/
│
├── 00_Core/                    ⭐ START HERE
│   ├── README.md              → Documentation guide
│   ├── START_HERE.md          → Week 2 tasks (current)
│   └── MIGRATION_SUMMARY.md   → Unity → Unreal migration log
│
├── 01_Domain/                  🎮 GAME DESIGN & TECH
│   ├── Design/
│   │   └── GDD.md             → Complete game design
│   └── Technical/
│       ├── UNREAL_CLEAN_ARCHITECTURE.md  → C++/Blueprint patterns
│       ├── HANGAR_HUB_DESIGN.md          → Meta-game hub system
│       ├── LOW_POLY_VHS_GUIDE.md         → Art style guide
│       ├── MOBILE_CONTROLS_DESIGN.md     → Touch controls
│       ├── PERFORMANCE_TARGETS.md        → Performance goals
│       └── TESTING_STRATEGY.md           → TDD approach
│
├── 02_Production/              📅 TIMELINE & LAUNCH
│   ├── Schedule/
│   │   └── MASTER_PRODUCTION_SCHEDULE.md → 14-week plan
│   ├── Launch/
│   │   ├── EARLY_ACCESS_STRATEGY.md      → EA roadmap
│   │   └── LAUNCH_PLAN.md                → Marketing plan
│   └── RISK_REGISTER.md                  → Risk management
│
└── 03_Features/                🔮 POST-LAUNCH
    └── Leaderboard/
        ├── LEADERBOARD_ARCHITECTURE_DIAGRAM.md
        ├── LEADERBOARD_IMPLEMENTATION_SUMMARY.md
        └── LEADERBOARD_QUICK_REF.md
```

---

## 🎯 Find What You Need

### I want to...

| Goal | Document |
|------|----------|
| **See this week's tasks** | [00_Core/START_HERE.md](00_Core/START_HERE.md) |
| **Understand the game** | [01_Domain/Design/GDD.md](01_Domain/Design/GDD.md) |
| **Learn the architecture** | [01_Domain/Technical/UNREAL_CLEAN_ARCHITECTURE.md](01_Domain/Technical/UNREAL_CLEAN_ARCHITECTURE.md) |
| **Create art assets** | [01_Domain/Technical/LOW_POLY_VHS_GUIDE.md](01_Domain/Technical/LOW_POLY_VHS_GUIDE.md) |
| **Build the hangar hub** | [01_Domain/Technical/HANGAR_HUB_DESIGN.md](01_Domain/Technical/HANGAR_HUB_DESIGN.md) |
| **Check the timeline** | [02_Production/Schedule/MASTER_PRODUCTION_SCHEDULE.md](02_Production/Schedule/MASTER_PRODUCTION_SCHEDULE.md) |
| **Review risks** | [02_Production/RISK_REGISTER.md](02_Production/RISK_REGISTER.md) |
| **Plan the launch** | [02_Production/Launch/EARLY_ACCESS_STRATEGY.md](02_Production/Launch/EARLY_ACCESS_STRATEGY.md) |
| **Implement leaderboards** | [03_Features/Leaderboard/](03_Features/Leaderboard/) |

---

## 📖 Documentation by Role

### 🎨 Artists
1. [LOW_POLY_VHS_GUIDE.md](01_Domain/Technical/LOW_POLY_VHS_GUIDE.md) - Visual style standards
2. [GDD.md](01_Domain/Design/GDD.md) - Sections 2, 4, 5 (Arenas, Weapons, Enemies)
3. [HANGAR_HUB_DESIGN.md](01_Domain/Technical/HANGAR_HUB_DESIGN.md) - Section: Scene Layout

**Current Focus (Week 2):** Heavy enemy models + Prometheus boss

### 💻 Programmers
1. [UNREAL_CLEAN_ARCHITECTURE.md](01_Domain/Technical/UNREAL_CLEAN_ARCHITECTURE.md) - Core architecture
2. [HANGAR_HUB_DESIGN.md](01_Domain/Technical/HANGAR_HUB_DESIGN.md) - Hangar implementation
3. [TESTING_STRATEGY.md](01_Domain/Technical/TESTING_STRATEGY.md) - TDD approach
4. [PERFORMANCE_TARGETS.md](01_Domain/Technical/PERFORMANCE_TARGETS.md) - Optimization goals

**Current Focus (Week 2):** Planning architecture for Week 7+ implementation

### 🎮 Game Designers
1. [GDD.md](01_Domain/Design/GDD.md) - Complete game design
2. [EARLY_ACCESS_STRATEGY.md](02_Production/Launch/EARLY_ACCESS_STRATEGY.md) - Content roadmap
3. [HANGAR_HUB_DESIGN.md](01_Domain/Technical/HANGAR_HUB_DESIGN.md) - Meta-game flow

**Current Focus (Week 2):** Reviewing enemy designs, boss mechanics

### 📱 Mobile Developers
1. [MOBILE_CONTROLS_DESIGN.md](01_Domain/Technical/MOBILE_CONTROLS_DESIGN.md) - Touch controls
2. [PERFORMANCE_TARGETS.md](01_Domain/Technical/PERFORMANCE_TARGETS.md) - Mobile optimization
3. [GDD.md](01_Domain/Design/GDD.md) - Section: Mobile Controls

**Current Focus (Week 2):** Planning mobile input for Week 10+ implementation

### 📊 Project Managers
1. [MASTER_PRODUCTION_SCHEDULE.md](02_Production/Schedule/MASTER_PRODUCTION_SCHEDULE.md) - Timeline
2. [RISK_REGISTER.md](02_Production/RISK_REGISTER.md) - Risk tracking
3. [LAUNCH_PLAN.md](02_Production/Launch/LAUNCH_PLAN.md) - Go-to-market strategy

**Current Focus (Week 2):** Monitoring Week 2 progress, preparing Week 3

---

## 🗓️ Current Status

**Week:** 2 of 14 (Nov 11-17, 2025)  
**Phase:** Art Production  
**Milestone:** Heavy Units & Boss Models

### This Week
- [ ] Atlas heavy bomber (1000-1500 tris)
- [ ] Raven-IX kamikaze (400-600 tris)
- [ ] Aegis turret (300-500 tris)
- [ ] Prometheus boss (2000-3000 tris, modular)
- [ ] Week 1 model refinement

### Next Week (Week 3)
- Materials and color palette
- Flat shading implementation
- VHS filter final tuning
- Color palette documentation

---

## 🎯 Project Overview

### What We're Building
A roguelite space shooter where you play as an alien defending against human invaders. Features:

- **Hangar hub** - Walk around as alien pilot, upgrade ship, launch missions
- **Wave-based combat** - Top-down space battles in planetary arenas
- **Dual progression** - Temporary upgrades (runs) + permanent upgrades (hangar)
- **Visual evolution** - Ship physically changes as you upgrade
- **Retro aesthetic** - Low-poly models + VHS post-processing

### Early Access Launch (Feb 16, 2026)
**Platforms:** PC (Steam), iOS, Android  
**Price:** $7.99 (increases to $9.99 at full release)

**Launch Content:**
- 3 planetary arenas
- 2-phase boss fight (Prometheus)
- Complete progression system (6 permanent upgrade categories)
- 8-10 temporary upgrades
- Hangar meta-game hub

**Post-Launch Updates (Free):**
- **Update 1 (Weeks 15-16):** +2 arenas
- **Update 2 (Weeks 17-19):** Boss 3rd phase, new enemies
- **Update 3 (Weeks 20-24):** Leaderboards, endless mode
- **Full Release (Week 25+):** Exit Early Access

---

## 🛠️ Technology Stack

**Engine:** Unreal Engine 5.7
- **Rendering:** Lumen (real-time GI), Nanite (virtualized geometry)
- **Language:** C++ (core systems) + Blueprints (rapid iteration)
- **Input:** Enhanced Input System (PC + mobile)
- **UI:** UMG (Unreal Motion Graphics)
- **Particles:** Niagara System
- **Materials:** Material Editor + Post Process
- **Testing:** Automation Framework (TDD)
- **Networking:** Built-in replication (future multiplayer)

**Tools:**
- Blender 3.6+ (3D modeling)
- Visual Studio 2022 / Rider (C++ IDE)
- Git (version control)

---

## 📝 Documentation Standards

### File Naming
- `UPPERCASE_WITH_UNDERSCORES.md` for top-level documents
- `PascalCase.md` for feature-specific documents
- No spaces in filenames

### Document Structure
All documents follow this template:
```markdown
# Title

**Key Metadata** (engine, status, owner, etc.)

---

## Purpose / Overview

---

## Content Sections

---

**Last Updated:** Date  
**Owner:** Name
```

### Markdown Style
- Use `##` for main sections, `###` for subsections
- Code blocks with language specification: ```cpp, ```blueprint
- Tables for comparisons and quick reference
- Emoji for visual hierarchy (sparingly)

---

## 🔄 Recent Updates

**Nov 13, 2025:**
- ✅ Migrated all documentation from Unity to Unreal Engine 5.7
- ✅ Created UNREAL_CLEAN_ARCHITECTURE.md (C++/Blueprint patterns)
- ✅ Updated HANGAR_HUB_DESIGN.md with Unreal terminology
- ✅ Added MIGRATION_SUMMARY.md for reference
- ✅ Updated all project timelines and tool references

**Nov 11, 2025:**
- ✅ Week 2 started: Heavy enemy models + boss
- ✅ Updated START_HERE.md with Week 2 tasks
- ✅ Completed Week 1: Player ship, 3 enemies, VHS filter

---

## 🤝 Contributing

See [CONTRIBUTING.md](../../CONTRIBUTING.md) in root for:
- Branch strategy
- Code style guidelines
- Pull request process
- Testing requirements

---

## 📧 Contact

**Developer:** Anderson Gonçalves  
**Project Repository:** (Add Git URL when public)  
**Documentation Issues:** Create an issue in the repository

---

**This documentation is the single source of truth for The Invasion: Reforged.**  
Keep it updated. Keep it accurate. Keep it useful.

