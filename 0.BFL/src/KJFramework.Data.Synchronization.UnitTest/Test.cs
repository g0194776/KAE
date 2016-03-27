using System;
using KJFramework.Messages.Attributes;
using KJFramework.Messages.Contracts;

namespace KJFramework.Data.Synchronization.UnitTest
{
    /// <summary>
    ///     ≤‚ ‘∂‘œÛ
    /// </summary>
    public class Test : IntellectObject , IComparable<Test>, IComparable
    {
        [IntellectProperty(10)]
        public string Name { get; set; }

        [IntellectProperty(11)]
        public int Age { get; set; }

        public int CompareTo(Test other)
        {
            if (Name != other.Name) return -1;
            if (Age != other.Age) return -1;
            return 0;
        }

        public override string ToString()
        {
            return "Name:" + Name + "," + "Age:" + Age;
        }

        public int CompareTo(object obj)
        {
            Test t = (Test)obj;
            if (Name != t.Name) return -1;
            if (Age != t.Age) return -1;
            return 0;
        }
    }
}