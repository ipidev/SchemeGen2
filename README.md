# Ultimate Random Scheme Generator (SchemeGen2)

The Ultimate Random Scheme Generator automatically creates playable game types (schemes) for [Worms Armageddon by Team17](https://www.team17.com/games/worms-armageddon/). Inspired by the original SchemeGen Pascal script I wrote a *long* time ago, this tool is capable of generating some pretty crazy gameplay experiences!

The kinds of schemes that get generated are determined by so-called "metaschemes" - schemes that generate schemes! Metaschemes specify the settings that will get randomly determined (e.g. turn time, ammo count) and the range of values that the setting could take. Metaschemes are written in XML, and so are freely customisable by the user.

[v1.0](https://github.com/ipidev/SchemeGen2/releases/tag/v1.0) was finished in 2020 but not publically released until 2022. Further changes are unlikely (unless me and my friends start playing Worms again!)

### Features
* Supports Armageddon v1/v2/v3 and World Party schemes.
* Randomises all weapons, settings, and extended options/RubberWorm parameters.
* Can create schemes in batches and/or using a preset seed.
* Includes several metaschemes for regular and specialised gameplay styles (Hysteria, Shopper, etc.)
* Generation is completely data-driven - no hard-coded techniques!

### Screenshot

![Screenshot of the Ultimate Random Scheme Generator](https://ipidev.net/self-made/ursg/screenshot.png)
