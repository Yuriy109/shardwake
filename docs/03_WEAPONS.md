# Shardwake — Weapon System

Shardwake has no fixed classes.

Players build their playstyle through weapons.

A weapon defines:
- combat role
- basic attack style
- available active skills
- passive skills
- scaling stats
- animation style

Each player can equip up to 2 weapons per run and swap between them during combat.

---

# Weapon Rules

Each weapon has:

- 6 active skills
- player equips 3 active skills
- 2 passive skills
- player equips 1 passive skill

Skills can be changed only in Haven before a run.

Weapon swap should have a cooldown.

Only the currently active weapon skills are available during combat.

---

# Current Weapons

## 1. Great Weapon

Weapons:
Two-handed sword / axe / hammer.

Role:
Heavy melee damage dealer.

Main scaling:
Strength.

Secondary scaling:
Vitality / Focus.

Active skills:
1. Heavy Strike — powerful frontal attack.
2. Whirlwind — circular attack around the player.
3. Charge — dash toward target or direction.
4. Earthbreaker — ground slam that damages and slows enemies.
5. Blood Rage — increases damage but lowers defense.
6. Shard Cleave — magical shard wave. Scales partly with Intelligence.

Passives:
1. Berserker — lower HP increases damage.
2. Heavy Blade — basic attacks are slower but stronger.

---

## 2. Sword & Shield

Role:
Defensive melee fighter / tank / protector.

Main scaling:
Strength / Vitality.

Secondary scaling:
Focus / Stamina.

Active skills:
1. Shield Bash — short hit that stuns or interrupts.
2. Defensive Stance — temporarily reduces incoming damage.
3. Chain Hook — pulls target toward the player.
4. Counterstrike — after block/taking damage, next attack deals bonus damage.
5. Guard Zone — protective area for player and nearby ally.
6. Aegis Pulse — magical shield pulse. Scales partly with Intelligence.

Passives:
1. Unbreakable — more armor, slightly lower movement speed.
2. Protector — nearby allies gain small defense bonus.

---

## 3. Dual Daggers

Role:
Fast melee assassin.

Main scaling:
Agility / Strength.

Secondary scaling:
Stamina / Dexterity.

Active skills:
1. Backstab Dash — dash behind or toward an enemy.
2. Poison Strike — applies damage over time.
3. Smoke Bomb — helps escape, drops aggro or briefly hides player.
4. Flurry — fast series of dagger attacks.
5. Throwing Dagger — ranged poke attack.
6. Shadow Trick — shadow strike or clone effect. Scales partly with Intelligence.

Passives:
1. Shadow Step — after dodge/roll, gain brief movement speed.
2. Venom Master — poison lasts longer or deals more damage.

---

## 4. Monk Battle Staff

Role:
Mobile melee fighter with staff combos, spacing, control and light mystic techniques.

Main scaling:
Agility / Strength.

Secondary scaling:
Focus / Intelligence.

Active skills:
1. Staff Combo — fast chained staff strikes.
2. Sweeping Staff — wide circular sweep.
3. Vault Kick — leap forward and strike target area.
4. Force Palm — short-range mystic push that knocks enemies back.
5. Rooting Strike — slows or briefly roots enemy.
6. Spirit Wave — mystic wave from the staff. Scales partly with Intelligence.

Passives:
1. Flowing Steps — after dodging, next basic attack or staff skill is faster.
2. Inner Focus — using different staff skills in sequence slightly reduces cooldowns.

---

## 5. Bow

Role:
Mobile ranged physical damage dealer.

Main scaling:
Dexterity.

Secondary scaling:
Agility / Focus.

Active skills:
1. Quick Shot — fast low-cooldown shot.
2. Power Shot — charged shot with high damage.
3. Arrow Rain — area damage zone.
4. Trap — roots or slows enemies.
5. Backstep — jump backward to create distance.
6. Arcane Arrow — magical arrow. Scales partly with Intelligence.

Passives:
1. Precise Hand — higher critical chance against distant targets.
2. Pathfinder — increased movement speed outside combat.

---

## 6. Mage Staff

Role:
Elemental ranged caster.

Main scaling:
Intelligence.

Secondary scaling:
Focus / Stamina.

Active skills:
1. Fireball — explosive projectile.
2. Frost Bolt — projectile that slows.
3. Lightning Strike — fast single-target damage.
4. Blink — short teleport.
5. Frost Field — area slow zone.
6. Meteor — delayed high-damage area attack.

Passives:
1. Elemental Resonance — using different elements in sequence gives bonus.
2. Fragile Genius — increases spell damage but reduces max HP.

---

## 7. Holy Relic

Role:
Support, healer, anti-undead caster.

Main scaling:
Intelligence.

Secondary scaling:
Focus / Vitality.

Active skills:
1. Heal — restores HP to self or ally.
2. Light Shield — applies temporary shield.
3. Light Beam — damages enemies or heals allies depending on target.
4. Blessing — temporarily increases self or ally damage.
5. Cleanse — removes negative effects.
6. Holy Ground — heals allies and damages undead/corrupted enemies.

Passives:
1. Grace — healing is stronger on low-HP targets.
2. Holy Resolve — after taking damage, gain brief damage reduction.

---

## 8. Necromancer Tome

Role:
Summoner and curse caster.

Main scaling:
Intelligence.

Secondary scaling:
Focus / Vitality.

Active skills:
1. Summon Skeleton — summons temporary skeleton minion.
2. Bone Spear — linear projectile damage.
3. Weakness Curse — reduces enemy damage.
4. Vampiric Touch — deals damage and heals player.
5. Bone Armor — temporary defensive shield.
6. Corpse Explosion — AoE explosion around corpse/defeated enemy.

Passives:
1. Bone Lord — summons last longer or become stronger.
2. Dark Harvest — kills restore small amount of HP or resource.

---

# Core Stats

Use 7 core stats:

- Strength — melee damage + small max HP
- Dexterity — ranged physical damage + small critical chance
- Intelligence — magic damage + small magic resistance
- Agility — dodge chance + attack speed + small movement speed
- Vitality — percentage max HP + small physical resistance
- Focus — cooldown reduction + faster skill activation
- Stamina — max energy for rolls + HP regeneration

Every stat should be at least slightly useful for every build.

---

# Design Rules

- This is weapon-based, not class-based.
- Player can equip up to 2 weapons per run.
- Weapon swap must exist and should have cooldown.
- Skills must be readable on mobile.
- No skill should require complex targeting in MVP.
- Every weapon must work in solo mode.
- Holy Relic becomes more valuable in duo mode, but must still be usable solo.
- Chain Hook should be implemented as reusable effect type: pull target toward caster.
- Skill numbers are placeholder values.
- Balance values must be stored separately from UI and view scripts.
- Do not hardcode skill logic inside UI components or MonoBehaviour view scripts.