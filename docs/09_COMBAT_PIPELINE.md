# Shardwake — Combat Pipeline

Combat should use one shared damage and status-effect pipeline.

Individual weapons and monsters should not calculate damage differently.

## Damage Flow

Attack or skill
↓
Create DamageRequest
↓
DamageCalculator applies armor/resistance
↓
Shield absorbs damage
↓
Health loses final damage
↓
Optional status effect is applied
↓
Death rules are triggered if HP reaches zero

## Damage Types

- Physical
- Magic
- Fire
- Frost
- Poison
- Shadow
- Holy
- True

Physical and poison damage use Armor.

Magic, fire, frost, shadow and holy damage use Magic Resistance.

True damage ignores mitigation.

## Status Effects

Initial MVP effects:

- Burn
- Poison
- Bleed
- Slow
- Root
- Stun
- Shield
- Heal Over Time
- Weakness

Status effects must be shared between:

- weapons
- monsters
- bosses
- traps
- future items

No system should implement its own custom poison/slow/stun logic.

## Design Rule

Status effects should create gameplay decisions, not visual noise.

Every status effect needs clear counterplay or readability.
