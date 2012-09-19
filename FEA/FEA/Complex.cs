using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FEA
{
	class Complex
	{
		private double re, im;
		
		public Complex()
		{
			this.re = 0;
			this.im = 0;
		}

		public Complex(double re, double im=0)
		{
			this.re = re;
			this.im = im;
		}

		public static Complex operator +(Complex c1, Complex c2)
		{
			return new Complex(c1.re + c2.re, c1.im + c2.im);
		}

		public static Complex operator -(Complex c1, Complex c2)
		{
			return new Complex(c1.re - c2.re, c1.im - c2.im);
		}

		public static Complex operator -(Complex c1)
		{
			return new Complex(-c1.re, -c1.im);
		}

		public static Complex operator *(Complex c1, Complex c2)
		{
			return new Complex(c1.re*c2.re-c1.im*c2.im, c1.re*c2.im+c2.re*c1.im);
		}

		public static Complex operator /(Complex c1, Complex c2)
		{
			double del = Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2);
			return new Complex(Convert.ToDouble(c1.re * c2.re + c1.im * c2.im) / del, Convert.ToDouble(c1.im * c2.re - c1.re * c2.im) / del);
		}
		//TODO: переопределение типа от double к Complex
	}
}
