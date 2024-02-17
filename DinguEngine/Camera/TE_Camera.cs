using Microsoft.Xna.Framework;
using DinguEngine.Inputs;
using DinguEngine.Shared;
using Microsoft.Xna.Framework.Graphics;

/* CECI n'est pas mon code
*/

namespace DinguEngine.Camera
{
    public class TE_Camera
    {
        // Construct a new Camera class with standard zoom (no scaling)
        public TE_Camera()
        {
            Zoom = (int)(TE_Manager.viewportWidth / 240);
            default_zoom = (int)(TE_Manager.viewportWidth / 240);
        }

        public void SetZoom(float zoom)
        {
            Zoom = zoom;
        }

        public float default_zoom;
        public float GetZoomRange()
        { return Zoom; }    

        // Centered Position of the Camera in pixels.
        public Vector2 Position { get; private set; }
        // Current Zoom level with 1.0f being standard
        public float Zoom { get; private set; }
        // Current Rotation amount with 0.0f being standard orientation
        public float Rotation { get; private set; }

        // Height and width of the viewport window which we need to adjust
        // any time the player resizes the game window.
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }


        private int2 renderOnScreen;
        // Center of the Viewport which does not account for scale
        public Vector2 ViewportCenter
        {
            get
            {
                return new Vector2(TE_Manager.screenW * 0.5f, TE_Manager.screenH * 0.5f);
            }
        }


        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public Matrix TranslationMatrix
        {
            get
            {
            

                return Matrix.CreateTranslation(-(int)Position.X,
                   -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
              Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        // Call this method with negative values to zoom out
        // or positive values to zoom in. It looks at the current zoom
        // and adjusts it by the specified amount. If we were at a 1.0f
        // zoom level and specified -0.5f amount it would leave us with
        // 1.0f - 0.5f = 0.5f so everything would be drawn at half size.
        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }

        public void ZoomOUT(float amount)
        {
            Zoom *= amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
            if (Zoom >= default_zoom)
            {
                Zoom = default_zoom;
            }
        }



        // Move the camera in an X and Y amount based on the cameraMovement param.
        // if clampToMap is true the camera will try not to pan outside of the
        // bounds of the map.
        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Vector2 newPosition =
            new Vector2(
         MathHelper.Lerp(Position.X, (Position.X+ cameraMovement.X), 0.08f),
         MathHelper.Lerp(Position.Y, (Position.Y+ cameraMovement.Y), 0.08f));// Position + cameraMovement; 

            if (clampToMap)
            {
                Position = MapClampedPosition(newPosition);
            }
            else
            {
                Position = newPosition;
            }
        }

        public Rectangle ViewportWorldBoundry()
        {
            Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            Vector2 viewPortBottomCorner =
               ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewPortCorner.X,
               (int)viewPortCorner.Y,
               (int)(viewPortBottomCorner.X - viewPortCorner.X),
               (int)(viewPortBottomCorner.Y - viewPortCorner.Y));
        }

        // Center the camera on specific pixel coordinates
        public void CenterOn(Vector2 position)
        {
            Position = position;
        }

        // Center the camera on a specific cell in the map
       /* public void CenterOn(Vector2 cell)
        {
            Position = CenteredPosition(cell, true);
        }*/

        private Vector2 CenteredPosition(Vector2 cell, bool clampToMap = false)
        {
            var cameraPosition = new Vector2(cell.X * 48,
               cell.Y * 48);
            var cameraCenteredOnTilePosition =
               new Vector2(cameraPosition.X + 48 / 2,
                   cameraPosition.Y + 48 / 2);
            if (clampToMap)
            {
                return MapClampedPosition(cameraCenteredOnTilePosition);
            }

            return cameraCenteredOnTilePosition;
        }

        // Clamp the camera so it never leaves the visible area of the map.
        public Vector2 MapClampedPosition(Vector2 position)
        {
            var cameraMax = new Vector2(TE_Manager.gridW * TE_Manager.tileW -
                (TE_Manager.screenW / Zoom/2) - TE_Manager.GetOffsetCameraMAX_X(),// / 2),
                TE_Manager.gridH * TE_Manager.tileH -
                (TE_Manager.screenH / Zoom/2));// 2));

            return Vector2.Clamp(position,
               new Vector2(
                   TE_Manager.screenW / Zoom  /2
               , TE_Manager.screenH / Zoom / 2),
               cameraMax);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition,
                Matrix.Invert(TranslationMatrix));
        }

        // Move the camera's position based on input
        public void HandleInput()
        {
            Vector2 cameraMovement = Vector2.Zero;

           
         /*   if (TE_Keyboard.IsZoomIn())
            {
                if (Zoom < 2.5f)
                {
                    AdjustZoom(0.25f);

                }
            }
            else if (TE_Keyboard.IsZoomOut())
            {
                if(Zoom>1)
                {
                    AdjustZoom(-0.25f);

                }
            }*/

            // When using a controller, to match the thumbstick behavior,
            // we need to normalize non-zero vectors in case the user
            // is pressing a diagonal direction.
           /* if (cameraMovement != Vector2.Zero)
            {
                cameraMovement.Normalize();
            }

            // scale our movement to move 25 pixels per second
            cameraMovement *= 25f;

            MoveCamera(cameraMovement, true);*/
        }
    }

}

