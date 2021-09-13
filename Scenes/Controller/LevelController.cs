using DungeonMasterVR.Meshes;
using DungeonMasterVR.Scripts;
using Godot;

namespace DungeonMasterVR.Scenes.Controller
{

    public class LevelController : Spatial
    {

        public override void _Ready()
        {
            GD.Print("LevelController->Ready");

            environment = GetNode<Spatial>("Environment");
            staticItems = GetNode<Spatial>("Static Items");
            dynamicItems = GetNode<Spatial>("Dynamic Items");

            floorScene = GD.Load<PackedScene>("res://Meshes/Floor.tscn");

            sandstoneMeshes = new ArrayMesh[16];
            for (var y = 0; y < 4; ++y)
            {
                for (var x = 0; x < 4; ++x)
                {
                    sandstoneMeshes[y * 4 + x] = GD.Load<ArrayMesh>($"res://Meshes/Floor/Sandstone/Floor_{x}{y}.tres");
                }
            }

            // ToDo: Move code to Load Map function
            floorCells = new Floor[225];
            for (var z = 0; z < 15; ++z)
            {
                for (var x = 0; x < 15; ++x)
                {
                    var floorCell = floorScene.Instance<Floor>();
                    var cellX = x - 7;
                    var cellZ = z - 7;
                    var posX = cellX * 3.0f;
                    var posZ = cellZ * 3.0f;
                    var meshX = (cellX % 4 + 4) % 4;
                    var meshZ = 3 - (((cellZ - 1) % 4 + 4) % 4);
                    // GD.Print($"CellX: {x} - PosX: {posX} - MeshX: {meshX}");
                    // GD.Print($"CellZ: {z} - PosZ: {posZ} - MeshZ: {meshZ}");

                    floorCell.Transform = new Transform(Basis.Identity, new Vector3(posX, 0.0f, posZ));
                    var mi = floorCell.GetNode<MeshInstance>("Floor");
                    mi.Mesh = sandstoneMeshes[meshZ * 4 + meshX];

                    environment.AddChild(floorCell);

                    floorCells[z * 15 + x] = floorCell;
                }
            }

            // Finish delayed setters:
            if (loadingMapFileName != null)
                MapFileName = loadingMapFileName;

        }

        [Export]
        public string MapFileName
        {
            get
            {
                if (map == null)
                    return null;
                return map.FileName;
            }
            set
            {
                if (!IsInsideTree()) {
                    loadingMapFileName = value;
                    return;
                }

                loadingMapFileName = null;
                if (map != null)
                {
                    if (map.FileName == value)
                        return;

                    map.Dispose();
                }

                map = Map.Load(value);
            }
        }

        // Private Fields
        private Spatial environment;
        private Spatial staticItems;
        private Spatial dynamicItems;
        private PackedScene floorScene;
        private ArrayMesh[] sandstoneMeshes;
        private Floor[] floorCells;

        private Map map;
        private string loadingMapFileName;

    }

}
