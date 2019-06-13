using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Accord.Math;
using Accord.Math.Transforms;
using AForge.Math;

namespace Lib.Fourier
{
    public static class Transforms
    {
        public static List<Complex> Fft(double[] data)
        {

            FastFourierTransform fft = new FastFourierTransform();
            var result = fft.Transform(new List<double>(data));
            return result;
    //        var convertedData = data.Select((v) => new Complex(v, 0))
          //                          .ToList()
            //                       .ToArray();
        //   return Fft(convertedData);
        }
        public static List<Complex> Fft(Complex[] data)
        {
            FourierTransform2.FFT(data, FourierTransform.Direction.Forward);
            return data.ToList();
        }

        public static Complex[] Ifft(Complex[] data)
        {
            FourierTransform2.FFT(data, FourierTransform.Direction.Backward);
            return data;
        }

        public static List<Complex> Dft(double[] realPoints)
        {
            List<Complex> points = RealToComplex(new List<Double>(realPoints));
            List<Complex> result = new List<Complex>();

            if ((points.Count != 0) && ((points.Count & (points.Count - 1)) != 0))
                throw new ArgumentException();

            for (int i = 0; i < points.Count; i++)
            {
                Complex complex = 0;

                for (int j = 0; j < points.Count; j++)
                    complex += new Complex(points[j].Real, points[j].Imaginary) * CoreFactor(i, j, points.Count);

                result.Add(complex / points.Count);
            }
            return result;
        }

        public static List<Complex> Dft(Complex[] data)
        {
            FourierTransform2.DFT(data, FourierTransform.Direction.Forward);

            return data.ToList();
        }

        public static Complex[] Idft(Complex[] data)
        {
            FourierTransform2.DFT(data, FourierTransform.Direction.Backward);

            return data;
        }

        public static List<double> Dct(double[] data)
        {
            CosineTransform.DCT(data);

            return data.ToList();
        }

        public static List<double> Idct(double[] data)
        {
            CosineTransform.IDCT(data);

            return data.ToList();
        }

        public static List<double> Fct(double[] data)
        {
            if (data == null)
                throw new NullReferenceException();
            var len = data.Length;
            var halfLen = len / 2;
            var temp = new Complex[len];
            for (var i = 0; i < halfLen; i++)
            {
                temp[i] = data[i * 2];
                temp[len - 1 - i] = data[i * 2 + 1];
            }

            if (len % 2 == 1)
                temp[halfLen] = data[len - 1];
            Fft(temp);
            for (var i = 0; i < len; i++)
                data[i] = (temp[i] * Complex.Exp(new Complex(0, -i * Math.PI / (len * 2)))).Real;

            return data.ToList();
        }

        public static List<double> Ifct(double[] data)
        {
            if (data == null)
                throw new NullReferenceException();
            var len = data.Length;
            if (len > 0)
                data[0] /= 2;
            var temp = new Complex[len];
            for (var i = 0; i < len; i++)
                temp[i] = data[i] * Complex.Exp(new Complex(0, -i * Math.PI / (len * 2)));
            Fft(temp);

            var halfLen = len / 2;
            for (var i = 0; i < halfLen; i++)
            {
                data[i * 2 + 0] = temp[i].Real;
                data[i * 2 + 1] = temp[len - 1 - i].Real;
            }

            if (len % 2 == 1)
                data[len - 1] = temp[halfLen].Real;

            return data.ToList();
        }


        public static List<Complex> RealToComplex(List<double> real)
        {
            List<Complex> result = new List<Complex>();

            foreach (double number in real)
                result.Add(new Complex(number, 0));

            return result;
        }

        private static Complex CoreFactor(int m, int n, int N)
        {
            return Complex.Exp(new Complex(0, -2 * Math.PI * m * n / N));
        }

        private static Complex ReverseCoreFactor(int m, int n, int N)
        {
            return Complex.Exp(new Complex(0, 2 * Math.PI * m * n / N));
        }
    }
    public class FastFourierTransform
    {
        private Dictionary<string, Complex> _factors = new Dictionary<string, Complex>();
        private Dictionary<string, Complex> _factorsReverse = new Dictionary<string, Complex>();
        public List<Complex> Transform(List<double> points)
        {
            List<Complex> transformed = new List<Complex>();
            int N = points.Count;
            transformed = SwitchSamples(points.Select(s => new Complex(s, 0)).ToList());
            return transformed.Select(c => c / N).ToList();
        }
        public List<double> ReverseTransform(List<Complex> points)
        {
            List<double> transformed = new List<double>();
            int N = points.Count;
            transformed = SwitchSamples(points, true).Select(c => c.Real).ToList();
            return transformed;
        }
        public List<Complex> SwitchSamples(List<Complex> points, bool reverse = false)
        {
            if (points.Count < 2)
            {
                return points;
            }
            List<Complex> oddPoints = new List<Complex>();
            List<Complex> evenPoints = new List<Complex>();
            for (int i = 0; i < points.Count / 2; i++)
            {
                evenPoints.Add(points[i * 2]);
                oddPoints.Add(points[i * 2 + 1]);
            }
            var result = Connect(SwitchSamples(evenPoints, reverse), SwitchSamples(oddPoints, reverse), reverse);
            return result;
        }
        private List<Complex> Connect(List<Complex> evenPoints, List<Complex> oddPoints, bool reverse)
        {
            int N = oddPoints.Count * 2;
            Complex[] result = new Complex[N];
            for (int i = 0; i < oddPoints.Count; i++)
            {
                if (reverse)
                {
                    if (!_factorsReverse.ContainsKey($"{i}, {N}"))
                        _factorsReverse[$"{i}, {N}"] = CalculateReverseFactor(i, 1, N);
                    result[i] = evenPoints[i] + (_factorsReverse[$"{i}, {N}"] * oddPoints[i]);
                    result[i + oddPoints.Count] = evenPoints[i] - (_factorsReverse[$"{i}, {N}"] * oddPoints[i]);
                }
                else
                {
                    if (!_factors.ContainsKey($"{i}, {N}"))
                        _factors[$"{i}, {N}"] = CalculateFactor(i, 1, N);
                    result[i] = evenPoints[i] + (_factors[$"{i}, {N}"] * oddPoints[i]);
                    result[i + oddPoints.Count] = evenPoints[i] - (_factors[$"{i}, {N}"] * oddPoints[i]);
                }

            }
            return result.ToList();
        }
        private Complex CalculateFactor(int m, int n, int N)
        {
            return Complex.Exp(new Complex(0, -2 * Math.PI * m * n / N));
        }
        private Complex CalculateReverseFactor(int m, int n, int N)
        {
            return Complex.Exp(new Complex(0, 2 * Math.PI * m * n / N));
        }
    }
}