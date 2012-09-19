using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FEA
{
    public partial class frmIniData : Form
    {
        public frmIniData()
        {
            InitializeComponent();
        }
		private int n;

		private void button1_Click(object sender, EventArgs e)
		{
			n = 3;

			/*
			Matrix A = new Matrix(n);
			A.SetA(n, 1, 10, 1, 0.43);
			dataGridView1.ColumnCount = 3*n-2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView1.Columns[i].Name = Convert.ToString(i);
			
			for (int i = 0; i < A.Rows(); i++)
			{
				string[] str = new string[3 * n - 2];
				for (int j = 0; j < A.Cols(); j++)
					str[j] = Convert.ToString(Math.Round(A.matrix[i, j],4));
				dataGridView1.Rows.Add(str);
			}
			*/

			/*
			A.Invert();
			dataGridView2.ColumnCount = 3 * n - 2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < A.Rows(); i++)
			{
				string[] str = new string[3 * n - 2];
				for (int j = 0; j < A.Cols(); j++)
					str[j] = Convert.ToString(Math.Round(A.matrix[i, j], 4));
				dataGridView2.Rows.Add(str);
			}
			*/
			
			/*
			Matrix B = new Matrix(n);
			B.SetB(n, 1, 10, 1, 0.43);
			dataGridView2.ColumnCount = 3 * n - 2;
			for (int i = 0; i < 3 * n - 2; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < B.Rows(); i++)
			{
				string[] str = new string[3 * n - 2];
				for (int j = 0; j < B.Cols(); j++)
					str[j] = Convert.ToString(Math.Round(B.matrix[i, j], 4));
				dataGridView2.Rows.Add(str);
			}
			*/
			
			
			Matrix test1 = new Matrix(n);
			test1.setTest1(n);
			Matrix test2 = new Matrix(n);
			test2.setTest2(n);
			Matrix resmult = new Matrix(n);

			resmult = test1 * test2;
			{
				dataGridView1.ColumnCount = n;
				for (int i = 0; i < n; i++)
					dataGridView1.Columns[i].Name = Convert.ToString(i);

				for (int i = 0; i < n; i++)
				{
					string[] str = new string[n];
					for (int j = 0; j < n; j++)
						str[j] = Convert.ToString(Math.Round(-test1.matrix[i, j], 4));
					dataGridView1.Rows.Add(str);
				}
			}
			
			dataGridView2.ColumnCount = n;
			for (int i = 0; i < n; i++)
				dataGridView2.Columns[i].Name = Convert.ToString(i);

			for (int i = 0; i < n; i++)
			{
				string[] str = new string[n];
				for (int j = 0; j < n; j++)
					str[j] = Convert.ToString(Math.Round(resmult.matrix[i, j], 4));
				dataGridView2.Rows.Add(str);
			}
			
		}
    }
}
