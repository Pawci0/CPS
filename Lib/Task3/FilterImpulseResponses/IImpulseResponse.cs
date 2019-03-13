using System.Collections.Generic;

namespace Lib.Task3.FilterImpulseResponses
{
    public interface IImpulseResponse
    {
        List<double> Create(int n, int m, double fo, double fp);
    }
}
