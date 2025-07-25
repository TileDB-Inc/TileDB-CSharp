using System;
using TileDB.CSharp.Marshalling.SafeHandles;
using TileDB.Interop;

namespace TileDB.CSharp;

/// <summary>
/// Represents a TileDB array schema evolution.
/// </summary>
public sealed unsafe class ArraySchemaEvolution : IDisposable
{
    private readonly Context _ctx;
    private readonly ArraySchemaEvolutionHandle _handle;

    /// <summary>
    /// Creates an <see cref="ArraySchemaEvolution"/>
    /// </summary>
    /// <param name="ctx">The <see cref="CSharp.Context"/> associated with this array schema evolution.</param>
    public ArraySchemaEvolution(Context ctx)
    {
        _ctx = ctx;
        _handle = ArraySchemaEvolutionHandle.Create(_ctx);
    }

    /// <summary>
    /// Disposes this <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    public void Dispose()
    {
        _handle.Dispose();
    }

    internal ArraySchemaEvolutionHandle Handle => _handle;

    /// <summary>
    /// Get context.
    /// </summary>
    /// <returns></returns>
    public Context Context() => _ctx;

    /// <summary>
    /// Adds an <see cref="Attribute"/> to the <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="attr">A fully constructed <see cref="Attribute"/> that will be added to the schema.</param>
    public void AddAttribute(Attribute attr)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var attrHandle = attr.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_evolution_add_attribute(ctxHandle, handle, attrHandle));
    }

    /// <summary>
    /// Drops an attribute with the given name from the <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="attrName">The name of the attribute that will be dropped from the schema.</param>
    public void DropAttribute(string attrName)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var msAttrName = new MarshaledString(attrName);
        _ctx.handle_error(Methods.tiledb_array_schema_evolution_drop_attribute(ctxHandle, handle, msAttrName));
    }

    /// <summary>
    /// Adds an <see cref="Enumeration"/> to the <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="enumeration">A fully constructed <see cref="Enumeration"/> that will be added to the schema.</param>
    /// <seealso cref="DropEnumeration"/>
    public void AddEnumeration(Enumeration enumeration)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var enumHandle = enumeration.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_evolution_add_enumeration(ctxHandle, handle, enumHandle));
    }

    /// <summary>
    /// Adds an <see cref="Enumeration"/> to the <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="enumeration">A fully constructed <see cref="Enumeration"/> that will be added to the schema.</param>
    /// <seealso cref="DropEnumeration"/>
    public void ExtendEnumeration(Enumeration enumeration)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var enumHandle = enumeration.Handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_schema_evolution_extend_enumeration(ctxHandle, handle, enumHandle));
    }

    /// <summary>
    /// Drops an enumeration with the given name from the <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="enumerationName">The name of the attribute that will be dropped from the schema.</param>
    /// <seealso cref="AddEnumeration"/>
    public void DropEnumeration(string enumerationName)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        using var msEnumerationName = new MarshaledString(enumerationName);
        _ctx.handle_error(Methods.tiledb_array_schema_evolution_drop_enumeration(ctxHandle, handle, msEnumerationName));
    }

    /// <summary>
    /// Drops an <see cref="Attribute"/> from the <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="attr">The <see cref="Attribute"/> whose name will be dropped from the schema.</param>
    public void DropAttribute(Attribute attr) => DropAttribute(attr.Name());

    /// <summary>
    /// Sets the timestamp range of this <see cref="ArraySchemaEvolution"/>.
    /// </summary>
    /// <param name="high">High value of timestamp range.</param>
    /// <param name="low">Low value of timestamp range.</param>
    public void SetTimeStampRange(ulong high, ulong low)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var handle = _handle.Acquire();
        _ctx.handle_error(
            Methods.tiledb_array_schema_evolution_set_timestamp_range(ctxHandle, handle, low, high));
    }

    /// <summary>
    /// Applies the <see cref="ArraySchemaEvolution"/> to an existing array.
    /// </summary>
    /// <param name="uri">The array's URI.</param>
    public void EvolveArray(string uri)
    {
        using var ctxHandle = _ctx.Handle.Acquire();
        using var msUri = new MarshaledString(uri);
        using var handle = _handle.Acquire();
        _ctx.handle_error(Methods.tiledb_array_evolve(ctxHandle, msUri, handle));
    }
}
