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

        public Filter(IPass pass, IWindow window)
        {
            Pass = pass;
            Window = window;
        }

        public List<double> GenerateOutput(int M, double K)
        {
            var result = new List<double>();

            var passValues = Pass.Generate(M, K);
            var windowValues = Window.Generate(passValues.Count, M);

            for(int i=0; i < passValues.Count; i++)
            {
                result.Add(passValues[i] * windowValues[i]);
            }

            return result;
        }

        public List<double> GenerateOutput(int M, double f0, double fp)
        {
            double K = Pass.CalculateK(f0, fp);
            return GenerateOutput(M, K);
        }
    }
}
