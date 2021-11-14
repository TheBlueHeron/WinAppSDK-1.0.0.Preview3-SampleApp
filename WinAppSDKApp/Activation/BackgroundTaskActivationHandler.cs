using WinAppSDKApp.BackgroundTasks;
using WinAppSDKApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace WinAppSDKApp.Activation
{
    public class BackgroundTaskActivationHandler : ActivationHandler<BackgroundActivatedEventArgs>
    {
        #region Properties

        /// <summary>
        /// Gets the collection of <see cref="BackgroundTask"/> instances.
        /// </summary>
        public static IEnumerable<BackgroundTask> BackgroundTasks { get; } = CreateInstances();

        #endregion

        #region Public methods and functions

        /// <summary>
        /// Returns the <see cref="BackgroundTaskRegistration"/> for <typeparamref name="T"/>, if present.
        /// </summary>
        /// <typeparam name="T">A <see cref="BackgroundTask"/> derived type</typeparam>
        /// <returns>A <see cref="BackgroundTaskRegistration"/> if one was registered for <typeparamref name="T"/>, else null</returns>
        public static BackgroundTaskRegistration GetBackgroundTaskRegistration<T>() where T : BackgroundTask
        {
            return BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == typeof(T).Name)
                ? (BackgroundTaskRegistration)BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == typeof(T).Name).Value
                : null;
        }

        /// <summary>
        /// Registers all <see cref="BackgroundTask"/> instances asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        public static async Task RegisterBackgroundTasksAsync()
        {
            BackgroundExecutionManager.RemoveAccess();
            BackgroundAccessStatus result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result is BackgroundAccessStatus.DeniedBySystemPolicy or BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (BackgroundTask task in BackgroundTasks)
            {
                task.Register();
            }
        }

        /// <summary>
        /// Starts the given <see cref="IBackgroundTaskInstance"/> in a 'Fire and forget' fashion.
        /// </summary>
        /// <param name="taskInstance">The <see cref="IBackgroundTaskInstance"/> to start</param>
        public static void Start(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTask task = BackgroundTasks.FirstOrDefault(b => b.Match(taskInstance?.Task?.Name));

            if (task == null)
            {
                // This condition should not be met. If it is, it means the background task to start was not found in the background tasks managed by this service.
                // Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                return;
            }

            task.RunAsync(taskInstance).FireAndForget();
        }

        #endregion

        #region Private methods and functions

        /// <summary>
        /// Creates all <see cref="BackgroundTask"/> objects needed.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{BackgroundTask}"/></returns>
        private static IEnumerable<BackgroundTask> CreateInstances()
        {
            List<BackgroundTask> backgroundTasks = new();

            backgroundTasks.Add(new BackgroundTask1());

            return backgroundTasks;
        }

        /// <inheritdoc/>
        protected override async Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            Start(taskInstance: args.TaskInstance);
            await Task.CompletedTask;
        }

        #endregion
    }
}