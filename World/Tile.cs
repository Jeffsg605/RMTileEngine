using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMTileEngine.World
{
	/// <summary>
	/// This class is the basic class for tiles that go into tile maps. Each tile has a wide variety of aspects that can be
	/// changed or overridden to make your own tile types. Upon creating a new tile, it will be automatically added to the
	/// TileDirectory.
	/// </summary>
	public class Tile
	{
		/// <summary>
		/// The ID of the block used for memory and saving maps. The id can only be used once.
		/// </summary>
		public int ID
		{
			get;
			protected set;
		}
		/// <summary>
		/// The name of the block. The name can only be used once.
		/// </summary>
		public string Name
		{
			get;
			protected set;
		}
		/// <summary>
		/// If the block responds to gravity and will fall when the space under it is open.
		/// </summary>
		public bool HasGravity = false;
		/// <summary>
		/// If the block should always be added to the update list no matter the circumstances.
		/// </summary>
		public bool AlwaysUpdate = false;

		/// <summary>
		/// Generic contstructor that takes in the id and name of the tile.
		/// </summary>
		/// <param name="id">The unique id of this tile.</param>
		/// <param name="name">The unique name of this tile.</param>
		public Tile(int id, string name)
		{
			//Check for id conflicts
			if (TileDirectory.idDictionary.ContainsKey(id))
				throw new Exception(String.Format("The id ({0}) is already assigned to another tile ({1}). You cannot reuse ids.",
					id, TileDirectory.idDictionary[id]));
			else
			{
				this.ID = id;
				TileDirectory.idDictionary.Add(id, this);
			}

			//Check for name conflicts
			if (TileDirectory.nameDictionary.ContainsKey(name))
				throw new Exception(String.Format("The name ({0}) is already assigned to another tile ({1}). You cannot reuse names.",
					name, TileDirectory.nameDictionary[name]));
			else
			{
				this.Name = name;
				TileDirectory.nameDictionary.Add(name, this);
			}

			TileDirectory.AddTile(this);
		}

		/// <summary>
		/// Overridable function to change what path the texture file is at.
		/// </summary>
		/// <returns>The path to the folder that contains the texture file for this tile.</returns>
		public virtual string GetTexturePath()
		{
			return TileEngine.GlobalInstance.TexturePath;
		}

		/// <summary>
		/// Overrideable function that allows the tile to pull its texture from a file not the same as the tile name.
		/// </summary>
		/// <returns>The name of the texture file that this tile pulls from.</returns>
		public virtual string GetFileName()
		{
			return this.Name;
		}

		/// <summary>
		/// Overrides the toString function to return the name of the block.
		/// </summary>
		/// <returns>The name of the block.</returns>
		public override string ToString()
		{
			return Name;
		}
	}
}