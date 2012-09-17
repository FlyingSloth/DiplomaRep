using System;
using System.Numerics;

public class Matrix
{
    private int cols, rows;
    public double[,] matrix;

	public Matrix()
	{
	}

    public Matrix(int n)
    {
        this.matrix = new double[n, n];
        this.rows = n;
        this.cols = n;
    }

    public void SetA(int n, double kc, double ec, int mc, double eps, double r)
    { 
        
    }

    public void SetB(int n, double kc, double ec, int mc, double eps, double r)
    {

    }


    //1
    private double pA11(double j, double h, double k, double e)
    {
		return -1 / h ^ 2 * log(j * h + h) - k ^ 2 * e * j - 3 / 2 * k ^ 2 * e + k ^ 2 * e * j ^ 2 * log(j * h + h) + 2 * k ^ 2 * e * j * log(j * h + h) + k ^ 2 * e * log(j * h + h) + 1 / h ^ 2 * log(j * h) - 2 * k ^ 2 * e * j * log(j * h) - k ^ 2 * e * j ^ 2 * log(j * h) - k ^ 2 * e * log(j * h);
    }
    private double pA12(double j, double h, double k, double e)
    {
		return 1 / h ^ 2 * log(j * h + h) + k ^ 2 * e * j + 1 / 2 * k ^ 2 * e - k ^ 2 * e * j ^ 2 * log(j * h + h) - k ^ 2 * e * j * log(j * h + h) - 1 / h ^ 2 * log(j * h) + k ^ 2 * e * j * log(j * h) + k ^ 2 * e * j ^ 2 * log(j * h);
    }
    private double pA13(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA14(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA15(double j, double h, double k, double e, double m)
    {
		return 
    }
    //2
    private double pA21(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA22(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA23(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA24(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA25(double j, double h, double k, double e, double m)
    {
		return 
    }
    //3
    private double pA31(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA32(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA33(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA34(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA35(double j, double h, double k, double e, double m)
    {
		return 
    }
    //4
    private double pA41(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA42(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA43(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA44(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA45(double j, double h, double k, double e, double m)
    {
		return 
    }
    //5
    private double pA51(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA52(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA53(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA54(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pA55(double j, double h, double k, double e, double m)
    {
		return 
    }

    //B
    private double pB11(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB12(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB21(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB22(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB33(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB44(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB45(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB54(double j, double h, double k, double e, double m)
    {
		return 
    }
    private double pB55(double j, double h, double k, double e, double m)
    {
		return 
    }
}
