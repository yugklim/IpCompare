using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IpCompare
{
    public static class IpCompareUtility
    {
        // regex to split ip range - f.e., splits 11.22.33.44/55 into two groups - 11.22.33.44 and 55
        private static string regExToSplitRange = @"^\b((?:\d{1,3}\.){3}(?:\d{1,3}){1})(?:(?:\\|\/)(\d{1,3})){0,1}\b";
        // regex to split the ip into parts - f.e., splits 11.22.33.44 into 4 parts: 11, 22, 33, and 44
        // it doesn't split wrong ip? - 11.22.33 is split into 0 groups? - so you can use it to verify the correctness of ip
        private static string regExToSplitIp = @"^\b(\d{1,3})(?:\.{1})(\d{1,3})(?:\.{1})(\d{1,3})(?:\.{1})(\d{1,3})\b";

        public static IEnumerable<string> GetIpParts(this string ip)
        {
            List<string> ipParts = Regex.Split(ip, regExToSplitIp).ToList();
            ipParts.RemoveAll(string.IsNullOrEmpty);
            return ipParts;
        }

        public static IEnumerable<string> GetIpRangeParts(this string ipRange)
        {
            List<string> rangeParts = Regex.Split(ipRange, regExToSplitRange).ToList();
            rangeParts.RemoveAll(string.IsNullOrEmpty);
            return rangeParts;
        }

        public static bool IsAValidIp(this string value)
        {
            List<string> ipParts = GetIpParts(value).ToList();
            int ip;
            return ipParts.Count() == 4
                && ipParts.All(ipPart => int.TryParse(ipPart, out ip) && ip <= 255)
                && (ipParts.Sum(ipPart => int.Parse(ipPart)) > 0);
        }

        public static bool IsAValidIpRange(this string value)
        {
            List<string> ipRangeParts = GetIpRangeParts(value).ToList();
            int rangeUpperLimit;
            return ipRangeParts.Count() == 2 && IsAValidIp(ipRangeParts[0])
                && int.TryParse(ipRangeParts[1], out rangeUpperLimit) && rangeUpperLimit > 0 && rangeUpperLimit <= 255
                && int.Parse(GetIpParts(ipRangeParts[0]).ToList()[3]) <= rangeUpperLimit;
        }

        public static bool IsIpRangeEqual(this string range1, string range2)
        {
            if (IsAValidIpRange(range1) && IsAValidIpRange(range2))
            {
                List<string> range1Parts = GetIpRangeParts(range1).ToList();
                List<string> range2Parts = GetIpRangeParts(range2).ToList();
                return range1Parts[0].IsIpEqualTo(range2Parts[0])
                    && int.Parse(range1Parts[1]) == int.Parse(range2Parts[1]);
            }

            return false;
        }

        public static bool IsIpEqualTo(this string ip1, string ip2)
        {
            if (IsAValidIp(ip1) && IsAValidIp(ip2))
            {
                List<string> ip1Parts = GetIpParts(ip1).ToList();
                List<string> ip2Parts = GetIpParts(ip2).ToList();
                return int.Parse(ip1Parts[0]) == int.Parse(ip2Parts[0])
                       && int.Parse(ip1Parts[1]) == int.Parse(ip2Parts[1])
                       && int.Parse(ip1Parts[2]) == int.Parse(ip2Parts[2])
                       && int.Parse(ip1Parts[3]) == int.Parse(ip2Parts[3]);
            }

            return false;
        }

        public static bool IsIpGreaterOrEqualThan(this string ip1, string ip2)
        {
            if (IsAValidIp(ip1) && IsAValidIp(ip2))
            {
                List<string> ip1Parts = GetIpParts(ip1).ToList();
                List<string> ip2Parts = GetIpParts(ip2).ToList();
                return int.Parse(ip1Parts[0]) == int.Parse(ip2Parts[0])
                       && int.Parse(ip1Parts[1]) == int.Parse(ip2Parts[1])
                       && int.Parse(ip1Parts[2]) == int.Parse(ip2Parts[2])
                       && int.Parse(ip1Parts[3]) >= int.Parse(ip2Parts[3]);
            }

            return false;
        }

        /// <summary>
        /// verifies whether addressVerified is in range. range can be a single ip: 11.11.11.11
        /// </summary>
        /// <param name="rangeOrIp"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIpInRange(this string ip, string rangeOrIp)
        {
            if (!ip.IsAValidIp() || !(rangeOrIp.IsAValidIpRange() || rangeOrIp.IsAValidIp()))
            {
                return false;
            }

            // just one address
            List<string> rangeParts = rangeOrIp.GetIpRangeParts().ToList();
            string startIp = rangeParts[0];
            if (rangeOrIp.IsAValidIp())
            {
                return startIp.IsIpEqualTo(ip);
            }

            // deal with range really
            List<string> ipParts = ip.GetIpParts().ToList();
            string endIp = rangeParts[1];
            return ip.IsIpGreaterOrEqualThan(startIp) && int.Parse(endIp) >= int.Parse(ipParts[3]);
        }
    }
}
