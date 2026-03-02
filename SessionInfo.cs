using System.Text.Json.Serialization;
using Windows.Media;
using Windows.Media.Control;

/// <summary>
/// Lightweight DTO that describes a media session's current information.
/// This is used to surface playback metadata and session identity.
/// </summary>
public class SessionInfo
{
    /// <summary>
    /// The application identifier for the media session (usually package or app id).
    /// </summary>
    public required string AppId { get; set; }

    /// <summary>
    /// The friendly name of the application owning the session.
    /// </summary>
    public required string AppName { get; set; }

    /// <summary>
    /// The simplified metadata for the media session, including title, artist, album.
    /// </summary>
    public required SimplifiedMetadata Metadata { get; set; }

    /// <summary>
    /// The playback status for the session (Playing, Paused, etc.).
    /// Serialized/deserialized as a string by <see cref="JsonStringEnumConverter"/>.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required GlobalSystemMediaTransportControlsSessionPlaybackStatus Status { get; set; }

    /// <summary>
    /// Timeline and seekability details for the current media item in this session.
    /// </summary>
    public required SessionTimelineInfo Timeline { get; set; }

    /// <summary>
    /// Whether shuffle is currently active for this session, or null if unknown.
    /// </summary>
    public bool? IsShuffleActive { get; set; }

    /// <summary>
    /// The current repeat mode for this session, or null if unknown.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MediaPlaybackAutoRepeatMode? RepeatMode { get; set; }
}
