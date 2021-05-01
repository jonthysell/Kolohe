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

        public readonly Game Game;

        private bool _exitRequested = false;

        public Engine(IView view)
        {
            View = view;

            Game = Game.NewGame();
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
                    Game.TryPlayerMove((Direction)((int)input - (int)EngineInput.DirectionUp));
                    break;
                case EngineInput.DirectionCenter:
                    Game.PlayerWait();
                    break;
                case EngineInput.HaltEngine:
                    Halt();
                    break;
            }
        }
    }
}
