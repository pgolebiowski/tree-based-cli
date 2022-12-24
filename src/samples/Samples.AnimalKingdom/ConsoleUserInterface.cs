using System;

namespace Samples.AnimalKingdom
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
