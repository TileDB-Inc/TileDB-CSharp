using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public static unsafe partial class Methods
    {
        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_alloc(tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t** array_schema_evolution);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_array_schema_evolution_free(tiledb_array_schema_evolution_t** array_schema_evolution);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_add_attribute(tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t* array_schema_evolution, tiledb_attribute_t* attribute);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_drop_attribute(tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t* array_schema_evolution, [NativeTypeName("const char *")] sbyte* attribute_name);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_set_timestamp_range(tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t* array_schema_evolution, [NativeTypeName("uint64_t")] ulong lo, [NativeTypeName("uint64_t")] ulong hi);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_timestamp_range(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, [NativeTypeName("uint64_t *")] ulong* lo, [NativeTypeName("uint64_t *")] ulong* hi);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_evolve(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_array_schema_evolution_t* array_schema_evolution);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_upgrade_version(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_add_point_ranges(tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong count);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_point_ranges(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong count);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_status_details(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_query_status_details_t* status);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_alloc_with_error(tiledb_config_t* config, tiledb_ctx_t** ctx, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_alloc(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* group_uri, tiledb_group_t** group);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_open(tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_query_type_t query_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_close(tiledb_ctx_t* ctx, tiledb_group_t* group);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_group_free(tiledb_group_t** group);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_set_config(tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_config(tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_config_t** config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_put_metadata(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t value_type, [NativeTypeName("uint32_t")] uint value_num, [NativeTypeName("const void *")] void* value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_delete_metadata(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_metadata(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_metadata_num(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t *")] ulong* num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_metadata_from_index(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t")] ulong index, [NativeTypeName("const char **")] sbyte** key, [NativeTypeName("uint32_t *")] uint* key_len, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_has_metadata_key(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("int32_t *")] int* has_key);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_add_member(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("const uint8_t")] byte relative);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_remove_member(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_member_count(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t *")] ulong* count);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_member_by_index(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t")] ulong index, [NativeTypeName("char **")] sbyte** uri, tiledb_object_t* type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_is_open(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("int32_t *")] int* is_open);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_uri(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char **")] sbyte** group_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_query_type(tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_query_type_t* query_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_dump_str(tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("char **")] sbyte** dump_ascii, int recursive);
    }
}
