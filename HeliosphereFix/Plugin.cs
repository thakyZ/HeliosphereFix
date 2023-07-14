using System;

using Dalamud.IoC;
using Dalamud.Plugin;

namespace HeliosphereFix;
public class Plugin : IDalamudPlugin {
  public string Name => "Heliosphere Fix";
  internal static GameFont GameFont { get; private set; }
  internal static DalamudPluginInterface PluginInterface { get; private set; }

  public Plugin(DalamudPluginInterface _interface) {
    PluginInterface = _interface;
    GameFont = new GameFont(_interface);
    // Add the BuildFonts function to be called every time fonts are rebuilt.
    // If you have custom font sizes especially  with the AXIS fonts it will
    // need to be re-rendered.
    PluginInterface.UiBuilder.BuildFonts += this.BuildFonts;

    // Run the function, BuildFonts, manually.
    this.BuildFonts();
  }

  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  private bool _isDisposed;

  protected virtual void Dispose(bool disposing) {
    if (disposing && !_isDisposed) {
      // Dispose of the BuildFonts event.
      PluginInterface.UiBuilder.BuildFonts -= this.BuildFonts;
      GameFont.Dispose();
      _isDisposed = true;
    }
  }

  // Builds the fonts every time fonts are redrawn.
  private void BuildFonts() => GameFont.PreRenderFonts(new() {
        (16, false),
        (16, true),
        (PluginUi.TitleSize, false),
        // Markdown Header Fonts calculated with: https://dotnetfiddle.net/pA1XmV
        (26, false), (22, false), (21, false), (20, false), (19, false)
    });
}

internal static class PluginUi {
    internal const int TitleSize = 36;
}