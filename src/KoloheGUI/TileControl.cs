﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace Kolohe.GUI
{
    public class TileControl : Panel
    {
        public const int ScaleFactor = 1;
        public const int TileWidth = 8 * ScaleFactor;
        public const int TileHeight = 16 * ScaleFactor;
        private TextBlock _child;

        public TileControl() : this(new GraphicTile()) { }

        public TileControl(GraphicTile graphicTile)
        {
            Width = TileWidth;
            Height = TileHeight;

            _child = new TextBlock();
            _child.FontFamily = FontFamily.Parse("Consolas");
            _child.FontSize *= ScaleFactor;
            _child.HorizontalAlignment = HorizontalAlignment.Center;
            _child.VerticalAlignment = VerticalAlignment.Center;
            _child.TextAlignment = TextAlignment.Center;
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
