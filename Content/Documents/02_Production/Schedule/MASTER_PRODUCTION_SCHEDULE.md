# Production Schedule: Early Access Launch

**Timeline:** 14 weeks (Nov 4, 2025 → Feb 16, 2026)  
**Strategy:** Art-first, feature freeze Week 11, EA launch Week 14  
**Post-EA:** 3 content updates → Full release Week 25+

---

## Phase Overview

**Weeks 1-6:** Art Production (all assets)  
**Weeks 7-11:** Implementation (feature freeze Week 11)  
**Weeks 12-14:** Polish & launch prep  
**Week 14:** Early Access launch  
**Weeks 15-24:** Content updates (3 major updates)  
**Week 25+:** Full release v1.0

---

## Phase 1: Art Production (Weeks 1-6)

### Week 1: Player Ship + Basic Enemies (Nov 4-10)
- Player ship (800-1500 tris, 12h)
- Falcon Mk I enemy (500-800 tris, 8h)
- Sentry-03 enemy (400-700 tris, 6h)
- VHS filter prototype (8h)
- UE5 project setup (6h)

**Deliverable:** 3 models in UE5, VHS filter functional

---

### Week 2: Heavy Units + Boss (Nov 11-17)
- Atlas heavy bomber (1000-1500 tris, 10h)
- Raven-IX kamikaze (400-600 tris, 6h)
- Aegis turret (300-500 tris, 4h)
- Prometheus boss modular (2000-3000 tris, 16h)
- Week 1 refinement (6h)

**Deliverable:** 4 additional enemies, boss with modular parts

---

### Week 3: Materials & Colors (Nov 18-24)
- Player ship materials (6h)
- Enemy materials (8h)
- Boss materials (6h)
- Color palette documentation (2h)
- VHS filter tuning (6h)
- Material library (4h)

**Deliverable:** All models with flat-shaded materials, VHS aesthetic finalized

---

### Week 4: Environments (Nov 25-Dec 1)
- 3 arena skyboxes: Aureon, Cryovex, Voltra-9 (24h)
- Environment props (20+ asteroids/debris, 8h)
- Arena lighting setups (6h)

**Deliverable:** 3 distinct arenas ready (2 more deferred to Update 1)

---

### Week 5: VFX + Particles (Dec 2-8)
- Weapon effects (plasma, ion, explosions, 14h)
- Pickup effects (scrap, level-up, 8h)
- Shield/damage overlays (4h)
- Engine trails (4h)
- Environmental particles (4h)

**Deliverable:** 15+ particle systems, <5000 particles total

---

### Week 6: UI + Hangar Hub (Dec 9-15)
- Hangar environment (12h)
- Alien pilot character + animations (8h)
- Repair drones + ambient props (6h)
- HUD elements (6h)
- Upgrade selection UI (6h)
- Menus + icon library (8h)

**Deliverable:** Complete hangar scene, all UI screens

**Milestone:** All visual assets complete, art-locked

---

## Phase 2: Implementation (Weeks 7-11)

### Week 7: Core Systems (Dec 16-22)
- Player movement (10h)
- Auto-fire weapons (8h)
- Health/damage (6h)
- Scrap collection (4h)
- Unit tests (12h)
- Mobile controls prototype (6h)

**Deliverable:** Core combat functional, 90%+ test coverage

---

### Week 8: Enemy AI (Dec 23-29)
- Falcon AI (10h)
- Sentry AI (8h)
- Atlas AI (10h)
- Wave spawning (8h)
- Object pooling (4h)

**Deliverable:** 3 enemy types, wave system, 60 FPS with 20 enemies

**Checkpoint:** Core combat playable (5-minute sessions)

---

### Week 9: Progression (Dec 30-Jan 5)
- XP system (6h)
- Level-up system (6h)
- Temporary upgrades (8-10 types, 12h)
- Upgrade selection UI (10h)
- VFX/audio feedback (4h)
- Unit tests (8h)

**Deliverable:** Full temporary progression, upgrade synergies working

---

### Week 10: Meta + Boss (Jan 6-12)
- Hangar scene implementation (8h)
- Permanent upgrades (6 categories, 10h)
- Quantum Entropy Matrix (4h)
- Save/load system (8h)
- Prometheus boss (2 phases, 12h)
- Unit tests (6h)

**Deliverable:** Complete meta loop, boss fight (phase 3 in Update 2)

**Checkpoint:** First playable - full run-death-upgrade loop

---

### Week 11: Polish + Feature Freeze (Jan 13-19)
- 3 arena effects (12h)
- Balance pass (8h)
- Mobile build testing (6h)
- Bug fixing (10h)
- Performance optimization (6h)
- **FEATURE FREEZE**

**Deliverable:** 3 arenas functional, 60 FPS stable, scope locked

**Milestone:** Feature complete, no new content after this

---

## Phase 3: Launch Prep (Weeks 12-14)

### Week 12: Sound + Testing (Jan 20-26)
- Sound design (12h)
- Music integration (6h)
- Playtesting (8h)
- Bug fixes (10h)
- Performance pass (6h)
- Mobile QA (6h)

---

### Week 13: Marketing (Jan 27-Feb 2)
- Steam EA page setup (8h)
- Gameplay trailer 60-90s (12h)
- Demo build (6h)
- DevLog 1: "Why EA?" (4h)
- Press kit (4h)
- Screenshot library (4h)

---

### Week 14: Launch (Feb 3-16)
- Final builds (PC, iOS, Android, 10h)
- Platform submission (8h)
- Pre-launch marketing (6h)
- **Feb 16: EARLY ACCESS LAUNCH**
- Community monitoring (ongoing)

**Price:** $7.99 PC, $4.99 mobile

---

## Phase 4: Post-EA Updates

### Update 1: Arena Expansion (Weeks 15-16, Feb 17-Mar 2)
- Zerion Nexus arena (8h)
- Ethereal Void arena (8h)
- Balance adjustments (8h)
- DevLog 2 (4h)

**Content:** +2 arenas (5 total)

---

### Update 2: Boss Enhancement (Weeks 17-19, Mar 3-23)
- Prometheus phase 3 (8h)
- 2 new enemy variants (12h)
- Additional permanent upgrades (6h)
- Balance pass (6h)
- DevLog 3 (4h)

**Content:** 3-phase boss, expanded enemy roster

---

### Update 3: Competitive Features (Weeks 20-24, Mar 24-Apr 27)
- Global leaderboards (16h)
- Daily/weekly challenges (12h)
- Endless survival mode (12h)
- Friends leaderboard integration (8h)
- Anti-cheat validation (8h)
- DevLog 4 (4h)

**Content:** Leaderboards, challenges, endless mode

---

## Phase 5: Full Release (Week 25+, Apr 28+)

- Exit Early Access
- Price increase: $9.99 PC (mobile stays $4.99)
- Full release marketing campaign
- Consider console ports

---

## Risk Management

**Art Phase Risks:**
- VHS filter too subtle → Increase intensity
- Models lack distinction → Exaggerate silhouettes
- Boss too complex → Simplify phase mechanics

**Implementation Risks:**
- Movement feels floaty → Adjust physics parameters
- Performance issues → Dynamic resolution, reduce particles
- Balance problems → Extended playtesting Week 11

**Launch Risks:**
- Bugs in EA → Week 11 feature freeze prevents last-minute issues
- Marketing timing → Assets ready Week 6 for devlogs
- Platform approval delays → Submit early, buffer time

---

## Success Metrics

**Week 1 PC Launch:**
- 1,000+ sales
- "Very Positive" Steam rating
- 10+ hours average playtime
- 40%+ return session rate

**Month 1 Mobile:**
- 5,000+ downloads
- 4.5+ star rating
- 35%+ day-7 retention
- <5% refund rate

---

**Last Updated:** November 13, 2025  
**Current Week:** Week 2 (Heavy Units & Boss Models)

