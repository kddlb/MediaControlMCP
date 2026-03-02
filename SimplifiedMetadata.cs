/// <summary>
/// Class to represent media metadata properties.
/// </summary>
public class SimplifiedMetadata
{
    /// <summary>
    /// The title of the media item, if available.
    /// </summary>
    public string? Title { get; set; }
    /// <summary> 
    /// The artist or primary performer of the media item, if available.
    /// </summary>
    public string? Artist { get; set; }
    /// <summary>
    /// The title of the album, if available.
    /// </summary>
    public string? AlbumTitle { get; set; }
    /// <summary>
    /// The album artist, if available. This is often the same as Artist but can differ for compilation albums or certain metadata configurations.
    /// </summary>
    public string? AlbumArtist { get; set; }

}