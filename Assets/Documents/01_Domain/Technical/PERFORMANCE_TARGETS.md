# Performance Targets

**Project:** The Invasion: Reforged  
**Engine:** Unity 2022.3 LTS (URP)  
**Platforms:** PC (Primary), iOS/Android (Secondary)  
**Updated:** November 3, 2025

---

## Target Performance

### PC (Primary Platform)

| Metric | Target | Minimum Acceptable |
|--------|--------|-------------------|
| **Frame Rate** | 60 FPS stable | 45 FPS (1% lows) |
| **Resolution** | 1920x1080 (native) | Scalable 720p-4K |
| **Memory Usage** | < 2GB RAM | < 3GB RAM |
| **VRAM Usage** | < 1GB | < 2GB |
| **Load Time (Main Menu)** | < 3 seconds | < 5 seconds |
| **Load Time (Arena)** | < 2 seconds | < 4 seconds |
| **Build Size** | < 500MB | < 800MB |

**Target Hardware:**
- CPU: Intel i5-8400 / AMD Ryzen 5 2600 (mid-range 2018)
- GPU: GTX 1060 6GB / RX 580 (mid-range 2016)
- RAM: 8GB system memory
- Storage: HDD (not SSD required)

### Mobile (iOS/Android)

| Metric | Target | Minimum Acceptable |
|--------|--------|-------------------|
| **Frame Rate** | 60 FPS on flagship | 30 FPS stable on mid-range |
| **Resolution** | Native (1080p-1440p) | Scalable to 720p |
| **Memory Usage** | < 1.5GB RAM | < 2GB RAM |
| **Battery Usage** | < 15% per 30 min | < 20% per 30 min |
| **Load Time (Cold Start)** | < 5 seconds | < 8 seconds |
| **Build Size (APK/IPA)** | < 200MB | < 300MB |
| **Download Size** | < 150MB | < 250MB |

**Target Devices:**
- **High-End:** iPhone 12+, Samsung Galaxy S21+, Pixel 6+
- **Mid-Range:** iPhone XR, Samsung Galaxy A52, Pixel 4a
- **Low-End (Minimum):** iPhone 8, Samsung Galaxy A32 (30 FPS target)

---

## Performance Budgets

### Per-Frame (60 FPS = 16.67ms)

| System | Budget | Critical Threshold |
|--------|--------|-------------------|
| **Rendering** | 8ms | 10ms |
| **Gameplay Logic** | 3ms | 5ms |
| **Physics** | 2ms | 3ms |
| **AI (10 enemies)** | 1ms | 2ms |
| **UI** | 0.5ms | 1ms |
| **Audio** | 0.5ms | 1ms |
| **Scripting (Mono)** | 1ms | 2ms |
| **Other** | 0.5ms | 1ms |
| **Total** | **16.0ms** | **25ms (40 FPS)** |

### Draw Call Budget

| Platform | Target Draw Calls | Maximum |
|----------|------------------|---------|
| **PC** | 200-500 | 1000 |
| **Mobile (High-End)** | 100-200 | 300 |
| **Mobile (Mid-Range)** | 50-100 | 150 |

### Polygon Budget (On-Screen)

| Platform | Target Triangles | Maximum |
|----------|-----------------|---------|
| **PC** | 100K-200K | 500K |
| **Mobile (High-End)** | 50K-100K | 200K |
| **Mobile (Mid-Range)** | 30K-50K | 100K |

---

## 📊 Asset Optimization Standards

### 3D Models (Triangle Counts)

| Asset Type | PC Target | Mobile Target |
|-----------|-----------|---------------|
| **Player Ship** | 1200-1500 | 800-1000 |
| **Basic Enemy** | 600-800 | 400-600 |
| **Heavy Enemy** | 1200-1500 | 800-1000 |
| **Boss** | 2500-3000 | 1500-2000 |
| **Environment Prop** | 200-500 | 100-300 |

### Textures

| Use Case | PC Resolution | Mobile Resolution | Format |
|----------|---------------|-------------------|--------|
| **Character/Ship** | 1024x1024 | 512x512 | PNG → Compressed |
| **Environment** | 2048x2048 | 1024x1024 | PNG → Compressed |
| **UI Elements** | 512x512 | 256x256 | PNG (Alpha) |
| **VFX** | 512x512 | 256x256 | PNG (Alpha) |
| **Skybox** | 2048x2048 | 1024x1024 | Compressed |

**Compression Settings (Unity):**
- **Opaque textures:** BC7 (PC), ASTC 6x6 (Mobile)
- **Transparent textures:** BC7 Alpha (PC), ASTC 6x6 (Mobile)
- **UI:** BC7 (PC), ASTC 4x4 (Mobile) - Higher quality for readability
- **Enable Mipmaps:** Yes (except UI)

### Particle Systems

| Effect Type | Max Particles | Max Systems Active |
|-------------|--------------|-------------------|
| **Player Weapons** | 50 per system | 3 simultaneous |
| **Enemy Weapons** | 30 per system | 10 simultaneous |
| **Explosions** | 100 burst | 5 simultaneous |
| **Environmental** | 200 persistent | 2 simultaneous |
| **Total On-Screen** | **1000** | **20 systems** |

### Audio

| Type | Format | Bitrate | Compression |
|------|--------|---------|-------------|
| **Music** | Ogg Vorbis | 128 kbps | Streaming |
| **SFX (Long)** | Ogg Vorbis | 96 kbps | Compressed in Memory |
| **SFX (Short)** | WAV | 16-bit | Decompress on Load |
| **Voice (if added)** | Ogg Vorbis | 64 kbps | Streaming |

---

## 🔍 Profiling & Testing Guidelines

### When to Profile

**Weekly (During Development):**
- Every Friday afternoon: 30-minute profiling session
- Check for regressions from the week
- Document any issues in Git commit

**Before Milestones:**
- Week 6 (Art Lock): Profile with all assets loaded
- Week 10 (First Playable): Full gameplay profiling
- Week 14 (Code Complete): Final optimization pass
- Week 16 (Release Candidate): Platform-specific testing

### Profiling Tools

**Unity Profiler (Primary):**
- CPU Usage (frame time breakdown)
- GPU Usage (draw calls, vertices)
- Memory (allocations, GC spikes)
- Rendering (batching, overdraw)

**Platform-Specific:**
- **PC:** NVIDIA Nsight (GPU), Intel VTune (CPU)
- **iOS:** Xcode Instruments
- **Android:** Android Profiler, Snapdragon Profiler

### Performance Testing Scenarios

**Standard Scenario (Baseline):**
- Player ship active
- 10 enemies on screen
- 3 weapon systems firing
- 5 particle effects active
- UI elements visible

**Stress Test (Maximum Load):**
- Player ship + boss
- 20 enemies on screen
- All weapon systems firing
- 15 particle effects active
- UI + debug overlays

**Mobile-Specific Tests:**
- 30-minute battery drain test
- Thermal throttling (20-minute sustained gameplay)
- Background/foreground switching
- Low-memory devices (iPhone 8, Galaxy A32)

---

## ⚡ Optimization Priorities

### Phase 1: Art Production (Weeks 1-6)

**Priority: Asset Optimization**
- Keep triangle counts within budget
- Use texture atlases where possible
- Test VHS filter performance early (Week 3)
- Profile with 10+ models on screen

**Deliverable (Week 6):**
- All assets meet triangle budget
- VHS filter runs at 60 FPS on target hardware
- Draw calls < 200 in test scene

### Phase 2: Gameplay Implementation (Weeks 7-14)

**Priority: Runtime Performance**
- Object pooling for enemies/projectiles (Week 7)
- AI optimization (LOD for distant enemies) (Week 8)
- GC allocation reduction (Week 10)
- Mobile build performance parity (Week 12)

**Deliverable (Week 14):**
- PC: Stable 60 FPS with 20 enemies
- Mobile: Stable 30 FPS on mid-range devices
- Zero memory leaks (30-minute test)

### Phase 3: Launch Polish (Weeks 15-17)

**Priority: Platform Optimization**
- iOS/Android certification compliance
- Battery optimization
- Loading time reduction
- Build size optimization

**Deliverable (Week 17):**
- All targets met (see tables above)
- No performance regressions from Week 14

---

## 🚨 Performance Red Flags

### Immediate Action Required If:

1. **Frame time exceeds 20ms (50 FPS)** on target hardware
   - **Action:** Profile immediately, identify bottleneck
   - **Escalation:** Consider cutting features if not fixable in 2 days

2. **Memory exceeds 3GB on PC** or **2GB on mobile**
   - **Action:** Check for leaks, reduce texture sizes
   - **Escalation:** May need to reduce asset quality

3. **Draw calls exceed 500 (PC)** or **200 (mobile)**
   - **Action:** Implement batching, reduce materials
   - **Escalation:** May need to simplify visual effects

4. **Build size exceeds 800MB (PC)** or **300MB (mobile)**
   - **Action:** Compress assets, remove unused resources
   - **Escalation:** Consider asset streaming

5. **Load time exceeds 10 seconds** anywhere
   - **Action:** Profile asset loading, implement async loading
   - **Escalation:** May need loading screen optimization

---

## 📱 Mobile-Specific Considerations

### Battery Optimization

**Techniques:**
- Reduce target frame rate to 30 FPS (mobile default)
- Implement frame rate throttling when idle
- Reduce particle counts on mobile
- Lower texture resolution
- Disable expensive post-processing (VHS filter intensity)

**Testing:**
- 30-minute continuous play should use < 20% battery
- Monitor thermal throttling on devices
- Test with "Low Power Mode" enabled (iOS)

### Device Tier System

**Implementation (Week 12):**
```csharp
public enum DeviceTier
{
    High,    // 60 FPS, full VHS filter, max particles
    Medium,  // 30 FPS, reduced VHS filter, reduced particles
    Low      // 30 FPS, minimal VHS filter, minimal particles
}
```

**Auto-Detection:**
- Use `SystemInfo.processorFrequency` and `SystemInfo.systemMemorySize`
- Benchmark first 5 seconds of gameplay
- Adjust quality based on detected performance

---

## 🎯 Optimization Techniques Approved for This Project

### Rendering Optimization

✅ **GPU Instancing** for repeated enemies (Falcon swarms)  
✅ **Static Batching** for environment props  
✅ **Texture Atlasing** for UI elements  
✅ **Occlusion Culling** for arena boundaries (if needed)  
✅ **LOD Groups** for boss model only (if mobile struggles)  

❌ **Dynamic Batching** - Not needed with low draw calls  
❌ **Lightmapping** - Real-time lighting is fine for this aesthetic  

### Gameplay Optimization

✅ **Object Pooling** for projectiles, enemies, particles  
✅ **Spatial Partitioning** (simple grid) for collision detection (if > 50 entities)  
✅ **Fixed Update Intervals** for AI (not every frame)  

❌ **ECS/DOTS** - Overkill for this scope, SOLID is sufficient  

### Memory Optimization

✅ **Asset Bundles** for arenas (load/unload per level)  
✅ **Texture Streaming** if build size exceeds 500MB  
✅ **Audio Streaming** for music  

❌ **Aggressive GC** tuning - Unity 2022.3 handles this well  

---

## 📈 Performance Tracking

### Weekly Metrics to Log

Create `PerformanceLog.md` and update weekly:

```markdown
## Week [X] Performance Report

**Date:** [Date]
**Build:** [Commit Hash]

**PC Performance:**
- Average FPS: [XX] fps
- 1% Lows: [XX] fps
- Frame Time: [XX] ms
- Draw Calls: [XXX]
- Memory: [X.X] GB

**Mobile Performance (Test Device: [Model]):**
- Average FPS: [XX] fps
- Frame Time: [XX] ms
- Battery per 30min: [XX]%

**Issues Found:**
- [List any regressions or problems]

**Actions Taken:**
- [List optimizations performed]
```

---

## ✅ Performance Milestones

### Week 6 (Art Lock)
- [ ] All models meet triangle budget
- [ ] VHS filter performance validated (60 FPS PC, 30 FPS mobile)
- [ ] Test scene with 10 enemies runs smoothly

### Week 10 (First Playable)
- [ ] Full gameplay loop runs at target FPS
- [ ] Object pooling implemented
- [ ] Memory usage within budget

### Week 14 (Code Complete)
- [ ] All performance targets met (see tables)
- [ ] No memory leaks in 30-minute test
- [ ] Mobile parity achieved (30 FPS stable)

### Week 17 (Launch)
- [ ] Performance validated on 5+ PC configs
- [ ] Performance validated on 10+ mobile devices
- [ ] Zero performance-related bug reports in beta

---

## 🆘 Performance Emergency Protocol

If performance targets are not met by Week 14:

**Tier 1 Cuts (Minimal Impact):**
- Reduce max enemies from 20 → 15
- Reduce particle counts by 30%
- Lower VHS filter quality on mobile

**Tier 2 Cuts (Moderate Impact):**
- Remove 2 arenas (launch with 3 instead of 5)
- Simplify boss to single-phase
- Delay mobile launch to Week 19

**Tier 3 Cuts (High Impact - Avoid if possible):**
- Reduce enemy types from 5 → 3
- Remove temporary upgrade system complexity
- PC-only launch (no mobile)

**Decision Point:** Week 13 (give 1 week to fix before escalating)

---

**Document Owner:** Anderson Gonçalves  
**Review Frequency:** Weekly during production  
**Last Performance Audit:** [To be conducted Week 6]  
**Next Review:** Week 6 - Art Lock Milestone

