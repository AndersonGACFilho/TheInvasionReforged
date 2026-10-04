#!/usr/bin/env python3
"""Fail when generated or machine-specific Unity files are tracked by Git.

Unity regenerates Library/, Temp/, obj/ and friends on every machine, and
UserSettings/ plus the IDE directories hold per-developer state. Tracking any
of them produces constant conflicts and leaks local paths into the history.

The check reads the Git index rather than the filesystem: a generated
directory sitting in the working tree is expected and fine, a tracked one is
not. Nothing is modified or deleted.

Standard library only, so it runs the same on Arch Linux and on Windows.
"""

from __future__ import annotations

import subprocess
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent

# Unity regenerates these at the repository root.
GENERATED_ROOT_DIRS = (
    "build",
    "builds",
    "library",
    "logs",
    "temp",
    "usersettings",
)

# Build output, which may sit next to any generated project file.
GENERATED_ANY_DIRS = ("obj",)

# Per-developer editor state.
IDE_DIRS = (".idea", ".vscode")

# Solution and project files Unity rewrites whenever it reimports scripts.
GENERATED_FILE_SUFFIXES = (
    ".booproj",
    ".csproj",
    ".pidb",
    ".sln",
    ".suo",
    ".unityproj",
    ".user",
    ".userprefs",
)


def tracked_files() -> list[str]:
    """Every path in the Git index, with forward slashes."""
    result = subprocess.run(
        ["git", "ls-files", "-z"],
        cwd=REPO_ROOT,
        capture_output=True,
        text=True,
    )

    if result.returncode != 0:
        message = result.stderr.strip() or "git ls-files failed"
        raise RuntimeError(message)

    return [path for path in result.stdout.split("\0") if path]


def classify(path: str) -> str | None:
    """Return why a tracked path is forbidden, or None when it is allowed."""
    segments = path.split("/")
    lowered = [segment.lower() for segment in segments]

    if lowered[0] in GENERATED_ROOT_DIRS:
        return "Generated Unity file is tracked"

    if any(segment in GENERATED_ANY_DIRS for segment in lowered[:-1]):
        return "Generated Unity file is tracked"

    if any(segment in IDE_DIRS for segment in lowered[:-1]):
        return "IDE-specific file is tracked"

    if lowered[-1].endswith(GENERATED_FILE_SUFFIXES):
        return "Generated project file is tracked"

    return None


def main() -> int:
    try:
        paths = tracked_files()
    except (OSError, RuntimeError) as error:
        print(f"ERROR: Cannot read the Git index: {error}")
        return 1

    offenders = sorted(
        (path, reason)
        for path in paths
        if (reason := classify(path)) is not None
    )

    if not offenders:
        print(
            f"No generated Unity files are tracked "
            f"({len(paths)} tracked paths checked)."
        )
        return 0

    for path, reason in offenders:
        print(f"ERROR: {reason}: {path}")

    print()
    print(f"Generated file validation failed: {len(offenders)} tracked path(s).")
    print("Untrack each path with 'git rm --cached <path>' (add -r for a")
    print("directory) and make sure .gitignore covers it. The files stay on")
    print("disk; only the Git index changes.")

    return 1


if __name__ == "__main__":
    sys.exit(main())
