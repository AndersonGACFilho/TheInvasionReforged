 isink it# Devlog #01 — Timed VO Script & Cut Plan

Title: Devlog #01 — Zero-G Movement & Auto-Fire — The Invasion Reforged
Length target: 6:00 - 7:30 (aim ~6:30)
Format: 16:9 (1080p) long-form devlog. Extract shorts from marked timestamps.

Placeholders: replace <STEAM_WISHLIST>, <DISCORD_INVITE>, <FULL_DEVLOG_URL> before publishing.

---

0:00–0:08 Hook (8s)
- Visual: Fast montage: before/after movement clip (0.5s each), close call with Raven-IX, scrap pickup, short UI level-up flash.
- VO script: "Zero‑G movement is live — and it finally feels right." (energetic)
- Text overlay: "Devlog #01 — Zero‑G Movement & Auto‑Fire"

0:08–0:25 Intro (17s)
- Visual: webcam face (optional) + game logo card
- VO: "Hi, I'm [Your Name], solo dev of The Invasion Reforged. Welcome to Devlog #01. Today I'll show how I tuned ship movement and implemented the auto-fire weapons." (friendly, concise)
- Lower-third: "[Your Name] — Solo Dev"

0:25–0:45 Goal & Context (20s)
- Visual: cut to gameplay slow pan
- VO: "Goal for this session: add inertia for a floaty-but-responsive feel, and get the automatic weapons firing in a way that rewards positioning and upgrades." (clear)
- On-screen bullets (short): "Goal: Inertia + Responsive Dash, Auto-fire weapons"

0:45–2:10 Movement Deep Dive (1:25)
- Visual: Show editor with movement parameters (acceleration curve, damping, top speed). Show pseudo-code snippet (highlighted) for 8–12s. Cut to gameplay showing drifting and corrections.
- VO (scripted):
  - "I modeled movement as a dampened velocity system. Instead of directly setting velocity, I apply acceleration toward the input direction and dampen existing velocity each frame. Here's the core idea:"
  - (Show pseudo-code overlay)
  - "Key tuning knobs: acceleration curve, damping factor, and top speed clamp. I used a non-linear acceleration curve (ease-out) so small inputs feel snappy but sustained input reaches top speed smoothly."
- B-roll notes: show before/after clips (split-screen) for 6–8s to emphasize change.
- On-screen callouts showing exact parameter values used in the build (example): Accel=14, Damping=0.92, TopSpeed=9, DashCooldown=6s, DashDistance=12.

2:10–2:45 Dash Ability Explanation (35s)
- Visual: gameplay clip showing dash escape vs. enemy; UI cooldown overlay animation
- VO: "To complement inertia, I added a short dash with cooldown. This preserves risk—you can't spam the dash—while giving players a clutch option to avoid kamikaze interceptors." (practical tone)
- Show small code snippet for dash activation and cooldown logic (2–3 lines) as overlay.

2:45–3:45 Auto-Fire Weapons (1:00)
- Visual: show WeaponSystem inspector (or script brief) then gameplay showing auto-fire behavior and different weapon pickups.
- VO:
  - "Weapons fire automatically based on your current loadout and level. The system chooses targets within a cone and fires according to weapon-specific rates."
  - "I built the system to be data-driven: new weapons are ScriptableObjects (or JSON configs) defining fire rate, cone angle, projectile type, and scaling with level."
- Visual callouts: show Pulse Beam, Orbital Sentinel, Ion Burst names with short clips.

3:45–4:10 Upgrade Interaction (25s)
- Visual: level-up selection pop-up during run, show choosing an upgrade and immediate effect.
- VO: "When you level up, temporary run upgrades can change how your auto-weapons behave—like adding homing or creating an orbiter. These temporary changes stack with permanent Hangar upgrades." (concise)

4:10–4:40 Problem & Fixes (30s)
- Visual: show older footage where movement felt sluggish or weapons overpowered; add captions: "Problem: Sluggish feel" / "Problem: Overpowered Ion Burst".
- VO: "Initial tuning made the ship feel sluggish. I increased responsiveness by adjusting the acceleration curve and added a dash to improve player agency. For weapons, I adjusted projectile spread and added level-scaling to avoid huge spike moments early." (honest, quick)

4:40–5:10 Live Playthrough Clip (30s)
- Visual: uninterrupted gameplay showcasing movement, dash, auto-fire, and scrap pickup — aim for an exciting sequence with a near-miss and a reward.
- VO: light commentary or leave music + SFX; consider short live VO like: "Here’s a run where those small corrections matter—watch the Raven-IX approach and the dash to escape." (optional)

5:10–5:40 Next Steps & Roadmap (30s)
- Visual: show GDD hangar screen mock + bullets for next tasks
- VO: "Next: enemy AI for Raven-IX, level spawn tuning, and the Hangar with permanent upgrades. Devlog #02 will focus on enemy AI and spawn design." (forward-looking)
- On-screen CTA bullets: "Wishlist on Steam — <STEAM_WISHLIST>" "Join Discord — <DISCORD_INVITE>"

5:40–6:00 End & CTA (20s)
- Visual: end slate with social links, Subscribe CTA, Steam wishlist link in description shown as text
- VO: "If you liked this, wishlist The Invasion Reforged and subscribe for weekly devlogs. Full devlog and code notes in the description. Thanks for watching!" (warm)

---

EXTRACTABLE SHORTS (timestamps to clip)
- Short A (15s teaser): 0:00–0:15 (hook + intro montage)
- Short B (30s tip): 0:45–1:15 (movement deep dive condensed) or 1:00–1:30 for before/after split
- Short C (45s BTS): 4:10–4:55 (problem & fixes + teachable moment)

---

B-ROLL & ASSETS CHECKLIST
- Gameplay captures: 1080p/60fps or 30fps depending on hardware
- Editor capture: record movement parameter tweaking (no UI clutter)
- Webcam: 720p optional, well-lit, neutral background
- Timelapse render: export a fast timelapse of tuning session for B-roll
- Music: royalty-free track with stems (lower for VO)
- SFX: scrap ping, level-up pulse, dash whoosh

---

YOUTUBE DESCRIPTION (ready-to-paste)
Devlog #01 — Zero-G Movement & Auto-Fire

In this devlog I implement inertia-based ship movement and the automatic weapon system. Watch gameplay, code notes, and tuning decisions.

Full devlog assets & code notes: <FULL_DEVLOG_URL>
Wishlist The Invasion Reforged: <STEAM_WISHLIST>
Join our Discord: <DISCORD_INVITE>

Chapters:
0:00 Hook
0:08 Intro
0:25 Goal & Context
0:45 Movement Deep Dive
2:10 Dash Ability
2:45 Auto-Fire Weapons
4:10 Problems & Fixes
4:40 Live Playthrough
5:10 Next Steps
5:40 End & CTA

Subscribe for weekly devlogs — new videos every Tuesday.

Music: [credits]
Assets: [credits]

---

LINKEDIN POST COPY (short native upload)
Devlog #01 — Zero-G Movement & Auto-Fire (Short)

Implemented a dampened velocity model to get zero-G movement that feels "floaty but responsive." Also added a short dash to give players a clutch option.

Why it matters: small tweaks in acceleration and damping completely change the player's sense of control. If you tune these wrong, the game feels sluggish or twitchy.

Full devlog & notes: <FULL_DEVLOG_URL>
Wishlist: <STEAM_WISHLIST>
#GameDev #IndieDev #MadeWithUnity

---

ASSET NAMES FOR EDITOR
- Devlog01_Gameplay.mp4
- Devlog01_EditorCapture.mp4
- Devlog01_Webcam.mp4
- Devlog01_Timelapse.mp4
- Devlog01_Music.mp3
- Devlog01_SFX_Whoosh.wav

---

NOTES
- Keep VO pacing natural; pause slightly between sentences for editing cuts.
- Keep captions burned-in for shorts and auto-generated subs for full YouTube (then clean up).
- Replace placeholders with actual links before publishing.

---

File created by automation on behalf of the dev. Edit names, values, and links as required before recording or publishing.
