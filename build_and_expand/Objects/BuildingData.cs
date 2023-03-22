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
            {600, Building.TownHall()},
            {999, Building.Road()}
        };

        public static Dictionary<int, List<int>> Dict_BuildingResourceLink => new Dictionary<int, List<int>>()
        {
            {100, new List<int>() {0}},
            {200, new List<int>() {}}, // log cabin -> trees
            {300, new List<int>() {0}}, // farm -> farmland
            {400, new List<int>() {4, 5, 6}}, // quarry  -> ore(s)
            {500, new List < int >() {0}},
            {600, new List < int >() {0}},
            {999, new List < int >() {0}}
        };

        public static Dictionary<int, string> Dic_ResourceNameKeys => new Dictionary<int, string>()
        {            
            {1, "Food"},
            {2, "Wood"},
            {3, "Workers"},
        };
    }
}
