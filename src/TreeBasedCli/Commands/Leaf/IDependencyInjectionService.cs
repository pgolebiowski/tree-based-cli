namespace TreeBasedCli
{
    /// <summary>
    /// An interface with only one method, <see cref="IDependencyInjectionService.Resolve{T}" />,
    /// which <see cref="TreeBasedCli" /> uses to obtain instances for dependencies
    /// declared in the parser and handler classes for leaf commands.
    /// </summary>
    public interface IDependencyInjectionService
    {
        /// <summary>
        /// <see cref="TreeBasedCli" /> uses this method to obtain instances for dependencies
        /// declared in the parser and handler classes for leaf commands.
        /// </summary>
        T Resolve<T>() where T : notnull;
    }
}