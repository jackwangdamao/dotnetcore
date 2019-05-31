﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFundamental.Operation
{
    internal class OperationTest
    {
        internal void ComputeShiftOperators()
        {
            int num = 3;
            var num2 = num << 3;

            Console.WriteLine(num2);

            Shox0xInfo();
        }

        internal void Shox0xInfo()
        {
            var value1 = 0x0001;
            var value2 = 0x0010;

            Console.WriteLine(value1);
            Console.WriteLine(value2);

            Console.WriteLine(1 & 2);

            var value = EnumTest.Value2 | EnumTest.Value3;
            //Console.WriteLine(Enum.Parse(value));
        }
    }

    [Flags]
    internal enum EnumTest
    {
        Value1 = 0,
        Value2 = 0x0001,
        Value3 = 0x0002
    }
}
