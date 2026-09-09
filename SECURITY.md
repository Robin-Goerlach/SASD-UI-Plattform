# Security Policy

## 1. Scope

This policy covers the SASD UI Platform, its NuGet packages, samples, build scripts, documentation, and third-party adapters. Consuming business applications remain responsible for authentication, authorization, secrets, database permissions, and network security.

## 2. Security Objectives

- no secrets in UI state, logs, screenshots, or sample projects;
- secure defaults for file, shell, clipboard, drag-and-drop, and WebView functions;
- a traceable supply chain with lock files, SBOMs, and third-party notices;
- minimum adapter privileges and explicit trust boundaries;
- invalid or manipulated UI-state files must not force an unsafe startup state;
- error presentation separates user-friendly messages from technical details.

## 3. Reporting Vulnerabilities

Security issues must not initially be published as public issues. Until a dedicated security contact exists, reports are submitted internally to the responsible SASD-GmbH project owner. A report should include:

- affected version and package;
- reproducible steps;
- expected and actual effect;
- possible exploitation and impact;
- available mitigations or workarounds.

## 4. Response Classes

| Class | Example | Response |
| --- | --- | --- |
| Critical | Remote code execution, secret leakage, arbitrary shell execution | Immediate analysis, stop publication, prepare a hotfix |
| High | Bypassable path validation, unsafe WebView bridge | Prioritized fix before the next regular release |
| Medium | Sensitive technical details in logs, insufficient allowlist | Fix in the next maintenance release |
| Low | Hardening or documentation gap without direct exploitation | Planned backlog item |

## 5. Secure Component Rules

### Shell and Processes

- never pass unchecked user-controlled text to a shell or command interpreter;
- prefer direct process arguments over composed command lines;
- use explicit allowlists for file types, protocols, and executable targets;
- require understandable confirmation before risky external actions.

### WebView2

- default-deny for navigation and resources;
- origin allowlist;
- block downloads by default or handle them through controlled policy;
- no generic JavaScript-to-.NET bridge;
- use a restrictive Content Security Policy for local content wherever possible;
- disable developer tools in production unless explicitly required.

### Files and Drag-and-Drop

- normalize paths and validate them against allowed locations and file types;
- apply size and count limits before loading;
- no automatic execution or active-content interpretation;
- handle symbolic links, network paths, and unexpected file extensions deliberately.

### UI State

- do not store passwords, tokens, complete email contents, or other secrets;
- use `SchemaVersion` and safe migration;
- write atomically and create backups;
- invalid data causes reset or partial-state rejection, never an unsafe fallback.

## 6. Dependency Security

- centrally managed package versions;
- restore lock files;
- vulnerability and license scans in CI;
- no binary downloads with unclear licensing;
- reproducible release builds from a clean checkout;
- an SBOM for each release;
- evaluate signing and provenance once the technical foundation is mature.

## 7. Security Approval for Adapters

Every R2/R3 adapter package requires a separate review of:

- input and file boundaries;
- process and network access;
- native components and lifecycle;
- logging of sensitive content;
- update and exit strategy;
- known vulnerabilities and support status.
