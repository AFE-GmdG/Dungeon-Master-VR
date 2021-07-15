using System;
using Godot;

namespace DungeonMasterVR.Scripts
{

    public static class Common
    {

        public static bool isSolidCell(CellType cellType)
        {
            return cellType == CellType.Granite
            || cellType == CellType.Sandstone
            || cellType == CellType.Clay
            || cellType == CellType.Soil
            || cellType == CellType.Coal
            || cellType == CellType.Copper
            || cellType == CellType.Iron
            || cellType == CellType.Silver
            || cellType == CellType.Gold
            || cellType == CellType.Diamond;
        }

        public static Color convertUInt8ToColor(int input)
        {
            // Clamp between 0 and 255
            int i = Mathf.Min(Mathf.Max(input, 0), 255);
            float r = (i & 0x3) / 4.0f + 0.125f;
            float g = ((i & 0xc) >> 2) / 4.0f + 0.125f;
            float b = ((i & 0x30) >> 4) / 4.0f + 0.125f;
            float a = ((i & 0xc0) >> 6) / 4.0f + 0.125f;
            return new Color(r, g, b, a);
        }

    }

}
