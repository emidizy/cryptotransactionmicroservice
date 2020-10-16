using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class Helper
    {
        public static string GenerateUniqueId(int outputLength)
        {
            string numberRange = "123456789";
            string uniqueId = string.Empty;
            int getindex;
            for (int i = 0; i < outputLength; i++)
            {
                string generatedDigit;
                do
                {
                    getindex = new Random().Next(1, numberRange.Length);
                    generatedDigit = numberRange.ToCharArray()[getindex].ToString();

                } while (uniqueId.IndexOf(generatedDigit) != -1);
                uniqueId += generatedDigit;
            }
            return $"{uniqueId}{DateTime.Now.TimeOfDay.Milliseconds.ToString()}";
        }


    }
}
