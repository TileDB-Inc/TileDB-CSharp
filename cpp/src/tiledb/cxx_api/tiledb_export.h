#ifndef TILEDB_EXPORT_H
#define TILEDB_EXPORT_H

#ifdef TILEDB_STATIC_DEFINE
#  define TILEDB_EXPORT
#  define TILEDB_NO_EXPORT
#else
#  ifndef TILEDB_EXPORT
#    ifdef TILEDB_CORE_OBJECTS_EXPORTS
        /* We are building this library */
#      define TILEDB_EXPORT __declspec(dllexport)
#    else
        /* We are using this library */
#      define TILEDB_EXPORT __declspec(dllimport)
#    endif
#  endif

#  ifndef TILEDB_NO_EXPORT
#    define TILEDB_NO_EXPORT 
#  endif
#endif

//#ifndef TILEDB_DEPRECATED
//#  define TILEDB_DEPRECATED __declspec(deprecated)
//#endif
#define TILEDB_DEPRECATED

//#ifndef TILEDB_DEPRECATED_EXPORT
//#  define TILEDB_DEPRECATED_EXPORT TILEDB_EXPORT TILEDB_DEPRECATED
//#endif
#define TILEDB_DEPRECATED_EXPORT

//#ifndef TILEDB_DEPRECATED_NO_EXPORT
//#  define TILEDB_DEPRECATED_NO_EXPORT TILEDB_NO_EXPORT TILEDB_DEPRECATED
//#endif
#define TILEDB_DEPRECATED_NO_EXPORT

#if 0 /* DEFINE_NO_DEPRECATED */
#  ifndef TILEDB_NO_DEPRECATED
#    define TILEDB_NO_DEPRECATED
#  endif
#endif

#endif /* TILEDB_EXPORT_H */
