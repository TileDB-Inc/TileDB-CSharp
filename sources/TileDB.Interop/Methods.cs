using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public static unsafe partial class Methods
    {
        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_type_to_str(tiledb_query_type_t query_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_query_type_t* query_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_type_to_str(tiledb_object_t object_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_object_t* object_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filesystem_to_str(tiledb_filesystem_t filesystem, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filesystem_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_filesystem_t* filesystem);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_datatype_to_str(tiledb_datatype_t datatype, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_datatype_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_datatype_t* datatype);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_type_to_str(tiledb_array_type_t array_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_array_type_t* array_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_layout_to_str(tiledb_layout_t layout, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_layout_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_layout_t* layout);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_type_to_str(tiledb_filter_type_t filter_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_filter_type_t* filter_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_option_to_str(tiledb_filter_option_t filter_option, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_option_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_filter_option_t* filter_option);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_encryption_type_to_str(tiledb_encryption_type_t encryption_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_encryption_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_encryption_type_t* encryption_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_status_to_str(tiledb_query_status_t query_status, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_status_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_query_status_t* query_status);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_walk_order_to_str(tiledb_walk_order_t walk_order, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_walk_order_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_walk_order_t* walk_order);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_mode_to_str(tiledb_vfs_mode_t vfs_mode, [NativeTypeName("const char **")] sbyte** str);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_mode_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_vfs_mode_t* vfs_mode);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* tiledb_coords();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint32_t")]
        public static extern uint tiledb_var_num();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint32_t")]
        public static extern uint tiledb_max_path();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint64_t")]
        public static extern ulong tiledb_datatype_size(tiledb_datatype_t type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint64_t")]
        public static extern ulong tiledb_offset_size();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint64_t")]
        public static extern ulong tiledb_timestamp_now_ms();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_version([NativeTypeName("int32_t *")] int* major, [NativeTypeName("int32_t *")] int* minor, [NativeTypeName("int32_t *")] int* rev);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_error_message(tiledb_error_t* err, [NativeTypeName("const char **")] sbyte** errmsg);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_error_free(tiledb_error_t** err);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_alloc(tiledb_ctx_t* ctx, tiledb_buffer_t** buffer);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_buffer_free(tiledb_buffer_t** buffer);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_set_type(tiledb_ctx_t* ctx, tiledb_buffer_t* buffer, tiledb_datatype_t datatype);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_get_type(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_t *")] tiledb_buffer_t* buffer, tiledb_datatype_t* datatype);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_get_data(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_t *")] tiledb_buffer_t* buffer, void** data, [NativeTypeName("uint64_t *")] ulong* num_bytes);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_set_data(tiledb_ctx_t* ctx, tiledb_buffer_t* buffer, void* data, [NativeTypeName("uint64_t")] ulong size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_alloc(tiledb_ctx_t* ctx, tiledb_buffer_list_t** buffer_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_buffer_list_free(tiledb_buffer_list_t** buffer_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_get_num_buffers(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, [NativeTypeName("uint64_t *")] ulong* num_buffers);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_get_buffer(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, [NativeTypeName("uint64_t")] ulong buffer_idx, tiledb_buffer_t** buffer);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_get_total_size(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, [NativeTypeName("uint64_t *")] ulong* total_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_flatten(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, tiledb_buffer_t** buffer);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_alloc(tiledb_config_t** config, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_config_free(tiledb_config_t** config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_set(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* param1, [NativeTypeName("const char *")] sbyte* value, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_get(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* param1, [NativeTypeName("const char **")] sbyte** value, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_load_from_file(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* filename, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_unset(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* param1, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_save_to_file(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* filename, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_compare(tiledb_config_t* lhs, tiledb_config_t* rhs, [NativeTypeName("uint8_t *")] byte* equal);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_alloc(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* prefix, tiledb_config_iter_t** config_iter, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_reset(tiledb_config_t* config, tiledb_config_iter_t* config_iter, [NativeTypeName("const char *")] sbyte* prefix, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_config_iter_free(tiledb_config_iter_t** config_iter);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_here(tiledb_config_iter_t* config_iter, [NativeTypeName("const char **")] sbyte** param1, [NativeTypeName("const char **")] sbyte** value, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_next(tiledb_config_iter_t* config_iter, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_done(tiledb_config_iter_t* config_iter, [NativeTypeName("int32_t *")] int* done, tiledb_error_t** error);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_alloc(tiledb_config_t* config, tiledb_ctx_t** ctx);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_ctx_free(tiledb_ctx_t** ctx);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_get_stats(tiledb_ctx_t* ctx, [NativeTypeName("char **")] sbyte** stats_json);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_get_config(tiledb_ctx_t* ctx, tiledb_config_t** config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_get_last_error(tiledb_ctx_t* ctx, tiledb_error_t** err);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_is_supported_fs(tiledb_ctx_t* ctx, tiledb_filesystem_t fs, [NativeTypeName("int32_t *")] int* is_supported);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_cancel_tasks(tiledb_ctx_t* ctx);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_set_tag(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* key, [NativeTypeName("const char *")] sbyte* value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_create(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* group_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_alloc(tiledb_ctx_t* ctx, tiledb_filter_type_t type, tiledb_filter_t** filter);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_filter_free(tiledb_filter_t** filter);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_get_type(tiledb_ctx_t* ctx, tiledb_filter_t* filter, tiledb_filter_type_t* type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_set_option(tiledb_ctx_t* ctx, tiledb_filter_t* filter, tiledb_filter_option_t option, [NativeTypeName("const void *")] void* value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_get_option(tiledb_ctx_t* ctx, tiledb_filter_t* filter, tiledb_filter_option_t option, void* value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_list_alloc(tiledb_ctx_t* ctx, tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_filter_list_free(tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_list_add_filter(tiledb_ctx_t* ctx, tiledb_filter_list_t* filter_list, tiledb_filter_t* filter);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_list_set_max_chunk_size(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t")] uint max_chunk_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_list_get_nfilters(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t *")] uint* nfilters);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_list_get_filter_from_index(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t")] uint index, tiledb_filter_t** filter);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filter_list_get_max_chunk_size(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t *")] uint* max_chunk_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_alloc(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* name, tiledb_datatype_t type, tiledb_attribute_t** attr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_attribute_free(tiledb_attribute_t** attr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_nullable(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("uint8_t")] byte nullable);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_filter_list(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, tiledb_filter_list_t* filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_cell_val_num(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("uint32_t")] uint cell_val_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("const char **")] sbyte** name);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_type(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, tiledb_datatype_t* type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_nullable(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("uint8_t *")] byte* nullable);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_filter_list(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_cell_val_num(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("uint32_t *")] uint* cell_val_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_cell_size(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("uint64_t *")] ulong* cell_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_dump(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("FILE *")] __sFILE* @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_fill_value(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void *")] void* value, [NativeTypeName("uint64_t")] ulong size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_fill_value(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void **")] void** value, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_fill_value_nullable(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void *")] void* value, [NativeTypeName("uint64_t")] ulong size, [NativeTypeName("uint8_t")] byte validity);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_fill_value_nullable(tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void **")] void** value, [NativeTypeName("uint64_t *")] ulong* size, [NativeTypeName("uint8_t *")] byte* valid);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_alloc(tiledb_ctx_t* ctx, tiledb_domain_t** domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_domain_free(tiledb_domain_t** domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_type(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, tiledb_datatype_t* type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_ndim(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("uint32_t *")] uint* ndim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_add_dimension(tiledb_ctx_t* ctx, tiledb_domain_t* domain, tiledb_dimension_t* dim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_dimension_from_index(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("uint32_t")] uint index, tiledb_dimension_t** dim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_dimension_from_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("const char *")] sbyte* name, tiledb_dimension_t** dim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_has_dimension(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("int32_t *")] int* has_dim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_dump(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("FILE *")] __sFILE* @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_alloc(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* name, tiledb_datatype_t type, [NativeTypeName("const void *")] void* dim_domain, [NativeTypeName("const void *")] void* tile_extent, tiledb_dimension_t** dim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_dimension_free(tiledb_dimension_t** dim);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_set_filter_list(tiledb_ctx_t* ctx, tiledb_dimension_t* dim, tiledb_filter_list_t* filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_set_cell_val_num(tiledb_ctx_t* ctx, tiledb_dimension_t* dim, [NativeTypeName("uint32_t")] uint cell_val_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_filter_list(tiledb_ctx_t* ctx, tiledb_dimension_t* dim, tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_cell_val_num(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("uint32_t *")] uint* cell_val_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("const char **")] sbyte** name);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_type(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, tiledb_datatype_t* type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_domain(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("const void **")] void** domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_tile_extent(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("const void **")] void** tile_extent);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_dump(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("FILE *")] __sFILE* @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_alloc(tiledb_ctx_t* ctx, tiledb_array_type_t array_type, tiledb_array_schema_t** array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_array_schema_free(tiledb_array_schema_t** array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_add_attribute(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_attribute_t* attr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_allows_dups(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, int allows_dups);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_allows_dups(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, int* allows_dups);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_domain(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_domain_t* domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_capacity(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, [NativeTypeName("uint64_t")] ulong capacity);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_cell_order(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_layout_t cell_order);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_tile_order(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_layout_t tile_order);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_coords_filter_list(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t* filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_offsets_filter_list(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t* filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_validity_filter_list(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t* filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_check(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_load(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_array_schema_t** array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_load_with_key(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, tiledb_array_schema_t** array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_array_type(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_array_type_t* array_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_capacity(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("uint64_t *")] ulong* capacity);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_cell_order(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_layout_t* cell_order);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_coords_filter_list(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_offsets_filter_list(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_validity_filter_list(tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t** filter_list);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_domain(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_domain_t** domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_tile_order(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_layout_t* tile_order);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_attribute_num(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("uint32_t *")] uint* attribute_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_attribute_from_index(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("uint32_t")] uint index, tiledb_attribute_t** attr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_attribute_from_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("const char *")] sbyte* name, tiledb_attribute_t** attr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_has_attribute(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("int32_t *")] int* has_attr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_dump(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("FILE *")] __sFILE* @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_alloc(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, tiledb_query_t** query);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_stats(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("char **")] sbyte** stats_json);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_config(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_config(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_config_t** config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_subarray(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const void *")] void* subarray);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer_var(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* buffer_off, [NativeTypeName("uint64_t *")] ulong* buffer_off_size, void* buffer_val, [NativeTypeName("uint64_t *")] ulong* buffer_val_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer_nullable(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size, [NativeTypeName("uint8_t *")] byte* buffer_validity_bytemap, [NativeTypeName("uint64_t *")] ulong* buffer_validity_bytemap_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer_var_nullable(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* buffer_off, [NativeTypeName("uint64_t *")] ulong* buffer_off_size, void* buffer_val, [NativeTypeName("uint64_t *")] ulong* buffer_val_size, [NativeTypeName("uint8_t *")] byte* buffer_validity_bytemap, [NativeTypeName("uint64_t *")] ulong* buffer_validity_bytemap_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_data_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_offsets_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_validity_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint8_t *")] byte* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer_var(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t **")] ulong** buffer_off, [NativeTypeName("uint64_t **")] ulong** buffer_off_size, void** buffer_val, [NativeTypeName("uint64_t **")] ulong** buffer_val_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer_nullable(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size, [NativeTypeName("uint8_t **")] byte** buffer_validity_bytemap, [NativeTypeName("uint64_t **")] ulong** buffer_validity_bytemap_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer_var_nullable(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t **")] ulong** buffer_off, [NativeTypeName("uint64_t **")] ulong** buffer_off_size, void** buffer_val, [NativeTypeName("uint64_t **")] ulong** buffer_val_size, [NativeTypeName("uint8_t **")] byte** buffer_validity_bytemap, [NativeTypeName("uint64_t **")] ulong** buffer_validity_bytemap_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_data_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_offsets_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t **")] ulong** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_validity_buffer(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint8_t **")] byte** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_layout(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_layout_t layout);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_condition(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const tiledb_query_condition_t *")] tiledb_query_condition_t* cond);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_finalize(tiledb_ctx_t* ctx, tiledb_query_t* query);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_query_free(tiledb_query_t** query);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_submit(tiledb_ctx_t* ctx, tiledb_query_t* query);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_submit_async(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("void (*)(void *)")] delegate* unmanaged[Cdecl]<void*, void> callback, void* callback_data);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_has_results(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("int32_t *")] int* has_results);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_status(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_query_status_t* status);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_type(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_query_type_t* query_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_layout(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_layout_t* query_layout);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_array(tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_array_t** array);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("const void *")] void* end, [NativeTypeName("const void *")] void* stride);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range_by_name(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("const void *")] void* start, [NativeTypeName("const void *")] void* end, [NativeTypeName("const void *")] void* stride);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range_var(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong start_size, [NativeTypeName("const void *")] void* end, [NativeTypeName("uint64_t")] ulong end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range_var_by_name(tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong start_size, [NativeTypeName("const void *")] void* end, [NativeTypeName("uint64_t")] ulong end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_num(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t *")] ulong* range_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_num_from_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* range_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("const void **")] void** start, [NativeTypeName("const void **")] void** end, [NativeTypeName("const void **")] void** stride);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_from_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("const void **")] void** start, [NativeTypeName("const void **")] void** end, [NativeTypeName("const void **")] void** stride);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var_size(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var_size_from_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, void* start, void* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var_from_name(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, void* start, void* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size_var(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size_off, [NativeTypeName("uint64_t *")] ulong* size_val);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size_nullable(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size_val, [NativeTypeName("uint64_t *")] ulong* size_validity);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size_var_nullable(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size_off, [NativeTypeName("uint64_t *")] ulong* size_val, [NativeTypeName("uint64_t *")] ulong* size_validity);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_fragment_num(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t *")] uint* num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_fragment_uri(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint64_t")] ulong idx, [NativeTypeName("const char **")] sbyte** uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_fragment_timestamp_range(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint64_t")] ulong idx, [NativeTypeName("uint64_t *")] ulong* t1, [NativeTypeName("uint64_t *")] ulong* t2);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_condition_alloc(tiledb_ctx_t* ctx, tiledb_query_condition_t** cond);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_query_condition_free(tiledb_query_condition_t** cond);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_condition_init(tiledb_ctx_t* ctx, tiledb_query_condition_t* cond, [NativeTypeName("const char *")] sbyte* attribute_name, [NativeTypeName("const void *")] void* condition_value, [NativeTypeName("uint64_t")] ulong condition_value_size, tiledb_query_condition_op_t op);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_condition_combine(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_condition_t *")] tiledb_query_condition_t* left_cond, [NativeTypeName("const tiledb_query_condition_t *")] tiledb_query_condition_t* right_cond, tiledb_query_condition_combination_op_t combination_op, tiledb_query_condition_t** combined_cond);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_alloc(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_array_t** array);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_set_open_timestamp_start(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong timestamp_start);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_set_open_timestamp_end(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong timestamp_end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_open_timestamp_start(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* timestamp_start);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_open_timestamp_end(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* timestamp_end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open_at(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, [NativeTypeName("uint64_t")] ulong timestamp);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open_with_key(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open_at_with_key(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, [NativeTypeName("uint64_t")] ulong timestamp);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_is_open(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("int32_t *")] int* is_open);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_reopen(tiledb_ctx_t* ctx, tiledb_array_t* array);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_reopen_at(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong timestamp);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_timestamp(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* timestamp);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_set_config(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_config(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_config_t** config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_close(tiledb_ctx_t* ctx, tiledb_array_t* array);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_array_free(tiledb_array_t** array);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_schema(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_array_schema_t** array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_query_type(tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t* query_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_create(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_create_with_key(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_with_key(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_vacuum(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain(tiledb_ctx_t* ctx, tiledb_array_t* array, void* domain, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_from_index(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint32_t")] uint idx, void* domain, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_from_name(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* name, void* domain, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_size_from_index(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint32_t")] uint idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_size_from_name(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_from_index(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint32_t")] uint idx, void* start, void* end, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_from_name(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* name, void* start, void* end, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_uri(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char **")] sbyte** array_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_encryption_type(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t* encryption_type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_put_metadata(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t value_type, [NativeTypeName("uint32_t")] uint value_num, [NativeTypeName("const void *")] void* value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_delete_metadata(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_metadata(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_metadata_num(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_metadata_from_index(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong index, [NativeTypeName("const char **")] sbyte** key, [NativeTypeName("uint32_t *")] uint* key_len, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_has_metadata_key(tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("int32_t *")] int* has_key);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_metadata(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_metadata_with_key(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, tiledb_config_t* config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_type(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path, tiledb_object_t* type);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_remove(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_move(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* old_path, [NativeTypeName("const char *")] sbyte* new_path);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_walk(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path, tiledb_walk_order_t order, [NativeTypeName("int32_t (*)(const char *, tiledb_object_t, void *)")] delegate* unmanaged[Cdecl]<sbyte*, tiledb_object_t, void*, int> callback, void* data);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_ls(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path, [NativeTypeName("int32_t (*)(const char *, tiledb_object_t, void *)")] delegate* unmanaged[Cdecl]<sbyte*, tiledb_object_t, void*, int> callback, void* data);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_alloc(tiledb_ctx_t* ctx, tiledb_config_t* config, tiledb_vfs_t** vfs);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_vfs_free(tiledb_vfs_t** vfs);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_get_config(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, tiledb_config_t** config);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_create_bucket(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_remove_bucket(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_empty_bucket(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_empty_bucket(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_bucket(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_bucket);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_create_dir(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_dir(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_dir);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_remove_dir(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_file(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_file);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_remove_file(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_dir_size(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_file_size(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_move_file(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_move_dir(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_copy_file(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_copy_dir(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_open(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, tiledb_vfs_mode_t mode, tiledb_vfs_fh_t** fh);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_close(tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_read(tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh, [NativeTypeName("uint64_t")] ulong offset, void* buffer, [NativeTypeName("uint64_t")] ulong nbytes);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_write(tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh, [NativeTypeName("const void *")] void* buffer, [NativeTypeName("uint64_t")] ulong nbytes);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_sync(tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_ls(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* path, [NativeTypeName("int32_t (*)(const char *, void *)")] delegate* unmanaged[Cdecl]<sbyte*, void*, int> callback, void* data);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_vfs_fh_free(tiledb_vfs_fh_t** fh);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_fh_is_closed(tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh, [NativeTypeName("int32_t *")] int* is_closed);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_touch(tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_uri_to_path(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("char *")] sbyte* path_out, [NativeTypeName("uint32_t *")] uint* path_length);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_enable();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_disable();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_reset();

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_dump([NativeTypeName("FILE *")] __sFILE* @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_dump_str([NativeTypeName("char **")] sbyte** @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_raw_dump([NativeTypeName("FILE *")] __sFILE* @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_raw_dump_str([NativeTypeName("char **")] sbyte** @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_free_str([NativeTypeName("char **")] sbyte** @out);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_heap_profiler_enable([NativeTypeName("const char *")] sbyte* file_name_prefix, [NativeTypeName("uint64_t")] ulong dump_interval_ms, [NativeTypeName("uint64_t")] ulong dump_interval_bytes, [NativeTypeName("uint64_t")] ulong dump_threshold_bytes);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_alloc(tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_fragment_info_t** fragment_info);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_fragment_info_free(tiledb_fragment_info_t** fragment_info);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_load(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_load_with_key(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_num(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t *")] uint* fragment_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_uri(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_size(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_dense(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("int32_t *")] int* dense);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_sparse(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("int32_t *")] int* sparse);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_timestamp_range(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* start, [NativeTypeName("uint64_t *")] ulong* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_from_index(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint did, void* domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_from_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char *")] sbyte* dim_name, void* domain);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_size_from_index(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint did, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_size_from_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_from_index(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint did, void* start, void* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_from_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char *")] sbyte* dim_name, void* start, void* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_num(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* mbr_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_from_index(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("uint32_t")] uint did, void* mbr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_from_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("const char *")] sbyte* dim_name, void* mbr);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_size_from_index(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("uint32_t")] uint did, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_size_from_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_from_index(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("uint32_t")] uint did, void* start, void* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_from_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("const char *")] sbyte* dim_name, void* start, void* end);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_cell_num(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* cell_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_version(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t *")] uint* version);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_has_consolidated_metadata(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("int32_t *")] int* has);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_unconsolidated_metadata_num(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t *")] uint* unconsolidated);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_to_vacuum_num(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t *")] uint* to_vacuum_num);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_to_vacuum_uri(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** uri);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_array_schema(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, tiledb_array_schema_t** array_schema);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_array_schema_name(tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** schema_name);

        [DllImport(LibDllImport.Path, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_dump(tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_fragment_info_t *")] tiledb_fragment_info_t* fragment_info, [NativeTypeName("FILE *")] __sFILE* @out);
    }
}
