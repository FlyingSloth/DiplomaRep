using System;

public class Matrix
{
    private int cols, rows;
    public double[,] matrix;

	public Matrix()
	{
	}

    public Matrix(int n)
    {
		this.matrix = new double[3 * n - 1, 3 * n - 1];
        this.rows = 3 * n - 1;
        this.cols = 3 * n - 1;
		for(int i = 0; i < this.Cols(); i++)
			for (int j = 0; j < this.Rows(); j++)
				this.matrix[i,j] = 0;
    }

	public int Rows()
	{
		return this.rows;
	}

	public int Cols()
	{
		return this.cols;
	}

    public void SetA(int n, double kc, double ec, int mc, double eps, double r)
    {
		double hc = 1.0/n;
		/*
		if (this.matrix == NULL) 
		{
			this.rows = 3*n-2;
			this.cols = 3*n-2;
			this.matrix = new double[3 * n - 2, 3 * n - 2];
		}
		else*/
		{ 
			this.matrix[0,0] = pA22(eps,hc,kc,ec)+ pA11(1,hc,kc,ec);
			this.matrix[0,1] = pA23(eps,hc,mc);
			this.matrix[0,2] = pA25(eps,hc,kc,ec,mc)+ pA14(1,hc,kc,ec,mc);
			this.matrix[1,0] = pA32(eps,hc,mc);
			this.matrix[1,1] = pA33(eps,hc,kc,ec,mc);
			this.matrix[1,2] = pA35(eps,hc,kc,ec);
			this.matrix[2,0] = pA52(eps,hc,kc,ec,mc)+ pA41(1,hc,kc,ec,mc);
			this.matrix[2,1] = pA53(eps,hc,kc,ec);
			this.matrix[2,2] = pA55(eps,hc,ec,mc)+ pA44(1,hc,ec,mc);

			this.matrix[0,3] = pA12(1,hc,kc,ec);
			this.matrix[0,4] = pA13(1,hc,mc);
			this.matrix[0,5] = pA15(1,hc,kc,ec,mc);
			this.matrix[2,3] = pA42(1,hc,kc,ec,mc);
			this.matrix[2,4] = pA43(1,hc,kc,ec);
			this.matrix[2,5] = pA45(1,hc,ec,mc);
			this.matrix[3,0] = pA21(1,hc,kc,ec);
			this.matrix[3,2] = pA24(1,hc,kc,ec,mc);
			this.matrix[3,3] = pA22(1,hc,kc,ec) + pA11(2,hc,kc,ec);
			this.matrix[3,4] = pA23(1,hc,mc);
			this.matrix[3,5] = pA25(1,hc,kc,ec,mc) + pA14(2,hc,kc,ec,mc);
			this.matrix[4,0] = pA31(1,hc,mc);
			this.matrix[4,2] = pA34(1,hc,kc,ec);
			this.matrix[4,3] = pA32(1,hc,mc);
			this.matrix[4,4] = pA33(1,hc,kc,ec,mc);
			this.matrix[4,5] = pA35(1,hc,kc,ec);
			this.matrix[5,0] = pA51(1,hc,kc,ec,mc);
			this.matrix[5,2] = pA54(1,hc,ec,mc);
			this.matrix[5,3] = pA52(1,hc,kc,ec,mc) + pA41(2,hc,kc,ec,mc);
			this.matrix[5,4] = pA53(1,hc,kc,ec);
			this.matrix[5,5] = pA55(1,hc,ec,mc) + pA44(2,hc,ec,mc);

			this.matrix[3,6] = pA12(2,hc,kc,ec);
			this.matrix[3,7] = pA13(2,hc,mc);
			this.matrix[3,8] = pA15(2,hc,kc,ec,mc);
			this.matrix[5,6] = pA42(2,hc,kc,ec,mc);
			this.matrix[5,7] = pA43(2,hc,kc,ec);
			this.matrix[5,8] = pA45(2,hc,ec,mc);
			this.matrix[6,3] = pA21(2,hc,kc,ec);
			this.matrix[6,5] = pA24(2,hc,kc,ec,mc);
			this.matrix[6,6] = pA22(2,hc,kc,ec) + pA11(3,hc,kc,ec);
			this.matrix[6,7] = pA23(2,hc,mc);
			this.matrix[6,8] = pA25(2,hc,kc,ec,mc) + pA14(3,hc,kc,ec,mc);
			this.matrix[7,3] = pA31(2,hc,mc);
			this.matrix[7,5] = pA34(2,hc,kc,ec);
			this.matrix[7,6] = pA32(2,hc,mc);
			this.matrix[7,7] = pA33(2,hc,kc,ec,mc);
			this.matrix[7,8] = pA35(2,hc,kc,ec);
			this.matrix[8,3] = pA51(2,hc,kc,ec,mc);
			this.matrix[8,5] = pA54(2,hc,ec,mc);
			this.matrix[8,6] = pA52(2,hc,kc,ec,mc) + pA41(3,hc,kc,ec,mc);
			this.matrix[8,7] = pA53(2,hc,kc,ec);
			this.matrix[8,8] = pA55(2,hc,ec,mc) + pA44(3,hc,ec,mc);
		}
    }

    public void SetB(int n, double kc, double ec, int mc, double eps, double r)
    {
		//
    }


    //1
    private double pA11(double j, double h, double k, double e)
    {
		return -1.0 / Math.Pow(h,2) * Math.Log((j+1.0)*h, Math.E) - Math.Pow(k,2) * e * j - 3.0 / 2 * Math.Pow(k,2) * e + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + 2 * Math.Pow(k,2) * e * j * Math.Log((j+1.0)*h, Math.E) + Math.Pow(k,2) * e * Math.Log((j+1.0)*h, Math.E) + 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) - 2 * Math.Pow(k,2) * e * j * Math.Log(j*h, Math.E) - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E) - Math.Pow(k,2) * e * Math.Log(j*h, Math.E);
    }
    private double pA12(double j, double h, double k, double e)
    {
		return 1.0 / Math.Pow(h,2) * Math.Log((j+1.0)*h, Math.E) + Math.Pow(k,2) * e * j + 1.0 / 2 * Math.Pow(k,2) * e - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) - Math.Pow(k,2) * e * j * Math.Log((j+1.0)*h, Math.E) - 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * j * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E);
    }
    private double pA13(double j, double h, double m)
    {
		return m * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) / h;
    }
    private double pA14(double j, double h, double k, double e, double m)
    {
		return -1.0 / 2 * k * e * m * (-2 * j - 3.0 + 2 * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + 4 * j * Math.Log((j+1.0)*h, Math.E) + 2 * Math.Log((j+1.0)*h, Math.E) - 2 * Math.Pow(j,2) * Math.Log(j*h, Math.E) - 4 * j * Math.Log(j*h, Math.E) - 2 * Math.Log(j*h, Math.E));
    }
    private double pA15(double j, double h, double k, double e, double m)
    {
		return -1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E));
    }
    //2
    private double pA21(double j, double h, double k, double e)
    {
		return 1.0 * (1.0 / Math.Pow(h,2) * Math.Log((j+1.0)*h, Math.E) + Math.Pow(k,2) * e * j + 1.0 / 2 * Math.Pow(k,2) * e - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) - Math.Pow(k,2) * e * j * Math.Log((j+1.0)*h, Math.E) - 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * j * Math.Log(j*h, Math.E) + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E));
    }
    private double pA22(double j, double h, double k, double e)
    {
		return -1.0 / Math.Pow(h,2) * Math.Log((j+1.0)*h, Math.E) - Math.Pow(k,2) * e * j + 1.0 / 2 * Math.Pow(k,2) * e + Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + 1.0 / Math.Pow(h,2) * Math.Log(j*h, Math.E) - Math.Pow(k,2) * e * Math.Pow(j,2) * Math.Log(j*h, Math.E);
    }
    private double pA23(double j, double h, double m)
    {
		return -m * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) / h;
    }
    private double pA24(double j, double h, double k, double e, double m)
    {
		return 1.0 * (-1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)));
    }
    private double pA25(double j, double h, double k, double e, double m)
    {
		return k * e * m * (j - 1.0 / 2 - Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + Math.Pow(j,2) * Math.Log(j*h, Math.E));
    }
    //3
    private double pA31(double j, double h, double m)
    {
		return -m * 1.0 * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) / h;
    }
    private double pA32(double j, double h, double m)
    {
		return m*1.0*(Math.Log(j+1.0, Math.E)*h-Math.Log(j*h, Math.E))/h;
    }
    private double pA33(double j, double h, double k, double e, double m)
    {
		return (Math.Pow(m,2) * Math.Log((j+1.0)*h, Math.E) - Math.Pow(k,2) * e * j * Math.Pow(h,2) - 1.0 / 2 * Math.Pow(k,2) * e * Math.Pow(h,2) - Math.Pow(m,2) * Math.Log(j*h, Math.E));
    }
    private double pA34(double j, double h, double k, double e)
    {
		return 1.0 / 2 * k * e * h * (2 * j + 1.0);
    }
    private double pA35(double j, double h, double k, double e)
    {
		return -1.0 / 2 * k * e * h * (2 * j + 1.0);
    }
    //4
    private double pA41(double j, double h, double k, double e, double m)
    {
		return -(-1.0 / 2 * k * e * m * (-2 * j - 3.0 + 2 * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + 4 * j * Math.Log((j+1.0)*h, Math.E) + 2 * Math.Log((j+1.0)*h, Math.E) - 2 * Math.Pow(j,2) * Math.Log(j*h, Math.E) - 4 * j * Math.Log(j*h, Math.E) - 2 * Math.Log(j*h, Math.E)));
    }
    private double pA42(double j, double h, double k, double e, double m)
    {
		return -(-1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)));
    }
    private double pA43(double j, double h, double k, double e)
    {
		return 1.0 / 2 * k * e * h * (2 * j + 1.0);
    }
    private double pA44(double j, double h, double e, double m)
    {
		return ((-2 * e * Math.Log((j+1.0)*h, Math.E) * j + e * j - e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + 3.0 / 2 * e - e * Math.Log((j+1.0)*h, Math.E) + e * Math.Pow(j,2) * Math.Log(j*h, Math.E) + e * Math.Log(j*h, Math.E) + 2 * e * Math.Log(j*h, Math.E) * j) * Math.Pow(m,2) - e * j - 1.0 / 2 * e);
    }
    private double pA45(double j, double h, double e, double m)
    {
		return ((e * Math.Log((j+1.0)*h, Math.E) * j - 1.0 / 2 * e - e * j + e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) - e * Math.Pow(j,2) * Math.Log(j*h, Math.E) - e * Math.Log(j*h, Math.E) * j) * Math.Pow(m,2) + 1.0 / 2 * e + e * j);
    }
    //5
    private double pA51(double j, double h, double k, double e, double m)
    {
		return -(1.0 * (-1.0 / 2 * k * e * m + k * e * m * Math.Pow(j,2) * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) - k * e * m * j + k * e * m * j * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E))));
    }
    private double pA52(double j, double h, double k, double e, double m)
    {
		return -(k * e * m * (j - 1.0 / 2 - Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) + Math.Pow(j,2) * Math.Log(j*h, Math.E)));
    }
    private double pA53(double j, double h, double k, double e)
    {
		return -1.0 * (1.0 / 2 * k * e * h * (2 * j + 1.0));
    }
    private double pA54(double j, double h, double e, double m)
    {
		return (e * Math.Log((j+1.0)*h, Math.E) * j - 1.0 / 2 * e - e * j + e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) - e * Math.Pow(j,2) * Math.Log(j*h, Math.E) - e * Math.Log(j*h, Math.E) * j) * Math.Pow(m,2) + 1.0 / 2 * e + e * j;
    }
    private double pA55(double j, double h, double e, double m)
    {
		return (e * j - e * Math.Pow(j,2) * Math.Log((j+1.0)*h, Math.E) - 1.0 / 2 * e + e * Math.Pow(j,2) * Math.Log(j*h, Math.E)) * Math.Pow(m,2) - e * j - 1.0 / 2 * e;
    }

    //B
    private double pB11(double j, double h)
    {
		return -3.0 / 2 - j + Math.Pow(j,2) * (Math.Log((j+1.0)*h, Math.E) - Math.Log(j*h, Math.E)) + (2 * Math.Log((j+1.0)*h, Math.E) - 2 * Math.Log(j*h, Math.E)) * j - Math.Log(j*h, Math.E) + Math.Log((j+1.0)*h, Math.E);
    }
    private double pB12(double j, double h)
    {
		return 1.0 / 2 + (-Math.Log((j+1.0)*h, Math.E) + Math.Log(j*h, Math.E)) * Math.Pow(j,2) + (Math.Log(j*h, Math.E) + 1.0 - Math.Log((j+1.0)*h, Math.E)) * j;
    }
    private double pB21(double j, double h)
    {
		return (1.0 / 2 + (-Math.Log((j+1.0)*h, Math.E) + Math.Log(j*h, Math.E)) * Math.Pow(j,2) + (Math.Log(j*h, Math.E) + 1.0 - Math.Log((j+1.0)*h, Math.E)) * j);
    }
    private double pB22(double j, double h)
    {
		return 1.0/2-j+Math.Pow(j,2)*(Math.Log(j+1.0, Math.E)*h-Math.Log(j*h, Math.E));
    }
    private double pB33(double j, double h)
    {
		return -(Math.Pow(h,2) * j + 1.0 / 2 * Math.Pow(h,2));
    }
    private double pB44(double j, double h, double e)
    {
		return -(Math.Pow(h,2) * j + 1.0 / 2 * Math.Pow(h,2));
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
}
