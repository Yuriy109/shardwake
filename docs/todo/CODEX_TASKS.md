# Codex Tasks — Next Large Refactor

Use these tasks when GPT-5.5 Codex limits return. Do them in order.

## 1. Replace greybox skill execution

Current `PlayerSkills` executes most skills as simple `OverlapSphere` damage.
Create reusable effect handlers for:

- frontal hit
- area hit
- dash hit
- projectile
- pull target toward caster
- knockback
- root/slow
- self buff
- heal/shield
- summon

Keep UI and MonoBehaviour view code separate from skill logic.

## 2. Introduce real loadout data

Create a runtime loadout object containing:

- weapon1
- weapon2
- selected 3 skill ids for each weapon
- selected passive for each weapon
- armor preset
- insured item ids

`ShardwakeSession` should consume this object instead of storing scattered fields.

## 3. Split ShardwakeSession

`ShardwakeSession` currently coordinates too much.
Extract systems:

- MatchTimer
- ExtractionManager
- DeathManager
- LootRunTracker
- WeaponLoadoutController
- ChronicleRequestBuilder

Keep `ShardwakeSession` as orchestration only.

## 4. Implement equipment runtime

Implement equipped slots:

- head
- chest
- hands
- legs
- feet
- weapon1
- weapon2
- ring1
- ring2
- amulet

Only armor contributes to equip load.

## 5. Implement portal schedule

Normal match duration: 720 seconds.
First portals: 360 seconds.
Hellgate entrance: 510 seconds.
Normal Shard collapse: 720 seconds.
Hellgate phase: up to 180 seconds.

## 6. Monster archetypes

Replace generic enemies with biome-specific archetypes:

- charger
- caster
- healer
- summoner
- tank
- ranged attacker
- swarm unit

Every dangerous attack needs a visible telegraph.

## Iteration 2 handoff — Weapon Loadout cleanup

Implemented before Codex returns:

- Added `WeaponPassiveDefinition`.
- Added `WeaponSkillLoadout` for selecting 3 active skills and 1 passive per weapon.
- Added `WeaponLoadout` with primary/secondary weapons and active weapon slot.
- Added `WeaponSlot` and `WeaponLoadoutRules`.
- Updated `WeaponDefinition` so passives are structured data, not plain strings.
- Updated `ShardwakeSession` to own a `WeaponLoadout` instead of loose selected/active weapon fields.
- Updated `PlayerSkills` to execute selected skills from the active weapon loadout.

Next Codex task:

Refactor skill execution into a separate `SkillExecutor` / `SkillEffectResolver` service so `PlayerSkills` does not contain effect logic. Keep `PlayerSkills` as input glue only.


---

# Iteration 3 Handoff — Equipment & Stat Application

Current state added before Codex returns:

- EquipmentLoadout component for armor preset and temporary stat bonuses.
- Equip load now affects player movement and roll through LoadEffects.
- Core stats now affect max HP, basic attack damage, basic attack cooldown, skill cooldown, and skill damage.
- Stamina now drives out-of-combat HP regeneration through PlayerRegeneration.
- Weapon scaling is centralized in WeaponScaling.

Next Codex task:

Implement real equipment-driven stat aggregation.

Requirements:

- Replace temporary serialized stat fields in EquipmentLoadout with actual equipped LootItem/EquipmentItem data.
- Aggregate affixes from armor, rings, amulet and weapons into CharacterStats.
- Keep EquipmentLoadout as the single source of equipped gear state.
- Keep ShardwakeSession free from inventory implementation details.
- Do not hardcode balance in UI or MonoBehaviour view scripts.

---

# Iteration 4 handoff — Inventory and equipment items

Implemented a first runtime link between loot and equipment.

Codex follow-up tasks:

1. Add a simple greybox equipment UI:
   - show backpack items
   - show equipment slot targets
   - allow equipping items from the backpack

2. Replace temporary serialized stat bonuses with real starter gear:
   - starter weapons
   - starter light/medium/heavy armor pieces
   - starter accessories empty by default

3. Split equipment data into data definitions later:
   - Weapon item definition
   - Armor item definition
   - Accessory item definition
   - Relic/material/consumable definitions

4. Keep rules from docs/PROJECT_RULES.md:
   - no gameplay logic in UI
   - no hardcoded balance inside view scripts
   - keep systems modular

---

# Iteration 5 Handoff — Monster Data Architecture

The project now has a first data-driven monster layer.

Added/updated:

- MonsterType
- MonsterFaction
- MonsterDifficulty
- MonsterAttackKind
- MonsterAttackDefinition
- MonsterDefinition
- MonsterDefinitions
- EnemyController now reads attack speed, attack range, aggro range, movement speed, HP and damage from MonsterDefinition
- GreyboxBootstrap now spawns biome-specific monsters instead of only Shardlings

Next Codex task:

Convert MonsterDefinitions from static code into ScriptableObject/data assets when the project is ready for content authoring.

Do not add complex monster AI yet.
Keep MVP enemies readable and simple.

## Iteration 6 Handoff — Status Effects + Damage Pipeline

Goal:
Continue from the new shared combat pipeline instead of implementing damage/status logic inside individual skills or monsters.

Already added:
- `DamageType`
- `DamageRequest`
- `DamageResult`
- `DamageCalculator`
- `StatusEffectType`
- `StatusEffectDefinition`
- `StatusEffectInstance`
- `StatusEffectController`
- shared shield absorption
- basic DOT support for burn/poison/bleed
- slow/root/stun movement support
- player skills now send damage requests and optional status effects
- monsters now send damage requests and can apply simple slow effects

Next Codex tasks:
1. Replace placeholder skill execution with per-effect handlers.
2. Add projectile support for ranged skills and ranged monsters.
3. Add proper AoE zones for ground effects.
4. Add hit team/faction filtering so player skills do not hit allies in duo mode.
5. Add UI icons/timers for active status effects.
6. Move all status effect magnitudes/durations into balance data.


---

# Iteration 7 Handoff — Skill Execution Cleanup

Implemented before Codex returns:

- Added `Runtime/Skills/SkillExecutionContext.cs`.
- Added `Runtime/Skills/SkillExecutor.cs`.
- Added `Runtime/Skills/SkillTargeting.cs`.
- Added `Runtime/Skills/SkillDamageTypeResolver.cs`.
- `PlayerSkills` is now input glue only:
  - reads selected active skill from `WeaponLoadout`
  - checks cooldown
  - sends execution to `SkillExecutor`
- Skill execution now supports reusable handling for:
  - healing
  - shields / guard zone
  - backstep / blink
  - dash hit
  - pull
  - knockback
  - AoE / frontal / ranged target filtering
  - damage pipeline + status effects

Next Codex task:

Replace the remaining greybox execution with real animation/cast timing:

- add skill cast time / windup support
- add projectile prefabs for ranged skills
- add trap objects
- add summon objects
- add visual telegraphs for player skills
- keep `SkillExecutor` data-driven and do not move logic back into `PlayerSkills`

---

# Iteration 8 Handoff — Monster AI Behaviors

Status: implemented by ChatGPT iteration.

Added:

- MonsterBehaviorType
- MonsterBehaviorTuning
- behavior field in MonsterDefinition
- behavior-based movement in EnemyController
- ranged kiting
- caster spacing
- support healing
- tank self-shield
- ambusher pressure
- poison / burn / frost status effects from specific monster types
- docs/10_MONSTER_AI.md

Next recommended Codex task:

Polish Monster AI into data-driven ScriptableObjects and add mini-boss definitions for each biome.


---

# Iteration 9 Handoff — Monster Movement Fix & Portal Channel

Implemented before Codex returns:

- Runtime-spawned monsters now receive the player target during greybox bootstrapping.
- `EnemyController` now has a fallback target resolver for scene-placed enemies.
- Extraction portals now require a short channel instead of instant extraction.
- Taking damage during the channel interrupts extraction.
- Portal channel state is kept inside `ExtractionPortal`, not UI.

Next Codex task:

Split extraction into `ExtractionManager`, `PortalState`, and `ExtractionChannel` so multiple portal types can exist later:
normal extraction portals, Hellgate entrances, and Hellgate exit portals.

## Iteration 10 handoff — Monster movement fix

- EnemyController now searches for PlayerController if no target was assigned.
- GreyboxBootstrap passes the player transform to spawned enemies.
- Aggro range is expanded for greybox testing so enemies visibly move during the prototype.
- Later, replace the greybox aggro shortcut with proper line-of-sight, sound, patrol and leash logic.


---

## Iteration 11 Handoff — Greybox Inventory/Equipment UI

Current greybox now includes a temporary inventory/equipment panel.

- Tab toggles the panel.
- E equips the first equipment item from backpack.
- Panel shows backpack slots, equipped slots, load category and core stats.

Next Codex task:
Replace the temporary debug panel with a proper Haven inventory/equipment UI:
- item cards
- equip/unequip buttons
- slot-based backpack grid
- compare equipped item vs backpack item
- mobile-friendly layout

---

# Iteration 12 Handoff — MVP Loop Bundle

AI Chronicle is disabled for now. Continue building the game without AI-dependent systems.

Implemented in this iteration:

- Normal extraction now requires channeling.
- Taking damage interrupts extraction.
- Hellgate entrance appears during late Shard phase.
- Queued Hellgate starts after the 12-minute Shard phase.
- Hellgate lasts up to 3 minutes.
- Hell extraction portals open after a short Hellgate delay.
- Expedition result now separates saved loot, dropped loot, and insurance returns.
- Basic in-memory Haven stash was added for extracted and returned items.
- Player death drops equipped items and backpack contents logically.
- Insurance returns insured items in the single-player greybox assumption.
- Mini-boss slots now spawn in biome modules with a chance.

Next Codex tasks:

1. Replace greybox in-memory stash with a real saved stash model.
2. Add mobile-friendly inventory and result screens.
3. Add dedicated Hellgate arena/transition instead of reusing the same map.
4. Add real mini-boss definitions instead of scaling existing monsters.
5. Add portal spawn balancing rules so extraction points are distributed fairly.
6. Add tests or debug validation for loot/drop/extraction flows.

## Iteration 13 handoff — Mobile HUD polish

Implemented a temporary mobile-first greybox HUD:

- top-left phase/objective panel
- player HP bar
- active weapon label
- loot/kills/alert line
- radial cooldown overlays for dash, weapon swap and active skills
- removed reliance on desktop-only status text for core playtest information

Next UI tasks for Codex:

1. Replace generated greybox UI with prefab-based mobile HUD.
2. Add proper skill icons instead of text labels.
3. Add portal and Hellgate minimap/edge markers.
4. Add backpack/equipment mobile screens.
5. Keep debug panels editor-only or development-build-only.

## Iteration 14 handoff — discovered-only portal markers

Current greybox behavior:
- Normal extraction portals, Hellgate entrances, and Hell extraction exits are not shown in HUD marker list until the player physically discovers them.
- Discovery happens by proximity to the portal/gate object.
- Discovered markers show closed/open state and approximate distance in the mobile HUD.
- Hellgate has two reusable entrances in greybox.

Next Codex task later:
- Replace the text marker list with a proper minimap/compass widget.
- Keep the same rule: markers appear only after discovery.
- Do not reveal undiscovered extraction or Hellgate locations.


# Post-MVP Codex Tasks

1. Replace greybox inventory panel with final mobile inventory UI.
2. Convert monster and weapon definitions to ScriptableObjects or external data assets.
3. Replace temporary summon/trap visuals with real VFX and prefabs.
4. Add real mini-boss mechanics per biome.
5. Add Android build validation and UI safe-area pass.

## Iteration 16 handoff — Haven stash and preparation loop

Implemented before Codex returns:

- Haven stash now supports `TryGet` and `TryTakeAt`.
- Haven screen now previews stored items.
- Haven screen has armor preset buttons: Light / Medium / Heavy.
- Haven screen can equip the first 3 stash items before a run.
- Equipping from backpack or stash recalculates player stats/combat values.

Next Codex tasks:

1. Replace greybox generated Haven UI with proper mobile screens.
2. Split weapon item equipment from combat weapon selection clearly.
3. Add real stash item selection instead of first-three quick buttons.
4. Persist stash through backend/account save later.

## Iteration 17 — Mobile Test Readiness Handoff

Status: prepared for first device test.

Current rules:
- Keep AI Chronicle disabled for MVP.
- Keep debug inventory hidden by default.
- Validate touch-only gameplay before adding more systems.

Next Codex tasks after mobile test:
1. Fix touch control issues discovered on device.
2. Replace greybox inventory debug panel with real mobile inventory UI.
3. Add mobile build profile and Android export checklist.
4. Profile FPS and reduce spawned objects if needed.
5. Add stronger visual feedback for discovered portals and Hellgates.


## Iteration 20 Handoff — Haven Hub

Implemented a greybox Haven hub layer:
- Expedition Gate tab
- Stash tab
- Merchant tab
- Tavern tab
- Job Board tab
- Shard Dust economy placeholder
- basic merchant sell/buy actions
- non-AI tavern rumors
- contract placeholder

Future Codex task:
Replace generated greybox Haven UI with proper mobile UI prefabs and persistent saved account state.

---

## Scale pass follow-up

Current MVP greybox scale:

- module size: 60 x 60 Unity units
- total map envelope: about 300 x 300 Unity units
- minimum corridor width: 6 units
- combat arena width: 15–25 units
- camera view radius: about 35–45 units

Future Codex task:
- keep all future art/environment work consistent with `MapScale.cs`
- do not hardcode map dimensions in UI, camera, spawners or gameplay systems
- make map scale tunable from one central place
