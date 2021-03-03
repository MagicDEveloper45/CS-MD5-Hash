using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MD5CS.Test
{
    [TestClass]
    public class MD5CSTests
    {
        static string GetHash(string str)
        {
            byte[] hash = System.Text.Encoding.ASCII.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            return result;           
        }

        [TestMethod]

        /* Summary
         * Testing MD5 methods, one of them is written manually, the other is called from the connected library. 
        */
        
        public void MD5Test1()
        {
            // arrange
            string str1 = "md5";
            string expected = GetHash(str1);

            // action
            MD5Sharp str2 = new MD5Sharp(str1);
            string actual = str2.calcMD5();

            // assert
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]

        /* Summary
         * Testing MD5 methods, one of them is written manually, the other is called from the connected library. 
        */
        public void MD5Test2()
        {
            // arrange
            string str1 = "smth";
            string expected = GetHash(str1);

            // action
            MD5Sharp str2 = new MD5Sharp(str1);
            string actual = str2.calcMD5();

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
