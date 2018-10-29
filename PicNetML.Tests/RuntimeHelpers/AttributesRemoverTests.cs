using NUnit.Framework;
using PicNetML.RuntimeHelpers;
using PicNetML.Tests.TestUtils;
using System.Linq;

namespace PicNetML.Tests.RuntimeHelpers
{
	[TestFixture]
	public class AttributesRemoverTests
	{
		[Test]
		public void test_removing_attributes_by_index()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 3);
			AttributesRemover ar = new AttributesRemover(rt);
			string[] initial = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			ar.RemoveAttributes(2, 3);
			string[] after = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			Assert.AreEqual(new[] { "survived", "pclass", "sex", "embarked" }, initial);
			Assert.AreEqual(new[] { "survived", "pclass" }, after);
		}

		[Test]
		public void test_removing_attributes_by_names()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 3);
			AttributesRemover ar = new AttributesRemover(rt);
			string[] initial = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			ar.RemoveAttributes("embarked", "sex");
			string[] after = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			Assert.AreEqual(new[] { "survived", "pclass", "sex", "embarked" }, initial);
			Assert.AreEqual(new[] { "survived", "pclass" }, after);
		}

		[Test]
		public void test_keep_attributes_by_index()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 3);
			AttributesRemover ar = new AttributesRemover(rt);
			string[] initial = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			ar.KeepAttributes(0, 2, 3);
			string[] after = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			Assert.AreEqual(new[] { "survived", "pclass", "sex", "embarked" }, initial);
			Assert.AreEqual(new[] { "survived", "sex", "embarked" }, after);
		}

		[Test]
		public void test_keep_attributes_by_names()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 3);
			AttributesRemover ar = new AttributesRemover(rt);
			string[] initial = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			ar.KeepAttributes("survived", "embarked", "sex");
			string[] after = rt.EnumerateAttributes.Select(a => a.Name).ToArray();
			Assert.AreEqual(new[] { "survived", "pclass", "sex", "embarked" }, initial);
			Assert.AreEqual(new[] { "survived", "sex", "embarked" }, after);
		}
	}
}