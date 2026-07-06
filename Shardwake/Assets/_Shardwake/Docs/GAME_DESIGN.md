# Shardwake Game Design

## One-line concept

Shardwake is a mobile-first, top-down, stylized fantasy extraction dungeon game inspired by Dark and Darker, presented like a colorful mobile action RPG.

## Core fantasy

Enter a dangerous Shard, fight monsters and rival Seekers, collect valuable loot, then escape through one of several portals before the Shard collapses.

## What this is

- Mobile extraction RPG.
- Top-down / 2.5D dungeon combat.
- PvPvE, not battle royale.
- Gear and carried loot create risk.
- Short sessions, readable combat, strong touch controls.
- Bright cartoon/stylized fantasy presentation.

## What this is not

- Not a last-player-standing game.
- Not a big MMORPG.
- Not a Diablo-style endless loot grinder.
- Not an AI-driven rules game.
- Not a grim realistic dungeon crawler visually.

## Mobile combat direction

- Basic attack is automatic when a valid enemy is in range.
- Player controls positioning, dodge/dash, skills, and consumables.
- Target control should be assisted, readable, and forgiving.
- Expected combat buttons:
  - Dash / dodge
  - Skill 1
  - Skill 2
  - Skill 3
  - Consumable 1
  - Consumable 2

## Extraction rules

- Portals are scattered across the map.
- Portals are not all available immediately.
- Portal color can communicate zone difficulty, risk, or extraction type.
- Player can extract with low loot or stay longer for better loot.
- Death does not need to show a dramatic "lost loot" screen every time, but the rules should preserve extraction risk.

## Dungeon direction

- Rooms and corridors, not open arenas.
- Doors, chokepoints, and sightline breaks.
- Chests placed in dangerous rooms.
- Monsters guard loot and routes.
- Later: AI rival Seeker as a PvP placeholder before network multiplayer.

## Map generation direction

- A match map is assembled from biome modules.
- The center is a fixed difficult landmark so players can learn one high-risk focal point.
- Outer biome modules are shuffled between slots each run.
- Modules can rotate in 90-degree steps so the same biome can create different approaches.
- Biomes should eventually become authored module prefabs with spawn points, portals, loot points, doors, hazards, and nav data.
- The server should own the seed/layout in multiplayer; clients only render and predict against replicated state.

## Online authority

- Server owns match rules, loot, mob scaling, extraction, and player inventory outcomes.
- Client shows controls, prediction, UI, animation, and VFX.
- Prototype code can simulate these systems locally, but should stay shaped like server-authored match state.
