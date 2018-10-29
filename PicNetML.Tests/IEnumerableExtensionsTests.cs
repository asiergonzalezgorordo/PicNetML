using java.util;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace PicNetML.Tests
{
	[TestFixture]
	public class IEnumerableExtensionsTests
	{
		[Test]
		public void test_GetMajority()
		{
			string[] possibilities = new[] { "a", "b", "c", "b", "c", "b" };
			Assert.AreEqual("b", possibilities.GetMajority());
		}

		[Test]
		public void test_Randomize()
		{
			int[] l = new[] { 1, 2, 3, 4, 5 };
			int[] rnd = l.Randomize().ToArray();
			CollectionAssert.AreNotEqual(l, rnd);
			CollectionAssert.AreEqual(l, rnd.OrderBy(v => v));
		}

		[Test]
		public void test_RandomSample()
		{
			int[] l = new[] { 1, 2, 3, 4, 5 };
			IEnumerable<int> rnd = l.RandomSample(2);
			int[] rnd2 = l.RandomSample(5).ToArray();

			Assert.AreEqual(2, rnd.Count());
			CollectionAssert.AreNotEqual(l, rnd2);
			CollectionAssert.AreEqual(l, rnd2.OrderBy(v => v));
		}

		[Test]
		public void test_RandomSampleWithReplacement()
		{
			int[] l = new[] { 1, 2, 3, 4, 5 };
			IEnumerable<int> rnd = l.RandomSampleWithReplacement(2);
			int[] rnd2 = l.RandomSampleWithReplacement(5).ToArray();

			Assert.AreEqual(2, rnd.Count());
			CollectionAssert.AreNotEqual(l, rnd2);
			CollectionAssert.AreNotEqual(l, rnd2.OrderBy(v => v));
		}

		[Test]
		public void test_ToArrayList()
		{
			int[] l = new[] { 1, 2, 3, 4, 5 };
			ArrayList al = l.ToArrayList();
			Assert.AreEqual(typeof(ArrayList), al.GetType());
			CollectionAssert.AreEqual(l, al);
		}

		[Test]
		public void test_ForEach2()
		{
			int[] l = new[] { 1, 2, 3, 4, 5 };
			List<int> l2 = new List<int>();
			l.ForEach2(v => l2.Add(v * 2));
			CollectionAssert.AreEqual(new[] { 2, 4, 6, 8, 10 }, l2);
		}

		[Test]
		public void test_ForEach2_witj_index()
		{
			int[] l = new[] { 1, 2, 3, 4, 5 };
			List<int> l2 = new List<int>();
			l.ForEach2((v, i) => l2.Add(v + i));
			CollectionAssert.AreEqual(new[] { 1, 3, 5, 7, 9 }, l2);
		}

		[Test]
		public void test_ToEnumerable()
		{
			ArrayList al = new ArrayList();
			al.add(1); al.add(2); al.add(3);
			IEnumerable<int> l = al.ToEnumerable<int>();
			CollectionAssert.AreEqual(new[] { 1, 2, 3 }, l);
		}
	}
}