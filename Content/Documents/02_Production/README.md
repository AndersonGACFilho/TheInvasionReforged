# 📊 Production - Project Management & Scheduling

**Bounded Context:** Timeline, milestones, resources, and launch planning  
**Strategy:** Early Access → Full Release  
**Engine:** Unreal Engine 5.7

---

## 📂 Structure

```
02_Production/
├── Schedule/                              # Timeline & Milestones
│   └── MASTER_PRODUCTION_SCHEDULE.md     # 14-week EA production plan (SSOT)
│
├── Launch/                                # Go-to-Market Strategy
│   ├── EARLY_ACCESS_STRATEGY.md          # EA roadmap & pricing
│   └── LAUNCH_PLAN.md                    # Marketing & distribution plan
│
└── RISK_REGISTER.md                       # Risk tracking & mitigation
```

---

## Purpose

This bounded context manages:

- **Project Timeline:** Phases, sprints, deliverables, deadlines
- **Resource Allocation:** Who does what and when (solo dev)
- **Risk Management:** Blockers, dependencies, contingencies
- **Launch Strategy:** Early Access → Full Release roadmap
- **Marketing:** Community building, Steam page, trailers

---

## 📅 Production Overview

### Early Access Timeline (14 Weeks)
```
PHASE 1: Art Production (Weeks 1-6)    ─────► Dec 15, 2025
    └─► All visual assets complete (3 arenas, enemies, boss, hangar)

PHASE 2: Gameplay Implementation (Weeks 7-11) ─────► Jan 20, 2026
    └─► Fully playable core loop (hangar + combat + progression)

PHASE 3: Early Access Launch (Weeks 12-14) ─────► Feb 16, 2026
    └─► Steam EA launch (PC, iOS, Android) - $7.99
```

### Post-Launch Content Updates
```
PHASE 4: Update 1 (Weeks 15-16)    ─────► Mar 2, 2026
    └─► +2 arenas (Zerion Nexus, Ethereal Void)

PHASE 5: Update 2 (Weeks 17-19)    ─────► Mar 23, 2026
    └─► Boss 3rd phase + new enemies

PHASE 6: Update 3 (Weeks 20-24)    ─────► Apr 27, 2026
    └─► Leaderboards + Endless mode

PHASE 7: Full Release (Week 25+)    ─────► Apr 28, 2026+
    └─► Exit Early Access - $9.99
```

### Key Milestones
- **Week 6 (Dec 15):** Art lock - All assets complete and in Unreal
- **Week 10 (Jan 19):** First playable build - Core loop functional
- **Week 11 (Jan 26):** Feature freeze - No new content
- **Week 14 (Feb 16):** 🚀 Early Access Launch (PC, iOS, Android)
- **Week 25+ (Apr 28+):** 🎯 Full Release v1.0

---

## 📋 Quick Reference

### For Producer/PM
- **[Schedule/MASTER_PRODUCTION_SCHEDULE.md](Schedule/MASTER_PRODUCTION_SCHEDULE.md)**
  - Week-by-week breakdown
  - Task dependencies
  - Critical path analysis
  - Risk mitigation strategies

### For Marketing Lead
- **[Launch/LAUNCH_PLAN.md](Launch/LAUNCH_PLAN.md)**
  - Pre-launch content calendar
  - Community building strategy
  - Steam page optimization
  - DevLog video schedule

---

## Production Philosophy

### Art-First Approach
Unlike traditional gamedev (code → prototype → polish art), this project prioritizes:
1. **Complete art production first** (Weeks 1-6)
2. **Implement with polished assets** (Weeks 7-14)
3. **Market with professional visuals** (Weeks 15-17)

**Rationale:**
- DevLogs need polished visuals 2-3 weeks before launch
- Beautiful assets motivate better coding
- Art scope locks early, preventing feature creep
- Clean architecture easier without time pressure

### Single Source of Truth (SSOT)
**MASTER_PRODUCTION_SCHEDULE.md** is the authoritative timeline:
- All dates, tasks, and deliverables reference this document
- No conflicting schedules in other docs
- Updated weekly during production

---

## Launch Readiness Checklist

### Week 15 (Pre-Launch)
- [ ] Steam page live with trailer
- [ ] itch.io page configured
- [ ] Demo build polished and tested
- [ ] DevLog #1 published

### Week 16 (T-Minus 1 Week)
- [ ] Press kit distributed
- [ ] Community Discord active
- [ ] DevLog #2 published
- [ ] Final QA pass complete

### Week 17 (Launch Week)
- [ ] Steam release button ready
- [ ] Social media campaign live
- [ ] DevLog #3 (launch day video)
- [ ] Post-launch support plan active

---

**Last Updated:** November 4, 2025  
**Next Review:** Weekly on Mondays

