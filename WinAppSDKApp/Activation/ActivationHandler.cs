using System.Threading.Tasks;

namespace WinAppSDKApp.Activation
{
    /// <summary>
    /// Base class for <see cref="IActivationHandler"/> implementations.
    /// </summary>
    /// <typeparam name="T">The type of the arguments object</typeparam>
    /// <seealso>https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/WinUI/activation.md</seealso>
    public abstract class ActivationHandler<T> : IActivationHandler
        where T : class
    {
        /// <summary>
        /// Override this method to add additional activation logic.
        /// </summary>
        /// <param name="args">Arguments of type T</param>
        /// <returns>A <see cref="Task"/></returns>
        protected abstract Task HandleInternalAsync(T args);

        /// <summary>
        /// Executes activation logic asynchronously.
        /// </summary>
        /// <param name="args">The arguments object</param>
        /// <returns>A <see cref="Task"/></returns>
        public async Task HandleAsync(object args)
        {
            await HandleInternalAsync(args as T);
        }

        /// <summary>
        /// Determines whether this handler can handle the given arguments.
        /// </summary>
        /// <param name="args">The arguments object</param>
        /// <returns>Boolean</returns>
        public bool CanHandle(object args)
        {
            return args is T && CanHandleInternal(args as T);
        }

        /// <summary>
        /// Override this method to determine if this ActivationHandler should handle this activation args.
        /// </summary>
        /// <param name="args">Arguments of type T</param>
        /// <returns>Bool</returns>
        protected virtual bool CanHandleInternal(T args)
        {
            return true;
        }
    }
}
