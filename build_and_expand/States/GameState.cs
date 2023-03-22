using build_and_expand.Content;
using build_and_expand.Objects;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace build_and_expand.States
{
    // class for saving game data
    public class GameStateData
    {
        // time data
        public int Day { get; set; } = 1;
        public int Year { get { return Day / 365; } set { } }

        // player inventory data
        public InventoryData PlayerInventory { get; set; } = new InventoryData();

        // tiles of map data
        public List<Tile> MapTiles { get; set; } = new List<Tile>();

        public GameStateData()
        {
            Year = 1998;
        }
    }

    public class GameState : State
    {
        #region PROPS

        #region LOADING
        protected bool IsSaving = false;
        public bool IsLoaded = false;
        public string LoadingText { get; set; } = "Loading Game...";
        #endregion

        #region GAME CONTENT
        // gamecontent: holds all sprites, effects, sounds, fonts
        public GameContent GameContent { get; set; }
        #endregion

        #region MAP AND CAMERA
        private Map _currentMap { get; set; }
        public Map CurrentMap => _currentMap;
        private Camera _currentCamera { get; set; }
        public Camera CurrentCamera => _currentCamera;

        #endregion

        #region MOUSE & KEYBOARD STATES
        private KeyboardState _previousKeyboardState { get; set; }
        private MouseState _previousMouseState { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public MouseState MouseState { get; set; }
        #endregion

        #region GAME STATE DATA
        public GameStateData GameStateData { get; set; }
        private const float _timeCycleDelay = 5; // seconds
        private float _remainingDelay = _timeCycleDelay;

        public Tile CurrentlySelectedTile { get; set; }
        public Tile CurrentlyPressedTile { get; set; } = null;
        #endregion

        #region COMPONENTS
        public List<Component> Components { get; set; } = new List<Component>();
        private Texture2D CursorTexture { get; set; }
        public HUD GameHUD { get; set; }
        public TileObject SelectedObject { get; set; }
        public Tile CurrentlyHoveredTile { get; set; }
        #endregion

        #endregion

        #region METHODS

        #region CONSTRUCTOR
        public GameState(Game game, GraphicsDevice graphicsDevice, ContentManager content, bool newgame) : base(game, graphicsDevice, content)
        {
            GameContent = new GameContent(content);
            _graphicsDevice = graphicsDevice;
            GameStateData = new GameStateData();
            _currentCamera = new Camera(_graphicsDevice);
            CursorTexture = content.Load<Texture2D>("Sprites/UI/cursor");

            LoadLoadingScreen();
            Task.Run(() => LoadHUD());
            if(newgame is true)
            {
                Console.WriteLine($"Starting new game...");
                Task.Run(() => NewGame());
            }
            else
            {
                Console.WriteLine($"Loading previous game...");
                Task.Run(() => LoadGame());
            }
        }

        public void LoadLoadingScreen()
        {
            Vector2 loading_bar_dimensions = new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 8);
            Vector2 loading_bar_location =
                new Vector2(_graphicsDevice.Viewport.Width / 4, (_graphicsDevice.Viewport.Height / 4) * 2.5f);
        }

        public void LoadHUD()
        {
            GameHUD = new HUD(GameContent, OnMenu);
            Components.Add(GameHUD);
        }

        public void OnMenu()
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }
        #endregion

        #region HANDLE MAP DATA

        /// <summary>
        /// Declare the loading text
        /// </summary>
        public void NewGame()
        {
            LoadingText = $"Loading map...";
            LoadNewMap();
            LoadingText = $"Wrapping things up...";
            IsLoaded = true;
        }

        /// <summary>
        /// Load the new game map
        /// </summary>
        public void LoadNewMap()
        {
            _currentMap = new Map(GameContent);
            foreach(Tile tile in _currentMap.grid.GridTiles)
            {
                tile.Click += TileOnClick;
                tile.Pressed += TileOnPressed;
                tile.Pressing += TileCurrentlyPressed;
            }
        }

        /// <summary>
        /// Declare the loading text, load the map
        /// </summary>
        public void LoadGame()
        {
            LoadingText = $"Loading map...";
            IsLoaded = LoadMap();
        }

        /// <summary>
        /// Load Map/Game Data from savefile (default file)
        /// Set the gamestate data from savefile gamestate data
        /// </summary>
        /// <returns>True or false, based on if data is loaded successfully</returns>
        public bool LoadMap()
        {
            try
            {
                LoadingText = $"Reading save files...";
                // try to read map data from json file
                String data = File.ReadAllText("gameData.json");

                // if the data read isn't null or empty, load the map | else, throw exception
                if(string.IsNullOrEmpty(data).Equals(true))
                {
                    LoadingText = $"Map data corrupted... one moment...";
                    throw new NotSupportedException("Error Reading Map Data: Data is empty.");
                }
                else
                {
                    Console.WriteLine($"Loading map...");

                    // deserialize data to gamestate data object
                    GameStateData loadedGameStateData = JsonConvert.DeserializeObject<GameStateData>(data);

                    // set gamestate data
                    GameStateData = loadedGameStateData;

                    // initialize loaded map
                    _currentMap = new Map(GameContent);
                    foreach(Tile tile in loadedGameStateData.MapTiles)
                    {
                        _currentMap.grid[tile.PositionToMap.X, tile.PositionToMap.Y] = new Tile(GameContent, tile.Object, tile.Position);
                    }

                    LoadingText = $"Putting things back together...";
                    // for each _tileData loaded
                    Console.WriteLine("Map restored");
                }

                LoadingText = $"Looping through map data completed...";
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error Loading Map Data: {e.Message}.");
                return false;
            }
        }

        #endregion

        #region UPDATE
        public override void Update(GameTime gameTime)
        {
            if(IsLoaded)
            {
                // handle current state input (keyboard / mouse)
                HandleInput();

                // update map and camera
                try
                {
                    _currentMap.Update(gameTime, this);
                    _currentCamera.Update();
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error drawing map: " + e.Message);
                }

                // also update ui components
                foreach(Component c in Components)
                {
                    c.Update(gameTime, this);
                }

                // update timer (day cycle)
                float timer = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _remainingDelay -= timer;
                if(_remainingDelay <= 0)
                {
                    // update gamestate data and reset timer
                    Task.Run(() => UpdateGameState(gameTime));
                    _remainingDelay = _timeCycleDelay;
                }
            }
            else
            {
                // game state is still loading
            }
        }

        /// <summary>
        /// Asynchronous Method ran on Update when the daycycle timer/interval is surpassed
        /// Buildings are processed, the day is advanced, and the game is saved (every 10 days)
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateGameState(GameTime gameTime)
        {
            // process all building in game
            ProcessBuildings();

            // advance day
            GameStateData.Day += 1;
            int day = GameStateData.Day;

            // if day is a multiple of 10, save the game
            if((day % 10).Equals(0))
            {
                SaveGame();
            }
        }

        public void ProcessBuildings()
        {
            foreach(Tile tile in _currentMap.grid.GridTiles)
            {
                // if the object ID is greater than 100 (is a building)
                if(tile.Object.ObjectId >= 100)
                {
                    GameStateData.PlayerInventory.AddObjectCycleOutputs(tile.Object);
                }
            }

            // subtract from food the amount of food per worker in total buildings
            GameStateData.PlayerInventory.Food -= (2 * GameStateData.PlayerInventory.Workers);
        }

        /// <summary>
        /// Asynchronous method to save the game in it's current state
        /// Loops through all tiles and saves their respective TILEDATA
        /// Compiles all the necessarry data for the GameStateData
        /// Deletes the previous backup, saves current save as a new backup, and saves the current gamestate data as a savefile
        /// </summary>
        public void SaveGame()
        {
            if(IsSaving.Equals(true))
            {
                return;
            }

            try
            {
                // delete previous backups
                File.Delete("gameData_backup.json");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // backup old map data first
            try
            {
                // change filename to backup format
                File.Move($"gameData.json", "gameData_backup.json");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error backing up previous map data: " + e.Message);
            }

            IsSaving = true;

            // prepare game data class
            List<Tile> newMapTiles = new List<Tile>();
            foreach(Tile tile in _currentMap.grid.GridTiles)
            {
                newMapTiles.Add(tile);
            }
            GameStateData.MapTiles = newMapTiles;

            // get current state data file and save
            String jsonData = JsonConvert.SerializeObject(GameStateData);
            File.WriteAllText("gameData.json", jsonData);
            IsSaving = false;
            Console.WriteLine("Finished Saving Map.");
        }

        private void TileCurrentlyPressed(object sender, EventArgs e)
        {
            CurrentlyPressedTile = (Tile)sender;
        }

        private void TileOnPressed(object sender, EventArgs e)
        {
            CurrentlyPressedTile = (Tile)sender;
        }

        private void TileOnClick(object sender, EventArgs e)
        {
            // get mouse data
            Point mousePosition = Mouse.GetState().Position;
            Rectangle mouseRectangle = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);
            if(GameHUD.Intersects(mouseRectangle).Equals(true))
            {
                return;
            }

            // get the tile clicked on
            Tile tile = (Tile)sender;
            CurrentlySelectedTile = tile;

            // reset currently pressed tile
            CurrentlyPressedTile = null;

            // if selected object to build
            if(SelectedObject != null)
            {
                // error sound
                SoundEffect Sound = GameContent.GetSoundEffect(2);
                // try to place/construct a building
                try
                {
                    // does the tile have a valid objectid,
                    // and does the clicked tile not already have an object in it?
                    if(SelectedObject.ObjectId > 0 && tile.Object.ObjectId <= 100)
                    {
                        // get a correctly casted version of the selected obj
                        Building obj = (Building)SelectedObject;

                        // check balance to see if player can afford building
                        if(GameStateData.PlayerInventory.CanBuildObject(obj).Equals(false))
                        {
                            throw new Exception("Can't afford to place!");
                        }

                        // play build sound
                        Sound = GameContent.GetSoundEffect(1);

                        // apply building to tile object
                        _currentMap.grid[tile.PositionToMap.X, tile.PositionToMap.Y].Object = obj;

                        // take away values from inventory
                        GameStateData.PlayerInventory.RemoveObjectCosts(obj);

                        // add static builds value to inventory
                        GameStateData.PlayerInventory.AddObjectStaticOutputs(obj);
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine($"ERROR ON TILE PLACE: {exception.Message}");
                }
                Sound.Play();
            }

        }

        public void DeleteBuildingButtonClick()
        {
            if(CurrentlySelectedTile is null)
            {
                CursorTexture = GameContent.GetUiTexture(12);

                foreach(Tile t in _currentMap.grid.GridTiles)
                {
                    t.Click += TileOnDemolish;
                }
            }
            else
            {
                foreach(Tile t in _currentMap.grid.GridTiles)
                {
                    t.Click -= TileOnDemolish;
                }
                CurrentlySelectedTile = null;
            }
        }

        private void TileOnDemolish(object sender, EventArgs e)
        {
            // delete a building
            foreach(Tile t in _currentMap.grid.GridTiles)
            {
                if(t != CurrentlySelectedTile)
                {
                    continue;
                }
                t.ObjectDestroyed = true;

                // play poof sound
                SoundEffect Sound = GameContent.GetSoundEffect(4);
                Sound.Play();

                GameStateData.PlayerInventory.AddObjectDestroyedCosts(t.Object);
            }
        }
        #endregion

        #region HANDLE INPUTS
        public void HandleInput()
        {
            // set previous keyboardstate = keyboardstate;
            _previousKeyboardState = KeyboardState;
            _previousMouseState = MouseState;

            // get current keyboard and mouse state
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            if(MouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed)
            {
                CursorTexture = GameContent.GetUiTexture(11);

                SelectedObject = null;
                foreach(Tile t in _currentMap.grid.GridTiles)
                {
                    t.Click -= TileOnDemolish;
                }
                CurrentlySelectedTile = null;
            }
        }
        #endregion

        #region DRAW
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Point mousePoint = Mouse.GetState().Position;
            Vector2 mouseVector = new Vector2(mousePoint.X, mousePoint.Y);

            if(IsLoaded)
            {
                // TWO SPRITE BATCHES:
                // First batch is for the game itself, the map, npcs, all that live things
                // Second batch is for UI and HUD rendering - separate from camera matrixes and all that ingame things

                spriteBatch.Begin(SpriteSortMode.Deferred,
                                    BlendState.AlphaBlend,
                                    null,
                                    null,
                                    null,
                                    null,
                                    CurrentCamera.getTransformation());
                // draw game here
                try
                {
                    _currentMap.Draw(gameTime, spriteBatch);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error drawing map:" + e.Message);
                }
                spriteBatch.End();

                //--------------------------------------------------------

                spriteBatch.Begin();
                // draw UI / HUD here 
                foreach(Component c in Components)
                {
                    c.Draw(gameTime, spriteBatch);
                }

                if(SelectedObject != null)
                {
                    if(SelectedObject.ObjectId > 0)
                    {
                        // zoom camera for scaling object texture
                        float zoom = _currentCamera.Zoom;

                        Texture2D texture = GameContent.GetTileTexture(SelectedObject.ObjectId);
                        Vector2 pos = mouseVector - new Vector2((int)(texture.Width * zoom) / 2,
                                                                (int)(texture.Height * zoom) - ((int)(texture.Height * zoom) * 0.25f));
                        Rectangle rectangle = new Rectangle((int)pos.X, (int)pos.Y,
                                                            (int)(texture.Width * zoom),
                                                            (int)(texture.Height * zoom));
                        spriteBatch.Draw(texture, destinationRectangle: rectangle, color: Color.White);
                    }
                }
            }
            else
            {
                // game state hasnt finished loading
                // most of whats drawn in here is strictly UI so only one spritebatch should be needed
                spriteBatch.Begin(samplerState: SamplerState.PointClamp);

                Vector2 scale = new Vector2
                {
                    X = _graphicsDevice.Viewport.Width,
                    Y = _graphicsDevice.Viewport.Height
                };
                Vector2 dimensions = new Vector2
                {
                    X = scale.X / 2,
                    Y = scale.Y / 2
                };
                float x = (GameContent.GetFont(1).MeasureString(LoadingText).X / 2);
                float y = (GameContent.GetFont(1).MeasureString(LoadingText).Y / 2);

                // draw loading text
                spriteBatch.DrawString(GameContent.GetFont(1), LoadingText, dimensions, Color.White, 0.0f, new Vector2(x, y), 1.0f, SpriteEffects.None, 1.0f);
            }

            Point msp = Mouse.GetState().Position;
            Vector2 mp = new Vector2(msp.X, msp.Y);
            spriteBatch.Draw(CursorTexture, mp, Color.White);
            spriteBatch.End();
        }
        #endregion

        #endregion
    }
}