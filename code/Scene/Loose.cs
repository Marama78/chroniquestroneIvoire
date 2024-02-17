using DinguEngine.Camera;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheShelter;

namespace _TheShelter.Scene
{
    public class Loose : ModelScene
    {
        public Loose(MainClass _mainclass) : base(_mainclass)
        {
        }

        public override void Draw(ref SpriteBatch _sp)
        {
            base.Draw(ref _sp);
        }

        public override void Draw_UI(ref SpriteBatch _spUI)
        {
            base.Draw_UI(ref _spUI);
        }

        public override void Load(ref ContentManager _content)
        {
            maincamera = new TE_Camera();
            uicamera = new TE_Camera();
            friendcamera = new TE_Camera();
            ennemycamera = new TE_Camera();
            base.Load(ref _content);
        }
        float chronoEND = 0.0f;
        public override void Update()
        {

            chronoEND += 0.25f;

            if(Mouse.GetState().LeftButton==ButtonState.Pressed)
            {
                main.ChangeScene(scene.combatmode);
            }

            if (chronoEND >= 25.0f)
            {
                main.ChangeScene(scene.start);
            }

            base.Update();
        }
    }
}
