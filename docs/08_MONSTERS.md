# Shardwake — Monsters

Monsters are not simple HP targets.

Every biome should have a distinct combat identity.

The player should win PvE encounters through:

- movement
- dodging
- target priority
- skill timing
- understanding enemy roles

---

# Monster Data

Every monster has:

- monster type
- display name
- faction
- difficulty
- max health
- movement speed
- aggro range
- attack damage
- attack speed
- attack range
- attack windup
- attack kind
- loot source

Attack speed and attack range are mandatory because Shardwake contains both melee and ranged monsters.

---

# Attack Kinds

Current MVP attack kinds:

- melee cone
- ranged shot
- leap
- ground zone

Every dangerous attack should be telegraphed.

---

# Biome Identity

Forest:
- wolves pressure movement
- spirits attack from range
- living roots create zones

Bandit Road:
- melee bandits pressure close range
- archers punish open movement
- scouts are fast flankers

Broken Shrine:
- cultists cast from range
- guardians are durable elites
- fanatics punish bad positioning

Frost Camp:
- frost enemies slow and pressure dodges

Goblin Den:
- goblins swarm
- spear goblins control range
- shamans attack from behind

Old Cemetery:
- undead are slower but harder to clear safely

Swamp Ruins:
- poison and slows create positional pressure

Crystal Hollow:
- armored enemies and ranged pressure

Ember Gate:
- fire enemies use burst and aggressive movement

Spider Nest:
- fast swarm enemies and venom pressure

Fallen Watchtower:
- ranged enemies and aerial threats

Sunken Village:
- slow pressure, ranged water magic and undead control

---

# Design Rule

Enemy variety is more important than enemy HP.

A new monster should create a new combat problem, not just bigger numbers.
