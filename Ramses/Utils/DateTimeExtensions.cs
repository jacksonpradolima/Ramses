using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Ramses.Utils
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Formata uma data correspondente ao primeiro dia do mês da data especificada, considerando a cultura atual
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatFirstDayOfMonth(this DateTime date)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "en-US")
                return date.ToString("MM/01/yyyy");
            else
                return date.ToString("01/MM/yyyy");
        }

        /// <summary>
        /// Formata a data especificada considerando a cultura atual
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string FormatToCulture(this DateTime date)
        {
            if (date != DateTime.MinValue)
            {
                if (System.Globalization.CultureInfo.CurrentCulture.Name == "en-US")
                    return date.ToString("MM/dd/yyyy");
                else
                    return date.ToString("dd/MM/yyyy");
            }
            else
                return "";
        }

        /// <summary>
        /// Método que converte uma string (Tue Jan 31 00:00:00 EST-0300 2012) em Data
        /// </summary>
        /// <param name="inputValue">String a ser convertida em Data</param>
        /// <returns>Data</returns>
        public static DateTime ConvertDateTimeWithTimeZone(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
                return DateTime.MinValue;

            var pattern = @"[a-zA-Z]+ (?<month>[a-zA-Z]+) (?<day>[0-9]+) [0-9]+:[0-9]+:[0-9]+ (?<timezone>[a-zA-Z]+-[0-9]+) (?<year>[0-9]+)";

            Regex findTz = new Regex(pattern, RegexOptions.Compiled);           
            var resultRegex = findTz.Match(inputValue).Groups;
            var day = resultRegex["day"].Value;
            var month = resultRegex["month"].Value;
            var year = resultRegex["year"].Value;

            var result = DateTime.MinValue;

            try
            {
                result = DateTime.Parse(string.Format("{0}-{1}-{2}", day, month, year));
            }
            catch (Exception){ }

            return result;
        }
    }
}