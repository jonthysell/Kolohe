// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading;

namespace Kolohe.CLI
{
    class Program
    {
        static CancellationTokenSource _cts = new CancellationTokenSource();

        static void Main(string[] args)
        {
            Console.TreatControlCAsInput = true;
            Console.CursorVisible = false;
            Console.ResetColor();

            Console.Title = $"{AppInfo.Name} v{AppInfo.Version}";

            var view = new ConsoleView();
            var engine = new Engine(view);

            var task = engine.StartAsync(_cts.Token);
            task.Wait();

            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        }
    }
}
