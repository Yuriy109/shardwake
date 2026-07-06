# Shardwake — Equipment

Shardwake has no class-locked equipment.

Players can combine any weapons, armor and accessories.

Equipment should support experimentation, not force predefined builds.

---

# Equipment Slots

Player has 10 equipment slots:

## Armor

- head
- chest
- hands
- legs
- feet

## Weapons

- weapon1
- weapon2

## Accessories

- ring1
- ring2
- amulet

Only armor affects equip load.

Weapons and accessories do not add weight.

---

# Armor Types

Armor has three categories:

- light
- medium
- heavy

Any player can wear any armor type.

Armor type affects mobility and protection.

---

# Equip Load

Equip load is calculated only from armor pieces.

Light load:

- fast roll
- long roll distance
- low protection

Medium load:

- balanced roll
- balanced protection

Heavy load:

- slow roll
- short roll distance
- high protection
- higher stagger resistance

Overloaded:

- bad mobility
- weak roll
- high protection
- should feel uncomfortable

---

# Design Goal

Armor should create real choices.

Examples:

- Mage in heavy armor = safer but slower
- Sword & Shield in light armor = mobile but less tanky
- Daggers in medium armor = balanced assassin
- Battle Staff in light armor = fast melee skirmisher

No armor setup should be correct for every build.

---

# Accessories

Accessories do not affect weight.

Rings and amulets provide:

- stats
- special effects
- build customization

Amulets should be more powerful and rarer than rings.

---

# Rarity

Equipment rarity affects:

- number of affixes
- affix value range
- chance of special effects
- AI flavor for high rarity items

Rarity should make items exciting, but not remove player choice.
Вот полный вариант для docs/04_EQUIPMENT.md:

# Shardwake — Equipment Slots & Armor Weight System
Shardwake uses weapon-based builds, not fixed classes.
Equipment should support flexible builds:
- mage in heavy armor
- sword-and-shield player in light armor
- bow + holy relic hybrid
- daggers + necromancer tome hybrid
Armor is never locked by weapon/class.
---
# Equipment Slots
Player has 10 equipped slots:
## Armor slots
- head
- chest
- hands
- legs
- feet
## Weapon slots
- weapon1
- weapon2
## Accessory slots
- ring1
- ring2
- amulet
Only armor slots affect equip load.
These slots DO NOT add weight:
- weapon1
- weapon2
- ring1
- ring2
- amulet
Weapons define combat style and skills.  
Accessories provide stats and special effects.  
Armor defines protection and mobility tradeoffs.
---
# Armor Categories
Each armor piece can be:
- light
- medium
- heavy
Base armor weights:
```csharp
const ARMOR_WEIGHTS = {
  head:  { light: 1.5, medium: 2.6, heavy: 4.7 },
  chest: { light: 3.5, medium: 6.2, heavy: 11.0 },
  hands: { light: 1.5, medium: 2.6, heavy: 4.7 },
  legs:  { light: 2.0, medium: 3.5, heavy: 6.3 },
  feet:  { light: 1.5, medium: 2.6, heavy: 4.7 },
};

⸻

Equip Load

Equip load is the sum of equipped armor weights only.

equipLoad =
  head.weight +
  chest.weight +
  hands.weight +
  legs.weight +
  feet.weight;

Do not include weapons, rings, or amulet in equip load.

⸻

Load Categories

Light Load: 0.0 - 12.9
Medium Load: 13.0 - 22.9
Heavy Load: 23.0 - 30.0
Overloaded: 30.1+

⸻

Load Effects

Light Load

* longest roll distance
* fastest roll animation
* largest dodge window
* best energy recovery
* lowest armor

Medium Load

* standard roll distance
* standard roll animation
* standard dodge window
* standard energy recovery
* balanced armor

Heavy Load

* shorter roll distance
* slower roll animation
* smaller dodge window
* slower energy recovery
* higher armor
* higher stagger resistance

Overloaded

* cannot roll properly
* very short dodge movement
* very high energy cost
* reduced movement speed
* very high armor

Overloaded should be possible but uncomfortable.

⸻

Design Goals

The armor system should create real build decisions.

Examples:

* full light armor = high mobility, low protection
* full medium armor = balanced
* full heavy armor = tanky but slow
* heavy chest + light other pieces = hybrid build
* medium/light mix = flexible PvPvE build

Do not lock armor by weapon.

Do not lock armor by class.

Do not hardcode this logic inside UI components.

⸻

Suggested Unity Structure

Assets/_Shardwake/Runtime/Equipment/
  EquipmentSlot.cs
  ArmorType.cs
  ArmorWeights.cs
  EquipLoadCalculator.cs
  LoadCategory.cs
  LoadEffects.cs
  AccessoryType.cs