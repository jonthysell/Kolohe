// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;

namespace Kolohe
{
    public interface IView
    {
        Task<EngineInput> ReadInputAsync(CancellationToken token);
        Task UpdateViewAsync(Engine engine, EngineInput input, CancellationToken token);
    }
}