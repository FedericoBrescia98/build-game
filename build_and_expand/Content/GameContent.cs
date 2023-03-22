using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace build_and_expand.Content
{
    /// <summary>
    /// Class for storing dynamic content
    /// Uses t for generic type, types range from Texture2D to SoundEffect etc
    /// Uses Id to identify specific content data within lists
    /// Stores path for loading content from contentmanager
    /// Data is the actual content itself, loaded from the path. T is the generic type for the data
    /// </summary>
    /// <typeparam name="T">Generic Content Type</typeparam>
    public class ContentData<T>
    {
        public ContentData(int id, string path, ContentManager content)
        {
            Id = id;
            Path = path;
            Data = content.Load<T>(path);
        }

        public int Id { get; set; } = 0;
        public string Path { get; set; } = string.Empty;
        public T Data { get; set; } = default;
    }

    /// <summary>
    /// Content management class to store ContentData lists.
    /// Loads all textures needed within the game and can return specified content from lists
    /// </summary>
    public class GameContent
    {
        // where to load content from
        private readonly ContentManager _content;

        // private lists to store contentdata
        private List<ContentData<Texture2D>> UiTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<SpriteFont>> FontsList { get; set; } = new List<ContentData<SpriteFont>>();
        private List<ContentData<Texture2D>> TileTexturesList { get; set; } = new List<ContentData<Texture2D>>();
        private List<ContentData<SoundEffect>> SoundEffectsList { get; set; } = new List<ContentData<SoundEffect>>();

        // list accessors
        public List<ContentData<Texture2D>> UiTextures
        {
            get { return UiTexturesList; }
            set { UiTexturesList = value; }
        }

        public List<ContentData<SpriteFont>> Fonts
        {
            get { return FontsList; }
            set { FontsList = value; }
        }

        public List<ContentData<Texture2D>> TileTextures
        {
            get { return TileTexturesList; }
            set { TileTexturesList = value; }
        }

        public List<ContentData<SoundEffect>> SoundEffects
        {
            get { return SoundEffectsList; }
            set { SoundEffectsList = value; }
        }

        // get specific content data based on an id from a content type category
        public Texture2D GetUiTexture(int id)
        {
            return (from a in UiTextures
                where a.Id.Equals(id)
                select a.Data).SingleOrDefault<Texture2D>();
        }

        public SpriteFont GetFont(int id)
        {
            return (from a in Fonts
                where a.Id.Equals(id)
                select a.Data).SingleOrDefault<SpriteFont>();
        }

        public Texture2D GetTileTexture(int id)
        {
            return (from a in TileTextures
                where a.Id.Equals(id)
                select a.Data).SingleOrDefault<Texture2D>();
        }

        public SoundEffect GetSoundEffect(int id)
        {
            return (from a in SoundEffects
                where a.Id.Equals(id)
                select a.Data).SingleOrDefault<SoundEffect>();
        }

        // loads all textures within the game
        public GameContent(ContentManager content)
        {
            _content = content;

            LoadUITextures();
            LoadFonts();
            LoadTileTextures();
            LoadSoundEffects();
        }

        public void LoadUITextures()
        {
            int i = 1;
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/food", _content)); // 1
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/wood", _content)); // 2
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/people", _content)); // 3
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/bottomNavBar", _content)); // 4
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/buttonHouse", _content)); // 5
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/buttonLogCabin", _content)); // 6
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/buttonWindmill", _content)); // 7
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/calendarIcon", _content)); // 8
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/miniStretchButton", _content)); // 9
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/buttonDemolish", _content)); // 10
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/cursor", _content)); // 11
            UiTextures.Add(new ContentData<Texture2D>(i++, "Sprites/UI/cursorHammer", _content)); // 12
        }

        public void LoadFonts()
        {
            int i = 1;
            Fonts.Add(new ContentData<SpriteFont>(i++, "Fonts/slkscr", _content));
        }

        public void LoadTileTextures()
        {
            // Load Tile Effect Textures within negative range
            TileTextures.Add(new ContentData<Texture2D>(-2, "Sprites/Tiles/FX/smoke", _content));
            TileTextures.Add(new ContentData<Texture2D>(-1, "Sprites/Tiles/FX/waterFX", _content));

            int i = 0;
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Natural/waterBase", _content)); // 0
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Natural/grass", _content)); // 1
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Natural/tree", _content)); // 2
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Natural/rock", _content)); // 3

            i = 100;
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Buildings/house1", _content)); // 100
            i = 200;
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Buildings/logCabin", _content)); // 200

            i = 500;
            TileTextures.Add(new ContentData<Texture2D>(i++, "Sprites/Tiles/Buildings/windMill", _content)); // 500
        }

        public void LoadSoundEffects()
        {
            int i = 1;
            SoundEffectsList.Add(new ContentData<SoundEffect>(i++, "Sounds/FX/Build", _content)); // 1
            SoundEffectsList.Add(new ContentData<SoundEffect>(i++, "Sounds/FX/Error", _content)); // 2
            SoundEffectsList.Add(new ContentData<SoundEffect>(i++, "Sounds/FX/Click", _content)); // 3
            SoundEffectsList.Add(new ContentData<SoundEffect>(i++, "Sounds/FX/Poof", _content)); // 4        
            //SoundEffectsList.Add(new ContentData<SoundEffect>(i++, "Sounds/FX/Glimmer", _content)); // 5

        }
    }

}
