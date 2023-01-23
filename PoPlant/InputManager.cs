using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace PoPlant
{
    class InputManager
    {
        int throttle = 0;
        int steering = 0;
        int clutch = 0;
        int count = 0;
        int Gear;
        float controller;

        PowerPlant pPlant;

        public int Throttle(GraphicsDeviceManager inGraphics)
        {
            KeyboardState state;

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W))
            {
                if (throttle < 100)
                {
                    throttle = throttle + 40;
                }
                if (throttle >= 100)
                {
                    throttle = 100;
                }



            }
            if (state.IsKeyDown(Keys.W) == false)
            {
                if (throttle > 0)
                {
                    throttle = throttle - 5;
                }
                if (throttle < 4)
                {
                    throttle = 0;
                }


            }

            //controller = (GamePad.GetState(PlayerIndex.One).Triggers.Right);
            //controller = controller * 100;
            //throttle = (int)controller;
            if (state.IsKeyDown(Keys.S))
            {

            }

            //Debug.WriteLine("throttle{0}", throttle);

            return throttle;
        }
        public int Clutch(GraphicsDeviceManager inGraphics)
        {
            KeyboardState state;

            state = Keyboard.GetState();

            if (clutch < 100)
            {

                clutch = clutch + 2;

            }
            if (clutch > 100)
            {
                clutch = 100;
            }

            else if (state.IsKeyDown(Keys.LeftControl))
            {
                if (clutch > 0)
                {
                    clutch = clutch - 4;



                }
                if (clutch <= 0)
                {
                    clutch = 0;
                }



            }
            //Debug.WriteLine("clutch:" + clutch);
            return clutch;
        }
        public int GearSelect(GraphicsDeviceManager inGraphics)
        {
            pPlant = new PowerPlant();
            KeyboardState state;

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.NumPad1))
            {
                Gear = 1;
            }
            if (state.IsKeyDown(Keys.NumPad2))
            {
                Gear = 2;
            }
            if (state.IsKeyDown(Keys.NumPad3))
            {
                Gear = 3;
            }
            if (state.IsKeyDown(Keys.NumPad4))
            {
                Gear = 4;
            }
            if (state.IsKeyDown(Keys.NumPad5))
            {
                Gear = 5;
            }
            if (state.IsKeyDown(Keys.NumPad6))
            {
                Gear = 6;
            }
            if (state.IsKeyDown(Keys.NumPad7))
            {
                Gear = 7;
            }
            if (state.IsKeyDown(Keys.NumPad8))
            {
                Gear = 8;
            }
            if (state.IsKeyDown(Keys.NumPad9))
            {
                Gear = 9;

            }
            if (state.IsKeyDown(Keys.NumPad0))
            {
                Gear = 10;
            }
            if (state.IsKeyDown(Keys.OemMinus))
            {
                Gear = 11;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                count = count + 1;
                if (count == 10)
                {

                    Gear = Gear + 1;

                    count = 0;
                }
            }

            if (state.IsKeyDown(Keys.Left))
            {
                count = count + 1;
                if (count == 10)
                {

                    Gear = Gear - 1;

                    count = 0;
                }
            }
            return Gear;
        }
        public int Steering(GraphicsDeviceManager inGraphics)
        {

            KeyboardState state;

            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.A))
            {


            }

            if (state.IsKeyDown(Keys.D))
            {

            }
            return steering;
        }
    }

}

