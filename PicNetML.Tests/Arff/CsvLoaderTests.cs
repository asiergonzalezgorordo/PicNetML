using NUnit.Framework;
using PicNetML.Arff;
using System.IO;
using System.Linq;

namespace PicNetML.Tests.Arff
{
	[TestFixture]
	public class CsvLoaderTests
	{
		[SetUp]
		[TearDown]
		public void CleanOutTestFiles()
		{
			Directory.GetFiles(".", ".csv").ForEach2(File.Delete);
		}

		[Test]
		public void Test_loading_file_without_a_preprocessor()
		{
			CsvLoader<C1> c = new CsvLoader<C1>();
			string csvcontent = "prop1,prop2\n0,1\n2,3";
			string file = "Test_loading_file_without_a_preprocessor.csv";
			File.WriteAllText(file, csvcontent);
			C1[] rows = c.Load(file).ToArray();

			Assert.AreEqual(2, rows.Length);
			Assert.AreEqual(0, rows[0].prop1);
			Assert.AreEqual(1, rows[0].prop2);
			Assert.AreEqual(2, rows[1].prop1);
			Assert.AreEqual(3, rows[1].prop2);
		}

		[Test]
		public void Test_loading_file_with_a_preprocessor()
		{
			CsvLoader<C1> c = new CsvLoader<C1>();
			string csvcontent = "prop1,prop2\n0,1\n2,3";
			string file = "Test_loading_file_with_a_preprocessor.csv";
			File.WriteAllText(file, csvcontent);
			C1[] rows = c.Load(file, l =>
			{
				string[] tokens = l.Split(',');
				Assert.AreEqual(2, tokens.Length);
				return string.Join(",", tokens.Select(t => "1" + t));
			}).ToArray();

			Assert.AreEqual(2, rows.Length);
			Assert.AreEqual(10, rows[0].prop1);
			Assert.AreEqual(11, rows[0].prop2);
			Assert.AreEqual(12, rows[1].prop1);
			Assert.AreEqual(13, rows[1].prop2);
		}

		[Test]
		public void Test_loading_lines_without_a_preprocessor()
		{
			CsvLoader<C1> c = new CsvLoader<C1>();
			string[] lines = new[] { "0,1", "2,3" };
			C1[] rows = c.LoadLines(lines).ToArray();

			Assert.AreEqual(2, rows.Length);
			Assert.AreEqual(0, rows[0].prop1);
			Assert.AreEqual(1, rows[0].prop2);
			Assert.AreEqual(2, rows[1].prop1);
			Assert.AreEqual(3, rows[1].prop2);
		}

		[Test]
		public void Test_loading_lines_with_a_preprocessor()
		{
			CsvLoader<C1> c = new CsvLoader<C1>();
			string[] lines = new[] { "0,1", "2,3" };
			C1[] rows = c.LoadLines(lines, l =>
			{
				string[] tokens = l.Split(',');
				Assert.AreEqual(2, tokens.Length);
				return string.Join(",", tokens.Select(t => "1" + t));
			}).ToArray();

			Assert.AreEqual(2, rows.Length);
			Assert.AreEqual(10, rows[0].prop1);
			Assert.AreEqual(11, rows[0].prop2);
			Assert.AreEqual(12, rows[1].prop1);
			Assert.AreEqual(13, rows[1].prop2);
		}

		private class C1
		{
			public int prop1 { get; set; }
			public int prop2 { get; set; }
		}
	}
}