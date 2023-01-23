using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace PoPlant
{

    public struct ratioDiff
    {
        public double ratio;
        public double diff;
    }

    class PowerPlant
    {

        public ratioDiff thisRD = new ratioDiff();
        public string line = "";
        public string[] powerArray = new string[10];
        public string[] parArray = new string[10];
        double RPM = 950;
        int MaxGear;


        public double ReadPower(double RPM)
        {
            double currentPower = 0;
            StreamReader sRead = new StreamReader("C:/Users/10KroczakJ.SCRCAT/source/repos/pPLant/PowerPlant/Content/98SupraDyno.txt");
            double roundRPM = Math.Round(RPM / 500.0) * 500;

            //Debug.WriteLine(roundRPM);
            do
            {
                line = sRead.ReadLine();
                powerArray = line.Split(':');
                if (int.Parse(powerArray[0]) == roundRPM)
                {
                    currentPower = int.Parse(powerArray[1]);
                }

            }
            while (!sRead.EndOfStream);



            sRead.Close();
            return currentPower;
        }
        public List<float> ReadPara()
        {
            List<float> carPars = new List<float>();
            StreamReader sRead = new StreamReader("C:/Users/10KroczakJ.SCRCAT/source/repos/pPLant/PowerPlant/Content/98SupraParameters.txt");
            do
            {
                line = sRead.ReadLine();
                parArray = line.Split(':');
                carPars.Add(float.Parse(parArray[1]));
            }
            while (!sRead.EndOfStream);
            sRead.Close();


            return carPars;
        }
        public double Power(double RPM)
        {
            string[] allPower = new string[10];
            double powerAtRpm = 1;
            powerAtRpm = ReadPower(RPM);
            //Debug.WriteLine("Power at this RPM" + powerAtRpm);
            return powerAtRpm;
        }
        public double RPMmod(double throttle, double fwWeight, double powerAtRpm, double revLimit, double idle, double transmissionLoad)
        {
            double maxRpmR = (powerAtRpm / 12) * fwWeight;
            double RPMrise = ((throttle) * powerAtRpm/2) / fwWeight;
            if (RPMrise > maxRpmR)
            {
                RPMrise = maxRpmR;
            }
            int MagicNum = 3;
            if (RPM > 1500)
            {
                MagicNum = 2;

            }
            if (RPM < 4000)
            {
                MagicNum = 3;
            }
            if (RPM < 1500)
            {
                MagicNum = 5;
            }
            if (RPM < 1000)
            {
                MagicNum = 25;
            }

            double RPMfall = (RPM / fwWeight) / MagicNum;
            //Debug.WriteLine("RPMRISE{0}", RPMrise);

            //Debug.WriteLine("throttle{0}", throttle);

            //Debug.WriteLine("powerAtRPM{0}", powerAtRpm);

            //Debug.WriteLine("fweight{0}", fwWeight);

            //Debug.WriteLine("RPMlimit{0}", revLimit);

            //Debug.WriteLine("RPMfall{0}", RPMfall);


            if (RPM < revLimit && RPM + RPMrise < revLimit + 250)
            {
                RPM = RPM + RPMrise;
            }
            if (RPM > revLimit)
            {
                RPM = RPM - 10;
            }
            //Debug.WriteLine(revLimit);



            if (RPM > idle)
            {
                RPM = RPM - RPMfall;
            }



            return RPM;
        }
        public ratioDiff gearReader(int gear)
        {


            List<float> carPars = ReadPara();
            int count = 0;
            for (int i = 0; i <= 9; i++)
            {
                if (carPars[i + 3] != 0)
                {
                    count = count + 1;
                }

            }
            MaxGear = count;
            //Debug.WriteLine(MaxGear);
            if (gear > MaxGear && gear != 11)
            {
                gear = MaxGear;
            }
            if (gear > 0)
            {

                thisRD.ratio = carPars[gear + 2];

            }


            thisRD.diff = carPars[14];
            //Debug.WriteLine(gear);

            return thisRD;
        }
        public double Transmission(ratioDiff thisRD, double RPM, double clutch)
        {
            if (thisRD.ratio == 0)
            {
                RPM = 0;
            }
            else
            {
                RPM = (RPM / thisRD.ratio) * clutch / 100;
                //Debug.WriteLine("clutch: " + clutch / 100);
            }

            //Debug.WriteLine("trans: {0}",thisRD.ratio);

            return RPM;
        }
        public double Differential(ratioDiff thisRD, double RPM)
        {
            RPM = RPM / thisRD.diff;
            //Debug.WriteLine("diff: {0}",thisRD.diff);


            return RPM;
        }
        public void DrawMessage(SpriteBatch spriteBatch, SpriteFont Font, string PARA, Vector2 pos)
        {

            spriteBatch.Begin(SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            null, null, null, null);

            spriteBatch.DrawString(Font, PARA.ToString(), pos, Color.DarkBlue, 0, new Vector2(5, 5), 2.5f, SpriteEffects.None, 0.5f);

            spriteBatch.End();

        }
    }
}