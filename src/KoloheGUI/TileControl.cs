// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using Avalonia.Controls;
using Avalonia.Media;

namespace Kolohe.GUI
{
    public class TileControl : Panel
    {
        public const int TileWidth = 8;
        public const int TileHeight = 16;
        private TextBlock _child;

        public TileControl() : this(new GraphicTile()) { }

        public TileControl(GraphicTile graphicTile)
        {
            Width = TileWidth;
            Height = TileHeight;

            _child = new TextBlock();
            _child.FontFamily = FontFamily.Parse("Consolas");
            Children.Add(_child);
            Update(graphicTile);
        }

        public void Update(GraphicTile graphicTile)
        {
            Background = new SolidColorBrush(graphicTile.BackgroundColor);
            _child.Foreground = new SolidColorBrush(graphicTile.ForegroundColor);
            _child.Text = graphicTile.Char.ToString();
        }

        public void Update(int tileX, int tileY)
        {
            SetValue(Canvas.LeftProperty, tileX * TileWidth);
            SetValue(Canvas.TopProperty, tileY * TileHeight);
        }
    }
}
