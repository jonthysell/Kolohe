// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.ComponentModel;

namespace Kolohe
{
    [DefaultValue(None)]
    public enum EngineInput
    {
        None = -1,
        MoveUp,
        MoveUpRight,
        MoveRight,
        MoveDownRight,
        MoveDown,
        MoveDownLeft,
        MoveLeft,
        MoveUpLeft,
        Wait,
        RefreshView,
    }
}
