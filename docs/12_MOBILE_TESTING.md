# Shardwake — Mobile Test Readiness

This document tracks the first Android/mobile test slice.

## Goal

The game must be playable without keyboard or mouse.

The first mobile test does not need final UI art.
It only needs clear touch controls and a complete MVP loop.

## Required Mobile Controls

- virtual joystick for movement
- Dash button
- Swap button
- Skill 1 / Skill 2 / Skill 3 buttons
- Consumable 1 / Consumable 2 buttons
- Bag button for temporary backpack testing
- Equip button for temporary gear testing

## Current Greybox Rules

- Debug inventory panel is hidden by default.
- BAG toggles it only when needed.
- EQUIP equips the first available equipment item for testing.
- Final mobile inventory UI will replace this later.

## Runtime Defaults

The project applies mobile-friendly runtime defaults:

- target frame rate: 60
- vSync disabled for mobile test consistency
- screen sleep disabled during playtest
- multi-touch enabled

## Before Android Build

Check:

- no compiler errors
- game works without keyboard
- portals can be discovered and used
- Hellgate can be entered
- death result works
- extraction result works
- stash persists during the app session
- touch buttons are not too small
- HUD does not cover combat
- FPS is acceptable on target device

## Not Required Yet

Do not block the first test on:

- final art
- final animations
- monetization
- AI Chronicle
- real multiplayer
- real backend
- polished inventory UI
