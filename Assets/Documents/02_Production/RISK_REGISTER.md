# Risk Register

**Project:** The Invasion: Reforged  
**Last Updated:** November 3, 2025  
**Review:** Weekly (Sundays)

---

## Risk Matrix

**Probability:** Low (10-30%) | Medium (30-60%) | High (60-90%)  
**Impact:** Low (<1 week delay) | Medium (1-2 weeks) | High (3+ weeks or project kill)

---

## Critical Risks

### RISK-001: No Buffer in Timeline
**Type:** Schedule  
**Probability:** High (70%)  
**Impact:** High (3+ weeks or quality cut)  
**Status:** MITIGATED (2-week buffer added)

**The Problem:**
Original 17-week timeline had zero slack. Any delay breaks launch.

**What Triggers It:**
- Get sick for a week+
- Unity breaking change
- Art takes 7 weeks not 6
- Boss/arena takes longer than expected

**What We're Doing:**
- Extended to 19 weeks (weeks 18-19 = buffer)
- Identified scope cuts (3 arenas not 5, 2-phase boss not 3)
- Weekly tracking to catch slippage early

**If It Still Goes Wrong:**
1. Cut to 2 arenas (saves 44-64 hours)
2. Remove boss (saves 30 hours)
3. Delay launch 1-2 weeks

**Owner:** Anderson  
**Next Check:** Week 6 (Dec 15)

---

### RISK-002: Scope Creep During Coding
**Type:** Scope  
**Probability:** High (60%)  
**Impact:** Medium-High (1-3 weeks delay)  
**Status:** ⚠️ MONITORING

**Description:**
Easy to add "just one more enemy" or "one more upgrade" during Weeks 7-14.

**Triggers:**
- Cool idea during development
- Playtester feedback suggesting new features
- Seeing similar games with more content

**Mitigation:**
- ✅ Feature Freeze defined (Week 11 - Jan 20)
- ✅ GDD locked (no new enemies/upgrades/arenas after Week 11)
- 🔄 Weekly scope review during Sunday check-ins

**Warning Signs:**
- Adding tasks to Week 12-14 that aren't bug fixes
- "This will only take 2 hours" (famous last words)
- Thinking "we need this to compete"

**Contingency Plan:**
- Any new feature ideas go into "Post-Launch Content Update" backlog
- If already implemented: Cut lowest-value feature to reclaim time

**Owner:** Anderson  
**Next Review:** Week 10 checkpoint (validate scope adherence)

---

## HIGH RISKS (Medium-High Probability or Impact)

### RISK-003: VHS Filter Performance Issues
**Category:** Technical  
**Probability:** Low (20%)  
**Impact:** Medium (1 week delay to create fallback)  
**Status:** 🟢 MONITORING

**Description:**
VHS post-processing filter may tank framerate on target hardware (GTX 1060).

**Triggers:**
- Week 3 profile shows <45 FPS with filter enabled
- Mobile devices drop below 30 FPS

**Mitigation:**
- ✅ Week 3 performance testing planned (MASTER_PRODUCTION_SCHEDULE.md)
- ✅ Fallback shader ready (simpler scanlines-only effect)
- 🔄 Weekly profiling ritual (Fridays)

**Contingency Plan:**
1. Disable chromatic aberration (most expensive effect)
2. Reduce scanline resolution
3. Use fallback shader (scanlines + vignette only)
4. Worst case: Remove filter, embrace pure low-poly aesthetic

**Owner:** Anderson  
**Next Review:** Week 3 (Nov 18-24) - Profile with filter

---

### RISK-004: Boss Fight Takes Longer Than Estimated
**Category:** Schedule  
**Probability:** Medium (40%)  
**Impact:** Medium (1-2 weeks delay)  
**Status:** ⚠️ MITIGATED (2-phase boss instead of 3)

**Description:**
Week 13 allocates 30 hours for boss. Multi-phase bosses often take 2x longer.

**Triggers:**
- Phase transitions buggy
- Shield generators don't destroy properly
- Boss AI more complex than expected
- Balancing takes multiple iterations

**Mitigation:**
- ✅ Boss simplified to 2 phases (3rd phase = Content Update 2)
- ✅ 2-week buffer available if needed
- 🔄 Week 10 checkpoint validates basic enemy AI before boss work

**Contingency Plan:**
1. Use 2-week buffer for boss completion
2. Simplify to 1-phase boss (just spawns drones + attacks)
3. Cut boss entirely, replace with "Super Wave" (50+ enemies)

**Owner:** Anderson  
**Next Review:** Week 13 (Jan 27 - Feb 2) - Boss implementation week

---

### RISK-005: Mobile Control Scheme Fails Playtesting
**Category:** Design  
**Probability:** Medium (30%)  
**Impact:** Medium (1 week to redesign + retest)  
**Status:** 🟢 MONITORING

**Description:**
Virtual joystick feels bad, buttons in wrong places, or auto-aim too aggressive.

**Triggers:**
- Week 2 prototype playtest feedback: "This is frustrating"
- Week 9 mobile build: Players can't dodge effectively
- High death rate compared to PC players

**Mitigation:**
- ✅ Mobile Controls Design Doc created (MOBILE_CONTROLS_DESIGN.md)
- ✅ Week 2 dedicated to control prototyping
- ✅ 3 playtesters lined up for Week 2 feedback
- 🔄 Alternative control scheme documented (tap-to-move)

**Contingency Plan:**
1. Switch to Scheme B (tap-to-move)
2. Increase auto-aim assist (soft → hard lock-on)
3. Add "Easy Mode" with stronger auto-aim for mobile
4. Worst case: PC-only launch, mobile delayed to Content Update 4

**Owner:** Anderson  
**Next Review:** Week 2 (Nov 11-17) - Control prototype playtest

---

### RISK-006: Save System Corruption or Loss
**Category:** Technical  
**Probability:** Low (15%)  
**Impact:** High (player rage, negative reviews, 1+ week to fix)  
**Status:** 🟢 MONITORING

**Description:**
Save system bug causes players to lose 10+ hours of meta-progression.

**Triggers:**
- Game crashes during save write
- Cloud sync conflict (playing on 2 devices)
- Unity build changes PlayerPrefs path
- File system error (out of disk space)

**Mitigation:**
- ✅ Save system robustness planned (Week 11)
- ✅ Save versioning for future updates
- ✅ Checksum validation to detect corruption
- 🔄 Multiple save slots (auto-backup)

**Warning Signs:**
- Players report "progress reset" in Discord/reviews
- Save file size = 0 bytes (corrupted write)

**Contingency Plan:**
1. Hotfix patch within 24 hours
2. Implement cloud save recovery tool
3. Offer "Skip to Wave 10" cheat code as compensation
4. Public apology + free Content Update 1

**Owner:** Anderson  
**Next Review:** Week 11 (Jan 20-26) - Save system testing

---

## 🟡 MEDIUM RISKS (Lower Probability or Impact)

### RISK-007: Unity LTS Breaking Change
**Category:** Technical  
**Probability:** Low (10%)  
**Impact:** Medium (1 week to update or rollback)  
**Status:** 🟢 MONITORING

**Description:**
Unity 2022.3 LTS update introduces breaking API change or critical bug.

**Mitigation:**
- ✅ Pin Unity version (2022.3.12f1 - current)
- ✅ Test updates in separate branch before merging
- ✅ Git allows rollback to previous Unity version

**Contingency Plan:**
- Stay on current version until post-launch
- Report bug to Unity, wait for hotfix

**Owner:** Anderson  
**Next Review:** Monthly (check Unity release notes)

---

### RISK-008: Art Takes 7 Weeks Instead of 6
**Category:** Schedule  
**Probability:** Medium (30%)  
**Impact:** Low-Medium (1 week delay, covered by buffer)  
**Status:** 🟢 MONITORING

**Description:**
Week 1-6 art phase runs over schedule due to modeling complexity or iteration.

**Triggers:**
- Week 6 checkpoint: Not all models complete
- VHS filter requires extra polish (looks unfinished)
- Hangar scene more complex than expected

**Mitigation:**
- ✅ Week 6 checkpoint for go/no-go decision
- ✅ 2-week buffer absorbs 1-week overrun
- 🔄 Weekly progress tracking (Sunday check-ins)

**Contingency Plan:**
1. Use 1 week of buffer (still have Week 19)
2. Cut lowest-priority models (Hangar props, environmental debris)
3. Simplify VHS filter to fallback shader

**Owner:** Anderson  
**Next Review:** Week 6 (Dec 15) - Art Complete checkpoint

---

### RISK-009: Marketing Bandwidth Overload (Week 15-17)
**Category:** Schedule  
**Probability:** Medium (40%)  
**Impact:** Low (DevLogs delayed, but game still launches)  
**Status:** ⚠️ MITIGATED (pre-record voiceovers)

**Description:**
Week 15-17 requires: DevLogs, trailer, Steam page, builds, bug fixes. Too much.

**Triggers:**
- DevLog 1 recording takes 2 days instead of 1
- Trailer editing requires re-shoots
- Critical bug found during Week 16, needs immediate fix

**Mitigation:**
- ✅ Pre-record DevLog voiceovers in Week 10-11
- ✅ Week 15-17: Just edit footage (much faster)
- ✅ 2-week buffer provides extra time if needed

**Contingency Plan:**
1. Delay DevLog 2 to Week 18 (post-launch)
2. Use AI voiceover (ElevenLabs) if voice recording fails
3. Simplify trailer (60 seconds instead of 90)

**Owner:** Anderson  
**Next Review:** Week 14 (Feb 9) - Launch prep planning

---

### RISK-010: Playtesting Reveals "Not Fun" Core Loop
**Category:** Design  
**Probability:** Low (15%)  
**Impact:** High (2-4 weeks to redesign or pivot)  
**Status:** 🟢 MONITORING

**Description:**
Week 10 "First Playable" checkpoint: Gameplay feels tedious/boring.

**Triggers:**
- Playtesters: "It's okay, I guess"
- High death rate but no "one more run" urge
- Movement feels sluggish or combat unsatisfying

**Mitigation:**
- ✅ Week 10 checkpoint specifically validates fun factor
- ✅ Vampire Survivors formula is proven (low risk)
- 🔄 Early prototype (Week 7-9) catches issues early

**Contingency Plan:**
1. Increase movement speed 20-30%
2. Add screen shake + particle effects (juice up combat)
3. Adjust enemy spawn rates (faster ramp-up)
4. Worst case: Pivot to simpler arena survival (no meta-progression)

**Owner:** Anderson  
**Next Review:** Week 10 (Jan 13-19) - First Playable checkpoint

---

### RISK-011: Solo Developer Burnout
**Category:** Human  
**Probability:** Medium (35%)  
**Impact:** Medium (1-2 weeks lost productivity)  
**Status:** 🟢 MONITORING

**Description:**
19 weeks at 45 hours/week = 855 hours. Risk of mental/physical exhaustion.

**Triggers:**
- Working 60+ hour weeks consistently
- Skipping weekends for weeks 12-14 (crunch)
- Feeling "I don't want to open Unity today"

**Mitigation:**
- ✅ Realistic 40-50 hour/week schedule (not 80-hour crunch)
- ✅ 2-week buffer reduces pressure
- 🔄 Weekly self-care check during Sunday reviews

**Warning Signs:**
- Missing daily workout/exercise
- Eating poorly (fast food, skipping meals)
- Sleep <7 hours/night for multiple days
- "I hate this project" thoughts

**Contingency Plan:**
1. Take 3-day weekend (use buffer time)
2. Reduce hours to 30/week for 1 week (recovery)
3. Cut scope further (2 arenas, no boss)
4. Delay launch by 1-2 weeks (health > deadlines)

**Owner:** Anderson  
**Next Review:** Weekly (Sunday check-ins include burnout assessment)

---

## 🟢 LOW RISKS (Low Probability and/or Low Impact)

### RISK-012: Steam/App Store Rejection
**Category:** Launch  
**Probability:** Low (10%)  
**Impact:** Low (1-3 days delay to fix issues)  
**Status:** 🟢 MONITORING

**Description:**
Platform rejects build due to policy violation, bugs, or metadata issues.

**Mitigation:**
- ✅ Follow platform guidelines (Steam, iOS, Android)
- ✅ Week 16 test builds catch issues early
- ✅ No controversial content (aliens vs humans is safe)

**Contingency Plan:**
- Fix issues within 24-48 hours
- Resubmit build

---

### RISK-013: Third-Party Asset License Issues
**Category:** Legal  
**Probability:** Very Low (5%)  
**Impact:** Low (replace asset, <1 day)  
**Status:** 🟢 MONITORING

**Description:**
Using asset (font, sound effect) without proper license.

**Mitigation:**
- ✅ All assets created in-house OR properly licensed
- ✅ Document asset sources in Git commit

**Contingency Plan:**
- Replace asset with alternative
- Purchase proper license

---

## 📅 Risk Review Calendar

| Week | Key Risks to Review | Checkpoint |
|------|---------------------|------------|
| Week 2 | RISK-005 (mobile controls) | Control prototype playtest |
| Week 3 | RISK-003 (VHS filter performance) | Profile with filter |
| Week 6 | RISK-008 (art overrun) | Art Complete checkpoint |
| Week 10 | RISK-010 (fun factor), RISK-002 (scope creep) | First Playable checkpoint |
| Week 11 | RISK-006 (save system) | Save system testing |
| Week 13 | RISK-004 (boss complexity) | Boss implementation |
| Week 14 | RISK-009 (marketing bandwidth) | Launch prep planning |
| Week 17 | ALL RISKS | Final go/no-go decision |

---

## Risk Mitigation Success Metrics

**Green Light (Low Risk):**
- 90%+ of tasks completed on time
- No critical bugs in backlog
- Playtesting feedback: "This is fun!"
- Performance: 60 FPS stable on target hardware

**Yellow Light (Medium Risk):**
- 80-89% of tasks completed on time
- 1-2 critical bugs in backlog
- Playtesting feedback: "It's okay"
- Performance: 45-59 FPS on target hardware

**Red Light (High Risk):**
- <80% of tasks completed on time
- 3+ critical bugs in backlog
- Playtesting feedback: "Not fun" or "Frustrating"
- Performance: <45 FPS on target hardware

**Action:** Red Light at any checkpoint = immediate mitigation planning (scope cuts, timeline extension, or pivot).

---

## Risk Register Maintenance

**Weekly Updates (Sundays):**
1. Review all risks (5 minutes)
2. Update probability/impact if situation changed
3. Add new risks if identified
4. Close resolved risks (mark with ✅ RESOLVED)

**Checkpoint Reviews:**
- Week 6, 10, 14, 17: Deep dive on all risks
- Document decisions in Git commit

**Risk Log Template:**
```markdown
### RISK-XXX: [Risk Title]
**Date Added:** YYYY-MM-DD
**Category:** Schedule / Technical / Design / Legal / Human
**Probability:** Low / Medium / High
**Impact:** Low / Medium / High
**Status:** 🟢 Monitoring / ⚠️ Mitigated / 🚨 Active / ✅ Resolved

**Description:** [What could go wrong?]

**Triggers:** [What signals this risk is happening?]

**Mitigation:** [What are we doing to prevent it?]

**Contingency Plan:** [What if it happens anyway?]

**Owner:** [Who is responsible?]
**Next Review:** [When to check on this again?]
```

---

**Last Updated:** November 3, 2025  
**Next Review:** November 10, 2025 (Week 2 Sunday check-in)  
**Owner:** Anderson Gonçalves

