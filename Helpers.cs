using Windows.Media.Control;

/// <summary>
/// Helper class with static methods.
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Retrieves the display name of an application given its AppUserModelId.
    /// </summary>
    /// <param name="AppUserModelId">The AppUserModelId of the application.</param>
    /// <returns>The display name of the application if found; otherwise, "Unknown App".</returns>
    public static string GetAppId(string AppUserModelId)
    {
        string appName = "Unknown App";
        try
        {
            var appInfo = Windows.ApplicationModel.AppInfo.GetFromAppUserModelId(AppUserModelId);
            appName = appInfo.DisplayInfo.DisplayName;
        }
        catch (System.Exception)
        {

        }
        return appName;
    }

    /// <summary>
    /// Converts the detailed media properties from a GlobalSystemMediaTransportControlsSession into a simplified metadata object. This method extracts relevant information such as title, artist, album title, and album artist, and handles special cases for certain applications (like Apple Music) that may format their metadata differently. The resulting SimplifiedMetadata object provides a more concise and standardized representation of the media properties for use in the application.
    /// </summary>
    /// <param name="metadata">The detailed media properties from a GlobalSystemMediaTransportControlsSession, which may include title, artist, album title, and album artist information. This parameter can be null, in which case default values will be used for the metadata fields.</param>
    /// <param name="sourceAppUserModelId">The AppUserModelId of the source application, used to handle any special formatting cases for certain applications (e.g., Apple Music). This allows the method to correctly parse and extract metadata even when the source application formats its metadata in a non-standard way.</param>
    /// <returns>A SimplifiedMetadata object containing the title, artist, album title, and album artist information extracted from the provided metadata. If any of the metadata fields are missing or null, default values such as "Unknown Title", "Unknown Artist", "Unknown Album", and "Unknown Album Artist" will be used to ensure that the returned object always contains valid information.</returns>
    public static SimplifiedMetadata GetMetadata(GlobalSystemMediaTransportControlsSessionMediaProperties metadata, string sourceAppUserModelId)
    {
        var artist = metadata?.Artist ?? "Unknown Artist";
        var albumTitle = metadata?.AlbumTitle ?? "Unknown Album";

        if (sourceAppUserModelId.Contains("AppleMusicWin"))
        {
            // Apple Music merges artist and album with " — "
            var parts = artist.Split(" — ");
            if (parts.Length == 2)
            {
                artist = parts[0];
                albumTitle = parts[1];
            }
        }

        return new SimplifiedMetadata
        {
            Title = metadata?.Title ?? "Unknown Title",
            Artist = artist,
            AlbumTitle = albumTitle,
            AlbumArtist = metadata?.AlbumArtist ?? "Unknown Album Artist"
        };

    }
}