# MediaControlMCP

MCP server for your stochastic parrot/AI partner(!) to talk to your Windows media players.

Uses the Windows [`GlobalSystemMediaTransportControlsSession`](https://learn.microsoft.com/en-us/uwp/api/windows.media.control.globalsystemmediatransportcontrolssession) API — the same one that powers the media controls in the Windows taskbar. Works with any app that integrates with it: Spotify, Apple Music, YouTube (browser), Windows Media Player, etc.

## Requirements

- Windows 10/11
- [.NET 10 runtime](https://dotnet.microsoft.com/download/dotnet/10.0)

## Build & Install

1. Create `%USERPROFILE%\.local\bin` if it doesn't exist and add it to your `PATH`
2. Run the build script from the repo root:

```powershell
.\build.ps1
```

This publishes `MediaControlMCP.exe` directly to `%USERPROFILE%\.local\bin`.

## MCP Configuration

### Claude Desktop (`claude_desktop_config.json`)

```json
{
  "mcpServers": {
    "mediaControl": {
      "command": "MediaControlMCP.exe"
    }
  }
}
```

## Available Tools

| Tool | Description |
|------|-------------|
| `GetSessions` | Returns all active media sessions |
| `GetCurrentSession` | Returns the currently playing (or most recently active) session |
| `TogglePlayPause` | Play/pause the current session |
| `SkipPrevious` | Skip to previous track |
| `SkipNext` | Skip to next track |
| `Rewind` | Rewind the current session |
| `FastForward` | Fast forward the current session |
| `ToggleShuffle` | Toggle shuffle on/off |
| `SetRepeatMode` | Set repeat mode (`None`, `Track`, or `List`) |

### Session data returned

Each session includes:
- **App** — name and system ID of the media app
- **Metadata** — title, artist, album title, album artist
- **Status** — playback status (Playing, Paused, Stopped, etc.)
- **Timeline** — current position, total length, seek range, last updated timestamp
- **Shuffle** — whether shuffle is active
- **RepeatMode** — current repeat mode (None, Track, List)