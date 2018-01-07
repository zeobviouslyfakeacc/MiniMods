# MiniMods for _The Long Dark_

This repository contains a few (in my opinion) useful mods that only consist of a few lines of code each.

Here's a short overview of what they do:

### RememberBreakDownItem

This mod remembers the tool you last used to break down furniture. That way, you no longer need to constantly switch from "using a knife" to "no tool" when breaking down branches or curtains.

In the break down tool selection panel, it also enables the use of the A and D keys to switch between tools and the space bar to start breaking down the item.

### EnableStatusBarPercentages

This mod re-enables some labels in the _status / first aid screen_ that show a percentage value of how full the warmth, fatigue, thirst and hunger status bars are.

These labels were apparently hidden, but fortunately not removed, when the new UI was released.

### WildlifeBegone

This mod removes most of the animals that would usually roam the world and greatly increases their respawn times, thus making you play _The Long Dark_ quite a bit differently.

One can no longer set up a base in one spot and stay self-sufficient by hunting the same animals from the same spawn points over and over again.
Instead, one now needs to become a pilgrim that moves between the various regions, hunting for food.

### DisableTracking

_The Long Dark_ sends analytics data to a 3rd-party service called [GameAnalytics](https://gameanalytics.com/), even though the game never informs you about this.

The data collected is rather trivial:
- Places you've visited
- Animal encounters
- Afflictions
- Time slept & calories used per day
- etc.

However, the data sent also includes an ID that's linked to your hardware, so you can be tracked over multiple play sessions.
Thus, GameAnalytics will be able to tell when you're playing what game and for how long.
Moreover, GameAnalytics offers their service for free -- so how do they make money?
> [GameAnalytics is] supported by our parent company, Mobvista, due to the global reach and detailed insights we provide into the gaming market.

Mobvista, in turn, is a mobile advertizing company.
So by playing _The Long Dark_, you're enabling Mobvista to create "better", more personalized advertisements...

Raphael once promised me that users would be informed of this data collecting,
and offered to add a setting to opt-out of these analytics.
However, after more than a year, neither has been implemented.

Instead, I made my own opt-out switch, in form of a mod.

### DisableBreathEffect

Removes the annoying 2D breath cloud that appears every few seconds when it's cold.

This mod used to be more useful in older TLD versions where the breath cloud was almost solid white,
even when the player was in a pitch black room.
This made it hard to see items in the dark and was very annoying in general,
which is why I made this mod to remove the ~~vape cloud~~ breath effect.

### AddModdedToVersionString

This mod has now been integrated into the mod loader itself and is thus obsolete.
Before that, this used to be an "example mod" that was always installed when the game was first patched.

The mod simply modifies the version string shown in the main menu and writes a short notice to the debug log.