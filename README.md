# Stas.ImGui.Overplay

A lightweight, transparent and clickable overlay library for Windows.

This is a fork of [Zaafar's **ClickableTransparentOverlay**](https://github.com/Zaafar/ClickableTransparentOverlay)
(https://github.com/Zaafar). All unnecessary, outdated and paid/licensed dependencies have been
removed, and the rendering stack has been modernized.

## Differences from upstream

- **`ImageCore.cs` replaces `SixLabors.ImageSharp`** — a small self-contained image helper
  (decode/encode PNG via `StbImageSharp` / `StbImageWriteSharp`) with no paid/licensing
  baggage.
- **`Hexa.NET.ImGui` replaces `ImGui.NET`** — gives **full compatibility with .NET AOT**
  (NativeAOT), so the overlay can be published as a trimmed, self-contained native binary.
- **`OverlayExt.cs`** adds convenience extensions on top of the base functionality, most
  importantly **`SetOverlayTopMost`** (and `SetOverlayClickable`), which toggle the window's
  TopMost / click-through state at runtime.

## Why `SetOverlayTopMost` matters

Being able to toggle TopMost at runtime is a critical debugging feature: if rendering ever
crashes, an always-on-top overlay window can steal focus and block navigation inside Visual
Studio. Calling `SetOverlayTopMost(false)` drops the window behind your tools so you can keep
working.

## Requirements

- .NET 10
- Windows, Linux, AOT

## Credits

Based on [ClickableTransparentOverlay](https://github.com/Zaafar/ClickableTransparentOverlay) by
**Zaafar**.
