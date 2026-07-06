# Shardwake — Greybox Map Scale Pass

This document records the current MVP map scale.

The goal of this pass is to define practical dimensions before choosing final art assets.

---

# Scale Rules

## Module Size

One biome module is currently:

```text
60 x 60 Unity units
```

The 13-module layout creates a map around:

```text
300 x 300 Unity units overall
```

This gives enough space for 12-minute extraction runs without making the first prototype feel empty.

---

# Player Scale

The player is treated as roughly:

```text
1.8–2.2 Unity units tall
```

This should stay consistent when replacing greybox capsules with real stylized 3D models.

---

# Gameplay Space

Minimum corridor width:

```text
6 Unity units
```

Combat arena target size:

```text
15–25 Unity units wide
```

Each module should contain:

- 2–4 points of interest
- several enemy packs
- 1–3 chest/loot locations
- possible portal marker locations
- optional mini-boss spawn

---

# Camera

The MVP camera is orthographic and should show roughly:

```text
35–45 Unity units around the player
```

This gives enough tactical view for a top-down mobile extraction game without revealing the whole module.

---

# Art Asset Requirements

When searching for asset packs, prefer assets that work with:

- stylized low-poly 3D
- readable top-down silhouettes
- large props
- simple materials
- modular fantasy environments
- mobile-friendly poly counts

Avoid assets that require:

- realistic scale precision
- dense decoration
- tiny readable details
- high-end shaders
- first-person scale assumptions
