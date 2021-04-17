// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.ComponentModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

using Kolohe;

namespace Kolohe.GUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Opened += MainWindow_Opened;
            Closing += MainWindow_Closing;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void MainWindow_Opened(object? sender, System.EventArgs e)
        {
            App.Current.Engine.StartAsync(App.Current.EngineCTS.Token);
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            App.Current.EngineCTS.Cancel();
        }
    }
}
