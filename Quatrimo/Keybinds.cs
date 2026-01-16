using FlatRedBall;
using FlatRedBall.Input;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quatrimo
{
    public static class Keybinds
    {
        public static DirectBind Left = new([Keys.A, Keys.Left, Keys.NumPad4]);
        public static DirectBind Right = new([Keys.D, Keys.Right, Keys.NumPad6]);
        public static DirectBind Down = new([Keys.S, Keys.Down, Keys.NumPad2]);
        public static DirectBind Up = new([Keys.W, Keys.Up, Keys.NumPad8]);
        public static DirectBind RotateLeft = new([Keys.Q, Keys.NumPad7]);
        public static DirectBind RotateRight = new([Keys.E, Keys.NumPad9]);

        public static DirectBind Slam = new([Keys.Space]);


        public static void UpdateBinds()
        {
            Left.UpdateKey();
            Right.UpdateKey();
            Down.UpdateKey();
            Up.UpdateKey();
            RotateLeft.UpdateKey();
            RotateRight.UpdateKey();
            Slam.UpdateKey();
        }

        public abstract class Keybind
        {
            /// <summary>
            /// Key was up last frame, down this frame
            /// </summary>
            public bool Pushed;
            /// <summary>
            /// Key is being held down
            /// </summary>
            public bool Held;
            /// <summary>
            /// Key was down last frame, up this frame
            /// </summary>
            public bool Released;
            /// <summary>
            /// Total time the key has been held
            /// </summary>
            public float TimeHeld;

            public virtual void UpdateKey()
            {
                UpdatePushed();
                UpdateHeld();
                UpdateReleased();
            }

            protected abstract void UpdatePushed();

            protected abstract void UpdateHeld();

            protected abstract void UpdateReleased();



        }

        public class DirectBind : Keybind
        {
            List<Keys> keys;

            public DirectBind(List<Keys> keys)
            {
                this.keys = keys;
            }

            protected override void UpdatePushed()
            {
                foreach (Keys key in keys)
                {
                    if (InputManager.Keyboard.KeyPushed(key))
                    {
                        Pushed = true;
                        return;
                    }
                }
                Pushed = false;
            }

            protected override void UpdateHeld()
            {
                foreach (Keys key in keys)
                {
                    if (InputManager.Keyboard.KeyDown(key))
                    {
                        Held = true;
                        TimeHeld += TimeManager.SecondDifference;
                        return;
                    }
                }
                TimeHeld = 0;
                Held = false;
            }

            protected override void UpdateReleased()
            {
                foreach (Keys key in keys)
                {
                    if (InputManager.Keyboard.KeyReleased(key))
                    {
                        Released = true;
                        return;
                    }
                }
                Released = false;
            }
        }

        //TODO: update later
        //buttonbind is for controller buttons, directionalbind is for 1d inputs
        //combinebind combines any kind of bind. it's like an or statement for a bunch of binds
        public class ButtonBind
        {

        }

        public class DirectionalBind
        {

        }

        public class CombinedBind
        {

        }
    }
}
