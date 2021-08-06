set -e -x
cd cpp
mkdir build && cd build
cmake ..
cmake --build . --target install --config Release