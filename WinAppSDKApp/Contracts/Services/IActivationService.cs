using System.Threading.Tasks;

namespace GoogleMapper.Contracts.Services
{
    /// <summary>
    /// Interface definition for an <see cref="GoogleMapper.Services.ActivationService"/>.
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