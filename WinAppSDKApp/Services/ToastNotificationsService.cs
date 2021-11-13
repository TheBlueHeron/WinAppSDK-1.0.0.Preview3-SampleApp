using CommunityToolkit.WinUI.Notifications;
using WinAppSDKApp.Activation;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;

namespace WinAppSDKApp.Services
{
    public class ToastNotificationsService : ActivationHandler<ToastNotificationActivatedEventArgsMock>
    {
        #region Public methods and functions

        /// <summary>
        /// Displays the given <see cref="ToastNotification"/>.
        /// </summary>
        /// <param name="toastNotification">The <see cref="ToastNotification"/> to display</param>
        public void ShowToastNotification(ToastNotification toastNotification)
        {
            try
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
            catch (Exception ex)
            {
                // TODO: log
                // TODO WTS: Adding ToastNotification can fail in rare conditions, please handle exceptions as appropriate to your scenario.
            }
        }

        /// <summary>
        /// Sample notification display.
        /// </summary>
        public void ShowSample()
        {
            ToastVisual visual = new();

            visual.BindingGeneric = new ToastBindingGeneric();
            visual.BindingGeneric.Children.Add(new AdaptiveText() { Text = "Sample Toast Notification" });
            visual.BindingGeneric.Children.Add(new AdaptiveText() { Text = "Click OK to see how activation from a toast notification can be handled in the ToastNotificationService." });

            // More about Toast Buttons at https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastbutton
            ToastActionsCustom actions = new();
            actions.Buttons.Add(new ToastButton("OK", "ToastButtonActivationArguments") { ActivationType = ToastActivationType.Foreground });
            actions.Buttons.Add(new ToastButtonDismiss("Cancel"));

            // More about the Launch property at https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent
            ToastContent content = new();
            content.Launch = "ToastContentActivationParams";
            content.Visual = visual;
            content.Actions = actions;

            // Add the content to the toast
            // TODO WTS: Set a unique identifier for this notification within the notification group. (optional)
            // More details at https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification.tag
            ToastNotification toast = new(content.GetXml()) { Tag = "ToastTag" };

            // And show the toast
            ShowToastNotification(toast);
        }

        #endregion

        #region Overrides

        /// <inheritdoc/>
        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgsMock args)
        {
            // TODO WTS: Handle activation from toast notification
            // More details at https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast

            await Task.CompletedTask;
        }

        #endregion
    }

    public class ToastNotificationActivatedEventArgsMock : IToastNotificationActivatedEventArgs
    {
        public ValueSet UserInput { get; }
        public string Argument { get; }
        public ActivationKind Kind { get; }
        public ApplicationExecutionState PreviousExecutionState { get; }
        public SplashScreen SplashScreen { get; }

        public ToastNotificationActivatedEventArgsMock(string arg, ValueSet userInput, ActivationKind kind, ApplicationExecutionState previousState, SplashScreen splashScreen)
        {
            Argument = arg;
            UserInput = userInput;
            Kind = kind;
            PreviousExecutionState = previousState;
            SplashScreen = splashScreen;
        }
    }
}