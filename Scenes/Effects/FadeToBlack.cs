using Godot;

namespace DungeonMasterVR.Scenes.Effects
{

    public class FadeToBlack : ColorRect
    {

        #region Backing Fields
        private float _alpha = 1.0f;
        private bool _isFaded = false;
        #endregion

        #region Exported Properties
        [Export]
        public float Duration { get; set; } = 2.0f;

        [Export]
        public bool IsFaded
        {
            get => _isFaded;
            set
            {
                _isFaded = value;
                SetProcess(true);
            }
        }
        #endregion

        #region Signals
        [Signal]
        public delegate void FinishedFading();
        #endregion

        #region Overwritten Base Methods
        public override void _Process(float delta)
        {
            base._Process(delta);
            if (IsFaded)
            {
                if (_alpha < 1.0)
                {
                    _alpha = Mathf.Clamp(_alpha + (delta / Duration), 0.0f, 1.0f);
                }
                else
                {
                    SetProcess(false);
                    EmitSignal(nameof(FinishedFading));
                }
            }
            else
            {
                if (_alpha > 0.0)
                {
                    _alpha = Mathf.Clamp(_alpha - (delta / Duration), 0.0f, 1.0f);
                }
                else
                {
                    SetProcess(false);
                    EmitSignal(nameof(FinishedFading));
                }
            }

            Color = new Color(0.0f, 0.0f, 0.0f, _alpha);
            Visible = _alpha > 0.0f;
        }
        #endregion

    }

}
