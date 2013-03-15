using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RMTileEngine.World
{
	/// <summary>
	/// This class holds the array of tiles that makes up the map. It includes many methods for accessing and mutating various
	/// components of the map.
	/// </summary>
	public class TileMap
	{
		/// <summary>
		/// The 2D array of chunks that makes up the actual TileMap.
		/// </summary>
		protected Chunk[,] chunkGrid;
		/// <summary>
		/// The list of chunks to draw on this frame.
		/// </summary>
		protected List<Chunk> drawableChunks;
		/// <summary>
		/// The width of the Tilemap.
		/// </summary>
		public int Width
		{
			get;
			protected set;
		}
		/// <summary>
		/// The height of the Tilemap.
		/// </summary>
		public int Height
		{
			get;
			protected set;
		}
		/// <summary>
		/// Width of the TileMap in chunks.
		/// </summary>
		public int ChunkWidth
		{
			get;
			private set;
		}
		/// <summary>
		/// Height of the TileMap in chunks.
		/// </summary>
		public int ChunkHeight
		{
			get;
			private set;
		}
		/// <summary>
		/// If the tilemap uses gravity to affect the tiles
		/// </summary>
		public GravityDirection TileGravityDirection = GravityDirection.DOWN;
		/// <summary>
		/// If the tilemap uses gravity to affect the entity.
		/// </summary>
		public GravityDirection EntityGravityDirection = GravityDirection.DOWN;
		/// <summary>
		/// The level of zoom for the map. This number is the exact width of the tiles being drawn.
		/// </summary>
		public int ZoomLevel = 20;
		/// <summary>
		/// The X Offset of this grid in pixels.
		/// </summary>
		public int XOffset
		{
			get;
			private set;
		}
		/// <summary>
		/// The Y Offset of this grid in pixels.
		/// </summary>
		public int YOffset
		{
			get;
			private set;
		}
		/// <summary>
		/// The radius around the player in chunks in which to keep chunks loaded
		/// </summary>
		public int LoadRadius = 5;
		/// <summary>
		/// The name identifier for this tilemap.
		/// </summary>
		public string Name = "";
		/// <summary>
		/// The tile coordinate from which the Load radius for chunk loading starts.
		/// </summary>
		public Vector2 ChunkLoadingOrigin = new Vector2(0, 0);

		#region Debug Variables
		/// <summary>
		/// The amount of blocks that were updated on the last call of the Update() function.
		/// </summary>
		public int LastUpdateCount
		{
			get;
			private set;
		}
		/// <summary>
		/// The amount of tiles drawn in the last frame.
		/// </summary>
		public int LastDrawCount
		{
			get;
			private set;
		}
		#endregion

		/// <summary>
		/// Generic constructor with random generation.
		/// </summary>
		/// <param name="width">Initial Width of the TileMap in chunks.</param>
		/// <param name="height">initial Height of the TileMap in chunks.</param>
		public TileMap(int width, int height)
		{
			this.ChunkWidth = width;
			this.ChunkHeight = height;
			this.Width = width * 20;
			this.Height = height * 20;
			chunkGrid = new Chunk[width, height];
			this.initalizeChunkGrid();
			(new StandardGenerator()).Generate(this);
		}

		/// <summary>
		/// Constructor used to create a new map completely filled with one Tile.
		/// </summary>
		/// <param name="width">Width of the TileMap.</param>
		/// <param name="height">Height of the TileMap.</param>
		/// <param name="t">The tile to fill the map with.</param>
		public TileMap(int width, int height, Tile t)
		{
			this.ChunkWidth = width;
			this.ChunkHeight = height;
			this.Width = width * 20;
			this.Height = height * 20;
			chunkGrid = new Chunk[width, height];
			this.initalizeChunkGrid();
			(new FullMapGenerator(t)).Generate(this);
		}

		/// <summary>
		/// Constructor used to create a new map using a custom terrain generator.
		/// </summary>
		/// <param name="width">Width of the TileMap.</param>
		/// <param name="height">Height of the TileMap.</param>
		/// <param name="generator">The custom generator used to generate the terrain for this map.</param>
		public TileMap(int width, int height, ICustomGenerator generator)
		{
			this.ChunkWidth = width;
			this.ChunkHeight = height;
			this.Width = width * 20;
			this.Height = height * 20;
			chunkGrid = new Chunk[width, height];
			this.initalizeChunkGrid();
			generator.Generate(this);
		}

		/// <summary>
		/// Called every frame to compile the update list and then update.
		/// </summary>
		/// <param name="gameTime">A snapshot of timing values in the game.</param>
		public void Update(GameTime gameTime)
		{
			updateChunkLoadingDefinitions();
			buildUpdateList();

			foreach (Vector2 vec in blocksToUpdate)
			{
				int x = (int)vec.X;
				int y = (int)vec.Y;
				Tile t = this.GetTileAt(x, y);

				if (t.HasGravity)
					updateGravity(x, y);
				
				if(t is LiquidTile)
					updateLiquid(x, y);
			}
		}

		/// <summary>
		/// Called every frame to draw the TileGrid.
		/// </summary>
		/// <param name="batch">The spritebatch to draw the grid with.</param>
		public void Draw(SpriteBatch batch)
		{
			updateChunksToDraw();

			LastDrawCount = 0;
			int screenBlockWidth = TileEngine.GlobalInstance.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / this.ZoomLevel;
			int screenBlockHeight = TileEngine.GlobalInstance.Game.GraphicsDevice.PresentationParameters.BackBufferHeight / this.ZoomLevel;

			foreach (Chunk c in drawableChunks)
			{

			}
		}

		/// <summary>
		/// Offsets the map on screen. Limits the map movement to always keep it on screen.
		/// </summary>
		/// <param name="x">The x distance to offset in pixels.</param>
		/// <param name="y">The y distance to offset in pixels.</param>
		public void OffsetMap(int x, int y)
		{
			XOffset += x;
			YOffset += y;

			if (XOffset > 0)
				XOffset = 0;
			else if (XOffset < -Width * this.ZoomLevel + TileEngine.GlobalInstance.Game.GraphicsDevice.PresentationParameters.BackBufferWidth)
				XOffset = -Width * this.ZoomLevel + TileEngine.GlobalInstance.Game.GraphicsDevice.PresentationParameters.BackBufferWidth;

			if (YOffset > 0)
				YOffset = 0;
			else if (YOffset < (-Height * this.ZoomLevel) + TileEngine.GlobalInstance.Game.GraphicsDevice.PresentationParameters.BackBufferHeight)
				YOffset = -Height * this.ZoomLevel + TileEngine.GlobalInstance.Game.GraphicsDevice.PresentationParameters.BackBufferHeight;
		}

		/// <summary>
		/// Offsets the grid on screen to a specific point.
		/// </summary>
		/// <param name="x">x position to offset the grid to.</param>
		/// <param name="y">y position to offset the grid to.</param>
		public void OffsetGridTo(int x, int y)
		{
			XOffset = x;
			YOffset = y;
		}

		/// <summary>
		/// Used to get a tile at a certain location. Checks to see if the tile is inside the map before returning.
		/// </summary>
		/// <param name="x">X coordinate of the tile to be returned.</param>
		/// <param name="y">Y coordinate of the tile to be returned.</param>
		/// <returns>The valid tile at the coordinates (x,y), if any.</returns>
		public Tile GetTileAt(int x, int y)
		{
			if (this.IsInRange(x, y))
			{
				Vector2 chunk = GetChunkForTileAt(x, y);
				Vector2 dist = GetDistanceFromChunkEdge(x, y);
				return chunkGrid[(int)chunk.X, (int)chunk.Y].GetTileAt((int)dist.X, (int)dist.Y);
			}

			return null;
		}

		/// <summary>
		/// Used to change a single tile.
		/// </summary>
		/// <param name="x">X coordinate of the tile to be changed.</param>
		/// <param name="y">Y coordinate of the tile to be changed.</param>
		/// <param name="t">The tile to change to.</param>
		/// <returns>If the tile was inside the grid and successfully changed.</returns>
		public bool SetTileAt(int x, int y, Tile t)
		{
			if (this.IsInRange(x, y))
			{
				Vector2 chunk = GetChunkForTileAt(x, y);
				Vector2 dist = GetDistanceFromChunkEdge(x, y);
				chunkGrid[(int)chunk.X, (int)chunk.Y].SetTileAt((int)dist.X, (int)dist.Y, t);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Used to set a square area to the same tile.
		/// </summary>
		/// <param name="x">Starting X coordinate.</param>
		/// <param name="y">Starting Y coordinate.</param>
		/// <param name="width">Width of area to be set.</param>
		/// <param name="height">Height of area to be set.</param>
		/// <param name="t">The tile to set the area to.</param>
		/// <returns>If the starting coordinates (x,y) were inside of the grid.</returns>
		public bool SetTileAtRange(int x, int y, int width, int height, Tile t)
		{
			for (int i = x; i <= width; i++)
				for (int j = y; j <= height; j++)
					this.SetTileAt(i, j, t);

			if (this.IsInRange(x, y))
				return true;

			return false;
		}

		/// <summary>
		/// Gets the chunk that the tile at (x, y) is in.
		/// </summary>
		/// <param name="x">The x coordinate to check.</param>
		/// <param name="y">The y corrdinate to check.</param>
		/// <returns>Returns the chunk coordinates of the tile at (x, y)</returns>
		public Vector2 GetChunkForTileAt(int x, int y)
		{
			return new Vector2((int)(x / 20), (int)(y / 20));
		}

		/// <summary>
		/// Gets the distance of the tile from the edge of the chunk it is in.
		/// </summary>
		/// <param name="x">The x coordinate to check.</param>
		/// <param name="y">The y coordinate to check.</param>
		/// <returns>The x and y distance of (x, y) from the edge of the containing chunk.</returns>
		public Vector2 GetDistanceFromChunkEdge(int x, int y)
		{
			Vector2 chunkAt = GetChunkForTileAt(x, y);

			return new Vector2(x - chunkAt.X * 20 - 1, y - chunkAt.Y * 20 - 1);
		}

		/// <summary>
		/// Checks if the provided coordinates are within the map bounds.
		/// </summary>
		/// <param name="x">The X coordinate to check.</param>
		/// <param name="y">The Y coordinate to check.</param>
		/// <returns>If (x,y) is within the TileMap.</returns>
		public bool IsInRange(int x, int y)
		{
			return (x >= 0 && x < Width && y >= 0 && y < Height);
		}

		/// <summary>
		/// Returns a copy of this TileMap, not the actual object itself.
		/// </summary>
		/// <param name="tiles">Whether or not to copy all of the tiles exactly, or just the size.</param>
		/// <returns>A exact copy of this TileMap if tiles is true, or an empty TileMap of the same size if tiles is false.</returns>
		public TileMap Copy(bool tiles)
		{
			if (!tiles)
				return new TileMap(this.ChunkWidth, this.ChunkHeight, new EmptyGenerator());

			//TODO return a copy of this Tilemap
			return null;
		}

		/// <summary>
		/// The list of the blocks to update on each update call.
		/// </summary>
		protected List<Vector2> blocksToUpdate = new List<Vector2>();
		/// <summary>
		/// Clears the previous update list and compiles a new one.
		/// </summary>
		protected void buildUpdateList()
		{
			blocksToUpdate.Clear();
			LastUpdateCount = 0;

			foreach (Chunk chunk in chunkGrid)
			{
				if (!chunk.Loaded)
					continue;

				int startX = chunk.ChunkX * 20;
				int startY = chunk.ChunkY * 20;

				if (chunk.ChunkY < (this.ChunkHeight - 1) && chunkGrid[chunk.ChunkX, chunk.ChunkY + 1].Loaded)
				{
					for (int i = startX; i < startX + 20; i++)
					{
						for (int j = startY; j < startY + 20; j++)
						{
							Tile t = this.GetTileAt(i, j);

							if(t.HasGravity && this.GetTileAt(i, j + 1) == TileDirectory.Empty)
								markNeedsUpdate(i, j);
							else if(t.AlwaysUpdate)
								markNeedsUpdate(i, j);
						}
					} 
				}
				else if (chunk.ChunkY < (this.ChunkHeight - 1) && !chunkGrid[chunk.ChunkX, chunk.ChunkY + 1].Loaded)
				{
					for (int i = startX; i < startX + 20; i++)
					{
						for (int j = startY; j < startY + 19; j++)
						{
							Tile t = this.GetTileAt(i, j);

							if (t.HasGravity && this.GetTileAt(i, j + 1) == TileDirectory.Empty)
								markNeedsUpdate(i, j);
							else if (t.AlwaysUpdate)
								markNeedsUpdate(i, j);
						}  
					}
				}
			}
		}

		/// <summary>
		/// Adds the specified coordinates to be updated if not already marked for an update.
		/// </summary>
		/// <param name="x">The x coordinate to mark.</param>
		/// <param name="y">The y coordinate to mark.</param>
		protected void markNeedsUpdate(int x, int y)
		{
			if (!blocksToUpdate.Contains(new Vector2(x, y)))
			{
				blocksToUpdate.Add(new Vector2(x, y));
			}
		}

		/// <summary>
		/// Updates the block a (x,y) for gravity.
		/// </summary>
		/// <param name="x">The x coordinate to update.</param>
		/// <param name="y">The y coordinate to update.</param>
		protected void updateGravity(int x, int y)
		{
			Tile t = this.GetTileAt(x, y);
			Tile t2 = this.GetTileAt(x, y + 1);

			if (t2 == TileDirectory.Empty)
			{
				this.SetTileAt(x, y + 1, t);
				this.SetTileAt(x, y, TileDirectory.Empty);
			}
		}

		/// <summary>
		/// Updates the block a (x,y) for liquid logic.
		/// </summary>
		/// <param name="x">The x coordinate to update.</param>
		/// <param name="y">The y coordinate to update.</param>
		protected void updateLiquid(int x, int y)
		{
			//TODO Update liquid logic
		}

		/// <summary>
		/// Updates which chunks are loaded and handles loading and unloading of chunks as needed.
		/// </summary>
		protected void updateChunkLoadingDefinitions()
		{
			//TODO calculate which chunks fall within the radius around the center point.
		}

		/// <summary>
		/// Updates the list of which chunks are onscreen and need to be drawn.
		/// </summary>
		protected void updateChunksToDraw()
		{
			//TODO calculate which chunks fall on screen so they can be drawn.
		}

		/// <summary>
		/// Initializes the chunk grid for the map.
		/// </summary>
		protected void initalizeChunkGrid()
		{
			for (int i = 0; i < this.ChunkWidth; i++)
				for (int j = 0; j < this.ChunkWidth; j++)
					this.chunkGrid[i, j] = new Chunk(this, i, j);
		}
	}

	/// <summary>
	/// The direction of gravity for the map.
	/// </summary>
	public enum GravityDirection
	{
		/// <summary>
		/// Towards bottom of the screen.
		/// </summary>
		DOWN,
		/// <summary>
		/// Towards the top of the screen.
		/// </summary>
		UP,
		/// <summary>
		/// Towards the right of the screen.
		/// </summary>
		RIGHT,
		/// <summary>
		/// Towards the left of the screen.
		/// </summary>
		LEFT,
		/// <summary>
		/// Denotes a map with no gravity.
		/// </summary>
		NONE
	}
}