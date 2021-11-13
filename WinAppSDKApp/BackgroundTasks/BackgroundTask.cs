using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace WinAppSDKApp.BackgroundTasks
{
    /// <summary>
    /// Base class for background tasks.
    /// </summary>
    public abstract class BackgroundTask
    {
        /// <summary>
        /// Registers the given <see cref="IBackgroundTaskInstance"/>.
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// Executes the given <see cref="IBackgroundTaskInstance"/> asynchronously.
        /// </summary>
        /// <param name="taskInstance">The <see cref="IBackgroundTaskInstance"/></param>
        /// <returns>A <see cref="Task"/></returns>
        public abstract Task RunAsyncInternal(IBackgroundTaskInstance taskInstance);

        /// <summary>
        /// Handles the <see cref="IBackgroundTaskInstance.Canceled"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="IBackgroundTaskInstance"/></param>
        /// <param name="reason">The <see cref="BackgroundTaskCancellationReason"/></param>
        public abstract void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

        /// <summary>
        /// Matches this object's type name to the given name.
        /// </summary>
        /// <param name="name">The name to match</param>
        /// <returns>Boolean, true if the names are equal (case-sensitive)</returns>
        public bool Match(string name)
        {
            return name == GetType().Name;
        }

        /// <summary>
        /// Subscribes the given <see cref="IBackgroundTaskInstance"/> to events and executes the given <see cref="IBackgroundTaskInstance"/> asynchronously.
        /// </summary>
        /// <param name="taskInstance">The <see cref="IBackgroundTaskInstance"/></param>
        /// <returns>A <see cref="Task"/></returns>
        public Task RunAsync(IBackgroundTaskInstance taskInstance)
        {
            SubscribeToEvents(taskInstance);

            return RunAsyncInternal(taskInstance);
        }

        /// <summary>
        /// Subscribes the given <see cref="IBackgroundTaskInstance"/> to events.
        /// </summary>
        /// <param name="taskInstance">The <see cref="IBackgroundTaskInstance"/></param>
        public void SubscribeToEvents(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }
    }
}