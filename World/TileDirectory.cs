using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMTileEngine.World
{
	/// <summary>
	/// The class that keeps track of the tiles added by the game implementing the engine. 
	/// </summary>
	internal static class TileDirectory
	{
		/// <summary>
		/// List of all added tiles.
		/// </summary>
		private static List<Tile> AddedTiles;

		public static void AddTile(Tile t)
		{
			if (AddedTiles.Contains(t))
				throw new RMTEException(String.Format("You cannot add the same tile ({0}) twice to the TileDirectory.", t));
		}

		#region Static Members
		/// <summary>
		/// The only included Tile, marks an completely empty tile.
		/// </summary>
		public static readonly Tile Empty;

		static TileDirectory()
		{
			AddedTiles = new List<Tile>();

			Empty = new Tile(0, "Empty");
		}

		//Dictionary of all Tiles and their ids
		internal static Dictionary<int, Tile> idDictionary = new Dictionary<int, Tile>();

		//List of all names of Tiles
		internal static Dictionary<string, Tile> nameDictionary = new Dictionary<string, Tile>();
		#endregion
	}
}