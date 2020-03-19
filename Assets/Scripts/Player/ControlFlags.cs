using System;

namespace Assets.Scripts.Player
{
    /// <summary>
    ///     Class to group and hold control flags and variables
    /// </summary>
    public sealed class ControlFlags
    {
        public float HorizontalMove { get; set; }           // Variable to store horizontal move value, range (-1.0f, 1.0f)
        public bool Jump { get; set; }                      // Flag to store jump value
        public bool Slide { get; set; }                     // Flag to store slide value
        public bool Glide { get; set; }                     // Flag to store glide value
        public bool Attack { get; set; }                    // Flag to store attack value
        public bool Throw { get; set; }                     // Flag to store throw value

        public ControlFlags()
        {
            HorizontalMove = 0.0f;
            Jump = false;
            Slide = false;
            Glide = false;
            Attack = false;
            Throw = false;
        }
    }
}
