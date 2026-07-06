# Ancient Grove

## Theme
Living magical forest.

## Biome Gameplay
Ancient Grove teaches:
- movement
- target priority
- avoiding area control
- killing support enemies first

Rule:
Each normal enemy has one main ability.
The boss combines all biome mechanics.

---

# Living Sapling

## Role
Swarm melee

## Difficulty
Easy

## Description
Small young forest creature. Weak alone, annoying in groups.

## AI
- Runs directly toward the player.
- Tries to surround the player.
- Attacks in groups.

## Basic Attack
Headbutt

## Ability
Seed Burst

After death leaves a small patch of roots.

Effect:
- small radius
- lasts 3 seconds
- light damage over time

## Required Animations
- Idle
- Run
- Attack
- Death

## Optional Animations
- Hit / Flinch

## VFX
- small leaves
- root patch
- green particles

## SFX
- small wood step
- tiny plant hit
- root crack

---

# Mossback Beast

## Role
Bruiser / charger

## Difficulty
Medium

## Description
Heavy forest beast covered in moss and bark.

## AI
- Walks toward the player.
- Uses Charge when in medium distance.
- After missing Charge, becomes vulnerable.

## Basic Attack
Heavy Claw Swipe

## Ability
Charge

Rushes forward in a straight line.

If hit:
- medium/high damage
- short stun, 0.3–0.5 sec

If missed:
- recovery window, 2 sec

## Required Animations
- Idle
- Walk
- Run / Charge
- Attack
- Death

## Optional Animations
- Hit / Flinch
- Stagger / Exhausted

## VFX
- dust
- leaf burst
- ground impact

## SFX
- heavy steps
- beast growl
- charge impact

---

# Grove Spirit

## Role
Support caster

## Difficulty
Medium

## Description
Calm forest spirit that keeps nearby creatures alive.

## AI
Priority:
1. Keep distance.
2. Heal wounded allies.
3. Attack only if no ally needs healing.

## Basic Attack
Nature Bolt

Small green projectile.

## Ability
Nature Blessing

Heals nearest wounded ally.

Rules:
- cooldown: 8–10 sec
- low/medium heal
- should not make fights too long

## Required Animations
- Idle
- Hover / Float
- Cast
- Death

## Optional Animations
- Hit / Flinch

## Not Needed
- Walk
- Run

The spirit can move in Unity while playing Idle/Hover.

## VFX
- green orb
- healing circle
- floating leaves
- soft glow

## SFX
- magic hum
- leaf whisper
- soft heal sound

---

# Ancient Treant

## Role
Biome boss

## Difficulty
Boss

## Description
Ancient protector of the Grove. Slow, heavy, controls space.

## AI
- Slowly approaches player.
- Uses area control.
- Summons Saplings.
- Becomes more aggressive at low HP.

## Basic Attack
Massive Arm Slam

Heavy melee hit in front of the boss.

## Ability 1
Root Wave

Slams ground. Roots travel outward.

Effect:
- line / cone area
- medium damage
- forces dodge

## Ability 2
Living Roots

Roots appear under player.

Effect:
- slow
- short duration
- low/no damage

## Ability 3
Summon Saplings

Summons 2 Living Saplings.

## Phase 2
Triggered at 50% HP.

Changes:
- faster attacks
- Root Wave cooldown reduced

## Phase 3
Triggered at 25% HP.

Treant absorbs Grove Spirit energy.

Effect:
- small heal
- green aura
- Root Wave becomes wider

## Required Animations
- Idle
- Walk
- Basic Attack / Slam
- Special Attack / Root Wave
- Summon / Cast
- Enrage / Roar
- Death

## Optional Animations
- Hit / Flinch
- Stunned
- Taunt

## VFX
- root wave
- ground cracks
- falling leaves
- green aura
- summon circle

## SFX
- heavy wood step
- deep tree groan
- ground slam
- roots breaking ground
- boss roar


