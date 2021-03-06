﻿using System;
using FEA;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using ei;
using System.Threading.Tasks;

public class Matrix
{
	#region "Private structures"
	private int cols, rows; // nubler of columns and rows
    public double[,] matrix; // array for matrix
	private const double epsR = 0.000001; // radius of inside rod (to avoid singularity in 0)
	private static double st1 = 0.0001, st2 = 0.0051;
	#endregion
	#region "Constructors"
	public Matrix()
	{
		this.cols = 0;
		this.rows = 0;
	}

	/// <summary>
	/// Square matrix constructor. Fills matrices with 0.
	/// </summary>
	/// <param name="n">Number of columns and rows will be 3*n-2</param>
    public Matrix(int n)
    {
		this.matrix = new double[3 * n - 2, 3 * n - 2];
        this.rows = 3 * n - 2;
        this.cols = 3 * n - 2;
		for(int i = 0; i < this.cols; i++)
			for (int j = 0; j < this.rows; j++)
				this.matrix[i,j] = 0;
    }

	public Matrix(int rows, int cols)
	{
		this.matrix = new double[rows, cols];
		this.rows = rows;
		this.cols = cols;
		for (int i = 0; i < this.rows; i++)
			for (int j = 0; j < this.cols; j++)
				this.matrix[i, j] = 0;
	}
	#endregion
	#region "Rows&Cols"
	/// <summary>
	/// Number of rows
	/// </summary>
	/// <returns></returns>
	public int Rows()
	{
		return this.rows;
	}

	/// <summary>
	/// Number of columns
	/// </summary>
	/// <returns></returns>
	public int Cols()
	{
		return this.cols;
	}
	#endregion
	#region "Final Matrices"
	//TODO: Multilayer
	/// <summary>
	/// Setting of matrix A
	/// </summary>
	/// <param name="n">number of columns and rows</param>
	/// <param name="kc">wave number</param>
	/// <param name="ec">permitivity</param>
	/// <param name="mc">mode</param>
	/// <param name="r">Width of layer</param>
    public void SetA(int n, double kc, int mc, WorkObject.LAY[] layers)
    {
		double eps = epsR;
		double hc = 1.0/n;
		double ec = layers[0].perm;
		double ec1 = 0;
		if (this.Cols() == 0 || this.Rows() == 0) 
		{
			this.rows = 3*n-2;
			this.cols = 3*n-2;
			this.matrix = new double[3 * n - 2, 3 * n - 2];
		}
		
		{ 
			this.matrix[0, 0] = (pA22(eps,hc,kc,ec)+ pA11(1,hc,kc,ec));
			this.matrix[0, 1] = (pA23(eps, hc, mc));
			this.matrix[0, 2] = (pA25(eps, hc, kc, ec, mc) + pA14(1, hc, kc, ec, mc));
			this.matrix[1, 0] = (pA32(eps, hc, mc));
			this.matrix[1, 1] = (pA33(eps, hc, kc, ec, mc));
			this.matrix[1, 2] = (pA35(eps, hc, kc, ec));
			this.matrix[2, 0] = (pA52(eps, hc, kc, ec, mc) + pA41(1, hc, kc, ec, mc));
			this.matrix[2, 1] = (pA53(eps, hc, kc, ec));
			this.matrix[2, 2] = (pA55(eps, hc, ec, mc) + pA44(1, hc, ec, mc));

			this.matrix[0, 3] = (pA12(1, hc, kc, ec));
			this.matrix[0, 4] = (pA13(1, hc, mc));
			this.matrix[0, 5] = (pA15(1, hc, kc, ec, mc));
			this.matrix[2, 3] = (pA42(1, hc, kc, ec, mc));
			this.matrix[2, 4] = (pA43(1, hc, kc, ec));
			this.matrix[2, 5] = (pA45(1, hc, ec, mc));
			this.matrix[3, 0] = (pA21(1, hc, kc, ec));
			this.matrix[3,2] = (pA24(1,hc,kc,ec,mc));
			this.matrix[3,3] = (pA22(1,hc,kc,ec) + pA11(2,hc,kc,ec));
			this.matrix[3,4] = (pA23(1,hc,mc));
			this.matrix[3,5] = (pA25(1,hc,kc,ec,mc) + pA14(2,hc,kc,ec,mc));
			this.matrix[4,0] = (pA31(1,hc,mc));
			this.matrix[4,2] = (pA34(1,hc,kc,ec));
			this.matrix[4,3] = (pA32(1,hc,mc));
			this.matrix[4,4] = (pA33(1,hc,kc,ec,mc));
			this.matrix[4,5] = (pA35(1,hc,kc,ec));
			this.matrix[5,0] = (pA51(1,hc,kc,ec,mc));
			this.matrix[5,2] = (pA54(1,hc,ec,mc));
			this.matrix[5,3] = (pA52(1,hc,kc,ec,mc) + pA41(2,hc,kc,ec,mc));
			this.matrix[5,4] = (pA53(1,hc,kc,ec));
			this.matrix[5,5] = (pA55(1,hc,ec,mc) + pA44(2,hc,ec,mc));

			this.matrix[3,6] = (pA12(2,hc,kc,ec));
			this.matrix[3,7] = (pA13(2,hc,mc));
			this.matrix[3,8] = (pA15(2,hc,kc,ec,mc));
			this.matrix[5,6] = (pA42(2,hc,kc,ec,mc));
			this.matrix[5,7] = (pA43(2,hc,kc,ec));
			this.matrix[5,8] = (pA45(2,hc,ec,mc));
			this.matrix[6,3] = (pA21(2,hc,kc,ec));
			this.matrix[6,5] = (pA24(2,hc,kc,ec,mc));
			this.matrix[6,6] = (pA22(2,hc,kc,ec) + pA11(3,hc,kc,ec));
			this.matrix[6,7] = (pA23(2,hc,mc));
			this.matrix[6,8] = (pA25(2,hc,kc,ec,mc) + pA14(3,hc,kc,ec,mc));
			this.matrix[7,3] = (pA31(2,hc,mc));
			this.matrix[7,5] = (pA34(2,hc,kc,ec));
			this.matrix[7,6] = (pA32(2,hc,mc));
			this.matrix[7,7] = (pA33(2,hc,kc,ec,mc));
			this.matrix[7,8] = (pA35(2,hc,kc,ec));
			this.matrix[8,3] = (pA51(2,hc,kc,ec,mc));
			this.matrix[8,5] = (pA54(2,hc,ec,mc));
			this.matrix[8,6] = (pA52(2,hc,kc,ec,mc) + pA41(3,hc,kc,ec,mc));
			this.matrix[8,7] = (pA53(2,hc,kc,ec));
			this.matrix[8,8] = (pA55(2,hc,ec,mc) + pA44(3,hc,ec,mc));

            for (int i1 = 1; i1 < n-3; i1++)
			{
				for (int ii = 1; ii < layers.Length; ii++)
				{
					ec1 = ec;
					if ((i1 + 3) * hc > layers[ii-1].R - st2)
						ec1 = layers[ii].perm;
					if ((i1 + 3) * hc > layers[ii-1].R - st1)
						ec = layers[ii].perm;
				}
				this.matrix[3 + i1 * 3, 6 + i1 * 3] = (pA12(2 + i1, hc, kc, ec));
				this.matrix[3 + i1 * 3, 7 + i1 * 3] = (pA13(2 + i1, hc, mc));
				this.matrix[3 + i1 * 3, 8 + i1 * 3] = (pA15(2 + i1, hc, kc, ec, mc));
				this.matrix[5 + i1 * 3, 6 + i1 * 3] = (pA42(2 + i1, hc, kc, ec, mc));
				this.matrix[5 + i1 * 3, 7 + i1 * 3] = (pA43(2 + i1, hc, kc, ec));
				this.matrix[5 + i1 * 3, 8 + i1 * 3] = (pA45(2 + i1, hc, ec, mc));
				this.matrix[6 + i1 * 3, 3 + i1 * 3] = (pA21(2 + i1, hc, kc, ec));
				this.matrix[6 + i1 * 3, 5 + i1 * 3] = (pA24(2 + i1, hc, kc, ec, mc));
				this.matrix[6 + i1 * 3, 6 + i1 * 3] = (pA22(2 + i1, hc, kc, ec) + pA11(3 + i1, hc, kc, ec1));
				this.matrix[6 + i1 * 3, 7 + i1 * 3] = (pA23(2 + i1, hc, mc));
				this.matrix[6 + i1 * 3, 8 + i1 * 3] = (pA25(2 + i1, hc, kc, ec, mc) + pA14(3 + i1, hc, kc, ec1, mc));
				this.matrix[7 + i1 * 3, 3 + i1 * 3] = (pA31(2 + i1, hc, mc));
				this.matrix[7 + i1 * 3, 5 + i1 * 3] = (pA34(2 + i1, hc, kc, ec));
				this.matrix[7 + i1 * 3, 6 + i1 * 3] = (pA32(2 + i1, hc, mc));
				this.matrix[7 + i1 * 3, 7 + i1 * 3] = (pA33(2 + i1, hc, kc, ec, mc));
				this.matrix[7 + i1 * 3, 8 + i1 * 3] = (pA35(2 + i1, hc, kc, ec));
				this.matrix[8 + i1 * 3, 3 + i1 * 3] = (pA51(2 + i1, hc, kc, ec, mc));
				this.matrix[8 + i1 * 3, 5 + i1 * 3] = (pA54(2 + i1, hc, ec, mc));
				this.matrix[8 + i1 * 3, 6 + i1 * 3] = (pA52(2 + i1, hc, kc, ec, mc) + pA41(3 + i1, hc, kc, ec1, mc));
				this.matrix[8 + i1 * 3, 7 + i1 * 3] = (pA53(2 + i1, hc, kc, ec));
				this.matrix[8 + i1 * 3, 8 + i1 * 3] = (pA55(2 + i1, hc, ec, mc) + pA44(3 + i1, hc, ec1, mc));
			}

			this.matrix[6 + 3 * (n - 4), 9 + 3 * (n - 4)] = (pA13(3 + n - 4, hc, mc));
			this.matrix[8 + 3 * (n - 4), 9 + 3 * (n - 4)] = (pA43(3 + n - 4, hc, kc, ec));
			this.matrix[9 + 3 * (n - 4), 6 + 3 * (n - 4)] = (pA31(3 + n - 4, hc, mc));
			this.matrix[9 + 3 * (n - 4), 8 + 3 * (n - 4)] = (pA34(3 + n - 4, hc, kc, ec));
			this.matrix[9 + 3 * (n - 4), 9 + 3 * (n - 4)] = (pA33(3 + n - 4, hc, kc, ec, mc));
		}
    }

	/// <summary>
	/// Setting of matrix B
	/// </summary>
	/// <param name="n">number of columns and rows</param>
	/// <param name="kc">wave number</param>
	/// <param name="ec">permitivity</param>
	/// <param name="mc">mode</param>
	/// <param name="r">Width of layer</param>
    public void SetB(int n, double kc, int mc, WorkObject.LAY[] layers)
    {
		double eps = epsR;
		double hc = 1.0/n;
		double ec = layers[0].perm;
		double ec1 = 0;
		if (this.Cols() == 0 || this.Rows() == 0)
		{
			this.rows = 3 * n - 2;
			this.cols = 3 * n - 2;
			this.matrix = new double[3 * n - 2, 3 * n - 2];
		}
		
		{
			this.matrix[0, 0] = (pB22(eps, hc) + pB11(1, hc));
			this.matrix[1, 1] = (pB33(eps, hc));
			this.matrix[2, 2] = (pB55(eps, hc, ec) + pB44(1, hc, ec));

			this.matrix[0, 3] = (pB12(1, hc));
			this.matrix[2, 5] = (pB45(1, hc, ec));
			this.matrix[3, 0] = (pB21(1, hc));
			this.matrix[3, 3] = (pB22(1, hc) + pB11(2, hc));
			this.matrix[4, 4] = (pB33(1, hc));
			this.matrix[5, 2] = (pB54(1, hc, ec));
			this.matrix[5, 5] = (pB55(1, hc, ec) + pB44(2, hc, ec));

			this.matrix[3, 6] = (pB12(2, hc));
			this.matrix[5, 8] = (pB45(2, hc, ec));
			this.matrix[6, 3] = (pB21(2, hc));
			this.matrix[6, 6] = (pB22(2, hc) + pB11(3, hc));
			this.matrix[7, 7] = (pB33(2, hc));
			this.matrix[8, 5] = (pB54(2, hc, ec));
			this.matrix[8, 8] = (pB55(2, hc, ec) + pB44(3, hc, ec));

            for (int i1 = 1; i1 < n-3; i1++)
			{
				for (int ii = 1; ii < layers.Length; ii++)
				{
					ec1 = ec;
					if ((i1 + 3) * hc > layers[ii - 1].R - st2)
						ec1 = layers[ii].perm;
					if ((i1 + 3) * hc > layers[ii - 1].R - st1)
						ec = layers[ii].perm;
				}
				this.matrix[3 + i1*3,6 + i1*3] = (pB12(2 + i1,hc));
				this.matrix[5 + i1*3,8 + i1*3] = (pB45(2 + i1,hc,ec));
				this.matrix[6 + i1*3,3 + i1*3] = (pB21(2 + i1,hc));
				this.matrix[6 + i1*3,6 + i1*3] = (pB22(2 + i1,hc) + pB11(3 + i1,hc));
				this.matrix[7 + i1*3,7 + i1*3] = (pB33(2 + i1,hc));
				this.matrix[8 + i1*3,5 + i1*3] = (pB54(2 + i1,hc,ec));
				this.matrix[8 + i1*3,8 + i1*3] = (pB55(2 + i1,hc,ec) + pB44(3 + i1,hc,ec1));
			}

			this.matrix[6 + 3*(n-4),9 + 3*(n-4)] = (pB12(3 + n-4,hc));
			this.matrix[9 + 3*(n-4),6 + 3*(n-4)] = (pB21(3 + n-4,hc));
			this.matrix[9 + 3*(n-4),9 + 3*(n-4)] = (pB33(3 + n-4,hc));
		}
    }
	#endregion
	#region "Initial matrices"
	//elements of elementary matrices 5x5
    //A 1 row
    private double pA11(double j, double h, double k, double e)
    {
		return -1.0 / Math.Pow(h,2) * Math.Log((j+1)*h, Math.E) - Math.Pow(k,2) * e * j - 3.0 / 2 * Math.Pow(k,2) * e + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + 2 * Math.Pow(k,2) * e * j * Math.Log((j+1)*h, Math.E) + Math.Pow(k,2) * e * Math.Log((j+1)*h, Math.E) + 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) - 2 * Math.Pow(k,2) * e * j * Math.Log(j*h, Math.E) - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E) - Math.Pow(k,2) * e * Math.Log(j*h, Math.E);
    }
    private double pA12(double j, double h, double k, double e)
    {
		return 1.0 / Math.Pow(h,2) * Math.Log((j+1)*h, Math.E) + Math.Pow(k,2) * e * j + 1.0 / 2 * Math.Pow(k,2) * e - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) - Math.Pow(k,2) * e * j * Math.Log((j+1)*h, Math.E) - 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * j * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E);
    }
    private double pA13(double j, double h, double m)
    {
		return Convert.ToDouble(m * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E))) / h;
    }
    private double pA14(double j, double h, double k, double e, double m)
    {
		return -1.0 / 2 * k * e * m * (-2 * j - 3.0 + 2 * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + 4 * j * Math.Log((j+1)*h, Math.E) + 2 * Math.Log((j+1)*h, Math.E) - 2 * Math.Pow(j,2) * Math.Log(j*h, Math.E) - 4 * j * Math.Log(j*h, Math.E) - 2 * Math.Log(j*h, Math.E));
    }
    private double pA15(double j, double h, double k, double e, double m)
    {
		return -1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E));
    }
	//A 2 row
    private double pA21(double j, double h, double k, double e)
    {
		return 1.0 * (1.0 / Math.Pow(h,2) * Math.Log((j+1)*h, Math.E) + Math.Pow(k,2) * e * j + 1.0 / 2 * Math.Pow(k,2) * e - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) - Math.Pow(k,2) * e * j * Math.Log((j+1)*h, Math.E) - 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * j * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E));
    }
    private double pA22(double j, double h, double k, double e)
    {
		return -1.0 / Math.Pow(h,2) * Math.Log((j+1)*h, Math.E) - Math.Pow(k,2) * e * j + 1.0 / 2 * Math.Pow(k,2) * e + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E);
    }
	private double pA23(double j, double h, double m)
    {
		return (-m * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) / h);
    }
    private double pA24(double j, double h, double k, double e, double m)
    {
		return 1.0 * (-1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)));
    }
    private double pA25(double j, double h, double k, double e, double m)
    {
		return k * e * m * (j - 1.0 / 2 - Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + Math.Pow(j,2) * Math.Log(j*h, Math.E));
    }
	//A 3 row
    private double pA31(double j, double h, double m)
    {
		return -m * 1.0 * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) / h;
    }
    private double pA32(double j, double h, double m)
    {
		return (m * (Math.Log((j + 1) * h, Math.E) - Math.Log(j * h, Math.E)) / h);
    }
    private double pA33(double j, double h, double k, double e, double m)
    {
		return (Math.Pow(m,2) * Math.Log((j+1)*h, Math.E) - Math.Pow(k,2) * e * j * Math.Pow(h,2) - 1.0 / 2 * Math.Pow(k,2) * e * Math.Pow(h,2) - Math.Pow(m,2) * Math.Log(j*h, Math.E));
    }
    private double pA34(double j, double h, double k, double e)
    {
		return 1.0 / 2 * k * e * h * (2 * j + 1.0);
    }
    private double pA35(double j, double h, double k, double e)
    {
		return -1.0 / 2 * k * e * h * (2 * j + 1.0);
    }
	//A 4 row
    private double pA41(double j, double h, double k, double e, double m)
    {
		return -(-1.0 / 2 * k * e * m * (-2 * j - 3.0 + 2 * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + 4 * j * Math.Log((j+1)*h, Math.E) + 2 * Math.Log((j+1)*h, Math.E) - 2 * Math.Pow(j,2) * Math.Log(j*h, Math.E) - 4 * j * Math.Log(j*h, Math.E) - 2 * Math.Log(j*h, Math.E)));
    }
    private double pA42(double j, double h, double k, double e, double m)
    {
		return -(-1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)));
    }
    private double pA43(double j, double h, double k, double e)
    {
		return 1.0 / 2 * k * e * h * (2 * j + 1.0);
    }
    private double pA44(double j, double h, double e, double m)
    {
		return ((-2 * e * Math.Log((j+1)*h, Math.E) * j + e * j - e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + 3.0 / 2 * e - e * Math.Log((j+1)*h, Math.E) + e * Math.Pow(j,2) * Math.Log(j*h, Math.E) + e * Math.Log(j*h, Math.E) + 2 * e * Math.Log(j*h, Math.E) * j) * Math.Pow(m,2) - e * j - 1.0 / 2 * e);
    }
    private double pA45(double j, double h, double e, double m)
    {
		return ((e * Math.Log((j+1)*h, Math.E) * j - 1.0 / 2 * e - e * j + e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) - e * Math.Pow(j,2) * Math.Log(j*h, Math.E) - e * Math.Log(j*h, Math.E) * j) * Math.Pow(m,2) + 1.0 / 2 * e + e * j);
    }
	//A 5 row
    private double pA51(double j, double h, double k, double e, double m)
    {
		return -(1.0 * (-1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E))));
    }
    private double pA52(double j, double h, double k, double e, double m)
    {
		return -(k * e * m * (j - 1.0 / 2 - Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) + Math.Pow(j,2) * Math.Log(j*h, Math.E)));
    }
    private double pA53(double j, double h, double k, double e)
    {
		return -1.0 * (1.0 / 2 * k * e * h * (2 * j + 1.0));
    }
    private double pA54(double j, double h, double e, double m)
    {
		return (e * Math.Log((j+1)*h, Math.E) * j - 1.0 / 2 * e - e * j + e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) - e * Math.Pow(j,2) * Math.Log(j*h, Math.E) - e * Math.Log(j*h, Math.E) * j) * Math.Pow(m,2) + 1.0 / 2 * e + e * j;
    }
    private double pA55(double j, double h, double e, double m)
    {
		return (e * j - e * Math.Pow(j,2) * Math.Log((j+1)*h, Math.E) - 1.0 / 2 * e + e * Math.Pow(j,2) * Math.Log(j*h, Math.E)) * Math.Pow(m,2) - e * j - 1.0 / 2 * e;
    }

    //B
	
    private double pB11(double j, double h)
    {
		return -1.5 - j + Math.Pow(j,2) * (Math.Log((j+1)*h, Math.E) - Math.Log(j*h, Math.E)) + (2 * Math.Log((j+1)*h, Math.E) - 2 * Math.Log(j*h, Math.E)) * j - Math.Log(j*h, Math.E) + Math.Log((j+1)*h, Math.E);
    }
    private double pB12(double j, double h)
    {
		return 1.0 / 2 + (-Math.Log((j+1)*h, Math.E) + Math.Log(j*h, Math.E)) * Math.Pow(j,2) + (Math.Log(j*h, Math.E) + 1.0 - Math.Log((j+1)*h, Math.E)) * j;
    }
    private double pB21(double j, double h)
    {
		return (1.0 / 2 + (-Math.Log((j+1)*h, Math.E) + Math.Log(j*h, Math.E)) * Math.Pow(j,2) + (Math.Log(j*h, Math.E) + 1.0 - Math.Log((j+1)*h, Math.E)) * j);
    }
	private double pB22(double j, double h)
    {
		return 0.5-j+Math.Pow(j,2)*(Math.Log((j+1)*h, Math.E)-Math.Log(j*h, Math.E));
    }
    private double pB33(double j, double h)
    {
		return -(Math.Pow(h,2) * j + 1.0 / 2 * Math.Pow(h,2));
    }
	private double pB44(double j, double h, double e)
    {
		return 1.0 / 12 * e * Math.Pow(h, 2) * (4 * j + 1.0);
    }
    private double pB45(double j, double h, double e)
    {
		return 1.0 / 12 * e * Math.Pow(h,2) * (2 * j + 1.0);
    }
    private double pB54(double j, double h, double e)
    {
		return 1.0 / 12 * e * Math.Pow(h,2) * (2 * j + 1.0);
    }
	private double pB55(double j, double h, double e)
    {
		return 1.0 / 12 * e * Math.Pow(h,2) * (4 * j + 3.0);
    }
	#endregion
	#region "Copy&Inverse"
	public Matrix Copy(Matrix M)
		{
			this.cols = M.cols;
			this.rows = M.rows;
			this.matrix = new double[this.rows, this.cols];

			for (int i = 0; i < M.rows; i++)
				for (int j = 0; j < M.cols; j++)
					this.matrix[i, j] = M.matrix[i, j];
			return this;
		}

	/// <summary>
	/// Returns inverted matrix
	/// </summary>
	/// <returns></returns>
	public Matrix Invert()
	{ 
		int n = this.rows;
        int[] row = new int[n];
        int[] col = new int[n];
        double[] temp = new double[n];
        int hold; // buffer
		int I_majel, J_majel; // place of major element
        double majel, abs_majel;// major element

        // rows and cols are gradient
        for (int k = 0; k < n; k++) 
		{
            row[k] = k;
            col[k] = k;
        }
        
        for (int k = 0; k < n; k++) 
		{
            // finding the major element
            majel = this.matrix[row[k],col[k]];//.ToDouble();
            I_majel = k;
            J_majel = k;
            for (int i = k; i < n; i++) 
			{
                for (int j = k; j < n; j++) 
				{
                    abs_majel = Math.Abs(majel);
                    if (Math.Abs(this.matrix[row[i],col[j]]) > abs_majel) 
					{
                        I_majel = i;
                        J_majel = j;
                        majel = this.matrix[row[i],col[j]];
                    }
                }
            }
            if (Math.Abs(majel) < 1.0E-10)
			{
				System.Windows.Forms.MessageBox.Show("Singularity!");
            }

            // changing places of k-row and k-col with majel
            hold = row[k];
            row[k] = row[I_majel];
            row[I_majel] = hold;
            hold = col[k];
            col[k] = col[J_majel];
            col[J_majel] = hold;
            // division of k-row on majel
            this.matrix[row[k],col[k]] = (1.0 / majel);
            for (int j = 0; j < n; j++) 
			{
                if (j != k) 
				{
                    this.matrix[row[k],col[j]] = this.matrix[row[k],col[j]] * this.matrix[row[k],col[k]];
                }
            }
            
            for (int i = 0; i < n; i++) 
			{
                if (k != i) {
                    for (int j = 0; j < n; j++) 
					{
                        if (k != j) 
						{
							this.matrix[row[i],col[j]] = this.matrix[row[i],col[j]] - this.matrix[row[i],col[k]] * this.matrix[row[k],col[j]];
                        }
                    }
					this.matrix[row[i],col[k]] = -this.matrix[row[i],col[k]] * this.matrix[row[k],col[k]];
                }
            }
        }

        // putting back rows
        for (int j = 0; j < n; j++) 
		{
            for (int i = 0; i < n; i++) 
			{
				temp[col[i]] = this.matrix[row[i],j];
            }
            for (int i = 0; i < n; i++) 
			{
				this.matrix[i,j] =(temp[i]);
            }
        }
        // putting back cols
        for (int i = 0; i < n; i++) 
		{
            for (int j = 0; j < n; j++) 
			{
				temp[row[j]] = this.matrix[i,col[j]];
            }
            for (int j = 0; j < n; j++) 
			{
				this.matrix[i,j] = (temp[j]);
            }
        }
		return this;
	}
	#endregion
	#region "Standart matrix operation"
	public static Matrix operator + (Matrix M1, Matrix M2)
	{ 
		if (M1.cols != M2.cols || M1.rows != M2.rows)
		{
			System.Windows.Forms.MessageBox.Show("Matrix's sizes don't match!");
		};
		Matrix res = new Matrix();
		res.matrix = new double[M1.rows, M1.cols];

		int i, j;
		for (i = 0; i < M1.rows; i++)
		{
			for (j = 0; j < M1.cols; j++)
			{
				res.matrix[i,j] = M1.matrix[i, j] + M2.matrix[i, j];
			}
		}
		return res;
	}

	public static Matrix operator +(Matrix M1, double d)
	{
		if (M1.cols != M1.rows)
		{
			System.Windows.Forms.MessageBox.Show("Matrix's sizes don't match!");
		};
		Matrix res = new Matrix();
		res.matrix = new double[M1.rows, M1.cols];

		int i, j;
		for (i = 0; i < M1.rows; i++)
		{
			for (j = 0; j < M1.cols; j++)
			{
				if (i == j)
					res.matrix[i, j] = M1.matrix[i, j] + d;
			}
		}
		return res;
	}

	public static Matrix operator -(Matrix M1, Matrix M2)
	{
		if (M1.cols != M2.cols || M1.rows != M2.rows)
		{
			System.Windows.Forms.MessageBox.Show("Matrix's sizes don't match!");
		};
		Matrix res = new Matrix();
		res.matrix = new double[M1.rows, M1.cols];

		int i, j;
		for (j = 0; j < M1.rows; j++)
		{
			for (i = 0; i < M1.cols; i++)
			{
				res.matrix[j, i] = M1.matrix[j, i] - M2.matrix[j, i];
			}
		}
		return res;
	}

	public static Matrix operator -(Matrix M1)
	{
		int i, j;
		for (i = 0; i < M1.rows; i++)
		{
			for (j = 0; j < M1.cols; j++)
			{
				M1.matrix[i,j] *= -1;
			}
		}
		return M1;
	}

	public static Matrix operator * (Matrix M1, Matrix M2)
	{
		if (M1.cols != M2.rows)
		{
			System.Windows.Forms.MessageBox.Show("Matrix's sizes don't match!");
		};

		Matrix res = new Matrix();
		res.matrix = new double[M1.rows, M2.cols];

		res.cols = M2.cols;
		res.rows = M1.rows;

		int i, j, k;

		for (i = 0; i < M1.rows; i++)
		{
			for (j = 0; j < M2.cols; j++)
			{
				res.matrix[i, j] = 0;
				for (k = 0; k < M1.cols; k++)
				{
					res.matrix[i, j] += M1.matrix[i, k] * M2.matrix[k, j];
				}
			}
		}
		return res;
	}

	public static Matrix operator *(double d, Matrix M1)
	{
		for (int i = 0; i < M1.rows; i++)
			for (int j = 0; j < M1.cols; j++)
				M1.matrix[i, j] = d * M1.matrix[i, j];
		return M1;
	}

	public static Matrix operator /(Matrix M1, double d)
	{
		for (int i = 0; i < M1.rows; i++)
			for (int j = 0; j < M1.cols; j++)
				M1.matrix[i, j] = M1.matrix[i, j]/d;
		return M1;
	}

	public static bool operator ==(Matrix M1, Matrix M2)
	{
		if (M1.cols != M2.cols || M1.rows != M2.rows) return false;
		for (int i = 0; i < M1.rows; i ++)
			for (int j = 0; j < M1.cols; j++)
			{
				if (M1.matrix[i, j] != M2.matrix[i, j]) return false;
			}
		return true;
	}

	public static bool operator !=(Matrix M1, Matrix M2)
	{
		if (M1.cols != M2.cols || M1.rows != M2.rows) return true;
		for (int i = 0; i < M1.rows; i++)
			for (int j = 0; j < M1.cols; j++)
			{
				if (M1.matrix[i, j] != M2.matrix[i, j]) return true;
			}
		return false;
	}
	#endregion
	#region "Eigenvalues"
	//Matlab Arrays for eigenvalues
	private MWArray[] res = null;
	private MWNumericArray real = null;
	private MWNumericArray imag = null;

	private Complex[] eigenvalues;
	
	/// <summary>
	/// Generalised eigenvalues of A and B (analog to MATLAB M = eig(A,B))
	/// </summary>
	/// <param name="B">Matrix B</param>
	/// <returns>Generalised eigenvalues</returns>
	public Complex[] eige(Matrix B)
	{
		//calling Matlab API
		Eig testob = new Eig();
		//Matlab Array which gets result of Matlab function eig(A,B)
		res = testob.Eigenvalues(2, (MWNumericArray)this.matrix, (MWNumericArray)B.matrix, this.Rows());
		//arrays for real and imaginary parts
		real = (MWNumericArray)res[0];
		imag = (MWNumericArray)res[1];
		//copying parts into CSharp arrays
		double[,] resCR = (double[,])real.ToArray(MWArrayComponent.Real);
		double[,] resCI = (double[,])imag.ToArray(MWArrayComponent.Real);

		this.eigenvalues = new Complex[21];
		Complex[] eigenbuf = new Complex[this.rows];
		
		for (int i = 0; i < this.rows; i++)
		{
			eigenbuf[i] = new Complex(resCR[i, 0], resCI[i,0]);
			eigenbuf[i] = eigenbuf[i].Pow(0.5);
		}

		Complex buf = new Complex();
		
		buf.quickSort(ref eigenbuf, 0, this.rows-1);

		for (int i = 0; i < 21; i++)
		{
			this.eigenvalues[i] = eigenbuf[200 + i - 1];
		}
		return this.eigenvalues;
	}
	#endregion
}
