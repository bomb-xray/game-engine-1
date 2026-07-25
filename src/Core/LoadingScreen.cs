using System;
using Engine.Audio;
using Engine.Graphics;

namespace Engine.Core
{
    /// <summary>
    /// Keyboard-only boot/menu flow. Rendering and platform input are intentionally
    /// kept behind the runner so the engine has no editor or platform-specific menu code.
    /// </summary>
    public enum LoadingState { Loading, PressToContinue, MainMenu, Options }

    public sealed class LoadingScreen
    {
        private readonly AudioSystem _audio;
        private float _progress;
        private bool _bright;
        private int _selected;

        public LoadingState State { get; private set; } = LoadingState.Loading;
        public int SelectedOption => _selected;
        public bool CursorVisible => false;
        public string[] MainMenuItems { get; } = { "PLAY", "OPTIONS", "EXIT" };
        public string[] OptionsItems { get; } = { "MASTER VOLUME", "MUSIC VOLUME", "SFX VOLUME", "BACK" };

        public LoadingScreen(AudioSystem audio) => _audio = audio;

        public void SetProgress(float value)
        {
            _progress = Math.Clamp(value, 0f, 1f);
            if (_progress >= 1f && State == LoadingState.Loading) State = LoadingState.PressToContinue;
        }

        public void Update(float deltaTime)
        {
            // The first image becomes brighter only after loading has completed.
            _bright = State != LoadingState.Loading;
        }

        public void Accept()
        {
            if (State == LoadingState.PressToContinue)
            {
                _audio.PlaySound("menu_confirm");
                State = LoadingState.MainMenu;
                _selected = 0;
            }
            else if (State == LoadingState.MainMenu)
            {
                if (_selected == 0) return; // PLAY intentionally disabled for now.
                if (_selected == 1) { State = LoadingState.Options; _selected = 0; }
                if (_selected == 2) Environment.Exit(0);
            }
            else if (State == LoadingState.Options && _selected == OptionsItems.Length - 1)
            {
                State = LoadingState.MainMenu; _selected = 1;
            }
        }

        public void MoveSelection(int direction, bool inOptions = false)
        {
            int count = inOptions ? OptionsItems.Length : MainMenuItems.Length;
            _selected = (_selected + direction + count) % count;
        }

        public bool IsBright => _bright;
        public float Progress => _progress;
    }
}
