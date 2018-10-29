using NUnit.Framework;
using PicNetML.Tests.TestUtils;

namespace PicNetML.Tests
{
	[TestFixture]
	public class AttributeExtensionsTests
	{
		[Test]
		public void test_attribute_to_enumerable_gives_all_distinct_nominal_vals()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow>("titanic_train.csv", 0, 10);
			weka.core.Attribute pcclass = rt.Impl.attribute(1);
			System.Collections.Generic.IEnumerable<string> values = pcclass.ToEnumerable();
			Assert.AreEqual(new[] { "1", "2", "3" }, values);
		}

		[Test]
		public void test_attribute_to_enumerable_gives_nothing_on_numerics()
		{
			Runtime rt = TestingHelpers.LoadSmallRuntime<TitanicDataRow2>("titanic_train.csv", 0, 10);
			weka.core.Attribute age = rt.Impl.attribute(4);
			System.Collections.Generic.IEnumerable<string> values = age.ToEnumerable();
			Assert.AreEqual(new string[0], values);
		}
	}
}