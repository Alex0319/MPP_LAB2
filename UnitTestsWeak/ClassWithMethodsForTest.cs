﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsWeak
{
    public class ClassWithMethodsForTest
    {
        public int resultIntValue { get; set; }
        public string resultStringValue { get; set; }
        public void Sum(int x, int y)
        {
            resultIntValue = x + y;
        }

        public void Mul(int x, int y,byte z)
        {
            resultIntValue = x * y * z;
        }

        public string makeString(string firstStr, int number,string secondString)
        {
            return firstStr + number.ToString() + secondString;
        }
    }
}
