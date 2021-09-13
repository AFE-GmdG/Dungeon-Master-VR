using Godot;

namespace DungeonMasterVR.Scripts
{

    public class Startup : Node
    {

        public override void _EnterTree()
        {
            base._EnterTree();
            OS.WindowBorderless = true;
            OS.WindowFullscreen = true;
            OS.VsyncEnabled = true;
            OS.SetWindowAlwaysOnTop(true);
            Engine.TargetFps = 60;
            Engine.IterationsPerSecond = 60;
            OS.SetWindowAlwaysOnTop(false);
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                if (inputEvent.IsActionPressed("capture_release") && !inputEvent.IsEcho())
                {
                    Input.SetMouseMode(Input.MouseMode.Visible);
                    GetTree().SetInputAsHandled();
                }
            }
            else
            {
                if (inputEvent.IsActionPressed("capture_click"))
                {
                    Input.SetMouseMode(Input.MouseMode.Captured);
                    GetTree().SetInputAsHandled();
                }
                else if (inputEvent.IsActionPressed("capture_release") && !inputEvent.IsEcho())
                {
                    GetTree().Quit();
                    GetTree().SetInputAsHandled();
                }
            }

        }

    }

}
