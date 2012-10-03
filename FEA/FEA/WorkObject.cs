using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEA;
using ei;

namespace FEA
{
    class WorkObject
    {
        public struct DISP
        {
            public double k;
            public Complex y;
        };

		public struct CRIT
		{
			public double R;
			public double k;
			public double y;
		};

        public DISP[] dispchar;

        private Complex[] eigen(int fen, double kc, double ec, int mc, double r)
        {
            Matrix A = new Matrix(fen);
            A.SetA(fen, kc, ec, mc, r);
            Matrix B = new Matrix(fen);
            B.SetB(fen, kc, ec, mc, r);
            return A.eige(B);
        }

        //TODO: дисперсионные характеристики
        /// <summary>
        /// Dispersion characteristics
        /// </summary>
        /// <param name="fe">Number of finite elements</param>
        /// <param name="Nsteps">Number of steps</param>
        /// <param name="step">Step</param>
        /// <param name="R">Radius of layer</param>
        public DISP[] dispersion(int fe, int Nsteps, double step, int mode, double ec, double R)
        {
            dispchar = new DISP[Nsteps+1];
            Complex[] E1 = new Complex[21];
            Complex zeroValue = new Complex();
            int minN = 0;

            E1 = eigen(fe, 0, ec, mode, R);
            zeroValue = E1[mode-1];
            dispchar[0].k = 0;
            dispchar[0].y = zeroValue;
			/*
			Complex[,] E2 = new Complex[Nsteps,21];
			int NP = Nsteps / 5;
			for (int ii = 1; ii < 6; ii++)
				Parallel.For(NP*(ii-1), NP*ii, (i0, loopState) =>
					{
						for (int i = 0; i < 21; i++)
							E2[i0, i] = eigen(fe, step * i0, ec, mode, R)[i];
					}
					);
			*/
            for (int i1 = 1; i1 < Nsteps + 1; i1++)
            {
                Complex[] E2 = new Complex[21];
                Complex[] buf = new Complex[21];
                Complex[] tempbuf = new Complex[21];
				E2 = eigen(fe, step * i1, ec, mode, R);

                for (int i2 = 0; i2 < 21; i2++)
                    buf[i2] = new Complex(Math.Abs(zeroValue.Re() - E2[i2].Re()), Math.Abs(zeroValue.Im() - E2[i2].Im()));

                for (int i = 0; i < 21; i++) tempbuf[i] = buf[i];
                zeroValue.quickSort(ref tempbuf,0,20);
                Complex minVal = tempbuf[0];
                

                for (int i3 = 0; i3 < 21; i3++)
                {
                    if (buf[i3] == minVal)
                        minN = i3;
                }
                dispchar[i1].k = step * i1;
                dispchar[i1].y = E2[minN];
                zeroValue = E2[minN];
            }
            return dispchar;
        }

		
    }
}
