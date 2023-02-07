using MURDoX.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MURDoX.Services.Extensions
{
    public static class EnumConverterExtensions
    {

        #region CONVERT CHANGELOGSTATUS FROM STRING
        /// <summary>
        /// converts a string representation of the ChangeLogStatus enum to a ChangeLogStatus
        /// </summary>
        /// <param name="value"></param>
        /// <returns>ChangeLogStatus</returns>
        public static ChangeLogStatus ConvertChangeLogStatusFromString(this string value)
        {
            var result = value.ToLower() switch
            {
                "development" => ChangeLogStatus.DEVELOPMENT,
                "implimented" => ChangeLogStatus.IMPLIMENTED,
                "onhold" => ChangeLogStatus.ONHOLD,
                "removed" => ChangeLogStatus.REMOVED,
                _ => ChangeLogStatus.DEFAULT,
            };

            return result;
        }
        #endregion

        #region CONVERT CHANGELOGSTATUS TO STRING
        /// <summary>
        /// converts a string representation of the ChangeLogStatus enum to a ChangeLogStatus
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static string ConvertChangeLogStatusToString(this ChangeLogStatus value)
        {
            var result = value switch
            {
                 ChangeLogStatus.DEVELOPMENT => "development",
                 ChangeLogStatus.IMPLIMENTED => "implimented",
                 ChangeLogStatus.ONHOLD =>  "onhold" ,
                 ChangeLogStatus.REMOVED => "removed",
                _ => "default",
            };

            return result;
        }
        #endregion

    }
}
