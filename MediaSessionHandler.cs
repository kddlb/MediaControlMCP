using Microsoft.Extensions.Hosting;
using Windows.Media.Control;

/// <summary>
/// Singleton service that manages the lifecycle of the GlobalSystemMediaTransportControlsSessionManager.
/// This ensures that the manager is initialized once and shared across the application, avoiding redundant initializations and potential resource leaks. The service implements IHostedService to integrate with the application's hosting lifecycle, allowing it to start and stop gracefully with the application. The Dispose method ensures that any resources held by the manager are released when the service is stopped.
/// </summary>
public class MediaSessionHandler : IHostedService, IDisposable
{
    /// <summary>
    /// The GlobalSystemMediaTransportControlsSessionManager instance that is initialized when the service starts. This manager is used to access and control media sessions across the system. It is marked as nullable to allow for proper disposal and cleanup when the service stops. The manager is initialized in the StartAsync method and set to null in the Dispose method to ensure that resources are released appropriately.
    /// </summary>
    private GlobalSystemMediaTransportControlsSessionManager? _manager;
    /// <summary>
    /// Starts the service by initializing the GlobalSystemMediaTransportControlsSessionManager. This method is called when the application starts and is responsible for setting up the media session manager that will be used throughout the application's lifecycle. The manager is initialized asynchronously, and any exceptions during initialization are not currently handled, which may be an area for improvement to ensure robustness. Once initialized, the manager can be accessed by other parts of the application to retrieve media session information and control playback.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous start operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _manager = await GlobalSystemMediaTransportControlsSessionManager
            .RequestAsync()
            .AsTask(cancellationToken);

        return;
    }

    /// <summary>
    /// Stops the service by disposing of the GlobalSystemMediaTransportControlsSessionManager. This method is called when the application is shutting down and is responsible for cleaning up any resources held by the media session manager. The Dispose method is called to release resources, and the manager is set to null to ensure that it is no longer accessible after the service has stopped. This helps prevent memory leaks and ensures that the application can shut down gracefully without leaving behind unmanaged resources. The method returns a completed task since there are no asynchronous operations needed for stopping the service.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous stop operation.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }
    /// <summary>
    /// Disposes of the GlobalSystemMediaTransportControlsSessionManager by setting it to null. This method is called when the service is stopped to ensure that any resources held by the manager are released properly. By setting the manager to null, we allow the garbage collector to reclaim any memory used by the manager instance, preventing potential memory leaks. This is a crucial step in managing the lifecycle of the media session manager and ensuring that the application can shut down cleanly without leaving behind unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _manager = null;
    }

    /// <summary>
    /// Returns the currently initialized media session manager, or null when not yet initialized.
    /// </summary>
    public GlobalSystemMediaTransportControlsSessionManager? TryGetManager()
    {
        return _manager;
    }
}
