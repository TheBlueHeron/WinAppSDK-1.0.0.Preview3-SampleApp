using System.Threading.Tasks;

namespace WinAppSDKApp.Contracts.Services
{
    /// <summary>
    /// Interface definition for an <see cref="WinAppSDKApp.Services.ActivationService"/>.
    /// </summary>
    public interface IActivationService
    {
        /// <summary>
        /// Activates the application asynchronously.
        /// </summary>
        /// <param name="activationArgs">Activation arguments</param>
        /// <returns>A <see cref="Task"/></returns>
        Task ActivateAsync(object activationArgs);
    }
}