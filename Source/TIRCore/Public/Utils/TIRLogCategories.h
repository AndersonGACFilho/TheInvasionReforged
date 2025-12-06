#pragma once

#include "CoreMinimal.h"

/**
 * TIR Log Category Declarations
 *
 * Centralized logging categories for the TIR project.
 *
 * Verbosity parameters:
 * - Second param: Default runtime verbosity (what shows by default)
 * - Third param: Compile-time maximum verbosity (what gets compiled in)
 *
 * Using 'Warning' as compile-time max for shipping builds prevents
 * verbose logs from bloating the executable. Change to 'All' during development
 * if you need Verbose/VeryVerbose logs.
 *
 * Usage:
 * - UE_LOG(LogTIRCore, Log, TEXT("Message"));
 * - UE_LOG(LogTIRCombat, Warning, TEXT("Something suspicious"));
 *
 * Runtime filtering via ini:
 * - [Core.Log] LogTIRCore=Warning
 */

// Core System - General-purpose logging
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRCore, Log, All);

// Combat System - Damage, weapons, projectiles
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRCombat, Log, All);

// Movement System - Actor movement, dashing, physics
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRMovement, Log, All);

// AI System - Enemy behavior, decision making, pathfinding
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRAI, Log, All);

// Progression System - XP, leveling, unlocks
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRProgression, Log, All);

// Spawning System - Object pooling, enemy spawning, wave management
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRSpawning, Log, All);

// UI System - HUD, menus, player-facing elements
TIRCORE_API DECLARE_LOG_CATEGORY_EXTERN(LogTIRUI, Log, All);