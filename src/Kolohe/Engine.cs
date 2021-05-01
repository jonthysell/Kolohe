// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Kolohe
{
    public class Engine
    {
        public readonly IView View;

        public readonly Player Player;

        public readonly Map Map;

        public readonly Creature?[,] Creatures;

        public int Time { get; private set; }

        private readonly Random _worldRandom = new Random();

        private bool _exitRequested = false;

        public Engine(IView view)
        {
            View = view;

            Player = new Player();
            Map = Map.GenerateWorldMap(_worldRandom);
            Creatures = new Creature?[Map.Width, Map.Height];

            Time = 0;

            SetPlayerPosition(Map.Width / 2, Map.Height / 2);
        }

        public void Halt()
        {
            _exitRequested = true;
        }

        public async Task StartAsync(CancellationToken token)
        {
            var input = EngineInput.RefreshView;
            while (!token.IsCancellationRequested && !_exitRequested)
            {
                await View.UpdateViewAsync(this, input, token);
                input = await View.ReadInputAsync(token);
                ProcessInput(input);
            }
        }

        private void ProcessInput(EngineInput input)
        {
            Trace.TraceInformation($"ProcessInput(EngineInput.{input});");
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

            if (Map.Contains(targetX, targetY) && Player.CanPlaceOnTile(Map[targetX, targetY]))
            {
                var currentCreature = Creatures[targetX, targetY];
                if (currentCreature is null || currentCreature == Player)
                {
                    // Open position
                    Creatures[Player.X, Player.Y] = null;
                    SetPlayerPosition(targetX, targetY);
                    Step();
                }
            }
        }

        private void SetPlayerPosition(int targetX, int targetY)
        {
            Player.X = targetX;
            Player.Y = targetY;
            Creatures[targetX, targetY] = Player;
        }

        private void Step()
        {
            Time++;
        }
    }
}
