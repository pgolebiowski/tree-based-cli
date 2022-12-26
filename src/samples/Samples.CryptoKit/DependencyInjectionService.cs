using Microsoft.Extensions.DependencyInjection;
using TreeBasedCli;

namespace Samples.CryptoKit
{
    public sealed class DependencyInjectionService : IDependencyInjectionService
    {
        private IServiceProvider serviceProvider;

        private static readonly Lazy<DependencyInjectionService> lazy
            = new Lazy<DependencyInjectionService>(() => new DependencyInjectionService());

        private DependencyInjectionService()
        {
            ServiceCollection services = BuildServiceCollection();
            this.serviceProvider = services.BuildServiceProvider();
        }

        public static DependencyInjectionService Instance => lazy.Value;

        private static ServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();

            // nothing here

            return services;
        }

        public T GetUnregisteredService<T>() where T : notnull
        {
            return ActivatorUtilities.CreateInstance<T>(this.serviceProvider);
        }

        public T Resolve<T>() where T : notnull
        {
            return this.GetUnregisteredService<T>();
        }
    }
}
