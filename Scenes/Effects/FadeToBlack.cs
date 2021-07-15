using Godot;

namespace DungeonMasterVR.Scenes.Effects
{

    public class FadeToBlack : ColorRect
    {

        private float _alpha = 1.0f;
        private bool _isFaded = false;

        [Export]
        public float duration { get; set; } = 2.0f;

        [Export]
        public bool isFaded
        {
            get => _isFaded;
            set
            {
                _isFaded = value;
                SetProcess(true);
            }
        }

        [Signal]
        public delegate void FinishedFading();

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (isFaded)
            {
                if (_alpha < 1.0)
                {
                    _alpha = Mathf.Clamp(_alpha + (delta / duration), 0.0f, 1.0f);
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
                    _alpha = Mathf.Clamp(_alpha - (delta / duration), 0.0f, 1.0f);
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
    }

}
