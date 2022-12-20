using System;
using System.Diagnostics;
using System.Linq;

namespace TileDB.CSharp
{
    partial class Config
    {
        /// <summary>
        /// Configuration options related to connectivity with remote arrays.
        /// </summary>
        public RestConfig Rest
        {
            get => new(this);
            set
            {
                // If the value points to this object, the options are already set.
                if (value._obj == this)
                {
                    return;
                }

                // If the value is backed by a ConfigBag (i.e. was created with new()),
                // we use ImportFromBag instead of setting each property. It is more efficient
                // because the values are already serialized into bytes and we only assign those we changed.
                if (value._obj is ConfigBag bag)
                {
                    ImportFromBag(bag);
                    return;
                }

                // The remaining case is manually copying the properties from one object to another.
                RestConfig rest = Rest;
                rest.ServerAddress = value.ServerAddress;
                rest.ServerSerializationFormat = value.ServerSerializationFormat;
                rest.Username = value.Username;
                rest.Password = value.Password;
                rest.Token = value.Token;
                rest.ResubmitIncomplete = value.ResubmitIncomplete;
                rest.DangerousIgnoreSslValidation = value.DangerousIgnoreSslValidation;
                rest.CreationAccessCredentialsName = value.CreationAccessCredentialsName;
                rest.RetryHttpCodes = value.RetryHttpCodes;
                rest.RetryCount = value.RetryCount;
                rest.RetryInitialDelay = value.RetryInitialDelay;
                rest.RetryDelayFactor = value.RetryDelayFactor;
                rest.LoadMetadataOnArrayOpen = value.LoadMetadataOnArrayOpen;
                rest.LoadNonEmptyDomainOnArrayOpen = value.LoadNonEmptyDomainOnArrayOpen;
                rest.UseRefactoredArrayOpen = value.UseRefactoredArrayOpen;
                rest.Curl = value.Curl;
            }
        }

        /// <summary>
        /// Contains configuration options related to connectivity with remote arrays hosted in a REST server.
        /// </summary>
        /// <seealso cref="Rest"/>
        public readonly partial struct RestConfig
        {
            internal readonly object _obj;

            public RestConfig() => _obj = new ConfigBag();

            internal RestConfig(Config config) => _obj = config;

            private static ReadOnlySpan<byte> ServerAddressName => "rest.server_address"u8;

            /// <summary>
            /// The URL to the REST server.
            /// </summary>
            public Uri ServerAddress
            {
                get => new(GetStringUnsafe(_obj, ServerAddressName, "https://api.tiledb.com"));
                set => SetStringUnsafe(_obj, ServerAddressName, value.AbsoluteUri);
            }

            private static ReadOnlySpan<byte> ServerSerializationFormatName => "rest.server_serialization_format"u8;

            /// <summary>
            /// The serialization format to be used for remote array requests.
            /// </summary>
            public RestServerSerializationFormat ServerSerializationFormat
            {
                get
                {
                    ReadOnlySpan<byte> value = GetUnsafe(this, ServerSerializationFormatName);

                    if (value.IsEmpty || value.SequenceEqual("CAPNP"u8))
                    {
                        return RestServerSerializationFormat.Capnp;
                    }

                    Debug.Assert(value.SequenceEqual("JSON"u8), "Invalid server serialization format value");
                    return RestServerSerializationFormat.Json;
                }
                set
                {
                    ReadOnlySpan<byte> valueString = value switch
                    {
                        RestServerSerializationFormat.Capnp => "CAPNP"u8,
                        RestServerSerializationFormat.Json => "JSON"u8,
                        _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid server serialization format value"),
                    };
                    SetUnsafe(this, ServerSerializationFormatName, valueString);
                }
            }

            private static ReadOnlySpan<byte> UsernameName => "rest.username"u8;

            /// <summary>
            /// The username to login to the REST server.
            /// </summary>
            /// <remarks>
            /// Will be ignored if <see cref="Token"/> is not empty.
            /// </remarks>
            /// <seealso cref="Password"/>
            public string Username
            {
                get => GetStringUnsafe(_obj, UsernameName, "");
                set => SetStringUnsafe(_obj, UsernameName, value);
            }

            private static ReadOnlySpan<byte> PasswordName => "rest.password"u8;

            /// <summary>
            /// The password to login to the REST server.
            /// </summary>
            /// <remarks>
            /// Will be ignored if <see cref="Token"/> is not empty.
            /// </remarks>
            /// <seealso cref="Username"/>
            public string Password
            {
                get => GetStringUnsafe(_obj, PasswordName, "");
                set => SetStringUnsafe(_obj, PasswordName, value);
            }

            private static ReadOnlySpan<byte> TokenName => "rest.token"u8;

            /// <summary>
            /// The API token to login to the REST server.
            /// </summary>
            /// <remarks>
            /// If not empty it will be used instead of <see cref="Username"/> and <see cref="Password"/>.
            /// </remarks>
            public string Token
            {
                get => GetStringUnsafe(_obj, TokenName, "");
                set => SetStringUnsafe(_obj, TokenName, value);
            }

            private static ReadOnlySpan<byte> ResubmitIncompleteName => "rest.resubmit_incomplete"u8;

            /// <summary>
            /// Whether to automatically resubmit incomplete queries returned by the server.
            /// </summary>
            public bool ResubmitIncomplete
            {
                get => GetBoolUnsafe(_obj, ResubmitIncompleteName, true);
                set => SetBoolUnsafe(_obj, ResubmitIncompleteName, value);
            }

            private static ReadOnlySpan<byte> DangerousIgnoreSslValidationName => "rest.ignore_ssl_validation"u8;

            /// <summary>
            /// Whether to skip validating the REST server's SSL certificate.
            /// </summary>
            /// <remarks>
            /// Setting this property to <see langword="true"/> will make the communication
            /// channel between the client and the server vulnerable to man-in-the-middle attacks.
            /// </remarks>
            public bool DangerousIgnoreSslValidation
            {
                get => GetBoolUnsafe(_obj, DangerousIgnoreSslValidationName, false);
                set => SetBoolUnsafe(_obj, DangerousIgnoreSslValidationName, value);
            }

            private static ReadOnlySpan<byte> CreationAccessCredentialsNameName => "rest.creation_access_credentials_name"u8;

            /// <summary>
            /// The name of the registered access key to use for creation of the REST server.
            /// </summary>
            public string CreationAccessCredentialsName
            {
                get => GetStringUnsafe(_obj, CreationAccessCredentialsNameName, "");
                set => SetStringUnsafe(_obj, CreationAccessCredentialsNameName, value);
            }

            private static ReadOnlySpan<byte> RetryHttpCodesName => "rest.retry_http_codes"u8;

            /// <summary>
            /// A comma-separated list of HTTP status codes that
            /// </summary>
            // TODO: Make it work with integer collections.
            public string RetryHttpCodes
            {
                get => GetStringUnsafe(_obj, RetryHttpCodesName, "503");
                set => SetStringUnsafe(_obj, RetryHttpCodesName, value);
            }

            private static ReadOnlySpan<byte> RetryCountName => "rest.retry_count"u8;

            /// <summary>
            /// The number of times to retry a failed REST requests
            /// </summary>
            public uint RetryCount
            {
                get => GetUInt32Unsafe(_obj, RetryCountName, 3);
                set => SetUInt32Unsafe(_obj, RetryCountName, value);
            }

            private static ReadOnlySpan<byte> RetryInitialDelayName => "rest.retry_initial_delay_ms"u8;

            /// <summary>
            /// The initial delay until retrying a failed REST request.
            /// </summary>
            /// <remarks>
            /// The value is rounded down to the nearest millisecond and clamped between 0 and <see cref="uint.MaxValue"/>.
            /// </remarks>
            public TimeSpan RetryInitialDelay
            {
                get => TimeSpan.FromMilliseconds(GetUInt32Unsafe(_obj, RetryInitialDelayName, 500));
                set
                {
                    uint ms = Math.Truncate(value.TotalMilliseconds) switch
                    {
                        < 0 => 0,
                        > uint.MaxValue => uint.MaxValue,
                        double x => (uint)x
                    };

                    SetUInt32Unsafe(_obj, RetryInitialDelayName, ms);
                }
            }

            private static ReadOnlySpan<byte> RetryDelayFactorName => "rest.retry_delay_factor"u8;

            /// <summary>
            /// The delay factor to exponentially wait until further retries of a failed REST request
            /// </summary>
            public float RetryDelayFactor
            {
                get => GetSingleUnsafe(_obj, RetryDelayFactorName, 1.25f);
                set => SetSingleUnsafe(_obj, RetryDelayFactorName, value);
            }

            private static ReadOnlySpan<byte> LoadMetadataOnArrayOpenName => "rest.load_metadata_on_array_open"u8;

            /// <summary>
            /// Whether to load and send array metadata together with the open array.
            /// </summary>
            public bool LoadMetadataOnArrayOpen
            {
                get => GetBoolUnsafe(_obj, LoadMetadataOnArrayOpenName, true);
                set => SetBoolUnsafe(_obj, LoadMetadataOnArrayOpenName, value);
            }

            private static ReadOnlySpan<byte> LoadNonEmptyDomainOnArrayOpenName => "rest.load_non_empty_domain_on_array_open"u8;

            /// <summary>
            /// Whether to load and send non-empty domain information together with the open array.
            /// </summary>
            public bool LoadNonEmptyDomainOnArrayOpen
            {
                get => GetBoolUnsafe(_obj, LoadNonEmptyDomainOnArrayOpenName, true);
                set => SetBoolUnsafe(_obj, LoadNonEmptyDomainOnArrayOpenName, value);
            }

            private static ReadOnlySpan<byte> UseRefactoredArrayOpenName => "rest.use_refactored_array_open"u8;

            /// <summary>
            /// Whether to use the new, experimental REST routes and APIs for opening an array.
            /// </summary>
            public bool UseRefactoredArrayOpen
            {
                get => GetBoolUnsafe(_obj, UseRefactoredArrayOpenName, false);
                set => SetBoolUnsafe(_obj, UseRefactoredArrayOpenName, value);
            }
        }
    }
}
