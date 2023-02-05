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
        /// <summary>
        /// converts a string representation of the ChangeLogStatus enum to a ChangeLogStatus
        /// </summary>
        /// <param name="value"></param>
        /// <returns>ChangeLogStatus</returns>
        public static ChangeLogStatus ConvertChangeLogStatusFromString(this string value)
        {
            var result = value switch
            {
                "development" => ChangeLogStatus.DEVELOPMENT,
                "implimented" => ChangeLogStatus.IMPLIMENTED,
                "onhold" => ChangeLogStatus.ONHOLD,
                "removed" => ChangeLogStatus.REMOVED,
                _ => ChangeLogStatus.DEFAULT,
            };

            return result;
        }
    }
}
