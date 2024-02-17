using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace DinguEngine.Scenes
{
    public struct cutsceneData
    {
        public string text;
        public Texture2D texture;
    }
    public class CutSceneGenerator
    {
        List<Texture2D> textures;
        List<string> text;
        List<float> timer;

        int state = 0;
        float alpha = 0.0f;
        float playerticks = 0.0f;
        int currentTextureIteration, currentTextIteration, currentTimerIteration=0;

        public float GetAlpha()
        { return alpha; }

        public (bool isOver, float alpha) AlphaIn()
        {
            if (alpha >= 1.0f)
            {
                alpha = 1.0f;
                state = 1;
                return (true,alpha);
            }

            alpha += 0.015f;
            return (false,alpha);
        }

        public (bool isOver, float alpha) AlphaOut()
        {
            if (alpha <=  0.0f)
            {
                alpha =  0.0f;
                return (true,alpha);
            }

            alpha -= 0.015f;
            return (false,alpha);
        }
        int mouseticks = 0, kbticks = 0;


      public (bool isOver,int iteration) Update()
      {
            if (timer == null) return (false, 0);

            if (state == 0)
            {
                bool temp = AlphaIn().isOver;
                if (temp)
                {
                    state = 1;
                };
            }

            if (state == 0) return (false, currentTimerIteration);
            playerticks += 0.010f;

            MouseState mousestate = Mouse.GetState();
            if(mousestate.LeftButton == ButtonState.Pressed && mouseticks==0)
            {
                mouseticks = 1;
                playerticks = timer[currentTextIteration];

            }
            if(mousestate.LeftButton == ButtonState.Released)
            {
                mouseticks = 0;

            }

           if(playerticks>= timer[currentTimerIteration] )
            {
                playerticks = timer[currentTimerIteration];

                bool alphaout = AlphaOut().isOver;
                if(alphaout) 
                { 
                    if(currentTimerIteration==timer.Count-1)
                        return (true, currentTimerIteration);

                    currentTimerIteration++;
                    playerticks = 0;
                    state = 0;
                }
            }



            return (false, currentTimerIteration);
        }

        public void SetTextures(ref List<Texture2D> _textures)
        {
            this.textures = new List<Texture2D>();
            this.textures = _textures;
        }

        public void SetText(ref List<string> _text)
        {
            text = new List<string>();
            text = _text;

        }

        public void SetTimer(ref List<float> _timer)
        {
            timer = new List<float>();
            timer = _timer;

        }

    }
}
