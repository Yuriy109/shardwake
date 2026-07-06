# Shardwake Prototype Notes

## Current playable loop

- Press Play in `Assets/Scenes/SampleScene.unity`.
- Preferred scene after the editor builder runs: `Assets/_Shardwake/Scenes/GreyboxShard.unity`.
- Runtime bootstrap creates a greybox Shard arena.
- Move with WASD in Editor or drag on a touch device.
- Move with WASD in Editor or the on-screen joystick.
- Basic attacks are automatic when enemies are in range; use Space or mouse click only as Editor debug input.
- Walk near gold chests to collect relic loot.
- Extraction portals are scattered across the map and become available during the run.
- Reach an available portal to extract with carried loot.
- Extraction, player death, and shard collapse now show an expedition result screen.
- The result screen includes kills, carried loot, and a mock Chronicle.
- Combat feedback includes hit flashes, damage numbers, a short attack arc, and loot pickup text.
- Loot now rolls power, affixes, rarity, and rare+ flavor text for the expedition result.
- Successful extractions add relic count and relic power to prototype Haven progress.
- The result screen has a `RUN AGAIN` button for fast loop testing.
- Expeditions now begin from a simple Haven panel with current city progress and an `ENTER SHARD` button.
- Haven now offers three loadouts before entry: Vanguard, Arcanist, and Wayfarer. They change health, movement, damage, cooldown, and attack reach.
- Rare unstable relics can trigger Shard Alert. In the prototype, Shard Alert empowers enemy damage across the map and is shown in HUD/results.
- Greybox map generator now builds a modular dungeon: a fixed hard center plus shuffled/rotated biome modules around it.
- Current biome modules include forest ruins, goblin den, old cemetery, swamp shrine, bandit road, lava gate, snow camp, and ancient grove.
- Enemies now telegraph melee attacks with a red danger marker before damage resolves.

## Online authority notes

- Shard Alert is modeled as session state so it can later become a server-authored match event.
- In production, the server decides when an unstable relic triggers alert, which mobs are empowered, and what rewards are kept.
- Clients should only render alert UI/VFX and consume replicated match state.

## Current mobile UX pass

- Left on-screen control: movement joystick.
- Basic attacks are automatic when enemies are in range.
- Right on-screen controls: dash, three skill slots, and two consumable slots.
- Player and enemies have world-space health bars.
- Keyboard and mouse remain available for quick Editor testing.

## Architecture rule

Runtime gameplay scripts live under `Assets/_Shardwake/Runtime` and are grouped by system. UI should display state only; deterministic game rules stay in gameplay modules.

## Next milestone

Turn the current code-defined biome modules into authored module prefabs/ScriptableObjects, then add doors, minimap readability, and biome-specific enemy identities.

## Generated asset workflow

- `Assets/_Shardwake/Editor/GreyboxSceneBuilder.cs` creates prototype prefabs, materials, and `GreyboxShard.unity`.
- It runs once automatically if `GreyboxShard.unity` is missing.
- It can be run manually from `Shardwake > Build Greybox Scene`; use this after generator changes to overwrite the generated greybox scene.
- Prototype Haven progress can be cleared from `Shardwake > Reset Haven Progress`.
