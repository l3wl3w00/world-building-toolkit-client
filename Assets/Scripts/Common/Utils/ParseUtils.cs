#nullable enable
using System.Linq;
using ModestTree;

namespace Common.Utils
{
    public static class ParseUtils
    {
        public static int ToInt(this string str)
        {
            var strAsCharList = str.ToCharArray().ToList();
            strAsCharList.RemoveAll(c => !char.IsNumber(c));
            if (strAsCharList.IsEmpty()) return 0;
            var paramWithOnlyNumbers = string.Join("",strAsCharList);
            return int.Parse(paramWithOnlyNumbers);
        }
        public static uint ToUInt(this string str)
        {
            var strAsCharList = str.ToCharArray().ToList();
            strAsCharList.RemoveAll(c => !char.IsNumber(c));
            if (strAsCharList.IsEmpty()) return 0;
            var paramWithOnlyNumbers = string.Join("",strAsCharList);
            return uint.Parse(paramWithOnlyNumbers);
        }
        
        public static ulong ToULong(this string str)
        {
            var strAsCharList = str.ToCharArray().ToList();
            strAsCharList.RemoveAll(c => !char.IsNumber(c));
            if (strAsCharList.IsEmpty()) return 0;
            var paramWithOnlyNumbers = string.Join("",strAsCharList);
            return ulong.Parse(paramWithOnlyNumbers);
        }
        public static double ToDouble(this string str, char decimalCharacter = ',')
        {
            var strAsCharList = str.ToCharArray().ToList();
            
            strAsCharList.RemoveAll(c => !(char.IsNumber(c) || c == decimalCharacter));
            if (strAsCharList.IsEmpty() || strAsCharList.Count == 1 && strAsCharList.First() == decimalCharacter) return 0;
            var paramWithOnlyNumbers = string.Join("",strAsCharList);
            return double.Parse(paramWithOnlyNumbers);
        }
    }
    
}