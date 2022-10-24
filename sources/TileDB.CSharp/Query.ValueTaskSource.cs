using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace TileDB.CSharp
{
    unsafe partial class Query
    {
        private ValueTaskSource? _reusableValueTaskSource;

        internal ValueTaskSource GetValueTaskSource() =>
            Interlocked.Exchange(ref _reusableValueTaskSource, null) ?? new(this);

        internal sealed class ValueTaskSource : IValueTaskSource
        {
            // .NET uses this environment variable to directly run async I/O callbacks
            // on dedicated I/O threads instead of the general-purpose thread pool. We
            // can reuse it and give the option to run the async callback directly on
            // TileDB Core's thread.
            private static readonly bool UnsafeInlineIOCompletionCallbacks =
                Environment.GetEnvironmentVariable("DOTNET_SYSTEM_NET_SOCKETS_INLINE_COMPLETIONS") == "1";

            private ManualResetValueTaskSourceCore<bool> _core;
            private readonly Query _query;

            public ValueTaskSource(Query query)
            {
                _query = query;
                if (!UnsafeInlineIOCompletionCallbacks)
                {
                    _core.RunContinuationsAsynchronously = true;
                }
            }

            public ValueTask AsValueTask() => new(this, _core.Version);

            public void GetResult(short token)
            {
                try
                {
                    _core.GetResult(token);
                }
                finally
                {
                    _core.Reset();
                    Volatile.Write(ref _query._reusableValueTaskSource, this);
                }
            }

            public ValueTaskSourceStatus GetStatus(short token) => _core.GetStatus(token);

            public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) =>
                _core.OnCompleted(continuation, state, token, flags);

            internal void SetResult() => _core.SetResult(false);
        }
    }
}
