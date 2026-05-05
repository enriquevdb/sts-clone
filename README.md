# STS Clone

A turn-based deckbuilding roguelike inspired by **Slay the Spire**, built from scratch in **Godot 4** with **C#**.

> This is a personal learning project. I'm building it to teach myself **Godot 4** and **C#** — focusing on game architecture, clean code practices, and the fundamentals of turn-based combat design.

---

## Why I'm Building This

I wanted a project that was:

- **Big enough to be challenging** — multiple interlocking systems (cards, combat, status effects, AI, progression).
- **Small enough to actually finish** — by scoping the game as a Slay the Spire-style roguelike, every system has a clear, well-known reference design.
- **A real test of architecture** — deckbuilders force you to write decoupled, data-driven code if you want to add new cards, enemies, and relics without rewriting everything.

The goal isn't to ship a commercial game. It's to **become a competent C# / Godot developer** by building something non-trivial end-to-end.

---

## What I'm Learning

- **C# fundamentals** — properties, generics, LINQ, events, interfaces, polymorphism.
- **Godot 4 with .NET** — nodes, scenes, signals, resources, exports, and the C# scripting workflow.
- **Game architecture** — composition over inheritance, separation of game logic from presentation, event-driven systems.
- **Turn-based combat design** — state machines, intent systems, status effect stacking, deck/hand/discard pile management.
- **Data-driven design** — using Godot `Resource` types so cards, enemies, and relics can be authored as data instead of hard-coded.

---

## Tech Stack

- **Engine:** Godot 4.x (.NET / Mono build)
- **Language:** C# (.NET)
- **Version Control:** Git

---

## Current Progress

### Phase 1 — Core Combat *(mostly complete)*

- [x] Turn system (player turn / enemy turn)
- [x] Hand, draw pile, and discard pile logic
- [x] Energy and block mechanics
- [x] Enemy intent system
- [x] End turn button + new hand draw
- [x] Win / Defeat screens

### Phase 2 — Cards & Deck Building *(in progress)*

- [ ] Full Attack card set (Strike, Bash, Headbutt)
- [ ] Full Skill card set (Defend, Shrug It Off, Flex)
- [ ] Power cards (Strength, Inflame, Metallicize)
- [ ] Status effects (Vulnerable implemented, Weak / Strength to come)

### Phases 3–6 *(planned)*

- Map & run progression (graph-based node map)
- Elite & Boss enemies
- Relic system
- Polish, animations, and main menu

---

<img width="1152" height="650" alt="image" src="https://github.com/user-attachments/assets/adf871ed-84e6-4e20-aafe-00e987fdc3cc" />

## Project Structure

```
/Cards          - Card base class + Attack / Skill / Power subtypes
/Managers       - CombatManager (turn flow + combat state)
/StatusEffect   - Status effect system (Vulnerable, etc.)
/UI             - Hand, HUD, victory / defeat screens
/Textures       - Sprites and art assets
Player.cs       - Player state (HP, energy, block)
Enemy.cs        - Enemy state + intent
Main.tscn       - Entry scene
```

---

## Running the Project

1. Install [Godot 4.x — .NET version](https://godotengine.org/download).
2. Make sure you have the [.NET SDK](https://dotnet.microsoft.com/download) installed.
3. Clone the repo:
   ```bash
   git clone https://github.com/enriquevdb/STS-CLONE.git
   ```
4. Open `project.godot` in Godot 4.
5. Build the C# solution (Godot will prompt you), then press **F5** to run.

---

## Credits

- Inspired by **Slay the Spire** by Mega Crit.
- Enemy sprites from [CraftPix](https://craftpix.net/) free asset packs (see `/Textures/Enemies/*/readme.txt` for individual licenses).

---

## License

This is a personal learning project — no formal license yet. Feel free to read and learn from the code.
