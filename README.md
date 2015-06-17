PixelWar
===========

PixelWar is a 2D tank-shooting game inspired by Worms, written in C# using the [MonoGame](http://www.monogame.net/) framework. 
It was developed in Visual Studio under Windows.  

In the beginning of the game, the tanks are placed on a pre-determined map (random map generator may be included at later stage). The tanks can move left and right, 
jump and shoot bazookas. The game allows control over the shooting power (visualised with concentric circles). PixelWar works in multiplayer mode.

   
Build
================

You will need Visual Studio (Visual Studio Community 2013 is free) and the MonoGame Pipeline tool 
which is part of the MonoGame framework. Grab the latest source code from Github and navigate to the `PixelWarGl` folder. 
Once there open the `PixelWarContent.mgcb` file with the Pipeline tool and build the content. 

You can then open `PixelWar.sln` from the root directory to build the game. 

Now enjoy the game!

Future Improvements 
===================

 - Include more weapons, e.g. granades.
 - Enable multiple tanks per player.