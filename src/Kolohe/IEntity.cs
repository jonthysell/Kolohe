// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

namespace Kolohe
{
    public interface IEntity
    {
        bool CanPlaceOnTile(MapTile mapTile);
    }
}
