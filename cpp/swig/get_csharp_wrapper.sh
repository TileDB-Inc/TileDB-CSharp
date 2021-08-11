# run first for linux, then run the second command for windows and macos
# for linux, use -DSWIGWORDSIZE64 to make sure int64_t is mapped to long int instead of long long int 
swig -c++ -csharp -namespace TileDB -DSWIGWORDSIZE64 -cpperraswarn    -w462,317,401  -I./ -I../src/tiledb/cxx_api   -outdir ../../TileDB.CSharp/src/ -o ../src/tiledb_csharp_cpp_wrapper_linux64.cxx   swig_tiledb.i
# for windows and macos
swig -c++ -csharp -namespace TileDB -cpperraswarn    -w462,317,401  -I./ -I../src/tiledb/cxx_api   -outdir ../../TileDB.CSharp/src/ -o ../src/tiledb_csharp_cpp_wrapper.cxx   swig_tiledb.i
