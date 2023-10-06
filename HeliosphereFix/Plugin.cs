using System;
using System.Diagnostics.CodeAnalysis;

using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace HeliosphereFix;
public class Plugin : IDalamudPlugin {
  public static string StaticName => "Heliosphere Fix";

  public string Name => StaticName;

  internal static GameFont GameFont { get; private set; } = null!;

  [PluginService]
  [AllowNull, NotNull]
  internal static DalamudPluginInterface Interface { get; private set; }
  [PluginService]
  [AllowNull, NotNull]
  internal static IPluginLog PluginLog { get; private set; }

  public Plugin() {
    GameFont = new GameFont();
    // Add the BuildFonts function to be called every time fonts are rebuilt.
    // If you have custom font sizes especially  with the AXIS fonts it will
    // need to be re-rendered.
    Interface.UiBuilder.BuildFonts += this.BuildFonts;

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
      Interface.UiBuilder.BuildFonts -= this.BuildFonts;
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
