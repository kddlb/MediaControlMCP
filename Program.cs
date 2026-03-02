using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Windows.Media;
using Windows.Media.Control;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton<MediaSessionHandler>();

builder.Services.AddHostedService(provider => provider.GetRequiredService<MediaSessionHandler>());

await builder.Build().RunAsync();

[McpServerToolType]
public static class EchoTool
{
    [McpServerTool, Description("Echoes the message back to the client.")]
    public static string Echo(string message) => $"hello {message}";
}

[McpServerToolType]
public  class MediaSessionTools
{
    private readonly MediaSessionHandler _mediaSessionHandler;
    public MediaSessionTools(MediaSessionHandler mediaSessionHandler)
    {
        _mediaSessionHandler = mediaSessionHandler;
    }

    private static SessionInfo ToSessionInfo(GlobalSystemMediaTransportControlsSession session)
    {
        var metadataTask = session.TryGetMediaPropertiesAsync().AsTask();
        metadataTask.Wait();
        var metadata = metadataTask.Result;
        var simplifiedMetadata = Helpers.GetMetadata(metadata, session.SourceAppUserModelId);

        var playbackInfo = session.GetPlaybackInfo();
        return new SessionInfo
        {
            AppId = session.SourceAppUserModelId,
            Metadata = simplifiedMetadata,
            Status = playbackInfo.PlaybackStatus,
            AppName = Helpers.GetAppId(session.SourceAppUserModelId),
            Timeline = ToTimelineInfo(session),
            IsShuffleActive = playbackInfo.IsShuffleActive,
            RepeatMode = playbackInfo.AutoRepeatMode,
        };
    }

    private static SessionTimelineInfo ToTimelineInfo(GlobalSystemMediaTransportControlsSession session)
    {
        var timeline = session.GetTimelineProperties();
        var controls = session.GetPlaybackInfo().Controls;
        var lengthSeconds = Math.Max(0, (timeline.EndTime - timeline.StartTime).TotalSeconds);

        return new SessionTimelineInfo
        {
            IsSeekEnabled = controls.IsPlaybackPositionEnabled,
            PositionSeconds = timeline.Position.TotalSeconds,
            LengthSeconds = lengthSeconds,
            MinSeekSeconds = timeline.MinSeekTime.TotalSeconds,
            MaxSeekSeconds = timeline.MaxSeekTime.TotalSeconds,
            StartSeconds = timeline.StartTime.TotalSeconds,
            EndSeconds = timeline.EndTime.TotalSeconds,
            LastUpdatedTimeUtc = timeline.LastUpdatedTime
        };
    }

    [McpServerTool, Description("Gets all the current sessions.")]
    public async Task<List<SessionInfo>> GetSessionsAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var sessions = manager.GetSessions();
        var sessionInfos = sessions.Select(session =>
        {
            return ToSessionInfo(session);
        }).ToList();

        return sessionInfos;
    }

    [McpServerTool, Description("Gets the current session. The current session is defined as the session that is currently playing, or if no session is playing, the most recently active session.")]
    public async Task<SessionInfo> GetCurrentSessionAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        return ToSessionInfo(session);
    }

    [McpServerTool, Description("Sends a play or pause command to the current session. The current session is defined as the session that is currently playing, or if no session is playing, the most recently active session.")]
    public async Task TogglePlayPauseAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        var success = await session.TryTogglePlayPauseAsync();
        if (!success)        {
            throw new InvalidOperationException("Failed to toggle play/pause on the current session.");
        }
    }

    //tools to skip previous, next, rewind and fast forward.
    [McpServerTool, Description("Sends a skip to previous command to the current session.")]
    public async Task SkipPreviousAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        var success = await session.TrySkipPreviousAsync();
        if (!success)
        {
            throw new InvalidOperationException("Failed to skip to previous on the current session.");
        }
    }

     [McpServerTool, Description("Sends a skip to next command to the current session.")] 
    public async Task SkipNextAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        var success = await session.TrySkipNextAsync();
        if (!success)
        {
            throw new InvalidOperationException("Failed to skip to next on the current session.");
        }
    }

     [McpServerTool, Description("Sends a rewind command to the current session.")]
    public async Task RewindAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        var success = await session.TryRewindAsync();
        if (!success)
        {
            throw new InvalidOperationException("Failed to rewind on the current session.");
        }
    }

     [McpServerTool, Description("Sends a fast forward command to the current session.")]
    public async Task FastForwardAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        var success = await session.TryFastForwardAsync();
        if (!success)
        {
            throw new InvalidOperationException("Failed to fast forward on the current session.");
        }
    }

    [McpServerTool, Description("Toggles shuffle on or off for the current session.")]
    public async Task ToggleShuffleAsync()
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        var current = session.GetPlaybackInfo().IsShuffleActive ?? false;
        var success = await session.TryChangeShuffleActiveAsync(!current);
        if (!success)
        {
            throw new InvalidOperationException("Failed to toggle shuffle on the current session.");
        }
    }

    [McpServerTool, Description("Sets the repeat mode for the current session. Valid values: None, Track, List.")]
    public async Task SetRepeatModeAsync(
        [Description("The repeat mode to set. Valid values: None (no repeat), Track (repeat current track), List (repeat playlist).")] string mode)
    {
        var manager = _mediaSessionHandler.TryGetManager() ?? throw new InvalidOperationException("MediaSessionManager is not available.");

        var session = manager.GetCurrentSession();
        if (session == null)
        {
            throw new InvalidOperationException("No active media session found.");
        }

        if (!Enum.TryParse<MediaPlaybackAutoRepeatMode>(mode, ignoreCase: true, out var repeatMode))
        {
            throw new ArgumentException($"Invalid repeat mode '{mode}'. Valid values: None, Track, List.");
        }

        var success = await session.TryChangeAutoRepeatModeAsync(repeatMode);
        if (!success)
        {
            throw new InvalidOperationException($"Failed to set repeat mode to '{mode}' on the current session.");
        }
    }


}