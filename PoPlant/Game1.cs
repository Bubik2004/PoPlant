using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework.Audio;
using System.Drawing;
using Microsoft.Xna.Framework.Media;

namespace PoPlant
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PowerPlant pPlant;
        private InputManager inputMan;
        private MFTire mf;
        private ratioDiff thisRD;
        private int gear;
        private float pitch = 1;
        private float mid = 500;
        private float low = 250;
        private float high = 1000;
        private float mod = 1;
        private double RPM;
        private double TransSpeed;
        private double DiffSpeed;
        private double throttle;
        private double clutch;
        private double cPower;
        private double flywheelWeight;
        private double revLimit;
        private double idle;
        private int millisecondsPerFrame = 1; //Update every 1 millisecond
        private double timeSinceLastUpdate = 0; //Accumulate the elapsed time
        //private AudioEngine AudioMan;

        private SpriteFont Font;
       
        SoundEffect RL;
        SoundEffect thtw;
        SoundEffect hund;
        SoundEffect ID;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
         


            thisRD = new ratioDiff();
            pPlant = new PowerPlant();
            List<float> carParameters = pPlant.ReadPara();
            flywheelWeight = (int)carParameters[0];
            revLimit = (int)carParameters[1];
            idle = (int)carParameters[2];
            inputMan = new InputManager();

            

            mf = new MFTire();


            //AudioMan = new AudioEngine();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("File");
          
            RL = Content.Load<SoundEffect>("redlinese");
            RL.CreateInstance();

            thtw = Content.Load<SoundEffect>("320se");
            thtw.CreateInstance();


            hund = Content.Load<SoundEffect>("100se");
            hund.CreateInstance();

            ID = Content.Load<SoundEffect>("idlese");
            ID.CreateInstance();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
         

            

            thisRD = pPlant.gearReader(gear = inputMan.GearSelect(_graphics));

            timeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastUpdate >= millisecondsPerFrame)
            {
                throttle = inputMan.Throttle(_graphics);
                clutch = inputMan.Clutch(_graphics);
                RPM = pPlant.RPMmod(throttle, flywheelWeight, cPower, revLimit, idle, 1);
                cPower = pPlant.Power(RPM);
                TransSpeed = pPlant.Transmission(thisRD, RPM, clutch);
                DiffSpeed = pPlant.Differential(thisRD, TransSpeed);
                mf.Initialize(DiffSpeed * 0.10472);
                timeSinceLastUpdate = 0;
            }

            float freq = (int)RPM / 60;
            float volMod;
            volMod = (float) throttle / 200;
            
            
            pitch = freq * 3;
            if (pitch < mid) 
            {
                if (pitch <= 100)
                {
                    pitch = pitch / 100;
                    ID.Play(0.7f, pitch-0.2f, 0);
                    hund.Play(0.5f+ volMod, pitch-0.5f, 0);

                }
                else if (pitch > 100 && pitch <250) 
                {

                    pitch = pitch / 250;
                    hund.Play(0.5f + volMod, pitch, 0);
                    pitch = pitch /400;
                    //thtw.Play(0.01f + volMod, pitch, 0);
                }
                else if (pitch > low && RPM < revLimit - 300)
                {
                    pitch = pitch / 950;
                    thtw.Play(0.5f + volMod, pitch, 0);
                }
                else if (RPM > revLimit - 300)
                {
                    pitch = pitch / 950;
                    thtw.Play(0.5f + volMod, pitch, 0);
                    RL.Play(0.5f + volMod, 0.25f, 0);
                    

                }
                pitch = 1;
            }
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            Vector2 PositionThrot = new Vector2(550, 50);
            Vector2 PositionClutch = new Vector2(550, 100);

            Vector2 PositionRPM = new Vector2(50, 50);
            Vector2 PositionPower = new Vector2(50, 100);
            Vector2 PositionG = new Vector2(50, 150);

            Vector2 PositionTrans = new Vector2(50, 350);
            Vector2 PositionDiff = new Vector2(50, 400);

            pPlant.DrawMessage(_spriteBatch, Font, ("Throttle:" + throttle.ToString()), PositionThrot);
            pPlant.DrawMessage(_spriteBatch, Font, ("Clutch:" + clutch.ToString()), PositionClutch);
            pPlant.DrawMessage(_spriteBatch, Font, "RPM:" + Math.Round(RPM), PositionRPM);
            pPlant.DrawMessage(_spriteBatch, Font, ("Horsepower:" + cPower.ToString()), PositionPower);
            pPlant.DrawMessage(_spriteBatch, Font, "Trans RPM:" + Math.Round(TransSpeed), PositionTrans);
            pPlant.DrawMessage(_spriteBatch, Font, "Differential RPM:" + Math.Round(DiffSpeed), PositionDiff);
            pPlant.DrawMessage(_spriteBatch, Font, "Gear: " + gear, PositionG);



            base.Draw(gameTime);
        }
    }
}
