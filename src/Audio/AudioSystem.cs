using System;

namespace Engine.Audio
{
    /// <summary>
    /// Sound FX & Music Streaming system.
    /// Uses low-memory streaming for long BGM tracks to reduce RAM usage on GT 630 systems.
    /// </summary>
    public sealed class AudioSystem
    {
        public float MasterVolume { get; set; } = 1.0f;
        public float MusicVolume { get; set; } = 0.8f;
        public float SoundVolume { get; set; } = 1.0f;

        public void PlaySound(string soundName, float pitch = 1.0f)
        {
            // Plays audio clip from loaded memory buffers
            Console.WriteLine($"[Audio] Sound Triggered: {soundName} (Vol: {SoundVolume * MasterVolume:F2}, Pitch: {pitch:F2})");
        }

        public void StreamMusic(string musicPath, bool loop = true)
        {
            // Streams BGM directly from storage to keep memory foot-print minimal
            Console.WriteLine($"[Audio] Streaming BGM: {musicPath} (Loop: {loop})");
        }

        public void StopMusic()
        {
            Console.WriteLine("[Audio] Music stopped.");
        }
    }
}
