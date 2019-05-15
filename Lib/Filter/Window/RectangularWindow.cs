﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter.Window
{
    public class RectangularWindow : IWindow
    {
        public List<double> Create(int M)
        {
            return Enumerable.Repeat(1.0, M - 1).ToList();
        }
    }
}
