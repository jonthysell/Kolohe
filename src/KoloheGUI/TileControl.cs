// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace Kolohe.GUI
{
    public class TileControl : TextBlock
    {
        public const int ScaleFactor = 1;
        public const int TileWidth = 16 * ScaleFactor;
        public const int TileHeight = 16 * ScaleFactor;
        //private TextBlock _child;

        public TileControl() : this(new GraphicTile()) { }

        public TileControl(GraphicTile graphicTile)
        {
            Width = TileWidth;
            Height = TileHeight;

            FontFamily = FontFamily.Parse("Square");
            FontSize = 16;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            TextAlignment = TextAlignment.Center;
            Update(graphicTile);
        }

        public void Update(GraphicTile graphicTile)
        {
            Background = new SolidColorBrush(graphicTile.BackgroundColor);
            Foreground = new SolidColorBrush(graphicTile.ForegroundColor);
            Text = graphicTile.Char.ToString();
        }

        public void Update(int tileX, int tileY)
        {
            SetValue(Canvas.LeftProperty, tileX * TileWidth);
            SetValue(Canvas.TopProperty, tileY * TileHeight);
        }
    }
}
