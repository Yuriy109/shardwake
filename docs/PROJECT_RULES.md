# Shardwake — Project Rules

These rules are mandatory for every contributor, AI agent, and future developer.

Breaking these rules requires an explicit design decision.

---

# Architecture

## 1. No God Objects

A single class must never control multiple gameplay systems.

Each system has one responsibility.

---

## 2. Small Files

Prefer files under 300 lines.

If a file becomes difficult to navigate, split it into smaller modules.

---

## 3. Modular Design

Gameplay systems must remain independent.

Examples:

- Combat
- Weapons
- Loot
- Inventory
- Equipment
- Extraction
- Hellgate
- AI
- Monsters
- UI

should never become tightly coupled.

---

## 4. UI Contains No Gameplay Logic

UI only displays information.

Gameplay decisions belong to game systems.

Never calculate damage, loot, cooldowns or game rules inside UI.

---

## 5. No Hardcoded Balance

All gameplay values must come from balance data.

Examples:

- weapon damage
- cooldowns
- enemy HP
- armor values
- loot chances
- portal timings

must never be hardcoded inside gameplay code.

---

## 6. Server Authority

The server is always authoritative.

The client never decides:

- loot
- damage
- extraction
- inventory
- player death
- item ownership

---

## 7. Data Driven Design

Whenever possible, new content should be added through data rather than code.

Adding a new weapon or monster should require minimal programming.

---

## 8. Every System Must Be Replaceable

A gameplay system should be replaceable without rewriting the rest of the project.

---

## Gameplay Rules

## 9. Every Mechanic Creates Choice

No mechanic should exist only for complexity.

Every mechanic must force meaningful decisions.

---

## 10. Risk Must Create Reward

The best rewards should require greater danger.

Safety should never outperform risk.

---

## 11. Death Must Matter

Death is part of the game.

Item loss creates tension.

Never remove this feeling.

---

## 12. Mobile First

Every feature must work comfortably on a phone.

Readability is more important than realism.

---

## 13. No Feature Creep

New mechanics are added only if they improve the core gameplay loop.

---

## AI Rules

AI never decides:

- balance
- stats
- rewards
- item power
- loot tables

AI is only used for flavor:

- Chronicles
- Item names
- Item stories
- NPC dialogue
- Tavern rumors

---

## Code Style

- Use clear naming.
- Avoid duplicated logic.
- Prefer composition over inheritance.
- Keep functions short.
- Keep systems isolated.
- Document important decisions.

---

## Before Adding Any Feature

Ask:

1. Does it improve the extraction loop?
2. Does it create meaningful choices?
3. Can it be understood on mobile?
4. Does it fit the world of Shardwake?
5. Can it remain modular?

If the answer is "no", redesign the feature.