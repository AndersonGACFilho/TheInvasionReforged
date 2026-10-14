using System;

namespace Code.Scenario.Starfield
{
    /// <summary>
    /// Game time that drives the nebula flow.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Separate from Time.time so the background is fully described by
    /// the camera position and this value: pausing freezes the clouds,
    /// and saving and restoring <see cref="Seconds"/> brings them back
    /// exactly as they were.
    /// </para>
    /// <para>
    /// Accumulated in double precision; the shader only ever receives a
    /// phase in [0, 1), so long sessions never lose precision.
    /// </para>
    /// </remarks>
    public class NebulaClock
    {
        public double Seconds { get; set; }

        public void Advance(float deltaTime)
        {
            Seconds += deltaTime;
        }

        /// <summary>
        /// Position in a flow cycle, in [0, 1).
        /// </summary>
        /// <param name="period">Cycle length in seconds.</param>
        /// <param name="frameRate">
        /// Steps per second, so the flow advances in frames like the
        /// sprites. 0 advances continuously.
        /// </param>
        public float Phase(float period, float frameRate)
        {
            double time = frameRate > 0f
                ? Math.Floor(Seconds * frameRate) / frameRate
                : Seconds;

            double cycle = Math.Max(1.0, period);

            return (float)(time % cycle / cycle);
        }
    }
}
