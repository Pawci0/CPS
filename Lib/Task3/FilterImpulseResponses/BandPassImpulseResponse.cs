using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Task3.FilterImpulseResponses
{
    public class BandPassImpulseResponse : IImpulseResponse
    {
        public List<double> Create(int n, int m, double fo, double fp)
        {
            IImpulseResponse response = new LowPassImpulseResponse();
            var result = response.Create(n, m, fo, fp);

            result = result.Select((x, i) => x * 2 * Math.Sin((Math.PI * i) / 2)).ToList();

            return result;
        }
    }
}
