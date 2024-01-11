using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

public sealed unsafe class ConfigIterator : IDisposable
{
    private readonly ConfigIteratorHandle _handle;
    private readonly ConfigHandle _hConfig;
    private bool _disposed;

    internal ConfigIterator(ConfigHandle hConfig, string prefix)
    {
        _hConfig = hConfig;
        _handle = ConfigIteratorHandle.Create(hConfig, prefix);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing && (!_handle.IsInvalid))
        {
            _handle.Dispose();
        }

        _disposed = true;
    }

    internal ConfigIteratorHandle Handle => _handle;

    public Tuple<string, string> Here()
    {
        tiledb_error_t* p_tiledb_error;
        sbyte* paramPtr;
        sbyte* valuePtr;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_iter_here(handle, &paramPtr, &valuePtr, &p_tiledb_error);
        }

        ErrorHandling.CheckLastError(&p_tiledb_error, status);

        string param = MarshaledStringOut.GetStringFromNullTerminated(paramPtr);
        string value = MarshaledStringOut.GetStringFromNullTerminated(valuePtr);
        return new Tuple<string, string>(param, value);
    }

    public void Next()
    {
        tiledb_error_t* p_tiledb_error;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_iter_next(handle, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
    }

    public bool Done()
    {
        tiledb_error_t* p_tiledb_error;
        int c_done;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_iter_done(handle, &c_done, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
        return c_done == 1;
    }

    public void Reset(string prefix)
    {
        tiledb_error_t* p_tiledb_error;
        using var ms_prefix = new MarshaledString(prefix);
        int status;
        using (var configHandle = _hConfig.Acquire())
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_iter_reset(configHandle, handle, ms_prefix, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
    }
}
