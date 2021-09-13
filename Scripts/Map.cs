using System;
using Godot;

namespace DungeonMasterVR.Scripts
{
    public class Map : IDisposable
    {
        public Map()
        {
            _mapSize = MapSize.Small;
            _mapCells = new Cell[(int)MapSize.Small];
            _gold = 100;
            _mana = 0;
        }

        public void Dispose()
        {
        }

        public static Map Load(string fileName)
        {
            GD.Print($"Loading Map: {fileName}");

            var map = new Map
            {
                _fileName = fileName,
            };

            return map;
        }

        // Public Properties
        public string FileName
        {
            get => _fileName;
        }

        // Constants
        private const uint MAGIC = 0x3170614d; // "Map1"

        // Private Fields;
        private MapSize _mapSize;
        private Cell[] _mapCells;
        private int _gold;
        private int _mana;

        private string _fileName;
    }

}
