# Codex Engineering Handoff

This directory contains small repository-owned helpers for running bounded Codex development tasks. It does **not** contain credentials, user-specific Codex configuration or model settings.

## Start a local task

From the repository root:

```powershell
pwsh ./eng/codex/preflight.ps1
```

The preflight checks only the local development prerequisites and repository shape. It does not install software, modify Git state or change Codex settings.

On Windows it recommends the full verification gate:

```powershell
pwsh ./build/verify.ps1
```

On a non-Windows Codex environment it recommends:

```powershell
pwsh ./build/verify.ps1 -CompileOnly
```

`-CompileOnly` is deliberately weaker than the Windows gate because WinForms runtime smoke tests are not executed. GitHub Actions remains the final Windows merge gate.

To run verification immediately after the preflight succeeds:

```powershell
pwsh ./eng/codex/preflight.ps1 -RunVerification
```

## Create a Codex task

Use the GitHub issue template **Codex development task**. A good task contains:

1. one observable goal;
2. the smallest relevant project/component scope;
3. an existing pattern to follow when one exists;
4. explicit out-of-scope items;
5. acceptance criteria and important edge cases;
6. the strategic decision boundary.

Codex should read the root `AGENTS.md` and the nearest scoped `AGENTS.md` before implementation.

## Recommended working pattern

```text
Issue / bounded prompt
        |
        v
Read AGENTS.md + roadmap/spec
        |
        v
Small implementation slice
        |
        v
Focused behavioral checks
        |
        v
build/verify.ps1
        |
        v
Pull request + Windows CI
        |
        v
Merge only when explicitly authorized
```

## What belongs here

Suitable repository-owned Codex engineering helpers include:

- preflight/verification wrappers;
- task templates;
- non-secret environment diagnostics;
- repeatable repository checks;
- documentation that keeps agent and human workflows aligned.

Do not store API keys, login tokens, MCP credentials, workstation paths, personal `config.toml` files or machine-specific sandbox policy in this directory.
