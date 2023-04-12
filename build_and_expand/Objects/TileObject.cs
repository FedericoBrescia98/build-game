using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace build_and_expand.Objects
{
    public class TileObject
    {
        public int ObjectId { get; set; } = 1;
        public int TerrainId { get; set; } = 1;

        #region CONSTRUCTION COSTS

        public int GoldCost { get; set; } = 0;
        public int WoodCost { get; set; } = 0;
        public int IronCost { get; set; } = 0;
        public int StoneCost { get; set; } = 0;
        public int WorkersCost { get; set; } = 0;
        public int FoodCost { get; set; } = 0;

        #endregion

        #region OUTPUTS

        public int GoldCycleOutput { get; set; } = 0;
        public int WoodCycleOutput { get; set; } = 0;
        public int IronCycleOutput { get; set; } = 0;
        public int StoneCycleOutput { get; set; } = 0;
        public int WorkersCycleOutput { get; set; } = 0;
        public int FoodCycleOutput { get; set; } = 0;

        public int GoldStaticOutput { get; set; } = 0;
        public int WoodStaticOutput { get; set; } = 0;
        public int IronStaticOutput { get; set; } = 0;
        public int StoneStaticOutput { get; set; } = 0;
        public int WorkersStaticOutput { get; set; } = 0;
        public int FoodStaticOutput { get; set; } = 0;

        #endregion
    }
}
