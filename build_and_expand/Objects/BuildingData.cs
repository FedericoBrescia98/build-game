using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace build_and_expand.Objects
{
    static class BuildingData
    {
        public static Dictionary<int, Building> Dict_BuildingFromObjectID => new Dictionary<int, Building>()
        {
            {100, Building.BasicHouse()},
            {200, Building.LogCabin()},
            {300, Building.Farm()},
            {400, Building.Quarry()},
            {500, Building.Windmill()},
            {1000, Building.Road()}
        };

        // for future range and location for buildings
        public static Dictionary<int, List<int>> Dict_BuildingResourceLink => new Dictionary<int, List<int>>()
        {
            {100, new List<int>() {0}},
            {200, new List<int>() {}}, // log cabin -> trees
            {300, new List<int>() {0}}, // farm -> farmland
            {400, new List<int>() {4, 5, 6}}, // quarry  -> ore(s)
            {500, new List < int >() {0}},
            {600, new List < int >() {0}},
            {1000, new List < int >() {0}}
        };
    }
}
