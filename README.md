# Project: The Invasion: Reforged

A 2D retro space shooter built in Unity, combining the addictive, wave-based survival of *Vampire Survivors* with classic top-down arcade action.

## About The Project

This project is a complete remake and reimagining of a game I first developed during my first semester of college. The goal is to apply modern software design principles and the full power of the Unity engine to create a polished, scalable, and fun roguelite experience.

In **The Invasion: Reforged**, you flip the script: you are the lone alien survivor, fending off endless waves of aggressive human fleets. By collecting their scrap, you can upgrade your ship with a vast array of alien technology, evolving with every run to become an unstoppable force.

-----

## Core Gameplay Mechanics

The gameplay loop is simple: **Survive. Evolve. Annihilate.** Collect experience from fallen enemies to choose from a random selection of upgrades, creating a unique build in every run.

### Special Crate Drops

Beyond standard resources, elite enemies have a chance to drop **special supply crates**. These crates contain rare and powerful **Alien Artifacts**. Finding an artifact grants a significant, permanent buff that **lasts for the rest of the current run**, fundamentally altering your strategy and power level. The player's **Luck** attribute directly influences the drop rate of these game-changing items.

#### Alien Artifact Examples

* **"Mega bomb Core":** Does not explode, but instead integrates into your ship. Once per level-up, you now also trigger a small explosion around your ship.
* **"Overcharged Hull":** Your ship's hull becomes unstable but powerful. You gain a massive permanent boost to damage and fire rate, but your max health is reduced.
* **"Quantum Thrusters":** A permanent upgrade to your evasion systems, granting you the ability to briefly phase through enemies and projectiles after taking damage.

-----

### Player Attributes

* **Thrusters Potency:** Base movement speed.
* **Hull Integrity:** Your ship's health.
* **Deflection Matrix:** A regenerating shield that absorbs damage.
* **Evasion Thrusters:** Chance to evade incoming projectiles.
* **Nanobots:** Passive health regeneration.

### Offensive Upgrades

* **Cooldown Reduction:** Fire your weapons faster.
* **Energy Amplifier:** Increases all outgoing damage.
* **Projectile Velocity:** Your projectiles travel faster.
* **Blast Radius:** Increases the area of effect for explosive weapons.
* **Multishot:** Fire additional projectiles with each attack.
* **Effect Duration:** Increases the duration of status effects or lingering attacks.

### Utility Upgrades

* **Tractor Beam Range:** Automatically collect pickups from a wider radius.
* **Data Analysis:** Gain more experience from each pickup.
* **Luck:** Increases your chances of getting rare upgrades and powerful artifact drops.
* **Scrap Multiplier:** Gain more currency during a run.
* **Tech Diagram:** Gain more permanent currency for meta-progression.

-----

## Architectural Goals & SOLID Principles

A primary goal of this project is to build a robust and maintainable codebase by adhering to SOLID principles.

### S - Single Responsibility Principle (SRP)

Components are designed to have one job. For example, `PlayerInputReader` only reads input, while `MovementHandler` only applies movement.

### O - Open/Closed Principle (OCP)

The system is **open for extension, but closed for modification**. This is heavily achieved using Scriptable Objects for weapons. Adding a new weapon never requires modifying the `PlayerAttackHandler`.

### L - Liskov Substitution Principle (LSP)

This is achieved by using interfaces like `IDamageable`. Projectiles can damage any object that implements this interface, whether it's a player, an enemy, or a destructible asteroid.

### I - Interface Segregation Principle (ISP)

Entities only implement the behaviors they need through small, specific interfaces (`IDamageable`, `IMovable`, etc.).

### D - Dependency Inversion Principle (DIP)

High-level modules do not depend on low-level ones. Game logic (`PlayerLifeManager`) is completely decoupled from presentation layers (`UIManager`) through the use of C\# events.

-----

## Getting Started

To run this project locally:

1.  Clone the repository:
    ```sh
    git clone https://github.com/AndersonGACFilho/TheInvasionReforged
    ```
2.  Open the project in Unity Hub using **Unity Editor version 6000.2.8f1**.
3.  Load the main scene located at `Assets/Scenes/Main.unity`.
4.  Press the Play button.

-----

## Roadmap

* [x] Core Player Movement & Input
* [x] Basic Weapon System using Scriptable Objects
* [ ] Experience & Level-Up System
* [ ] UI for Health, Shield, and Experience
* [ ] Wave Spawning Logic
* [ ] Alien Artifact System (run-long passive items)
* [ ] Add 3 unique enemy types
* [ ] Implement 10 distinct player upgrades
* [ ] Main Menu & Game Over Screen

-----

## Contact

Anderson G. A. C. Filho - [https://www.linkedin.com/in/agacf](https://www.linkedin.com/in/agacf) - andersonfilho09@gmail.com

Project Link: [https://github.com/AndersonGACFilho/TheInvasionReforged](https://github.com/AndersonGACFilho/TheInvasionReforged)