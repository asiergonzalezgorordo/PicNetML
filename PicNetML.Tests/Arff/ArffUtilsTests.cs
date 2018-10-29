using NUnit.Framework;
using PicNetML.Arff;
using System;
using System.IO;

namespace PicNetML.Tests.Arff
{
	[TestFixture()]
	public class ArffUtilsTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[Test()]
		public void GetNonNullableTypeTest()
		{
			Assert.AreEqual(typeof(int), ArffUtils.GetNonNullableType(typeof(int?)));
			Assert.AreEqual(typeof(DateTime), ArffUtils.GetNonNullableType(typeof(DateTime?)));
			Assert.AreEqual(typeof(bool), ArffUtils.GetNonNullableType(typeof(bool?)));
		}
	}
}