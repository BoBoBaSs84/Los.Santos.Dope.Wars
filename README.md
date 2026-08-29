[![CI](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/actions/workflows/ci.yml)

[![.NET](https://img.shields.io/badge/net48-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars)
[![C#](https://img.shields.io/badge/C%23-13.0-239120)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars)
[![Issues](https://img.shields.io/github/issues/BoBoBaSs84/Los.Santos.Dope.Wars)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/issues)
[![Commit](https://img.shields.io/github/last-commit/BoBoBaSs84/Los.Santos.Dope.Wars)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/commits/main/)
[![RepoSize](https://img.shields.io/github/repo-size/BoBoBaSs84/Los.Santos.Dope.Wars)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars)
[![License](https://img.shields.io/github/license/BoBoBaSs84/Los.Santos.Dope.Wars)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/blob/main/LICENSE)
[![Release](https://img.shields.io/github/v/release/BoBoBaSs84/Los.Santos.Dope.Wars)](https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/releases/latest)

# Los.Santos.Dope.Wars

This mod re-adds drug trafficking mini-missions that were scrapped during GTA5 development. I've inspired myself on GTA:Chinatown Wars drug dealing mechanics. There are multiple drug dealers found around Los Santos and Blaine County. They restock every night and their prices change. You can make a good profit by buying low and selling high.

## Features

### Street dealers

- **50 fixed dealer spots** spread across Los Santos and Blaine County. By default a dealer has to be **found** before he shows on the map — walk near his spot once and a tip-off names the neighbourhood he is in.
- Dealers spawn as random gang members, **armed** (a random weapon with 500 rounds, unequipped) and wearing **body armour**. They hold their ground; shoot or punch one and he turns hostile and fights back.
- At a wanted level the dealers **scatter** and stop trading until you cool off (threshold is configurable; 1 star by default).
- Kill a dealer and he is **gone for a few in-game days** (3 by default) — the cooldown is saved, so it survives a reload. He can optionally drop his weapon and some cash.

### The economy

- **Six drugs** — Cocaine, Heroin, Acid, Meth, Ketamine and Weed. The expensive drugs are the scarce ones, which is what keeps them worth trafficking.
- Once every **24 in-game hours** (configurable, 1–168) the whole market is re-dealt: a fresh quantity of each drug enters the world and is split **unevenly** across the roster. A tip-off tells you prices have moved.
- Each dealer's **price and buying appetite follow from his share** of that supply. A dealer sitting on a pile sells cheap (down to ~0.55× the reference price) and buys nothing; a dealer with nothing sells dear (up to ~1.45×) and is hungry to buy. Prices are fixed for the cycle — trading with a dealer moves his stock and his appetite, never his price.
- The dealer takes a **15% cut on both sides of every trade**, so buying and selling at the _same_ dealer is a loss. You make money by finding the glut and the drought and moving between them.
- **Special offers**: a chance each cycle (25% by default) that one known dealer is either dumping a drug cheap or paying over the odds for one. An anonymous tip names the drug and the area but not the dealer — a flagged blip is how you find which one it is.

### Trading

- The **buy / sell menu opens automatically** when you walk up to a dealer and closes when you walk off or pick up a wanted level. Drugs sit in a 2×3 tile grid — BUY on the left, SELL on the right — with a good-deal arrow that compares a dealer's price against what you actually paid.
- Pick an amount and confirm. A purchase is capped by your cash, your bag space and his stock; a sale is capped by what you are carrying and how much he still wants.
- Default controls (rebindable in the INI): **NumPad 8/2/4/6** to move, **NumPad 5** to confirm, **2** to start/stop a taco-van session, **E** to buy the taco front.

### The bag

- You carry drugs in a bag that starts at **100 slots** and grows by **10 per level**, with milestone bonuses on top (+200 at level 50, +300 more at level 100).
- **Dying or being arrested confiscates your entire carried stash** (both are configurable). Anything you have already sold is safe.

### Police

- Every completed dealer trade has a chance (**10% by default**) of being a **setup**: the dealer bolts and the police arrive with a 2-star trafficking wanted level.

### Levelling and perks

- Profitable sales earn **XP** — only the margin over what you paid counts, a loss earns nothing. There are **100 levels** on a steepening curve, with a ticker on every level and a milestone on every tenth:

| Level | Unlock                                                            |
| ----: | ----------------------------------------------------------------- |
|    10 | **Van stealing**                                                  |
|    20 | **Taco Bomb drug front** (purchasable)                            |
|    30 | Dealers charge you 10% less to buy                                |
|    40 | +25% XP from every sale                                           |
|    50 | +200 bag slots                                                    |
|    60 | Work the taco van faster before it draws heat                     |
|    70 | Dealers pay you 10% more to sell                                  |
|    80 | More taco-van customers                                           |
|    90 | The taco van no longer turns local gangs against you              |
|   100 | **Kingpin** — +300 bag slots, buy 15% cheaper and sell 15% higher |

### Van stealing (level 10)

Roughly **1 in 50 parked vans** (specific models) is carrying a hidden stash and gets a blip and a minimap flash. Steal one — you may pick up a wanted level in the act — shake the police, and drive it to the **nearest safe house**. The contents (a coin flip per drug, 1–14 grams each, whatever fits in your bag) are yours. Bail out of the van and the run is off.

### Taco Bomb drug front (level 20)

Buy the front ($200,000 by default; it is marked on the map) and use it to move product. Climb into the taco van, start a session, and **stop near a customer** to sell one random drug from your bag at a **0–50% markup** over market price. Selling too fast (five sales inside a tight window) adds a wanted star, and running the van marks you as fair game for the local gangs until level 90. A session ends when you stop it, run out of drugs, or leave the van, and reports what you made.

### Saving and settings

- Progress **autosaves** to `scripts\LSDW.sav` after every trade, discovery, delivery and milestone. Reloading the script cleans up every ped, blip and vehicle the mod spawned.
- A commented `scripts\LSDW.ini` is written on first run and is hand-editable — sections `[Keys]`, `[Economy]`, `[Dealers]` and `[Police]` cover the keybinds, the restock interval and market size, dealer behaviour and the police response. Every value is range-checked on load, so a bad edit falls back to a default instead of breaking the mod.

## Getting started

- Download and install Script Hook V (including the ASI loader). You may download it from http://www.dev-c.com/gtav/scripthookv/ or have a look here https://gtaforums.com/topic/932648-script-hook-v/
- Download the latest Script Hook V .NET and copy the ASI into your game folder. You may download it from https://github.com/scripthookvdotnet/scripthookvdotnet-nightly/releases
- Download the latest (stable ;) mod assembly from here https://github.com/BoBoBaSs84/Los.Santos.Dope.Wars/releases and copy the assembly into your scripts folder
  OR
- build it yourself, copy the binaries into your scripts folder

## Licensing

Los.Santos.Dope.Wars is licensed under the MIT License. See [LICENSE](LICENSE) for the full license text.
