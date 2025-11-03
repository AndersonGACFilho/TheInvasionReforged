# Leaderboard System Architecture Diagram

> **WARNING: POST-LAUNCH FEATURE - NOT IN INITIAL RELEASE**  
> **Implementation Timeline:** Weeks 17-19 after initial game launch  
> **Reason for Deferral:** System complexity requires dedicated focus on backend infrastructure, anti-cheat, multi-platform integration, and security. Core game will launch first to establish player base and ensure stability before adding competitive features.

## System Architecture Overview

```mermaid
graph TB
    subgraph "CLIENT LAYER"
        GS[Game Session<br/>• Track Stats<br/>• Calculate Score<br/>• Checksum]
        HT[Hangar Terminal<br/>• View Ranks<br/>• Filter Tabs<br/>• Friends View<br/>• Challenges]
        PR[Post-Run UI<br/>• Show Rank<br/>• Achievements<br/>• Personal Best<br/>• Beat Friends]
    end
    
    subgraph "LEADERBOARD SYSTEM CORE"
        SM[Score Manager<br/>• Validate<br/>• Submit<br/>• Queue]
        DM[Display Manager<br/>• Fetch Data<br/>• Cache<br/>• Format]
        CM[Challenge Manager<br/>• Generate Seed<br/>• Apply Mods<br/>• Track Time]
    end
    
    subgraph "PLATFORM ABSTRACTION LAYER"
        LBAPI[ILeaderboardAPI<br/>• SubmitScore<br/>• FetchRankings<br/>• GetPlayerRank]
        AUTH[IAuthProvider<br/>• Authenticate<br/>• GetPlayerID<br/>• RefreshToken]
        FRIENDS[IFriendsProvider<br/>• GetFriends<br/>• GetScores<br/>• SendNotif]
    end
    
    subgraph "BACKEND PROVIDERS"
        CB[Custom Backend<br/>Firebase/PlayFab/AWS]
        STEAM[Steam Services<br/>• Auth<br/>• Leaderboards<br/>• Achievements]
        MOBILE[Mobile Services<br/>• Game Center<br/>• Google Play<br/>• Friends API]
    end
    
    subgraph "BACKEND SERVICES"
        GW[API Gateway<br/>Regional: NA/EU/ASIA/OCE]
        SS[Score Service<br/>• Validate<br/>• Store<br/>• Query]
        AS[Auth Service<br/>• JWT<br/>• OAuth<br/>• Ban]
        CS[Challenge Service<br/>• Generate<br/>• Schedule<br/>• Reward]
        AC[Anti-Cheat<br/>• Validate<br/>• Flag Anomaly<br/>• Replay Check]
        DB[(Database<br/>Scores/Users/Replays)]
        CACHE[(Cache<br/>Redis<br/>Top 1000)]
    end
    
    GS --> SM
    HT --> DM
    PR --> SM
    HT -.-> DM
    
    SM --> LBAPI
    DM --> LBAPI
    CM --> LBAPI
    SM --> AUTH
    DM --> FRIENDS
    
    LBAPI --> CB
    LBAPI --> STEAM
    LBAPI --> MOBILE
    AUTH --> CB
    AUTH --> STEAM
    AUTH --> MOBILE
    FRIENDS --> STEAM
    FRIENDS --> MOBILE
    
    CB --> GW
    STEAM --> GW
    MOBILE --> GW
    
    GW --> SS
    GW --> AS
    GW --> CS
    
    SS --> AC
    SS --> DB
    SS --> CACHE
    AS --> DB
    CS --> DB
    
    style GS fill:#4A90E2
    style HT fill:#4A90E2
    style PR fill:#4A90E2
    style SM fill:#50C878
    style DM fill:#50C878
    style CM fill:#50C878
    style LBAPI fill:#F5A623
    style AUTH fill:#F5A623
    style FRIENDS fill:#F5A623
    style CB fill:#BD10E0
    style STEAM fill:#BD10E0
    style MOBILE fill:#BD10E0
    style AC fill:#D0021B
    style DB fill:#7ED321
    style CACHE fill:#7ED321
```

## Score Submission Flow

```mermaid
sequenceDiagram
    participant Player
    participant GameSession
    participant ScoreManager
    participant AntiCheat
    participant Backend
    participant Database
    participant PostRunUI
    
    Player->>GameSession: Complete Run
    GameSession->>GameSession: Calculate Final Stats
    GameSession->>GameSession: Generate Checksum
    GameSession->>ScoreManager: Submit Score
    
    ScoreManager->>ScoreManager: Client Validation<br/>(Hash, Timestamp, Version)
    ScoreManager->>Backend: API Call (HTTPS/JWT)
    
    Backend->>AntiCheat: Validate Submission
    AntiCheat->>AntiCheat: Verify Checksum
    AntiCheat->>AntiCheat: Check Time Ratios
    AntiCheat->>AntiCheat: Cross-Reference Stats
    AntiCheat->>AntiCheat: Anomaly Detection (3σ)
    
    alt Score Valid
        AntiCheat->>Backend: Accept
        Backend->>Database: Store Score
        Database->>Backend: Return Player Rank
        Backend->>ScoreManager: Return Rank #247
        ScoreManager->>PostRunUI: Display Rank Achievement
        PostRunUI->>Player: Show "You ranked #247!"
    else Score Suspicious
        AntiCheat->>Backend: Flag for Review
        Backend->>Database: Store with Flag
        Backend->>ScoreManager: Return Pending Status
    else Score Invalid
        AntiCheat->>Backend: Reject
        Backend->>ScoreManager: Return Error
        ScoreManager->>Player: Submission Failed
    end
```

## Leaderboard Viewing Flow

```mermaid
sequenceDiagram
    participant Player
    participant HangarTerminal
    participant DisplayManager
    participant Cache
    participant Backend
    participant Database
    
    Player->>HangarTerminal: Open Terminal
    HangarTerminal->>DisplayManager: Request Rankings
    
    DisplayManager->>Cache: Check Cache (5-min TTL)
    
    alt Cache Hit
        Cache->>DisplayManager: Return Top 1000
        DisplayManager->>HangarTerminal: Format & Display
        HangarTerminal->>Player: Show Leaderboard
    else Cache Miss
        Cache->>DisplayManager: Not Found
        DisplayManager->>Backend: API Call
        Backend->>Database: Query Rankings
        Database->>Backend: Return Data
        Backend->>DisplayManager: Return Rankings
        DisplayManager->>Cache: Store in Cache
        DisplayManager->>HangarTerminal: Format & Display
        HangarTerminal->>Player: Show Leaderboard
    end
    
    Player->>HangarTerminal: Filter by Category
    HangarTerminal->>DisplayManager: Request Specific Category
    DisplayManager->>Cache: Check Category Cache
    Cache->>DisplayManager: Return Data
    DisplayManager->>HangarTerminal: Update Display
```

## Anti-Cheat Security Flow

```mermaid
flowchart TD
    START([Player Submits Score]) --> CV[Client Validation]
    
    CV --> |Hash game state<br/>Timestamp<br/>Build version| TS[Transport Security]
    
    TS --> |HTTPS/TLS<br/>JWT Auth<br/>Rate Limiting| SV[Server Validation]
    
    SV --> |Verify checksum<br/>Check score ranges<br/>Cross-ref stats<br/>Version whitelist| AD[Anomaly Detection]
    
    AD --> |Statistical analysis<br/>3σ deviation<br/>Pattern recognition<br/>Velocity checks| DECISION{Score Status?}
    
    DECISION --> |Normal Range| ACCEPT[ACCEPT<br/>Add to Database]
    DECISION --> |Suspicious| FLAG[FLAG FOR REVIEW<br/>Manual Check Queue]
    DECISION --> |Obvious Cheat| REJECT[REJECT<br/>Ban User]
    
    FLAG --> MANUAL{Manual Review}
    MANUAL --> |Verified| ACCEPT
    MANUAL --> |Confirmed Cheat| REJECT
    
    ACCEPT --> DB[(Database)]
    DB --> PLAYER([Show Player Rank])
    
    REJECT --> BAN[(Ban List)]
    BAN --> ERROR([Submission Failed])
    
    style START fill:#4A90E2
    style CV fill:#F5A623
    style TS fill:#F5A623
    style SV fill:#F5A623
    style AD fill:#F5A623
    style ACCEPT fill:#7ED321
    style FLAG fill:#F8E71C
    style REJECT fill:#D0021B
    style MANUAL fill:#BD10E0
    style PLAYER fill:#50C878
    style ERROR fill:#D0021B
```

## Leaderboard Categories Structure

```mermaid
graph LR
    subgraph "GLOBAL LEADERBOARDS (Persistent)"
        G1[Survival Time<br/>Primary: Seconds<br/>Tiebreaker: Kills]
        G2[Enemies Defeated<br/>Primary: Kill Count<br/>Tiebreaker: Time]
        G3[Scrap Collected<br/>Primary: Scrap Total<br/>Tiebreaker: Time]
        G4[Boss Kill Time<br/>Primary: Minutes<br/>Tiebreaker: Damage]
        G5[Longest Streak<br/>Primary: Runs > Wave 10<br/>Reset: Death < Wave 10]
    end
    
    subgraph "CHALLENGE LEADERBOARDS (Time-Limited)"
        C1[Daily Challenge<br/>24h Fixed Seed<br/>Composite Score:<br/>40% Survival<br/>40% Enemies<br/>20% Scrap]
        C2[Weekly Challenge<br/>7-day Modifiers<br/>Unique Scoring<br/>Regional Filters<br/>Top 1000]
    end
    
    subgraph "SOCIAL LEADERBOARDS"
        F1[Steam Friends<br/>Auto-sync<br/>All Categories<br/>Beat Notifications]
        F2[Game Center<br/>iOS Integration<br/>All Categories<br/>Beat Notifications]
        F3[Google Play<br/>Android Integration<br/>All Categories<br/>Beat Notifications]
    end
    
    ROOT[Leaderboard System] --> GLOBAL
    ROOT --> CHALLENGE
    ROOT --> SOCIAL
    
    GLOBAL --> G1
    GLOBAL --> G2
    GLOBAL --> G3
    GLOBAL --> G4
    GLOBAL --> G5
    
    CHALLENGE --> C1
    CHALLENGE --> C2
    
    SOCIAL --> F1
    SOCIAL --> F2
    SOCIAL --> F3
    
    style ROOT fill:#BD10E0
    style GLOBAL fill:#4A90E2
    style CHALLENGE fill:#F5A623
    style SOCIAL fill:#50C878
```

## Implementation Timeline

```mermaid
gantt
    title Leaderboard Implementation Timeline (Weeks 17-19.5)
    dateFormat  YYYY-MM-DD
    section Phase 1: Backend
    Choose Backend Provider     :done, p1_1, 2025-11-03, 1d
    Set Up Authentication       :done, p1_2, after p1_1, 2d
    Create API Endpoints        :active, p1_3, after p1_2, 3d
    Implement Anti-Cheat        :p1_4, after p1_3, 3d
    Deploy to Dev Environment   :p1_5, after p1_4, 1d
    
    section Phase 2: UI
    Design Hangar Terminal      :p2_1, after p1_3, 2d
    Build Leaderboard Screen    :p2_2, after p2_1, 3d
    Create Category Tabs        :p2_3, after p2_2, 2d
    Post-Run UI Integration     :p2_4, after p2_3, 2d
    Implement Pagination        :p2_5, after p2_4, 1d
    
    section Phase 3: Challenges
    Daily Challenge System      :p3_1, after p2_2, 3d
    Weekly Challenge System     :p3_2, after p3_1, 3d
    Reward Distribution         :p3_3, after p3_2, 2d
    Friends Integration         :p3_4, after p2_5, 2d
    Privacy Settings            :p3_5, after p3_4, 1d
    
    section Phase 4: Testing
    Load Testing                :p4_1, after p3_3, 2d
    Cross-Platform Testing      :p4_2, after p4_1, 2d
    Mobile Optimization         :p4_3, after p4_2, 2d
    Community Beta Testing      :p4_4, after p4_3, 3d
    Security Audit              :p4_5, after p4_4, 2d
```

## Reward Structure

```mermaid
graph TD
    subgraph "GLOBAL RANK REWARDS"
        R1[Top 10<br/>Ethereal Champion Trail<br/>Animated Effect<br/>Gold Glow in UI]
        R2[Top 100<br/>Void Crystal Hull<br/>Pulsing Effect<br/>Silver Glow in UI]
        R3[Top 1000<br/>Chromatic Fade Pattern<br/>Color-Shifting<br/>Bronze Glow in UI]
    end
    
    subgraph "CHALLENGE REWARDS"
        C1[Daily Top 100<br/>Exclusive Daily Skin<br/>24h Only<br/>Rotating Set]
        C2[Weekly Top 10<br/>Hall of Champions<br/>Permanent Emblem<br/>Terminal Feature]
    end
    
    PLAYER[Player Performance] --> |Global Rank| GLOBAL
    PLAYER --> |Challenge Rank| CHALLENGE
    
    GLOBAL --> R1
    GLOBAL --> R2
    GLOBAL --> R3
    
    CHALLENGE --> C1
    CHALLENGE --> C2
    
    R1 --> COSMETICS[Cosmetic Unlocks<br/>Applied to Ship]
    R2 --> COSMETICS
    R3 --> COSMETICS
    C1 --> COSMETICS
    C2 --> COSMETICS
    
    style PLAYER fill:#4A90E2
    style R1 fill:#FFD700
    style R2 fill:#C0C0C0
    style R3 fill:#CD7F32
    style C1 fill:#F5A623
    style C2 fill:#BD10E0
    style COSMETICS fill:#50C878
```

## Success Metrics Dashboard

```mermaid
graph LR
    subgraph "Week 1 Targets"
        W1_1[20%+ View Boards]
        W1_2[10%+ Submit Score]
        W1_3[5%+ Try Daily]
    end
    
    subgraph "Month 1 Targets"
        M1_1[35%+ Engagement]
        M1_2[15%+ Daily Challenge]
        M1_3[10%+ Weekly Challenge]
    end
    
    subgraph "Quarter 1 Targets"
        Q1_1[Top 1000 Filled]
        Q1_2[50%+ Players Submitted]
        Q1_3[500+ Daily Participants]
    end
    
    LAUNCH[System Launch] --> WEEK1
    WEEK1 --> W1_1
    WEEK1 --> W1_2
    WEEK1 --> W1_3
    
    W1_3 --> MONTH1
    MONTH1 --> M1_1
    MONTH1 --> M1_2
    MONTH1 --> M1_3
    
    M1_3 --> QUARTER1
    QUARTER1 --> Q1_1
    QUARTER1 --> Q1_2
    QUARTER1 --> Q1_3
    
    Q1_3 --> SUCCESS[Mature Competitive Scene]
    
    style LAUNCH fill:#4A90E2
    style SUCCESS fill:#7ED321
```

## Cost Scaling Estimates

```mermaid
graph TD
    subgraph "User Base Growth"
        U1[1,000 Users<br/>50k API calls/day<br/>2 GB Database<br/>512 MB Cache<br/>$20-30/month]
        U2[5,000 Users<br/>250k API calls/day<br/>10 GB Database<br/>2 GB Cache<br/>$50-80/month]
        U3[10,000 Users<br/>500k API calls/day<br/>20 GB Database<br/>4 GB Cache<br/>$100-150/month]
        U4[50,000 Users<br/>2.5M API calls/day<br/>100 GB Database<br/>16 GB Cache<br/>$300-500/month]
        U5[100,000 Users<br/>5M API calls/day<br/>200 GB Database<br/>32 GB Cache<br/>$600-1000/month]
    end
    
    START[Launch] --> U1
    U1 --> |Growth| U2
    U2 --> |Growth| U3
    U3 --> |Growth| U4
    U4 --> |Growth| U5
    
    U1 -.->|Free Tier Available| FIREBASE[Firebase Spark:<br/>50k reads/day<br/>20k writes/day<br/>FREE]
    
    U2 -.-> OPTIMIZATION[Optimization Strategy:<br/>• Redis Cache<br/>• Regional Servers<br/>• Pagination<br/>• Compression]
    
    U5 -.-> SCALING[Scaling Strategy:<br/>• Load Balancers<br/>• CDN Integration<br/>• Database Sharding<br/>• Regional Replication]
    
    style START fill:#4A90E2
    style U1 fill:#7ED321
    style U2 fill:#F8E71C
    style U3 fill:#F5A623
    style U4 fill:#BD10E0
    style U5 fill:#D0021B
    style FIREBASE fill:#50C878
    style OPTIMIZATION fill:#4A90E2
    style SCALING fill:#BD10E0
```

---

## Technical Notes

### Firebase Free Tier Limits
- **Spark Plan (Free):** 50k reads/day, 20k writes/day
- **Blaze Plan (Pay-as-you-go):** $0.06 per 100k reads, $0.18 per 100k writes

### Redis Cache Configuration
- Top 1000 entries cached per category
- 5-minute TTL (Time To Live)
- Significantly reduces database load
- Estimated 70-80% reduction in API calls

### Regional Server Distribution
- **NA:** North America (AWS us-east-1)
- **EU:** Europe (AWS eu-west-1)
- **ASIA:** Asia Pacific (AWS ap-southeast-1)
- **OCE:** Oceania (AWS ap-southeast-2)
- Regional replication increases costs ~30%

### Anti-Cheat Statistical Thresholds
- **Normal Range:** Within 3σ (standard deviations) of mean
- **Suspicious:** 3σ to 5σ deviation → Flagged for manual review
- **Obvious Cheat:** >5σ deviation → Automatic rejection + ban

---

**End of Architecture Diagrams**

