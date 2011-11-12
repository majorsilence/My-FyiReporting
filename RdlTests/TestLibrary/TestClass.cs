using System;
using System.Collections.Generic;
using System.Text;

namespace TestLibrary
{
    public class TestClass
    {
        public static string Test()
        { 
            return "baseclass";
        }
    }
    public class XTestClass : TestClass 
    {
        public static string XTest()
        {
            return "inherited class";
        }
    }
    
    public static class Util
    {
        public static DateTime? GetDateTime()
        {
            return DateTime.Now;
        }
        public static decimal Test(DateTime? d)
        {
            return 0;
        }
    }
}
