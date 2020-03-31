using System;

namespace Assets.Scripts.Player
{
    /// <summary>
    ///     Class to group and hold control flags and variables
    /// </summary>
    public sealed class ControlFlags
    {
        #region Properties
        public float HorizontalMove { get; set; }           // Variable to store horizontal move value, range (-1.0f, 1.0f)
        public bool Jump { get; set; }                      // Flag to store jump value
        public bool Slide { get; set; }                     // Flag to store slide value
        public bool Glide { get; set; }                     // Flag to store glide value
        public bool Attack { get; set; }                    // Flag to store attack value
        public bool Throw { get; set; }                     // Flag to store throw value
        #endregion

        #region Constructor
        public ControlFlags()
        {
            HorizontalMove = 0.0f;
            Jump = false;
            Slide = false;
            Glide = false;
            Attack = false;
            Throw = false;
        }
        #endregion

        #region Public methods
        public void ResetFlags()
        {
            Attack = false;
            Glide = false;
            HorizontalMove = 0.0f;
            Jump = false;
            Slide = false;
            Throw = false;
        }
        #endregion
    }
}
