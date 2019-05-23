using Lib.Filter.Pass;
using Lib.Filter.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Filter
{
    public class Filter
    {
        public IPass Pass { get; set; }
        public IWindow Window { get; set; }
        public int M { get; set; }
        public double K { get; set; }

        public Filter(IPass pass, IWindow window, int m, double k)
        {
            Pass = pass;
            Window = window;
            M = m;
            K = k;
        }

        public List<double> GenerateOutput()
        {
            var result = new List<double>();

            var passValues = Pass.Generate(M, K);
            var windowValues = Window.Generate(passValues.Count, M);

            for (int i = 0; i < passValues.Count; i++)
            {
                result.Add(passValues[i] * windowValues[i]);
            }

            return result;
        }

        public List<double> GenerateOutput(int m, double k)
        {
            var result = new List<double>();

            var passValues = Pass.Generate(m, k);
            var windowValues = Window.Generate(passValues.Count, m);

            for(int i=0; i < passValues.Count; i++)
            {
                result.Add(passValues[i] * windowValues[i]);
            }

            return result;
        }

        public List<double> GenerateOutput(int m, double f0, double fp)
        {
            double k = Pass.CalculateK(f0, fp);
            return GenerateOutput(m, k);
        }
    }
}
