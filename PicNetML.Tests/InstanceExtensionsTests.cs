using NUnit.Framework;
using PicNetML.Tests.TestUtils;
using System.IO;
using System.Linq;

namespace PicNetML.Tests
{
	[TestFixture]
	public class InstanceExtensionsTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[Test]
		public void test_ToStrings_gets_the_string_representation_of_the_instance()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow2>("titanic_train.csv", 0, 3);
			weka.core.Instance instance = rt[0].Impl;
			System.Collections.Generic.IEnumerable<string> strs = instance.ToStrings();
			Assert.AreEqual(new[] { "0", "3", "Braund, Mr. Owen Harris", "male", "22", "1", "0", "A/5 21171", "7.25", "?", "S" }, strs);
		}

		[Test]
		public void test_ToEnumerableAttributes()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 3);
			System.Collections.Generic.IEnumerable<PmlAttribute> atts = rt[0].Impl.ToEnumerableAttributes();
			string[] names = atts.Select(a => a.Name).ToArray();
			Assert.AreEqual(new[] { "survived", "pclass", "sex", "embarked" }, names);
		}
	}
}