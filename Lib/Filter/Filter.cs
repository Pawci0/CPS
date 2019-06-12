using System.Collections.Generic;
using Lib.Filter.Pass;
using Lib.Filter.Window;

namespace Lib.Filter
{
    public class Filter
    {
        public Filter(IPass pass, IWindow window, int m, double k)
        {
            Pass = pass;
            Window = window;
            M = m;
            K = k;
        }

        public IPass Pass { get; set; }
        public IWindow Window { get; set; }
        public int M { get; set; }
        public double K { get; set; }

        public List<double> GenerateOutput()
        {
            var result = new List<double>();

            var passValues = Pass.Generate(M, K);
            var windowValues = Window.Generate(passValues.Count, M);

            for (var i = 0; i < passValues.Count; i++) result.Add(passValues[i] * windowValues[i]);

            return result;
        }

        public List<double> GenerateOutput(int m, double k)
        {
            var result = new List<double>();

            var passValues = Pass.Generate(m, k);
            var windowValues = Window.Generate(passValues.Count, m);

            for (var i = 0; i < passValues.Count; i++) result.Add(passValues[i] * windowValues[i]);

            return result;
        }

        public List<double> GenerateOutput(int m, double f0, double fp)
        {
            var k = Pass.CalculateK(f0, fp);
            return GenerateOutput(m, k);
        }
    }
}