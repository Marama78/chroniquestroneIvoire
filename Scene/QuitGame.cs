using DinguEngine.Camera;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTI_RPG;

namespace CTI_RPG.Scene
{
    public class QuitGame : ModelScene
    {
        float chronoEND = 0.0f;
        public QuitGame(MainClass _mainclass) : base(_mainclass)
        {
        }

        public override void Load(ref ContentManager _content)
        {
            maincamera = new TE_Camera( );
            uicamera = new TE_Camera( );
            friendcamera = new TE_Camera( );
            ennemycamera = new TE_Camera( );
            base.Load(ref _content);
        }

        public override void Update()
        {

            chronoEND += 0.25f;

            if(chronoEND >=5.0f)
            {
                main.Exit();
            }
            base.Update();
        }
    }
}
