// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Kolohe
{
    public interface IView
    {
        Task<EngineInput> ReadInputAsync();
        Task UpdateViewAsync(Engine engine, EngineInput input);
    }
}