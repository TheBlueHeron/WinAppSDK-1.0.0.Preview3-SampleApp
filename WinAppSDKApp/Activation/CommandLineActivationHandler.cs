using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace GoogleMapper.Activation
{
    /// <summary>
    /// <see cref="IActivationHandler"/> for commandline arguments.
    /// <seealso>https://docs.microsoft.com/en-us/uwp/api/windows.applicationmodel.activation.commandlineactivatedeventargs</seealso>
    /// </summary>
    public class CommandLineActivationHandler : ActivationHandler<CommandLineActivatedEventArgsMock>
    {
        #region Overrides

        /// <inheritdoc/>
        protected override async Task HandleInternalAsync(CommandLineActivatedEventArgsMock args)
        {
            //CommandLineActivationOperation operation = args.Operation;

            // Because these are supplied by the caller, they should be treated as untrustworthy.
            string[] cmdLine = args.Arguments;

            // The directory where the command-line activation request was made.
            // This is typically not the install location of the app itself, but could be any arbitrary path.
            //string activationPath = operation.CurrentDirectoryPath;

            // TODO WTS: parse the cmdLineString to determine what to do.
            // If doing anything async, get a deferral first.
            // Using deferral = operation.GetDeferral()
            //     Await ParseCmdString(cmdLineString, activationPath)
            // End Using
            //
            // If the arguments warrant showing a different view on launch, that can be done here.
            // NavigationService.Navigate(GetType(CmdLineActivationSamplePage), cmdLineString)
            // If you do nothing, the app will launch like normal.

            await Task.CompletedTask;
        }

        /// <inheritdoc/>
        protected override bool CanHandleInternal(CommandLineActivatedEventArgsMock args)
        {
            return args?.Arguments.Any() ?? false;
            //return args?.Operation.Arguments.Any() ?? false; // Only handle a commandline launch if arguments are passed
        }

        #endregion
    }

    public class CommandLineActivatedEventArgsMock : ICommandLineActivatedEventArgs
    {
        public string[] Arguments { get; }
        public CommandLineActivationOperation Operation { get; }
        public ActivationKind Kind { get; }
        public ApplicationExecutionState PreviousExecutionState { get; }
        public SplashScreen SplashScreen { get; }

        public CommandLineActivatedEventArgsMock(string[] args, ActivationKind kind, ApplicationExecutionState previousState, SplashScreen splashScreen)
        {
            Arguments = args;
            //Operation = operation; // CommandLineActivationOperation operation,
            Kind = kind;
            PreviousExecutionState = previousState;
            SplashScreen = splashScreen;
        }
    }
}