#swig -c++ -csharp -namespace TileDB -cpperraswarn   -D_WIN64 -w462,317,401  -I./ -I../cpp/src/tiledb/cxx_api   -outdir ../csharp/wrapper_win64/ -o ../cpp/src/tiledb_wrapper_win64.cxx   swig_tiledb.i
swig -c++ -csharp -namespace TileDB -cpperraswarn    -w462,317,401  -I./ -I../src/tiledb/cxx_api   -outdir ../../TileDB.CSharp/src/ -o ../src/tiledb_csharp_cpp_wrapper.cxx   swig_tiledb.i
