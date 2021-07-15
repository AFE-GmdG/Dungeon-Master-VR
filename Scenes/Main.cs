using DungeonMasterVR.Scenes.Effects;
using Godot;

namespace DungeonMasterVR.Scenes
{

    public class Main : Spatial
    {

        #region Other Fields
        private float _initialBlackTime = 0.5f;
        private FadeToBlack _fadeToBlack;
        #endregion

        #region Overwritten Base Methods
        public override void _Ready()
        {
            base._Ready();

            _fadeToBlack = GetNode<FadeToBlack>("FadeToBlack");
            _fadeToBlack.Duration = 2.0f;
            _fadeToBlack.IsFaded = true;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            if (_initialBlackTime > 0.0f)
            {
                _initialBlackTime -= delta;
                if (_initialBlackTime <= 0.0f)
                {
                    _initialBlackTime = 0.0f;
                    _fadeToBlack.IsFaded = false;
                }
                return;
            }
        }
        #endregion

    }

}
