# Devlog #01 — VO Recording Script (English)

Use this file as a line-by-line script for recording your English voice-over. Each section is short for easy recording and editing.

0:00–0:08 Hook (8s)
"Zero‑G movement is live — and it finally feels right."

0:08–0:25 Intro (17s)
"Hi, I'm [Your Name], solo dev of The Invasion Reforged. Welcome to Devlog number one. Today I'm tuning ship movement and the auto-fire weapons."

0:25–0:45 Goal & Context (20s)
"Goal for this session: add inertia for a floaty-but-responsive feel, and get the automatic weapons firing in a way that rewards positioning and upgrades."

0:45–2:10 Movement Deep Dive (record as short lines)
"I modeled movement as a dampened velocity system."
"Instead of directly setting velocity, the ship accelerates toward input and existing velocity is dampened each frame."
"Key tuning knobs are: the acceleration curve, the damping factor, and a top speed clamp."
"I used a non-linear acceleration curve so small inputs feel snappy while sustained input reaches top speed smoothly."

2:10–2:45 Dash Ability Explanation (35s)
"To complement inertia, I added a short dash with a cooldown."
"This preserves risk — you can't spam the dash — while giving players a clutch option to avoid kamikaze interceptors."

2:45–3:45 Auto-Fire Weapons (1:00)
"Weapons fire automatically based on your current loadout and level."
"The system chooses targets within a cone and fires according to weapon-specific rates."
"It's data-driven: new weapons are defined by ScriptableObjects or JSON configs that set fire rate, cone angle, projectile type, and scaling with level."

3:45–4:10 Upgrade Interaction (25s)
"When you level up, temporary run upgrades can change how your auto-weapons behave — for example, adding homing or creating an orbiter."
"These temporary changes stack with permanent Hangar upgrades."

4:10–4:40 Problem & Fixes (30s)
"Initial tuning made the ship feel sluggish."
"I increased responsiveness by adjusting the acceleration curve and added the dash to improve player agency."
"For weapons, I adjusted projectile spread and added level-scaling to avoid huge power spikes early."

4:40–5:10 Live Playthrough Clip (30s)
(Optional live commentary — keep short)
"Here's a run where small corrections matter — watch the Raven-IX approach and the dash that saves the ship."

5:10–5:40 Next Steps & Roadmap (30s)
"Next: enemy AI for Raven-IX, level spawn tuning, and the Hangar with permanent upgrades."
"Devlog number two will focus on enemy AI and spawn design."

5:40–6:00 End & CTA (20s)
"If you liked this, wishlist The Invasion Reforged and subscribe for weekly devlogs."
"Full devlog and code notes are in the description. Thanks for watching!"

---

Recording tips (short)
- Record short lines one at a time and leave 1–2 seconds of silence before and after each line for easy cutting.
- Use consistent mic position, sample at 48 kHz, 24-bit if possible.
- Save raw takes as separate files named: Devlog01_EN_take01.wav

Replace placeholders before publishing: <STEAM_WISHLIST>, <DISCORD_INVITE>, <FULL_DEVLOG_URL>.
