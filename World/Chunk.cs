using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMTileEngine.World
{
	/// <summary>
	/// Holds a 20 by 20 area of tiles. Can load/unload all tiles in area at the same time, allowing for efficient
	/// run-time memory use and saving/loading. DO NOT MAKE INSTANCES OF THIS CLASS OUTSIDE OF A TILEMAP
	/// </summary>
	public class Chunk
	{
		//Actual grid of tiles within this chunk
		private Tile[,] tileGrid = new Tile[20, 20];

		/// <summary>
		/// The X location of the chunk on the tilemap
		/// </summary>
		public int ChunkX
		{
			get;
			private set;
		}
		/// <summary>
		/// The Y location of the chunk of the tilemap
		/// </summary>
		public int ChunkY
		{
			get;
			private set;
		}
		/// <summary>
		/// If the chunk is loaded into memory or not.
		/// </summary>
		public bool Loaded
		{
			get;
			private set;
		}

		//The Tilemap that this belongs to.
		private TileMap parent;

		internal Chunk(TileMap p, int x, int y)
		{
			this.ChunkX = x;
			this.ChunkY = y;
			this.parent = p;

			for (int i = 0; i < 20; i++)
				for (int j = 0; j < 20; j++)
					this.SetTileAt(i, j, TileDirectory.Empty);

			this.Loaded = true;
		}

		/// <summary>
		/// Draws all of the tiles in the chunk.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Draw(int x, int y)
		{

		}

		/// <summary>
		/// Gets the tile at (x, y) relative to this chunk.
		/// </summary>
		/// <param name="x">X coordinate of the tile to get.</param>
		/// <param name="y">Y coordinate of the tile to get.</param>
		/// <returns>The tile at (x, y).</returns>
		public Tile GetTileAt(int x, int y)
		{
			if (this.IsInRange(x, y))
				return tileGrid[x, y];

			return null;
		}

		/// <summary>
		/// Sets the tile at (x, y).
		/// </summary>
		/// <param name="x">X coordinate of the tile to set.</param>
		/// <param name="y">Y coordinate of the tile to set.</param>
		/// <param name="t">The tile to set (x, y) to.</param>
		/// <returns>If the tile was able to be changed.</returns>
		public bool SetTileAt(int x, int y, Tile t)
		{
			if (!this.IsInRange(x, y))
				return false;

			tileGrid[x, y] = t;
			return true;
		}

		/// <summary>
		/// Unloads the chunk from memory.
		/// </summary>
		/// <param name="save">If the chunk should be saved to the disk or not.</param>
		/// <returns>If the chunk was successfully unloaded and/or saved to the disk.</returns>
		public bool UnloadChunk(bool save)
		{
			if (!this.Loaded)
				return true;

			//TODO implement unloading chunks

			this.tileGrid = null;
			this.Loaded = false;
			return true;
		}

		/// <summary>
		/// Loads the chunk from a saved file.
		/// </summary>
		/// <returns>If the chunk was found in the file. False means the chunk was not found or corrupted, and was regenerated.</returns>
		public bool LoadChunk()
		{
			if (this.Loaded)
				return true;

			//TODO implement loading chunks
			this.Loaded = true;
			return true;
		}

		/// <summary>
		/// Checks to see if the provided coordinates are within the bounds of the chunk.
		/// </summary>
		/// <param name="x">X coordinate to check.</param>
		/// <param name="y">Y coordinate to check.</param>
		/// <returns>If the coordinated (x, y) are in range.</returns>
		public bool IsInRange(int x, int y)
		{
			if (!this.Loaded)
				throw new RMTEException("You are trying to access the unloaded chunk at (" + this.ChunkX + "," + this.ChunkY + ") for TileMap: " + parent.Name + ".");

			return (x >= 0 && x < 20 && y >= 0 && y < 20);
		}
	}
}