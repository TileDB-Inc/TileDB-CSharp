/**
 * @file   tiledb
 *
 * @author Ravi Gaddipati
 *
 * @section LICENSE
 *
 * The MIT License
 *
 * @copyright Copyright (c) 2017-2020 TileDB, Inc.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 * @section DESCRIPTION
 *
 * This file declares the C++ API for TileDB.
 */

#ifndef TILEDB_CPP_H
#define TILEDB_CPP_H

#include "tiledb_cxx_array.h"
#include "tiledb_cxx_array_schema.h"
#include "tiledb_cxx_attribute.h"
#include "tiledb_cxx_config.h"
#include "tiledb_cxx_context.h"
#include "tiledb_cxx_deleter.h"
#include "tiledb_cxx_dimension.h"
#include "tiledb_cxx_domain.h"
#include "tiledb_cxx_exception.h"
#include "tiledb_cxx_filter.h"
#include "tiledb_cxx_filter_list.h"
#include "tiledb_cxx_group.h"
#include "tiledb_cxx_object.h"
#include "tiledb_cxx_object_iter.h"
#include "tiledb_cxx_query.h"
#include "tiledb_cxx_schema_base.h"
#include "tiledb_cxx_stats.h"
#include "tiledb.h"
#include "tiledb_cxx_utils.h"
#include "tiledb_cxx_version.h"
#include "tiledb_cxx_vfs.h"

#endif  // TILEDB_CPP_H