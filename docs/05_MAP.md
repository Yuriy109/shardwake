# Shardwake — Map System

The world is built from handcrafted biome modules.

Maps are not fully procedural.

Instead, each run assembles a new layout from predefined modules.

This provides replayability while keeping maps readable and balanced.

---

# Layout

The map contains 13 biome modules.

Example layout:

        A
     B  C  D

E  F  G  H  I

     J  K  L

        M

Every run randomizes module placement.

Players should never immediately know where they spawned.

---

# Biome Modules

Each module is handcrafted.

Modules contain:

- terrain
- enemies
- mini boss
- chests
- decorations
- portal spawn locations
- extraction locations
- events

Modules can rotate to increase variety.

---

# Biome Types

Current planned biomes:

- Forest
- Ruins
- Swamp
- Cemetery
- Snow
- Crystal Cavern
- Bandit Camp
- Spider Nest
- Forgotten Temple
- Flooded Village
- Ashlands
- Ancient Tower
- Corrupted Grove

Each biome has unique enemies and mechanics.

---

# Exploration

Players should never know:

- where other players spawned
- where portals will activate
- where the best loot is

Exploration is always rewarded.

---

# Mini Bosses

Every biome contains one mini boss.

Mini bosses:

- protect valuable loot
- increase biome identity
- require different strategies

They should rely on mechanics rather than massive health pools.

---

# Portals

Portal locations are predefined.

Activation is random.

When a player discovers a portal location, it becomes permanently marked on the minimap.

---

# Hellgate

After the normal match ends, Hellgate becomes available.

Players who wish to continue may enter.

Hellgate contains:

- stronger enemies
- higher loot quality
- increased PvP pressure
- limited escape opportunities

Hellgate is optional.

Extracting before Hellgate is always a valid choice.

---

# Design Goals

Maps should encourage:

- exploration
- risk assessment
- memorable encounters
- player interaction

Players should feel lost during their first runs but gradually learn each biome without memorizing the entire map.

# Map Biomes (MVP)

The first version of the game uses **13 handcrafted biome modules**.

Each match consists of:

- 13 biome modules
- fixed overall layout
- biome modules are shuffled between slots
- modules may rotate if connector points remain compatible
- each module contains:
  - enemy packs
  - loot spawn points
  - chest spawn points
  - decoration
  - mini boss spawn point
  - extraction marker candidates
  - Hellgate marker (optional)

The map layout:

        A

     B  C  D

E  F  G  H  I

     J  K  L

        M

The center (G) is NOT the highest loot location.

It is simply a crossroads connecting many paths.

Players should never feel forced to rush the center.

High-risk content appears later through Hellgates.

---

## Biomes

### A — Ancient Grove

Theme:
Ancient magical forest.

Enemies:
- Wolves
- Forest Spirits
- Living Roots

Mini Boss:
Ancient Treant

---

### B — Bandit Road

Theme:
Broken trade road with abandoned wagons.

Enemies:
- Bandits
- Bandit Archers
- Scouts

Mini Boss:
Bandit Captain

---

### C — Broken Shrine

Theme:
Ancient ruined temple.

Enemies:
- Cultists
- Temple Guardians
- Fanatics

Mini Boss:
Shrine Keeper

---

### D — Frost Camp

Theme:
Frozen military camp.

Enemies:
- Ice Goblins
- Frost Wolves
- Frost Shamans

Mini Boss:
Frost Brute

---

### E — Goblin Den

Theme:
Goblin settlement.

Enemies:
- Goblins
- Spear Goblins
- Goblin Shamans

Mini Boss:
Goblin Chief

---

### F — Old Cemetery

Theme:
Forgotten cemetery.

Enemies:
- Skeletons
- Zombies
- Ghosts

Mini Boss:
Grave Warden

---

### G — Shard Crossing

Theme:
Central crossroads built around a giant crystal shard.

Enemies:
- Corrupted Guards
- Shard Creatures

Mini Boss:
Shard Sentinel

---

### H — Swamp Ruins

Theme:
Ancient ruins inside poisonous swamps.

Enemies:
- Slimes
- Swamp Creatures
- Poison Frogs

Mini Boss:
Swamp Hag

---

### I — Crystal Hollow

Theme:
Crystal cave.

Enemies:
- Crystal Beetles
- Crystal Golems
- Crystal Bats

Mini Boss:
Crystal Guardian

---

### J — Ember Gate

Theme:
Burned fortress entrance.

Enemies:
- Fire Imps
- Ash Hounds

Mini Boss:
Ember Knight

Possible Hellgate spawn.

---

### K — Spider Nest

Theme:
Huge underground spider cave.

Enemies:
- Spiders
- Venom Spiders
- Cocoon Hatchlings

Mini Boss:
Broodmother

---

### L — Fallen Watchtower

Theme:
Destroyed watchtower.

Enemies:
- Crossbow Bandits
- Harpies

Mini Boss:
Tower Captain

---

### M — Sunken Village

Theme:
Flooded abandoned village.

Enemies:
- Drowned
- Water Spirits
- Fisher Zombies

Mini Boss:
Drowned Elder

---

# Match Flow

0–8 min

Normal exploration.

Loot.

PvPvE.

Risk management.

8 min

Hellgates appear.

Players may ignore them or enter.

Hellgate contains:

- stronger monsters
- better loot
- higher risk
- mini boss

Hellgate lasts maximum 3 minutes.

After that it collapses.

Normal world also begins collapsing near the end of the match.

Only extraction portals remain. мини-босс не всегда появляется.

Mini bosses do not always spawn.

Each biome has a mini boss spawn point, but the actual mini boss spawn chance should be around 40–50% for MVP.

This prevents every run from feeling predictable.