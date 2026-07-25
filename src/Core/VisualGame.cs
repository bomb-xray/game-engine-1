using Raylib_cs;

namespace Engine.Core;

public sealed class VisualGame
{
    private readonly string _game;
    private Texture2D _splash, _menu;
    private Sound _confirm;
    private Music _music;
    private bool _hasSplash, _hasMenu, _hasSound, _hasMusic;
    private enum State { Splash, Menu, Options }
    private State _state = State.Splash;
    private float _fade, _brightness;
    private int _selected;
    private readonly string[] _items = { "PLAY", "OPTIONS", "EXIT" };
    private readonly string[] _options = { "MASTER VOLUME", "MUSIC VOLUME", "SFX VOLUME", "BACK" };

    public VisualGame(string gameFolder) => _game = Path.GetFullPath(gameFolder);

    public void Run()
    {
        string ui = Path.Combine(_game, "assets", "ui");
        string audio = Path.Combine(_game, "assets", "audio");
        Raylib.SetConfigFlags(ConfigFlags.VSyncHint | ConfigFlags.Undecorated);
        Raylib.InitWindow(1280, 720, "GT-Engine");
        Raylib.SetExitKey(KeyboardKey.Null);
        Raylib.HideCursor();
        Load(ref _splash, Path.Combine(ui, "image.png"), ref _hasSplash);
        Load(ref _menu, Path.Combine(ui, "negri.png"), ref _hasMenu);
        Load(ref _confirm, Path.Combine(audio, "menu_confirm.wav"), ref _hasSound);
        string music = new[] { "untrust.ogg", "untrust.wav", "untrust.mp3" }
            .Select(x => Path.Combine(audio, x)).FirstOrDefault(File.Exists) ?? "";
        if (music.Length > 0) { _music = Raylib.LoadMusicStream(music); _hasMusic = true; Raylib.PlayMusicStream(_music); Raylib.SetMusicVolume(_music, .8f); }
        while (!Raylib.WindowShouldClose()) { Update(); Draw(); if (_hasMusic) Raylib.UpdateMusicStream(_music); }
        if (_hasMusic) Raylib.UnloadMusicStream(_music); if (_hasSound) Raylib.UnloadSound(_confirm);
        if (_hasSplash) Raylib.UnloadTexture(_splash); if (_hasMenu) Raylib.UnloadTexture(_menu); Raylib.CloseWindow();
    }

    private static void Load(ref Texture2D texture, string path, ref bool ok)
    { if (File.Exists(path)) { texture = Raylib.LoadTexture(path); ok = true; } }
    private bool Accept() => Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space) || Raylib.IsMouseButtonPressed(MouseButton.Left);
    private void Update()
    {
        float dt = Raylib.GetFrameTime();
        if (_state == State.Splash) { _fade = Math.Min(1, _fade + dt * .7f); _brightness = Math.Min(1, _brightness + dt * .35f); if (_fade >= 1 && Accept()) { if (_hasSound) Raylib.PlaySound(_confirm); _state = State.Menu; _selected = 0; } return; }
        int count = _state == State.Options ? _options.Length : _items.Length;
        if (Raylib.IsKeyPressed(KeyboardKey.Down) || Raylib.IsKeyPressed(KeyboardKey.S)) _selected = (_selected + 1) % count;
        if (Raylib.IsKeyPressed(KeyboardKey.Up) || Raylib.IsKeyPressed(KeyboardKey.W)) _selected = (_selected + count - 1) % count;
        if (Raylib.IsKeyPressed(KeyboardKey.Escape) && _state == State.Options) { _state = State.Menu; _selected = 1; }
        if (Accept()) { if (_hasSound) Raylib.PlaySound(_confirm); if (_state == State.Menu && _selected == 1) { _state = State.Options; _selected = 0; } else if (_state == State.Menu && _selected == 2) Raylib.CloseWindow(); else if (_state == State.Options && _selected == 3) { _state = State.Menu; _selected = 1; } }
    }
    private void Draw()
    {
        Raylib.BeginDrawing(); Raylib.ClearBackground(Color.Black);
        if (_state == State.Splash) { DrawCover(_splash, _hasSplash, ColorAlpha(Color.White, _brightness)); if (_fade >= 1) Raylib.DrawText("PRESS ANY KEY", 530, 630, 28, Color.White); }
        else { DrawCover(_menu, _hasMenu, Color.White); Raylib.DrawRectangle(0,0,1280,720,ColorAlpha(Color.Black,.35f)); var list = _state == State.Options ? _options : _items; Raylib.DrawText(_state == State.Options ? "OPTIONS" : "", 500, 160, 42, Color.White); for(int i=0;i<list.Length;i++) { var c=i==_selected?Color.Yellow:Color.White; Raylib.DrawText(list[i], 500, 270+i*65, 32, c); } }
        Raylib.EndDrawing();
    }
    private static void DrawCover(Texture2D t, bool ok, Color tint) { if (!ok) return; var src = new Rectangle(0,0,t.Width,t.Height); var dst = new Rectangle(0,0,Raylib.GetScreenWidth(),Raylib.GetScreenHeight()); Raylib.DrawTexturePro(t,src,dst,new Vector2(0,0),0,tint); }
    private static Color ColorAlpha(Color c,float a) => new(c.R,c.G,c.B,(byte)(Math.Clamp(a,0,1)*255));
}
