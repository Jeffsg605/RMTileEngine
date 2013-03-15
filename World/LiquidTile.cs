using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMTileEngine.World
{
	/// <summary>
	/// The class that will eventually represent liquid tiles and have fields unique to liquids.
	/// </summary>
	public class LiquidTile : Tile
	{
		/// <summary>
		/// Standard constructor directly to the tile constructor.
		/// </summary>
		/// <param name="id">The unique id of this tile.</param>
		/// <param name="name">The unique name of this tile.</param>
		public LiquidTile(int id, string name)
			: base(id, name)
		{

		}
	}
}