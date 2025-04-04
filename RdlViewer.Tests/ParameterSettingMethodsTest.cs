﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace RdlViewer.Tests
{
    [TestFixture]
    public class ParameterSettingMethodsTest
    {
        [Test]
        public void SetJsonParameterWithAmpersAndSpecialValues()
        {
            var sut = new Majorsilence.Reporting.RdlViewer.RdlViewer();

            var paramDic = new Dictionary<string, string>();

            paramDic.Add("testparam1", "testvalue1");
            paramDic.Add("ampersand", "test & value1");
            paramDic.Add("badsigns", "{}[]?*!\\\"");

            var paramString = JsonConvert.SerializeObject(paramDic);

            sut.SetReportParametersAsJson(paramString);

            var result = sut.Parameters;

            Assert.That(result.Contains("testvalue1"));
            Assert.That(result.Contains("test & value1"));
            Assert.That(result.Contains("{}[]?*!\\\""));
        }

        [TestCase("normaltest", "normalvalue", ExpectedResult = true)] // this is how it works
        [TestCase("testwithampersand", "here & there", ExpectedResult = false)] // this is to demonstrate the ampersand-parameter-problem
        [Test]
        public bool SetParametersWithAmpersandSeparation(string key, string value)
        {
            var sut = new Majorsilence.Reporting.RdlViewer.RdlViewer();

            var paramString = key + "=" + value;

            sut.Parameters = paramString;

            var result = sut.Parameters;

            return result.Contains(key) && result.Contains(value);

        }

    }
}
