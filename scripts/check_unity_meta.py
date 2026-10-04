#!/usr/bin/env python3
"""Validate Unity .meta integrity under Assets/.

Unity keeps the GUID that every asset reference points at inside a sibling
.meta file. A missing .meta breaks the reference for everyone who pulls the
asset; an orphan .meta points at an asset that is no longer there.

This script only reports. It never creates, deletes or rewrites a .meta file,
because a regenerated .meta carries a new GUID and silently breaks existing
references.

Standard library only, so it runs the same on Arch Linux and on Windows.
"""

from __future__ import annotations

import os
import subprocess
import sys
from pathlib import Path

META_SUFFIX = ".meta"

REPO_ROOT = Path(__file__).resolve().parent.parent
ASSETS_DIR = REPO_ROOT / "Assets"

# Names Unity itself ignores. They are not assets, so they must not carry a
# .meta companion.
HIDDEN_NAMES = {"cvs"}
HIDDEN_SUFFIXES = (".tmp",)

# Directories Unity imports as a single asset: the directory gets one .meta
# and nothing inside it does.
OPAQUE_DIR_SUFFIXES = (
    ".androidlib",
    ".bundle",
    ".framework",
    ".plugin",
    ".xcframework",
)


def is_hidden(name: str) -> bool:
    """Return True when Unity ignores an asset with this name."""
    lowered = name.lower()

    return (
        name.startswith(".")
        or name.endswith("~")
        or lowered in HIDDEN_NAMES
        or lowered.endswith(HIDDEN_SUFFIXES)
    )


def is_opaque_dir(name: str) -> bool:
    """Return True when Unity imports this directory as a single asset."""
    return name.lower().endswith(OPAQUE_DIR_SUFFIXES)


def relative(path: Path) -> str:
    """Repository-relative path with forward slashes, on every platform."""
    return path.relative_to(REPO_ROOT).as_posix()


def collect(assets_dir: Path) -> tuple[list[Path], list[Path]]:
    """Walk Assets/ and return (assets needing a .meta, .meta files found).

    The Assets directory itself is excluded: Unity does not give it a .meta.
    """
    assets: list[Path] = []
    metas: list[Path] = []

    for dirpath, dirnames, filenames in os.walk(assets_dir):
        current = Path(dirpath)

        descend: list[str] = []

        for name in sorted(dirnames):
            if is_hidden(name):
                continue

            assets.append(current / name)

            if not is_opaque_dir(name):
                descend.append(name)

        # Prune in place so os.walk skips hidden and opaque directories.
        dirnames[:] = descend

        for name in sorted(filenames):
            if name.startswith("."):
                continue

            if name.endswith(META_SUFFIX):
                metas.append(current / name)
                continue

            if is_hidden(name):
                continue

            assets.append(current / name)

    return assets, metas


def git_ignored(paths: list[Path]) -> set[Path]:
    """Return the subset of paths Git is configured to ignore.

    Locally ignored content is not part of the repository, so it cannot break
    another contributor's checkout. When Git is unavailable the check simply
    runs over everything.
    """
    if not paths:
        return set()

    payload = "".join(f"{relative(path)}\0" for path in paths)

    try:
        result = subprocess.run(
            ["git", "check-ignore", "-z", "--stdin"],
            cwd=REPO_ROOT,
            input=payload,
            capture_output=True,
            text=True,
        )
    except (OSError, ValueError):
        return set()

    # 0: some paths matched. 1: none matched. Anything else: not a usable
    # repository, so fall back to checking every path.
    if result.returncode not in (0, 1):
        return set()

    return {
        REPO_ROOT / line
        for line in result.stdout.split("\0")
        if line
    }


def main() -> int:
    if not ASSETS_DIR.is_dir():
        print(f"ERROR: Assets directory not found: {relative(ASSETS_DIR)}")
        return 1

    assets, metas = collect(ASSETS_DIR)

    ignored = git_ignored(assets + metas)

    asset_paths = {path for path in assets if path not in ignored}
    meta_paths = {path for path in metas if path not in ignored}

    missing = sorted(
        (
            Path(f"{path}{META_SUFFIX}")
            for path in asset_paths
            if Path(f"{path}{META_SUFFIX}") not in meta_paths
        ),
        key=relative,
    )

    orphans = sorted(
        (
            path
            for path in meta_paths
            if Path(str(path)[: -len(META_SUFFIX)]) not in asset_paths
        ),
        key=relative,
    )

    for path in missing:
        print(f"ERROR: Missing meta file: {relative(path)}")

    for path in orphans:
        print(f"ERROR: Orphan meta file: {relative(path)}")

    if not missing and not orphans:
        print(
            f"Unity meta files are consistent "
            f"({len(asset_paths)} assets checked)."
        )
        return 0

    print()
    print(
        f"Unity meta validation failed: "
        f"{len(missing)} missing, {len(orphans)} orphan."
    )
    print("Open the project in Unity so it imports the assets and writes the")
    print("missing meta files, and delete an orphan meta only together with")
    print("the asset it belonged to. Never hand-create a meta file.")

    return 1


if __name__ == "__main__":
    sys.exit(main())
