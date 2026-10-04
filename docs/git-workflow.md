# Git workflow

Commits, branches, local development and the lifetime of a branch.

See also: [Development setup](development-setup.md) ·
[GitHub automation](github-automation.md)

## Conventional Commits

```text
<type>(<scope>): <description>

<body>

Refs #<issue>
```

| Type       | Purpose                                           |
|------------|---------------------------------------------------|
| `feat`     | New functionality                                 |
| `fix`      | Bug fix                                           |
| `refactor` | Internal restructuring without changing behavior  |
| `perf`     | Performance improvement                           |
| `test`     | Test changes                                      |
| `docs`     | Documentation                                     |
| `chore`    | Tooling, repository configuration, or maintenance |

Example:

```text
feat(starfield): add deterministic cell generation

Generate stars deterministically from the world seed and cell coordinates.

Refs #8
```

The scope is the subsystem the change lands in — a module, a script, a
configuration area. The description is imperative and lowercase, and the body
explains why rather than restating the diff.

Running:

```bash
git commit
```

opens the [`.gitmessage`](../.gitmessage) template with that skeleton
prefilled, provided `commit.template` is configured (see
[Development setup](development-setup.md#git-configuration)).

`Refs #<issue>` links the commit to its Issue without closing it. Closing is
the Pull Request's job.

## Development branch convention

```text
dev/<type>/<assignee>/<issue-number>-<task-name>
```

Example:

```text
dev/feat/andersongacfilho/8-implement-deterministic-ecs-parallax-starfield
```

`<type>` is the same set as the commit types, `<assignee>` is the GitHub
login in lowercase, and `<task-name>` is the Issue title slugified.

## Hotfix branch convention

```text
hotfix/<assignee>/<issue-number>-<task-name>
```

Hotfix branches start from the configured production base branch (`HOTFIX_BASE_BRANCH`), not from the development base,
so an urgent fix does
not carry unreleased work with it. See
[GitHub automation](github-automation.md#optional-repository-variables).

## Branch immutability

**Once a branch is created for an Issue, its name is immutable.**

Changing any of:

- Issue title
- assignee
- development type label

must **not** rename the existing branch. The automation detects a branch for
the Issue number and does nothing.

This matters because:

- renaming breaks the local tracking of whoever has the branch checked out;
- it rewrites upstream configuration under a developer who is mid-task;
- IDE branch lists, stashes and run configurations go stale;
- the old name can be recreated by accident, leaving two branches for one
  Issue.

The Issue number is the stable identifier connecting the branch to the work
item. The rest of the branch name is a human-readable label that was accurate
when the branch was cut, and nothing more.

If a name is genuinely wrong, close the Issue and open a new one rather than
renaming the branch under an active checkout.

## Local development

```bash
git fetch origin
git switch dev/feat/andersongacfilho/8-implement-deterministic-ecs-parallax-starfield
```

Commit in small, reviewable steps. The pre-commit hooks run on each commit
and are fast by design; if one fails, it prints the offending paths and what
to do about them.

## Pull Request workflow

```text
Issue
→ In Progress
→ branch created
→ local development
→ commits
→ Pull Request
→ review
→ merge
→ Done
```

Use Issue-closing syntax in the **Pull Request description**, not in the
intermediate commits:

```text
Closes #8
```

An intermediate commit that says `Closes #8` closes the Issue the moment it
reaches the base branch, which is usually before the work is actually
reviewed. Intermediate commits use `Refs #8`; the Pull Request closes the
Issue once it merges.

## Branch lifecycle

A branch exists from the moment the Issue enters `In Progress` until its
Pull Request merges. After the merge, delete the remote branch and prune
locally:

```bash
git switch master
git pull
git fetch --prune
```

Move the Issue to `Done` as part of the merge. The automation decides by
looking at the branches that currently exist, so an Issue left in
`In Progress` with its branch deleted has its branch created again on the
next poll.
