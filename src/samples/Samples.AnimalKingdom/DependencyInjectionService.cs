using System;
using Microsoft.Extensions.DependencyInjection;
using TreeBasedCli;

namespace Samples.AnimalKingdom
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

            services.AddTransient<IUserInterface, ConsoleUserInterface>();

            return services;
        }

        public T GetUnregisteredService<T>() where T : notnull
        {
            return ActivatorUtilities.CreateInstance<T>(this.serviceProvider);
        }

        public TParser ResolveParser<TArguments, TParser>()
            where TArguments : IParsedCommandArguments
            where TParser : ICommandArgumentParser<TArguments>
        {
            return this.GetUnregisteredService<TParser>();
        }

        public THandler ResolveHandler<TArguments, THandler>()
            where TArguments : IParsedCommandArguments
            where THandler : ICommandHandler<TArguments>
        {
            return this.GetUnregisteredService<THandler>();
        }
    }
}
