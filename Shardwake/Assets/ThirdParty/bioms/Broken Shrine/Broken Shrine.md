# Broken Shrine

## Theme

A forgotten shrine corrupted by blood rituals and dark cults.

## Biome Gameplay

Broken Shrine teaches:

- target priority
- interrupting dangerous casters
- avoiding ritual zones
- dealing with enemy buffs

Unlike Bandit Road, enemies cooperate through rituals and dark magic.

Rule:
Every enemy has one unique role.
The boss combines all ritual mechanics.

---

# Cult Acolyte

## Role

Basic Melee

## Difficulty

Easy

## Description

Young cult recruit.

Weak individually but dangerous in groups.

## AI

- Runs directly toward the player.
- Never retreats.
- Protects nearby Priest if possible.

## Basic Attack

Ritual Dagger

Quick melee slash.

## Ability

Blood Sacrifice

When HP falls below 25%, explodes in corrupted blood.

Effects:

- small AoE damage
- applies Bleed for 3 seconds

Cooldown

One-time only.

## Required Animations

- Idle
- Walk
- Run
- Attack
- Death

## Optional Animations

- Hit

## VFX

- blood splash
- dark smoke
- red rune

## SFX

- dagger slash
- blood burst
- scream

---

# Blood Cultist

## Role

Fast Assassin

## Difficulty

Medium

## Description

A fanatic completely devoted to the shrine.

Moves unpredictably.

## AI

- Circles around the player.
- Frequently changes direction.
- Looks for openings.

## Basic Attack

Dual Ritual Slash

## Ability

Blood Dash

Quick dash through the player.

Effects

- medium damage
- applies short Bleed

Cooldown

5 seconds.

## Required Animations

- Idle
- Run
- Attack
- Dash
- Death

## Optional Animations

- Hit

## VFX

- blood trail
- red particles

## SFX

- fast footsteps
- dash
- blade slash

---

# Shrine Priest

## Role

Support Caster

## Difficulty

Medium

## Description

Performs corrupted rituals to strengthen allies.

Highest target priority in the biome.

## AI

Priority

1. Buff allies.
2. Curse player.
3. Attack only if alone.

Keeps maximum distance.

## Basic Attack

Blood Bolt

Medium-range projectile.

## Ability

Dark Ritual

Creates a ritual circle.

Enemies standing inside receive:

- increased damage
- increased movement speed

Duration

8 seconds.

Cooldown

10 seconds.

## Required Animations

- Idle
- Cast
- Death

## Optional Animations

- Hit

## Not Needed

- Walk
- Run

The Priest can slowly float or move while playing Idle.

## VFX

- ritual circle
- blood runes
- red energy
- floating symbols

## SFX

- chanting
- dark magic
- ritual hum

---

# Skorn, Shrine Boss

## Role

Biome Boss

## Difficulty

Boss

## Description

Ancient executioner possessed by the shrine.

Slow, brutal and relentless.

## AI

- Slowly approaches player.
- Controls battlefield.
- Uses blood rituals.
- Becomes more aggressive as health decreases.

## Basic Attack

Heavy Cleaver Smash

Wide frontal attack.

## Ability 1

Blood Wave

Slams the ground.

A wave of corrupted blood travels forward.

Effects

- medium damage
- applies Bleed

## Ability 2

Summon Acolytes

Summons two Cult Acolytes.

Cooldown

20 seconds.

## Ability 3

Blood Prison

Marks several locations.

After a short delay, blood spikes erupt.

Player must dodge.

## Phase 2

Triggered at 50% HP.

Changes

- Blood Wave cooldown reduced
- attacks faster
- summons more frequently

## Phase 3

Triggered at 25% HP.

Consumes the nearby ritual energy.

Effects

- temporary damage increase
- Blood Prison covers larger area
- becomes much more aggressive

## Required Animations

- Idle
- Walk
- Heavy Attack
- Ground Slam
- Summon
- Roar
- Death

## Optional Animations

- Hit
- Taunt

## VFX

- blood wave
- blood spikes
- ritual circle
- corruption aura
- summon effect

## SFX

- heavy footsteps
- deep roar
- blood explosion
- ritual chanting
- ground impact