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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

		private void button1_Click(object sender, EventArgs e)
		{
			Matrix A = new Matrix(9);
			A.SetA(9, 1, 10, 1, 0.000001, 0.43);
			dataGridView1.ColumnCount = 9;
			for (int i = 0; i < 9; i++)
				dataGridView1.Columns[i].Name = Convert.ToString(i);
			
			for (int i = 0; i < A.Rows(); i++)
			{
				string[] str = new string[9];
				for (int j = 0; j < A.Cols(); j++)
					str[j] = Convert.ToString(A.matrix[i, j]);
				dataGridView1.Rows.Add(str);
			}
					
		}
    }
}
