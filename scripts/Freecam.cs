using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamAudioDotnet.scripts
{
    public partial class Freecam : Camera3D
    {
        public float Pitch { get; set; } = 0.0f;
        public float Yaw { get; set; } = 0.0f;
        [Export]
        public float Sensitivity { get; set; } = 0.05f;
        [Export]
        public float JoySensitivity { get; set; } = 1.0f;

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion e && Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                Yaw -= Mathf.DegToRad(e.Relative.X * Sensitivity);
                Pitch -= Mathf.DegToRad(e.Relative.Y * Sensitivity);

                Pitch = Mathf.Clamp(Pitch, Mathf.DegToRad(-90), Mathf.DegToRad(90));
            }
        }

        public override void _Process(double delta)
        {
            Vector3 direction = Vector3.Zero;

            direction.X -= Input.GetActionStrength("move_left");
            direction.X += Input.GetActionStrength("move_right");
            direction.Z -= Input.GetActionStrength("move_forward");
            direction.Z += Input.GetActionStrength("move_backward");

            Transform3D trans = Transform3D.Identity;

            // rotate trans based on pitch and yaw
            trans = trans.Rotated(new Vector3(1, 0, 0), Pitch);
            trans = trans.Rotated(new Vector3(0, 1, 0), Yaw);


            direction = trans.Basis * direction;

            Vector2 joyLook = Input.GetVector("look_left", "look_right", "look_up", "look_down");

            Yaw = Yaw - Mathf.DegToRad(joyLook.X * JoySensitivity);
            Pitch = Pitch - Mathf.DegToRad(joyLook.Y * JoySensitivity);

            Rotation = new Vector3(Mathf.Clamp(Pitch, Mathf.DegToRad(-90), Mathf.DegToRad(90)),
                                            Yaw,
                                            Rotation.Z);

            Position += direction * (float)delta * 10.0f;

            if (Input.IsActionJustPressed("ui_pause"))
            {
                if (Input.MouseMode == Input.MouseModeEnum.Captured)
                {
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                }
                else
                {
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                }
            }
        }
    }
}
