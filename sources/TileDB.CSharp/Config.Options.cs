using System;
using System.Diagnostics;

namespace TileDB.CSharp
{
    partial class Config
    {
        private static ReadOnlySpan<byte> EnvironmentVariablePrefixName => "config.env_var_prefix"u8;

        /// <summary>
        /// Prefix of environmental variables for reading configuration parameters.
        /// </summary>
        public string EnvironmentVariablePrefix
        {
            get => GetStringUnsafe(this, EnvironmentVariablePrefixName);
            set => SetStringUnsafe(this, EnvironmentVariablePrefixName, value);
        }

        private static ReadOnlySpan<byte> LoggingLevelName => "config.logging_level"u8;

        /// <summary>
        /// The logging level configured, possible values: <c>0</c>: fatal,
        /// <c>1</c>: error, <c>2</c>: warn, <c>3</c>: info <c>4</c>: debug, <c>5</c>: trace
        /// </summary>
        public uint LoggingLevel
        {
            get => GetUInt32Unsafe(this, LoggingLevelName);
            set => SetUInt32Unsafe(this, LoggingLevelName, value);
        }

        private static ReadOnlySpan<byte> LoggingFormatName => "config.logging_format"u8;

        /// <summary>
        /// The logs' format.
        /// </summary>
        public LoggingFormat LoggingFormat
        {
            get
            {
                ReadOnlySpan<byte> value = GetUnsafe(this, LoggingFormatName);

                if (value.IsEmpty || value.SequenceEqual("JSON"u8))
                {
                    return LoggingFormat.Json;
                }

                Debug.Assert(value.SequenceEqual("DEFAULT"u8), "Invalid logging format value");
                return LoggingFormat.Default;
            }
            set
            {
                ReadOnlySpan<byte> valueString = value switch
                {
                    LoggingFormat.Default => "DEFAULT"u8,
                    LoggingFormat.Json => "JSON"u8,
                    _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Invalid logging format value"),
                };
                SetUnsafe(this, LoggingFormatName, valueString);
            }
        }
    }
}
