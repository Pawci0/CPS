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
            var convertedData = data.Select((v) => new Complex(v, 0))
                                    .ToList()
                                    .ToArray();
            return Fft(convertedData);
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

        public static List<Complex> Dft(double[] data)
        {
            var convertedData = data.Select((v) => new Complex(v, 0))
                                    .ToList()
                                    .ToArray();
            return Dft(convertedData);
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
    }
}