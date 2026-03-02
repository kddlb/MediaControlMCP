/// <summary>
/// Timeline and seekability data for a media session.
/// </summary>
public class SessionTimelineInfo
{
    /// <summary>
    /// Whether this session currently supports changing playback position.
    /// </summary>
    public required bool IsSeekEnabled { get; set; }

    /// <summary>
    /// The current playback position in seconds.
    /// </summary>
    public required double PositionSeconds { get; set; }

    /// <summary>
    /// The total media length in seconds.
    /// </summary>
    public required double LengthSeconds { get; set; }

    /// <summary>
    /// The minimum seekable position in seconds.
    /// </summary>
    public required double MinSeekSeconds { get; set; }

    /// <summary>
    /// The maximum seekable position in seconds.
    /// </summary>
    public required double MaxSeekSeconds { get; set; }

    /// <summary>
    /// The timeline start time in seconds.
    /// </summary>
    public required double StartSeconds { get; set; }

    /// <summary>
    /// The timeline end time in seconds.
    /// </summary>
    public required double EndSeconds { get; set; }

    /// <summary>
    /// The UTC timestamp when this timeline snapshot was last updated by the system.
    /// </summary>
    public required DateTimeOffset LastUpdatedTimeUtc { get; set; }
}
