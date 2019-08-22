# Spoiler warning

This is the developer commentary of Yet Another Asteroid Clone. You are strongly encouraged to play the game and understand what it really is about before reading this document.

---

I have had the idea of making a meta-game to mock modern games for a while. Games should be simple - insert disc, press button, start playing, yet games today are anything but that. You need to log in to the platform's account, log in to the game's account, download updates to console firmware, download updates to the game, wait for 2 minutes of loading, look at 10 logos and copyright statements, watch 5 minutes of story, go through a forced 30-minute tutorial, before you can actually get to the fun stuff. This is absurd. Each individual step on its own does make sense, because it is necessary to, say, warn people about risks of seizures, but do we really need every single one of these steps? How about at least options to skip some?

Anyway, with the ProgrammerHumor hackathon I finally have the motivation to start working on this idea. It would be a simple and fun game, but with as much frustration before the actual game as possible. I threw in everything I could think of: disclaimers, licenses, story, updates, accounts, announcements, settings, tutorial, you name it. None of them are skippable at the beginning.

Now as a developer naturally I need a dev path, or backdoor, to skip these time-consuming things. But I went one step beyond that: why not use the chance to mock loot boxes as well? We have seen big games lock important stuff behind loot boxes before (looking at you EA), so I shall do the same: lock the ability to skip stuff behind loot boxes. And thus the Space Station is born.

There is a time limit to this hackathon so I cannot take the time to produce assets, and have to rely on external resources. To make matters worse I am forced to allow commercial use of this open source project, so I cannot use any asset whose license does not allow commercial use. Therefore I am truly thankful that Freesound, Google Fonts and Pixabay exist. Without any of them it would be near impossible to finish this game on time. If I still feel like it after the hackathon ends, I may compose an original soundtrack for this game as I always do.

Finally, aside from the Space Station there is another hidden dev path in the "Intro" scene. Read Assets\Scripts\Ui\Intro.cs to find out.