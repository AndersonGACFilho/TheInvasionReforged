# Testing Strategy

**Project:** The Invasion: Reforged  
**Approach:** TDD + Playtesting  
**Coverage:** 90%+ for business logic  
**Updated:** November 3, 2025

---

## Philosophy

### Core Ideas

1. **TDD:** Write tests before code
2. **Pure C#:** Separate from Unity MonoBehaviours for easy testing
3. **Fast:** Tests run in under 5 seconds
4. **Coverage:** 90%+ for core systems
5. **Automated:** CI/CD runs tests every commit

### Testing Pyramid

```
       E2E Tests (10%)
      Full gameplay, platform testing
      
    Integration Tests (20%)
   System interactions, Unity Play Mode
   
  Unit Tests (70%)
 Pure C# logic, fast and isolated
```

---

## Unit Testing

### What Gets 90%+ Coverage

**Pure C# Systems (100% required)**

**Combat System:**
```csharp
// AttackExecutor.cs - Pure C# combat logic
[Test]
public void AttackExecutor_DealsDamageCorrectly()
{
    // Arrange
    var attacker = CreateMockAttacker(damage: 10f);
    var target = CreateMockTarget(health: 100f);
    var executor = new AttackExecutor(attacker);
    
    // Act
    executor.ExecuteAttack(target);
    
    // Assert
    Assert.AreEqual(90f, target.CurrentHealth);
}

[Test]
public void AttackExecutor_RespectsCooldown()
{
    var executor = new AttackExecutor(cooldown: 1f);
    executor.ExecuteAttack(target);
    
    bool canAttackImmediately = executor.CanAttack();
    
    Assert.IsFalse(canAttackImmediately);
}
```

**Stats System:**
```csharp
// StatModifierSystem.cs
[Test]
public void StatModifier_AppliesAdditiveModifiers()
{
    var baseStats = new CharacterStats { Damage = 10f };
    var modifier = new StatModifier { Type = StatType.Damage, Value = 5f };
    
    float result = StatCalculator.ApplyModifiers(baseStats, modifier);
    
    Assert.AreEqual(15f, result);
}

[Test]
public void StatModifier_AppliesMultiplicativeModifiers()
{
    var baseStats = new CharacterStats { Damage = 10f };
    var modifier = new StatModifier { Type = StatType.Damage, Multiplier = 1.5f };
    
    float result = StatCalculator.ApplyModifiers(baseStats, modifier);
    
    Assert.AreEqual(15f, result);
}
```

**Progression System:**
```csharp
// XPSystem.cs
[Test]
public void XPSystem_LevelsUpAtThreshold()
{
    var xpSystem = new XPSystem(levelUpThreshold: 100);
    bool leveledUp = xpSystem.AddExperience(100);
    
    Assert.IsTrue(leveledUp);
    Assert.AreEqual(2, xpSystem.CurrentLevel);
}

[Test]
public void XPSystem_OverflowXPCarriesOver()
{
    var xpSystem = new XPSystem(levelUpThreshold: 100);
    xpSystem.AddExperience(150);
    
    Assert.AreEqual(2, xpSystem.CurrentLevel);
    Assert.AreEqual(50, xpSystem.CurrentXP);
}
```

#### ✅ Strategy Pattern Implementations (80%+ Coverage)

**Movement Strategies:**
```csharp
[Test]
public void MeleeMovementStrategy_StopsAtTargetDistance()
{
    var strategy = ScriptableObject.CreateInstance<MeleeMovementStrategy>();
    strategy.stopDistance = 1f;
    
    var mockController = new MockMovable();
    Vector2 selfPos = Vector2.zero;
    Vector2 targetPos = new Vector2(0.5f, 0); // Within stop distance
    
    strategy.ExecuteMovement(selfPos, targetPos, mockController);
    
    Assert.IsTrue(mockController.StopCalled);
}
```

**Attack Strategies:**
```csharp
[Test]
public void AOEAttackStrategy_HitsMultipleTargets()
{
    var strategy = ScriptableObject.CreateInstance<AOEAttackStrategy>();
    strategy.radius = 5f;
    
    var targets = CreateMockTargetsInRadius(3);
    
    strategy.Execute(attacker, targets);
    
    Assert.AreEqual(3, targets.Count(t => t.WasDamaged));
}
```

#### ✅ AI State Machine (70%+ Coverage)

```csharp
[Test]
public void EnemyAIContext_TransitionsOnHealthThreshold()
{
    var context = new EnemyAIContext();
    context.healthProvider.SetHealth(20); // Low health
    
    var state = new EnemyCombatState();
    state.Enter(context);
    
    // Should transition to flee state
    Assert.IsTrue(context.OnHealthLow.WasInvoked);
}

[Test]
public void ChaseState_StopsAtAttackRange()
{
    var context = CreateMockContext();
    var chaseState = new EnemyChaseState();
    
    context.targetPosition = Vector2.zero;
    context.selfPosition = new Vector2(1f, 0); // Within attack range
    
    chaseState.UpdateLogic(context);
    
    Assert.IsTrue(context.OnTargetInAttackRange.WasInvoked);
}
```

### What Doesn't Need Unit Tests

❌ **Unity MonoBehaviour Lifecycle** (Awake, Start, Update)  
❌ **Inspector-configured values** (test data-driven logic instead)  
❌ **Simple getters/setters** (no business logic)  
❌ **Unity API calls** (Transform.position, etc.)  

**Instead:** Test the LOGIC called by MonoBehaviours, not the MonoBehaviours themselves.

---

## 🔗 Integration Testing Strategy

### What Gets Integration Tested (20% of Test Suite)

#### ✅ Unity Play Mode Tests

**System Integration:**
```csharp
[UnityTest]
public IEnumerator PlayerShip_TakeDamage_UpdatesHealthUI()
{
    // Arrange
    var player = InstantiatePlayer();
    var ui = GameObject.Find("HealthBar").GetComponent<HealthBarUI>();
    
    // Act
    player.GetComponent<IDamageable>().TakeDamage(10f);
    yield return null; // Wait one frame
    
    // Assert
    Assert.AreEqual(90f, ui.DisplayedHealth);
}

[UnityTest]
public IEnumerator EnemySpawner_SpawnsEnemiesOverTime()
{
    var spawner = CreateSpawner();
    spawner.spawnInterval = 1f;
    
    yield return new WaitForSeconds(2.5f);
    
    int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    Assert.AreEqual(2, enemyCount);
}
```

**Combat Integration:**
```csharp
[UnityTest]
public IEnumerator PlayerWeapon_DestroyEnemyOnKill()
{
    var player = InstantiatePlayer();
    var enemy = InstantiateEnemy();
    enemy.GetComponent<CharacterManager>().SetHealth(10f);
    
    // Player deals 10 damage
    player.GetComponent<PlayerCombatManager>().Attack(enemy);
    
    yield return new WaitForSeconds(0.5f);
    
    Assert.IsNull(enemy); // Enemy should be destroyed
}
```

#### ✅ Multi-System Interactions

**Progression → UI:**
- Leveling up shows correct UI animation
- XP bar updates correctly
- Upgrade choices display properly

**AI → Combat → Movement:**
- Enemy chases player
- Enemy stops at attack range
- Enemy attacks with correct cooldown

**Save/Load:**
- Saving progress persists data
- Loading restores correct state
- Cloud sync works correctly

---

## 🎮 Playtesting Strategy

### Playtest Phases

#### **Phase 1: Internal Testing (Weeks 7-12)**

**Who:** You (developer)  
**Frequency:** Daily  
**Focus:** Core mechanics, bug finding

**Testing Checklist (Daily):**
- [ ] Can complete full run (10-15 minutes)
- [ ] No game-breaking bugs
- [ ] Performance acceptable (60 FPS PC)
- [ ] Controls feel responsive

**Data to Collect:**
- Average run length
- Death location (which wave)
- Upgrade choices made
- Player feedback (voice record thoughts while playing)

#### **Phase 2: Friends & Family (Week 13)**

**Who:** 5-10 non-developer gamers  
**Duration:** 2 hours per person  
**Focus:** Usability, fun factor, difficulty

**Test Protocol:**
```
1. Introduction (5 min)
   - No tutorial, just "figure it out"
   - Observe first-time experience

2. Guided Play (30 min)
   - Ask questions while they play
   - Note confusion points
   - Record deaths and upgrades

3. Feedback Interview (10 min)
   - "What was fun?"
   - "What was confusing?"
   - "Would you play again?"
   - "What would you change?"

4. Repeat runs (1 hour)
   - Let them play freely
   - Observe retention

5. Survey (5 min)
   - Rate 1-10: Fun, Difficulty, Controls, Visuals
```

**Data to Collect:**
- Time to first death (difficulty curve)
- Upgrade choices (balance)
- Completion rate (too hard/easy?)
- Retention (do they keep playing?)

**Success Criteria:**
- 80%+ understand controls without tutorial
- 60%+ complete at least 3 runs
- Average fun rating: 7+/10
- No game-breaking bugs

#### **Phase 3: Public Beta (Week 14)**

**Who:** 50-100 Steam playtesters  
**Platform:** Steam Beta (or itch.io demo)  
**Duration:** 1 week  
**Focus:** Stability, balance, bugs

**How to Recruit:**
- Reddit: r/roguelites, r/playmygame
- Discord: Roguelite community servers
- Twitter: #indiegamedev #screenshotsaturday
- Friends of friends

**Feedback Collection:**
- Google Form survey (anonymous)
- Discord channel for bug reports
- Analytics: Unity Analytics or custom telemetry

**Data to Collect:**
- Crash reports
- Average session length
- Upgrade popularity (balance)
- Player retention (day 1, day 3, day 7)
- Performance issues by hardware

**Success Criteria:**
- Zero crash bugs
- Average session length: 15+ minutes
- 40%+ retention on day 3
- Positive sentiment in Discord/Reddit

---

## 📊 Test Coverage Monitoring

### Tools

**Unity Test Runner:**
- Built into Unity
- Runs both Edit Mode (unit) and Play Mode (integration) tests
- Access via `Window > General > Test Runner`

**Code Coverage Package:**
```powershell
# Install Unity Code Coverage package
# Window > Package Manager > Code Coverage
```

**Coverage Report Generation:**
```powershell
# Generate coverage report (do this weekly)
# Unity Menu: Window > Analysis > Code Coverage
# Enable: Enable Code Coverage
# Generate: Generate Report
```

### Weekly Coverage Check

**Every Friday (End of Week):**
1. Run all tests: `Ctrl+Alt+T` (Rider) or Test Runner (Unity)
2. Generate coverage report
3. Check coverage percentage
4. Identify untested code
5. Add tests if coverage < 90% for critical systems

**Coverage Targets by Week:**

| Week | Target Coverage | Focus Area |
|------|----------------|------------|
| **Week 7** | 50%+ | Combat system |
| **Week 8** | 60%+ | AI system |
| **Week 9** | 70%+ | Progression system |
| **Week 10** | 80%+ | All core systems |
| **Week 11-12** | 85%+ | Edge cases |
| **Week 13-14** | 90%+ | Full coverage |

---

## 🐛 Bug Tracking & Triage

### Bug Severity Levels

| Level | Description | Response Time |
|-------|-------------|--------------|
| **P0 - Critical** | Game-breaking, crashes, data loss | Fix immediately (same day) |
| **P1 - High** | Major feature broken, progression blocked | Fix within 2 days |
| **P2 - Medium** | Minor feature broken, workaround exists | Fix within 1 week |
| **P3 - Low** | Visual glitch, polish issue | Fix before launch (or defer) |
| **P4 - Enhancement** | Nice-to-have, not a bug | Consider for post-launch |

### Bug Reporting Template

```markdown
## Bug Report

**Severity:** [P0/P1/P2/P3/P4]
**Found in Week:** [X]
**Platform:** [PC/iOS/Android]
**Reproducible:** [Always/Sometimes/Rare]

**Steps to Reproduce:**
1. 
2. 
3. 

**Expected Behavior:**
[What should happen]

**Actual Behavior:**
[What actually happens]

**Screenshot/Video:**
[Attach if relevant]

**Additional Context:**
- Unity version: 2022.3.X
- Device: [Specs]
- Build: [Commit hash]
```

### Bug Triage Process

**Daily (During Weeks 13-17):**
1. Review all new bug reports
2. Assign severity level
3. Prioritize P0/P1 bugs
4. Fix critical bugs before continuing feature work
5. Log fixes in Git commit messages

---

## 🚀 Pre-Launch Testing Checklist

### Week 15 (Code Freeze → Launch -2 weeks)

**Functionality Testing:**
- [ ] Full gameplay loop works start to finish
- [ ] All 5 arenas load correctly
- [ ] Boss fight works without bugs
- [ ] Meta-progression saves/loads correctly
- [ ] All upgrade paths functional
- [ ] UI responsive at all resolutions

**Performance Testing:**
- [ ] Stable 60 FPS on target PC hardware
- [ ] Stable 30 FPS on target mobile devices
- [ ] No memory leaks (30-minute test)
- [ ] Load times within targets
- [ ] Battery usage acceptable (mobile)

**Platform-Specific Testing:**
- [ ] PC: Windows 10/11 (5+ different configs)
- [ ] iOS: iPhone 8+, iPad (5+ devices)
- [ ] Android: Samsung/Pixel/OnePlus (10+ devices)

**Localization (If Applicable):**
- [ ] Text displays correctly (no overflow)
- [ ] Special characters work (if PT-BR)
- [ ] Audio/VO matches text (if using)

### Week 16 (Launch -1 week)

**Regression Testing:**
- [ ] All Week 15 tests passed again
- [ ] No new bugs introduced by polish
- [ ] Performance still meets targets

**Certification (Mobile):**
- [ ] Apple TestFlight beta submitted
- [ ] Google Play internal testing completed
- [ ] No policy violations

**Steam Build:**
- [ ] Demo build uploaded and tested
- [ ] Full game build uploaded (private)
- [ ] Achievements tested
- [ ] Cloud saves tested

### Week 17 (Launch Day)

**Final Checks:**
- [ ] All known P0/P1 bugs fixed
- [ ] Release notes written
- [ ] Launch trailer uploaded
- [ ] Social media scheduled
- [ ] Monitoring dashboard ready (crash reports)

---

## 🛠️ Testing Tools & Infrastructure

### Unit Testing Framework

**NUnit (Unity Default):**
```csharp
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class CombatSystemTests
{
    [Test]
    public void DamageCalculation_WorksCorrectly()
    {
        // Test implementation
    }
}
```

### Mocking Framework

**NSubstitute (Recommended):**
```powershell
# Install via Package Manager
# Add to manifest.json:
# "com.unity.nuget.nsubstitute": "5.0.0"
```

```csharp
using NSubstitute;

[Test]
public void TestWithMock()
{
    var mockTarget = Substitute.For<IDamageable>();
    mockTarget.CurrentHealth.Returns(100f);
    
    // Test with mock
}
```

### Analytics & Telemetry (Week 13+)

**Unity Analytics (Built-in):**
- Player session length
- Progression funnels
- Crash reports

**Custom Events:**
```csharp
// Track important events
Analytics.CustomEvent("player_death", new Dictionary<string, object>
{
    { "wave_number", currentWave },
    { "time_survived", timeSurvived },
    { "upgrades_chosen", upgradeList }
});
```

---

## 📈 Testing Metrics & KPIs

### Code Quality Metrics

| Metric | Target | How to Measure |
|--------|--------|---------------|
| **Test Coverage** | 90%+ | Unity Code Coverage |
| **Test Execution Time** | < 5 sec | Test Runner |
| **Passing Tests** | 100% | Test Runner |
| **Code Warnings** | 0 | Unity Console |

### Playtesting Metrics

| Metric | Target | How to Measure |
|--------|--------|---------------|
| **Average Session** | 15+ min | Analytics |
| **Day 3 Retention** | 40%+ | Analytics |
| **Completion Rate** | 60%+ | Manual tracking |
| **Fun Rating** | 7+/10 | Survey |

### Performance Metrics

| Metric | Target | How to Measure |
|--------|--------|---------------|
| **Crash Rate** | < 0.1% | Analytics |
| **Frame Rate** | 60 FPS (PC) | Profiler |
| **Load Time** | < 5 sec | Manual testing |
| **Memory Leaks** | 0 | Profiler (30-min test) |

---

## ✅ Testing Milestones

### Week 7 (First Code Week)
- [ ] Unit test framework set up
- [ ] First 10 unit tests written
- [ ] TDD workflow established

### Week 10 (First Playable)
- [ ] 50+ unit tests passing
- [ ] 5+ integration tests passing
- [ ] 50%+ code coverage

### Week 13 (Friends & Family)
- [ ] 100+ unit tests passing
- [ ] 20+ integration tests passing
- [ ] 80%+ code coverage
- [ ] 5 playtesters recruited

### Week 14 (Code Complete)
- [ ] 90%+ code coverage
- [ ] All P0/P1 bugs fixed
- [ ] Public beta ready

### Week 17 (Launch)
- [ ] Zero P0 bugs
- [ ] Zero P1 bugs known at launch
- [ ] All platforms tested
- [ ] Performance targets met

---

**Document Owner:** Anderson Gonçalves  
**Review Frequency:** Weekly  
**Last Updated:** November 3, 2025  
**Next Review:** Week 7 (Start of coding phase)

