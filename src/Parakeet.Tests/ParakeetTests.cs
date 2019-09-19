using System;
using System.Data;
using Shouldly;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Parakeet.Tests
{
    public class ParakeetTests
    {
        private TestClass _testClass = new TestClass();

        [Fact]
        public void maps_class_to_dynamicparams()
        {
            var result = Parakeet<TestClass>.Generate(_testClass);

            result.ParameterNames.Count().ShouldBe(25);

            result.Get<string>("StringValue").ShouldBe(_testClass.StringValue);
            // result.Get<string?>("NullableStringValue").ShouldBe(_testClass.NullableStringValue);
            result.Get<Guid>("GuidValue").ShouldBe(_testClass.GuidValue);
            result.Get<Guid?>("NullableGuidValue").ShouldBe(_testClass.NullableGuidValue);
            result.Get<char>("CharValue").ShouldBe(_testClass.CharValue);
            result.Get<char?>("NullableCharValue").ShouldBe(_testClass.NullableCharValue);
            result.Get<int>("IntValue").ShouldBe(_testClass.IntValue);
            result.Get<int?>("NullableIntValue").ShouldBe(_testClass.NullableIntValue);
            result.Get<bool>("BoolValue").ShouldBe(_testClass.BoolValue);
            result.Get<bool?>("NullableBoolValue").ShouldBe(_testClass.NullableBoolValue);
            result.Get<decimal>("DecimalValue").ShouldBe(_testClass.DecimalValue);
            result.Get<decimal?>("NullableDecimalValue").ShouldBe(_testClass.NullableDecimalValue);
            result.Get<long>("LongValue").ShouldBe(_testClass.LongValue);
            result.Get<long?>("NullableLongValue").ShouldBe(_testClass.NullableLongValue);
            result.Get<byte>("ByteValue").ShouldBe(_testClass.ByteValue);
            result.Get<byte?>("NullableByteValue").ShouldBe(_testClass.NullableByteValue);
            result.Get<TestEnum>("EnumValue").ShouldBe(_testClass.EnumValue);
            result.Get<TestEnum?>("NullableEnumValue").ShouldBe(_testClass.NullableEnumValue);
            result.Get<DateTimeOffset>("DateTimeOffsetValue").ShouldBe(_testClass.DateTimeOffsetValue);
            result.Get<DateTimeOffset?>("NullableDateTimeOffsetValue").ShouldBe(_testClass.NullableDateTimeOffsetValue);
            result.Get<DateTime>("DateTimeValue").ShouldBe(_testClass.DateTimeValue);
            result.Get<DateTime?>("NullableDateTimeValue").ShouldBe(_testClass.NullableDateTimeValue);
            result.Get<string>("NewName").ShouldBe(_testClass.OldName);
            result.Get<NotEligible>("NotEligibleButIncludeAnywayValue").ShouldBe(_testClass.NotEligibleButIncludeAnywayValue);

            // Since the TableValueParamter type is internal to Dapper, we can't use Get<T>
            // to inspect the value. We'll have to be satisfied for checking for it's existence.
            result.ParameterNames.Where(x => x == "DataTableValue").Any().ShouldBeTrue();
            result.ParameterNames.Where(x => x == "EnumerableDataRecords").Any().ShouldBeTrue();

            // Anything marked as ParakeetIgnore, even if it's eligible, should be ignored.
            result.ParameterNames.Where(x => x == "Ignored").Any().ShouldBeFalse();

            // Anything not eligble should be ignored unless it has the Parakeet attribute
            result.ParameterNames.Where(x => x == "NotEligible").Any().ShouldBeFalse();
        }

        [Fact]
        public void default_attribute_values()
        {
            var attr = new ParakeetAttribute("MyName");

            attr.Name.ShouldBe("MyName");
            attr.TableName.ShouldBeNull();
            attr.Direction.ShouldBe(ParameterDirection.Input);
            attr.DbType.ShouldBeNull();
            attr.Size.ShouldBeNull();
            attr.Precision.ShouldBeNull();
            attr.Scale.ShouldBeNull();
        }

        [Fact]
        public void optional_named_parameters()
        {
            var attr = new ParakeetAttribute(propertyName: "MyName", tableName: "dbo.MyTable", dbType: DbType.Binary);

            attr.Name.ShouldBe("MyName");
            attr.TableName.ShouldBe("dbo.MyTable");
            attr.Direction.ShouldBe(ParameterDirection.Input);
            attr.DbType.ShouldBe(DbType.Binary);
            attr.Size.ShouldBeNull();
            attr.Precision.ShouldBeNull();
            attr.Scale.ShouldBeNull();
        }
    }

    public class TestClass
    {
        public string StringValue { get; set; } = "Sam";

        // public string? NullableStringValue { get; set; } = null;

        public Guid GuidValue { get; set; } = Guid.NewGuid();

        public Guid? NullableGuidValue { get; set; } = null;

        public char CharValue { get; set; } = 'a';

        public char? NullableCharValue { get; set; } = null;

        public int IntValue { get; set; } = 100;

        public int? NullableIntValue { get; set; } = null;

        public bool BoolValue { get; set; } = true;

        public bool? NullableBoolValue { get; set; } = null;

        public decimal DecimalValue { get; set; } = 10.34M;

        public decimal? NullableDecimalValue { get; set; } = null;

        public long LongValue { get; set; } = (long)1000000000;

        public long? NullableLongValue { get; set; } = null;

        public byte ByteValue { get; set; } = (byte)1;

        public byte? NullableByteValue { get; set; } = null;

        public TestEnum EnumValue { get; set; } = TestEnum.TestValue1;

        public TestEnum? NullableEnumValue { get; set; } = null;

        public DateTimeOffset DateTimeOffsetValue { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset? NullableDateTimeOffsetValue { get; set; } = null;

        public DateTime DateTimeValue { get; set; } = DateTime.Now;

        public DateTime? NullableDateTimeValue { get; set; } = null;

        public DataTable DataTableValue { get; set; } = new DataTable();

        public IEnumerable<IDataRecord> EnumerableDataRecords { get; set; } = new List<IDataRecord>();

        [ParakeetIgnore]
        public string Ignored { get; set; }

        [Parakeet(propertyName: "NewName")]
        public string OldName { get; set; }

        public NotEligible NotEligibleValue { get; set; } = new NotEligible();

        [Parakeet]
        public NotEligible NotEligibleButIncludeAnywayValue { get; set; } = new NotEligible();
    }

    public class NotEligible { }

    public enum TestEnum
    {
        TestValue1,
        TestValue2
    }
}