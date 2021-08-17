/**
 * @file   query_condition.h
 *
 * @section LICENSE
 *
 * The MIT License
 *
 * @copyright Copyright (c) 2021 TileDB, Inc.
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
 * This file declares the C++ API for the TileDB QueryCondition object.
 */

#ifndef TILEDB_CPP_API_QUERY_CONDITION_H
#define TILEDB_CPP_API_QUERY_CONDITION_H

#include "tiledb_cxx_context.h"
#include "tiledb.h"
#include "tiledb_cxx_enum.h"
#include <string>
#include <type_traits>

namespace tiledb {

class QueryCondition {
 public:
  /* ********************************* */
  /*     CONSTRUCTORS & DESTRUCTORS    */
  /* ********************************* */

  /**
   * Creates a TileDB query condition object.
   * @param ctx TileDB context.
   */
  QueryCondition(const std::shared_ptr<tiledb::Context>& ctx)
      : ctx_(ctx) {
    tiledb_query_condition_t* qc;
    ctx->handle_error(tiledb_query_condition_alloc(ctx->ptr().get(), &qc));
    query_condition_ = std::shared_ptr<tiledb_query_condition_t>(qc, deleter_);
  }

  /** Copy constructor. */
  QueryCondition(const QueryCondition&) = default;

  /** Move constructor. */
  QueryCondition(QueryCondition&&) = default;

  /** Destructor. */
  ~QueryCondition() = default;

  /**
   * Constructs an instance directly from a C-API query condition object.
   *
   * @param ctx The TileDB context.
   * @param qc The C-API query condition object.
   */
  QueryCondition(const std::shared_ptr<tiledb::Context>& ctx, tiledb_query_condition_t* const qc)
      : ctx_(ctx)
      , query_condition_(
            std::shared_ptr<tiledb_query_condition_t>(qc, deleter_)) {
  }

  /* ********************************* */
  /*     CONSTRUCTORS & DESTRUCTORS    */
  /* ********************************* */

  /** Copy-assignment operator. */
  QueryCondition& operator=(const QueryCondition&) = default;

  /** Move-assignment operator. */
  QueryCondition& operator=(QueryCondition&&) = default;

  /* ********************************* */
  /*                API                */
  /* ********************************* */

  /**
   * Initialize a TileDB query condition object.
   *
   * **Example:**
   *
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "my_array", TILEDB_READ);
   * tiledb::Query query(ctx, array, TILEDB_READ);
   *
   * int cmp_value = 5;
   * tiledb::QueryCondition qc;
   * qc.init(ctx, "a1", &cmp_value, sizeof(int), TILEDB_LT);
   * query.set_condition(qc);
   * @endcode
   *
   * @param ctx TileDB context.
   * @param attribute_name The name of the attribute to compare against.
   * @param condition_value The fixed value to compare against.
   * @param condition_value_size The byte size of `condition_value`.
   * @param op The comparison operation between each cell value and
   * `condition_value`.
   */
  void init(
      const std::string& attribute_name,
      const void* condition_value,
      uint64_t condition_value_size,
      tiledb::QueryConditionOperatorType optype) {
    tiledb_query_condition_op_t op = (tiledb_query_condition_op_t)optype;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_query_condition_init(
        c_ctx,
        query_condition_.get(),
        attribute_name.c_str(),
        condition_value,
        condition_value_size,
        op));
  }

  /**
   * Initializes a TileDB query condition object.
   *
   * **Example:**
   *
   * @code{.cpp}
   * tiledb::Context ctx;
   * tiledb::Array array(ctx, "my_array", TILEDB_READ);
   * tiledb::Query query(ctx, array, TILEDB_READ);
   *
   * int cmp_value = 5;
   * tiledb::QueryCondition qc;
   * qc.init(ctx, "a1", &cmp_value, sizeof(int), TILEDB_LT);
   * query.set_condition(qc);
   * @endcode
   *
   * @param ctx TileDB context.
   * @param attribute_name The name of the attribute to compare against.
   * @param condition_value The fixed value to compare against.
   * @param condition_value_size The byte size of `condition_value`.
   * @param op The comparison operation between each cell value and
   * `condition_value`.
   */
  void init(
      const std::string& attribute_name,
      const std::string& condition_value,
      tiledb::QueryConditionOperatorType optype) {
    tiledb_query_condition_op_t op = (tiledb_query_condition_op_t)optype;
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    ctx_->handle_error(tiledb_query_condition_init(
        c_ctx,
        query_condition_.get(),
        attribute_name.c_str(),
        condition_value.c_str(),
        condition_value.size(),
        op));
  }

  /** Returns a shared pointer to the C TileDB query condition object. */
  std::shared_ptr<tiledb_query_condition_t> ptr() const {
    return query_condition_;
  }

  /**
   * Combines this instance with another instance to form a multi-clause
   * condition object.
   *
   * **Example:**
   *
   * @code{.cpp}
   * int qc1_cmp_value = 10;
   * tiledb::QueryCondition qc1(
   *   ctx, "a1", &qc1_cmp_value, sizeof(int), ILEDB_LT);
   * int qc2_cmp_value = 3;
   * tiledb::QueryCondition qc2(
   *   ctx, "a1", &qc2_cmp_value, sizeof(int), TILEDB_GE);
   *
   * tiledb::QueryCondition qc3 = qc1.combine(qc2, TILEDB_AND);
   * query.set_condition(qc3);
   * @endcode
   *
   * @param rhs The right-hand-side query condition object.
   * @param combination_op The logical combination operator that combines this
   * instance with `rhs`.
   */
  QueryCondition combine(
      const QueryCondition& rhs,
      tiledb::QueryConditionCombinationOperatorType combination_optype) const {
    tiledb_query_condition_combination_op_t combination_op = (tiledb_query_condition_combination_op_t)combination_optype;    
    tiledb_ctx_t* c_ctx = ctx_->ptr().get();
    tiledb_query_condition_t* combined_qc;
    ctx_->handle_error(tiledb_query_condition_combine(
        c_ctx,
        query_condition_.get(),
        rhs.ptr().get(),
        combination_op,
        &combined_qc));

    return QueryCondition(ctx_, combined_qc);
  }

  /* ********************************* */
  /*          STATIC FUNCTIONS         */
  /* ********************************* */

  /**
   * Factory function for creating a new query condition with a string
   * datatype.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * auto a1 = tiledb::QueryCondition::create(ctx, "a1", "foo", TILEDB_LE);
   * @endcode
   *
   * @tparam T Datatype of the attribute. Can either be arithmetic type or
   * string.
   * @param ctx The TileDB context.
   * @param name The attribute name.
   * @param value The value to compare against.
   * @param op The comparison operator.
   * @return A new QueryCondition object.
   */
  static QueryCondition create(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& attribute_name,
      const std::string& value,
      tiledb::QueryConditionOperatorType optype) {
 
    QueryCondition qc(ctx);
    qc.init(attribute_name, value, optype);
    return qc;
  }

  /**
   * Factory function for creating a new query condition with datatype T.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * auto a1 = tiledb::QueryCondition::create<int>(ctx, "a1", 5, TILEDB_LE);
   * auto a2 = tiledb::QueryCondition::create<float>(ctx, "a3", 3.5,
   *   TILEDB_GT);
   * auto a3 = tiledb::QueryCondition::create<double>(ctx,
   *   "a4", 10.0, TILEDB_LT);
   * @endcode
   *
   * @tparam T Datatype of the attribute. Can either be arithmetic type or
   * string.
   * @param ctx The TileDB context.
   * @param name The attribute name.
   * @param value The value to compare against.
   * @param op The comparison operator.
   * @return A new QueryCondition object.
   */
  template <typename T>
  static QueryCondition create(
      const std::shared_ptr<tiledb::Context>& ctx,
      const std::string& attribute_name,
      T value,
      tiledb::QueryConditionOperatorType optype) {
    tiledb_query_condition_op_t op = (tiledb_query_condition_op_t)optype;
    QueryCondition qc(ctx);
    qc.init(attribute_name, &value, sizeof(T), op);
    return qc;
  }

  /**
   * Factory function for creating a new query condition with datatype T.
   *
   * **Example:**
   * @code{.cpp}
   * tiledb::Context ctx;
   * auto a1 = tiledb::QueryCondition::create_for_datatype(ctx, DataType::TILEDB_INT32 "a1", 5, TILEDB_LE);
   * auto a2 = tiledb::QueryCondition::create_for_datatype(ctx, DataType::TILEDB_FLOAT64, "a2", "3.5",
   *   TILEDB_GT);
   * auto a3 = tiledb::QueryCondition::create_for_datatype(ctx, DataType::TILEDB_FLOAT64,
   *   "a3", "10.0", TILEDB_LT);
   * @endcode
   *
   * @param ctx The TileDB context.
   * @param datatype The DataType type.
   * @param name The attribute name.
   * @param strvalue The string value to compare against.
   * @param op The comparison operator.
   * @return A new QueryCondition object.
   */
  static QueryCondition create_for_datatype(
      const std::shared_ptr<tiledb::Context>& ctx,
      tiledb::DataType datatype,
      const std::string& attribute_name,
      std::string strvalue,
      tiledb::QueryConditionOperatorType optype) {
    tiledb_query_condition_op_t op = (tiledb_query_condition_op_t)optype;
    QueryCondition qc(ctx);

    switch (datatype) {
      case DataType::TILEDB_INT8:
        {
          int8_t v = (int8_t)atoi(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(int8_t), optype);
        }
        break;
      case DataType::TILEDB_UINT8:
        {
          uint8_t v = (uint8_t)atoi(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(uint8_t), optype);
        }
        break;
      case DataType::TILEDB_INT16:
        {
          int16_t v = (int16_t)atoi(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(int16_t), optype);
        }
        break;
      case DataType::TILEDB_UINT16:
        {
          uint16_t v = (uint16_t)atoi(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(uint16_t), optype);
        }
        break;
      case DataType::TILEDB_INT32:
        {
          int32_t v = atoi(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(int32_t), optype);
        }
        break;
      case DataType::TILEDB_UINT32:
        {
          uint32_t v = (uint32_t)atoll(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(uint32_t), optype);
        }
        break;
      case DataType::TILEDB_INT64:
        {
          int64_t v = (int64_t)atoll(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(int64_t), optype);
        }
        break;
      case DataType::TILEDB_UINT64:
        {
          uint64_t v = (uint64_t)atoll(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(uint64_t), optype);
        }
        break;
      case DataType::TILEDB_FLOAT32:
        {
          float v = (float)atof(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(float), optype);
        }
        break;
      case DataType::TILEDB_FLOAT64:
        {
          double v = atof(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(double), optype);
        }
        break;
      case TILEDB_DATETIME_YEAR:
      case TILEDB_DATETIME_MONTH:
      case TILEDB_DATETIME_WEEK:
      case TILEDB_DATETIME_DAY:
      case TILEDB_DATETIME_HR:
      case TILEDB_DATETIME_MIN:
      case TILEDB_DATETIME_SEC:
      case TILEDB_DATETIME_MS:
      case TILEDB_DATETIME_US:
      case TILEDB_DATETIME_NS:
      case TILEDB_DATETIME_PS:
      case TILEDB_DATETIME_FS:
      case TILEDB_DATETIME_AS:
        {
          int64_t v = (int64_t)atoll(strvalue.c_str());
           qc.init(attribute_name, &v, sizeof(int64_t), optype);
        }
        break;
      case TILEDB_STRING_ASCII:
      case TILEDB_CHAR:
      case TILEDB_STRING_UTF8:
      case TILEDB_STRING_UTF16:
      case TILEDB_STRING_UTF32:
      case TILEDB_STRING_UCS2:
      case TILEDB_STRING_UCS4:
        {
          qc.init(attribute_name,strvalue,optype);
        }
        break;
      case TILEDB_ANY:
        // Not supported domain types
        throw TileDBError("Invalid Dim type");
      default:
        break;
    }//switch       

   

    return qc;
  }  

 private:
  /* ********************************* */
  /*         PRIVATE ATTRIBUTES        */
  /* ********************************* */

  /** Deleter wrapper. */
  impl::Deleter deleter_;

  /** The TileDB context. */
  std::shared_ptr<Context> ctx_; 

  /** Pointer to the TileDB C query object. */
  std::shared_ptr<tiledb_query_condition_t> query_condition_;
};

}  // namespace tiledb

#endif  // TILEDB_CPP_API_QUERY_CONDITION_H
