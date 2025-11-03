# Launch Plan - Early Access

**Updated:** November 3, 2025  
**Strategy:** Early Access → Content Updates → Full Release

---

## Early Access Overview

### EA v0.8 (Week 14 - Feb 16, 2026)
**What's In:**
- Complete core gameplay (movement, combat, progression)
- 3 enemy types (Falcon, Sentry, Atlas)
- Prometheus boss with 2-phase fight
- 3 planetary arenas (Aureon Prime, Cryovex, Voltra-9)
- Full meta-progression (Alien Hangar)
- 6 permanent upgrade categories
- Quantum Entropy Matrix
- 8-10 temporary in-run upgrades
- Save/load with cloud sync
- Polish: VFX, SFX, UI, particles
- PC build (Steam - $7.99)
- Mobile builds (iOS/Android - $4.99)
- Premium pricing (no ads, no IAP)

**Coming in Updates (Free):**
- Update 1: +2 arenas (Weeks 15-16)
- Update 2: Boss 3rd phase + new enemies (Weeks 17-19)
- Update 3: Leaderboards + endless mode (Weeks 20-24)

**Full Release v1.0 (Week 25+ - May 2026):**
- Exit Early Access
- Price increase to $9.99 (PC)
- Full release marketing push

---

## Why Leaderboards Are Post-Launch

### Technical Complexity
Leaderboards require significant infrastructure that would delay the entire project:

| Component | Complexity | Time Required |
|-----------|------------|---------------|
| Backend API (Firebase/PlayFab) | High | 30-50 hours |
| Anti-cheat system | Very High | 20-30 hours |
| Cross-platform integration | High | 15-25 hours |
| Security auditing | High | 10-15 hours |
| Load testing at scale | Medium | 10-15 hours |
| Challenge generation system | Medium | 15-20 hours |
| Friends list integration | Medium | 10-15 hours |
| **TOTAL** | | **110-180 hours (3-4 weeks)** |

### Development Philosophy
**Focus on Core First:**
- Players need a polished, stable core experience at launch
- Leaderboards are enhancement, not requirement for fun
- Single-player roguelite is complete without competitive features
- Meta-progression (Hangar upgrades) provides retention at launch

**Risk Mitigation:**
- Launching with untested backend infrastructure = high risk of crashes
- Cannot properly load test until live player base exists
- Security vulnerabilities in leaderboards could compromise entire game
- Bugs in leaderboard system would overshadow positive launch reception

**Community Building:**
- Leaderboards are more meaningful with established player community
- Launching with 100 players competing is less engaging than 5,000+
- Post-launch "big update" generates press coverage and sales bump
- Builds anticipation and gives players something to look forward to

### Business Benefits
**Launch Clean:**
- Reviewers focus on core gameplay, not buggy leaderboards
- Simpler support burden (no backend issues to troubleshoot)
- Faster bug fixes (fewer systems to maintain)
- Better first impression

**Post-Launch Engagement:**
- "Major Free Update" marketing opportunity
- Re-engages lapsed players
- Attracts new players with "now has leaderboards" messaging
- Demonstrates ongoing development commitment

---

## Early Access Content Timeline

### Week 14: EARLY ACCESS LAUNCH (Feb 16, 2026)
- Game goes live on Steam ($7.99 EA price), iOS App Store, Google Play ($4.99)
- Monitor for critical bugs and hotfix as needed
- Begin community building (Discord, subreddit, social media)
- Track analytics: retention, session length, progression rates
- Exclusive "Vanguard" ship skin for EA buyers

### Weeks 15-16: Content Update 1 - Arena Expansion (Mar 2, 2026)
**Release Target:** 2 weeks post-EA launch  
**Type:** FREE UPDATE

**What's Included:**
- 2 new planetary arenas (Zerion Nexus, Ethereal Void)
- Aegis Node stationary turret enemy
- Arena-specific enemy variants
- Balance adjustments based on player feedback

**Why This First:**
- Relatively simple to implement (reuses existing systems)
- Shows commitment to ongoing updates
- Adds content variety without technical risk
- Can be developed while monitoring EA stability

**Estimated Time:** 30-50 hours

---

### Weeks 17-19: Content Update 2 - Boss Enhancement (Mar 23, 2026)
**Release Target:** 5 weeks post-EA launch  
**Type:** FREE UPDATE

**What's Included:**
- Prometheus boss 3rd phase (devastating beam weapons)
- Raven-IX kamikaze interceptor enemy
- 2+ additional enemy variants (Falcon MK II, Atlas Heavy)
- Additional permanent upgrades (2-3 new categories)
- Balance pass for boss difficulty

**Why This Second:**
- Adds significant replayability
- Pure client-side (no backend needed)
- Builds excitement for Update 3 (competitive features)

**Estimated Time:** 40-60 hours

---

### Weeks 20-24: Content Update 3 - Leaderboards & Endless Mode (Apr 27, 2026)
**Release Target:** 10-11 weeks post-EA launch  
**Type:** FREE UPDATE (Major)  
**Priority:** HIGH

**What's Included:**
- **Global Leaderboards:**
  - Survival Time
  - Enemies Defeated
  - Scrap Collected
  - Boss Kill Time
  - Longest Streak

- **Challenge Leaderboards:**
  - Daily Challenge (24h fixed seed)
  - Weekly Challenge (7-day modifiers)

- **Social Features:**
  - Friends leaderboards
  - Beat notifications
  - Platform integration (Steam, Game Center, Google Play)

- **Endless Mode:**
  - Unlimited waves
  - Escalating difficulty
  - Dedicated endless leaderboard

- **Rewards:**
  - Cosmetic ship skins for top ranks
  - Exclusive daily skins
  - Hall of Champions emblem

**Why This Third:**
- Player base is established (hundreds/thousands of players)
- Backend can be properly scaled for known user count
- Security can be tested with real player behavior patterns
- Community is engaged and excited for competitive features
- Game stability is proven

**Estimated Time:** 110-180 hours (2.5-4 weeks)

**See detailed documentation:**
- `GDD.md` - Section 13
- `Leaderboard/LEADERBOARD_QUICK_REF.md`
- `Leaderboard/LEADERBOARD_ARCHITECTURE_DIAGRAM.md`
- `Leaderboard/LEADERBOARD_IMPLEMENTATION_SUMMARY.md`

---

### Week 25+: FULL RELEASE v1.0 (May 4, 2026)
**Release Target:** 12 weeks post-EA launch  
**Type:** EXIT EARLY ACCESS

**What's Included:**
- All content complete (5 arenas, 3-phase boss, leaderboards, endless)
- Early Access status removed on all platforms
- PC price increase: $7.99 → $9.99
- Mobile price remains: $4.99
- "Full Release" marketing campaign
- Press release and media outreach
- Potential console port exploration

**Success Metrics:**
- 2,000+ total copies sold (EA + Full)
- 80%+ positive reviews
- Active player base for post-v1.0 support

---

## Early Access Launch Week Priorities (Week 14)

### Pre-Launch Checklist (Week 13)
- [ ] Final QA pass - no critical bugs
- [ ] Steam Early Access page live (with roadmap)
- [ ] App Store and Google Play submissions approved
- [ ] Press kit distributed to indie game reviewers
- [ ] Social media accounts active (Twitter, Discord)
- [ ] Discord server set up with channels
- [ ] Support email configured
- [ ] Analytics integrated (Unity Analytics / Firebase)
- [ ] Crash reporting configured (Sentry, Crashlytics)
- [ ] Early Access trailer public
- [ ] DevLog 1 published: "Why Early Access?"

### Launch Day (Feb 16, 2026)
- [ ] Builds live on Steam ($7.99), iOS ($4.99), Android ($4.99)
- [ ] Trailer public and promoted across all channels
- [ ] Steam Early Access roadmap visible on store page
- [ ] Discord announcement with welcome message
- [ ] Monitor crash reports (hourly for first 24h)
- [ ] Monitor player feedback (Discord, reviews, forums)
- [ ] Track key metrics: DAU, session length, retention
- [ ] Engage with community (respond to questions)
- [ ] Respond to early reviews
- [ ] Prepare hotfix if critical issues found

### Week 1 Post-Launch (Feb 16-23)
- [ ] Daily community check-ins (Discord, Steam forums)
- [ ] Track analytics: Which arena is most popular?
- [ ] Note balance issues: Is boss too hard/easy?
- [ ] Begin planning Update 1 based on feedback
- [ ] Hotfix critical bugs within 24-48 hours
- [ ] Thank early supporters (social posts, Discord shoutouts)

### Week 1 Post-Launch
- [ ] Daily community updates
- [ ] Hotfix critical bugs (if any)
- [ ] Begin Content Update 1 development
- [ ] Analyze player behavior and balance
- [ ] Gather feedback for future updates
- [ ] Monitor sales and adjust marketing if needed

---

## Success Metrics

### Launch Week (Week 12)
- **Sales:** 1,000+ copies (PC + Mobile combined)
- **Retention:** 40%+ day-1 to day-2
- **Crash Rate:** <1% of sessions
- **Rating:** 4.0+ stars (mobile), "Positive" or better (Steam)
- **Session Length:** 30+ minutes average

### Month 1 (Weeks 12-15)
- **Sales:** 5,000+ copies
- **DAU/MAU:** 25%+ (daily active / monthly active)
- **Reviews:** 100+ total, 80%+ positive
- **Community:** 500+ Discord members

### Quarter 1 (Weeks 12-24)
- **Sales:** 15,000+ copies
- **Content Updates Delivered:** All 3 updates live
- **Leaderboard Participation:** 35%+ of active players
- **Review Score:** "Very Positive" on Steam, 4.5+ mobile

---

## Marketing Timeline

### Pre-Launch (Weeks 10-12)
- Gameplay trailer release
- GIF marketing on Twitter/Reddit
- Press outreach to indie game sites
- Wishlist campaign on Steam

### Launch (Week 12)
- Launch announcement on all channels
- Engage with reviewers and streamers
- Post to r/roguelites, r/iosgaming, r/AndroidGaming
- Monitor and respond to all social media mentions

### Post-Launch Updates (Weeks 13, 15, 17)
- "Free Update Available" announcements
- Update trailers showcasing new content
- Re-engage lapsed players via email/push
- Generate press coverage for major updates (Update 3)

---

## Contingency Plans

### If Launch Goes Poorly (<500 sales Week 1)
- **Marketing Pivot:** Increase social media presence, reach out to streamers
- **Price Adjustment:** Consider launch week discount
- **Content Acceleration:** Release Update 1 earlier (Week 13 instead of 14)
- **Community Focus:** Double down on Discord engagement

### If Critical Bug Found at Launch
- **Hotfix Priority:** Drop everything, fix immediately
- **Communication:** Be transparent with community about issue and ETA
- **Compensation:** Consider temporary discount or bonus content for early adopters
- **Post-Mortem:** Document what went wrong and update QA process

### If Leaderboard Development Overruns
- **Phase Release:** Launch global leaderboards first, challenges later
- **MVP Approach:** Launch with fewer categories initially
- **Communication:** Set clear expectations with community about timeline
- **Alternative:** Use platform-native leaderboards (Steam) as interim solution

---

## Key Takeaways

1. **Launch is Week 12** - Core game only, no leaderboards
2. **Leaderboards are Week 17-19** - Post-launch, after stability proven
3. **Focus on Quality** - Better to launch polished core than rush complex systems
4. **Community First** - Build player base before adding competitive features
5. **Free Updates** - All post-launch content is free, no DLC paywalls
6. **Iterative Approach** - Learn from each update before adding complexity

---

**Questions about launch plan? See:**
- `ROADMAP_12W.md` - Detailed week-by-week breakdown
- `GDD.md` - Full game design document
- `Leaderboard/` folder - All leaderboard documentation

**Status:** ✅ All documentation updated to reflect post-launch leaderboard strategy

