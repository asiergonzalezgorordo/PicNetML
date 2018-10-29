using NUnit.Framework;
using PicNetML.Clss;
using PicNetML.Tests.TestUtils;
using System.IO;
using System.Linq;

namespace PicNetML.Tests.Clss
{
	[TestFixture]
	public class BasicClassifierTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[Test]
		public void test_simple_evaluation()
		{
			Runtime rt = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv"));
			rt.Classifiers.Trees.RandomForest.
					NumExecutionSlots(4).
					NumFeatures(5).
					NumTrees(50).
					EvaluateWithCrossValidation();
		}

		[Test]
		public void test_making_single_predictions_from_trained_model()
		{
			Runtime rt = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv"));
			RandomForest classifier = rt.Classifiers.Trees.RandomForest.
					NumExecutionSlots(4).
					NumFeatures(5).
					NumTrees(50);

			TitanicDataRow row = new TitanicDataRow
			{
				age = 10,
				pclass = "1",
				sex = "male",
				embarked = "C"
			};
			double prediction = classifier.ClassifyRow(row);
			double proba = classifier.ClassifyRowProba(row);
			Assert.AreEqual(0, prediction);
			Assert.IsTrue(proba < 0.5);
		}

		[Test]
		public void test_building_predictions_lines()
		{
			Runtime rt = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv"));
			RandomForest cls = rt.Classifiers.Trees.RandomForest.
					NumExecutionSlots(4).
					NumFeatures(5).
					NumTrees(50);

			Runtime testset = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_test.csv"), preprocessor: TestLinePreproc);
			int count = testset.NumInstances;
			System.Collections.Generic.List<string> lines = testset.GeneratePredictions(GeneratePredictionLine, cls);
			Assert.AreEqual(count, lines.Count);
		}

		private string TestLinePreproc(string line)
		{
			// Need to add a survived column to test data to meet the contract defined
			// in TitanicRow below.
			System.Collections.Generic.List<string> tokens = line.Split(',').ToList();
			tokens.Insert(1, "0");
			return string.Join(",", tokens);
		}

		private string GeneratePredictionLine(double prediction, int index)
		{
			return string.Format("{0},{1}", (index + 1), prediction);
		}

		[Test]
		public void testing_saving_and_loading_saved_model()
		{
			// Save
			Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv")).
					Classifiers.Trees.RandomForest.
							NumExecutionSlots(4).
							NumFeatures(5).
							NumTrees(50).
							FlushToFile("titanic_randor_forest.model");

			// Load
			IUntypedBaseClassifier<weka.classifiers.Classifier> classifier = BaseClassifier.Read("titanic_randor_forest.model");
			TitanicDataRow row = new TitanicDataRow
			{
				age = 10,
				pclass = "1",
				sex = "male",
				embarked = "C"
			};
			// Classify
			double prediction = classifier.ClassifyInstance(Runtime.BuildInstance(0, row));
			double proba = classifier.ClassifyInstanceProba(Runtime.BuildInstance(0, row));

			Assert.AreEqual(0.0, prediction);
			Assert.IsTrue(proba < 0.5);
		}
	}
}