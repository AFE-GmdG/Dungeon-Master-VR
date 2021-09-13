using DungeonMasterVR.Scenes.Controller;
using Godot;

namespace DungeonMasterVR.Scenes
{

    public class DungeonTest : Spatial
    {

        public override void _Ready()
        {
            base._Ready();
            _levelController = GetNode<LevelController>("LevelController");
        }

        private LevelController _levelController;

    }

}
