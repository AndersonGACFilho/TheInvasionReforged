# 🎮 Domain - Game Design & Technical Specifications

**Bounded Context:** Core game domain knowledge and technical implementation standards

---

## 📂 Structure

```
01_Domain/
├── Design/                      # Game Design Domain
│   └── GDD.md                  # Game Design Document (Single Source of Truth)
│
└── Technical/                   # Technical Implementation Domain
    ├── LOW_POLY_VHS_GUIDE.md   # Art style & asset creation standards
    └── UNITY_CLEAN_ARCHITECTURE.md  # Code patterns & SOLID principles
```

---

## 🎯 Purpose

This bounded context contains the **ubiquitous language** and **domain knowledge** for:

- **Game Design:** Core mechanics, systems, progression, balancing
- **Technical Standards:** Architecture patterns, art style, implementation guidelines

---

## 📋 Quick Reference

### For Game Designers
- **[Design/GDD.md](Design/GDD.md)** - Complete game design specification
  - Core gameplay loop
  - Progression systems
  - Enemy behaviors
  - Balancing parameters

### For Programmers
- **[Technical/UNITY_CLEAN_ARCHITECTURE.md](Technical/UNITY_CLEAN_ARCHITECTURE.md)** - Code architecture
  - SOLID principles
  - Design patterns
  - Testing standards (TDD)
  - Multiplayer readiness

### For Artists
- **[Technical/LOW_POLY_VHS_GUIDE.md](Technical/LOW_POLY_VHS_GUIDE.md)** - Visual standards
  - Low-poly modeling guidelines
  - VHS filter specifications
  - Color palette
  - Asset optimization

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

