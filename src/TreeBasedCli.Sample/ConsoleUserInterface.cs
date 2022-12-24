using System;

namespace TreeBasedCli.Sample
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
