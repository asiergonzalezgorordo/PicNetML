using NUnit.Framework;
using PicNetML.Tests.TestUtils;
using System.IO;
using System.Linq;

namespace PicNetML.Tests
{
	[TestFixture]
	public class InstancesExtensionsTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[Test]
		public void test_GetAttrStrings()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 5);
			System.Collections.Generic.IEnumerable<string> vals = rt.Impl.GetAttrStrings(1);
			CollectionAssert.AreEqual(new[] { "3", "1", "3", "1", "3" }, vals);
		}

		[Test]
		public void test_GetAttrDoubles()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 5);
			System.Collections.Generic.IEnumerable<double> vals = rt.Impl.GetAttrDoubles(1);
			CollectionAssert.AreEqual(new[] { 2.0, 0.0, 2.0, 0.0, 2.0 }, vals);
		}

		[Test]
		public void test_ToEnumerableAttributes()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 5);
			System.Collections.Generic.IEnumerable<PmlAttribute> atts = rt.Impl.ToEnumerableAttributes();
			string[] names = atts.Select(a => a.Name).ToArray();
			CollectionAssert.AreEqual(new[] { "survived", "pclass", "sex", "embarked" }, names);
		}

		[Test]
		public void test_ToEnumerable()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 5);
			System.Collections.Generic.IEnumerable<PmlInstance> instances = rt.Impl.ToEnumerable();
			Assert.AreEqual(5, instances.Count());
		}
	}
}