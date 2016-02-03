/**********************************************************************************************************************
 * Taoqi is a Customer Relationship Management program created by Taoqi Software, Inc. 
 * Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved.
 * 
 * This program is free software: you can redistribute it and/or modify it under the terms of the 
 * GNU Affero General Public License as published by the Free Software Foundation, either version 3 
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * See the GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>. 
 * 
 * You can contact Taoqi Software, Inc. at email address support@Taoqi.com. 
 * 
 * In accordance with Section 7(b) of the GNU Affero General Public License version 3, 
 * the Appropriate Legal Notices must display the following words on all interactive user interfaces: 
 * "Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved."
 *********************************************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Xml;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;

namespace Taoqi
{
	/// <summary>
	/// Summary description for Sql.
	/// </summary>
	public class Sql
	{
		public static string HexEncode(byte[] aby)
		{
			string hex = "0123456789abcdef" ;
			StringBuilder sb = new StringBuilder();
			for ( int i = 0 ; i < aby.Length ; i++ )
			{
				sb.Append(hex[(aby[i] & 0xf0) >> 4]);
				sb.Append(hex[ aby[i] & 0x0f]);
			}
			return sb.ToString();
		}

		public static string FormatSQL(string s, int nMaxLength)
		{
			if ( Sql.IsEmptyString(s) )
				s = "null";
			else
				s = "'" + Sql.EscapeSQL(s) + "'";
			if ( nMaxLength > s.Length )
				return s + Strings.Space(nMaxLength - s.Length);
			return s;
		}

		public static string EscapeSQL(string str)
		{
			str = str.Replace("\'", "\'\'");
			return str;
		}

		public static string EscapeSQLLike(string str)
		{
			str = str.Replace(@"\", @"\\");
			str = str.Replace("%" , @"\%");
			str = str.Replace("_" , @"\_");
			return str;
		}

		// 04/05/2012   EscapeXml is needed in the SearchView. 
		public static string EscapeXml(string str)
		{
			str = str.Replace("\"", "&quot;");
			str = str.Replace("\'", "&apos;");
			str = str.Replace("<" , "&lt;"  );
			str = str.Replace(">" , "&gt;"  );
			str = str.Replace("&" , "&amp;" );
			return str;
		}

		public static string EscapeJavaScript(string str)
		{
			str = str.Replace(@"\", @"\\");
			str = str.Replace("\'", "\\\'");
			str = str.Replace("\"", "\\\"");
			// 07/31/2006   Stop using VisualBasic library to increase compatibility with Mono. 
			str = str.Replace("\t", "\\t");
			str = str.Replace("\r", "\\r");
			str = str.Replace("\n", "\\n");
			return str;
		}

		// 11/06/2013   Make sure to JavaScript escape the text as the various languages may introduce accents. 
		public static string[] EscapeJavaScript(string[] arr)
		{
			string[] arrClean = null;
			if ( arr != null )
			{
				arrClean = new string[arr.Length];
				arr.CopyTo(arrClean, 0);
				for ( int i = 0; i < arrClean.Length; i++ )
				{
					arrClean[i] = Sql.EscapeJavaScript(arrClean[i]);
				}
			}
			return arrClean;
		}

		public static bool IsEmptyString(string str)
		{
			if ( str == null || str == String.Empty )
				return true;
			return false;
		}

		public static bool IsEmptyString(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return true;
			if ( obj.ToString() == String.Empty )
				return true;
			return false;
		}

		public static string ToString(string str)
		{
			if ( str == null )
				return String.Empty;
			return str;
		}

		public static string ToString(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return String.Empty;
			return obj.ToString();
		}

		public static object ToDBString(string str)
		{
			if ( str == null )
				return DBNull.Value;
			if ( str == String.Empty )
				return DBNull.Value;
			return str;
		}

		public static object ToDBString(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			string str = obj.ToString();
			if ( str == String.Empty )
				return DBNull.Value;
			return str ;
		}

		public static byte[] ToBinary(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return new byte[0];
			return (byte[]) obj;
		}

		public static object ToDBBinary(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			return obj ;
		}

		public static object ToDBBinary(byte[] aby)
		{
			if ( aby == null )
				return DBNull.Value;
			else if ( aby.Length == 0 )
				return DBNull.Value;
			return aby ;
		}

		public static DateTime ToDateTime(DateTime dt)
		{
			return dt;
		}

		public static DateTime ToDateTime(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DateTime.MinValue;
			// If datatype is DateTime, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.DateTime") )
				return Convert.ToDateTime(obj) ;
			if ( !Information.IsDate(obj) )
				return DateTime.MinValue;
			return Convert.ToDateTime(obj);
		}

		public static string ToDateString(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return String.Empty;
			// If datatype is DateTime, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.DateTime") )
				return Convert.ToDateTime(obj).ToShortDateString() ;
			if ( !Information.IsDate(obj) )
				return String.Empty;
			return Convert.ToDateTime(obj).ToShortDateString();
		}

		// 05/27/2007   It looks better to show nothing than to show 01/01/0001 12:00:00 AM. 
		public static string ToString(DateTime dt)
		{
			// If datatype is DateTime, then nothing else is necessary. 
			if ( dt == DateTime.MinValue )
				return String.Empty;
			return dt.ToString();
		}

		public static string ToDateString(DateTime dt)
		{
			// If datatype is DateTime, then nothing else is necessary. 
			if ( dt == DateTime.MinValue )
				return String.Empty;
			return dt.ToShortDateString();
		}

		public static string ToTimeString(DateTime dt)
		{
			// If datatype is DateTime, then nothing else is necessary. 
			if ( dt == DateTime.MinValue )
				return String.Empty;
			return dt.ToShortTimeString();
		}

		public static object ToDBDateTime(DateTime dt)
		{
			// 03/28/2010   Check for SQL Server minimum date. 
			// SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
			if ( dt == DateTime.MinValue )
				return DBNull.Value;
			else if ( dt.Year < 1753 )
				return DBNull.Value;
			return dt;
		}

		public static object ToDBDateTime(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			if ( !Information.IsDate(obj) )
				return DBNull.Value;
			DateTime dt = Convert.ToDateTime(obj);
			if ( dt == DateTime.MinValue )
				return DBNull.Value;
			return dt;
		}

		public static bool IsEmptyGuid(Guid g)
		{
			if ( g == Guid.Empty )
				return true;
			return false;
		}

		public static bool IsEmptyGuid(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return true;
			string str = obj.ToString();
			if ( str == String.Empty )
				return true;
			Guid g = XmlConvert.ToGuid(str);
			if ( g == Guid.Empty )
				return true;
			return false;
		}

		public static Guid ToGuid(Guid g)
		{
			return g;
		}

		public static Guid ToGuid(Byte[] b)
		{
			// 08/09/2005   Convert byte array to a true Guid. 
			Guid g = new Guid((b[0]+(b[1]+(b[2]+b[3]*256)*256)*256),(short)(b[4]+b[5]*256),(short)(b[6]+b[7]*256),b[8],b[9],b[10],b[11],b[12],b[13],b[14],b[15]);
			return g;
		}

		public static Guid ToGuid(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return Guid.Empty;
			// If datatype is Guid, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Guid") )
				return (Guid) obj ;
			// 08/09/2005   Oracle returns RAW(16). 
			// 08/10/2005   Attempting to use RAW has too many undesireable consequences.  Use CHAR(36) instead. 
			/*
			if ( obj.GetType() == Type.GetType("System.Byte[]") )
			{
				//MemoryStream ms = new MemoryStream(16);
				//BinaryFormatter b = new BinaryFormatter();
				//b.Serialize(ms, obj);
				//return new Guid(ms.ToArray());
				//Byte[] b = (System.Array) obj;
				System.Array a = obj as System.Array;
				Byte[] b = a as Byte[];
				//return new Guid(b);
				// 08/09/2005   Convert byte array to a true Guid. 
				Guid g = new Guid((b[0]+(b[1]+(b[2]+b[3]*256)*256)*256),(short)(b[4]+b[5]*256),(short)(b[6]+b[7]*256),b[8],b[9],b[10],b[11],b[12],b[13],b[14],b[15]);
				return g;
			}
			*/
			string str = obj.ToString();
			if ( str == String.Empty )
				return Guid.Empty;
			return XmlConvert.ToGuid(str);
		}

		public static object ToDBGuid(Guid g)
		{
			if ( g == Guid.Empty )
				return DBNull.Value;
			return g;
		}

		public static object ToDBGuid(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			// If datatype is Guid, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Guid") )
				return obj;
			string str = obj.ToString();
			if ( str == String.Empty )
				return DBNull.Value;
			Guid g = XmlConvert.ToGuid(str);
			if ( g == Guid.Empty )
				return DBNull.Value;
			return g ;
		}


		public static Int32 ToInteger(Int32 n)
		{
			return n;
		}

		public static Int32 ToInteger(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return 0;
			// If datatype is Integer, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Int32") )
				return Convert.ToInt32(obj);
			else if ( obj.GetType() == Type.GetType("System.Boolean") )
				return (Int32) (Convert.ToBoolean(obj) ? 1 : 0) ;
			else if ( obj.GetType() == Type.GetType("System.Single") )
				return Convert.ToInt32(Math.Floor((System.Single) obj)) ;
			string str = obj.ToString();
			if ( str == String.Empty )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return Int32.Parse(str, NumberStyles.Any);
			Int32 nValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			Int32.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out nValue);
			return nValue;
		}

		public static long ToLong(long n)
		{
			return n;
		}

		public static long ToLong(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return 0;
			// If datatype is Integer, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Int64") )
				return Convert.ToInt64(obj);
			string str = obj.ToString();
			if ( str == String.Empty )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return Int64.Parse(str, NumberStyles.Any);
			Int64 nValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			Int64.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out nValue);
			return nValue;
		}

		// 10/22/2013   The Twitter table uses bigint, which we defined years ago as Int64. 
		public static long ToInt64(long n)
		{
			return n;
		}

		public static long ToInt64(object obj)
		{
			return ToLong(obj);
		}

		public static short ToShort(short n)
		{
			return n;
		}

		public static short ToShort(int n)
		{
			return (short) n;
		}

		public static short ToShort(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return 0;
			// 12/02/2005   Still need to convert object to Int16. Cast to short will not work. 
			if ( obj.GetType() == Type.GetType("System.Int32") || obj.GetType() == Type.GetType("System.Int16") )
				return Convert.ToInt16(obj);
			string str = obj.ToString();
			if ( str == String.Empty )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return Int16.Parse(str, NumberStyles.Any);
			Int16 nValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			Int16.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out nValue);
			return nValue;
		}

		public static object ToDBInteger(Int32 n)
		{
			return n;
		}

		public static object ToDBInteger(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			// If datatype is Integer, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Int32") )
				return obj;
			string str = obj.ToString();
			if ( str == String.Empty || !Information.IsNumeric(str))
				return DBNull.Value;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return Int32.Parse(str, NumberStyles.Any);
			Int32 nValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			Int32.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out nValue);
			return nValue;
		}

		// 10/24/2013   We need DBLong to handle a Twitter ID. 
		public static object ToDBLong(Int32 n)
		{
			return Convert.ToInt64(n);
		}

		public static object ToDBLong(Int64 n)
		{
			return n;
		}

		public static object ToDBLong(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			if ( obj.GetType() == Type.GetType("System.Int64") )
				return obj;
			string str = obj.ToString();
			if ( str == String.Empty || !Information.IsNumeric(str))
				return DBNull.Value;
			Int64 nValue = 0;
			Int64.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out nValue);
			return nValue;
		}

		public static float ToFloat(float f)
		{
			return f;
		}

		public static float ToFloat(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return 0;
			// If datatype is Double, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Double") )
				return (float) Convert.ToSingle(obj);
			string str = obj.ToString();
			if ( str == String.Empty || !Information.IsNumeric(str) )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return float.Parse(str, NumberStyles.Any);
			float fValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			float.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out fValue);
			return fValue;
		}

		public static float ToFloat(string str)
		{
			if ( str == null )
				return 0;
			if ( str == String.Empty || !Information.IsNumeric(str) )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return float.Parse(str, NumberStyles.Any);
			float fValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			float.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out fValue);
			return fValue;
		}

		public static object ToDBFloat(float f)
		{
			return f;
		}

		public static object ToDBDouble(double f)
		{
			return f;
		}

		public static object ToDBFloat(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			// If datatype is Double, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Double") )
				return obj;
			string str = obj.ToString();
			if ( str == String.Empty || !Information.IsNumeric(str) )
				return DBNull.Value;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return float.Parse(str, NumberStyles.Any);
			float fValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			float.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out fValue);
			return fValue;
		}


		public static double ToDouble(double d)
		{
			return d;
		}

		public static double ToDouble(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return 0;
			// If datatype is Double, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Double") )
				return Convert.ToDouble(obj);
			string str = obj.ToString();
			if ( str == String.Empty || !Information.IsNumeric(str) )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return double.Parse(str, NumberStyles.Any);
			double dValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			double.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out dValue);
			return dValue;
		}

		public static double ToDouble(string str)
		{
			if ( str == null )
				return 0;
			if ( str == String.Empty || !Information.IsNumeric(str) )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return double.Parse(str, NumberStyles.Any);
			double dValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			double.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out dValue);
			return dValue;
		}


		public static Decimal ToDecimal(Decimal d)
		{
			return d;
		}

		public static Decimal ToDecimal(double d)
		{
			return Convert.ToDecimal(d);
		}

		public static Decimal ToDecimal(float f)
		{
			return Convert.ToDecimal(f);
		}

		public static Decimal ToDecimal(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return 0;
			// If datatype is Decimal, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Decimal") )
				return Convert.ToDecimal(obj);
			string str = obj.ToString();
			if ( str == String.Empty )
				return 0;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return Decimal.Parse(str, NumberStyles.Any);
			Decimal dValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			Decimal.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out dValue);
			return dValue;
		}

		public static object ToDBDecimal(Decimal d)
		{
			return d;
		}

		public static object ToDBDecimal(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			// If datatype is Decimal, then nothing else is necessary. 
			if ( obj.GetType() == Type.GetType("System.Decimal") )
				return obj;
			string str = obj.ToString();
			if ( str == String.Empty || !Information.IsNumeric(str) )
				return DBNull.Value;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return Decimal.Parse(str, NumberStyles.Any);
			Decimal dValue = 0;
			// 03/16/2011   We need to allow any style so that separators will get ignored. 
			Decimal.TryParse(str, NumberStyles.Any, System.Threading.Thread.CurrentThread.CurrentCulture, out dValue);
			return dValue;
		}


		public static Boolean ToBoolean(Boolean b)
		{
			return b;
		}

		public static Boolean ToBoolean(Int32 n)
		{
			return (n == 0) ? false : true ;
		}

		public static Boolean ToBoolean(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return false;
			if ( obj.GetType() == Type.GetType("System.Int32") )
				return (Convert.ToInt32(obj) == 0) ? false : true ;
			// 01/15/2007   Allow a Byte field to also be treated as a boolean. 
			if ( obj.GetType() == Type.GetType("System.Byte") )
				return (Convert.ToByte(obj) == 0) ? false : true ;
			// 12/19/2005   MySQL 5 returns SByte for a TinyInt. 
			if ( obj.GetType() == Type.GetType("System.SByte") )
				return (Convert.ToSByte(obj) == 0) ? false : true ;
			// 12/17/2005   Oracle returns booleans as Int16. 
			if ( obj.GetType() == Type.GetType("System.Int16") )
				return (Convert.ToInt16(obj) == 0) ? false : true ;
			// 03/06/2006   Oracle returns SYNC_CONTACT as decimal.
			if ( obj.GetType() == Type.GetType("System.Decimal") )
				return (Convert.ToDecimal(obj) == 0) ? false : true ;
			if ( obj.GetType() == Type.GetType("System.String") )
			{
				string s = obj.ToString().ToLower();
				return (s == "true" || s == "on" || s == "1") ? true : false ;
			}
			if ( obj.GetType() != Type.GetType("System.Boolean") )
				return false;
			// 03/05/2011   Lets start using TryParse to protect against non-numeric strings. 
			// This should prevent ugly exceptions when an alpha string is used. 
			//return bool.Parse(obj.ToString());
			bool bValue = false;
			bool.TryParse(obj.ToString(), out bValue);
			return bValue;
		}

		public static object ToDBBoolean(Boolean b)
		{
			// 03/22/2006   DB2 requires that we convert the boolean to an integer.  It makes sense to do so for all platforms. 
			return b ? 1 : 0;
		}

		public static object ToDBBoolean(object obj)
		{
			if ( obj == null || obj == DBNull.Value )
				return DBNull.Value;
			if ( obj.GetType() != Type.GetType("System.Boolean") )
			{
				// 10/01/2006   Return 0 instead of false, as false can be converted to text. 
				string s = obj.ToString().ToLower();
				return (s == "true" || s == "on" || s == "1") ? 1 : 0 ;
			}
			// 03/22/2006   DB2 requires that we convert the boolean to an integer.  It makes sense to do so for all platforms. 
			return Convert.ToBoolean(obj) ? 1 : 0;
		}

		// 07/24/2010   We need a way to specify collation in a query. 
		// Oracle, IBM DB2 and PostgreSQL are case-significant, so we only need to deal with collation on SQL Server and MySQL. 
		// 07/24/2010   Instead of managing collation in code, it is better to change the collation on the field in the database. 
		/*
		public static string CaseSensitiveCollation(IDbConnection con)
		{
			string sCollation = String.Empty;
			if ( IsSQLServer(con) )
				sCollation = " collate SQL_Latin1_General_CP1_CS_AS";
			// http://dev.mysql.com/doc/refman/5.0/en/charset-collate.html
			else if ( IsMySQL(con) )
				sCollation = " collate latin1_bin";
			return sCollation;
		}
		*/

		public static bool IsSQLServer(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "System.Data.SqlClient.SqlCommand") ;
		}

		public static bool IsSQLServer(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "System.Data.SqlClient.SqlConnection") ;
		}

		public static bool IsOracleDataAccess(IDbCommand cmd)
		{
			// 08/15/2005   Type.GetType("Oracle.DataAccess.Client.OracleCommand") is returning NULL.  Use FullName instead. 
			return (cmd != null) && (cmd.GetType().FullName == "Oracle.DataAccess.Client.OracleCommand") ;
		}

		public static bool IsOracleDataAccess(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "Oracle.DataAccess.Client.OracleConnection") ;
		}

		public static bool IsOracleSystemData(IDbCommand cmd)
		{
			// 08/15/2005   Type.GetType("Oracle.DataAccess.Client.OracleCommand") is returning NULL.  Use FullName instead. 
			return (cmd != null) && (cmd.GetType().FullName == "System.Data.OracleClient.OracleCommand") ;
		}

		public static bool IsOracleSystemData(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "System.Data.OracleClient.OracleConnection") ;
		}

		public static bool IsOracle(IDbCommand cmd)
		{
			return IsOracleDataAccess(cmd) || IsOracleSystemData(cmd);
		}

		public static bool IsOracle(IDbConnection con)
		{
			return IsOracleDataAccess(con) || IsOracleSystemData(con);
		}

		// 08/29/2008   Allow testing of PostgreSQL. 
		public static bool IsPostgreSQL(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "Npgsql.NpgsqlCommand") ;
		}

		public static bool IsPostgreSQL(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "Npgsql.NpgsqlConnection") ;
		}

		public static bool IsMySQL(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "MySql.Data.MySqlClient.MySqlCommand") ;
		}

		public static bool IsMySQL(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "MySql.Data.MySqlClient.MySqlConnection") ;
		}

		public static bool IsDB2(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "IBM.Data.DB2.DB2Command") ;
		}

		public static bool IsDB2(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "IBM.Data.DB2.DB2Connection") ;
		}

		public static bool IsSqlAnywhere(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "iAnywhere.Data.AsaClient.AsaCommand") ;
		}

		public static bool IsSqlAnywhere(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "iAnywhere.Data.AsaClient.AsaConnection") ;
		}

		public static bool IsSybase(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "Sybase.Data.AseClient.AseCommand") ;
		}

		public static bool IsSybase(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "Sybase.Data.AseClient.AseConnection") ;
		}

		// 09/12/2010   Add support for EffiProz. 
		public static bool IsEffiProz(IDbCommand cmd)
		{
			return (cmd != null) && (cmd.GetType().FullName == "System.Data.EffiProz.EfzCommand") ;
		}

		public static bool IsEffiProz(IDbConnection con)
		{
			return (con != null) && (con.GetType().FullName == "System.Data.EffiProz.EfzConnection") ;
		}

		// 09/06/2008   PostgreSQL does not require that we stream the bytes, so lets explore doing this for all platforms. 
		// 09/20/2010   EffiProz does not require that we stream the bytes, so lets explore doing this for all platforms. 
		public static bool StreamBlobs(IDbConnection con)
		{
			if      ( IsPostgreSQL      (con) ) return false;
			else if ( IsSQLServer       (con) ) return false;
			else if ( IsEffiProz        (con) ) return false;
			else if ( IsOracleDataAccess(con) ) return true;
			else if ( IsOracleSystemData(con) ) return true;
			else if ( IsDB2             (con) ) return true;
			else if ( IsMySQL           (con) ) return true;
			else if ( IsSqlAnywhere     (con) ) return true;
			else if ( IsSybase          (con) ) return true;
			return true;
		}

		public static string ExpandParameters(IDbCommand cmd)
		{
			try
			{
				if ( cmd.CommandType == CommandType.Text )
				{
					string sSql = cmd.CommandText;
					CultureInfo ciEnglish = CultureInfo.CreateSpecificCulture("en-US");
					foreach(IDbDataParameter par in cmd.Parameters)
					{
						if ( par.Value == null || par.Value == DBNull.Value )
						{
							sSql = sSql.Replace(par.ParameterName, "null");
						}
						else
						{
							switch ( par.DbType )
							{
								case DbType.Boolean:
									// 04/26/2008   DbType.Boolean is used by SQL Server. 
									// 07/24/2009   Use 1 or 0 instead of true/false. 
									sSql = sSql.Replace(par.ParameterName, Sql.ToBoolean(par.Value) ? "1" : "0");
									break;
								case DbType.Int16:
									// 03/22/2006   DbType.Boolean gets saved as DbType.Int16 (when using DB2). 
									sSql = sSql.Replace(par.ParameterName, par.Value.ToString());
									break;
								case DbType.Int32:
									sSql = sSql.Replace(par.ParameterName, par.Value.ToString());
									break;
								case DbType.Int64:
									sSql = sSql.Replace(par.ParameterName, par.Value.ToString());
									break;
								case DbType.Decimal:
									sSql = sSql.Replace(par.ParameterName, par.Value.ToString());
									break;
								case DbType.DateTime:
									// 01/21/2006   Brazilian culture is having a problem with date formats.  Try using the european format yyyy/MM/dd HH:mm:ss. 
									// 06/13/2006   Italian has a problem with the time separator.  Use the value from the culture from CalendarControl.SqlDateTimeFormat. 
									// 06/14/2006   The Italian problem was that it was using the culture separator, but DataView only supports the en-US format. 
									sSql = sSql.Replace(par.ParameterName, "\'" + Convert.ToDateTime(par.Value).ToString(CalendarControl.SqlDateTimeFormat, ciEnglish.DateTimeFormat) + "\'");
									break;
								default:
									if ( Sql.IsEmptyString(par.Value) )
										sSql = sSql.Replace(par.ParameterName, "null");
									else
										sSql = sSql.Replace(par.ParameterName, "\'" + par.Value.ToString() + "\'");
									break;
							}
						}
					}
					return sSql;
				}
				else if ( cmd.CommandType == CommandType.StoredProcedure )
				{
					StringBuilder sbSql = new StringBuilder();
					sbSql.Append(cmd.CommandText);
					int nParameterIndex = 0;
					if ( IsOracle(cmd) || Sql.IsDB2(cmd) || IsPostgreSQL(cmd) )
						sbSql.Append("(");
					else
						sbSql.Append(" ");

					CultureInfo ciEnglish = CultureInfo.CreateSpecificCulture("en-US");
					foreach(IDbDataParameter par in cmd.Parameters)
					{
						if ( nParameterIndex > 0 )
							sbSql.Append(", ");
						if ( par.Value == null || par.Value == DBNull.Value )
						{
							sbSql.Append("null");
						}
						else
						{
							switch ( par.DbType )
							{
								case DbType.Boolean:
									// 04/26/2008   DbType.Boolean is used by SQL Server. 
									// 07/24/2009   Use 1 or 0 instead of true/false. 
									sbSql.Append(Sql.ToBoolean(par.Value) ? "1" : "0");
									break;
								case DbType.Int16:
									// 03/22/2006   DbType.Boolean gets saved as DbType.Int16 (when using DB2). 
									sbSql.Append(par.Value.ToString());
									break;
								case DbType.Int32:
									sbSql.Append(par.Value.ToString());
									break;
								case DbType.Int64:
									sbSql.Append(par.Value.ToString());
									break;
								case DbType.Decimal:
									sbSql.Append(par.Value.ToString());
									break;
								case DbType.DateTime:
									// 01/21/2006   Brazilian culture is having a problem with date formats.  Try using the european format yyyy/MM/dd HH:mm:ss. 
									// 06/13/2006   Italian has a problem with the time separator.  Use the value from the culture from CalendarControl.SqlDateTimeFormat. 
									// 06/14/2006   The Italian problem was that it was using the culture separator, but DataView only supports the en-US format. 
									sbSql.Append("\'" + Convert.ToDateTime(par.Value).ToString(CalendarControl.SqlDateTimeFormat, ciEnglish.DateTimeFormat) + "\'");
									break;
								default:
									if ( Sql.IsEmptyString(par.Value) )
										sbSql.Append("null");
									else
										sbSql.Append("\'" + par.Value.ToString() + "\'");
									break;
							}
						}
						nParameterIndex++;
					}
					if ( IsOracle(cmd) || Sql.IsDB2(cmd) || IsPostgreSQL(cmd))
						sbSql.Append(");");
					return sbSql.ToString();
				}
			}
			catch
			{
			}
			return cmd.CommandText;
		}

		public static string ClientScriptBlock(IDbCommand cmd)
		{
			// 90/05/2009   Apply SQL statement separator to all dumped SQL strings. 
			string sSQL = Sql.ExpandParameters(cmd);
			if ( sSQL.EndsWith(ControlChars.CrLf) )
				sSQL = sSQL.Substring(0, sSQL.Length - 2) + ";" + ControlChars.CrLf;
			else
				sSQL += ";";
			return "<script type=\"text/javascript\">sDebugSQL += '" + Sql.EscapeJavaScript(sSQL) + "';</script>";
		}

		// 07/18/2006   SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
		public enum SqlFilterMode
		{
			  Exact
			, StartsWith
			, Contains
		}

		public static IDbDataParameter FindParameter(IDbCommand cmd, string sName)
		{
			IDbDataParameter par = null;
			// 12/17/2005   Convert the name to Oracle or MySQL parameter format. 
			if ( !sName.StartsWith("@") )
				sName = "@" + sName;
			sName = CreateDbName(cmd, sName.ToUpper());
			if ( cmd.Parameters.Contains(sName) )
			{
				par = cmd.Parameters[sName] as IDbDataParameter;
			}
			return par;
		}

		public static void SetParameter(IDbDataParameter par, string sValue)
		{
			if ( par != null )
			{
				switch ( par.DbType )
				{
					// 01/20/2011   Should be using ToDBGuid(). 
					case DbType.Guid    : par.Value = Sql.ToDBGuid    (sValue);  break;
					case DbType.Int16   : par.Value = Sql.ToDBInteger (sValue);  break;
					case DbType.Int32   : par.Value = Sql.ToDBInteger (sValue);  break;
					case DbType.Int64   : par.Value = Sql.ToDBInteger (sValue);  break;
					case DbType.Double  : par.Value = Sql.ToDBFloat   (sValue);  break;
					case DbType.Decimal : par.Value = Sql.ToDBDecimal (sValue);  break;
					case DbType.Byte    : par.Value = Sql.ToDBBoolean (sValue);  break;
					// 10/01/2006   DB2 wants to use the Boolean data type. 
					case DbType.Boolean : par.Value = Sql.ToDBBoolean (sValue);  break;
					case DbType.DateTime: par.Value = Sql.ToDBDateTime(sValue);  break;
					//case DbType.Binary  : ;  par.Size = nLength;  break;
					default             :
						// 01/09/2006   use ToDBString. 
						par.Value = Sql.ToDBString(sValue);
						// 09/04/2010   The Size should already be set, even for custom fields. 
						// Allowing the size to be set will allow a trunctation error during import. 
						// "Error: String or binary data would be truncated. The statement has been terminated."
						// Only update the size if it is zero. 
						if ( par.Size == 0 )
							par.Size  = sValue.Length;
						break;
				}
			}
		}

		// 09/17/2013   Add Business Rules to import. 
		public static void SetParameter(IDbDataParameter par, object oValue)
		{
			if ( par != null )
			{
				switch ( par.DbType )
				{
					case DbType.Guid    : par.Value = Sql.ToDBGuid    (oValue);  break;
					case DbType.Int16   : par.Value = Sql.ToDBInteger (oValue);  break;
					case DbType.Int32   : par.Value = Sql.ToDBInteger (oValue);  break;
					case DbType.Int64   : par.Value = Sql.ToDBInteger (oValue);  break;
					case DbType.Double  : par.Value = Sql.ToDBFloat   (oValue);  break;
					case DbType.Decimal : par.Value = Sql.ToDBDecimal (oValue);  break;
					case DbType.Byte    : par.Value = Sql.ToDBBoolean (oValue);  break;
					case DbType.Boolean : par.Value = Sql.ToDBBoolean (oValue);  break;
					case DbType.DateTime: par.Value = Sql.ToDBDateTime(oValue);  break;
					default             :
						par.Value = Sql.ToDBString(oValue);
						if ( par.Size == 0 )
							par.Size  = Sql.ToString(oValue).Length;
						break;
				}
			}
		}

		public static void SetParameter(IDbCommand cmd, string sName, string sValue)
		{
			IDbDataParameter par = FindParameter(cmd, sName);
			if ( par != null )
			{
				// 10/30/2008   PostgreSQL has issues treating integers as booleans and booleans as integers. 
				// When importing records, we need to fix the parameters so that we send to PostgreSQL an integer data type. 
				if ( IsPostgreSQL(cmd) && par.DbType == DbType.Boolean )
					par.DbType = DbType.Int32;
				SetParameter(par, sValue);
			}
		}

		// 04/04/2006   SOAP needs a way to set a DateTime that has already been converted to server time. 
		public static void SetParameter(IDbCommand cmd, string sName, DateTime dtValueInServerTime)
		{
			IDbDataParameter par = FindParameter(cmd, sName);
			if ( par != null )
			{
				par.Value = Sql.ToDBDateTime(dtValueInServerTime);
			}
		}

		// 09/19/2006   Import needs an easier way to set a Guid parameter. 
		public static void SetParameter(IDbCommand cmd, string sName, Guid gValue)
		{
			IDbDataParameter par = FindParameter(cmd, sName);
			if ( par != null )
			{
				if ( Sql.IsEmptyGuid(gValue) )
				{
					par.Value = DBNull.Value;
				}
				else
				{
					if ( IsSQLServer(cmd) || Sql.IsSqlAnywhere(cmd) )
						par.Value = Sql.ToDBGuid(gValue);
					else
						par.Value = gValue.ToString().ToUpper();
				}
			}
		}

		// 02/02/2010   Import needs to be able to set an Integer. 
		public static void SetParameter(IDbCommand cmd, string sName, int nValue)
		{
			IDbDataParameter par = FindParameter(cmd, sName);
			if ( par != null )
			{
				par.Value = Sql.ToDBInteger(nValue);
			}
		}

		// 08/22/2011   Send Campaigns needs to be able to set a boolean. 
		public static void SetParameter(IDbCommand cmd, string sName, bool bValue)
		{
			IDbDataParameter par = FindParameter(cmd, sName);
			if ( par != null )
			{
				par.Value = Sql.ToDBBoolean(bValue);
			}
		}

		public static IDbCommand CreateInsertParameters(IDbConnection con, string sTABLE_NAME)
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			IDbCommand cmdInsert = con.CreateCommand() ;
			using ( DataTable dt = new DataTable() )
			{
				string sSQL;
				// 09/28/2006   DELETED, DATE_ENTERED and DATE_MODIFIED should use default values. 
				// 02/29/2008 Niall.  Some SQL Server 2005 installations require matching case for the parameters. 
				// Since we force the parameter to be uppercase, we must also make it uppercase in the command text. 
				sSQL = "select *                       " + ControlChars.CrLf
				     + "  from vwSqlColumns            " + ControlChars.CrLf
				     + " where ObjectName = @OBJECTNAME" + ControlChars.CrLf
				     + "   and ObjectType = 'U'        " + ControlChars.CrLf
				     + "   and ColumnName not in ('DELETED', 'DATE_ENTERED', 'DATE_MODIFIED')" + ControlChars.CrLf
				     + " order by colid                " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					// 09/02/2008   Standardize the case of metadata tables to uppercase.  PostgreSQL defaults to lowercase. 
					Sql.AddParameter(cmd, "@OBJECTNAME", Sql.MetadataName(cmd, sTABLE_NAME));
					using ( DbDataAdapter da = dbf.CreateDataAdapter() )
					{
						((IDbDataAdapter)da).SelectCommand = cmd;
						da.Fill(dt);
					}
				}
				// Build the command text.  This is necessary in order for the parameter function
				// to properly replace the @ symbol with the database-specific token. 
				StringBuilder sb = new StringBuilder();
				sb.Append("insert into " + sTABLE_NAME + "(");
				for( int i=0 ; i < dt.Rows.Count; i++ )
				{
					DataRow row = dt.Rows[i];
					if ( i > 0 )
						sb.Append(", ");
					sb.Append(Sql.ToString (row["ColumnName"]));
				}
				sb.AppendLine(")");
				sb.AppendLine("values(");
				for( int i=0 ; i < dt.Rows.Count; i++ )
				{
					DataRow row = dt.Rows[i];
					if ( i > 0 )
						sb.Append(", ");
					// 12/20/2005   Need to use the correct parameter token. 
					sb.Append(CreateDbName(cmdInsert, "@" + Sql.ToString (row["ColumnName"])));
				}
				sb.AppendLine(")");
				cmdInsert.CommandText = sb.ToString();
				
				foreach ( DataRow row in dt.Rows )
				{
					string sName      = Sql.ToString (row["ColumnName"]);
					string sCsType    = Sql.ToString (row["CsType"    ]);
					int    nLength    = Sql.ToInteger(row["length"    ]);
					IDbDataParameter par = Sql.CreateParameter(cmdInsert, "@" + sName, sCsType, nLength);
				}
			}
			return cmdInsert;
		}

		// 09/28/2006   Grant public access to CreateDbName so that we can access from the import function. 
		public static string CreateDbName(IDbCommand cmd, string sField)
		{
			// 09/06/2008   @ seems to work for PostgreSQL, but the manual mentions the colon. 
			if ( IsOracle(cmd) || IsPostgreSQL(cmd) )
			{
				sField = sField.Replace("@", ":");
			}
			else if ( IsMySQL(cmd) )
			{
				// 12/20/2005   The MySQL provider makes the parameter names upper case.  
				sField = sField.Replace("@", "?IN_").ToUpper();
			}
			else if ( IsSqlAnywhere(cmd) )
			{
				// 04/21/2006   SQL Anywhere does not support named parameters. 
				// http://www.ianywhere.com/developer/product_manuals/sqlanywhere/0902/en/html/dbpgen9/00000527.htm
				// The Adaptive Server Anywhere .NET data provider uses positional parameters that are marked with a question mark (?) instead of named parameters.
				sField = "?";
			}
			// 09/12/2010   EffiProz stored procedure field names start with IN_. 
			// 09/15/2010   EffiProz now supports @. 
			/*
			else if ( IsEffiProz(cmd) )
			{
				sField = sField.Replace("@", "@IN_").ToUpper();
			}
			*/
			return sField;
		}

		public static string ExtractDbName(IDbCommand cmd, string sParameterName)
		{
			string sField = sParameterName;
			// 09/06/2008   @ seems to work for PostgreSQL, but the manual mentions the colon. 
			if ( IsOracle(cmd) || IsPostgreSQL(cmd) )
			{
				if ( sField.StartsWith(":") )
					sField = sField.Substring(1);
			}
			else if ( IsOracleSystemData(cmd) )
			{
				if ( cmd.CommandType == CommandType.Text )
				{
					if ( sField.StartsWith(":") )
						sField = sField.Substring(1);
				}
				else
				{
					if ( sField.StartsWith("IN_") )
						sField = sField.Substring(3);
				}
			}
			else if ( IsMySQL(cmd) )
			{
				if ( sField.StartsWith("?IN_") )
					sField = sField.Substring(4);
			}
			else
			{
				if ( sField.StartsWith("@") )
					sField = sField.Substring(1);
			}
			return sField;
		}

		// 12/17/2008   We need to be able to create a parameter while importing. 
		public static IDbDataParameter CreateParameter(IDbCommand cmd, string sField)
		{
			IDbDataParameter par = cmd.CreateParameter();
			if ( par == null )
			{
				// 10/14/2005  MySql is not returning a value from CreateParameter.  It will have to be created from the factory. 
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				par = dbf.CreateParameter();
			}
			// 08/13/2005   Oracle uses a different ParameterToken. 
			// 09/06/2008   @ seems to work for PostgreSQL, but the manual mentions the colon. 
			if ( IsOracleDataAccess(cmd) || IsPostgreSQL(cmd) )
			{
				sField = sField.Replace("@", ":");
				if ( cmd.CommandType == CommandType.Text )
					cmd.CommandText = cmd.CommandText.Replace("@", ":");
			}
			else if ( IsOracleSystemData(cmd) )
			{
				if ( cmd.CommandType == CommandType.Text )
				{
					// 08/03/2006   System.Data.OracleClient requires the colon for Text parameters. 
					sField = sField.Replace("@", ":");
					cmd.CommandText = cmd.CommandText.Replace("@", ":");
				}
				else
				{
					// 08/03/2006   System.Data.OracleClient does not like the colon in the parameter name, but the name must match precicely. 
					// All Taoqi parameter names for Oracle start with IN_. 
					sField = sField.Replace("@", "IN_");
				}
			}
				// 10/18/2005   MySQL uses a different ParameterToken. 
			else if ( IsMySQL(cmd) )
			{
				// 12/20/2005   The MySQL provider makes the parameter names upper case.  
				sField = sField.Replace("@", "?IN_").ToUpper();
				if ( cmd.CommandType == CommandType.Text )
					cmd.CommandText = cmd.CommandText.Replace("@", "?IN_");
			}
			else if ( IsSqlAnywhere(cmd) )
			{
				// 04/21/2006   SQL Anywhere does not support named parameters. Replace with ?.
				// http://www.ianywhere.com/developer/product_manuals/sqlanywhere/0902/en/html/dbpgen9/00000527.htm
				// The Adaptive Server Anywhere .NET data provider uses positional parameters that are marked with a question mark (?) instead of named parameters.
				cmd.CommandText = cmd.CommandText.Replace(sField.ToUpper(), "?");
			}
			// 09/12/2010   EffiProz stored procedure field names start with IN_. 
			// 09/15/2010   EffiProz now supports @. 
			/*
			else if ( IsEffiProz(cmd) )
			{
				if ( cmd.CommandType == CommandType.Text )
					cmd.CommandText = cmd.CommandText.Replace(sField, sField.Replace("@", "@IN_"));
				sField = sField.Replace("@", "@IN_").ToUpper();
			}
			*/
			// 12/17/2005   Make the parameter name uppercase so that it can be easily found in the SetParameter function. 
			par.ParameterName = sField.ToUpper();
			cmd.Parameters.Add(par);
			return par;
		}
		
		public static IDbDataParameter CreateParameter(IDbCommand cmd, string sField, string sCsType, int nLength)
		{
			IDbDataParameter par = Sql.CreateParameter(cmd, sField);
			switch ( sCsType )
			{
				case "Guid"    :
					// 09/12/2010   EffiProz supports Guids. 
					if ( Sql.IsSQLServer(cmd) || Sql.IsSqlAnywhere(cmd) || Sql.IsEffiProz(cmd) )
					{
						par.DbType        = DbType.Guid;
					}
					else
					{
						// 08/11/2005   Oracle does not support Guids, nor does MySQL. 
						par.DbType        = DbType.String;
						par.Size          = 36;  // 08/13/2005   Only set size for variable length fields. 
					}
					break;
				case "short"     :  par.DbType        = DbType.Int16     ;  break;
				case "Int32"     :  par.DbType        = DbType.Int32     ;  break;
				case "Int64"     :  par.DbType        = DbType.Int64     ;  break;
				case "float"     :  par.DbType        = DbType.Double    ;  break;
				case "decimal"   :  par.DbType        = DbType.Decimal   ;  break;
				case "bool"      :
					// 10/01/2006   DB2 seems to prefer Boolean.  Oracle wants Byte.  
					// We are going to use Boolean for all but Oracle as this what we have tested extensively in the AddParameter(,,bool) function below. 
					if ( Sql.IsOracle(cmd) )
						par.DbType        = DbType.Byte   ;
					else
						par.DbType        = DbType.Boolean;
					break;
				case "DateTime"  :  par.DbType        = DbType.DateTime  ;  break;
				case "byte[]"    :  par.DbType        = DbType.Binary    ;  par.Size = nLength;  break;
				// 01/24/2006   A severe error occurred on the current command. The results, if any, should be discarded. 
				// MS03-031 security patch causes this error because of stricter datatype processing.  
				// http://www.microsoft.com/technet/security/bulletin/MS03-031.mspx.
				// http://support.microsoft.com/kb/827366/
				case "ansistring":  par.DbType        = DbType.AnsiString;  par.Size = nLength;  break;
				//case "string"  :  par.DbType        = DbType.String    ;  par.Size = nLength;  break;
				default          :  par.DbType        = DbType.String    ;  par.Size = nLength;  break;
			}
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, short nValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Int16;
			//par.Size          = 4;
			par.Value         = Sql.ToDBInteger(nValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, int nValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Int32;
			//par.Size          = 4;
			par.Value         = Sql.ToDBInteger(nValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, long nValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Int64;
			//par.Size          = 4;
			// 10/24/2013   ToDBInteger will truncate Int64 values, so make sure to use ToDBLong. 
			par.Value         = Sql.ToDBLong(nValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, float fValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Double;
			//par.Size          = 8;
			par.Value         = Sql.ToDBFloat(fValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, double fValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Double;
			//par.Size          = 8;
			par.Value         = Sql.ToDBDouble(fValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, Decimal dValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Decimal;
			//par.Size          = 8;
			par.Value         = Sql.ToDBDecimal(dValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, bool bValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			// 03/22/2006   Not sure why DbType.Byte was used when DbType.Boolean is available. 
			// 03/22/2006   DB2 requires that we convert the boolean to an integer.  It makes sense to do so for all platforms. 
			// 03/31/2006   Oracle does not like DbType.Boolean.  That must be why we used DbType.Byte.
			// 08/29/2008   PostgreSQL has issues treating integers as booleans and booleans as integers. 
			if ( IsOracle(cmd) )
			{
				par.DbType        = DbType.Byte;
				par.Value         = Sql.ToDBBoolean(bValue);
			}
			else if ( IsPostgreSQL(cmd) )
			{
				par.DbType        = DbType.Int32;
				//par.Size          = 4;
				par.Value         = bValue ? 1 : 0;
			}
			else
			{
				par.DbType        = DbType.Boolean;
				par.Value         = Sql.ToDBBoolean(bValue);
			}
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, Guid gValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			// 10/18/2005   SQL Server is the only one that accepts a native Guid data type. 
			// 09/12/2010   EffiProz supports Guids. 
			if ( IsSQLServer(cmd) || Sql.IsSqlAnywhere(cmd) || Sql.IsEffiProz(cmd) )
			{
				par.DbType        = DbType.Guid;
				//par.Size          = 16;
				par.Value         = Sql.ToDBGuid(gValue);
			}
			else
			{
				// 08/11/2005   Oracle does not support Guids, nor does MySQL. 
				// 04/09/2006   AnsiStringFixedLength is the most appropriate mapping.  
				// 04/09/2006   Sybase is having a problem, but this does not help. 
				par.DbType        = DbType.AnsiStringFixedLength;
				par.Size          = 36;  // 08/13/2005   Only set size for variable length fields. 
				if ( Sql.IsEmptyGuid(gValue) )
					par.Value     = DBNull.Value;
				else
					par.Value     = gValue.ToString().ToUpper();  // 08/15/2005   Guids are stored in Oracle in upper case. 
			}
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, DateTime dtValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.DateTime;
			//par.Size          = 8;
			par.Value         = Sql.ToDBDateTime(dtValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, string sValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.String;
			// 08/13/2005   Only set size for variable length fields. 
			// 07/17/2008   sValue can be NULL. 
			par.Size          = (sValue != null) ? sValue.Length : 0;
			par.Value         = Sql.ToDBString(sValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, string sValue, bool bAllowEmptyString)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.String;
			// 08/13/2005   Only set size for variable length fields. 
			// 07/17/2008   sValue can be NULL. 
			par.Size          = (sValue != null) ? sValue.Length : 0;
			// 09/20/2005   the SQL IN clause does not allow NULL. 
			par.Value         = bAllowEmptyString ? sValue : Sql.ToDBString(sValue);
			return par;
		}

		// 11/19/2012   Allow varchar(max). 
		public static IDbDataParameter AddAnsiParam(IDbCommand cmd, string sField, string sValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.AnsiString;
			par.Size          = (sValue != null) ? sValue.Length : 0;
			par.Value         = Sql.ToDBString(sValue);
			return par;
		}

		public static IDbDataParameter AddAnsiParam(IDbCommand cmd, string sField, string sValue, int nSize)
		{
			// 08/13/2005   Truncate the string if it exceeds the specified size. 
			// The field should have been validated on the client side, so this is just defensive programming. 
			// 10/09/2005  sValue can be null. 
			if ( sValue != null )
			{
				// 04/29/2008   Some custom fields have not been updating because the MAX_SIZE is 0. Use the actual string length. 
				if ( nSize == 0 )
					nSize = sValue.Length;
				else if ( sValue.Length > nSize )
					sValue = sValue.Substring(0, nSize);
			}
			// 01/24/2006   A severe error occurred on the current command. The results, if any, should be discarded. 
			// MS03-031 security patch causes this error because of stricter datatype processing.  
			// http://www.microsoft.com/technet/security/bulletin/MS03-031.mspx.
			// http://support.microsoft.com/kb/827366/
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.AnsiString;
			par.Size          = nSize;  // 08/13/2005   Only set size for variable length fields. 
			par.Value         = Sql.ToDBString(sValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, string sValue, int nSize)
		{
			// 08/13/2005   Truncate the string if it exceeds the specified size. 
			// The field should have been validated on the client side, so this is just defensive programming. 
			// 10/09/2005  sValue can be null. 
			if ( sValue != null )
			{
				// 04/29/2008   Some custom fields have not been updating because the MAX_SIZE is 0. Use the actual string length. 
				if ( nSize == 0 )
					nSize = sValue.Length;
				else if ( sValue.Length > nSize )
					sValue = sValue.Substring(0, nSize);
			}
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.String;
			par.Size          = nSize;  // 08/13/2005   Only set size for variable length fields. 
			par.Value         = Sql.ToDBString(sValue);
			return par;
		}

		public static IDbDataParameter AddParameter(IDbCommand cmd, string sField, byte[] byValue)
		{
			IDbDataParameter par = CreateParameter(cmd, sField);
			par.DbType        = DbType.Binary;
			// 07/06/2008   byValue might be NULL. 
			par.Size          = (byValue != null) ? byValue.Length : 0;  // 08/13/2005   Only set size for variable length fields. 
			par.Value         = Sql.ToDBBinary(byValue);
			return par;
		}

		public static void AppendParameter(IDbCommand cmd, int nValue, string sField, bool bIsEmpty)
		{
			if ( !bIsEmpty )
			{
				cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
				//cmd.Parameters.Add("@" + sField, SqlDbType.Int, 4).Value = nValue;
				Sql.AddParameter(cmd, "@" + sField, nValue);
			}
		}

		// 09/01/2006   Add Float parameter. 
		public static void AppendParameter(IDbCommand cmd, float fValue, string sField, bool bIsEmpty)
		{
			if ( !bIsEmpty )
			{
				cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
				//cmd.Parameters.Add("@" + sField, SqlDbType.Int, 4).Value = nValue;
				Sql.AddParameter(cmd, "@" + sField, fValue);
			}
		}

		public static void AppendParameter(IDbCommand cmd, Decimal dValue, string sField, bool bIsEmpty)
		{
			if ( !bIsEmpty )
			{
				cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
				//cmd.Parameters.Add("@" + sField, DbType.Decimal, 8).Value = dValue;
				Sql.AddParameter(cmd, "@" + sField, dValue);
			}
		}

		// 04/27/2008   The boolean AppendParameter now requires the IsEmpty flag. 
		// SearchView was the only place where the value was also used to determine if empty. 
		public static void AppendParameter(IDbCommand cmd, bool bValue, string sField, bool bIsEmpty)
		{
			if ( !bIsEmpty )
			{
				cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
				//cmd.Parameters.Add("@" + sField, DbType.Byte, 1).Value = bValue;
				Sql.AddParameter(cmd, "@" + sField, bValue);
			}
		}

		// 12/26/2006   We need to determine the next available placeholder name. 
		public static string NextPlaceholder(IDbCommand cmd, string sField)
		{
			int    nPlaceholderIndex = 0;
			string sFieldPlaceholder = sField;
			IDataParameter par = FindParameter(cmd, sFieldPlaceholder);
			while ( par != null )
			{
				// If the field placeholder exists, increment the index and search again. 
				nPlaceholderIndex++ ;
				sFieldPlaceholder = sField + nPlaceholderIndex.ToString();
				par = FindParameter(cmd, sFieldPlaceholder);
			}
			return sFieldPlaceholder;
		}

		public static void AppendParameter(IDbCommand cmd, Guid gValue, string sField)
		{
			// 12/26/2006   We are having a problem with the ASSIGNED_USER_ID that is set 
			// during an ACL filter and the same field being set in a search criteria. 
			// To solve the problem, create an alternate placeholder name. 
			string sFieldPlaceholder = NextPlaceholder(cmd, sField);
			// 02/05/2006   DB2 is the same as Oracle in that searches are case-significant. 

			// 09/18/2008   DB2 has an issue with using a placeholder in a function. 
			// http://bytes.com/forum/thread182742.html
			// If you read the rules for function resolution in the SQL Reference, you'll see that they are very sensitive to the data types of the parameters. 
			// Unfortunately, a parameter marker doesn't have a type when it is precompiled, so DB2 doesn't know what type to use and you get the error.
			// ERROR [42610] [IBM][DB2/NT] SQL0418N A statement contains a use of a parameter marker that is not valid. SQLSTATE=42610
			// 09/18/2008   Since .NET supplies the GUID in upper case, we can remove the upper() around the placeholder. 
			// In our new platforms, the GUID is stored in uppercase, so we don't need the upper() on either side. 
			if ( IsOracle(cmd) || Sql.IsDB2(cmd) || IsPostgreSQL(cmd) )
				cmd.CommandText += "   and " + sField + " = @" + sFieldPlaceholder + ControlChars.CrLf;
			else
				cmd.CommandText += "   and " + sField + " = @" + sFieldPlaceholder + ControlChars.CrLf;
			//cmd.Parameters.Add("@" + sField, DbType.Guid, 1).Value = gValue;
			Sql.AddParameter(cmd, "@" + sFieldPlaceholder, gValue);
		}

		public static void AppendParameter(IDbCommand cmd, Guid gValue, string sField, bool bIsEmpty)
		{
			if ( !bIsEmpty )
			{
				AppendParameter(cmd, gValue, sField);
			}
		}

		public static void AppendParameter(IDbCommand cmd, DateTime dtValue, string sField)
		{
			if ( dtValue != DateTime.MinValue )
			{
				cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
				//cmd.Parameters.Add("@" + sField, DbType.DateTime, 8).Value = dtValue;
				Sql.AddParameter(cmd, "@" + sField, dtValue);
			}
		}

		// 07/25/2006   Support the Between clause for dates. 
		public static void AppendParameter(IDbCommand cmd, DateTime dtValue1, DateTime dtValue2, string sField)
		{
			// 07/25/2006   The between clause is greater than or equal to Value1 and less than or equal to Value2.
			// We want the query to be less than Value2.
			//cmd.CommandText += "   and " + sField + " between @" + sField + "_1 and @" + sField + "_2" + ControlChars.CrLf;
			// 12/17/2007   Allow either date value to be NULL so that we can do greater than or less than searches. 
			if ( dtValue1 != DateTime.MinValue )
			{
				cmd.CommandText += "   and " + sField + " >= @" + sField + "_1" + ControlChars.CrLf;
				Sql.AddParameter(cmd, "@" + sField + "_1", dtValue1);
			}
			if ( dtValue2 != DateTime.MinValue )
			{
				cmd.CommandText += "   and " + sField + " <  @" + sField + "_2" + ControlChars.CrLf;
				Sql.AddParameter(cmd, "@" + sField + "_2", dtValue2);
			}
		}

		public static void AppendParameter(IDbCommand cmd, string sValue, string sField)
		{
			if ( !IsEmptyString(sValue) )
			{
				cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
				Sql.AddParameter(cmd, "@" + sField, sValue, sValue.Length);
			}
		}

		public static void AppendParameter(IDbCommand cmd, string sValue, SqlFilterMode mode, string sField)
		{
			if ( !IsEmptyString(sValue) )
			{
				if ( IsOracle(cmd) || Sql.IsDB2(cmd) || IsPostgreSQL(cmd) )
				{
					// 09/18/2008   DB2 has an issue with using a placeholder in a function. 
					// http://bytes.com/forum/thread182742.html
					// If you read the rules for function resolution in the SQL Reference, you'll see that they are very sensitive to the data types of the parameters. 
					// Unfortunately, a parameter marker doesn't have a type when it is precompiled, so DB2 doesn't know what type to use and you get the error.
					// ERROR [42610] [IBM][DB2/NT] SQL0418N A statement contains a use of a parameter marker that is not valid. SQLSTATE=42610
					// 09/18/2008   Since this is just for searching, we can insert the value in uppercase instead of using the upper() function on the right. 
					switch ( mode )
					{
						case SqlFilterMode.Exact:
							cmd.CommandText += "   and upper(" + sField + ") = @" + sField + ControlChars.CrLf;
							Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), sValue.Length);
							break;
						case SqlFilterMode.StartsWith:
							// 08/29/2005   Oracle uses || to concatenate strings. 
							cmd.CommandText += "   and upper(" + sField + ") like @" + sField + " || '%'" + ControlChars.CrLf;
							sValue = EscapeSQLLike(sValue);
							// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
							// 09/02/2008   PostgreSQL requires two slashes. 
							if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
							{
								sValue = sValue.Replace("\\", "\\\\");
								cmd.CommandText += " escape '\\\\'";
							}
							else
								cmd.CommandText += " escape '\\'";
							Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), sValue.Length);
							break;
						case SqlFilterMode.Contains:
							// 08/29/2005   Oracle uses || to concatenate strings. 
							cmd.CommandText += "   and upper(" + sField + ") like '%' || @" + sField + " || '%'" + ControlChars.CrLf;
							sValue = EscapeSQLLike(sValue);
							// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
							// 09/02/2008   PostgreSQL requires two slashes. 
							if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
							{
								sValue = sValue.Replace("\\", "\\\\");
								cmd.CommandText += " escape '\\\\'";
							}
							else
								cmd.CommandText += " escape '\\'";
							Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), sValue.Length);
							break;
					}
				}
				else
				{
					switch ( mode )
					{
						case SqlFilterMode.Exact:
							cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
							Sql.AddParameter(cmd, "@" + sField, sValue, sValue.Length);
							break;
						case SqlFilterMode.StartsWith:
							// 08/29/2005   SQL Server uses + to concatenate strings. 
							cmd.CommandText += "   and " + sField + " like @" + sField + " + '%'" + ControlChars.CrLf;
							sValue = EscapeSQLLike(sValue);
							// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
							// 09/02/2008   PostgreSQL requires two slashes. 
							if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
							{
								sValue = sValue.Replace("\\", "\\\\");
								cmd.CommandText += " escape '\\\\'";
							}
							else
								cmd.CommandText += " escape '\\'";
							Sql.AddParameter(cmd, "@" + sField, sValue, sValue.Length);
							break;
						case SqlFilterMode.Contains:
							// 08/29/2005   SQL Server uses + to concatenate strings. 
							cmd.CommandText += "   and " + sField + " like '%' + @" + sField + " + '%'" + ControlChars.CrLf;
							sValue = EscapeSQLLike(sValue);
							// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
							// 09/02/2008   PostgreSQL requires two slashes. 
							if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
							{
								sValue = sValue.Replace("\\", "\\\\");
								cmd.CommandText += " escape '\\\\'";
							}
							else
								cmd.CommandText += " escape '\\'";
							Sql.AddParameter(cmd, "@" + sField, sValue, sValue.Length);
							break;
					}
				}
			}
		}

		public static void AppendParameter(IDbCommand cmd, string sValue, int nSize, SqlFilterMode mode, string sField)
		{
			if ( !IsEmptyString(sValue) )
			{
				SearchBuilder sb = new SearchBuilder(sValue, cmd);
				// 08/15/2005   Oracle uses || to concatenate strings. 
				// Also use upper() to make the compares case insignificant. 
				// 02/05/2006   DB2 use || to concatenate strings.  
				// Also use upper() to make the compares case insignificant. 

				// 07/18/2006   SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
				if ( mode == SqlFilterMode.Contains )
					mode = SqlFilterMode.StartsWith ;
				if ( IsOracle(cmd) || Sql.IsDB2(cmd) || IsPostgreSQL(cmd) )
				{
					// 09/18/2008   DB2 has an issue with using a placeholder in a function. 
					// http://bytes.com/forum/thread182742.html
					// If you read the rules for function resolution in the SQL Reference, you'll see that they are very sensitive to the data types of the parameters. 
					// Unfortunately, a parameter marker doesn't have a type when it is precompiled, so DB2 doesn't know what type to use and you get the error.
					// ERROR [42610] [IBM][DB2/NT] SQL0418N A statement contains a use of a parameter marker that is not valid. SQLSTATE=42610
					// 09/18/2008   Since this is just for searching, we can insert the value in uppercase instead of using the upper() function on the right. 
					switch ( mode )
					{
						case SqlFilterMode.Exact:
							cmd.CommandText += "   and upper(" + sField + ") = @" + sField + ControlChars.CrLf;
							Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), nSize);
							break;
						case SqlFilterMode.StartsWith:
							cmd.CommandText += sb.BuildQuery("   and ", sField);
							break;
						/*
						case SqlFilterMode.Contains:
							// 08/29/2005   Oracle uses || to concatenate strings. 
							cmd.CommandText += "   and upper(" + sField + ") like '%' || @" + sField + " || '%'" + ControlChars.CrLf;
							sValue = EscapeSQLLike(sValue);
							// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
							// 09/02/2008   PostgreSQL requires two slashes. 
							if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
							{
								sValue = sValue.Replace("\\", "\\\\");
								cmd.CommandText += " escape '\\\\'";
							}
							else
								cmd.CommandText += " escape '\\'";
							Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), nSize);
							break;
						*/
					}
				}
				else
				{
					switch ( mode )
					{
						case SqlFilterMode.Exact:
							cmd.CommandText += "   and " + sField + " = @" + sField + ControlChars.CrLf;
							Sql.AddParameter(cmd, "@" + sField, sValue, nSize);
							break;
						case SqlFilterMode.StartsWith:
							cmd.CommandText += sb.BuildQuery("   and ", sField);
							break;
						/*
						case SqlFilterMode.Contains:
							// 08/29/2005   SQL Server uses + to concatenate strings. 
							cmd.CommandText += "   and " + sField + " like '%' + @" + sField + " + '%'" + ControlChars.CrLf;
							sValue = EscapeSQLLike(sValue);
							// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
							// 09/02/2008   PostgreSQL requires two slashes. 
							if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
							{
								sValue = sValue.Replace("\\", "\\\\");
								cmd.CommandText += " escape '\\\\'";
							}
							else
								cmd.CommandText += " escape '\\'";
							Sql.AddParameter(cmd, "@" + sField, sValue, nSize);
							break;
						*/
					}
				}
			}
		}

		public static void AppendParameter(IDbCommand cmd, string sValue, int nSize,  SqlFilterMode mode, string[] arrField)
		{
			if ( !IsEmptyString(sValue) )
			{
				SearchBuilder sb = new SearchBuilder(sValue, cmd);
				cmd.CommandText += "   and (1 = 0" + ControlChars.CrLf;
				// 08/15/2005   Oracle uses || to concatenate strings. 
				// Also use upper() to make the compares case insignificant. 
				// 02/05/2006   DB2 use || to concatenate strings.  
				// Also use upper() to make the compares case insignificant. 

				// 07/18/2006   SqlFilterMode.Contains behavior has be deprecated. It is now the same as SqlFilterMode.StartsWith. 
				if ( mode == SqlFilterMode.Contains )
					mode = SqlFilterMode.StartsWith ;
				if ( IsOracle(cmd) || Sql.IsDB2(cmd) || IsPostgreSQL(cmd) )
				{
					// 09/18/2008   DB2 has an issue with using a placeholder in a function. 
					// http://bytes.com/forum/thread182742.html
					// If you read the rules for function resolution in the SQL Reference, you'll see that they are very sensitive to the data types of the parameters. 
					// Unfortunately, a parameter marker doesn't have a type when it is precompiled, so DB2 doesn't know what type to use and you get the error.
					// ERROR [42610] [IBM][DB2/NT] SQL0418N A statement contains a use of a parameter marker that is not valid. SQLSTATE=42610
					// 09/18/2008   Since this is just for searching, we can insert the value in uppercase instead of using the upper() function on the right. 
					switch ( mode )
					{
						case SqlFilterMode.Exact:
							foreach ( string sField in arrField )
							{
								cmd.CommandText += "        or upper(" + sField + ") = @" + sField + ControlChars.CrLf;
								Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), nSize);
							}
							break;
						case SqlFilterMode.StartsWith:
							// 07/18/2006   We need to use SearchBuilder even when searching multiple fields, such as the PHONE fields. 
							foreach ( string sField in arrField )
							{
								cmd.CommandText += sb.BuildQuery("         or ", sField);
							}
							break;
						/*
						case SqlFilterMode.Contains:
							sValue = EscapeSQLLike(sValue);
							if ( IsMySQL(cmd) )
								sValue = sValue.Replace("\\", "\\\\");
							foreach ( string sField in arrField )
							{
								cmd.CommandText += "        or upper(" + sField + ") like '%' || @" + sField + " || '%'" + ControlChars.CrLf;
								// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
								// 09/02/2008   PostgreSQL requires two slashes. 
								if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
									cmd.CommandText += " escape '\\\\'";
								else
									cmd.CommandText += " escape '\\'";
								Sql.AddParameter(cmd, "@" + sField, sValue.ToUpper(), nSize);
							}
							break;
						*/
					}
				}
				else
				{
					switch ( mode )
					{
						case SqlFilterMode.Exact:
							foreach ( string sField in arrField )
							{
								cmd.CommandText += "        or " + sField + " = @" + sField + ControlChars.CrLf;
								Sql.AddParameter(cmd, "@" + sField, sValue, nSize);
							}
							break;
						case SqlFilterMode.StartsWith:
							// 07/18/2006   We need to use SearchBuilder even when searching multiple fields, such as the PHONE fields. 
							foreach ( string sField in arrField )
							{
								cmd.CommandText += sb.BuildQuery("         or ", sField);
							}
							break;
						/*
						case SqlFilterMode.Contains:
							sValue = EscapeSQLLike(sValue);
							if ( IsMySQL(cmd) )
								sValue = sValue.Replace("\\", "\\\\");
							foreach ( string sField in arrField )
							{
								cmd.CommandText += "        or " + sField + " like '%' + @" + sField + " + '%'" + ControlChars.CrLf;
								// 07/16/2006   MySQL requires that slashes be escaped, even in the escape clause. 
								// 09/02/2008   PostgreSQL requires two slashes. 
								if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
									cmd.CommandText += " escape '\\\\'";
								else
									cmd.CommandText += " escape '\\'";
								Sql.AddParameter(cmd, "@" + sField, sValue, nSize);
							}
							break;
						*/
					}
					cmd.CommandText += "       )" + ControlChars.CrLf;
				}
			}
		}

		public static void AppendParameter(IDbCommand cmd, ListControl lst, string sField)
		{
			int nCount = 0;
			StringBuilder sb = new StringBuilder();
			foreach(ListItem item in lst.Items)
			{
				if ( item.Selected && item.Value.Length > 0 )
				{
					// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
					if ( nCount > 0 )
						sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
					// 12/20/2005   Need to use the correct parameter token. 
					// 10/16/2006   Use a 3-char format string to prevent ExpandParamters from performing incomplete replacements. 
					sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("000")));
					//cmd.Parameters.Add("@" + sField + nCount.ToString("000"), DbType.Guid, 16).Value = item.Value;
					Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), item.Value);
					nCount++;
				}
			}
			if ( sb.Length > 0 )
			{
				cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
			}
		}

		// 09/02/2010   Add ability to add an array with nulls. 
		public static void AppendParameterWithNull(IDbCommand cmd, string[] arr, string sField)
		{
			if ( arr != null )
			{
				int nCount = 0;
				bool bIncludeNull = false;
				StringBuilder sb = new StringBuilder();
				foreach(string item in arr)
				{
					if ( item.Length > 0 )
					{
						// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
						if ( nCount > 0 )
							sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
						sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("000")));
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), item);
						nCount++;
					}
					else
					{
						bIncludeNull = true;
					}
				}
				if ( sb.Length > 0 )
				{
					if ( bIncludeNull )
						cmd.CommandText += "   and (" + sField +" is null or " + sField +" in (" + sb.ToString() + "))" + ControlChars.CrLf;
					else
						cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
				else if ( bIncludeNull )
				{
					cmd.CommandText += "   and " + sField +" is null" + ControlChars.CrLf;
				}
			}
		}

		public static void AppendParameterWithNull(IDbCommand cmd, ListControl lst, string sField)
		{
			int nCount = 0;
			bool bIncludeNull = false;
			StringBuilder sb = new StringBuilder();
			foreach(ListItem item in lst.Items)
			{
				if ( item.Selected )
				{
					if ( item.Value.Length > 0 )
					{
						// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
						if ( nCount > 0 )
							sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
						// 12/20/2005   Need to use the correct parameter token. 
						// 10/16/2006   Use a 3-char format string to prevent ExpandParamters from performing incomplete replacements. 
						sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("000")));
						//cmd.Parameters.Add("@" + sField + nCount.ToString("000"), DbType.Guid, 16).Value = item.Value;
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), item.Value);
						nCount++;
					}
					else
					{
						bIncludeNull = true;
					}
				}
			}
			if ( sb.Length > 0 )
			{
				if ( bIncludeNull )
					cmd.CommandText += "   and (" + sField +" is null or " + sField +" in (" + sb.ToString() + "))" + ControlChars.CrLf;
				else
					cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
			}
			else if ( bIncludeNull )
			{
				cmd.CommandText += "   and " + sField +" is null" + ControlChars.CrLf;
			}
		}

		// 01/27/2010   We need to make it easy to append a Guid array. 
		public static void AppendParameter(IDbCommand cmd, Guid[] arr, string sField)
		{
			if ( arr != null )
			{
				int nCount = 0;
				StringBuilder sb = new StringBuilder();
				foreach ( Guid item in arr )
				{
					if ( !Sql.IsEmptyGuid(item) )
					{
						// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
						if ( nCount > 0 )
							sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
						sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("0000")));
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("0000"), item);
						nCount++;
					}
				}
				if ( sb.Length > 0 )
				{
					cmd.CommandText += "   and ";
					cmd.CommandText += sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
			}
		}

		public static void AppendParameter(IDbCommand cmd, string[] arr, string sField)
		{
			AppendParameter(cmd, arr, sField, false);
		}

		public static void AppendParameter(IDbCommand cmd, string[] arr, string sField, bool bOrClause)
		{
			if ( arr != null )
			{
				int nCount = 0;
				StringBuilder sb = new StringBuilder();
				foreach(string item in arr)
				{
					// 09/20/2005  Allow an empty string to be a valid selection.
					//if ( item.Length > 0 )
					{
						// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
						if ( nCount > 0 )
							sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
						// 12/20/2005   Need to use the correct parameter token. 
						// 05/27/2006   The number of parameters may exceed 10.
						// 10/16/2006   Use a 3-char format string to prevent ExpandParamters from performing incomplete replacements. 
						sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("000")));
						//cmd.Parameters.Add("@" + sField + nCount.ToString("000"), DbType.Guid, 16).Value = item.Value;
						// 09/20/2005   The SQL IN clause does not allow NULL in the set.  Use an empty string instead. 
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), item, true);
						nCount++;
					}
				}
				if ( sb.Length > 0 )
				{
					// 02/20/2006   We sometimes need ot use the OR clause. 
					if ( bOrClause )
						cmd.CommandText += "    or ";
					else
						cmd.CommandText += "   and ";
					cmd.CommandText += sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
			}
		}

		// 04/05/2012   Fix to use AND as well as contains search. 
		public static void AppendLikeParameters(IDbCommand cmd, string[] arr, string sField)
		{
			if ( arr != null )
			{
				int nCount = 0;
				cmd.CommandText += "   and ( 1 = 1" + ControlChars.CrLf;
				foreach(string item in arr)
				{
					if ( item.Length > 0 )
					{
						string sFieldInsertion = CreateDbName(cmd, "@" + sField + nCount.ToString("000"));
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), "%" + item + "%", true);
						cmd.CommandText += "        and " + sField +" like " + sFieldInsertion + ControlChars.CrLf;
						nCount++;
					}
				}
				cmd.CommandText += "       )" + ControlChars.CrLf;
			}
		}

		// 04/05/2012   AppendLikeModules is a special like that assumes that the search is for a module related value 
		public static void AppendLikeModules(IDbCommand cmd, string[] arr, string sField)
		{
			if ( arr != null )
			{
				int nCount = 0;
				cmd.CommandText += "   and ( 1 = 0" + ControlChars.CrLf;
				foreach(string item in arr)
				{
					if ( item.Length > 0 )
					{
						string sFieldInsertion = CreateDbName(cmd, "@" + sField + nCount.ToString("000"));
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), item + ".%", true);
						cmd.CommandText += "         or " + sField +" like " + sFieldInsertion + ControlChars.CrLf;
						nCount++;
					}
				}
				cmd.CommandText += "       )" + ControlChars.CrLf;
			}
		}

		public static void AppendParameter(DataView vw, string[] arr, string sField, bool bOrClause)
		{
			if ( arr != null )
			{
				int nCount = 0;
				StringBuilder sb = new StringBuilder();
				foreach(string item in arr)
				{
					// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
					if ( nCount > 0 )
						sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
					sb.Append("\'" + item.Replace("\'", "\'\'") + "\'");
					nCount++;
				}
				if ( sb.Length > 0 )
				{
					// 02/20/2006   We cannot set the filter in parts; it must be set fully formed. 
					if ( bOrClause )
						vw.RowFilter += "    or " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
					else
						vw.RowFilter += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
			}
		}

		public static void AppendGuids(IDbCommand cmd, ListBox lst, string sField)
		{
			// 06/02/2014   Change code to call array method so that large item counts can be handled. 
			List<string> arr = new List<string>();
			foreach(ListItem item in lst.Items)
			{
				if ( item.Selected && item.Value.Length > 0 )
				{
					arr.Add(item.Value);
				}
			}
			AppendGuids(cmd, arr.ToArray(), sField);
		}

		public static void AppendGuids(IDbCommand cmd, string[] arr, string sField)
		{
			if ( arr != null )
			{
				int nCount = 0;
				StringBuilder sb = new StringBuilder();
				// 04/18/2014   When referencing several thousand users, we need to avoid using parameters. 
				// The incoming tabular data stream (TDS) remote procedure call (RPC) protocol stream is incorrect. Too many parameters were provided in this RPC request. The maximum is 2100.
				if ( arr.Length > 2000 )
				{
					foreach(string item in arr)
					{
						if ( item.Length > 0 )
						{
							if ( nCount > 0 )
								sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
							sb.AppendLine("\'" + Sql.EscapeSQL(item) + "\'");
							nCount++;
						}
					}
					cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
				else
				{
					foreach(string item in arr)
					{
						if ( item.Length > 0 )
						{
							// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
							if ( nCount > 0 )
								sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
							// 12/20/2005   Need to use the correct parameter token. 
							// 10/16/2006   Use a 3-char format string to prevent ExpandParamters from performing incomplete replacements. 
							sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("000")));
							//cmd.Parameters.Add("@" + sField + nCount.ToString("000"), DbType.Guid, 16).Value = new Guid(item.Value);
							Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), new Guid(item));
							nCount++;
						}
					}
				}
				if ( sb.Length > 0 )
				{
					cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
			}
		}

		// 11/13/2009   The offline client will sent an array of Guids. 
		public static void AppendGuids(IDbCommand cmd, Guid[] arr, string sField)
		{
			if ( arr != null )
			{
				int nCount = 0;
				StringBuilder sb = new StringBuilder();
				// 04/18/2014   When referencing several thousand users, we need to avoid using parameters. 
				// The incoming tabular data stream (TDS) remote procedure call (RPC) protocol stream is incorrect. Too many parameters were provided in this RPC request. The maximum is 2100.
				if ( arr.Length > 2000 )
				{
					foreach(Guid item in arr)
					{
						if ( nCount > 0 )
							sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
						sb.AppendLine("\'" + item.ToString() + "\'");
						nCount++;
					}
					cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
				else
				{
					foreach(Guid item in arr)
					{
						// 01/01/2011   Add breaks to the SQL to reduce horizontal scrolling. 
						if ( nCount > 0 )
							sb.Append(ControlChars.CrLf + Strings.Space(sField.Length) + "           ,");
						// 12/20/2005   Need to use the correct parameter token. 
						// 10/16/2006   Use a 3-char format string to prevent ExpandParamters from performing incomplete replacements. 
						sb.Append(CreateDbName(cmd, "@" + sField + nCount.ToString("000")));
						//cmd.Parameters.Add("@" + sField + nCount.ToString("000"), DbType.Guid, 16).Value = new Guid(item.Value);
						Sql.AddParameter(cmd, "@" + sField + nCount.ToString("000"), item);
						nCount++;
					}
				}
				if ( sb.Length > 0 )
				{
					cmd.CommandText += "   and " + sField +" in (" + sb.ToString() + ")" + ControlChars.CrLf;
				}
			}
		}

		public static byte[] ToByteArray(IDbDataParameter parBYTES)
		{
			byte[] binData = null;
			int size = (parBYTES == null ? 0 : parBYTES.Size);
			binData = new byte[size];
			if ( size > 0 )
			{
				// 10/20/2005   Convert System.Array to a byte array. 
				GCHandle handle = GCHandle.Alloc(parBYTES.Value, GCHandleType.Pinned);
				IntPtr ptr = handle.AddrOfPinnedObject();
				Marshal.Copy(ptr, binData, 0, size);
				handle.Free();
			}
			return binData;
		}

		public static byte[] ToByteArray(System.Array arrBYTES)
		{
			byte[] binData = null;
			int size = (arrBYTES == null ? 0 : arrBYTES.Length);
			binData = new byte[size];
			if ( size > 0 )
			{
				// 10/20/2005   Convert System.Array to a byte array. 
				GCHandle handle = GCHandle.Alloc(arrBYTES, GCHandleType.Pinned);
				IntPtr ptr = handle.AddrOfPinnedObject();
				Marshal.Copy(ptr, binData, 0, size);
				handle.Free();
			}
			return binData;
		}

		public static byte[] ToByteArray(object oBlob)
		{
			byte[] binData = null;
			Type tBlob = oBlob.GetType();
			if ( tBlob == typeof(System.Byte[]) )
			{
				binData = oBlob as byte[];
			}
			else if ( tBlob == typeof(System.Array) )
			{
				binData = Sql.ToByteArray(oBlob as System.Array);
			}
			else
			{
				throw(new Exception("Unsupported blob type " + oBlob.GetType().ToString()));
			}
			return binData;
		}

		public static string[] ToStringArray(ListBox lst)
		{
			// 02/09/2007   This function is only used in the Reports, and we only want the selected items. 
			ArrayList arr = new ArrayList();
			for ( int i=0; i < lst.Items.Count; i++ )
			{
				if ( lst.Items[i].Selected )
				{
					arr.Add(lst.Items[i].Value);
				}
			}
			return arr.ToArray(Type.GetType("System.String")) as string[];
		}

		public static void LimitResults(IDbCommand cmd, int nMaxRows)
		{
			if ( IsMySQL(cmd) || IsPostgreSQL(cmd) )
				cmd.CommandText += " limit " + nMaxRows.ToString();
			else if ( IsOracle(cmd) )
				cmd.CommandText = "select * from (" + cmd.CommandText + ") where rownum <= " + nMaxRows.ToString();
			else if ( IsDB2(cmd) )
				cmd.CommandText += " fetch first " + nMaxRows.ToString() + " rows only";
			else if ( IsSQLServer(cmd) )
			{
				if ( cmd.CommandText.ToLower().StartsWith("select") )
					cmd.CommandText = "select top " + nMaxRows.ToString() + cmd.CommandText.Substring(6);
			}
		}

		// 11/27/2009   We also need a connection-based version of MetadataName. 
		public static string MetadataName(IDbConnection con, string sNAME)
		{
			// 09/02/2008   Tables and field names in DB2 must be in uppercase. 
			// 09/02/2008   Tables and field names in Oracle must be in uppercase. 
			// 11/27/2009   Truncate Oracle names to 30 characters. 
			// 06/03/2010   Substring will throw an exception if the length is less than the end. 
			if ( IsOracle(con) )
				return sNAME.ToUpper().Substring(0, Math.Min(sNAME.Length, 30));
			else if ( IsDB2(con) )
				return sNAME.ToUpper();
			// 09/02/2008   Tables and field names in PostgreSQL must be in uppercase. 
			else if ( IsPostgreSQL(con) )
				return sNAME.ToLower();
			// 09/02/2008   SQL Server and MySQL are not typically case significant, 
			// but SQL Server can be configured to be case significant.  Ignore that case for now. 
			return sNAME;
		}

		// 09/02/2008   Standardize the case of metadata tables to uppercase.  PostgreSQL defaults to lowercase. 
		public static string MetadataName(IDbCommand cmd, string sNAME)
		{
			// 09/02/2008   Tables and field names in DB2 must be in uppercase. 
			// 09/02/2008   Tables and field names in Oracle must be in uppercase. 
			// 11/27/2009   Truncate Oracle names to 30 characters. 
			// 06/03/2010   Substring will throw an exception if the length is less than the end. 
			if ( IsOracle(cmd) )
				return sNAME.ToUpper().Substring(0, Math.Min(sNAME.Length, 30));
			// 09/16/2010   EffiProz requires uppercase metadata names. 
			else if ( IsDB2(cmd) || IsEffiProz(cmd) )
				return sNAME.ToUpper();
			// 09/02/2008   Tables and field names in PostgreSQL must be in uppercase. 
			else if ( IsPostgreSQL(cmd) )
				return sNAME.ToLower();
			// 09/02/2008   SQL Server and MySQL are not typically case significant, 
			// but SQL Server can be configured to be case significant.  Ignore that case for now. 
			return sNAME;
		}

		// 02/08/2008   We need to build a list of the fields used by the dynamic grid. 
		public static string FormatSelectFields(UniqueStringCollection arrSelectFields)
		{
			StringBuilder sb = new StringBuilder();
			foreach ( string sField in arrSelectFields )
			{
				if ( sb.Length > 0 )
					sb.Append("     , ");
				sb.AppendLine(sField);
			}
			return sb.ToString();
		}

		public static byte[] ReadImage(Guid gID, IDbConnection con, string sCommandText)
		{
			using ( MemoryStream stm = new MemoryStream() )
			{
				using ( BinaryWriter writer = new BinaryWriter(stm) )
				{
					using ( IDbCommand cmd = con.CreateCommand() )
					{
						cmd.CommandText = sCommandText;
						cmd.CommandType = CommandType.StoredProcedure;
						
						const int BUFFER_LENGTH = 4*1024;
						int idx  = 0;
						int size = 0;
						byte[] binData = new byte[BUFFER_LENGTH];
						IDbDataParameter parID          = Sql.AddParameter(cmd, "@ID"         , gID    );
						IDbDataParameter parFILE_OFFSET = Sql.AddParameter(cmd, "@FILE_OFFSET", idx    );
						IDbDataParameter parREAD_SIZE   = Sql.AddParameter(cmd, "@READ_SIZE"  , size   );
						IDbDataParameter parBYTES       = Sql.AddParameter(cmd, "@BYTES"      , binData);
						parBYTES.Direction = ParameterDirection.InputOutput;
						do
						{
							parID         .Value = gID          ;
							parFILE_OFFSET.Value = idx          ;
							parREAD_SIZE  .Value = BUFFER_LENGTH;
							size = 0;
							if ( Sql.IsOracle(cmd) || Sql.IsDB2(cmd) ) // || Sql.IsMySQL(cmd) )
							{
								cmd.ExecuteNonQuery();
								binData = Sql.ToByteArray(parBYTES);
								if ( binData != null )
								{
									size = binData.Length;
									writer.Write(binData);
									idx += size;
								}
							}
							else
							{
								using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
								{
									if ( rdr.Read() )
									{
										binData = Sql.ToByteArray((System.Array) rdr[0]);
										if ( binData != null )
										{
											size = binData.Length;
											writer.Write(binData);
											idx += size;
										}
									}
								}
							}
						}
						while ( size == BUFFER_LENGTH );
					}
				}
				return stm.ToArray();
			}
		}

		// 09/10/2009   The PageResults method expects page-related parameters. 
		public static void PageResults(IDbCommand cmd, string sTableName, string sOrderByClause, int nCurrentPageIndex, int nPageSize)
		{
			if ( cmd.CommandText.StartsWith("select ") )
				cmd.CommandText = cmd.CommandText.Substring(7);
			
			cmd.CommandText = cmd.CommandText.Replace(ControlChars.CrLf, ControlChars.CrLf + "        ");
			if ( IsSQLServer(cmd) || IsOracle(cmd) || IsDB2(cmd) || IsPostgreSQL(cmd) )
			{
				// 09/08/2009   Custom Paging in ASP.NET 2.0 with SQL Server 2005 
				// http://aspnet.4guysfromrolla.com/articles/031506-1.aspx
				// 09/08/2009   DB2 supports the windowing functions. 
				// http://www.lazydba.com/db2/2__1099.html
				// 09/08/2009   Oracle works just like SQL Server. 
				// http://www.oracle.com/technology/oramag/oracle/06-sep/o56asktom.html
				// http://www.oracle.com/technology/oramag/oracle/07-jan/o17asktom.html
				// 09/08/2009   PostgreSQL 8.4 will support windowing functions. 
				// http://www.postgresql.org/docs/current/static/functions-window.html

				// 09/08/2009   Instead of using TOP, just specify the filter bounds on RowNumber so that the code will work for Oracle, DB2 and PostgreSQL. 
				cmd.CommandText = "select *" + ControlChars.CrLf
				                + "  from (select row_number() over(" + sOrderByClause.Trim() + ") as RowNumber" + ControlChars.CrLf
				                + "             , " 
				                + cmd.CommandText + ControlChars.CrLf
				                + "       ) " + sTableName + ControlChars.CrLf
				                + " where RowNumber >  " + (nCurrentPageIndex       * nPageSize).ToString() + ControlChars.CrLf
				                + "   and RowNumber <= " + ((nCurrentPageIndex + 1) * nPageSize).ToString() + ControlChars.CrLf;
			}
			else if ( IsMySQL(cmd) )
			{
				// http://jimlife.wordpress.com/2008/09/09/displaying-row-number-rownum-in-mysql/
				cmd.CommandText = "select *" + ControlChars.CrLf
				                + "  from (select (@rownum := @rownum + 1) as RowNumber" + ControlChars.CrLf
				                + "             , " 
				                + cmd.CommandText.Replace(" from ", " from (select @rownum := 0) r" + ControlChars.CrLf + "             , ") + ControlChars.CrLf
				                + "        " + sOrderByClause + ControlChars.CrLf
				                + "       ) " + sTableName + ControlChars.CrLf
				                + " where RowNumber >  " + (nCurrentPageIndex       * nPageSize).ToString() + ControlChars.CrLf
				                + "   and RowNumber <= " + ((nCurrentPageIndex + 1) * nPageSize).ToString() + ControlChars.CrLf;
			}
			/*
			else if ( IsPostgreSQL(cmd) )
			{
				// 09/08/2009  PostgreSQL 8.3 does not support the windowing functions. 
				// http://www.phpbuilder.com/board/showthread.php?t=10312295
				// http://www.postgresql.org/docs/8.3/static/sql-createsequence.html
				cmd.CommandText = "create temporary sequence " + sTableName + "_Sequence;" + ControlChars.CrLf
				                + "select *" + ControlChars.CrLf
				                + "  from (select nextval('" + sTableName + "_Sequence') as RowNumber" + ControlChars.CrLf
				                + "             , " 
				                + cmd.CommandText + ControlChars.CrLf
				                + "        " + sOrderByClause + ControlChars.CrLf
				                + "       ) " + sTableName + ControlChars.CrLf
				                + " where RowNumber >  " + (nCurrentPageIndex       * nPageSize).ToString() + ControlChars.CrLf
				                + "   and RowNumber <= " + ((nCurrentPageIndex + 1) * nPageSize).ToString() + ControlChars.CrLf;
			}
			*/
		}

		// 09/10/2009   The WindowResults method uses Offset and MaxRecords (as used in the SOAP calls). 
		public static void WindowResults(IDbCommand cmd, string sTableName, string sOrderByClause, int nOffset, int nMaxRecords)
		{
			if ( cmd.CommandText.StartsWith("select ") )
				cmd.CommandText = cmd.CommandText.Substring(7);
			
			cmd.CommandText = cmd.CommandText.Replace(ControlChars.CrLf, ControlChars.CrLf + "        ");
			if ( IsSQLServer(cmd) || IsOracle(cmd) || IsDB2(cmd) || IsPostgreSQL(cmd) )
			{
				cmd.CommandText = "select *" + ControlChars.CrLf
				                + "  from (select row_number() over(" + sOrderByClause.Trim() + ") as RowNumber" + ControlChars.CrLf
				                + "             , " 
				                + cmd.CommandText + ControlChars.CrLf
				                + "       ) " + sTableName + ControlChars.CrLf
				                + " where RowNumber >= " + nOffset.ToString() + ControlChars.CrLf
				                + "   and RowNumber <  " + (nOffset + nMaxRecords).ToString() + ControlChars.CrLf;
			}
			else if ( IsMySQL(cmd) )
			{
				cmd.CommandText = "select *" + ControlChars.CrLf
				                + "  from (select (@rownum := @rownum + 1) as RowNumber" + ControlChars.CrLf
				                + "             , " 
				                + cmd.CommandText.Replace(" from ", " from (select @rownum := 0) r" + ControlChars.CrLf + "             , ") + ControlChars.CrLf
				                + "        " + sOrderByClause + ControlChars.CrLf
				                + "       ) " + sTableName + ControlChars.CrLf
				                + " where RowNumber >= " + nOffset.ToString() + ControlChars.CrLf
				                + "   and RowNumber <  " + (nOffset + nMaxRecords).ToString() + ControlChars.CrLf;
			}
		}

		// 10/07/2009   We need to create our own global transaction ID to support auditing and workflow on SQL Azure, PostgreSQL, Oracle, DB2 and MySQL. 
		// This is because SQL Server 2005 and 2008 are the only platforms that support a global transaction ID with sp_getbindtoken. 
		// 10/24/2009   As a performance optimziation, we need a way to avoid calling spSYSTEM_TRANSACTIONS_Create for every transaction. 
		public static IDbTransaction BeginTransaction(IDbConnection con)
		{
			IDbTransaction trn = con.BeginTransaction();
			if ( !SplendidInit.bUseSQLServerToken )
			{
				Guid gSPLENDID_TRANSACTION_ID = Guid.NewGuid();
				SqlProcs.spSYSTEM_TRANSACTIONS_Create(ref gSPLENDID_TRANSACTION_ID, trn);
			}
			return trn;
		}

		// 10/20/2009   Try to be more efficient by using a reader. 
		public static void WriteStream(IDataReader rdr, int nFieldIndex, BinaryWriter writer)
		{
			// 10/20/2009   Read in 64K chunks. 
			const int BUFFER_LENGTH = 64*1024;
			long idx   = 0;
			long size  = 0;
			byte[] binData = new byte[BUFFER_LENGTH];
			while ( (size = rdr.GetBytes(nFieldIndex, idx, binData, 0, BUFFER_LENGTH)) > 0 )
			{
				writer.Write(binData, 0, (int) size);
				idx += size;
			}
		}

		public static void Trace(IDbCommand cmd)
		{
#if DEBUG
			System.Diagnostics.Trace.WriteLine("Sql.Trace:	exec dbo." + Sql.ExpandParameters(cmd) + ";");
#endif
		}

		// 08/03/2011   Generic duplication method. 
		public static Guid Duplicate(HttpContext Context, string sModuleName, Guid gID)
		{
			Guid gNewID = Guid.Empty;
			DataTable dtColumns = SplendidCache.ImportColumns(sModuleName);
			DataView vwColumns = new DataView(dtColumns);
			
			string sTABLE_NAME = Sql.ToString(Context.Application["Modules." + sModuleName + ".TableName"]);
			DbProviderFactory dbf = DbProviderFactories.GetFactory(Context.Application);
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				DataTable dt = new DataTable();
				sSQL = "select * " + ControlChars.CrLf
				     + "  from vw" + sTABLE_NAME + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Security.Filter(cmd, sModuleName, "list");
					Sql.AppendParameter(cmd, gID, "ID");
					using ( DbDataAdapter da = dbf.CreateDataAdapter() )
					{
						((IDbDataAdapter)da).SelectCommand = cmd;
						da.Fill(dt);
					}
				}
				if ( dt.Rows.Count > 0 )
				{
					using ( IDbTransaction trn = Sql.BeginTransaction(con) )
					{
						try
						{
							IDbCommand cmdImport = SqlProcs.Factory(con, "sp" + sTABLE_NAME + "_Update");
							IDbCommand cmdImportCSTM = null;
							vwColumns.RowFilter = "CustomField = 1";
							if ( vwColumns.Count > 0 )
							{
								vwColumns.Sort = "colid";
								cmdImportCSTM = con.CreateCommand();
								cmdImportCSTM.CommandType = CommandType.Text;
								cmdImportCSTM.CommandText = "update " + sTABLE_NAME + "_CSTM" + ControlChars.CrLf;
								int nFieldIndex = 0;
								foreach ( DataRowView row in vwColumns )
								{
									string sNAME     = Sql.ToString(row["ColumnName"]).ToUpper();
									string sCsType   = Sql.ToString(row["ColumnType"]);
									int    nMAX_SIZE = Sql.ToInteger(row["Size"]);
									if ( nFieldIndex == 0 )
										cmdImportCSTM.CommandText += "   set ";
									else
										cmdImportCSTM.CommandText += "     , ";
									cmdImportCSTM.CommandText += sNAME + " = @" + sNAME + ControlChars.CrLf;
									
									IDbDataParameter par = null;
									switch ( sCsType )
									{
										case "Guid"    :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, Guid.Empty             );  break;
										case "short"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0                      );  break;
										case "Int32"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0                      );  break;
										case "Int64"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0                      );  break;
										case "float"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0.0f                   );  break;
										case "decimal" :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, new Decimal()          );  break;
										case "bool"    :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, false                  );  break;
										case "DateTime":  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, DateTime.MinValue      );  break;
										default        :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, String.Empty, nMAX_SIZE);  break;
									}
									nFieldIndex++;
								}
								cmdImportCSTM.CommandText += " where ID_C = @ID_C" + ControlChars.CrLf;
								Sql.AddParameter(cmdImportCSTM, "@ID_C", Guid.Empty);
							}
							vwColumns.RowFilter = "";
							
							cmdImport.Transaction = trn;
							if ( cmdImportCSTM != null )
								cmdImportCSTM.Transaction = trn;
							
							bool bEnableTeamManagement  = Crm.Config.enable_team_management();
							bool bRequireTeamManagement = Crm.Config.require_team_management();
							bool bRequireUserAssignment = Crm.Config.require_user_assignment();
							foreach ( DataRow row in dt.Rows )
							{
								foreach(IDbDataParameter par in cmdImport.Parameters)
								{
									string sParameterName = Sql.ExtractDbName(cmdImport, par.ParameterName).ToUpper();
									if ( sParameterName == "TEAM_ID" && bEnableTeamManagement && bRequireTeamManagement )
										par.Value = Sql.ToDBGuid(Security.TEAM_ID);  // 02/20/2013   Make sure to convert Guid.Empty to DBNull. 
									else if ( sParameterName == "ASSIGNED_USER_ID" && bRequireUserAssignment )
										par.Value = Sql.ToDBGuid(Security.USER_ID);  // 02/20/2013   Make sure to convert Guid.Empty to DBNull. 
									// 02/20/2013   We need to set the MODIFIED_USER_ID. 
									else if ( sParameterName == "MODIFIED_USER_ID" )
										par.Value = Sql.ToDBGuid(Security.USER_ID);
									else
										par.Value = DBNull.Value;
								}
								if ( cmdImportCSTM != null )
								{
									foreach(IDbDataParameter par in cmdImportCSTM.Parameters)
									{
										par.Value = DBNull.Value;
									}
								}
								IDbDataParameter parID   = Sql.FindParameter(cmdImport, "ID");
								IDbDataParameter parID_C = null;
								if ( cmdImportCSTM != null )
									parID_C = Sql.FindParameter(cmdImportCSTM, "ID_C");

								Sql.SetParameter(cmdImport, "@MODIFIED_USER_ID", Security.USER_ID);
								foreach ( DataColumn col in dt.Columns )
								{
									string sColumnName = col.ColumnName;
									bool bSkipColumn = false;
									// 08/03/2011   We do not want to copy the ID or the sequential numbers. 
									if ( sColumnName == "ID" || sColumnName == "ID_C" )
										bSkipColumn = true;
									else if ( sColumnName.EndsWith("_NUM") || sColumnName.EndsWith("_NUMBER") )
										bSkipColumn = true;
									if ( !bSkipColumn )
									{
										IDbDataParameter par = Sql.FindParameter(cmdImport, sColumnName);
										if ( par != null )
										{
											par.Value = row[sColumnName];
										}
										else if ( cmdImportCSTM != null )
										{
											par = Sql.FindParameter(cmdImportCSTM, sColumnName);
											if ( par != null )
											{
												par.Value = row[sColumnName];
											}
										}
									}
								}
								cmdImport.ExecuteNonQuery();
								if ( parID != null )
								{
									gNewID = Sql.ToGuid(parID.Value);
									if ( cmdImportCSTM != null && parID_C != null )
									{
										parID_C.Value = gNewID;
										cmdImportCSTM.ExecuteNonQuery();
									}
								}
							}
							trn.Commit();
						}
						catch
						{
							trn.Rollback();
							throw;
						}
					}
				}
			}
			return gNewID;
		}

		// 10/26/2011   Dynamic lists needs a way to create a pretty display name. 
		public static string CamelCaseModules(L10N L10n, string sName)
		{
			sName = sName.Replace("_LINE_ITEMS", "LineItems");
			string[] arrName = sName.Split('_');
			for ( int i = 0; i < arrName.Length; i++ )
			{
				if( String.Compare(arrName[i], "ID", true) == 0 )
					arrName[i] = arrName[i].ToUpper();
				else
					arrName[i] = L10n.Term(".moduleList." + arrName[i]);
			}
			sName = String.Join(" ", arrName);
			return sName;
		}
	}

	// 10/24/2010   We need a Sql object that we can intanciate for use by the RulesWizard. 
	public class SqlObj
	{
		public bool     IsEmptyString(string   str) { return Sql.IsEmptyString(str); }
		public bool     IsEmptyString(object   obj) { return Sql.IsEmptyString(obj); }
		public string   ToString     (string   str) { return Sql.ToString     (str); }
		public string   ToString     (object   obj) { return Sql.ToString     (obj); }
		public object   ToDBString   (string   str) { return Sql.ToDBString   (str); }
		public object   ToDBString   (object   obj) { return Sql.ToDBString   (obj); }
		public byte[]   ToBinary     (object   obj) { return Sql.ToBinary     (obj); }
		public object   ToDBBinary   (object   obj) { return Sql.ToDBBinary   (obj); }
		public object   ToDBBinary   (byte[]   aby) { return Sql.ToDBBinary   (aby); }
		public DateTime ToDateTime   (DateTime dt ) { return Sql.ToDateTime   (dt ); }
		public DateTime ToDateTime   (object   obj) { return Sql.ToDateTime   (obj); }
		public string   ToDateString (object   obj) { return Sql.ToDateString (obj); }
		public string   ToString     (DateTime dt ) { return Sql.ToString     (dt ); }
		public string   ToDateString (DateTime dt ) { return Sql.ToDateString (dt ); }
		public string   ToTimeString (DateTime dt ) { return Sql.ToTimeString (dt ); }
		public object   ToDBDateTime (DateTime dt ) { return Sql.ToDBDateTime (dt ); }
		public object   ToDBDateTime (object   obj) { return Sql.ToDBDateTime (obj); }
		public bool     IsEmptyGuid  (Guid     g  ) { return Sql.IsEmptyGuid  (g  ); }
		public bool     IsEmptyGuid  (object   obj) { return Sql.IsEmptyGuid  (obj); }
		public Guid     ToGuid       (Guid     g  ) { return Sql.ToGuid       (g  ); }
		public Guid     ToGuid       (Byte[]   b  ) { return Sql.ToGuid       (b  ); }
		public Guid     ToGuid       (object   obj) { return Sql.ToGuid       (obj); }
		public object   ToDBGuid     (Guid     g  ) { return Sql.ToDBGuid     (g  ); }
		public object   ToDBGuid     (object   obj) { return Sql.ToDBGuid     (obj); }
		public Int32    ToInteger    (Int32    n  ) { return Sql.ToInteger    (n  ); }
		public Int32    ToInteger    (object   obj) { return Sql.ToInteger    (obj); }
		public long     ToLong       (long     n  ) { return Sql.ToLong       (n  ); }
		public long     ToLong       (object   obj) { return Sql.ToLong       (obj); }
		public short    ToShort      (short    n  ) { return Sql.ToShort      (n  ); }
		public short    ToShort      (int      n  ) { return Sql.ToShort      (n  ); }
		public short    ToShort      (object   obj) { return Sql.ToShort      (obj); }
		public object   ToDBInteger  (Int32    n  ) { return Sql.ToDBInteger  (n  ); }
		public object   ToDBInteger  (object   obj) { return Sql.ToDBInteger  (obj); }
		public float    ToFloat      (float    f  ) { return Sql.ToFloat      (f  ); }
		public float    ToFloat      (object   obj) { return Sql.ToFloat      (obj); }
		public float    ToFloat      (string   str) { return Sql.ToFloat      (str); }
		public object   ToDBFloat    (float    f  ) { return Sql.ToDBFloat    (f  ); }
		public object   ToDBFloat    (object   obj) { return Sql.ToDBFloat    (obj); }
		public double   ToDouble     (double   d  ) { return Sql.ToDouble     (d  ); }
		public double   ToDouble     (object   obj) { return Sql.ToDouble     (obj); }
		public double   ToDouble     (string   str) { return Sql.ToDouble     (str); }
		public Decimal  ToDecimal    (Decimal  d  ) { return Sql.ToDecimal    (d  ); }
		public Decimal  ToDecimal    (double   d  ) { return Sql.ToDecimal    (d  ); }
		public Decimal  ToDecimal    (float    f  ) { return Sql.ToDecimal    (f  ); }
		public Decimal  ToDecimal    (object   obj) { return Sql.ToDecimal    (obj); }
		public object   ToDBDecimal  (Decimal  d  ) { return Sql.ToDBDecimal  (d  ); }
		public object   ToDBDecimal  (object   obj) { return Sql.ToDBDecimal  (obj); }
		public Boolean  ToBoolean    (Boolean  b  ) { return Sql.ToBoolean    (b  ); }
		public Boolean  ToBoolean    (Int32    n  ) { return Sql.ToBoolean    (n  ); }
		public Boolean  ToBoolean    (object   obj) { return Sql.ToBoolean    (obj); }
		public object   ToDBBoolean  (Boolean  b  ) { return Sql.ToDBBoolean  (b  ); }
		public object   ToDBBoolean  (object   obj) { return Sql.ToDBBoolean  (obj); }
		// 12/19/2012   Provide a UrlEncode method. 
		public string   UrlEncode    (string   str) { return HttpUtility.UrlEncode(str); }
	}

	public class UniqueStringCollection : StringCollection
	{
		new public int Add(string value)
		{
			if ( !base.Contains(value) )
				return base.Add(value);
			return -1;
		}

		new public void AddRange(string[] value)
		{
			foreach ( string s in value )
			{
				if ( !base.Contains(s) )
					base.Add(s);
			}
		}

		// 03/17/2011   We need to treat a comma-separated list of fields as an array. 
		public void AddFields(string sField)
		{
			sField = sField.Replace(" ", "");
			string[] value = sField.Split(',');
			foreach ( string s in value )
			{
				if ( !base.Contains(s) )
					base.Add(s);
			}
		}
	}

	// 01/27/2010   We need to maintain a list of unique guids. 
	[Serializable]
	public class UniqueGuidCollection : List<Guid>
	{
		new public void Add(Guid value)
		{
			if ( !base.Contains(value) )
				base.Add(value);
		}
	}
}


