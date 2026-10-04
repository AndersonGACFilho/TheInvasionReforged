# Development setup

Local environment setup and the repository rules a Unity project needs in
order to survive more than one machine.

See also: [Git workflow](git-workflow.md) ·
[GitHub automation](github-automation.md)

## Supported development environments

The repository is developed on **Arch Linux** and **Windows**, and everything
here is expected to work on both. The editor version is pinned:

```text
6000.2.8f1
```

Install it through Unity Hub. A different editor version will rewrite
`ProjectSettings/ProjectVersion.txt` and the serialized assets it touches.

## Git configuration

Run once per clone:

```bash
git config --local core.autocrlf false
git config --local core.eol lf
git config --local commit.template .gitmessage
```

`.gitattributes` is the source of truth for line endings. It pins every text
file to LF, so a Windows checkout and a Linux checkout produce byte-identical
files and Unity's serialized assets stop producing whole-file diffs. The
local settings above keep Git from second-guessing that on Windows.

### Case sensitivity

Linux filesystems are generally case-sensitive; Windows filesystems are
generally case-insensitive. A file that differs from another only by case
exists twice on Linux and collides on Windows, so **never create two paths
that differ only by case**:

```text
PlayerController.cs
playercontroller.cs
```

The `check-case-conflict` pre-commit hook rejects this before it reaches the
history. Renaming only the case of an existing file needs two commits (`git mv` to a temporary name, then to the final
one) for the same reason.

## Pre-commit

The hooks are configured in
[`.pre-commit-config.yaml`](../.pre-commit-config.yaml) and are deliberately
fast. Unity compilation, EditMode and PlayMode tests and builds are **not**
run on commit; that work belongs in CI.

### General repository checks

| Check                     | What it catches                               |
|---------------------------|-----------------------------------------------|
| `check-merge-conflict`    | Conflict markers left in a file               |
| `check-case-conflict`     | Filenames that collide on Windows             |
| `mixed-line-ending`       | CRLF sneaking into text files (fixed to LF)   |
| `trailing-whitespace`     | Trailing spaces                               |
| `end-of-file-fixer`       | Missing final newline                         |
| `check-added-large-files` | Binaries over 10 MB committed outside Git LFS |
| `check-yaml`              | Malformed workflow and pre-commit YAML        |

`check-yaml` is scoped to `.github/` and `.pre-commit-config.yaml` on
purpose. Unity serialized files are **not** plain YAML and must never be
parsed by a generic YAML hook.

### Unity-specific checks

| Script                                                                                | What it catches                                    |
|---------------------------------------------------------------------------------------|----------------------------------------------------|
| [`scripts/check_unity_meta.py`](../scripts/check_unity_meta.py)                       | Missing `.meta` files, orphan `.meta` files        |
| [`scripts/check_unity_generated_files.py`](../scripts/check_unity_generated_files.py) | Generated or machine-specific files tracked by Git |

Both use the Python standard library only, report without changing anything,
and can be run directly:

```bash
python scripts/check_unity_meta.py
python scripts/check_unity_generated_files.py
```

They exit `0` when the repository is clean and non-zero with one `ERROR:`
line per problem:

```text
ERROR: Missing meta file: Assets/Sprites/Player.png.meta
ERROR: Orphan meta file: Assets/Sprites/OldEnemy.png.meta
ERROR: Generated Unity file is tracked: Library/ArtifactDB
ERROR: IDE-specific file is tracked: .idea/workspace.xml
```

A missing `.meta` is fixed by opening the project in Unity and letting it
import the asset. Never hand-write one.

## Arch Linux setup

```bash
sudo pacman -S pre-commit git-lfs
git lfs install
pre-commit install
pre-commit run --all-files
```

## Windows setup

```powershell
winget install GitHub.GitLFS
py -m pip install pre-commit
git lfs install
pre-commit install
pre-commit run --all-files
```

The local hooks call `python`, so it must be on `PATH`. If only the `py`
launcher resolves, add the interpreter directory to `PATH` rather than
editing the hook entries, which are shared with Linux.

## Unity source control configuration

The project must serialize assets as text, otherwise scenes and prefabs are
binary blobs that cannot be reviewed or merged. Verify in the editor:

```text
Edit
└── Project Settings
    └── Editor
        └── Asset Serialization
            └── Mode: Force Text
```

This is already committed as `m_SerializationMode: 2` in
`ProjectSettings/EditorSettings.asset`; the check is for when a new machine
or a reimport changes it.

### Meta files

- Unity `.meta` files **must stay tracked**.
- A `.meta` holds the asset's GUID, and every reference in a scene, prefab or
  ScriptableObject points at that GUID rather than at the path.
- Deleting or regenerating a `.meta` mints a new GUID and silently breaks
  every reference to the asset. Move and rename assets from inside Unity so
  the `.meta` travels with them.

## Source-controlled Unity directories

```text
Assets/
Packages/
ProjectSettings/
```

## Generated directories

Unity and the IDEs regenerate these per machine. They must never be
committed:

```text
Library/
Temp/
Logs/
obj/
Build/
Builds/
UserSettings/
```

Per-developer editor state:

```text
.idea/
.vscode/
```

All of them are covered by [`.gitignore`](../.gitignore), and
`scripts/check_unity_generated_files.py` fails the commit if one is tracked
anyway.

## Git LFS

Large binary **source** assets belong in Git LFS, which stores a pointer in
the history and the payload out of band. The patterns already tracked in
[`.gitattributes`](../.gitattributes):

```text
*.psd
*.fbx
*.blend
*.wav
*.mp4
```

Inspect the configuration and what is actually stored in LFS:

```bash
git lfs track
git lfs ls-files
```

LFS is not free: it adds a fetch step and quota to every clone. Small
imported assets such as sprites, icons and short sound effects are better off
as ordinary Git objects. Reach for LFS when a file is a large authoring
source, not simply because it is binary.
