# GitHub automation

How Issues, GitHub Project V2 and automatic branch creation fit together.

Implemented by
[`.github/workflows/create-issue-branch.yml`](../.github/workflows/create-issue-branch.yml).

See also: [Git workflow](git-workflow.md) ·
[Development setup](development-setup.md)

## Project V2 behavior

Work is tracked as GitHub Issues on a personal **Project V2** board. A
development branch is created automatically once an eligible Issue reaches
the Project status:

```text
In Progress
```

GitHub Actions does **not** receive a repository event when the Status field
of a personal Project V2 item changes, so the workflow polls the project
every five minutes instead. It also runs on the `issues` events `labeled`,
`unlabeled`, `assigned`, `unassigned` and `edited`, which cover the case of
an Issue that is already `In Progress` but was still missing its assignee or
type label, and it can be started by hand from the Actions tab (`workflow_dispatch`).

A status change therefore takes up to five minutes to produce a branch. That
is a limitation of the event model, not a failure.

## Requirements for automatic branch creation

An Issue gets a branch when all of the following hold:

- exactly one development-type label
- exactly one assignee
- Project V2 status `In Progress`
- no existing branch for that Issue number

Supported development labels:

```text
feat
fix
refactor
perf
test
docs
chore
hotfix
```

A missing label or assignee is logged and skipped. More than one of either is
logged as a warning and skipped, because the branch name would be ambiguous.

## Branch creation behavior

Development:

```text
dev/<type>/<assignee>/<issue-number>-<task-name>
```

Hotfix:

```text
hotfix/<assignee>/<issue-number>-<task-name>
```

`<task-name>` comes from the Issue title with any leading `[TAG]` prefix
removed, accents stripped, lowercased, and every run of non-alphanumeric
characters collapsed into `-`.

If the Issue already has a branch:

```text
NO-OP
```

An existing branch is never renamed, whatever changed on the Issue. See
[branch immutability](git-workflow.md#branch-immutability).

Branches are created with the `createLinkedBranch` mutation, so they are
linked to the Issue and appear under the Issue's **Development** section
rather than only in the branch list.

## Non-development Issues

Design and art Issues live on the same Project board, for example:

```text
design
art
```

With no supported development-type label they get no branch, and the
workflow logs the skip and moves on. This is expected behavior, not an
error.

## GitHub Actions configuration

```text
Settings
└── Secrets and variables
    └── Actions
```

### Required repository variable

```text
PROJECT_NUMBER=<project-number>
```

The number is the last path segment of the project URL:

```text
https://github.com/users/AndersonGACFilho/projects/3
```

which means:

```text
PROJECT_NUMBER=3
```

The workflow fails fast when this variable is missing or not a positive
integer.

### Optional repository variables

| Variable             | Default       | Purpose                               |
|----------------------|---------------|---------------------------------------|
| `DEV_BASE_BRANCH`    | `master`      | Base for `dev/*` branches             |
| `HOTFIX_BASE_BRANCH` | `master`      | Base for `hotfix/*` branches          |
| `IN_PROGRESS_STATUS` | `In Progress` | Project status that triggers creation |

Once a dedicated development branch exists, set:

```text
DEV_BASE_BRANCH=develop
```

Then:

```text
feat
fix
refactor
perf
test
docs
chore
    ↓
develop
```

and:

```text
hotfix
   ↓
master
```

### Required repository secret

```text
PROJECT_TOKEN
```

The workflow authenticates with this token to:

- read Project V2
- inspect Issue metadata
- create repository branches
- link branches to Issues

The default `GITHUB_TOKEN` cannot read a personal Project V2, which is why a
separate token is needed. Create it in GitHub and store it as a repository
secret; it is never written into the repository, and no token value belongs
in any file here.

## Issue lifecycle

```text
Issue created
     ↓
    Todo
     ↓
define requirements
     ↓
assign developer
     ↓
apply development label
     ↓
In Progress
     ↓
GitHub Action
     ↓
validate Issue
     ↓
create branch
     ↓
link branch to Issue
     ↓
development
     ↓
Pull Request
     ↓
merge
     ↓
Done
```

Pure design and art Issues follow the same statuses without ever receiving a
Git branch.
