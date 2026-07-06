# Shardwake — Monster AI Behaviors

Monster combat must not feel like simple auto-attack target dummies.

Each monster uses a behavior archetype. The archetype controls how the monster moves, attacks and reacts to the player.

---

# Behavior Archetypes

## Bruiser

Default melee enemy.

- walks toward player
- attacks in melee range
- simple but readable

## Swarm

Fast weak enemy.

- closes distance aggressively
- low HP
- dangerous in groups

## Charger

Leaping or dashing enemy.

- tries to reach the player quickly
- uses leap-style attacks
- punishes players who stand still

## Ranged Kiter

Ranged enemy.

- keeps distance
- retreats if the player comes too close
- strafes while attacking

## Caster

Magic/ranged zone enemy.

- stays at medium range
- uses ground zones or projectiles
- creates positioning pressure

## Support Healer

Support enemy.

- tries to stay behind allies
- heals damaged allied monsters
- should be killed early

## Tank

Slow durable enemy.

- high HP
- lower mobility
- can shield itself
- blocks space

## Ambusher

Fast aggressive enemy.

- closes distance quickly
- attacks often
- punishes low awareness

---

# Design Rules

Every biome should feel different through enemy behavior, not only visuals.

Examples:

- forest = roots, wolves, support spirits
- bandits = ranged pressure and scouts
- cemetery = slow undead and ghosts
- swamp = poison and slows
- frost = slows and zone control
- spiders = swarm and poison
- hell = fire, fast pressure and burst

Enemies should be readable on mobile.

Strong attacks need telegraphs.

Mini-bosses should extend these archetypes rather than rely only on huge HP.
