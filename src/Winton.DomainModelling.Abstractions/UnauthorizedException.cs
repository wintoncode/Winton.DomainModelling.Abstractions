namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     An error indicating that the action being performed is not authorized.
    /// </summary>
    public class UnauthorizedException : DomainException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnauthorizedException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}