using Windows.ApplicationModel.Background;
using Windows.System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace GoogleMapper.BackgroundTasks
{
    /// <summary>
    /// Sample <see cref="BackgroundTask"/>.
    /// </summary>
    public sealed class BackgroundTask1 : BackgroundTask
    {
        #region Objects and variables

        private bool _cancelRequested = false;
        private BackgroundTaskDeferral _deferral;
        private IBackgroundTaskInstance _taskInstance;

        #endregion

        #region Properties

        /// <summary>
        /// Execution progress message.
        /// </summary>
        public static string Message { get; set; }

        #endregion

        #region Overrides

        /// <inheritdoc/>
        public override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;

            // TODO WTS: Insert code to handle the cancelation request here.
            // Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/handle-a-cancelled-background-task
        }

        /// <inheritdoc/>
        public override void Register()
        {
            string taskName = GetType().Name;
            IBackgroundTaskRegistration taskRegistration = BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName).Value;

            if (taskRegistration == null)
            {
                BackgroundTaskBuilder builder = new() { Name = taskName };

                // TODO WTS: Define the trigger for your background task and set any (optional) conditions
                // More details at https://docs.microsoft.com/windows/uwp/launch-resume/create-and-register-an-inproc-background-task
                builder.SetTrigger(new TimeTrigger(15, false));
                builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));

                taskRegistration = builder.Register();
            }
        }

        /// <inheritdoc/>
        public override Task RunAsyncInternal(IBackgroundTaskInstance taskInstance)
        {
            if (_taskInstance == null)
            {
                return Task.CompletedTask;
            }

            _deferral = taskInstance.GetDeferral();

            return Task.Run(() =>
            {
                // TODO WTS: Insert the code that should be executed in the background task here.
                // This sample initializes a timer that counts to 100 in steps of 10. It updates Message each time.

                // Documentation:
                //      * General: https://docs.microsoft.com/windows/uwp/launch-resume/support-your-app-with-background-tasks
                //      * Debug: https://docs.microsoft.com/windows/uwp/launch-resume/debug-a-background-task
                //      * Monitoring: https://docs.microsoft.com/windows/uwp/launch-resume/monitor-background-task-progress-and-completion

                // To show the background progress and message on any page in the application,
                // subscribe to the Progress and Completed events.
                // You can do this via "BackgroundTaskService.GetBackgroundTasksRegistration"

                _taskInstance = taskInstance;
                _ = ThreadPoolTimer.CreatePeriodicTimer(new TimerElapsedHandler(SampleTimerCallback), TimeSpan.FromSeconds(1));
            });
        }

        #endregion

        #region Private methods and functions

        /// <summary>
        /// Callback function for the timer.
        /// </summary>
        /// <param name="timer">The <see cref="ThreadPoolTimer"/></param>
        private void SampleTimerCallback(ThreadPoolTimer timer)
        {
            if (!_cancelRequested && (_taskInstance.Progress < 100))
            {
                _taskInstance.Progress += 10;
                Message = $"Background Task {_taskInstance.Task.Name} running";
            } else
            {
                timer.Cancel();

                if (_cancelRequested)
                {
                    Message = $"Background Task {_taskInstance.Task.Name} cancelled";
                } else
                {
                    Message = $"Background Task {_taskInstance.Task.Name} finished";
                }

                _deferral?.Complete();
            }
        }

        #endregion
    }
}