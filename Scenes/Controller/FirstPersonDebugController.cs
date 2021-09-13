using Godot;

namespace DungeonMasterVR.Scenes.Controller
{

    public class FirstPersonDebugController : KinematicBody
    {

        public override void _Ready()
        {
            neck = GetNode<Position3D>("Neck");
            camera = neck.GetNode<Camera>("Camera");
        }

        public override void _UnhandledInput(InputEvent inputEvent)
        {
            var mouseMotionInputEvent = inputEvent as InputEventMouseMotion;
            if (mouseMotionInputEvent != null && Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                RotateY(-mouseMotionInputEvent.Relative.x * mouseSensitivity);
                neck.RotateX(-mouseMotionInputEvent.Relative.y * mouseSensitivity);
                neck.Rotation = new Vector3(Mathf.Clamp(neck.Rotation.x, -1.2f, 1.2f), 0.0f, 0.0f);
            }
            base._UnhandledInput(inputEvent);
        }

        public override void _PhysicsProcess(float delta)
        {
            var desiredVelocity = GetInput() * maxSpeed;
            velocity.x = desiredVelocity.x;
            velocity.y += gravity * delta;
            velocity.z = desiredVelocity.z;
            velocity = MoveAndSlide(velocity, Vector3.Up, true);

            base._PhysicsProcess(delta);
        }

        private Vector3 GetInput()
        {
            var inputDirection = new Vector3();

            if (Input.IsActionPressed("fp_move_forward"))
                inputDirection -= GlobalTransform.basis.z;
            if (Input.IsActionPressed("fp_move_back"))
                inputDirection += GlobalTransform.basis.z;
            if (Input.IsActionPressed("fp_strafe_left"))
                inputDirection -= GlobalTransform.basis.x;
            if (Input.IsActionPressed("fp_strafe_right"))
                inputDirection += GlobalTransform.basis.x;

            if (inputDirection.LengthSquared() > 0.0f)
                return inputDirection.Normalized();

            return inputDirection;
        }

        public float gravity = -30.0f;
        public float maxSpeed = 8.0f;
        public float mouseSensitivity = 0.002f; // Radians per pixel

        private Vector3 velocity;
        private Position3D neck;
        private Camera camera;
    }

}
