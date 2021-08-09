/**
 * @file   tiledb_cxx_string_util.h
 *
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
 * This file declares the C++ API for string utility functions.
 */

 
#ifndef TILEDB_CPP_API_STRING_UTIL_H
#define TILEDB_CPP_API_STRING_UTIL_H

#include <cstdint>
#include <iostream>
#include <sstream>
#include <fstream>
#include <iomanip>
#include <cmath>
#include <string>
#include <algorithm>
#include <locale>
#include <cctype>
#include <vector>
#include <set>

namespace tiledb {

class  StringUtil {
public:
	StringUtil() {} // = delete;
	StringUtil(const StringUtil&) {} // = delete;
	StringUtil(StringUtil&&) {} // = delete;
	~StringUtil() {} // = delete;

	StringUtil& operator=(const StringUtil&) { return *this; } // = delete;
	StringUtil& operator=(StringUtil&&) { return *this; } // = delete;

	//! Is the given character blank (empty or contains only space characters)?
	/*!
		\param ch - Character to check
		\return 'true' if given character is blank, 'false' if given character is not blank
	*/
	static bool is_blank(char ch);
	//! Is the given C-string blank (empty or contains only space characters)?
	/*!
		\param str - C-string to check
		\return 'true' if given C-string is blank, 'false' if given C-string is not blank
	*/
	static bool is_blank(const char* str);
	//! Is the given string blank (empty or contains only space characters)?
	/*!
		\param str - String to check
		\return 'true' if given string is blank, 'false' if given string is not blank
	*/
	static bool is_blank(const std::string& str);

	//! Convert the given character to lower case
	/*!
		\param ch - Character to convert
	*/
	static char to_lower(char ch);
	//! Convert the given character to UPPER case
	/*!
		\param ch - Character to convert
	*/
	static char to_upper(char ch);

	//! Convert the given constant string converted to lower case
	/*!
		\param str - String to convert
		\return String converted to lower case
	*/
	static std::string to_lower(const std::string& str);
	//! Convert the the given constant string converted to UPPER case
	/*!
		\param str - String to convert
		\return String converted to UPPER case
	*/
	static std::string to_upper(const std::string& str);

	 

	//! Trims space characters from the start of the given constant string
	/*!
		\param str - String to trim
		\return Trimmed string
	*/
	static std::string to_ltrim(const std::string& str);
	//! Trims space characters from the end of the given constant string
	/*!
		\param str - String to trim
		\return Trimmed string
	*/
	static std::string to_rtrim(const std::string& str);
	//! Trims space characters from the both sides of the given constant string
	/*!
		\param str - String to trim
		\return Trimmed string
	*/
	static std::string to_trim(const std::string& str);

	/**
	* @brief to trim dot suffix, for example 000300.SH will be trimmed to 000300
	* 
	* @param str
	*/
	static std::string to_trim_dot_suffix(const std::string& str);

	/**
	* @brief get dot suffix , for example 000300.SH will return SH
	*
	* @param str
	*/
	static std::string get_dot_suffix(const std::string& str);

	/**
	* @brief convert string to double
	*
	* @param str
	*
	* @return double value
	*/
	static double to_double(const std::string& str);

	/**
	* @brief convert string to int32
	*
	* @param str
	*
	* @return int32 value
	*/
	static int to_int(const std::string& str);

	/**
	* @brief convert string to int64
	*
	* @param str
	*
	* @return int64 value
	*/
	static int64_t to_int64(const std::string& str);



	 
	//! Is the given string contains the given character?
	/*!
		\param str - String to search in
		\param ch - Character to find
		\return 'true' if the substring was found, 'false' if the substring was not found
	*/
	static bool contains(const std::string& str, const char ch);
	//! Is the given string contains the given C-string?
	/*!
		\param str - String to search in
		\param substr - Substring to find
		\return 'true' if the substring was found, 'false' if the substring was not found
	*/
	static bool contains(const std::string& str, const char* substr);
	//! Is the given string contains the given substring?
	/*!
		\param str - String to search in
		\param substr - Substring to find
		\return 'true' if the substring was found, 'false' if the substring was not found
	*/
	static bool contains(const std::string& str, const std::string& substr);



	//! Count all occurrences of substring
	/*!
		\param str - Modifying string
		\param substr - Substring to find
		\return Count of all substring occurrences
	*/
	static size_t count_all(const std::string& str, const std::string& substr);

	//! Replace the first occurrence of substring with another substring
	/*!
		\param str - Modifying string
		\param substr - Substring to find
		\param with - Substring to replace
		\return 'true' if first occurrence of substring was found and relapsed, 'false' if the substring was not found
	*/
	static bool replace_first(std::string& str, const std::string& substr, const std::string& with);
	//! Replace the last occurrence of substring with another substring
	/*!
		\param str - Modifying string
		\param substr - Substring to find
		\param with - Substring to replace
		\return 'true' if last occurrence of substring was found and relapsed, 'false' if the substring was not found
	*/
	static bool replace_last(std::string& str, const std::string& substr, const std::string& with);
	//! Replace all occurrences of substring with another substring
	/*!
		\param str - Modifying string
		\param substr - Substring to find
		\param with - Substring to replace
		\return 'true' if all occurrences of substring were found and relapsed, 'false' if the substring was not found
	*/
	static bool replace_all(std::string& str, const std::string& substr, const std::string& with);

	//! Checks the given string for specific prefix
	/*!
		\param str - String to check
		\param prefix - Prefix to check
		\return 'true' if the given string starts with the specific prefix, 'false' if the given string does not start with the specific prefix
	*/
	static bool starts_with(const std::string& str, const std::string& prefix);
	//! Checks the given string for specific suffix
	/*!
		\param str - String to check
		\param suffix - Suffix to check
		\return 'true' if the given string ends with the specific suffix, 'false' if the given string does not end with the specific suffix
	*/
	static bool ends_with(const std::string& str, const std::string& suffix);

	/**
	* @brief is the str starts with any of word in set  
	*
	* @param str
	* @param wordset
	*
	* @return true or false
	*/
	static bool starts_with_any_of_words(const std::string& str, const std::set<std::string>& wordset);

	/**
	* @brief is the str ends with any of word in set
	*
	* @param str
	* @param wordset
	*
	* @return true or false
	*/
	static bool ends_with_any_of_words(const std::string& str, const std::set<std::string>& wordset);

	/**
	* @brief is any of word in set is substr of str
	*
	* @param str
	* @param wordset
	*
	* @return true or false
	*/
	static bool is_any_of_words_in_str(const std::string& str, const std::set<std::string>& wordset);

	/**
	* @brief are all of words in set is substr of str
	*
	* @param str
	* @param wordset
	*
	* @return true or false
	*/
	static bool is_all_of_words_in_str(const std::string& str, const std::set<std::string>& wordset);

	//! Split the string into tokens by the given delimiter character
	/*!
		\param str - String to split
		\param delimiter - Delimiter character
		\param skip_empty - Skip empty substrings flag (default is false)
		\return Vector of tokens
	*/
	static std::vector<std::string> split(const std::string& str, char delimiter, bool skip_empty = false);
	//! Split the string into tokens by the given delimiter string
	/*!
		\param str - String to split
		\param delimiter - Delimiter string
		\param skip_empty - Skip empty substrings flag (default is false)
		\return Vector of tokens
	*/
	static std::vector<std::string> split(const std::string& str, const std::string& delimiter, bool skip_empty = false);
	//! Split the string into tokens by the any character in the given delimiter string
	/*!
		\param str - String to split
		\param delimiters - Delimiters string
		\param skip_empty - Skip empty substrings flag (default is false)
		\return Vector of string tokens
	*/
	static std::vector<std::string> split_by_any(const std::string& str, const std::string& delimiters, bool skip_empty = false);

	//! Join tokens into the string
	/*!
		\param tokens - Vector of string tokens
		\param skip_empty - Skip empty tokens flag (default is false)
		\param skip_blank - Skip blank tokens flag (default is false)
		\return Joined string
	*/
	static std::string join(const std::vector<std::string>& tokens, bool skip_empty = false, bool skip_blank = false);
	//! Join tokens into the string with delimiter character
	/*!
		\param tokens - Vector of string tokens
		\param delimiter - Delimiter character
		\param skip_empty - Skip empty tokens flag (default is false)
		\param skip_blank - Skip blank tokens flag (default is false)
		\return Joined string
	*/
	static std::string join(const std::vector<std::string>& tokens, char delimiter, bool skip_empty = false, bool skip_blank = false);
	//! Join tokens into the string with delimiter C-string
	/*!
		\param tokens - Vector of string tokens
		\param delimiter - Delimiter C-string
		\param skip_empty - Skip empty tokens flag (default is false)
		\param skip_blank - Skip blank tokens flag (default is false)
		\return Joined string
	*/
	static std::string join(const std::vector<std::string>& tokens, const char* delimiter, bool skip_empty = false, bool skip_blank = false);
	//! Join tokens into the string with delimiter string
	/*!
		\param tokens - Vector of string tokens
		\param delimiter - Delimiter string
		\param skip_empty - Skip empty tokens flag (default is false)
		\param skip_blank - Skip blank tokens flag (default is false)
		\return Joined string
	*/
	static std::string join(const std::vector<std::string>& tokens, const std::string& delimiter, bool skip_empty = false, bool skip_blank = false);


	static std::vector<double> split_str_to_double_vector(const std::string& str, const std::string& delimiter=",");

	static std::string join_double_vector_to_str(const std::vector<double>& vs, const std::string& delimiter=",", int precesion=12);

	static std::vector<int> split_str_to_int_vector(const std::string& str, const std::string& delimiter=",");

	static std::string join_int_vector_to_str(const std::vector<int>& vs, const std::string& delimiter=",");

	static std::vector<int64_t> split_str_to_int64_vector(const std::string& str, const std::string& delimiter = ",");

	static std::string join_int64_vector_to_str(const std::vector<int64_t>& vs, const std::string& delimiter = ",");

	//static bool isValidJsonStr(const std::string& s);
	//static std::string getValueForKeyInJsonStr(const std::string& s, const std::string& key);
	//static std::vector<std::string> getKeysForJsonStr(const std::string& s);
	//static std::string removeKeysForJsonStr(const std::string& s, std::vector<std::string>& keys);
	//
	//static std::string keepKeysForJsonStr(const std::string& s, std::vector<std::string>& keep_keys);

	//! Converts arbitrary datatypes into string using std::ostringstream
	/*!
		\param value - Value to convert
		\return Result converted string
	*/
	template <typename T>
	static std::string to_string(const T& value);
	//! Converts strings to arbitrary datatypes using std::istringstream
	/*!
		\param str - String converted into the value
		\return Result converted value
	*/
	template <typename T>
	static T from_string(const std::string& str);

public:


	/**
	* @brief convert int to string
	*
	* @param i integer
	*
	* @return string
	*/
	static std::string int_to_str(int i);

	/**
	* @brief convert unsigned int to string
	*
	* @param unsigned int i
	*
	* @return string
	*/
	static std::string uint_to_str(unsigned int i);

	/**
	* @brief convert long to string
	*
	* @param l long
	*
	* @return string
	*/
	static std::string long_to_str(long l);

	/**
	* @brief convert longlong to string
	*
	* @param ll long long
	*
	* @return string
	*/
	static std::string longlong_to_str(long long ll);

	/**
	* @brief convert size_t to string
	*
	* @param st size_t
	*
	* @return string
	*/
	static std::string size_t_to_str(size_t st);

	/**
	* @brief convert int64_t to string
	*
	* @param i int64_t
	*
	* @return string
	*/
	static std::string int64_to_str(int64_t i);

	/**
	* @brief convert double to string
	*
	* @param d double
	*
	* @return string
	*/
	static std::string double_to_str(double d);

	/**
	* @brief convert bool to string
	*
	* @param b bool
	*
	* @return string
	*/
	static std::string bool_to_str(bool b);

private:
	static char toLowerInternal(char ch);
	static char toUpperInternal(char ch);


//public:
//	/**
//	* @brief match pattern or not
//	*
//	* @param s
//	* @param pattern string
//	*
//	* @return matched vector
//	*/
//	static bool matchPattern(const std::string& s, const std::string& pattern);
//
//	/**
//	* @brief get matched vector , the first vector item is input string itself if matches
//	*
//	* @param s
//	* @param pattern string
//	*
//	* @return matched vector
//	*/
//	static std::vector<std::string> getMatchedGroups(const std::string& s, const std::string& pattern);
//
};//class

////
template <typename T>
inline std::string StringUtil::to_string(const T& value)
{
	std::ostringstream ss;
	ss << value;
	return ss.str();
}

template <typename T>
inline T StringUtil::from_string(const std::string& str)
{
	T result;
	std::istringstream(std::string(str)) >> result;
	return result;
}

template <>
inline const char* StringUtil::from_string(const std::string& str)
{
	return std::string(str).c_str();
}

//inline std::string StringUtil::fromString(const std::string& str)
//{
//	return std::string(str);
//}

template <>
inline std::string StringUtil::from_string(const std::string& str)
{
	return str;
}

template <>
inline bool StringUtil::from_string(const std::string& str)
{
	std::string value = to_lower(str);
	if ((value == "true") || (value == "yes") || (value == "on") || (value == "1"))
		return true;
	if ((value == "false") || (value == "no") || (value == "off") || (value == "0"))
		return false;

//	assert("Invalid boolean value represented in string!");
	return false;
}

}//namespace
 

#endif