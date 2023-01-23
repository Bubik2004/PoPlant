using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

namespace PoPlant
{
    class MFTire
    {

        public void Initialize(double WAV)
        {


            int VL = 3831;
            double WR = 12.65;
            double K = tireTreadLongitudinalVelocity(WR, WAV, 120);
            //Debug.WriteLine(WAV);
            double Fx = forceAtContactPoint(VL, 10, 1.9, 1, 0.97, K);
           // Debug.WriteLine("Force at contact point" + Fx);

        }
        public double tireTreadLongitudinalVelocity(double WR, double WAV, double WHLV)
        {
            double TTLV = WR * WAV;
            double WSV = wheelSlipVelocity(WHLV, WR, WAV, TTLV);
            double K = wheelSlip(WSV, WHLV);
            return K;
        }

        public double wheelSlipVelocity(double WHLV, double WR, double WAV, double TTLV)
        {
            //double TTLV = tireTreadLongitudinalVelocity(WR, 866);
            double WSV = TTLV - WHLV;
            return WSV;
        }

        public double wheelSlip(double WSV, double WHLV)
        {
            // double WSV = wheelSlipVelocity(WHLV, WR, WAV);
            double WS = WSV / WHLV;

            return WS;
        }
        public float forceAtContactPoint(double VL, double B, double C, double D, double E, double K)
        {
            double Fx = VL * D * Math.Sin(C * Math.Atan(B * K - E * (B * K - Math.Atan(B * K))));
            float Fx_asFloat = (float)Fx;



            return Fx_asFloat;
        }

    }

}


