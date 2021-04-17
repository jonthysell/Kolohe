// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace Kolohe.GUI
{
    public class App : Application
    {
        public static new App Current => (App)Application.Current;

        private Engine? _engine;
        public GraphicView? _graphicView;

        private Task? _engineTask;
        private readonly CancellationTokenSource _engineCTS = new CancellationTokenSource();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Startup += Desktop_Startup;
                desktop.Exit += Desktop_Exit;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void Desktop_Startup(object? sender, ControlledApplicationLifetimeStartupEventArgs e)
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var window = new MainWindow();
                _graphicView = new GraphicView(window, 80, 24);
                _engine = new Engine(_graphicView);

                window.Opened += Window_Opened;
                window.Closing += Window_Closing;

                desktop.MainWindow = window;
            }
        }

        private void Desktop_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
        {
        }

        private void Window_Opened(object? sender, EventArgs e)
        {
            _engineTask = Task.Run(async () =>
            {
                if (_engine is not null)
                {
                    await _engine.StartAsync(_engineCTS.Token);    
                }

                Dispatcher.UIThread.Post(() =>
                {
                    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        desktop.MainWindow.Closing -= Window_Closing;
                        desktop.MainWindow.Close();
                    }
                });
            });
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            _engineCTS.Cancel();
            _engine?.Halt();
            _engineTask?.Wait();
            e.Cancel = true;
        }
    }
}
