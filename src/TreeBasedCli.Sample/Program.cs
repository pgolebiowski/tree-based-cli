using System;
using System.Threading.Tasks;

namespace TreeBasedCli.Sample
{
    internal class Program
    {
        private static async Task<int> Main(string[] arguments)
        {
            ArgumentHandlerSettings argumentHandlerSettings = ArgumentHandlerSettingsBuilder.Build();
            var argumentHandler = new ArgumentHandler(argumentHandlerSettings);

            try
            {
                await argumentHandler.HandleAsync(arguments);
            }
            catch (MessageOnlyException exception)
            {
                Console.WriteLine(exception.Message);
                return 1;
            }
            catch (Exception exception)
            {
                string type = exception.GetType().Name;

                Console.WriteLine($"Program aborted with an exception of type '{type}':");
                Console.WriteLine($"\n{exception.Message}");
                Console.WriteLine($"\n{exception.StackTrace}");

                if (exception.InnerException is not null)
                {
                    Console.WriteLine("\nInner exception:");
                    Console.WriteLine($"\n{exception.InnerException.Message}");
                    Console.WriteLine($"\n{exception.InnerException.StackTrace}");
                }

                return 1;
            }

            return 0;
        }
    }
}
