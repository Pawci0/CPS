using Lib.Task3.FilterImpulseResponses;
using Lib.Task3.Helpers;
using Lib.Task3.WindowFunctions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.Task3
{
    public class Filter
    {
        private IImpulseResponse _response;
        private IWindowFunction _window;

        public Filter ImpulseResponse(IImpulseResponse response)
        {
            _response = response;
            return this;
        }

        public Filter WindowFunction(IWindowFunction window)
        {
            _window = window;
            return this;
        }

        public List<double> FilterOperation(List<double> points, int m, double fo, double fp)
        {
            if (_response == null)
                throw new Exception("Response is null");
            return OperationsHelper.Convolution(_response.Create(points.Count, m, fo, fp).Zip((_window ?? new RectangularWindow()).Create(points.Count, m), (x, y) => x * y).ToList(), points);
        }

        public List<double> FilterOperation2(List<double> points, int m, double fo, double fp)
        {
            if (_response == null)
                throw new Exception("Response is null");
            return _response.Create(points.Count, m, fo, fp).Zip((_window ?? new RectangularWindow()).Create(points.Count, m), (x, y) => x * y).ToList();
        }
    }
}
