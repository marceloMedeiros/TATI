using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Extensions
{
    public static class MyExtensions
    {
        public static string Truncate(this string value, int length)
        {
            if (value != null)
            {
                return (value.Length > length) ? value.Substring(0, length) : value;
            }
            else
            {
                return null;
            }
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            propertyName = propertyName.ToLower();
            return obj.GetType().GetProperties().Single(pi => pi.Name.ToLower() == propertyName).GetValue(obj, null);
        }

        public static byte? ConvertToByte(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToByte(value);
            }
            else
            {
                return null;
            }
        }

        public static short? ConvertToInt16(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToInt16(value);
            }
            else
            {
                return null;
            }
        }

        public static int ConvertToInt(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return 0;
            }
        }

        public static int? ConvertToInt32(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return null;
            }
        }

        public static long? ConvertToInt64(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return null;
            }
        }

        public static long ConvertToLong(this string value)
        {
            return Convert.ToInt64(value);

        }

        public static decimal? ConvertToDecimal(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return null;
            }
        }

        public static decimal ConvertToNumeric(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return 0;
            }
        }

        public static float ConvertToFloat(this string value)
        {
            if (value != String.Empty)
            {
                return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                return 0;
            }
        }

        public static double? ConvertToDoubleNullable(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return null;
            }
        }

        public static double ConvertToDouble(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return 0;
            }
        }

        public static DateTime ToDateTime(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ConvertToDateTime(this string value)
        {
            if (value != String.Empty)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return null;
            }
        }

        public static Boolean IsNull(this object value)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsNullOrEmpty(this string value)
        {
            if ((value == null) || (value == String.Empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsZero(this decimal value)
        {
            if (value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsZero(this int value)
        {
            if (value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsNullOrIsZero(this decimal? value)
        {
            if ((value == null) || (value == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsNullOrIsZero(this int? value)
        {
            if ((value == null) || (value == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        */
        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        //public static long? ToInt64OrNull(this object value)
        //{
        //    return value.IsNull() ? null : value.ToString().ConvertToInt64();
        //}
        public static long? ConvertToInt64(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return null;
            }
        }

        public static long ConvertToLong(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return 0;
            }
        }

        public static decimal? ConvertToDecimalNullable(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return null;
            }
        }

        public static decimal ConvertToDecimal(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToDecimal(value);
            }
            else
            {
                return 0;
            }
        }

        public static double ConvertToDouble(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToDouble(value);
            }
            else
            {
                return 0;
            }
        }

        public static int? ConvertToInt32(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return null;
            }
        }

        public static int ConvertToInt(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return 0;
            }
        }

        public static int? ConvertToIntNullable(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToInt32(value);
            }
            else
            {
                return null;
            }
        }

        public static string ConvertToString(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return value.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        public static DateTime ConvertToDateTime(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static DateTime? ConvertToDateTimeOrNull(this object value)
        {
            if (!value.IsNull() && value.ToString() != String.Empty)
            {
                return Convert.ToDateTime(value);
            }
            else
            {
                return null;
            }
        }

        public static string ConvertToRealCurrency(this decimal value)
        {
            var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C2}", value);
            return valorFormatado;
        }

        public static string ConvertToRealCurrency(this decimal? value)
        {
            var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C2}", value);
            return valorFormatado;
        }

        public static string ConvertToRealCurrency(this object value)
        {
            var valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C2}", value);
            return valorFormatado;
        }

        public static string ConvertDateToDataPorExtenso(this DateTime value)
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            DateTimeFormatInfo dtfi = culture.DateTimeFormat;

            int dia = value.Day;
            int ano = value.Year;
            string mes = culture.TextInfo.ToTitleCase(dtfi.GetMonthName(value.Month));
            string diasemana = culture.TextInfo.ToTitleCase(dtfi.GetDayName(value.DayOfWeek));
            string data = diasemana + ", " + dia + " de " + mes + " de " + ano;

            return data;
        }

        public static string ConvertDateToDataPorExtenso(this DateTime? value)
        {
            return ConvertDateToDataPorExtenso(value);
        }

        public static string ConvertDateToDataPorExtenso(this object value)
        {
            return ConvertDateToDataPorExtenso(value);
        }

        public static string ToString(this DateTime? value, string mascara)
        {
            if (value != null)
            {
                return ((DateTime)value).ToString(mascara);
            }
            else
            {
                return String.Empty;
            }
        }

        public static bool In<T>(this T item, params T[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            return items.Contains(item);
        }

        public static string Unmask(this string value)
        {
            if (value != null)
            {
                Regex Rgx = new Regex("[^a-zA-Z0-9]");
                return Rgx.Replace(value, ""); ;
            }
            else
            {
                return String.Empty;
            }
        }

        public static string RemoveAcentos(this string text)
        {
            if (text != null)
            {
                var normalizedString = text.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();

                foreach (var c in normalizedString)
                {
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            }
            else
            {
                return String.Empty;
            }
        }

        public static string UnmaskFile(this string value)
        {
            if (value != null)
            {
                Regex Rgx = new Regex("[^a-zA-Z0-9.]");
                return Rgx.Replace(value, ""); ;
            }
            else
            {
                return String.Empty;
            }
        }

        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        public static Boolean isDate(this string value)
        {
            DateTime dateValue;
            return DateTime.TryParseExact(value, Constantes.MascaraData, null, DateTimeStyles.None, out dateValue);
        }

        public static Boolean isCEP(this string value)
        {
            if (value != null)
            {
                Regex Rgx = new Regex("/(^[0-9]{5}-[0-9]{3}$|^[0-9]{8}$)/");
                if (Rgx.IsMatch(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean isEmail(this string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static Boolean isCPF(this string value)
        {
            if (value != null)
            {
                Regex Rgx = new Regex("/(^[0-9]{3}.[0-9]{3}.[0-9]{3}-[0-9]{2}$|^[0-9]{11}$)/");
                if (Rgx.IsMatch(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean isCNPJ(this string value)
        {
            if (value != null)
            {
                Regex Rgx = new Regex("/(^[0-9]{2}.[0-9]{3}.[0-9]{3}/[0-9]{4}-[0-9]{2}$|^[0-9]{14}$)/");
                if (Rgx.IsMatch(value))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static Boolean isLatLng(this string value)
        {
            if (value != null)
            {
                //Regex Rgx = new Regex("/^([-+]?)([\\d]{1,2})(((\\.)(\\d+)(,)))(\\s*)(([-+]?)([\\d]{1,3})((\\.)(\\d+))?)$/g");
                if (Regex.Matches(value, @"[a-zA-Z]").Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static T SingleOrDefault<T>(this List<T> lst)
        {
            return lst[0];
        }

        public static string ToUSStringValue(this decimal value)
        {
            //return decimal.Round(value, 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.GetCultureInfo("en-US"));
            return value.ToString("F", CultureInfo.GetCultureInfo("en-US"));
        }

        public static bool ConvertToBool(this object value)
        {
            if (!value.IsNull() && (value.ToString() == "1" || value.ToString() == "True"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static object GetValue(this DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? row[column] : null;
        }

        public static string ToSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            return new string((value ?? string.Empty).Skip(startIndex).Take(length).ToArray());
        }

        public static IQueryable<T> ToQueryable<T>(this T instance)
        {
            return new[] { instance }.AsQueryable();
        }

        public static void EnableTab(System.Windows.Forms.TabPage page, bool enable)
        {
            foreach (System.Windows.Forms.Control ctl in page.Controls)
            {
                ctl.Enabled = enable;
            }
        }

        public static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
            {
                return false;
            }

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            }

            resto = (soma % 11);
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
            {
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            }

            resto = (soma % 11);
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public static bool IsCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            if (resto < 2)
            {
                resto = 0;
            }
            else
            {
                resto = 11 - resto;
            }

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

    }
}
