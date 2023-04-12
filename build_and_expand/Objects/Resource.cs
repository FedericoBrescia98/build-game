using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace build_and_expand.Objects
{
    public class Resource : TileObject
    {
        public Resource(List<float> settings, List<int> costs,
            List<int> cycleOutputs, List<int> staticOutputs)
        {
            // set tileobject properties
            ObjectId = (int)settings[0];

            // set outputs per cycle
            GoldCycleOutput = cycleOutputs[0];
            WoodCycleOutput = cycleOutputs[1];
            IronCycleOutput = cycleOutputs[2];
            StoneCycleOutput = cycleOutputs[3];
            WorkersCycleOutput = cycleOutputs[4];
            FoodCycleOutput = cycleOutputs[5];

            // set static outputs
            GoldStaticOutput = staticOutputs[0];
            WoodStaticOutput = staticOutputs[1];
            IronStaticOutput = staticOutputs[2];
            StoneStaticOutput = staticOutputs[3];
            WorkersStaticOutput = staticOutputs[4];
            FoodStaticOutput = staticOutputs[5];

            // set constuction costs
            GoldCost = costs[0];
            WoodCost = costs[1];
            IronCost = costs[2];
            StoneCost = costs[3];
            WorkersCost = costs[4];
            FoodCost = costs[5];
        }

        public static TileObject Water()
        {
            return new TileObject()
            {
                ObjectId = 0
            };
        }

        public static TileObject Tree()
        {
            return new TileObject()
            {
                ObjectId = 2
            };
        }

        public static TileObject Ore()
        {
            return new TileObject()
            {
                ObjectId = 3
            };
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
