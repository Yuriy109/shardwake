using System.Collections.Generic;

namespace Shardwake.Weapons
{
    public static class WeaponDefinitions
    {
        private static readonly Dictionary<WeaponType, WeaponDefinition> Definitions = new()
        {
            [WeaponType.GreatWeapon] = new WeaponDefinition(
                WeaponType.GreatWeapon,
                "Great Weapon",
                "Slow heavy melee damage with large AoE and punish windows.",
                "Strength",
                "Vitality / Focus",
                5.55f,
                112f,
                30f,
                0.58f,
                2.05f,
                new[]
                {
                    Skill("heavy_strike", "Heavy Strike", "Powerful frontal attack.", SkillEffectType.FrontalHit, 36f, 2.35f, 5f, "Strength"),
                    Skill("whirlwind", "Whirlwind", "Circular attack around the player.", SkillEffectType.AreaHit, 24f, 2.65f, 8f, "Strength"),
                    Skill("charge", "Charge", "Dash toward target or direction.", SkillEffectType.DashHit, 22f, 2f, 7f, "Strength"),
                    Skill("earthbreaker", "Earthbreaker", "Ground slam that damages and slows enemies.", SkillEffectType.GroundSlam, 30f, 2.75f, 10f, "Strength"),
                    Skill("blood_rage", "Blood Rage", "Temporarily increases damage but lowers defense.", SkillEffectType.SelfBuff, 0f, 0f, 14f, "Focus"),
                    Skill("shard_cleave", "Shard Cleave", "Magical shard wave released from the weapon.", SkillEffectType.Projectile, 28f, 2.45f, 9f, "Intelligence")
                },
                Passives(("berserker", "Berserker", "Lower HP increases damage."), ("heavy_blade", "Heavy Blade", "Basic attacks are slower but stronger."))),

            [WeaponType.SwordAndShield] = new WeaponDefinition(
                WeaponType.SwordAndShield,
                "Sword & Shield",
                "Defensive melee control, protection and disruption.",
                "Strength / Vitality",
                "Focus / Stamina",
                5.45f,
                125f,
                22f,
                0.48f,
                1.8f,
                new[]
                {
                    Skill("shield_bash", "Shield Bash", "Short-range hit that stuns or interrupts.", SkillEffectType.FrontalHit, 18f, 1.8f, 6f, "Strength"),
                    Skill("defensive_stance", "Defensive Stance", "Temporarily reduces incoming damage.", SkillEffectType.SelfBuff, 0f, 0f, 12f, "Vitality"),
                    Skill("chain_hook", "Chain Hook", "Pulls target toward the player.", SkillEffectType.Pull, 12f, 4.5f, 10f, "Focus"),
                    Skill("counterstrike", "Counterstrike", "Next attack after blocking/taking damage is empowered.", SkillEffectType.Counter, 24f, 1.9f, 9f, "Strength"),
                    Skill("guard_zone", "Guard Zone", "Small protective area for player and ally.", SkillEffectType.GuardZone, 0f, 2.6f, 15f, "Vitality"),
                    Skill("aegis_pulse", "Aegis Pulse", "Magical shield pulse around the player.", SkillEffectType.ShieldPulse, 22f, 2.3f, 9f, "Intelligence")
                },
                Passives(("unbreakable", "Unbreakable", "More armor, slightly lower movement speed."), ("protector", "Protector", "Nearby allies gain a small defense bonus."))),

            [WeaponType.Daggers] = new WeaponDefinition(
                WeaponType.Daggers,
                "Dual Daggers",
                "Fast melee assassin with mobility, burst and poison.",
                "Agility / Strength",
                "Stamina / Dexterity",
                6.65f,
                90f,
                17f,
                0.28f,
                1.45f,
                new[]
                {
                    Skill("backstab_dash", "Backstab Dash", "Dash behind or toward an enemy.", SkillEffectType.DashHit, 24f, 1.7f, 6f, "Agility"),
                    Skill("poison_strike", "Poison Strike", "Applies damage over time.", SkillEffectType.PoisonHit, 18f, 1.55f, 5f, "Agility"),
                    Skill("smoke_bomb", "Smoke Bomb", "Creates smoke to escape or drop aggro.", SkillEffectType.Smoke, 0f, 2.2f, 12f, "Stamina"),
                    Skill("flurry", "Flurry", "Fast series of dagger attacks.", SkillEffectType.MultiHit, 26f, 1.65f, 7f, "Agility"),
                    Skill("throwing_dagger", "Throwing Dagger", "Ranged poke attack.", SkillEffectType.RangedHit, 20f, 4.5f, 5f, "Dexterity"),
                    Skill("shadow_trick", "Shadow Trick", "Mystic shadow strike or clone effect.", SkillEffectType.AreaHit, 22f, 2.1f, 9f, "Intelligence")
                },
                Passives(("shadow_step", "Shadow Step", "After dodge or roll, gain brief movement speed."), ("venom_master", "Venom Master", "Poison lasts longer or deals slightly more damage."))),

            [WeaponType.MonkBattleStaff] = new WeaponDefinition(
                WeaponType.MonkBattleStaff,
                "Monk Battle Staff",
                "Mobile melee staff combos, spacing, control and light mystic techniques.",
                "Agility / Strength",
                "Focus / Intelligence",
                6.2f,
                100f,
                21f,
                0.38f,
                1.95f,
                new[]
                {
                    Skill("staff_combo", "Staff Combo", "Fast chained staff strikes in front of the player.", SkillEffectType.MultiHit, 24f, 1.95f, 5f, "Agility"),
                    Skill("sweeping_staff", "Sweeping Staff", "Wide circular sweep around the player.", SkillEffectType.AreaHit, 24f, 2.35f, 7f, "Strength"),
                    Skill("vault_kick", "Vault Kick", "Leap forward with staff and kick the target area.", SkillEffectType.DashHit, 22f, 2f, 7f, "Agility"),
                    Skill("force_palm", "Force Palm", "Mystic push that knocks enemies back.", SkillEffectType.Knockback, 16f, 2.1f, 8f, "Focus"),
                    Skill("rooting_strike", "Rooting Strike", "Precise staff hit that slows or roots.", SkillEffectType.Root, 18f, 1.9f, 8f, "Agility"),
                    Skill("spirit_wave", "Spirit Wave", "Mystic wave released from the staff.", SkillEffectType.Projectile, 24f, 2.4f, 8f, "Intelligence")
                },
                Passives(("flowing_steps", "Flowing Steps", "After dodging, next basic attack or staff skill is faster."), ("inner_focus", "Inner Focus", "Using different staff skills in sequence slightly reduces cooldowns."))),

            [WeaponType.Bow] = new WeaponDefinition(
                WeaponType.Bow,
                "Bow",
                "Mobile ranged pressure, traps, kiting and positioning.",
                "Dexterity",
                "Agility / Focus",
                6.1f,
                92f,
                19f,
                0.42f,
                5f,
                new[]
                {
                    Skill("quick_shot", "Quick Shot", "Fast low-cooldown shot.", SkillEffectType.RangedHit, 18f, 5.4f, 3f, "Dexterity"),
                    Skill("power_shot", "Power Shot", "Charged shot with high damage.", SkillEffectType.RangedHit, 34f, 5.8f, 7f, "Dexterity"),
                    Skill("arrow_rain", "Arrow Rain", "Area damage zone.", SkillEffectType.AreaHit, 24f, 2.7f, 10f, "Dexterity"),
                    Skill("trap", "Trap", "Places a trap that roots or slows enemies.", SkillEffectType.Trap, 10f, 2.2f, 11f, "Focus"),
                    Skill("backstep", "Backstep", "Jump backward to create distance.", SkillEffectType.Backstep, 0f, 0f, 7f, "Stamina"),
                    Skill("arcane_arrow", "Arcane Arrow", "Magical arrow projectile.", SkillEffectType.Projectile, 26f, 5f, 8f, "Intelligence")
                },
                Passives(("precise_hand", "Precise Hand", "Higher critical chance against distant targets."), ("pathfinder", "Pathfinder", "Increased movement speed outside combat."))),

            [WeaponType.MageStaff] = new WeaponDefinition(
                WeaponType.MageStaff,
                "Mage Staff",
                "Elemental ranged caster with AoE, burst and control.",
                "Intelligence",
                "Focus / Stamina",
                5.6f,
                82f,
                16f,
                0.52f,
                4.6f,
                new[]
                {
                    Skill("fireball", "Fireball", "Explosive projectile.", SkillEffectType.Projectile, 30f, 4.6f, 6f, "Intelligence"),
                    Skill("frost_bolt", "Frost Bolt", "Projectile that slows.", SkillEffectType.Projectile, 20f, 4.7f, 5f, "Intelligence"),
                    Skill("lightning_strike", "Lightning Strike", "Fast single-target damage.", SkillEffectType.RangedHit, 28f, 4.8f, 6f, "Intelligence"),
                    Skill("blink", "Blink", "Short teleport.", SkillEffectType.Blink, 0f, 0f, 9f, "Focus"),
                    Skill("frost_field", "Frost Field", "Area slow zone.", SkillEffectType.AreaHit, 18f, 2.9f, 10f, "Intelligence"),
                    Skill("meteor", "Meteor", "Delayed high-damage area attack.", SkillEffectType.AreaHit, 42f, 3.1f, 14f, "Intelligence")
                },
                Passives(("elemental_resonance", "Elemental Resonance", "Using different elements in sequence gives a bonus."), ("fragile_genius", "Fragile Genius", "Increases spell damage but reduces max HP."))),

            [WeaponType.HolyRelic] = new WeaponDefinition(
                WeaponType.HolyRelic,
                "Holy Relic",
                "Healing, shields, support and anti-corrupted damage.",
                "Intelligence",
                "Focus / Vitality",
                5.7f,
                96f,
                14f,
                0.55f,
                4.2f,
                new[]
                {
                    Skill("heal", "Heal", "Restores HP to self or ally.", SkillEffectType.Heal, 0f, 0f, 8f, "Intelligence"),
                    Skill("light_shield", "Light Shield", "Applies a temporary shield.", SkillEffectType.Shield, 0f, 0f, 11f, "Focus"),
                    Skill("light_beam", "Light Beam", "Damages enemies or heals allies.", SkillEffectType.RangedHit, 22f, 4.4f, 6f, "Intelligence"),
                    Skill("blessing", "Blessing", "Temporarily increases ally or self damage.", SkillEffectType.SelfBuff, 0f, 0f, 13f, "Focus"),
                    Skill("cleanse", "Cleanse", "Removes negative effects.", SkillEffectType.Cleanse, 0f, 0f, 12f, "Focus"),
                    Skill("holy_ground", "Holy Ground", "Zone that heals allies and damages corrupted enemies.", SkillEffectType.AreaHit, 18f, 2.9f, 12f, "Intelligence")
                },
                Passives(("grace", "Grace", "Healing is stronger on low-HP targets."), ("holy_resolve", "Holy Resolve", "After taking damage, gain brief damage reduction."))),

            [WeaponType.NecromancerTome] = new WeaponDefinition(
                WeaponType.NecromancerTome,
                "Necromancer Tome",
                "Summons, curses, sustain and dark magic.",
                "Intelligence",
                "Focus / Vitality",
                5.5f,
                88f,
                15f,
                0.56f,
                4.2f,
                new[]
                {
                    Skill("summon_skeleton", "Summon Skeleton", "Summons a temporary skeleton minion.", SkillEffectType.Summon, 0f, 0f, 14f, "Intelligence"),
                    Skill("bone_spear", "Bone Spear", "Linear projectile damage.", SkillEffectType.Projectile, 28f, 4.7f, 6f, "Intelligence"),
                    Skill("weakness_curse", "Weakness Curse", "Reduces enemy damage.", SkillEffectType.Curse, 0f, 3.2f, 11f, "Focus"),
                    Skill("vampiric_touch", "Vampiric Touch", "Deals damage and heals the player.", SkillEffectType.Sustain, 22f, 2.2f, 8f, "Intelligence"),
                    Skill("bone_armor", "Bone Armor", "Temporary defensive shield.", SkillEffectType.Shield, 0f, 0f, 11f, "Focus"),
                    Skill("corpse_explosion", "Corpse Explosion", "Area burst near defeated enemies.", SkillEffectType.CorpseExplosion, 34f, 2.8f, 10f, "Intelligence")
                },
                Passives(("bone_lord", "Bone Lord", "Summons last longer or become stronger."), ("dark_harvest", "Dark Harvest", "Kills restore a small amount of HP or resource.")))
        };

        public static WeaponDefinition Get(WeaponType type)
        {
            return Definitions[type];
        }

        private static WeaponSkillDefinition Skill(string id, string displayName, string description, SkillEffectType effectType, float baseDamage, float radius, float cooldown, string scalingStat)
        {
            return new WeaponSkillDefinition(id, displayName, description, effectType, baseDamage, radius, cooldown, scalingStat);
        }

        private static WeaponPassiveDefinition[] Passives(params (string Id, string DisplayName, string Description)[] passives)
        {
            var result = new WeaponPassiveDefinition[passives.Length];
            for (var i = 0; i < passives.Length; i++)
            {
                result[i] = new WeaponPassiveDefinition(passives[i].Id, passives[i].DisplayName, passives[i].Description);
            }

            return result;
        }
    }
}
