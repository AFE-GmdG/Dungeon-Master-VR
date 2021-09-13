namespace DungeonMasterVR.Scripts
{

    public struct Cell
    {

        public CellType type;
        public FogOfWar fogOfWar;
        public int health;
        public float maxMana;
        public float managain;
        public float humidity;
        public float temperature;

        // A MaxHealth of 1000 means indestructible.
        // A Value of 0 destroys the current type or set it to the "digged" version.
        // Degradable raw materials have a maximum value of 500 Units.
        // Degradable raw materials with a current health value of 0 will become SandstoneDigged.
        // The map can have been started with any value equal to or below this value.
        public int MaxHealth
        {
            get
            {
                switch (type)
                {
                    case CellType.Granite:
                        return 1000;
                    case CellType.Sandstone:
                        return 100;
                    case CellType.Clay:
                        return 80;
                    case CellType.Soil:
                        return 60;
                    case CellType.Coal:
                        return 500;
                    case CellType.Copper:
                        return 500;
                    case CellType.Iron:
                        return 500;
                    case CellType.Silver:
                        return 500;
                    case CellType.Gold:
                        return 500;
                    case CellType.Diamond:
                        return 1000;
                }
                return 0;
            }
        }

    }

}
