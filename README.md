# SpeedMod

**SINCE BattleTech Patch 1.1 THIS MOD IS NOT A NECESSITY ANYMORE**

BattleTech mod (using ModTek and DynModLib) that allows to speed up gameplay.

## Requirements
** Warning: Uses the experimental BTML mod loader and ModTek **

either
* install BattleTechModTools using [BattleTechModInstaller](https://github.com/CptMoore/BattleTechModTools/releases)

or
* install [BattleTechModLoader](https://github.com/Mpstark/BattleTechModLoader/releases) using [instructions here](https://github.com/Mpstark/BattleTechModLoader)
* install [ModTek](https://github.com/Mpstark/ModTek/releases) using [instructions here](https://github.com/Mpstark/ModTek)
* install [DynModLib](https://github.com/CptMoore/DynModLib/releases) using [instructions here](https://github.com/CptMoore/DynModLib)

## Features

BattleTech 1.1 introduced a fast forward key, however it's not a toggle and the animation acceleration feels way off.

This mod modifies the behavior of the fast forward key and the acceleration curve of the animations.

Setting | Type | Default | Description
--- | --- | --- | ---
FastForwardKeyIsToggle | bool | true | true allows the space bar to be used as a toggle instead of having to spam it at the time
SpeedUpIsConstant | bool | true | true means that the animations don't accelerate/deaccelerate anymore

The speed up factors are found as json patches in the mod folder, right now I use the same factor the mod used before 1.1 

## Download

Downloads can be found on [github](https://github.com/CptMoore/SpeedMod/).

## Install

After installing BTML, ModTek and DynModLib, put the mod into the \BATTLETECH\Mods\ folder and launch the game.

## Development

* Use git
* Use Visual Studio or DynModLib to compile the project
