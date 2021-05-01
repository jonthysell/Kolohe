// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Kolohe
{
    public class Game
    {
        public int WorldSeed { get; private set; }

        public int GameSeed { get; private set; }

        private readonly Random _worldRand;

        private readonly Random _gameRand;

        private readonly List<GameInput> _inputHistory = new List<GameInput>();

        public int Time { get; private set; }

        public Map WorldMap { get; private set; }

        public readonly Creature?[,] Creatures;

        public Player Player { get; private set; }

        public static Game NewGame()
        {
            var rand = new Random();
            var game = new Game(rand.Next(), rand.Next());
            return game;
        }

        public static Game LoadGame(Stream input)
        {
            using var reader = new BinaryReader(input);

            int worldSeed = reader.ReadInt32();
            int gameSeed = reader.ReadInt32();

            var game = new Game(worldSeed, gameSeed);

            int length = reader.ReadInt32();
            for (int i = 0; i < length; i++)
            {
                var gameInput = (GameInput)reader.ReadByte();
                game.PlayInput(gameInput);
            }

            return game;
        }

        public Game(int worldSeed, int gameSeed)
        {
            WorldSeed = worldSeed;
            GameSeed = gameSeed;

            _worldRand = new Random(worldSeed);
            _gameRand = new Random(gameSeed);

            Time = 0;

            WorldMap = Map.GenerateWorldMap(_worldRand);
            Creatures = new Creature?[WorldMap.Width, WorldMap.Height];
            Player = Player.GeneratePlayer(_gameRand);

            SetPlayerPosition(WorldMap.Width / 2, WorldMap.Height / 2);
        }

        public void Save(Stream output)
        {
            using var writer = new BinaryWriter(output);

            writer.Write(WorldSeed);
            writer.Write(GameSeed);

            writer.Write(_inputHistory.Count);
            for (int i = 0; i < _inputHistory.Count; i++)
            {
                writer.Write((byte)_inputHistory[i]);
            }
        }

        internal void PlayInput(GameInput input)
        {
            switch (input)
            {
                case GameInput.PlayerMoveUp:
                case GameInput.PlayerMoveUpRight:
                case GameInput.PlayerMoveRight:
                case GameInput.PlayerMoveDownRight:
                case GameInput.PlayerMoveDown:
                case GameInput.PlayerMoveDownLeft:
                case GameInput.PlayerMoveLeft:
                case GameInput.PlayerMoveUpLeft:
                    TryPlayerMove((Direction)((int)input - (int)GameInput.PlayerMoveUp));
                    break;
                case GameInput.PlayerWait:
                    PlayerWait();
                    break;
            }
        }

        public bool TryPlayerMove(Direction direction)
        {
            (int dx, int dy) = direction.GetDeltas();
            int targetX = Player.X + dx;
            int targetY = Player.Y + dy;

            if (WorldMap.Contains(targetX, targetY) && Player.CanPlaceOnTile(WorldMap[targetX, targetY]))
            {
                var currentCreature = Creatures[targetX, targetY];
                if (currentCreature is null || currentCreature == Player)
                {
                    // Open position
                    Creatures[Player.X, Player.Y] = null;
                    SetPlayerPosition(targetX, targetY);
                    Tick((GameInput)((int)GameInput.PlayerMoveUp + (int)direction));
                    return true;
                }
            }

            return false;
        }

        public void PlayerWait()
        {
            Tick(GameInput.PlayerWait);
        }

        private void SetPlayerPosition(int targetX, int targetY)
        {
            Player.X = targetX;
            Player.Y = targetY;
            Creatures[targetX, targetY] = Player;
        }

        private void Tick(GameInput input)
        {
            Time++;
            _inputHistory.Add(input);
        }
    }
}
