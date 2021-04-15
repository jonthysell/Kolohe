// Copyright (c) Jon Thysell <http://jonthysell.com>
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

        public Engine(IView view)
        {
            View = view;

            Player = new Player();
            Map = Map.GenerateWorldMap(_gameSeed);

            Time = 0;

            Player.X = 2;
            Player.Y = 2;
        }

        public async Task RefreshViewAsync()
        {
            await View.UpdateViewAsync(this, EngineInput.RefreshView);
        }

        public async Task StartAsync(CancellationToken token)
        {
            await View.UpdateViewAsync(this, EngineInput.RefreshView);
            while (!token.IsCancellationRequested)
            {
                var input = await View.ReadInputAsync();
                ProcessInput(input);
                await View.UpdateViewAsync(this, input);
            }
        }

        private void ProcessInput(EngineInput input)
        {
            switch (input)
            {
                case EngineInput.MoveUp:
                case EngineInput.MoveUpRight:
                case EngineInput.MoveRight:
                case EngineInput.MoveDownRight:
                case EngineInput.MoveDown:
                case EngineInput.MoveDownLeft:
                case EngineInput.MoveLeft:
                case EngineInput.MoveUpLeft:
                    ProcessMove((Direction)input);
                    break;
                case EngineInput.Wait:
                    Step();
                    break;
            }
        }

        private void ProcessMove(Direction direction)
        {
            int targetX = Player.X + _directionDelta[(int)direction][0];
            int targetY = Player.Y + _directionDelta[(int)direction][1];

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

        private static int[][] _directionDelta = new int[][]
        {
            new int[] { 0, -1 },
            new int[] { 1, -1 },
            new int[] { 1, 0 },
            new int[] { 1, 1 },
            new int[] { 0, 1 },
            new int[] { -1, 1 },
            new int[] { -1, 0 },
            new int[] { -1, 1 },
        };
    }
}
