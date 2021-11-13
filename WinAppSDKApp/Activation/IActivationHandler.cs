using System.Threading.Tasks;

namespace WinAppSDKApp.Activation
{
    /// <summary>
    /// Interface definition for activation handlers.
    /// </summary>
    public interface IActivationHandler
    {
        /// <summary>
        /// Determines whether the handler can handle these arguments.
        /// </summary>
        bool CanHandle(object args);

        /// <summary>
        /// Handles the given arguments asynchronously.
        /// </summary>
        Task HandleAsync(object args);
    }
}