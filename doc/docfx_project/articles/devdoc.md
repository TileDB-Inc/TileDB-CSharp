## Layout for directories and files
```
TileDB-CSharp                                     # project folder
    |_ cpp                                        # cpp folder
      |_ cmake                                    # cmake modules folder
      |_ src
        |_ tiledb                                 # tiledb core library api folder
        |_ CMakeLists.txt                
        |_ tiledb_csharp_cpp_wrapper.cxx          # cpp wrapper file for windows/macos
        |_ tiledb_csharp_cpp_wrapper_linux64.cxx  # cpp wrapper file for linux         
      |_ swig
        |_ get_csharp_wrapper.sh                  # swig script to genrate binding files
        |_ swig_common.i                          # common swig file
        |_ swig_tiledb.i                          # tiledb swig file
        |_ swig_tiledb_exception.i                # tiledb exception swig file
      |_ tool
        |_ tiledb_code_util.py                    # python script for generating swig_tiledb.i
        |_ ...
      |_ CMakeLists.txt
    |_ dist                                       # default distribution folder
    |_ doc                                        # docfx project folder
    |_ examples                                   # examples
      |_ ...
    |_ test                                       # tests
      |_ ...
    |_ TileDB.CSharp                              # csharp folder
      |_ benchmark                                # benchmark folder
        |_ ...
      |_ src                                      # csharp wrapper files
        |_ Array.cs
        |_ ArraySchema.cs
        |_ ...
    |_ ...                  

```

## Generate swig file and binding files
```
cd cpp/tool
ipython
>>>import tiledb_code_util
>>>d=tiledb_code_util.get_swig_for_tiledb()
>>>quit
cp tiledb_swig.i ../swig
cd ../swig
chmod +x get_csharp_wrapper.sh
./get_csharp_wrapper.sh
```