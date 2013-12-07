using System;

namespace Sharparam.Foobar2kLib.Tester
{
    class Program
    {
        static void Main()
        {
            var foobar = new Foobar();

            while (true)
            {
                var input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                    foobar.MessageManager.WriteMessage(input);
            }
        }
    }
}
