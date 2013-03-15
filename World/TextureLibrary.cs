using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace RMTileEngine.World
{
	/// <summary>
	/// Class that stores all of the loaded textures for the tiles.
	/// </summary>
	internal static class TextureLibrary
	{
		//Dictionary of all loaded Tile textures
		public static Dictionary<string, Texture2D> TileTextures = new Dictionary<string, Texture2D>();

		//ContentManager from the main game used to load content
		private static ContentManager Content;

		private static string texturePath;

		public static void Init(ContentManager content, string path)
		{
			Content = content;
			texturePath = path;
		}

		public static void AddTextureForTile(Tile t)
		{
			TileTextures.Add(t.Name, Content.Load<Texture2D>(texturePath + t.GetFileName()));
		}

		public static Texture2D GetTileTexture(Tile t)
		{
			return GetTileTexture(t.Name);
		}

		public static Texture2D GetTileTexture(string name)
		{
			if (!TileTextures.ContainsKey(name))
				throw new RMTEException("There is no loaded texture for the Tile with name \"" +name+  "\".");

			return TileTextures[name];
		}

		public static Dictionary<string, Texture2D> GetTextureLibrary()
		{
			return TileTextures;
		}
	}
}