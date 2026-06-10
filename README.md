# [Survival Madness]
Date Started : 03 June 2026
Date End Prototype : 10 June 2026

## Game Overview
This project is a 2D turn-based survival deckbuilder set in a dark, Lovecraftian horror universe. The core mechanic revolves around strategic resource management through a dynamic card system. The player is trapped in a mysterious room and must manage their limited Action Points (Energy) to play cards that balance three vital stats: Health, Sanity, and Supplies.

Each turn represents a single day. The player draws a hand of cards—ranging from survival actions and resources to debilitating curses—and must make tactical decisions on how to spend their Energy. At the end of the player's turn, the oppressive environment automatically drains their Sanity and Supplies. To progress, players must survive these nightly drains and reach the Upgrade Stage (every 5 days) to draft new cards or permanently upgrade their core stats.

- Win Condition: Successfully survive the environment's passive drain until Day 10 (which unlocks an optional "Endless Mode" stretch goal).
- Lose Condition: The player's Health, Sanity, or Supplies is reduced to 0. Each depleted stat triggers a unique narrative Game Over scenario.

## How to Run
Step-by-step instructions to run your build.

- Engine & version used: Unity 2022.3.62f3
- Build location: https://king0333xxxx.itch.io/madnesssurvival

## Technical Decisions
Over the course of developing this prototype, several key architectural decisions were made to prioritize modularity, scalability, and clean code principles within a tight 7-day timeframe:

1. Data-Driven Architecture (Scriptable Objects)

- Decision: Cards and their effects are built entirely using Unity's ScriptableObjects (CardData and CardEffect).
- Reasoning: This separates static data from runtime logic. It allows game designers to create, tweak, and mix-and-match card effects directly in the Unity Inspector without touching a single line of C# code, adhering to the Open-Closed Principle.

2. Event-Driven UI (Observer Pattern)

- Decision: Implemented C# Action events (e.g., OnStatsChanged, OnHandUpdated) to handle communication between core systems and the UI.
- Reasoning: This completely decouples the View (UI) from the Model (Stats/Deck). UI scripts like UIStatsManager only update when an event is invoked, eliminating the need for expensive Update() polling and preventing "spaghetti code."

3. State Machine for Turn Logic

- Decision: Used an enum-based Finite State Machine in the GameManager (StartRun, PlayerTurn, EnvironmentTurn, UpgradeStage).
- Reasoning: Deckbuilders require strict phase sequencing. A centralized State Machine ensures that players cannot exploit the game (e.g., trying to play cards during the environment's turn) and makes adding new phases (like the Upgrade Stage) straightforward.

4. Finite Deck Management & Fisher-Yates Shuffle

- Decision: The deck is treated as physical lists of cards moving between Draw, Hand, and Discard piles, utilizing the O(n) Fisher-Yates algorithm for shuffling.
- Reasoning: Instead of relying on pure RNG/Gacha for card draws, maintaining physical lists ensures accurate deckbuilding mechanics where the probability of drawing a card shifts dynamically based on the remaining deck composition.

5. Dynamic Curse & Status System

- Decision: Separated negative card effects into two logic flows: "Permanent Passive Drains" triggered at End Turn, and "Aura Effects" (e.g., temporary Max Health penalties) calculated dynamically via Getters.
- Reasoning:This allows for complex, stackable debuffs without permanently corrupting base player data.

6. Modular Daily Event System

- Decision: Replaced hardcoded nightly stat drains with a DailyEventData ScriptableObject system.
- Reasoning: Events (like a "Blood Moon") are triggered based on probability or specific day intervals, handled safely through Coroutines to pause the game loop and deliver narrative pacing.

7. Dynamic Prefab Instantiation

- Decision: Make Diferent Card Template According Card Type.
- Reasoning: To support diverse card visuals, the UIHandContainer uses a lightweight factory approach, mapping CardType enums to distinct UI prefabs (Action, Resource, Curse, Upgrade) before instantiation.

8. Coroutine-Based Visual State Machine

- Decision: Make Avatar Reaction to Player Decision
- Reasoning: Implemented temporary 1-second UI avatar reactions (interrupt states) on top of permanent stat-based base expressions without cluttering the Update() loop.

What was deliberately NOT done:

- No Enemy AI: To keep the scope achievable in 7 days and focus purely on card system architecture, traditional enemies were replaced with an "Environmental Drain" mechanic.
- World Space Cards: Cards are rendered entirely in the UI Canvas rather than 2D World Space. This leverages Unity's HorizontalLayoutGroup to automatically handle complex card spacing and sorting mathematics.
- Object Pooling for Hand Cards: Given the maximum hand size is small (around 4-7 cards), standard Instantiate and Destroy were used for UI Hand rendering. Building a robust object pool was deemed an unnecessary premature optimization for this specific prototype scope.

## What I Would Do With More Time
If I had an additional 2–3 days to expand this prototype, I would prioritize the following:

- Drag-and-Drop Interaction: Transition from the current "Click-to-Play" system to a tactile drag-and-drop system (using IBeginDragHandler, IDragHandler, etc.) with visual targeting arrows for better game feel.
- Audio Manager Integration: Implement an event-driven Audio System to trigger visceral SFX when Sanity drops, or when drawing/discarding cards.
- Advanced Card Synergies: Add an Event Bus to track "cards played this turn" to enable synergy mechanics (e.g., "Deal double Effect if the previous card was an Action card").
- Save/Load Run State: Implement JSON serialization to save the player's current deck, day, and stats, allowing them to quit and resume mid-run.

## Known Issues
- UI Layout Recalculation Snapping: Because cards are instantiated simultaneously and rely on Unity's HorizontalLayoutGroup, there is occasionally a one-frame visual "snap" as the layout forces the cards into their new positions.
- Lack of Discard/Draw Pile Visual Browsing: The player currently cannot click the Draw/Discard piles to see a pop-up of the exact cards remaining in those lists.
```

### Scope Discipline
You have 5–7 days. **Timebox deliberately.** A working, well-structured prototype that does one thing well is worth far more than an ambitious scope that is broken or incomplete. If you run out of time, document what is missing and describe how you would have approached it.
