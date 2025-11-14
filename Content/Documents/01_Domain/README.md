# 🎮 Domain - Game Design & Technical Specifications

**Bounded Context:** Core game domain knowledge and technical implementation standards  
**Engine:** Unreal Engine 5.7

---

## 📂 Structure

```
01_Domain/
├── Design/                      # Game Design Domain
│   └── GDD.md                  # Game Design Document (Single Source of Truth)
│
└── Technical/                   # Technical Implementation Domain
    ├── LOW_POLY_VHS_GUIDE.md           # Art style & asset creation standards
    ├── UNREAL_CLEAN_ARCHITECTURE.md    # Code patterns & SOLID principles
    ├── HANGAR_HUB_DESIGN.md            # Meta-game hub technical spec
    ├── HANGAR_HUB_QUICK_REF.md         # Quick reference for hangar system
    ├── MOBILE_CONTROLS_DESIGN.md       # Touch controls & mobile UX
    ├── PERFORMANCE_TARGETS.md          # Performance benchmarks
    └── TESTING_STRATEGY.md             # TDD approach & test coverage
```

---

## 🎯 Purpose

This bounded context contains the **ubiquitous language** and **domain knowledge** for:

- **Game Design:** Core mechanics, systems, progression, balancing
- **Technical Standards:** Architecture patterns, art style, implementation guidelines
- **Platform Requirements:** Mobile controls, performance targets, testing standards

---

## 📋 Quick Reference

### For Game Designers
- **[Design/GDD.md](Design/GDD.md)** - Complete game design specification
  - Core gameplay loop (hangar ↔ combat)
  - Progression systems (temporary + permanent)
  - Enemy behaviors and wave mechanics
  - Balancing parameters
  - Planetary arenas and boss fights

### For Programmers
- **[Technical/UNREAL_CLEAN_ARCHITECTURE.md](Technical/UNREAL_CLEAN_ARCHITECTURE.md)** - Code architecture
  - SOLID principles in C++/Blueprints
  - Actor/Component design patterns
  - Interface segregation
  - Testing standards (TDD with Automation Framework)
  - Multiplayer readiness
- **[Technical/HANGAR_HUB_DESIGN.md](Technical/HANGAR_HUB_DESIGN.md)** - Hangar meta-game system
  - Scene layout and structure
  - Character controller design
  - Terminal interaction system
  - Ship visual progression
  - Launch/return sequences
- **[Technical/TESTING_STRATEGY.md](Technical/TESTING_STRATEGY.md)** - Test-driven development
  - Unit/Integration/E2E test pyramid
  - Automation Framework usage
  - 90%+ coverage targets

### For Artists
- **[Technical/LOW_POLY_VHS_GUIDE.md](Technical/LOW_POLY_VHS_GUIDE.md)** - Visual standards
  - Low-poly modeling guidelines
  - VHS filter specifications (Material Editor)
  - Color palette (retro synthwave)
  - Asset optimization for mobile
  - Blender export settings

### For Mobile Development
- **[Technical/MOBILE_CONTROLS_DESIGN.md](Technical/MOBILE_CONTROLS_DESIGN.md)** - Touch controls
  - Virtual joystick layout
  - Button placement optimization
  - Gesture controls
  - Enhanced Input System integration
- **[Technical/PERFORMANCE_TARGETS.md](Technical/PERFORMANCE_TARGETS.md)** - Performance goals
  - PC: 60 FPS minimum, 144 FPS target
  - Mobile: 30 FPS minimum, 60 FPS on high-end
  - Memory budgets
  - Draw call limits

---

## 🧠 DDD Principles Applied

### Ubiquitous Language
All team members use consistent terminology from the GDD:
- "Scrap" not "currency" or "coins"
- "Hangar" not "hub" or "menu"
- "Wave" not "round" or "level"

### Domain Entities
Core concepts with identity and lifecycle:
- Player Ship (with stats, upgrades, weapons)
- Enemy Units (with behaviors, drops, scaling)
- Temporary Upgrades (during run)
- Permanent Upgrades (meta-progression)

### Value Objects
Immutable domain concepts:
- Damage values
- Movement speeds
- XP thresholds
- Scrap costs

---

**Last Updated:** November 4, 2025

