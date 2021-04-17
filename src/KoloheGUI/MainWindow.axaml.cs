// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Kolohe.GUI
{
    public class MainWindow : Window
    {
        public Canvas? ScreenCanvas => _screenCanvas ??= this.FindControl<Canvas>(nameof(ScreenCanvas));
        private Canvas? _screenCanvas;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
