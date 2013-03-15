RMTileEngine
============

#### Introduction
A simple 2D tile engine based on the XNA/MONOGAME .NET framework. This is currently very early in production, the
initial commit was only made in mid-March. It is based on a Windows Phone 8 class library, but it should work with any
other platform supported by Monogame. Made with Monogame 3.0, which you will need in order to develop for this, or to use 
it in any of your games or to play a game developed with this engine.

#### What You Need
Monogame 3.0: http://monogame.codeplex.com/releases/view/100041

#### How to Use This Engine
This section will be updated when we have a useable release. Keep track of the changelog in the Finalized folder.

For now, Tiles can be created by extending the Tile class, or just creating a new object of Tile. Tiles are often marked readonly,
but dont have to be. Just know that a non-readonly Tile can cause problems that may not be so obvious. Tiles are automatically added 
to the Engine in the base constructor. Textures are automatically added based on the name of the tile, but the Tile class contains 
overrideable functions that allow you to change that. Finally, there is a very basic implementation of a terrain generator interface, 
called ICustomGenerator, in which you can define a custom generate() function which can be passed into the constructor of a new map 
to generate the map with that generator.

#### For Developers
This was originally going to be a nice simple library for a school project, but I thought it could go further than that,
so I pushed it to GitHub.

If you want to help (and I openly welcome help), just submit a detailed Pull Request, for contact me to become a collaborator. If you do make a pull
request, please include a detailed changelog, using the next available build version (x.x.x.build#). Please do not make any
changes to the major or minor numbers, but the revision number can be changed if the update it large enough. Please do not
make a pull request if you are new to Monogame / C# / Programming in general, and please respect the coding style used
throughout the existing classes.

And yes, that includes commenting. The compiler will yell at you for not using proper XML comments.

#### Planned Features
* Custom filetype to save maps based on tile ids
* A Minecraft-like system of TileEntities that allow for Tiles with inventories and other functions
* Different kinds of tiles, like gas and liquid.
* An overlay, that allows extra textures to be drawn after everything else (like grass on dirt drawn in front of other players, or roofs)
* Highly customizable physical properties (gravity strength, wind, ect.)
* (Maybe) An item system, to standardize how storage in tiles works