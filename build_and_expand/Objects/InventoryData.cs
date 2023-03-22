using build_and_expand.States;
using Newtonsoft.Json;
using System;

namespace build_and_expand.Objects
{
    public class InventoryData
    {
        public static int ResourceMax = 500;
        public int Gold
        {
            get => _gold;
            set { _gold = value > ResourceMax ? ResourceMax : value; }
        }
        public int Wood
        {
            get => _wood;
            set { _wood = value > ResourceMax ? ResourceMax : value; }
        }
        public int Iron
        {
            get => _iron;
            set { _iron = value > ResourceMax ? ResourceMax : value; }
        }
        public int Stone
        {
            get => _stone;
            set { _stone = value > ResourceMax ? ResourceMax : value; }
        }
        public int Workers
        {
            get => _workers;
            set { _workers = value > ResourceMax ? ResourceMax : value; }
        }
        public int Food
        {
            get => _food;
            set { _food = value > ResourceMax ? ResourceMax : value; }
        }

        [JsonIgnore] private int _gold;
        [JsonIgnore] private int _wood;
        [JsonIgnore] private int _iron;
        [JsonIgnore] private int _stone;
        [JsonIgnore] private int _workers;
        [JsonIgnore] private int _food;

        public InventoryData()
        {
            ResetInventoryToBase();
        }

        public void ResetInventoryToBase()
        {
            Gold = 10;
            Wood = 50;
            Iron = 0;
            Stone = 0;
            Workers = 4;
            Food = 100;
        }

        public bool RemoveResource(string resource, int amount_requested)
        {
            if(amount_requested == 0)
            {
                return true;
            }

            try
            {
                if(string.IsNullOrEmpty(resource))
                {
                    throw new NotSupportedException("Resource name cannot be null or empty.");
                }

                if(amount_requested <= 0)
                {
                    throw new NotSupportedException("Cannot request a resource amount equal or less than zero.");
                }

                // switch based on resource name
                // try and subtract amount requested from resource
                // return true on success, false otherwise
                switch(resource.ToLower())
                {
                    case "gold":
                        Gold -= amount_requested;
                        return true;
                    case "wood":
                        Wood -= amount_requested;
                        return true;
                    case "stone":
                        Stone -= amount_requested;
                        return true;
                    case "iron":
                        Iron -= amount_requested;
                        return true;
                    case "workers":
                        Workers -= amount_requested;
                        return true;
                    case "food":
                        Food -= amount_requested;
                        return true;
                    default:
                        return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error getting resource: " + e.Message);
                return false;
            }
        }

        public bool RequestResource(string resource, int amount_requested)
        {
            if(amount_requested.Equals(0))
            {
                return true;
            }

            try
            {
                if(string.IsNullOrEmpty(resource))
                {
                    throw new NotSupportedException("Resource name cannot be null or empty.");
                }

                if(amount_requested <= 0)
                {
                    throw new NotSupportedException("Cannot request a resource amount equal or less than zero.");
                }

                // switch based on resource name
                // check amount requested from resource
                // return true if available, false otherwise
                switch(resource.ToLower())
                {
                    case "gold":
                        if(amount_requested <= Gold)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "wood":
                        if(amount_requested <= Wood)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "iron":
                        if(amount_requested <= Iron)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "stone":
                        if(amount_requested <= Stone)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "workers":
                        if(amount_requested <= Workers)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case "food":
                        if(amount_requested <= Food)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error getting resource: " + e.Message);
                return false;
            }
        }

        public void AddObjectCycleOutputs(TileObject tileObject)
        {
            Gold += tileObject.GoldCycleOutput;
            Wood += tileObject.WoodCycleOutput;
            Iron += tileObject.IronCycleOutput;
            Workers += tileObject.WorkersCycleOutput;
            Food += tileObject.FoodCycleOutput;
            Stone += tileObject.StoneCycleOutput;
        }

        public bool CanBuildObject(TileObject tileObject)
        {
            bool i1 = RequestResource("food", tileObject.FoodCost);
            bool i2 = RequestResource("wood", tileObject.WoodCost);
            bool i3 = RequestResource("workers", tileObject.WorkersCost);
            bool i4 = RequestResource("gold", tileObject.GoldCost);
            bool i5 = RequestResource("iron", tileObject.IronCost);
            bool i6 = RequestResource("stone", tileObject.StoneCost);

            return (i1 && i2 && i3 && i4 && i5 && i6);
        }

        public void RemoveObjectCosts(TileObject tileObject)
        {
            Gold -= tileObject.GoldCost;
            Wood -= tileObject.WoodCost;
            Iron -= tileObject.IronCost;
            Workers -= tileObject.WorkersCost;
            Food -= tileObject.FoodCost;
            Stone -= tileObject.StoneCost;
        }

        public void AddObjectStaticOutputs(TileObject tileObject)
        {
            Gold += tileObject.GoldStaticOutput;
            Wood += tileObject.WoodStaticOutput;
            Iron += tileObject.IronStaticOutput;
            Workers += tileObject.WorkersStaticOutput;
            Food += tileObject.FoodStaticOutput;
            Stone += tileObject.StoneStaticOutput;
        }

        public void AddObjectDestroyedCosts(TileObject tileObject)
        {
            Gold += (tileObject.GoldCost / 2);
            Wood += (tileObject.WoodCost / 2);
            Iron += (tileObject.IronCost / 2);
            Stone += (tileObject.StoneCost / 2);
            Food += (tileObject.FoodCost / 2);
            Workers += (tileObject.WorkersCost);
        }
    }
}
