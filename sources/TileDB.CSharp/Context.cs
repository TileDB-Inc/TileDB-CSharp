using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp
{
    /// <summary>
    /// Represents a TileDB context.
    /// </summary>
    public sealed unsafe class Context : IDisposable
    {
        private readonly ContextHandle _handle;

        private bool IsDefault { get; init; }

        /// <summary>
        /// Creates a <see cref="Context"/>.
        /// </summary>
        public Context()
        {
            _handle = ContextHandle.Create();
            SetDefaultTags();
        }

        /// <summary>
        /// Creates a <see cref="Context"/> with an associated <see cref="CSharp.Config"/>.
        /// </summary>
        /// <param name="config">The context's config.</param>
        public Context(Config config)
        {
            _handle = ContextHandle.Create(config.Handle);
            SetDefaultTags();
        }

        private void SetDefaultTags()
        {
            SetTag("x-tiledb-api-language", "csharp");
        }

        /// <summary>
        /// Disposes the <see cref="Context"/>.
        /// </summary>
        /// <remarks>
        /// Calling this method on the context returned by <see cref="GetDefault"/> will have no effect.
        /// </remarks>
        public void Dispose()
        {
            if (IsDefault)
            {
                return;
            }

            _handle.Dispose();
        }

        internal ContextHandle Handle => _handle;

        private static readonly Context _default = new() { IsDefault = true };

        /// <summary>
        /// Gets the default <see cref="Context"/>.
        /// </summary>
        public static Context GetDefault() => _default;

        /// <summary>
        /// Gets a JSON string with statistics about the context.
        /// </summary>
        public string Stats()
        {
            sbyte* result = null;
            try
            {
                using var handle = _handle.Acquire();
                ErrorHandling.ThrowOnError(Methods.tiledb_ctx_get_stats(handle, &result));

                return MarshaledStringOut.GetStringFromNullTerminated(result);
            }
            finally
            {
                if (result is not null)
                {
                    ErrorHandling.ThrowOnError(Methods.tiledb_stats_free_str(&result));
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="Context"/>'s <see cref="Config"/>.
        /// </summary>
        public Config Config()
        {
            var handle = new ConfigHandle();
            tiledb_config_t* config = null;
            var successful = false;
            try
            {
                using var ctxHandle = _handle.Acquire();
                handle_error(Methods.tiledb_ctx_get_config(ctxHandle, &config));
                successful = true;
            }
            finally
            {
                if (successful)
                {
                    handle.InitHandle(config);
                }
                else
                {
                    handle.SetHandleAsInvalid();
                }
            }
            return new Config(handle);
        }

        /// <summary>
        /// Gets the last error message associated with this <see cref="Context"/>.
        /// </summary>
        public string LastError()
        {
            var sb_result = new StringBuilder();

            tiledb_error_t* p_tiledb_error;
            int status;
            using (var handle = _handle.Acquire())
            {
                status = Methods.tiledb_ctx_get_last_error(handle, &p_tiledb_error);
            }
            if (status == (int)Status.TILEDB_OK)
            {
                sbyte* messagePtr;

                status = Methods.tiledb_error_message(p_tiledb_error, &messagePtr);

                if (status == (int)Status.TILEDB_OK)
                {
                    string message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                    sb_result.Append(message);
                }
                else
                {
                    var message = Enum.IsDefined(typeof(Status), status) ? ((Status)status).ToString() : ("Unknown error with code:" + status);
                    sb_result.Append(" Context.last_error,caught exception:" + message);
                }
            }
            else
            {
                var message = Enum.IsDefined(typeof(Status), status) ? ((Status)status).ToString() : ("Unknown error with code:" + status);
                sb_result.Append(" Context.last_error,caught exception:" + message);
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            return sb_result.ToString();
        }

        /// <summary>
        /// Cancels the asynchronous tasks associated with this <see cref="Context"/>.
        /// </summary>
        public void CancelTasks()
        {
            using var handle = _handle.Acquire();
            handle_error(Methods.tiledb_ctx_cancel_tasks(handle));
        }

        /// <summary>
        /// Checks whether the given <see cref="FileSystemType"/> is supported.
        /// </summary>
        /// <param name="fileSystem">The file system type to check.</param>
        public bool IsFileSystemSupported(FileSystemType fileSystem)
        {
            using var handle = _handle.Acquire();
            int result;
            handle_error(Methods.tiledb_ctx_is_supported_fs(handle, (tiledb_filesystem_t)fileSystem, &result));
            return result != 0;
        }

        /// <summary>
        /// Sets a string key-value “tag” on the given context.
        /// </summary>
        /// <param name="key">The tag's key.</param>
        /// <param name="value">The tag's value.</param>
        /// <exception cref="ArgumentException"><paramref name="key"/> or
        /// <paramref name="value"/> are <see langword="null"/> or empty.</exception>
        public void SetTag(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Context.set_tag, key or value is null or empty!");
            }

            using var handle = _handle.Acquire();
            using var ms_key = new MarshaledString(key);
            using var ms_value = new MarshaledString(value);
            handle_error(Methods.tiledb_ctx_set_tag(handle, ms_key, ms_value));
        }

        /// <summary>
        /// Returns the TileDB object type for a given resource path.
        /// </summary>
        /// <param name="uri">The path to check.</param>
        /// <returns>
        /// The type of the object in <paramref name="uri"/> or
        /// <see cref="ObjectType.Invalid"/> if it is not a TileDB object.</returns>
        public ObjectType GetObjectType(string uri)
        {
            using var handle = _handle.Acquire();
            using var ms_uri = new MarshaledString(uri);
            tiledb_object_t type;
            handle_error(Methods.tiledb_object_type(handle, ms_uri, &type));
            return (ObjectType)type;
        }

        /// <summary>
        /// Deletes a TileDB resource (group or array).
        /// </summary>
        /// <param name="uri">The resource's path.</param>
        public void RemoveObject(string uri)
        {
            using var handle = _handle.Acquire();
            using var ms_uri = new MarshaledString(uri);
            handle_error(Methods.tiledb_object_remove(handle, ms_uri));
        }

        /// <summary>
        /// Moves a TileDB resource (group or array).
        /// </summary>
        /// <param name="oldUri">The resource's old directory.</param>
        /// <param name="newUri">The resource's new directory.</param>
        public void MoveObject(string oldUri, string newUri)
        {
            using var handle = _handle.Acquire();
            using var ms_oldUri = new MarshaledString(oldUri);
            using var ms_newUri = new MarshaledString(newUri);
            handle_error(Methods.tiledb_object_move(handle, ms_oldUri, ms_newUri));
        }

        [UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
        private static int VisitCallback(sbyte* uriPtr, tiledb_object_t objectType, void* arg)
        {
            VisitCallbackData* data = (VisitCallbackData*)arg;
            Debug.Assert(data->Exception is null);
            bool shouldContinue;
            try
            {
                string uri = MarshaledStringOut.GetStringFromNullTerminated(uriPtr);
                shouldContinue = data->Invoke(uri, (ObjectType)objectType);
            }
            catch (Exception e)
            {
                data->Exception = ExceptionDispatchInfo.Capture(e);
                shouldContinue = false;
            }
            return shouldContinue ? 1 : 0;
        }

        /// <summary>
        /// Visits the TileDB objects contained in <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="callbackArg"/>.</typeparam>
        /// <param name="uri">The URI to check.</param>
        /// <param name="walkOrder">The order to recursively visit the objects.
        /// Setting this parameter to <see langword="null"/> will visit only
        /// the top-level objects of <paramref name="uri"/>.</param>
        /// <param name="callback">A callback that gets called for each TileDB object that got found.
        /// It accepts its URI, its <see cref="ObjectType"/> and <paramref name="callbackArg"/>.</param>
        /// <param name="callbackArg">An argument that will be passed to <paramref name="callback"/>.</param>
        /// <remarks>
        /// This method ignores any file system object that is not TileDB-related.
        /// </remarks>
        /// <seealso cref="GetChildObjects"/>
        public void VisitChildObjects<T>(string uri, WalkOrderType? walkOrder, Func<string, ObjectType, T, bool> callback, T callbackArg)
        {
            // See VFS.VisitChildren for comments on how it works.
            ValueTuple<Func<string, ObjectType, T, bool>, T> genericCallbackData = (callback, callbackArg);
            var callbackData = new VisitCallbackData()
            {
                Callback = static (uri, objectType, arg) =>
                {
                    var dataPtr = (ValueTuple<Func<string, ObjectType, T, bool>, T>*)arg;
                    return dataPtr->Item1(uri, objectType, dataPtr->Item2);
                },
                CallbackArgument = (IntPtr)(&genericCallbackData)
            };

            using var handle = _handle.Acquire();
            using var ms_uri = new MarshaledString(uri);
            switch (walkOrder)
            {
                case WalkOrderType order:
                    handle_error(Methods.tiledb_object_walk(handle, ms_uri, (tiledb_walk_order_t)order, &VisitCallback, &callbackData));
                    break;
                case null:
                    handle_error(Methods.tiledb_object_ls(handle, ms_uri, &VisitCallback, &callbackData));
                    break;
            }
            callbackData.Exception?.Throw();
        }

        /// <summary>
        /// Returns the TileDB objects contained in <paramref name="uri"/>.
        /// </summary>
        /// <param name="uri">The URI to check.</param>
        /// <param name="walkOrder">The order to recursively visit the objects.
        /// Setting this parameter to <see langword="null"/> will visit only
        /// the top-level objects of <paramref name="uri"/>.</param>
        /// <remarks>
        /// This method ignores any file system object that is not TileDB-related.
        /// </remarks>
        /// <returns>
        /// A <see cref="List{T}"/> with the URIs of each TileDB object in <paramref name="uri"/>,
        /// along with its <see cref="ObjectType"/>.
        /// </returns>
        public List<(string Uri, ObjectType ObjectType)> GetChildObjects(string uri, WalkOrderType? walkOrder)
        {
            var list = new List<(string Uri, ObjectType ObjectType)>();
            VisitChildObjects(uri, walkOrder, static (uri, objectType, list) =>
            {
                list.Add((uri, objectType));
                return true;
            }, list);
            return list;
        }

        internal void handle_error(int rc)
        {
            var status = (Status)Methods.tiledb_status(rc);

            if (status == Status.TILEDB_OK)
            {
                return;
            }

            string message;
            tiledb_error_t* p_tiledb_error;
            using (var handle = _handle.Acquire())
            {
                status = (Status)Methods.tiledb_status(Methods.tiledb_ctx_get_last_error(handle, &p_tiledb_error));
            }
            if (status == Status.TILEDB_OK)
            {
                sbyte* messagePtr;
                status = (Status)Methods.tiledb_status(Methods.tiledb_error_message(p_tiledb_error, &messagePtr));

                if (status == Status.TILEDB_OK)
                {
                    message = MarshaledStringOut.GetStringFromNullTerminated(messagePtr);
                }
                else
                {
                    message = $"Error during tiledb_error_message: {status}";
                }
            }
            else
            {
                message = $"Error during tiledb_ctx_get_last_error: {status}";
            }
            Methods.tiledb_error_free(&p_tiledb_error);

            throw new TileDBException(message) { StatusCode = (int)status };
        }

        private struct VisitCallbackData
        {
            public Func<string, ObjectType, IntPtr, bool> Callback;

            public IntPtr CallbackArgument;

            public ExceptionDispatchInfo? Exception;

            public bool Invoke(string uri, ObjectType objectType) => Callback(uri, objectType, CallbackArgument);
        }
    }
}
