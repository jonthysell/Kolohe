﻿// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kolohe
{
    public class Engine
    {
        public readonly IView View;

        public readonly Player Player;

        public readonly Map Map;

        public int Time { get; private set; }

        private readonly Random _gameSeed = new Random();
        private readonly Random _playerSeed = new Random();

        private bool _exitRequested = false;

        public Engine(IView view)
        {
            View = view;

            Player = new Player();
            Map = Map.GenerateWorldMap(_gameSeed);

            Time = 0;

            Player.X = Map.Width / 2;
            Player.Y = Map.Height / 2;
        }

        public async Task RefreshViewAsync()
        {
            await View.UpdateViewAsync(this, EngineInput.RefreshView);
        }

        public async Task StartAsync(CancellationToken token)
        {
            var input = EngineInput.RefreshView;
            while (!token.IsCancellationRequested && !_exitRequested)
            {
                await View.UpdateViewAsync(this, input);
                input = await View.ReadInputAsync();
                ProcessInput(input);
            }
        }

        private void ProcessInput(EngineInput input)
        {
            switch (input)
            {
                case EngineInput.DirectionUp:
                case EngineInput.DirectionUpRight:
                case EngineInput.DirectionRight:
                case EngineInput.DirectionDownRight:
                case EngineInput.DirectionDown:
                case EngineInput.DirectionDownLeft:
                case EngineInput.DirectionLeft:
                case EngineInput.DirectionUpLeft:
                    ProcessPlayerMove((Direction)((int)input - (int)EngineInput.DirectionUp));
                    break;
                case EngineInput.DirectionCenter:
                    Step();
                    break;
                case EngineInput.HaltEngine:
                    _exitRequested = true;
                    break;
            }
        }

        private void ProcessPlayerMove(Direction direction)
        {
            (int dx, int dy) = direction.GetDeltas();
            int targetX = Player.X + dx;
            int targetY = Player.Y + dy;

            if (Map.Contains(targetX, targetY))
            {
                if (Map[targetX, targetY] != MapTile.None)
                {
                    // Clear movement
                    Player.X = targetX;
                    Player.Y = targetY;
                    Step();
                }
            }
        }

        private void Step()
        {
            Time++;
        }
    }
}
