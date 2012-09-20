using System;
using FEA;

public class Matrix
{
    private int cols, rows; // nubler of columns and rows
    public Complex[,] matrix; // array for matrix
	private const double epsR = 0.000001; // radius of inside rod (to avoid singularity in 0)

	public Matrix()
	{
		this.cols = 0;
		this.rows = 0;
	}

	/// <summary>
	/// Square matrix constructor. Fills matrices with 0.
	/// </summary>
	/// <param name="n">Number of columns and rows</param>
    public Matrix(int n)
    {
		this.matrix = new Complex[3 * n - 2, 3 * n - 2];
        this.rows = 3 * n - 2;
        this.cols = 3 * n - 2;
		for(int i = 0; i < this.cols; i++)
			for (int j = 0; j < this.rows; j++)
				this.matrix[i,j] = new Complex();
    }

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

	//TODO: Multilayer
	/// <summary>
	/// Setting of matrix A
	/// </summary>
	/// <param name="n">number of columns and rows</param>
	/// <param name="kc">wave number</param>
	/// <param name="ec">permitivity</param>
	/// <param name="mc">mode</param>
	/// <param name="r">Width of layer</param>
    public void SetA(int n, double kc, double ec, int mc, double r)
    {
		double eps = epsR;
		double hc = 1.0/n;
		double ec1 = 0;
		if (this.Cols() == 0 || this.Rows() == 0) 
		{
			this.rows = 3*n-2;
			this.cols = 3*n-2;
			this.matrix = new Complex[3 * n - 2, 3 * n - 2];
		}
		else
		{ 
			this.matrix[0, 0] = new Complex(pA22(eps,hc,kc,ec)+ pA11(1,hc,kc,ec));
			this.matrix[0, 1] = new Complex(pA23(eps, hc, mc));
			this.matrix[0, 2] = new Complex(pA25(eps, hc, kc, ec, mc) + pA14(1, hc, kc, ec, mc));
			this.matrix[1, 0] = new Complex(pA32(eps, hc, mc));
			this.matrix[1, 1] = new Complex(pA33(eps, hc, kc, ec, mc));
			this.matrix[1, 2] = new Complex(pA35(eps, hc, kc, ec));
			this.matrix[2, 0] = new Complex(pA52(eps, hc, kc, ec, mc) + pA41(1, hc, kc, ec, mc));
			this.matrix[2, 1] = new Complex(pA53(eps, hc, kc, ec));
			this.matrix[2, 2] = new Complex(pA55(eps, hc, ec, mc) + pA44(1, hc, ec, mc));

			this.matrix[0, 3] = new Complex(pA12(1, hc, kc, ec));
			this.matrix[0, 4] = new Complex(pA13(1, hc, mc));
			this.matrix[0, 5] = new Complex(pA15(1, hc, kc, ec, mc));
			this.matrix[2, 3] = new Complex(pA42(1, hc, kc, ec, mc));
			this.matrix[2, 4] = new Complex(pA43(1, hc, kc, ec));
			this.matrix[2, 5] = new Complex(pA45(1, hc, ec, mc));
			this.matrix[3, 0] = new Complex(pA21(1, hc, kc, ec));
			this.matrix[3,2] = new Complex(pA24(1,hc,kc,ec,mc));
			this.matrix[3,3] = new Complex(pA22(1,hc,kc,ec) + pA11(2,hc,kc,ec));
			this.matrix[3,4] = new Complex(pA23(1,hc,mc));
			this.matrix[3,5] = new Complex(pA25(1,hc,kc,ec,mc) + pA14(2,hc,kc,ec,mc));
			this.matrix[4,0] = new Complex(pA31(1,hc,mc));
			this.matrix[4,2] = new Complex(pA34(1,hc,kc,ec));
			this.matrix[4,3] = new Complex(pA32(1,hc,mc));
			this.matrix[4,4] = new Complex(pA33(1,hc,kc,ec,mc));
			this.matrix[4,5] = new Complex(pA35(1,hc,kc,ec));
			this.matrix[5,0] = new Complex(pA51(1,hc,kc,ec,mc));
			this.matrix[5,2] = new Complex(pA54(1,hc,ec,mc));
			this.matrix[5,3] = new Complex(pA52(1,hc,kc,ec,mc) + pA41(2,hc,kc,ec,mc));
			this.matrix[5,4] = new Complex(pA53(1,hc,kc,ec));
			this.matrix[5,5] = new Complex(pA55(1,hc,ec,mc) + pA44(2,hc,ec,mc));

			this.matrix[3,6] = new Complex(pA12(2,hc,kc,ec));
			this.matrix[3,7] = new Complex(pA13(2,hc,mc));
			this.matrix[3,8] = new Complex(pA15(2,hc,kc,ec,mc));
			this.matrix[5,6] = new Complex(pA42(2,hc,kc,ec,mc));
			this.matrix[5,7] = new Complex(pA43(2,hc,kc,ec));
			this.matrix[5,8] = new Complex(pA45(2,hc,ec,mc));
			this.matrix[6,3] = new Complex(pA21(2,hc,kc,ec));
			this.matrix[6,5] = new Complex(pA24(2,hc,kc,ec,mc));
			this.matrix[6,6] = new Complex(pA22(2,hc,kc,ec) + pA11(3,hc,kc,ec));
			this.matrix[6,7] = new Complex(pA23(2,hc,mc));
			this.matrix[6,8] = new Complex(pA25(2,hc,kc,ec,mc) + pA14(3,hc,kc,ec,mc));
			this.matrix[7,3] = new Complex(pA31(2,hc,mc));
			this.matrix[7,5] = new Complex(pA34(2,hc,kc,ec));
			this.matrix[7,6] = new Complex(pA32(2,hc,mc));
			this.matrix[7,7] = new Complex(pA33(2,hc,kc,ec,mc));
			this.matrix[7,8] = new Complex(pA35(2,hc,kc,ec));
			this.matrix[8,3] = new Complex(pA51(2,hc,kc,ec,mc));
			this.matrix[8,5] = new Complex(pA54(2,hc,ec,mc));
			this.matrix[8,6] = new Complex(pA52(2,hc,kc,ec,mc) + pA41(3,hc,kc,ec,mc));
			this.matrix[8,7] = new Complex(pA53(2,hc,kc,ec));
			this.matrix[8,8] = new Complex(pA55(2,hc,ec,mc) + pA44(3,hc,ec,mc));

			for (int i1 = 1; i1 < n-3; i1++)
			{
				ec1 = ec;
				if ((i1 + 3)*hc > r-0.0051)
					ec1 = 1;
				if ((i1 + 3)*hc > r-0.0001)
					ec = 1;

				this.matrix[3 + i1*3,6 + i1*3] = new Complex(pA12(2 + i1,hc,kc,ec));
				this.matrix[3 + i1*3,7 + i1*3] = new Complex(pA13(2 + i1,hc,mc));
				this.matrix[3 + i1*3,8 + i1*3] = new Complex(pA15(2 + i1,hc,kc,ec,mc));
				this.matrix[5 + i1*3,6 + i1*3] = new Complex(pA42(2 + i1,hc,kc,ec,mc));
				this.matrix[5 + i1*3,7 + i1*3] = new Complex(pA43(2 + i1,hc,kc,ec));
				this.matrix[5 + i1*3,8 + i1*3] = new Complex(pA45(2 + i1,hc,ec,mc));
				this.matrix[6 + i1*3,3 + i1*3] = new Complex(pA21(2 + i1,hc,kc,ec));
				this.matrix[6 + i1*3,5 + i1*3] = new Complex(pA24(2 + i1,hc,kc,ec,mc));
				this.matrix[6 + i1*3,6 + i1*3] = new Complex(pA22(2 + i1,hc,kc,ec) + pA11(3 + i1,hc,kc,ec1));
				this.matrix[6 + i1*3,7 + i1*3] = new Complex(pA23(2 + i1,hc,mc));
				this.matrix[6 + i1*3,8 + i1*3] = new Complex(pA25(2 + i1,hc,kc,ec,mc) + pA14(3 + i1,hc,kc,ec1,mc));
				this.matrix[7 + i1*3,3 + i1*3] = new Complex(pA31(2 + i1,hc,mc));
				this.matrix[7 + i1*3,5 + i1*3] = new Complex(pA34(2 + i1,hc,kc,ec));
				this.matrix[7 + i1*3,6 + i1*3] = new Complex(pA32(2 + i1,hc,mc));
				this.matrix[7 + i1*3,7 + i1*3] = new Complex(pA33(2 + i1,hc,kc,ec,mc));
				this.matrix[7 + i1*3,8 + i1*3] = new Complex(pA35(2 + i1,hc,kc,ec));
				this.matrix[8 + i1*3,3 + i1*3] = new Complex(pA51(2 + i1,hc,kc,ec,mc));
				this.matrix[8 + i1*3,5 + i1*3] = new Complex(pA54(2 + i1,hc,ec,mc));
				this.matrix[8 + i1*3,6 + i1*3] = new Complex(pA52(2 + i1,hc,kc,ec,mc) + pA41(3 + i1,hc,kc,ec1,mc));
				this.matrix[8 + i1*3,7 + i1*3] = new Complex(pA53(2 + i1,hc,kc,ec));
				this.matrix[8 + i1*3,8 + i1*3] = new Complex(pA55(2 + i1,hc,ec,mc) + pA44(3 + i1,hc,ec1,mc));
			}

			this.matrix[6 + 3 * (n - 4), 9 + 3 * (n - 4)] = new Complex(pA13(3 + n - 4, hc, mc));
			this.matrix[8 + 3 * (n - 4), 9 + 3 * (n - 4)] = new Complex(pA43(3 + n - 4, hc, kc, ec));
			this.matrix[9 + 3 * (n - 4), 6 + 3 * (n - 4)] = new Complex(pA31(3 + n - 4, hc, mc));
			this.matrix[9 + 3 * (n - 4), 8 + 3 * (n - 4)] = new Complex(pA34(3 + n - 4, hc, kc, ec));
			this.matrix[9 + 3 * (n - 4), 9 + 3 * (n - 4)] = new Complex(pA33(3 + n - 4, hc, kc, ec, mc));
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
    public void SetB(int n, double kc, double ec, int mc, double r)
    {
		double eps = epsR;
		double hc = 1.0/n;
		double ec1 = 0;
		if (this.Cols() == 0 || this.Rows() == 0)
		{
			this.rows = 3 * n - 2;
			this.cols = 3 * n - 2;
			this.matrix = new Complex[3 * n - 2, 3 * n - 2];
		}
		else
		{
			this.matrix[0, 0] = new Complex(pB22(eps, hc) + pB11(1, hc));
			this.matrix[1, 1] = new Complex(pB33(eps, hc));
			this.matrix[2, 2] = new Complex(pB55(eps, hc, ec) + pB44(1, hc, ec));

			this.matrix[0, 3] = new Complex(pB12(1, hc));
			this.matrix[2, 5] = new Complex(pB45(1, hc, ec));
			this.matrix[3, 0] = new Complex(pB21(1, hc));
			this.matrix[3, 3] = new Complex(pB22(1, hc) + pB11(2, hc));
			this.matrix[4, 4] = new Complex(pB33(1, hc));
			this.matrix[5, 2] = new Complex(pB54(1, hc, ec));
			this.matrix[5, 5] = new Complex(pB55(1, hc, ec) + pB44(2, hc, ec));

			this.matrix[3, 6] = new Complex(pB12(2, hc));
			this.matrix[5, 8] = new Complex(pB45(2, hc, ec));
			this.matrix[6, 3] = new Complex(pB21(2, hc));
			this.matrix[6, 6] = new Complex(pB22(2, hc) + pB11(3, hc));
			this.matrix[7, 7] = new Complex(pB33(2, hc));
			this.matrix[8, 5] = new Complex(pB54(2, hc, ec));
			this.matrix[8, 8] = new Complex(pB55(2, hc, ec) + pB44(3, hc, ec));

			for (int i1 = 1; i1 < n - 3; i1++)
			{
				ec1 = ec;
				if ((i1 + 3) * hc > r - 0.0051)
					ec1 = 1;
				if ((i1 + 3) * hc > r - 0.0001)
					ec = 1;
				this.matrix[3 + i1*3,6 + i1*3] = new Complex(pB12(2 + i1,hc));
				this.matrix[5 + i1*3,8 + i1*3] = new Complex(pB45(2 + i1,hc,ec));
				this.matrix[6 + i1*3,3 + i1*3] = new Complex(pB21(2 + i1,hc));
				this.matrix[6 + i1*3,6 + i1*3] = new Complex(pB22(2 + i1,hc) + pB11(3 + i1,hc));
				this.matrix[7 + i1*3,7 + i1*3] = new Complex(pB33(2 + i1,hc));
				this.matrix[8 + i1*3,5 + i1*3] = new Complex(pB54(2 + i1,hc,ec));
				this.matrix[8 + i1*3,8 + i1*3] = new Complex(pB55(2 + i1,hc,ec) + pB44(3 + i1,hc,ec1));
			}
			this.matrix[6 + 3*(n-4),9 + 3*(n-4)] = new Complex(pB12(3 + n-4,hc));
			this.matrix[9 + 3*(n-4),6 + 3*(n-4)] = new Complex(pB21(3 + n-4,hc));
			this.matrix[9 + 3*(n-4),9 + 3*(n-4)] = new Complex(pB33(3 + n-4,hc));
		}
    }

	
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

	public void setTest1(int n)
	{
		this.rows = n;
		this.cols = n;
		this.matrix = new Complex[n, n];
		this.matrix[0, 0] = new Complex(1);
		this.matrix[0, 1] = new Complex(2);
		this.matrix[0, 2] = new Complex(3);
		this.matrix[1, 0] = new Complex(1);
		this.matrix[1, 1] = new Complex(2);
		this.matrix[1, 2] = new Complex(3);
		this.matrix[2, 0] = new Complex(1);
		this.matrix[2, 1] = new Complex(2);
		this.matrix[2, 2] = new Complex(3);
	}

	public void setTest2(int n)
	{
		this.rows = n;
		this.cols = n;
		this.matrix = new Complex[n, n];
		this.matrix[0, 0] = new Complex(3);
		this.matrix[0, 1] = new Complex(2);
		this.matrix[0, 2] = new Complex(1);
		this.matrix[1, 0] = new Complex(3);
		this.matrix[1, 1] = new Complex(2);
		this.matrix[1, 2] = new Complex(1);
		this.matrix[2, 0] = new Complex(3);
		this.matrix[2, 1] = new Complex(2);
		this.matrix[2, 2] = new Complex(1);
	}

	public Matrix Copy(Matrix M)
		{
			this.cols = M.cols;
			this.rows = M.rows;
			this.matrix = new Complex[this.rows, this.cols];

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
            majel = this.matrix[row[k],col[k]].ToDouble();
            I_majel = k;
            J_majel = k;
            for (int i = k; i < n; i++) 
			{
                for (int j = k; j < n; j++) 
				{
                    abs_majel = Math.Abs(majel);
                    if (Math.Abs(this.matrix[row[i],col[j]].ToDouble()) > abs_majel) 
					{
                        I_majel = i;
                        J_majel = j;
                        majel = this.matrix[row[i],col[j]].ToDouble();
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
            this.matrix[row[k],col[k]] = new Complex(1.0 / majel);
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
				temp[col[i]] = this.matrix[row[i],j].ToDouble();
            }
            for (int i = 0; i < n; i++) 
			{
				this.matrix[i,j] = new Complex(temp[i]);
            }
        }
        // putting back cols
        for (int i = 0; i < n; i++) 
		{
            for (int j = 0; j < n; j++) 
			{
				temp[row[j]] = this.matrix[i,col[j]].ToDouble();
            }
            for (int j = 0; j < n; j++) 
			{
				this.matrix[i,j] = new Complex(temp[j]);
            }
        }
		return this;
	}

	public static Matrix operator + (Matrix M1, Matrix M2)
	{ 
		if (M1.cols != M2.cols || M1.rows != M2.rows)
		{
			System.Windows.Forms.MessageBox.Show("Matrix's sizes don't match!");
		};
		Matrix res = new Matrix();
		res.matrix = new Complex[M1.rows, M1.cols];

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
		res.matrix = new Complex[M1.rows, M1.cols];

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
		res.matrix = new Complex[M1.rows, M1.cols];

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
		res.matrix = new Complex[M1.rows, M2.cols];

		res.cols = M2.cols;
		res.rows = M1.rows;

		int i, j, k;

		for (i = 0; i < M1.rows; i++)
		{
			for (j = 0; j < M2.rows; j++)
			{
				res.matrix[i, j] = new Complex();
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

	//TODO: eigenvalues
	/*
	// это первая половина алгоритма, вторую доизучить
	// в оригинале a - изначальная матрица, n - её размерность (ибо квадратная)
	//d - диагональ трёхдиагональной матрицы
	void Eigenval(float **a, int n, float *d, float *e) 
	{ 
		int l,k,j,i; 
		Complex scale,hh,h,g,f; 
		// Проход по стадиям процесса редукции  
		for( i=n; i>=2; i--) 
		{ 
			l=i-1; h=scale=0.; 
			// сложный процесс везде, кроме последней стадии 
			if(l>1) 
			{ 
				// вычислить шкалу 
				for(k=1; k<=l; k++) 
					scale += fabs(a[i][k]); 
				// малая величина шкалы -> пропустить преобразование 
				if(scale==0.) e[i]=a[i][l]; 
				else 
				{ 
					// отмасштабировать строку и вычислить s2 в h  
					for(k=1;k<=l;k++) 
					{ 
						a[i][k]/=scale; h += a[i][k]*a[i][k]; 
					} 
					// вычислить вектор u 
					f=a[i][l]; 
					g=(f>=0.?-sqrt(h):sqrt(h)); 
					e[i]=scale*g; h -= f*g; 
					// записать u на место i-го ряда a  
					a[i][l]=f-g; 
					// вычисление u/h, Au, p, K  
					f=0.; 
					for(j=1;j<=l;j++) 
					{ 
						// следующая инструкция не нужна, если не требуются вектора, 
						//     она содержит загрузку u/h в столбец a 
						a[j][i]=a[i][j]/h; 
						// сформировать элемент Au (в g)  
						g=0.; 
						for(k=1;k<=j;k++) 
							g += a[j][k]*a[i][k]; 
						for(k=j+1;k<=l;k++) 
							g += a[k][j]*a[i][k]; 
						// загрузить элемент p во временно неиспользуемую область e
						e[j]=g/h; 
						// подготовка к формированию K  
						f += e[j]*a[i][j]; 
					} 
					// Сформировать K  
					hh=f/(h+h); 
					for(j=1;j<=l;j++) 
					{ 
						// Сформировать q и поместить на место p (в e) 
						f=a[i][j]; e[j]=g=e[j]-hh*f; 
						// Трансформировать матрицу a 
						for(k=1;k<=j;k++) a[j][k] -= (f*e[k]+g*a[i][k]); 
					} 
				} 
			} 
			else e[i]=a[i][l]; 
			d[i]=h; 
		} 
		// если не нужны собственные вектора, опустите следующую инструкцию  
		d[1]=0.; 
		// эту опускать не надо  
		e[1]=0.; 
		// Все содержание цикла, кроме одной инструкции, можно опустить, если не 
		//требуются собственные вектора 
		for(i=1;i<=n;i++) 
		{ 
			l=i-1; 
			// этот блок будет пропущен при i=1
			if(d[i]!=0.) 
			{ 
				for(j=1;j<=l;j++) 
				{ 
					g=0.; 
					// формируем PQ, используя u и u/H  
					for(k=1;k<=l;k++) 
						g += a[i][k]*a[k][j]; 
					for(k=1;k<=l;k++) 
						a[k][j] -= g*a[k][i]; 
				} 
			} 
			// эта инструкция остается  
			d[i]=a[i][i]; 
			// ряд и колонка матрицы a преобразуются к единичной, для след. итерации  
			a[i][i]=0.; 
			for(j=1;j<=l;j++) 
				a[j][i]=a[i][j]=0.; 
		} 
	} 
	*/
}
