# Low-Poly Retro Art Guide

**Style:** Low-Poly + VHS = Retro Space Arcade  
**Inspired By:** Star Fox, Devil Daggers, Dusk, Hyperspace Dogfights  
**Goal:** 80s/90s arcade feel with modern clarity

---

## Visual Pillars

### Low-Poly Modeling

**Triangle Budget:**
- Player Ship: 800-1500 tris
- Basic Enemies (Falcon, Sentry, Raven-IX): 400-800 tris
- Heavy Enemies (Atlas, Aegis): 1000-1500 tris
- Boss (Prometheus): 2000-3000 tris
- Props: 100-500 tris each

**Rules:**
- Sharp angles - no smooth subdivisions
- Geometric shapes - cubes, pyramids, cylinders
- Flat shading - no normal maps
- Bold silhouettes - recognizable from distance
- Minimal detail - suggestive not realistic

**Do:**
- Use basic shapes
- Keep edges hard
- Embrace facets
- Design for silhouettes
- Think "origami spaceship"

**Don't:**
- Subdivide or smooth
- Add complex curves
- Use high-res textures
- Over-detail models
- Try for "realistic"

---

## Modeling Examples

### Player Ship (Alien Tech)

```
Triangle Count: 1200
Key Features:
- Pointed cockpit (pyramid)
- Angular wings (flat planes)
- Glowing energy core (emissive cube)
- Engine exhausts (glowing cylinders)

Modeling Steps (Blender):
1. Start with cube for main body
2. Extrude and scale to create cockpit point
3. Add flat planes for wings (no thickness)
4. Place small cubes/cylinders for details
5. NO SUBDIVISION - keep everything faceted
6. UV unwrap (flat colors only)
```

### Falcon Mk I (Human Fighter)

**Concept:** Triangular fighter, sharp and aggressive

```
Triangle Count: 600
Key Features:
- Triangle-based fuselage
- Angular wings
- Visible cockpit (darker color)
- Engine glow points

Modeling Approach:
- Literally start with a stretched pyramid
- Add wing planes
- Small cubes for cockpit/engines
- Total time: 30-45 minutes
```

### Prometheus (Capital Ship Boss)

**Concept:** Massive geometric fortress

```
Triangle Count: 2500
Key Features:
- Large cubic main body
- Modular attachments (shield generators)
- Multiple weapon hardpoints
- Glowing weak points

Construction:
- Main body: elongated cube
- Towers: smaller cubes on top
- Guns: cylinders pointing outward
- Shields: transparent cubes around weak points
```

---

## Color Palette (Retro Aesthetic)

### Player/Alien Colors
```
Primary:   #00FFCC (Cyan/Teal)      - Ship hull
Secondary: #FF00FF (Magenta)        - Energy glow
Accent:    #FFFF00 (Yellow)         - Highlights
Dark:      #001122 (Deep Blue-Black)- Shadows
```

### Enemy/Human Colors
```
Primary:   #888888 (Gray)           - Hull
Secondary: #FF3333 (Red)            - Warning lights
Accent:    #3366FF (Blue)           - Shields
Dark:      #222222 (Charcoal)       - Details
```

### Environment Colors
```
Space:     #000000 (Black) with slight blue tint
Nebula:    Gradient from #FF6600 to #FF0066 (Aureon Prime)
Ice:       #00CCFF to #FFFFFF (Cryovex)
Lava:      #FF0000 to #FFAA00 (Voltra-9)
```

**Palette Rules:**
- **Limited colors** (3-4 per model)
- **High contrast** (vibrant vs dark)
- **Flat shading** (no gradients on models)
- **Emissive materials** for glows

---

## VHS Filter Implementation (Unity)

### Post-Processing Stack Setup

**1. Install Post-Processing Package**
```
Window > Package Manager > Post Processing
```

**2. Create Post-Processing Volume**
```
GameObject > Volume > Global Volume
Add Component > Post-Process Volume
Check "Is Global"
```

**3. VHS Effect Components**

#### A. Scanlines Overlay
```csharp
// Custom shader for scanlines
Shader "Custom/Scanlines" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.3
        _ScanlineCount ("Scanline Count", Float) = 400
    }
    
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            sampler2D _MainTex;
            float _ScanlineIntensity;
            float _ScanlineCount;
            
            float4 frag (v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv);
                
                // Scanline effect
                float scanline = sin(i.uv.y * _ScanlineCount * 3.14159) * 0.5 + 0.5;
                col.rgb *= 1.0 - (scanline * _ScanlineIntensity);
                
                return col;
            }
            ENDCG
        }
    }
}
```

#### B. Chromatic Aberration
```
Post-Process Volume > Add Override > Chromatic Aberration
Intensity: 0.3-0.5 (subtle red/blue shift)
```

#### C. Film Grain
```
Post-Process Volume > Add Override > Grain
Intensity: 0.4-0.6
Size: 1.5-2.0
Colored: Yes (slight color noise)
```

#### D. Color Grading (Retro Palette)
```
Post-Process Volume > Add Override > Color Grading
Temperature: +10 (slight warmth)
Tint: -5 (slight magenta)
Saturation: +20 (vibrant)
Contrast: +10 (punchy blacks)

LUT (Optional): Create custom LUT with desaturated look
```

#### E. Vignette
```
Post-Process Volume > Add Override > Vignette
Intensity: 0.3-0.4
Smoothness: 0.4
Rounded: Yes
```

#### F. CRT Screen Curvature (Optional)
```csharp
// Custom shader for CRT distortion
Shader "Custom/CRTCurvature" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion", Float) = 0.1
    }
    
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            sampler2D _MainTex;
            float _Distortion;
            
            float2 CRTCurve(float2 uv) {
                uv = uv * 2.0 - 1.0;
                float2 offset = abs(uv.yx) / float2(6.0, 4.0);
                uv = uv + uv * offset * offset;
                uv = uv * 0.5 + 0.5;
                return uv;
            }
            
            float4 frag (v2f i) : SV_Target {
                float2 curvedUV = CRTCurve(i.uv);
                
                // Vignette at edges
                if (curvedUV.x < 0.0 || curvedUV.x > 1.0 || 
                    curvedUV.y < 0.0 || curvedUV.y > 1.0) {
                    return float4(0, 0, 0, 1);
                }
                
                return tex2D(_MainTex, curvedUV);
            }
            ENDCG
        }
    }
}
```

### VHS Settings Profile (Unity Asset)

```csharp
[CreateAssetMenu(fileName = "VHSSettings", menuName = "Game/VHS Settings")]
public class VHSSettings : ScriptableObject {
    [Header("Scanlines")]
    public float scanlineIntensity = 0.3f;
    public float scanlineCount = 400f;
    
    [Header("Chromatic Aberration")]
    public float chromaticIntensity = 0.4f;
    
    [Header("Film Grain")]
    public float grainIntensity = 0.5f;
    public float grainSize = 1.5f;
    
    [Header("Color Grading")]
    public float temperature = 10f;
    public float tint = -5f;
    public float saturation = 20f;
    public float contrast = 10f;
    
    [Header("Vignette")]
    public float vignetteIntensity = 0.35f;
    
    [Header("CRT Curvature")]
    public bool enableCRTCurve = true;
    public float distortionAmount = 0.1f;
}
```

---

## Lighting Setup (Per Arena)

### Aureon Prime (Orange Dust)
```
Directional Light:
- Color: #FF6600 (Orange)
- Intensity: 0.8
- Shadows: Hard

Ambient:
- Sky Color: #FF8844
- Equator: #884400
- Ground: #442200

Fog:
- Color: #FF6600
- Density: 0.02 (thick)
```

### Cryovex (Blue Ice)
```
Directional Light:
- Color: #00CCFF (Cyan)
- Intensity: 1.0
- Shadows: Soft

Ambient:
- Sky Color: #0088FF
- Equator: #004488
- Ground: #002244

Fog:
- Color: #00CCFF
- Density: 0.015
```

### Voltra-9 (Red Sun)
```
Directional Light:
- Color: #FF0000 (Red)
- Intensity: 1.2
- Shadows: Hard

Ambient:
- Sky Color: #FF4400
- Equator: #AA2200
- Ground: #550000

Fog:
- Color: #FF2200
- Density: 0.01
- Heat distortion shader
```

---

## VFX (Retro Particle Style)

### Weapon Fire (Plasma Beam)
```
Particle System:
- Shape: Cone (narrow)
- Start Size: 0.1-0.3
- Start Color: Cyan (#00FFFF)
- Emission: 50 particles/sec
- Renderer: Billboard, Additive blend

Material:
- Unlit shader
- Additive blending
- Simple gradient texture (white center, fade edges)
```

### Explosions (Enemy Death)
```
Particle System:
- Shape: Sphere
- Start Size: 0.5-2.0 (random)
- Start Color: Orange to Yellow gradient
- Emission: 20 burst
- Lifetime: 0.5 seconds
- Velocity: Radial outward

Extra:
- Screen shake (magnitude: 0.2)
- Time.timeScale = 0.9 for 0.1 sec (slight slowmo)
```

### Scrap Collection
```
Particle System:
- Shape: Point
- Start Size: 0.2
- Start Color: Blue (#00FFFF)
- Emission: 5 particles
- Velocity: Toward player (attraction)

Animation:
- Scale up as approaching player
- Glow intensity increases
- Audio: Metallic ping on collect
```

---

## Material Setup (Unity URP)

### Player Ship Material
```
Material: PlayerShip_Mat
Shader: URP/Lit (Unlit for pure flat look)

Properties:
- Base Color: #00FFCC (Cyan)
- Emission: #FF00FF (Magenta), Intensity: 2
- Smoothness: 0 (completely matte)
- Metallic: 0

Note: Emissive parts created with separate material
      for energy core/engines
```

### Enemy Material
```
Material: EnemyHull_Mat
Shader: URP/Lit

Properties:
- Base Color: #888888 (Gray)
- Emission: None on hull
- Smoothness: 0.2
- Metallic: 0.8 (slightly metallic)

Separate material for warning lights:
- Base Color: #FF0000
- Emission: #FF0000, Intensity: 3
- Pulsing animation via script
```

### Environment Props (Asteroids, Debris)
```
Material: SpaceDebris_Mat
Shader: URP/Unlit

Properties:
- Base Color: #444444 (Dark gray)
- No emission
- No reflections

Random rotation via script for variety
```

---

## Animation & "Juice"

### Ship Movement Tilt
```csharp
public class ShipTilt : MonoBehaviour {
    [SerializeField] private float tiltAmount = 15f;
    [SerializeField] private float tiltSpeed = 5f;
    
    private Vector2 _moveInput;
    
    void Update() {
        // Tilt based on movement direction
        float targetTiltX = -_moveInput.y * tiltAmount;
        float targetTiltZ = -_moveInput.x * tiltAmount;
        
        Quaternion targetRotation = Quaternion.Euler(targetTiltX, 0, targetTiltZ);
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, 
            targetRotation, 
            tiltSpeed * Time.deltaTime
        );
    }
}
```

### Screen Shake (On Hit)
```csharp
public class CameraShake : MonoBehaviour {
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;
        
        while (elapsed < duration) {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = originalPos + new Vector3(x, y, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = originalPos;
    }
}

// Usage
void OnPlayerHit() {
    StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
}
```

### Hit Flash (Damage Feedback)
```csharp
public class HitFlash : MonoBehaviour {
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;
    
    private Material _material;
    private Color _originalColor;
    
    void Start() {
        _material = _renderer.material;
        _originalColor = _material.color;
    }
    
    public void Flash() {
        StartCoroutine(FlashRoutine());
    }
    
    private IEnumerator FlashRoutine() {
        _material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        _material.color = _originalColor;
    }
}
```

---

## Performance Optimization (Low-Poly Benefits)

### Why Low-Poly is Perfect for Multiplayer

**1. Low Triangle Count = High Performance**
- Player ship (1200 tris) vs typical game (50,000+ tris)
- Can render 100+ enemies simultaneously
- Perfect for wave-based gameplay
- Mobile-friendly from day one

**2. Simple Materials = Fast Rendering**
- No complex shaders
- Minimal texture lookups
- Fast material switching
- Low VRAM usage

**3. Network-Friendly**
- Simple meshes = small asset bundles
- Flat colors = no texture downloads
- Fast instantiation
- Low bandwidth for sync

### Optimization Checklist
- [ ] All models under triangle budget
- [ ] Static batching enabled for props
- [ ] GPU instancing for repeated enemies
- [ ] Object pooling for projectiles/enemies
- [ ] LOD not needed (already low-poly!)
- [ ] Occlusion culling for arena boundaries

---

## Reference Images & Inspirations

### Games to Study
1. **Star Fox (SNES/N64)** - Low-poly ships, colorful
2. **Devil Daggers** - Retro FPS, VHS filter, red aesthetic
3. **Dusk** - Retro shooter, chunky pixels
4. **Superhot** - Minimalist, low-poly, bold colors
5. **Hyperspace Dogfights** - Low-poly space combat

### Visual Moodboard
```
[Geometric spaceships]
[80s neon grids]
[VHS static overlays]
[Scanline effects]
[Vibrant vs dark contrast]
[Faceted low-poly aesthetics]
```

---

## Tomorrow's Task (November 4)

### Morning: Blender Setup
1. Open Blender
2. Create new project: "TheInvasionReforged_Models"
3. Set unit scale to match Unity (1 Blender unit = 1 Unity meter)
4. Create first model: Player ship (800-1500 tris)
   - Start with cube
   - Model for 2-3 hours
   - NO smooth shading
   - Keep it angular

### Afternoon: Unity VHS Filter
1. Create new Unity project (URP template)
2. Install Post-Processing package
3. Create Global Volume
4. Add scanlines, chromatic aberration, film grain
5. Import Blender ship model
6. Test with flat-shaded material
7. Screenshot and compare to retro references

### Goal: By end of Day 1
- [ ] Player ship base mesh complete (Blender)
- [ ] VHS filter working (Unity)
- [ ] One screenshot that looks retro/aesthetic
- [ ] Excited about the visual direction!

---

**Remember: Embrace the low-poly aesthetic. Less is more. Make it look intentionally retro, not accidentally unfinished!**

