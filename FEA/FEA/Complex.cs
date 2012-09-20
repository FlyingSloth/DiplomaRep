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

		// +: compl+compl, compl+double,double+compl
		public static Complex operator +(Complex c1, Complex c2)
		{
			return new Complex(c1.re + c2.re, c1.im + c2.im);
		}

		public static Complex operator +(Complex c1, double d)
		{
			return new Complex(c1.re + d, c1.im);
		}

		public static Complex operator +(double d, Complex c1)
		{
			return new Complex(c1.re + d, c1.im);
		}

		// -: compl-compl, compl-double,double-compl, -compl
		public static Complex operator -(Complex c1, Complex c2)
		{
			return new Complex(c1.re - c2.re, c1.im - c2.im);
		}

		public static Complex operator -(Complex c1, double d)
		{
			return new Complex(c1.re - d, c1.im);
		}

		public static Complex operator -(double d, Complex c1)
		{
			return new Complex(d - c1.re, - c1.im);
		}

		public static Complex operator -(Complex c1)
		{
			return new Complex(-c1.re, -c1.im);
		}

		// *: compl*compl, compl*double,double*compl
		public static Complex operator *(Complex c1, Complex c2)
		{
			return new Complex(c1.re*c2.re-c1.im*c2.im, c1.re*c2.im+c2.re*c1.im);
		}

		public static Complex operator *(Complex c1, double d)
		{
			return new Complex(c1.re*d, c1.im*d);
		}

		public static Complex operator *(double d, Complex c1)
		{
			return new Complex(d*c1.re, d*c1.im);
		}

		// /: compl/compl, compl/double,double/compl
		public static Complex operator /(Complex c1, Complex c2)
		{
			double del = Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2);
			return new Complex(Convert.ToDouble(c1.re * c2.re + c1.im * c2.im) / del, Convert.ToDouble(c1.im * c2.re - c1.re * c2.im) / del);
		}

		public static Complex operator /(Complex c1, double d)
		{
			return new Complex(c1.re / d, c1.im / d);
		}

		public static Complex operator /(double d, Complex c2)
		{
			double del = Math.Pow(c2.re, 2) + Math.Pow(c2.im, 2);
			return new Complex(Convert.ToDouble(d*c2.re) / del, Convert.ToDouble(-d*c2.im) / del);
		}

		//Eiler
		public double Abs()
		{
			return Math.Sqrt(Math.Pow(this.re, 2) + Math.Pow(this.im, 2));
		}

		public double Arg()
		{
			return (180 / Math.PI) * Math.Atan(this.im / this.re);
		}

		//comparsion (according to MATLAB)
		public static bool operator >(Complex c1, Complex c2)
		{
			double a1 = c1.re;
			double a2 = c2.re;
			double b1 = c1.im;
			double b2 = c2.im;
			
			if (c1.Abs() < c2.Abs()) return false;
			if (c1.Abs() > c2.Abs()) return true;
			if (c1.Abs() == c2.Abs())
			{
				if ((a1 > 0 && a2 > 0 && b1 > 0 && b2 > 0) || (a1 < 0 && a2 < 0 && b1 < 0 && b2 < 0))
				{
					if (c1.Arg() > c2.Arg()) return true;
					return false;
				}
				if ((a1 < 0 && a2 > 0 && b1 > 0 && b2 > 0))
				{
					return true;
				}
				if ((a1 > 0 && a2 < 0 && b1 < 0 && b2 < 0))
				{
					return true;
				}
				if ((a1 < 0 && a2 < 0 && b1 > 0 && b2 > 0) || (a1 > 0 && a2 > 0 && b1 < 0 && b2 < 0))
				{
					if (Math.Abs(c1.Arg()) < Math.Abs(c2.Arg())) return true;
					return false;
				}
				if (b1 > 0 && b2 < 0) return true;
				if (a1 == 0 || a2 == 0)
				{
					if (c1.im > c2.im) return true;
					return false;
				}
				if (b1 == 0 || b2 == 0)
				{
					if (c1.re > c2.re) return true;
					return false;
				}
				return false;
			}
			return false;
		}

		public static bool operator <(Complex c1, Complex c2)
		{
			double a1 = c1.re;
			double a2 = c2.re;
			double b1 = c1.im;
			double b2 = c2.im;

			if (c1.Abs() < c2.Abs()) return true;
			if (c1.Abs() > c2.Abs()) return false;
			if (c1.Abs() == c2.Abs())
			{
				if ((a1 > 0 && a2 > 0 && b1 > 0 && b2 > 0) || (a1 < 0 && a2 < 0 && b1 < 0 && b2 < 0))
				{
					if (c1.Arg() < c2.Arg()) return true;
					return false;
				}
				if ((a1 < 0 && a2 > 0 && b1 > 0 && b2 > 0))
				{
					return false;
				}
				if ((a1 > 0 && a2 < 0 && b1 < 0 && b2 < 0))
				{
					return false;
				}
				if ((a1 < 0 && a2 < 0 && b1 > 0 && b2 > 0) || (a1 > 0 && a2 > 0 && b1 < 0 && b2 < 0))
				{
					if (Math.Abs(c1.Arg()) > Math.Abs(c2.Arg())) return true;
					return false;
				}
				if (b1 > 0 && b2 < 0) return true;
				if (a1 == 0 || a2 == 0)
				{
					if (c1.im < c2.im) return true;
					return false;
				}
				if (b1 == 0 || b2 == 0)
				{
					if (c1.re < c2.re) return true;
					return false;
				}
				return false;
			}
			return false;
		}

		public static bool operator ==(Complex c1, Complex c2)
		{
			if (c1.re == c2.re && c1.im == c2.im) return true;
			return false;
		}
		
		//Complex to double
		public double ToDouble()
		{
			if (this.im == 0) return this.re;
			else return 0;
		}
	}
}
