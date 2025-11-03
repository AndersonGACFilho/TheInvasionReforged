# 📊 Production - Project Management & Scheduling

**Bounded Context:** Timeline, milestones, resources, and launch planning

---

## 📂 Structure

```
02_Production/
├── Schedule/                              # Timeline & Milestones
│   └── MASTER_PRODUCTION_SCHEDULE.md     # 17-week production plan (SSOT)
│
└── Launch/                                # Go-to-Market Strategy
    └── LAUNCH_PLAN.md                    # Marketing & distribution plan
```

---

## Purpose

This bounded context manages:

- **Project Timeline:** Phases, sprints, deliverables, deadlines
- **Resource Allocation:** Who does what and when
- **Risk Management:** Blockers, dependencies, contingencies
- **Launch Strategy:** Marketing, distribution, community building

---

## 📅 Production Overview

### Timeline (17 Weeks)
```
PHASE 1: Art Production (Weeks 1-6)    ─────► Dec 15, 2025
    └─► All visual assets complete

PHASE 2: Gameplay Implementation (Weeks 7-14) ─────► Feb 9, 2026
    └─► Fully playable vertical slice

PHASE 3: Marketing & Launch (Weeks 15-17) ─────► Mar 2, 2026
    └─► Steam release + itch.io
```

### Key Milestones
- **Week 6:** Art lock (all assets ready for implementation)
- **Week 10:** First playable build (core loop functional)
- **Week 14:** Code complete (feature freeze)
- **Week 17:** Launch day (Steam + itch.io)

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

