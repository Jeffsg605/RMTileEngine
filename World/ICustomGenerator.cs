using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMTileEngine.World
{
	/// <summary>
	/// The interface that can be implemented to create a custom terrain generator.
	/// </summary>
	public interface ICustomGenerator
	{
		/// <summary>
		/// The method that actually generates the terrain for the TileMap.
		/// </summary>
		/// <param name="tileMap">The TileMap to generate the terrain into.</param>
		void Generate(TileMap tileMap);
	}

	/// <summary>
	/// Implementation of the ICustomGenerator used to generate a ompletely empty map.
	/// </summary>
	public class EmptyGenerator : ICustomGenerator
	{
		/// <summary>
		/// Used to generate a completely empty map.
		/// </summary>
		/// <param name="tileMap">The TileMap to generate the terrain into.</param>
		public void Generate(TileMap tileMap)
		{
			//TODO implement an empty map.
		}
	}

	/// <summary>
	/// Implementation of the ICustomGenerator that generates a standard, Minecraft-Like terrain with ore from the OreDirectory.
	/// </summary>
	public class StandardGenerator : ICustomGenerator
	{
		/// <summary>
		/// Used to generate a map with standard terrain.
		/// </summary>
		/// <param name="tileMap">The TileMap to generate the terrain into.</param>
		public void Generate(TileMap tileMap)
		{
			//TODO implement standard map.
		}
	}

	/// <summary>
	/// Implementation of ICustomGenerator that created a TileMap completely full of one tile.
	/// </summary>
	public class FullMapGenerator : ICustomGenerator
	{
		/// <summary>
		/// The tile to generate with.
		/// </summary>
		public Tile TileToGenerate;

		/// <summary>
		/// Sets the tile to generate with.
		/// </summary>
		public FullMapGenerator(Tile t)
		{
			this.TileToGenerate = t;
		}

		/// <summary>
		/// Generates a Tilemap completely full of the specified tile.
		/// </summary>
		/// <param name="tileMap">TileMap to generate the terrain into.</param>
		public void Generate(TileMap tileMap)
		{
			for (int i = 0; i < tileMap.Width; i++)
				for (int j = 0; j < tileMap.Height; j++)
					tileMap.SetTileAt(i, j, TileToGenerate);
		}
	}
}