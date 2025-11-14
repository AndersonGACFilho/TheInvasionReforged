# Hangar Hub - Quick Reference

**What is it?** The hangar is your meta-game hub - a 3D space where you walk as the alien pilot between combat missions.

---

## The Flow

```
HANGAR → LAUNCH → COMBAT → RETURN → HANGAR
  ↑                                      ↓
  └──────────────────────────────────────┘
```

### In Hangar (Meta-Game)
1. **Walk around** as alien pilot (third-person)
2. **Approach terminals** to view/purchase permanent upgrades
3. **See your ship** being repaired on central platform
4. **Enter cockpit** when ready to launch
5. Launch sequence plays

### In Combat (Gameplay)
6. **Wave-based combat** in space arenas
7. **Kill enemies** → collect scrap
8. **Level up** → pick temporary upgrades
9. **Defeat boss** OR die

### Return to Hangar
10. **Landing sequence** plays automatically
11. **Exit cockpit**, resume walking as pilot
12. **Scrap summary** shows what you earned (70% if died)
13. Repeat

---

## Key Features

### No Traditional Menus
- Everything is spatial and interactive
- Walk to terminals instead of clicking menu buttons
- See your ship evolve visually as you upgrade

### Upgrade Terminals (6 Total)
| Terminal | Color | Upgrades |
|----------|-------|----------|
| Xenonic Integrity Core | Cyan | Hull, shields, speed |
| Primary Combat Protocols | Orange | Weapons |
| Advanced Tactical | Purple | Abilities |
| Gravitational Field | Green | Pickup range |
| Biomechanical Evolution | Red | Bio-stats |
| Quantum Entropy Matrix | Purple | Loot luck |

### Visual Ship Evolution
Ship model changes as you buy upgrades:
- **Damaged** → Repaired hull
- **Basic weapons** → Advanced weapon systems
- **Dull engines** → Glowing bio-tech thrusters
- **Starter** → Legendary appearance

### Atmosphere
- Repair drones working on ship
- Energy flowing through walls
- Welding sparks, ambient sounds
- Industrial alien architecture
- Dark with cyan/blue lighting

---

## Controls

### PC
- **WASD** - Move alien pilot
- **E** - Interact with terminals/ship
- **Space** - Skip launch/return sequences (after first time)
- **ESC** - Pause menu

### Mobile
- **Left joystick** - Move alien pilot
- **Right button** - Interact (appears near terminals/ship)
- **Skip button** - Skip sequences (after first time)

---

## Timing

| Sequence | Duration | Skippable? |
|----------|----------|------------|
| **Launch** | 8 seconds | Yes (after first time) |
| **Combat** | 15-25 minutes | No |
| **Return** | 10 seconds | Yes (after first death) |
| **Hangar time** | 1-5 minutes | Player-controlled |

---

## Development Timeline

### Week 6 (Art)
- Hangar environment (40m x 40m x 15m)
- Alien pilot character (800-1200 tris)
- 6 upgrade terminals (color-coded)
- 3-5 repair drones
- Ship visual states (5 upgrade levels)

### Week 10 (Programming)
- Character controller (movement, interaction)
- Terminal system (UI, purchase logic)
- Ship interaction (launch/return)
- Scene transitions (hangar ↔ combat)
- Ship visual state manager
- Save/load integration

---

## References

- **Full Design:** [HANGAR_HUB_DESIGN.md](../01_Domain/Technical/HANGAR_HUB_DESIGN.md)
- **Game Design:** [GDD.md](../01_Domain/Design/GDD.md)
- **Production Schedule:** [MASTER_PRODUCTION_SCHEDULE.md](../02_Production/Schedule/MASTER_PRODUCTION_SCHEDULE.md)

---

**TL;DR:** Walk around as alien, upgrade ship at terminals, enter cockpit to launch missions. No traditional menus - everything is in-world.

