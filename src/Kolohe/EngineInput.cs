// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.ComponentModel;

namespace Kolohe
{
    [DefaultValue(None)]
    public enum EngineInput
    {
        None = -1,
        DirectionUp,
        DirectionUpRight,
        DirectionRight,
        DirectionDownRight,
        DirectionDown,
        DirectionDownLeft,
        DirectionLeft,
        DirectionUpLeft,
        DirectionCenter,
        ModifiedDirectionUp,
        ModifiedDirectionUpRight,
        ModifiedDirectionRight,
        ModifiedDirectionDownRight,
        ModifiedDirectionDown,
        ModifiedDirectionDownLeft,
        ModifiedDirectionLeft,
        ModifiedDirectionUpLeft,
        ModifiedDirectionCenter,
        RefreshView,
        HaltEngine,
    }
}
