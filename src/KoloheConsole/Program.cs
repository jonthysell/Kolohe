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
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.CursorVisible = false;
            Console.ResetColor();

            var view = new ConsoleView();
            var engine = new Engine(view);

            var task = engine.StartAsync(_cts.Token);
            task.Wait();

            Console.CursorVisible = true;
            Console.ResetColor();
            Console.Clear();
        }

        private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
        {
            _cts.Cancel();
            e.Cancel = true;
        }
    }
}
