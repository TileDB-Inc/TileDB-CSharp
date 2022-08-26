using System.Runtime.InteropServices;

namespace TileDB.Interop
{
    public static unsafe partial class Methods
    {
        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_type_to_str(tiledb_query_type_t query_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_query_type_t* query_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_type_to_str(tiledb_object_t object_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_object_t* object_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filesystem_to_str(tiledb_filesystem_t filesystem, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filesystem_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_filesystem_t* filesystem);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_datatype_to_str(tiledb_datatype_t datatype, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_datatype_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_datatype_t* datatype);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_type_to_str(tiledb_array_type_t array_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_array_type_t* array_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_layout_to_str(tiledb_layout_t layout, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_layout_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_layout_t* layout);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_encryption_type_to_str(tiledb_encryption_type_t encryption_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_encryption_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_encryption_type_t* encryption_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_status_to_str(tiledb_query_status_t query_status, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_status_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_query_status_t* query_status);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_walk_order_to_str(tiledb_walk_order_t walk_order, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_walk_order_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_walk_order_t* walk_order);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_mode_to_str(tiledb_vfs_mode_t vfs_mode, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_mode_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_vfs_mode_t* vfs_mode);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* tiledb_coords();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint32_t")]
        public static extern uint tiledb_var_num();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint32_t")]
        public static extern uint tiledb_max_path();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint64_t")]
        public static extern ulong tiledb_offset_size();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint64_t")]
        public static extern ulong tiledb_datatype_size(tiledb_datatype_t type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("uint64_t")]
        public static extern ulong tiledb_timestamp_now_ms();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* tiledb_timestamps();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_version([NativeTypeName("int32_t *")] int* major, [NativeTypeName("int32_t *")] int* minor, [NativeTypeName("int32_t *")] int* rev);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_error_message(tiledb_error_t* err, [NativeTypeName("const char **")] sbyte** errmsg);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_error_free(tiledb_error_t** err);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_buffer_t** buffer);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_buffer_free(tiledb_buffer_t** buffer);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_set_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_buffer_t* buffer, tiledb_datatype_t datatype);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_get_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_t *")] tiledb_buffer_t* buffer, tiledb_datatype_t* datatype);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_get_data([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_t *")] tiledb_buffer_t* buffer, void** data, [NativeTypeName("uint64_t *")] ulong* num_bytes);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_set_data([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_buffer_t* buffer, void* data, [NativeTypeName("uint64_t")] ulong size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_buffer_list_t** buffer_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_buffer_list_free(tiledb_buffer_list_t** buffer_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_get_num_buffers([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, [NativeTypeName("uint64_t *")] ulong* num_buffers);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_get_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, [NativeTypeName("uint64_t")] ulong buffer_idx, tiledb_buffer_t** buffer);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_get_total_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, [NativeTypeName("uint64_t *")] ulong* total_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_buffer_list_flatten([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_buffer_list_t *")] tiledb_buffer_list_t* buffer_list, tiledb_buffer_t** buffer);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_alloc(tiledb_config_t** config, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_config_free(tiledb_config_t** config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_set(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* param1, [NativeTypeName("const char *")] sbyte* value, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_get(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* param1, [NativeTypeName("const char **")] sbyte** value, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_unset(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* param1, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_load_from_file(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* filename, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_save_to_file(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* filename, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_compare(tiledb_config_t* lhs, tiledb_config_t* rhs, [NativeTypeName("uint8_t *")] byte* equal);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_alloc(tiledb_config_t* config, [NativeTypeName("const char *")] sbyte* prefix, tiledb_config_iter_t** config_iter, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_reset(tiledb_config_t* config, tiledb_config_iter_t* config_iter, [NativeTypeName("const char *")] sbyte* prefix, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_config_iter_free(tiledb_config_iter_t** config_iter);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_here(tiledb_config_iter_t* config_iter, [NativeTypeName("const char **")] sbyte** param1, [NativeTypeName("const char **")] sbyte** value, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_next(tiledb_config_iter_t* config_iter, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_config_iter_done(tiledb_config_iter_t* config_iter, [NativeTypeName("int32_t *")] int* done, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_alloc(tiledb_config_t* config, [NativeTypeName("tiledb_ctx_t **")] tiledb_ctx_t** ctx);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_ctx_free([NativeTypeName("tiledb_ctx_t **")] tiledb_ctx_t** ctx);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_get_stats([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("char **")] sbyte** stats_json);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_get_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_config_t** config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_get_last_error([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_error_t** err);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_is_supported_fs([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_filesystem_t fs, [NativeTypeName("int32_t *")] int* is_supported);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_cancel_tasks([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_set_tag([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* key, [NativeTypeName("const char *")] sbyte* value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_create([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* group_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("capi_return_t")]
        public static extern int tiledb_filter_list_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_filter_list_free(tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("capi_return_t")]
        public static extern int tiledb_filter_list_add_filter([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_filter_list_t* filter_list, [NativeTypeName("tiledb_filter_t *")] tiledb_filter_t* filter);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("capi_return_t")]
        public static extern int tiledb_filter_list_set_max_chunk_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t")] uint max_chunk_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("capi_return_t")]
        public static extern int tiledb_filter_list_get_nfilters([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t *")] uint* nfilters);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("capi_return_t")]
        public static extern int tiledb_filter_list_get_filter_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t")] uint index, [NativeTypeName("tiledb_filter_t **")] tiledb_filter_t** filter);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("capi_return_t")]
        public static extern int tiledb_filter_list_get_max_chunk_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_filter_list_t *")] tiledb_filter_list_t* filter_list, [NativeTypeName("uint32_t *")] uint* max_chunk_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* name, tiledb_datatype_t type, tiledb_attribute_t** attr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_attribute_free(tiledb_attribute_t** attr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("uint8_t")] byte nullable);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, tiledb_filter_list_t* filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_cell_val_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("uint32_t")] uint cell_val_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("const char **")] sbyte** name);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, tiledb_datatype_t* type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("uint8_t *")] byte* nullable);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_cell_val_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("uint32_t *")] uint* cell_val_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_cell_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("uint64_t *")] ulong* cell_size);

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_attribute_dump([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_attribute_t *")] tiledb_attribute_t* attr, [NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_fill_value([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void *")] void* value, [NativeTypeName("uint64_t")] ulong size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_fill_value([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void **")] void** value, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_set_fill_value_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void *")] void* value, [NativeTypeName("uint64_t")] ulong size, [NativeTypeName("uint8_t")] byte validity);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_attribute_get_fill_value_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_attribute_t* attr, [NativeTypeName("const void **")] void** value, [NativeTypeName("uint64_t *")] ulong* size, [NativeTypeName("uint8_t *")] byte* valid);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_domain_t** domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_domain_free(tiledb_domain_t** domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, tiledb_datatype_t* type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_ndim([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("uint32_t *")] uint* ndim);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_add_dimension([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_domain_t* domain, tiledb_dimension_t* dim);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_dimension_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("uint32_t")] uint index, tiledb_dimension_t** dim);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_get_dimension_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("const char *")] sbyte* name, tiledb_dimension_t** dim);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_domain_has_dimension([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("int32_t *")] int* has_dim);

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_domain_dump([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_domain_t *")] tiledb_domain_t* domain, [NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* name, tiledb_datatype_t type, [NativeTypeName("const void *")] void* dim_domain, [NativeTypeName("const void *")] void* tile_extent, tiledb_dimension_t** dim);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_dimension_free(tiledb_dimension_t** dim);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_set_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_dimension_t* dim, tiledb_filter_list_t* filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_set_cell_val_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_dimension_t* dim, [NativeTypeName("uint32_t")] uint cell_val_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_dimension_t* dim, tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_cell_val_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("uint32_t *")] uint* cell_val_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("const char **")] sbyte** name);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, tiledb_datatype_t* type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_domain([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("const void **")] void** domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_dimension_get_tile_extent([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("const void **")] void** tile_extent);

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_dimension_dump([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_dimension_t *")] tiledb_dimension_t* dim, [NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_type_t array_type, tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_array_schema_free(tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_add_attribute([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_attribute_t* attr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_allows_dups([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, int allows_dups);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_allows_dups([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, int* allows_dups);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_version([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, [NativeTypeName("uint32_t *")] uint* version);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_domain([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_domain_t* domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_capacity([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, [NativeTypeName("uint64_t")] ulong capacity);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_cell_order([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_layout_t cell_order);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_tile_order([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_layout_t tile_order);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_coords_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t* filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_offsets_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t* filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_set_validity_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t* filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_check([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_load([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_load_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_array_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_array_type_t* array_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_capacity([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("uint64_t *")] ulong* capacity);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_cell_order([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_layout_t* cell_order);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_coords_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_offsets_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_validity_filter_list([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, tiledb_filter_list_t** filter_list);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_domain([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_domain_t** domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_tile_order([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_layout_t* tile_order);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_attribute_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("uint32_t *")] uint* attribute_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_attribute_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("uint32_t")] uint index, tiledb_attribute_t** attr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_get_attribute_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("const char *")] sbyte* name, tiledb_attribute_t** attr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_has_attribute([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("int32_t *")] int* has_attr);

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_array_schema_dump([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, [NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, tiledb_query_t** query);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_stats([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("char **")] sbyte** stats_json);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_config_t** config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_subarray([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const void *")] void* subarray);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_subarray_t([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* buffer_off, [NativeTypeName("uint64_t *")] ulong* buffer_off_size, void* buffer_val, [NativeTypeName("uint64_t *")] ulong* buffer_val_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size, [NativeTypeName("uint8_t *")] byte* buffer_validity_bytemap, [NativeTypeName("uint64_t *")] ulong* buffer_validity_bytemap_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_buffer_var_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* buffer_off, [NativeTypeName("uint64_t *")] ulong* buffer_off_size, void* buffer_val, [NativeTypeName("uint64_t *")] ulong* buffer_val_size, [NativeTypeName("uint8_t *")] byte* buffer_validity_bytemap, [NativeTypeName("uint64_t *")] ulong* buffer_validity_bytemap_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_data_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_offsets_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_validity_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint8_t *")] byte* buffer, [NativeTypeName("uint64_t *")] ulong* buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t **")] ulong** buffer_off, [NativeTypeName("uint64_t **")] ulong** buffer_off_size, void** buffer_val, [NativeTypeName("uint64_t **")] ulong** buffer_val_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size, [NativeTypeName("uint8_t **")] byte** buffer_validity_bytemap, [NativeTypeName("uint64_t **")] ulong** buffer_validity_bytemap_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_buffer_var_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t **")] ulong** buffer_off, [NativeTypeName("uint64_t **")] ulong** buffer_off_size, void** buffer_val, [NativeTypeName("uint64_t **")] ulong** buffer_val_size, [NativeTypeName("uint8_t **")] byte** buffer_validity_bytemap, [NativeTypeName("uint64_t **")] ulong** buffer_validity_bytemap_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_data_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, void** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_offsets_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t **")] ulong** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_validity_buffer([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint8_t **")] byte** buffer, [NativeTypeName("uint64_t **")] ulong** buffer_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_layout([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_layout_t layout);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_set_condition([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const tiledb_query_condition_t *")] tiledb_query_condition_t* cond);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_finalize([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_query_free(tiledb_query_t** query);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_submit([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_submit_async([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("void (*)(void *)")] delegate* unmanaged[Cdecl]<void*, void> callback, void* callback_data);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_has_results([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("int32_t *")] int* has_results);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_status([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_query_status_t* status);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_query_type_t* query_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_layout([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_layout_t* query_layout);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_array([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, tiledb_array_t** array);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("const void *")] void* end, [NativeTypeName("const void *")] void* stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range_by_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("const void *")] void* start, [NativeTypeName("const void *")] void* end, [NativeTypeName("const void *")] void* stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong start_size, [NativeTypeName("const void *")] void* end, [NativeTypeName("uint64_t")] ulong end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_range_var_by_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong start_size, [NativeTypeName("const void *")] void* end, [NativeTypeName("uint64_t")] ulong end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t *")] ulong* range_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_num_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* range_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("const void **")] void** start, [NativeTypeName("const void **")] void** end, [NativeTypeName("const void **")] void** stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("const void **")] void** start, [NativeTypeName("const void **")] void** end, [NativeTypeName("const void **")] void** stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var_size_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_range_var_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size_off, [NativeTypeName("uint64_t *")] ulong* size_val);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size_val, [NativeTypeName("uint64_t *")] ulong* size_validity);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_est_result_size_var_nullable([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* size_off, [NativeTypeName("uint64_t *")] ulong* size_val, [NativeTypeName("uint64_t *")] ulong* size_validity);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_fragment_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint32_t *")] uint* num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_fragment_uri([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint64_t")] ulong idx, [NativeTypeName("const char **")] sbyte** uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_fragment_timestamp_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint64_t")] ulong idx, [NativeTypeName("uint64_t *")] ulong* t1, [NativeTypeName("uint64_t *")] ulong* t2);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_subarray_t([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, tiledb_subarray_t** subarray);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_condition_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_condition_t** cond);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_query_condition_free(tiledb_query_condition_t** cond);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_condition_init([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_condition_t* cond, [NativeTypeName("const char *")] sbyte* attribute_name, [NativeTypeName("const void *")] void* condition_value, [NativeTypeName("uint64_t")] ulong condition_value_size, tiledb_query_condition_op_t op);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_condition_combine([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_condition_t *")] tiledb_query_condition_t* left_cond, [NativeTypeName("const tiledb_query_condition_t *")] tiledb_query_condition_t* right_cond, tiledb_query_condition_combination_op_t combination_op, tiledb_query_condition_t** combined_cond);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_array_t *")] tiledb_array_t* array, tiledb_subarray_t** subarray);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_set_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_subarray_free(tiledb_subarray_t** subarray);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_set_coalesce_ranges([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, int coalesce_ranges);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_set_subarray([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray_s, [NativeTypeName("const void *")] void* subarray_v);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_add_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("const void *")] void* end, [NativeTypeName("const void *")] void* stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_add_range_by_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("const void *")] void* start, [NativeTypeName("const void *")] void* end, [NativeTypeName("const void *")] void* stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_add_range_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong start_size, [NativeTypeName("const void *")] void* end, [NativeTypeName("uint64_t")] ulong end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_add_range_var_by_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong start_size, [NativeTypeName("const void *")] void* end, [NativeTypeName("uint64_t")] ulong end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t *")] ulong* range_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_num_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* range_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("const void **")] void** start, [NativeTypeName("const void **")] void** end, [NativeTypeName("const void **")] void** stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("const void **")] void** start, [NativeTypeName("const void **")] void** end, [NativeTypeName("const void **")] void** stride);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_var_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_var_size_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_var([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("uint64_t")] ulong range_idx, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_get_range_var_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_subarray_t *")] tiledb_subarray_t* subarray, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t")] ulong range_idx, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_array_t** array);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_set_open_timestamp_start([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong timestamp_start);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_set_open_timestamp_end([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong timestamp_end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_open_timestamp_start([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* timestamp_start);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_open_timestamp_end([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* timestamp_end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_delete_fragments([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("uint64_t")] ulong timestamp_start, [NativeTypeName("uint64_t")] ulong timestamp_end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open_at([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, [NativeTypeName("uint64_t")] ulong timestamp);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_open_at_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t query_type, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, [NativeTypeName("uint64_t")] ulong timestamp);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_is_open([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("int32_t *")] int* is_open);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_reopen([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_reopen_at([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong timestamp);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_timestamp([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* timestamp);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_set_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_config_t** config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_close([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_array_free(tiledb_array_t** array);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_schema([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_query_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, tiledb_query_type_t* query_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_create([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_create_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, [NativeTypeName("const tiledb_array_schema_t *")] tiledb_array_schema_t* array_schema, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_vacuum([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, void* domain, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint32_t")] uint idx, void* domain, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* name, void* domain, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_size_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint32_t")] uint idx, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_size_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint32_t")] uint idx, void* start, void* end, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_non_empty_domain_var_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* name, void* start, void* end, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_uri([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char **")] sbyte** array_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_encryption_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t* encryption_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_put_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t value_type, [NativeTypeName("uint32_t")] uint value_num, [NativeTypeName("const void *")] void* value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_delete_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_metadata_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t *")] ulong* num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_get_metadata_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("uint64_t")] ulong index, [NativeTypeName("const char **")] sbyte** key, [NativeTypeName("uint32_t *")] uint* key_len, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_has_metadata_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_t* array, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("int32_t *")] int* has_key);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_metadata_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path, tiledb_object_t* type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_remove([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_move([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* old_path, [NativeTypeName("const char *")] sbyte* new_path);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_walk([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path, tiledb_walk_order_t order, [NativeTypeName("int32_t (*)(const char *, tiledb_object_t, void *)")] delegate* unmanaged[Cdecl]<sbyte*, tiledb_object_t, void*, int> callback, void* data);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_object_ls([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* path, [NativeTypeName("int32_t (*)(const char *, tiledb_object_t, void *)")] delegate* unmanaged[Cdecl]<sbyte*, tiledb_object_t, void*, int> callback, void* data);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_config_t* config, tiledb_vfs_t** vfs);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_vfs_free(tiledb_vfs_t** vfs);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_get_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, tiledb_config_t** config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_create_bucket([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_remove_bucket([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_empty_bucket([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_empty_bucket([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_empty);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_bucket([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_bucket);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_create_dir([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_dir([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_dir);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_remove_dir([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_is_file([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("int32_t *")] int* is_file);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_remove_file([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_dir_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_file_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_move_file([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_move_dir([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_copy_file([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_copy_dir([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* old_uri, [NativeTypeName("const char *")] sbyte* new_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_open([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri, tiledb_vfs_mode_t mode, tiledb_vfs_fh_t** fh);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_close([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_read([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh, [NativeTypeName("uint64_t")] ulong offset, void* buffer, [NativeTypeName("uint64_t")] ulong nbytes);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_write([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh, [NativeTypeName("const void *")] void* buffer, [NativeTypeName("uint64_t")] ulong nbytes);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_sync([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_ls([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* path, [NativeTypeName("int32_t (*)(const char *, void *)")] delegate* unmanaged[Cdecl]<sbyte*, void*, int> callback, void* data);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_vfs_fh_free(tiledb_vfs_fh_t** fh);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_fh_is_closed([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_fh_t* fh, [NativeTypeName("int32_t *")] int* is_closed);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_vfs_touch([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_vfs_t* vfs, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_uri_to_path([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("char *")] sbyte* path_out, [NativeTypeName("uint32_t *")] uint* path_length);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_enable();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_disable();

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_reset();

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_stats_dump([NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_dump_str([NativeTypeName("char **")] sbyte** @out);

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_stats_raw_dump([NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_raw_dump_str([NativeTypeName("char **")] sbyte** @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_stats_free_str([NativeTypeName("char **")] sbyte** @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_heap_profiler_enable([NativeTypeName("const char *")] sbyte* file_name_prefix, [NativeTypeName("uint64_t")] ulong dump_interval_ms, [NativeTypeName("uint64_t")] ulong dump_interval_bytes, [NativeTypeName("uint64_t")] ulong dump_threshold_bytes);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_fragment_info_t** fragment_info);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_fragment_info_free(tiledb_fragment_info_t** fragment_info);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_set_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_load([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_load_with_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, tiledb_encryption_type_t encryption_type, [NativeTypeName("const void *")] void* encryption_key, [NativeTypeName("uint32_t")] uint key_length);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** name);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t *")] uint* fragment_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_uri([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_fragment_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_dense([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("int32_t *")] int* dense);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_sparse([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("int32_t *")] int* sparse);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_timestamp_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* start, [NativeTypeName("uint64_t *")] ulong* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint did, void* domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char *")] sbyte* dim_name, void* domain);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_size_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint did, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_size_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint did, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_non_empty_domain_var_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char *")] sbyte* dim_name, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* mbr_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("uint32_t")] uint did, void* mbr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("const char *")] sbyte* dim_name, void* mbr);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_size_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("uint32_t")] uint did, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_size_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("const char *")] sbyte* dim_name, [NativeTypeName("uint64_t *")] ulong* start_size, [NativeTypeName("uint64_t *")] ulong* end_size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("uint32_t")] uint did, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_mbr_var_from_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t")] uint mid, [NativeTypeName("const char *")] sbyte* dim_name, void* start, void* end);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_cell_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint64_t *")] ulong* cell_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_version([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("uint32_t *")] uint* version);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_has_consolidated_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("int32_t *")] int* has);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_unconsolidated_metadata_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t *")] uint* unconsolidated);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_to_vacuum_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t *")] uint* to_vacuum_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_to_vacuum_uri([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_array_schema([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_array_schema_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint32_t")] uint fid, [NativeTypeName("const char **")] sbyte** schema_name);

        // [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        // [return: NativeTypeName("int32_t")]
        // public static extern int tiledb_fragment_info_dump([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_fragment_info_t *")] tiledb_fragment_info_t* fragment_info, [NativeTypeName("FILE *")] _IO_FILE* @out);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t** array_schema_evolution);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_array_schema_evolution_free(tiledb_array_schema_evolution_t** array_schema_evolution);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_add_attribute([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t* array_schema_evolution, tiledb_attribute_t* attribute);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_drop_attribute([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t* array_schema_evolution, [NativeTypeName("const char *")] sbyte* attribute_name);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_evolution_set_timestamp_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_evolution_t* array_schema_evolution, [NativeTypeName("uint64_t")] ulong lo, [NativeTypeName("uint64_t")] ulong hi);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_schema_timestamp_range([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_array_schema_t* array_schema, [NativeTypeName("uint64_t *")] ulong* lo, [NativeTypeName("uint64_t *")] ulong* hi);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_evolve([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_array_schema_evolution_t* array_schema_evolution);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_upgrade_version([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_subarray_add_point_ranges([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_subarray_t* subarray, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong count);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_add_point_ranges([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("uint32_t")] uint dim_idx, [NativeTypeName("const void *")] void* start, [NativeTypeName("uint64_t")] ulong count);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_relevant_fragment_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const tiledb_query_t *")] tiledb_query_t* query, [NativeTypeName("uint64_t *")] ulong* relevant_fragment_num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_query_get_status_details([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_query_t* query, [NativeTypeName("tiledb_query_status_details_t *")] tiledb_experimental_query_status_details_t* status);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_ctx_alloc_with_error(tiledb_config_t* config, [NativeTypeName("tiledb_ctx_t **")] tiledb_ctx_t** ctx, tiledb_error_t** error);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_array_consolidate_fragments([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* array_uri, [NativeTypeName("const char **")] sbyte** fragment_uris, [NativeTypeName("const uint64_t")] ulong num_fragments, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_alloc([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* group_uri, tiledb_group_t** group);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_open([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_query_type_t query_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_close([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void tiledb_group_free(tiledb_group_t** group);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_set_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_config([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_config_t** config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_put_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t value_type, [NativeTypeName("uint32_t")] uint value_num, [NativeTypeName("const void *")] void* value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_delete_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_metadata_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t *")] ulong* num);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_metadata_from_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t")] ulong index, [NativeTypeName("const char **")] sbyte** key, [NativeTypeName("uint32_t *")] uint* key_len, tiledb_datatype_t* value_type, [NativeTypeName("uint32_t *")] uint* value_num, [NativeTypeName("const void **")] void** value);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_has_metadata_key([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* key, tiledb_datatype_t* value_type, [NativeTypeName("int32_t *")] int* has_key);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_add_member([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* uri, [NativeTypeName("const uint8_t")] byte relative, [NativeTypeName("const char *")] sbyte* name);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_remove_member([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_member_count([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t *")] ulong* count);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_member_by_index([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("uint64_t")] ulong index, [NativeTypeName("char **")] sbyte** uri, tiledb_object_t* type, [NativeTypeName("char **")] sbyte** name);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_member_by_name([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char *")] sbyte* name, [NativeTypeName("char **")] sbyte** uri, tiledb_object_t* type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_is_open([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("int32_t *")] int* is_open);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_uri([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("const char **")] sbyte** group_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_get_query_type([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, tiledb_query_type_t* query_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_dump_str([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_group_t* group, [NativeTypeName("char **")] sbyte** dump_ascii, [NativeTypeName("const uint8_t")] byte recursive);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_consolidate_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* group_uri, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_group_vacuum_metadata([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* group_uri, tiledb_config_t* config);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filestore_schema_create([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* uri, tiledb_array_schema_t** array_schema);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filestore_uri_import([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* filestore_array_uri, [NativeTypeName("const char *")] sbyte* file_uri, tiledb_mime_type_t mime_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filestore_uri_export([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* file_uri, [NativeTypeName("const char *")] sbyte* filstore_array_uri);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filestore_buffer_import([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* filestore_array_uri, void* buf, [NativeTypeName("size_t")] ulong size, tiledb_mime_type_t mime_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filestore_buffer_export([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* filestore_array_uri, [NativeTypeName("size_t")] ulong offset, void* buf, [NativeTypeName("size_t")] ulong size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_filestore_size([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, [NativeTypeName("const char *")] sbyte* filestore_array_uri, [NativeTypeName("size_t *")] ulong* size);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_mime_type_to_str(tiledb_mime_type_t mime_type, [NativeTypeName("const char **")] sbyte** str);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_mime_type_from_str([NativeTypeName("const char *")] sbyte* str, tiledb_mime_type_t* mime_type);

        [DllImport("libtiledb", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("int32_t")]
        public static extern int tiledb_fragment_info_get_total_cell_num([NativeTypeName("tiledb_ctx_t *")] tiledb_ctx_t* ctx, tiledb_fragment_info_t* fragment_info, [NativeTypeName("uint64_t *")] ulong* count);
    }
}
