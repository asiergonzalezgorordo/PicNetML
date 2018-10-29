using NUnit.Framework;
using PicNetML.AttrSel.Algs;
using PicNetML.Tests.TestUtils;
using System.Linq;

namespace PicNetML.Tests.AttrSel
{
	[TestFixture]
	public class BasicAttributeSelectionTests
	{
		[Test]
		public void simple_attribute_selection_tests_with_indexes()
		{
			Runtime rt = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv"));
			BestFirst alg = rt.AttributeSelections.Algorithms.BestFirst.
					Direction(BestFirst.EDirection.Bi_directional).
					LookupCacheSize(10);
			PicNetML.AttrSel.Evals.CfsSubset eval = rt.AttributeSelections.Evaluators.CfsSubset.
				LocallyPredictive(true).
				MissingSeparate(true);

			int[] indexes = alg.SearchIndexes(eval);
			Assert.AreEqual(new[] { 2, 0 }, indexes);
		}

		[Test]
		public void simple_attribute_selection_tests_with_new_runtime()
		{
			Runtime rt = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv"));
			BestFirst alg = rt.AttributeSelections.Algorithms.BestFirst.
					Direction(BestFirst.EDirection.Bi_directional).
					LookupCacheSize(10);
			PicNetML.AttrSel.Evals.CfsSubset eval = rt.AttributeSelections.Evaluators.CfsSubset.
				LocallyPredictive(true).
				MissingSeparate(true);

			Runtime newrt = alg.Search(eval);
			string[] names = newrt.EnumerateAttributes.Select(a => a.Name).ToArray();
			Assert.AreEqual(new[] { "sex", "survived" }, names);
		}
	}
}