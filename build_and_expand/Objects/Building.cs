using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace build_and_expand.Objects
{
    public class Building : TileObject
    {
        public string Name { get; set; } = "Base Building";
        public bool RequiresRoad { get; private set; } = true;

        public Building(List<float> settings, List<int> costs,
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

            // set construction costs
            GoldCost = costs[0];
            WoodCost = costs[1];
            IronCost = costs[2];
            StoneCost = costs[3];
            WorkersCost = costs[4];
            FoodCost = costs[5];
        }

        #region GENERATE BUILDINGS FACTORY METHODS

        // construct a basic house
        public static Building BasicHouse()
        {
            List<float> settings = new List<float>()
            {
                100, // object id: 100 = BasicHouse
            };

            // set costs
            List<int> costs = new List<int>()
            {
                0, // gold
                25, // wood
                0, // iron
                0, // stone
                0, // workers
                0 // food
            };

            List<int> cycleOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            List<int> staticOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                4, // workers
                0, // food
            };

            return new Building(settings, costs, cycleOutputs, staticOutputs)
            {
                Name = "Basic House"
            };
        }

        // construct a log cabin
        public static Building LogCabin()
        {
            List<float> settings = new List<float>()
            {
                200,
            };

            // set costs
            List<int> costs = new List<int>()
            {
                0, // gold
                20, // wood
                0, // iron
                0, // stone
                1, // workers
                0 // food
            };

            List<int> cycleOutputs = new List<int>()
            {
                0, // gold
                10, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            List<int> staticOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            return new Building(settings, costs, cycleOutputs, staticOutputs)
            {
                Name = "Log Cabin"
            };
        }

        public static Building Farm()
        {
            List<float> settings = new List<float>()
            {
                300,
            };

            // set costs
            List<int> costs = new List<int>()
            {
                0, // gold
                25, // wood
                0, // iron
                0, // stone
                1, // workers
                0 // food
            };

            List<int> cycleOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                10, // food
            };

            List<int> staticOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            return new Building(settings, costs, cycleOutputs, staticOutputs)
            {
                Name = "Farm"
            };
        }

        public static Building Quarry()
        {
            List<float> settings = new List<float>()
            {
                400,
            };

            // set costs
            List<int> costs = new List<int>()
            {
                0, // gold
                50, // wood
                0, // iron
                0, // stone
                2, // workers
                0 // food
            };

            List<int> cycleOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                10, // iron
                10, // stone
                0, // workers
                0, // food
            };

            List<int> staticOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                10, // iron
                10, // stone
                0, // workers
                0, // food
            };

            return new Building(settings, costs, cycleOutputs, staticOutputs)
            {
                Name = "Quarry"
            };
        }

        public static Building Windmill()
        {
            List<float> settings = new List<float>()
            {
                500,
            };

            // set costs
            List<int> costs = new List<int>()
            {
                0, // gold
                40, // wood
                0, // iron
                0, // stone
                1, // workers
                0 // food
            };

            List<int> cycleOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                10, // food
            };

            List<int> staticOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            return new Building(settings, costs, cycleOutputs, staticOutputs)
            {
                Name = "Windmill",
                RequiresRoad = false
            };
        }

        public static Building Road()
        {
            List<float> settings = new List<float>()
            {
                1000,
            };

            // set costs
            List<int> costs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                10, // stone
                0, // workers
                0 // food
            };

            List<int> cycleOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            List<int> staticOutputs = new List<int>()
            {
                0, // gold
                0, // wood
                0, // iron
                0, // stone
                0, // workers
                0, // food
            };

            return new Building(settings, costs, cycleOutputs, staticOutputs)
            {
                Name = "Road",
                RequiresRoad = false
            };
        }

        #endregion

        public void Update(GameTime gameTime)
        {
        }
    }
}
