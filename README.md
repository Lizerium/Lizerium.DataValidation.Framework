<h1 align="center">🧪 Lizerium Tests 🧪</h1>

<p align="center">
  A suite of engineering tests, validators, and verification tools for game data<br/>
  in Freelancer-like projects and modifications within the <b>Lizerium</b> ecosystem.
</p>

<p align="center">
  <img src="https://shields.dvurechensky.pro/badge/.NET-8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://shields.dvurechensky.pro/badge/Test%20Layer-Game%20Data%20Validation-1565C0?style=for-the-badge" />
  <img src="https://shields.dvurechensky.pro/badge/Formats-INI%20DLL%20CMP%20MAT%20TXM-37474F?style=for-the-badge" />
  <img src="https://shields.dvurechensky.pro/badge/Scope-Freelancer%20Like%20Projects-2E7D32?style=for-the-badge" />
  <img src="https://shields.dvurechensky.pro/badge/Status-In%20Development-F9A825?style=for-the-badge" />
</p>

<div align="center" style="margin: 20px 0; padding: 10px; background: #1c1917; border-radius: 10px;">
  <strong>🌐 Language: </strong>
  
  <a href="./README.ru.md" style="color: #F5F752; margin: 0 10px;">
    🇷🇺 Russian
  </a>
  | 
  <span style="color: #0891b2; margin: 0 10px;">
    ✅ 🇺🇸 English (current)
  </span>
</div>

---

> [!NOTE]
> This project is part of the **Lizerium** ecosystem and belongs to the following direction:
>
> - [`Lizerium.Frameworks.Structs`](https://github.com/Lizerium/Lizerium.Frameworks.Structs)
>
> If you are looking for related engineering and supporting tools, start there.

---

## About the Project

**Lizerium Tests** is a framework for automated engineering validation of game data, configurations, resource references, and structural dependencies used in Freelancer-like games and modifications.

This project is not intended for the game client itself, but for **engineering-level content validation**:

- `INI` configurations
- `DLL` resources (`ids_name`, `ids_info`)
- asset paths
- references to effects, sounds, models, and containers
- logical dependencies between game entities
- overall technical integrity of a modification

This layer helps detect in advance:

- broken references
- missing resources
- typos in keys
- invalid values
- conflicting or incomplete game entries
- errors that would normally appear only during gameplay

The project is especially useful for:

- maintaining large-scale modifications
- mass refactoring of game data
- migration of legacy configurations
- building custom game toolchains
- supporting complex content pipelines

> [!TIP]
> Test results and intermediate diagnostic data are written to separate log files, allowing the project to be used not only as a test layer but also as a technical auditing tool for game data.

---

## Current Test Coverage

### General Tests

- [x] Verify that all files expected to be `BINI` are correctly encoded
- [x] Check for animation presence in `CMP` files
- [x] Validate `Hardpoints` in `CMP` files
- [x] Validate textures in `STARSPHERE`, `FX`
- [x] Check `use_animation` references in `CMP` files
- [x] Detect potential animation capability in `CMP`
- [x] Find all missing `material_library` references
- [x] Find all missing `item_icon` references
- [x] Find all missing `DA_archetype` references
- [x] Validate presence of all infocards
- [x] Validate all `ids_name` entries
- [x] Validate CRC values for effects
- [x] Validate effect texture paths
- [x] Validate all `explosion_arch` entries
- [x] Validate all `shield_type` entries
- [x] Validate all `debris_type` entries
- [x] Validate all `[Effect]` entries
- [x] Validate all `[Sound]` entries
- [x] Validate all containers (`LootCrate`, `CargoPod`)
- [x] Validate faction definitions (`faction =`)

> Files involved:

- `initialworld.ini`
- `ReShade.ini`
- `explosions.ini`
- `weapon_equip.ini`
- `weaponmoddb.ini`
- `engines_ale.ini`
- `effects.ini`
- `sounds.ini`
- `engine_sounds.ini`
- `select_equip.ini`
- `misc_equip.ini`
- `weapon_equip.ini`

> [!CAUTION]
> Output is written to `TESTS_LOGGING/GlobalTests.ini`

---

### INI → EQUIPMENT

#### CommoditiesPerFactionTests.ini

- [x] Validate data types in all fields
- [x] Validate commodity values in `MarketGood` fields within `FactionGood`

> [!CAUTION]
> Output: `TESTS_LOGGING/CommoditiesPerFactionTests.ini`

---

#### engine_equip.ini

- [x] Extract all unique keys from sections
- [x] Extract all types in the file
- [ ] Validate variable correctness
  - [x] `ids_name` / `ids_info` in DLL resources
  - [x] Expected number of sections
  - [ ] Validate parsing of each `[Engine]` section via `TryParse`
  - [ ] Ensure required fields exist (`nickname`, `ids_name`, `mass`, etc.)
  - [ ] Ensure required string fields are not empty
  - [ ] Detect unknown or invalid keys
  - [ ] Ensure all engine nicknames are unique
  - [ ] Validate numeric ranges (`mass`, `max_force`, etc.)
  - [ ] Validate `reverse_fraction` logic
  - [ ] Validate cruise parameters (`cruise_speed`, etc.)
  - [ ] Validate range formats (`character_pitch_range`, etc.)
  - [ ] Validate logical consistency between fields
  - [ ] Validate sound logic consistency
  - [ ] Detect empty or corrupted sections
  - [ ] Validate `volume` and `power_usage` values

> [!CAUTION]
> Output: `TESTS_LOGGING/EngineEquipTests.ini`

---

#### select_equip.ini

- [x] Extract unique keys
- [x] Extract types
- [x] Validate `AttachedFX` / `use_throttle`
- [ ] Validate Armor
- [ ] Validate CargoPod
- [ ] Validate Commodity
- [ ] Validate InternalFX
- [ ] Validate ShieldGenerator
- [ ] Validate Shield

> [!CAUTION]
> Output: `TESTS_LOGGING/EngineEquipTests.ini`

---

### FLHook Tests | Generation

- [ ] Validate armor regeneration configuration completeness

---

## Configuration

### `app_settings.json`

Used to point tests to a specific Freelancer-like game/modification.  
Works in combination with the structural map:

- [`Lizerium.Game.Structs`](https://github.com/Lizerium/Lizerium.Game.Structs)

Steps:

1. Create `app_settings.json` in:  
   `Lizerium.Tests/SETTINGS/app_settings.json`

2. Example config:  
   [App Settings Example](Lizerium.Tests/Configs/app_settings_example.json)

---

### `ExpectedBiniFiles.json`

- [Exclusion list](Lizerium.Tests/Configs/ExpectedBiniFiles.json)

Used to define files expected to be encoded in `BINI` format.

---

## Relation to Other Directions

This layer is connected with:

- [`LizeriumDataToolkit`](https://github.com/Lizerium/LizeriumDataToolkit)
- [`Lizerium.Game.Structs`](https://github.com/Lizerium/Lizerium.Game.Structs)
- [`Lizerium.Game.INI`](https://github.com/Lizerium/Lizerium.Game.INI)
- [`Lizerium.Game.Dlls`](https://github.com/Lizerium/Lizerium.Game.Dlls)
- [`Lizerium.Game.SPH`](https://github.com/Lizerium/Lizerium.Game.SPH)

Temporary sources (use latest mod build):

- `Lizerium.Game.CMP`
- `Lizerium.Game.MAT`
- `Lizerium.Game.TXM`
- `Lizerium.Game.3DB`
- `Lizerium.Game.Music`

XML layer:

- [`Lizerium.Game.XML.CMP`](https://github.com/Lizerium/Lizerium.Game.XML.CMP)
- [`Lizerium.Game.XML.MAT`](https://github.com/Lizerium/Lizerium.Game.XML.MAT)
- [`Lizerium.Game.XML.TXM`](https://github.com/Lizerium/Lizerium.Game.XML.TXM)
- [`Lizerium.Game.XML.SPH`](https://github.com/Lizerium/Lizerium.Game.XML.SPH)
- [`Lizerium.Game.XML.3DB`](https://github.com/Lizerium/Lizerium.Game.XML.3DB)
- [`Lizerium.Game.XML.ALE`](https://github.com/Lizerium/Lizerium.Game.XML.ALE)
- [`Lizerium.Game.XML.ANM`](https://github.com/Lizerium/Lizerium.Game.XML.ANM)
- [`Lizerium.Game.XML.DFM`](https://github.com/Lizerium/Lizerium.Game.XML.DFM)
- [`Lizerium.Game.XML.UTF`](https://github.com/Lizerium/Lizerium.Game.XML.UTF)
- [`Lizerium.Game.XML.VMS`](https://github.com/Lizerium/Lizerium.Game.XML.VMS)
