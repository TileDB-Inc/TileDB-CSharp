using System;
using System.Diagnostics;

namespace TileDB.CSharp
{
    partial class Config
    {
        partial struct RestConfig
        {
            /// <summary>
            /// Configuration options of libcurl which is used to perform HTTP requests.
            /// </summary>
            public CurlConfig Curl
            {
                get => new(_obj);
                set
                {
                    if (value._obj == _obj)
                    {
                        return;
                    }

                    if (value._obj is ConfigBag bag)
                    {
                        // If we have two freestanding RestConfig objects x1 and x2, and
                        // call x1.Rest.Curl = x2.Rest.Curl, all x2's options would be set,
                        // and so we specify the prefix of the options we want.
                        ImportFromBag(_obj, bag, "rest.curl.");
                        return;
                    }

                    CurlConfig curl = Curl;
                    curl.Verbose = value.Verbose;
                    curl.BufferSize = value.BufferSize;
                }
            }

            /// <summary>
            /// Contains configuration options of libcurl which is used to perform HTTP requests.
            /// </summary>
            /// <seealso cref="Curl"/>
            public readonly struct CurlConfig
            {
                internal readonly object _obj;

                public CurlConfig() => _obj = new ConfigBag();

                internal CurlConfig(object obj)
                {
                    Debug.Assert(obj is CurlConfig or ConfigBag);
                    _obj = obj;
                }

                private static ReadOnlySpan<byte> VerboseName => "rest.curl.verbose"u8;

                /// <summary>
                /// Whether to print verbose logs to stdout.
                /// </summary>
                public bool Verbose
                {
                    get => GetBoolUnsafe(_obj, VerboseName, false);
                    set => SetBoolUnsafe(_obj, VerboseName, value);
                }

                private static ReadOnlySpan<byte> BufferSizeName => "rest.curl.buffer_size"u8;

                /// <summary>
                /// The size of curl's internal buffer.
                /// </summary>
                public uint BufferSize
                {
                    get => GetUInt32Unsafe(_obj, BufferSizeName, 524288);
                    set => SetUInt32Unsafe(_obj, BufferSizeName, value);
                }
            }
        }
    }
}
