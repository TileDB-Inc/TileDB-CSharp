#swig -c++ -csharp -namespace TileDB -cpperraswarn   -D_WIN64 -w462,317,401  -I./ -I../cpp/src/tiledb/cxx_api   -outdir ../csharp/wrapper_win64/ -o ../cpp/src/tiledb_wrapper_win64.cxx   swig_tiledb.i
#for linux, use -DSWIGWORDSIZE64 to make sure int64_t is mapped to long int instead of long long int 
swig -c++ -csharp -namespace TileDB -DSWIGWORDSIZE64 -cpperraswarn    -w462,317,401  -I./ -I../cpp/src/tiledb/cxx_api   -outdir ../csharp/csharp_wrapper/ -o ../cpp/src/tiledb_csharp_cpp_wrapper_linux64.cxx   swig_tiledb.i
