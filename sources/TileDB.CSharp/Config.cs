using System;
using System.Text;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB config object.
/// </summary>
public sealed unsafe class Config : IDisposable
{
    private readonly ConfigHandle _handle;

    /// <summary>
    /// Creates a <see cref="Config"/>.
    /// </summary>
    public Config()
    {
        _handle = ConfigHandle.Create();
    }

    internal Config(ConfigHandle handle)
    {
        _handle = handle;
    }

    /// <summary>
    /// Disposes this <see cref="Config"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal ConfigHandle Handle => _handle;

    /// <summary>
    /// Sets a parameter.
    /// </summary>
    /// <param name="param">The parameter's name.</param>
    /// <param name="value">The parameter's value.</param>
    /// <exception cref="ArgumentException"><paramref name="param"/> is <see langword="null"/> or empty.</exception>
    public void Set(string param, string value)
    {
        if (string.IsNullOrEmpty(param) || string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Config.set, param or value is null or empty!");
        }

        using var ms_param = new MarshaledString(param);
        using var ms_value = new MarshaledString(value);

        tiledb_error_t* p_tiledb_error;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_set(handle, ms_param, ms_value, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
    }

    /// <summary>
    /// Gets the value of a parameter.
    /// </summary>
    /// <param name="param">The parameter's name</param>
    /// <exception cref="ArgumentException"><paramref name="param"/> is <see langword="null"/> or empty.</exception>
    public string Get(string param)
    {
        if (string.IsNullOrEmpty(param))
        {
            throw new ArgumentException("Config.get, param or value is null or empty!");
        }

        using var ms_param = new MarshaledString(param);
        sbyte* result;

        tiledb_error_t* p_tiledb_error;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_get(handle, ms_param, &result, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);

        return MarshaledStringOut.GetStringFromNullTerminated(result);
    }

    /// <summary>
    /// Unsets a parameter.
    /// </summary>
    /// <param name="param">The parameter's name.</param>
    /// <exception cref="ArgumentException"><paramref name="param"/> is <see langword="null"/> or empty.</exception>
    public void Unset(string param)
    {
        if (string.IsNullOrEmpty(param))
        {
            throw new ArgumentException("Config.set, param is null or empty!");
        }

        using var ms_param = new MarshaledString(param);

        tiledb_error_t* p_tiledb_error;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_unset(handle, ms_param, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
    }

    /// <summary>
    /// Loads the options from a file.
    /// </summary>
    /// <param name="filename">The file's name.</param>
    /// <exception cref="ArgumentException"><paramref name="filename"/> is <see langword="null"/> or empty.</exception>
    public void LoadFromFile(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentException("Config.load_from_file, filename is null or empty!");
        }

        using var ms_filename = new MarshaledString(filename);

        tiledb_error_t* p_tiledb_error;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_load_from_file(handle, ms_filename, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
    }

    /// <summary>
    /// Saves the options from a file.
    /// </summary>
    /// <param name="filename">The file's name.</param>
    /// <exception cref="ArgumentException"><paramref name="filename"/> is <see langword="null"/> or empty.</exception>
    public void SaveToFile(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentException("Config.save_to_file, filename is null or empty!");
        }

        using var ms_filename = new MarshaledString(filename);

        tiledb_error_t* p_tiledb_error;
        int status;
        using (var handle = _handle.Acquire())
        {
            status = Methods.tiledb_config_save_to_file(handle, ms_filename, &p_tiledb_error);
        }
        ErrorHandling.CheckLastError(&p_tiledb_error, status);
    }

    /// <summary>
    /// Gets a <see cref="ConfigIterator"/> that enumerates over all options whose parameter namess start with a given prefix.
    /// </summary>
    /// <param name="prefix">The parameters' name's prefix.</param>
    public ConfigIterator Iterate(string prefix)
    {
        return new ConfigIterator(_handle, prefix);
    }

    /// <summary>
    /// Checks if two <see cref="Config"/>s have the same content.
    /// </summary>
    /// <param name="other">The config to compare this one with.</param>
    public bool Cmp(ref Config other)
    {
        using var handle = Handle.Acquire();
        using var otherHandle = other.Handle.Acquire();
        byte equal;
        Methods.tiledb_config_compare(handle, otherHandle, &equal);
        return equal == 1;
    }
}
