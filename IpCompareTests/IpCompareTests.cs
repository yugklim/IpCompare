using System;
using System.Collections.Generic;
using System.Linq;
using IpCompare;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IpCompareTests
{
    [TestClass]
    public class IpCompareTests
    {
        [TestMethod]
        public void Verify_Whether_GetIpParts_Returns_Correct_Results()
        {
            string ip = "11.22.333.4";
            List<string> ipParts = ip.GetIpParts().ToList();
            Assert.AreEqual(ipParts.Count(), 4);
            Assert.AreEqual(ipParts[0], "11");
            Assert.AreEqual(ipParts[1], "22");
            Assert.AreEqual(ipParts[2], "333");
            Assert.AreEqual(ipParts[3], "4");

            ip = "11.22.333";
            ipParts = ip.GetIpParts().ToList();
            Assert.AreNotEqual(ipParts.Count(), 4);
        }

        [TestMethod]
        public void Verify_Whether_IsStringAValidIp_Returns_Correct_Results()
        {
            Assert.IsTrue("11.11.11.11".IsAValidIp());
            Assert.IsFalse(@"11.11.11.11\15".IsAValidIp());
            Assert.IsFalse(@"11.11.11".IsAValidIp());
            Assert.IsFalse("11.11.11.1d".IsAValidIp());
            Assert.IsTrue("11.11.255.12".IsAValidIp());
            Assert.IsFalse("11.11.256.12".IsAValidIp());
            Assert.IsFalse("0.0.0.0".IsAValidIp());
            Assert.IsFalse("000.000.000.000".IsAValidIp());
        }

        [TestMethod]
        public void Verify_Whether_GetIpRangeParts_Returns_Correct_Results()
        {
            string ipRange = "11.11.11.11/15";
            List<string> ipRangeParts = ipRange.GetIpRangeParts().ToList();
            Assert.AreEqual(ipRangeParts.Count(), 2);
            Assert.AreEqual(ipRangeParts[0], "11.11.11.11");
            Assert.AreEqual(ipRangeParts[1], "15");

            ipRange = @"11.11.11.11\15";
            ipRangeParts = ipRange.GetIpRangeParts().ToList();
            Assert.AreEqual(ipRangeParts.Count(), 2);
            Assert.AreEqual(ipRangeParts[0], "11.11.11.11");
            Assert.AreEqual(ipRangeParts[1], "15");
        }

        [TestMethod]
        public void Verify_Whether_IsStringAValidIpRange_Returns_Correct_Results()
        {
            Assert.IsTrue("11.11.11.11/15".IsAValidIpRange());
            Assert.IsTrue(@"11.11.11.11\15".IsAValidIpRange());
            Assert.IsFalse(@"11.11.11.11\9".IsAValidIpRange());
            Assert.IsFalse("11.11.11.11".IsAValidIpRange());
            Assert.IsFalse("11.11.11.1d/113".IsAValidIpRange());
            Assert.IsFalse(@"11.11.11.11\ss".IsAValidIpRange());
            Assert.IsFalse(@"11.11.11.11\0".IsAValidIpRange());
            Assert.IsTrue(@"11.11.11.11\255".IsAValidIpRange());
            Assert.IsFalse(@"11.11.11.11\256".IsAValidIpRange());
        }

        [TestMethod]
        public void Verify_Whether_IpLessOrEqualThan_Works_Right()
        {
            Assert.IsTrue("11.11.11.11".IsIpEqualTo("11.11.11.11"));
            Assert.IsFalse("11.11.11.11".IsIpEqualTo("11.11.14.12"));
            Assert.IsFalse("11.11.11.112".IsIpEqualTo("11.11.11.1"));
            Assert.IsFalse("11.11.bb.11".IsIpEqualTo("11.11.bb.12"));
        }

        [TestMethod]
        public void Verify_Whether_IpRangeEqual_Works_Right()
        {
            Assert.IsFalse("11.11.11.11/11".IsIpRangeEqual("11.11.11.11/12")); 
            Assert.IsFalse("11.11.11.11/11".IsIpRangeEqual(@"11.11.11.11\12"));
            Assert.IsFalse("11.11.11.11/13".IsIpRangeEqual("11.11.11.11/12"));
            Assert.IsFalse("11.11.11.xx/11".IsIpRangeEqual("11.11.11.xx/12"));
            Assert.IsFalse("11.11.11.11/11".IsIpRangeEqual("11.11.11.11/12"));
        }

        [TestMethod]
        public void Verify_IsIpInRange_Works_Right()
        {
            Assert.IsTrue("11.11.11.11".IsIpInRange("11.11.11.11"));
            Assert.IsTrue("11.11.11.12".IsIpInRange("11.11.11.11/15"));
            Assert.IsTrue("11.11.11.12".IsIpInRange(@"11.11.11.11\15"));
            Assert.IsFalse("11.11.11.11".IsIpInRange("11.11.11.12"));
        }

    }
}
