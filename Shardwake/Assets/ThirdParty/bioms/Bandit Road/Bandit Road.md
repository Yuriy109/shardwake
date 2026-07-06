# Bandit Road

## Theme

A dangerous road controlled by organized bandits.

## Biome Gameplay

Bandit Road teaches:

- melee combat
- positioning
- recognizing enemy roles
- dodging heavy attacks

Unlike Ancient Grove, this biome contains no magic.
Every enemy relies only on physical combat.

Rule:
Each normal enemy has one unique combat role.

---

# Bandit Grunt

## Role

Basic Melee

## Difficulty

Easy

## Description

The most common bandit.

Poorly equipped but attacks aggressively.

## AI

- Runs directly toward the player.
- Attacks the nearest target.
- Never retreats.

## Basic Attack

Knife Slash

Fast melee attack.

## Ability

None

Grunt is intentionally simple.

## Required Animations

- Idle
- Walk
- Run
- Attack
- Death

## Optional Animations

- Hit / Flinch

## VFX

- dust
- small hit sparks

## SFX

- footsteps
- knife swing
- hit

---

# Bandit Rogue

## Role

Fast Assassin

## Difficulty

Medium

## Description

Fast and agile bandit.

Looks for openings instead of fighting head-on.

## AI

- Constantly circles around the player.
- Uses Dash Strike whenever possible.
- Quickly repositions after attacking.

## Basic Attack

Quick Dual Slash

## Ability

Dash Strike

Quickly dashes through the player.

Effects:

- medium damage
- very high mobility

After the dash, briefly pauses.

Cooldown:
5 seconds.

## Required Animations

- Idle
- Run
- Attack
- Dash
- Death

## Optional Animations

- Hit / Flinch

## VFX

- dust
- motion blur

## SFX

- quick footsteps
- fast slash

---

# Bandit Brute

## Role

Heavy Bruiser

## Difficulty

Medium

## Description

A huge heavily built bandit.

Slow but extremely dangerous.

## AI

- Slowly approaches the player.
- Uses Ground Smash whenever the player is close.
- Rarely changes target.

## Basic Attack

Heavy Punch

## Ability

Ground Smash

Slams the ground with both fists.

Effects:

- medium damage
- small shockwave
- short knockback

Cooldown:
8 seconds.

## Required Animations

- Idle
- Walk
- Punch
- Ground Slam
- Death

## Optional Animations

- Hit
- Stagger

## VFX

- dust
- ground crack
- impact wave

## SFX

- heavy footsteps
- ground impact
- deep punch

---

# Bandit Captain

## Role

Biome Boss

## Difficulty

Boss

## Description

Leader of the local bandit gang.

Uses tactics instead of brute force.

## AI

- Engages the player directly.
- Buffs nearby bandits.
- Becomes more aggressive at low HP.

## Basic Attack

Three-hit Combo

Fast melee combo.

## Ability 1

War Cry

Buffs nearby bandits.

Effects:

- increased movement speed
- increased attack speed

Duration:
6 seconds.

## Ability 2

Leap Strike

Jumps toward the player.

Creates a small impact wave on landing.

## Ability 3

Execution Strike

Powerful overhead attack.

Long wind-up.

Very high damage.

Easy to dodge if anticipated.

## Phase 2

Triggered at 50% HP.

Changes:

- attacks faster
- uses War Cry more often

## Phase 3

Triggered at 25% HP.

Stops supporting allies.

Focuses entirely on the player.

Movement speed increased.

Combo attacks become faster.

## Required Animations

- Idle
- Walk
- Run
- Combo Attack
- Leap
- Roar
- Heavy Attack
- Death

## Optional Animations

- Hit
- Taunt

## VFX

- dust
- shockwave
- buff aura

## SFX

- roar
- heavy impacts
- leap landing
- sword swing