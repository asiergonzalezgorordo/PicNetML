﻿using NUnit.Framework;
using PicNetML.Tests.TestUtils;
using System.IO;

namespace PicNetML.Tests.Clstr
{
	[TestFixture]
	public class BasicClustererTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[Test]
		public void test_basic_clustering()
		{
			Runtime rt = Runtime.LoadFromFile<TitanicDataRow>(0, TestingHelpers.GetResourceFileName("titanic_train.csv"));

			// Remove the classifier (which upsets clusterers)
			rt = rt.Filters.UnsupervisedAttribute.Remove.AttributeIndices("1").RunFilter();

			PicNetML.Clstr.IBaseClusterer<weka.clusterers.SimpleKMeans> clusterer = rt.Clusterers.SimpleKMeans.
				 NumClusters(10).
				 Build();

			Assert.AreEqual(1, clusterer.ClusterInstance(rt[0]));
			Assert.AreEqual(0, clusterer.ClusterInstance(rt[2]));
			Assert.AreEqual(0, clusterer.ClusterInstance(rt[3]));
			Assert.AreEqual(1, clusterer.ClusterInstance(rt[4]));
		}
	}
}