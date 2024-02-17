using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

/* élève de l'école de jeux vidéos, gamecodeur.fr, depuis 2017,
 * ceci est un extrait d'un code perso, dans le pur esprit viking
 * Faîtes en bon usage. Pas besoin de crédits. Un viking, çà vit de
 * conquêtes et de victoire, brouaaaaaahhhhhh!!!!
 * ---------------------------------------------------------------
 * tu peux le recopier, l'utiliser à des fins perso et commerciales,
 * ou bien le modifier à ta guise.
 * EBB Dan Marama
*/

namespace DinguEngine.Scenes
{
    public abstract class TE_ModelScene
    {
        protected ContentManager contentManager;

        protected TE_ModelScene(ref ContentManager _content)
        {
            contentManager = _content;
        }

        public abstract void LoadData();
        public abstract void Draw(ref SpriteBatch _spritebatch);
        public abstract void Update();

        protected void AddTexture2D(string _filepath, out Texture2D _target)
        {
            try
            {
                _target = contentManager.Load<Texture2D>(_filepath);
                return;
            }
            catch
            {
                _target = null;
            }
        }

        protected void AddSoundEffect(string _filepath, out SoundEffect _target)
        {

            try
            {
                _target = contentManager.Load<SoundEffect>(_filepath);
                return;
            }
            catch
            {
                _target = null;
            }
        }

        protected void AddMusic(string _filepath, out Song _target)
        {
            try
            {
                _target = contentManager.Load<Song>(_filepath);
                return;
            }
            catch
            {
                _target = null;
            }
        }
    }
}
