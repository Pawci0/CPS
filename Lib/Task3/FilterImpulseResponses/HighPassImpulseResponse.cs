using System.Collections.Generic;
using System.Linq;

namespace Lib.Task3.FilterImpulseResponses
{
    public class HighPassImpulseResponse : IImpulseResponse
    {
        public List<double> Create(int n, int m, double fo, double fp)
        {
            IImpulseResponse response = new LowPassImpulseResponse();
            var result = response.Create(n, m, fo, fp);

            result = result.Select((x, i) => x * (i % 2 == 0 ? 1 : -1)).ToList();

            return result;
        }
    }
}
