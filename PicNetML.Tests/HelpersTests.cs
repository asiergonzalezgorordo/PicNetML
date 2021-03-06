﻿using NUnit.Framework;
using PicNetML.Arff;
using System;
using System.IO;
using System.Linq;

namespace PicNetML.Tests
{
	[TestFixture]
	public class HelpersTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[SetUp, TearDown]
		public void RemoveWorkingFiles()
		{
			Directory.GetFiles(".", "*.obj").ForEach2(File.Delete);
		}

		[Test]
		public void test_range_with_positive_step_functions()
		{
			decimal[] actual = Helpers.Range(0.1m, 0.3m, 0.1m);
			CollectionAssert.AreEqual(new[] { 0.1m, 0.2m, 0.3m }, actual, "Actual: " + string.Join(", ", actual));
		}

		[Test]
		public void test_range_with_negative_step_functions()
		{
			decimal[] actual = Helpers.Range(0.3m, 0.1m, -0.1m);
			CollectionAssert.AreEqual(new[] { 0.3m, 0.2m, 0.1m }, actual, "Actual: " + string.Join(", ", actual));
		}

		[Test]
		public void test_range_arg_valudation()
		{
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => Helpers.Range(0.3m, 0.1m, 0.1m));
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => Helpers.Range(0.3m, 0.3m, 0.1m));
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => Helpers.Range(0.1m, 0.3m, -0.1m));
			Assert.Throws(typeof(ArgumentOutOfRangeException), () => Helpers.Range(0.3m, 0.3m, -0.1m));
		}

		[Test]
		public void test_serialisation_and_deserialisation()
		{
			string file = "test.obj";
			SerialisationObj o = new SerialisationObj();
			SerialisationObj serialised = Helpers.Serialise(o, file);
			SerialisationObj deserialised = Helpers.Deserialise<SerialisationObj>(file);

			Assert.AreEqual(o, serialised);

			Assert.AreNotEqual(o, deserialised);
			Assert.AreEqual(o.p, deserialised.p);
		}

		[Test]
		public void test_get_or_serialise_functionality()
		{
			string file = "test.obj";
			SerialisationObj o = new SerialisationObj();
			int count = 0;
			Func<SerialisationObj> creater = () =>
			{
				count++;
				return o;
			};
			SerialisationObj created = Helpers.GetOrSetSerialised(file, creater);
			SerialisationObj deserialised = Helpers.GetOrSetSerialised(file, creater);
			Assert.AreEqual(1, count);
			Assert.AreEqual(o, created);

			Assert.AreNotEqual(o, deserialised);
			Assert.AreEqual(o.p, deserialised.p);
		}

		[Test]
		public void test_get_value_on_simple_objects()
		{
			SimpleObj s = new SimpleObj { p = "p1" };
			Assert.AreEqual("p1", Helpers.GetValue<string>(s, "p"));
			Assert.AreEqual("p1", Helpers.GetValue(s, "p"));
		}

		[Test]
		public void test_get_value_on_igetvalue_objects()
		{
			IGetValueObj s = new IGetValueObj();
			Assert.AreEqual("p1", Helpers.GetValue<string>(s, "p"));
			Assert.AreEqual("p1", Helpers.GetValue(s, "p"));
		}

		[Test]
		public void test_get_value_on_extendable_objects()
		{
			ExtendableObj<SimpleObj> ex = ExtendableObj.Create(new SimpleObj { p = "p1" });
			ex.AddString("p2", "v2");

			Assert.AreEqual("p1", Helpers.GetValue<string>(ex, "p"));
			Assert.AreEqual("p1", Helpers.GetValue(ex, "p"));
			Assert.AreEqual("v2", Helpers.GetValue<string>(ex, "p2"));
			Assert.AreEqual("v2", Helpers.GetValue(ex, "p2"));
		}

		[Test]
		public void test_set_value_on_simple_objects()
		{
			SimpleObj s = new SimpleObj { p = "p1" };
			Helpers.SetValue(s, "p", "p2");

			Assert.AreEqual("p2", s.p);
			Assert.AreEqual("p2", Helpers.GetValue<string>(s, "p"));
			Assert.AreEqual("p2", Helpers.GetValue(s, "p"));
		}

		[Test]
		public void test_set_value_on_extendable_objects()
		{
			ExtendableObj<SimpleObj> ex = ExtendableObj.Create(new SimpleObj { p = "p1" });
			ex.AddString("p2", "v2");

			Helpers.SetValue(ex, "p", "p2");
			Helpers.SetValue(ex, "p2", "v3");

			Assert.AreEqual("p2", ex.BaseObject.p);
			Assert.AreEqual("p2", Helpers.GetValue<string>(ex, "p"));
			Assert.AreEqual("p2", Helpers.GetValue(ex, "p"));

			Assert.AreEqual("v3", Helpers.GetValue<string>(ex, "p2"));
			Assert.AreEqual("v3", Helpers.GetValue(ex, "p2"));
		}

		[Test]
		public void test_rows_where_prop_is_value_functionality()
		{
			SimpleObj[] rows = new[] {
				new SimpleObj { p = "1" },
				new SimpleObj { p = "2" },
				new SimpleObj { p = "2" },
				new SimpleObj { p = "3" },
			};
			System.Collections.Generic.IEnumerable<SimpleObj> result = Helpers.RowsWherePropIsValue(rows, "p", "2");
			CollectionAssert.AreEqual(new[] { rows[1], rows[2] }, result);
		}

		[Test]
		public void test_get_props_on_simple_objects()
		{
			string[] names = Helpers.GetProps(typeof(SimpleObj)).Select(p => p.Name).ToArray();
			Assert.AreEqual(new[] { "p" }, names);
		}

		[Test]
		public void test_get_props_on_extendable_objects_gets_from_base_type()
		{
			string[] names = Helpers.GetProps(typeof(ExtendableObj<SimpleObj>)).Select(p => p.Name).ToArray();
			Assert.AreEqual(new[] { "p" }, names);
		}

		[Test]
		public void test_cloning_on_simple_objs()
		{
			SimpleObj s = new SimpleObj { p = "v" };
			SimpleObj cloned = Helpers.Clone(s);
			Assert.AreNotEqual(s, cloned);
			Assert.AreEqual(s.p, cloned.p);
		}

		[Test]
		public void test_cloning_on_icloneables()
		{
			CloneableObj c = new CloneableObj { p = 100 };
			CloneableObj cloned = Helpers.Clone(c);
			Assert.AreEqual(100, c.p);
			Assert.AreEqual(101, cloned.p);
		}

		[Test]
		public void test_cloning_on_extendables_with_simple_base()
		{
			SimpleObj s = new SimpleObj { p = "v" };
			ExtendableObj<SimpleObj> ex = ExtendableObj.Create(s);
			ex.AddString("p2", "v2");
			ExtendableObj<SimpleObj> cloned = Helpers.Clone(ex);

			Assert.AreNotEqual(ex, cloned);
			Assert.AreEqual(ex.GetValue("p"), cloned.GetValue("p"));
			Assert.AreEqual(ex.GetValue("p2"), cloned.GetValue("p2"));
		}

		[Test]
		public void test_cloning_on_extendables_with_clonable_base()
		{
			CloneableObj c = new CloneableObj { p = 100 };
			ExtendableObj<CloneableObj> ex = ExtendableObj.Create(c);
			ex.AddString("p2", "v2");
			ExtendableObj<CloneableObj> cloned = Helpers.Clone(ex);

			Assert.AreNotEqual(ex, cloned);
			Assert.AreEqual(100, ex.GetValue("p"));
			Assert.AreEqual(101, cloned.GetValue("p"));
			Assert.AreEqual(ex.GetValue("p2"), cloned.GetValue("p2"));
		}

		[Serializable] private class SerialisationObj { public string p = "property value"; }

		private class SimpleObj { public string p { get; set; } }
	}

	internal class IGetValueObj : IGetValue
	{
		public object GetValue(string property)
		{
			if (property == "p")
			{
				return "p1";
			}

			throw new ArgumentException("property");
		}
	}

	internal class CloneableObj : ICloneable
	{
		public int p { get; set; }

		public object Clone()
		{
			return new CloneableObj { p = p + 1 };
		}
	}
}