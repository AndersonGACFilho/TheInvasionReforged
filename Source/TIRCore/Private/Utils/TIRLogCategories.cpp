#include "Utils/TIRLogCategories.h"

/**
 * TIR Log Category Definitions
 *
 * Each DEFINE_LOG_CATEGORY must match a corresponding DECLARE_LOG_CATEGORY_EXTERN.
 *
 * Usage:
 * - Use UE_LOG(LogTIRCore, Log, TEXT("Message")) to log messages.
 * - Filter logs in the Output Log by category name.
 * - Control verbosity via ini: [Core.Log] LogTIRCore=Warning
 */

DEFINE_LOG_CATEGORY(LogTIRCore);
DEFINE_LOG_CATEGORY(LogTIRCombat);
DEFINE_LOG_CATEGORY(LogTIRMovement);
DEFINE_LOG_CATEGORY(LogTIRAI);
DEFINE_LOG_CATEGORY(LogTIRProgression);
DEFINE_LOG_CATEGORY(LogTIRSpawning);
DEFINE_LOG_CATEGORY(LogTIRUI);