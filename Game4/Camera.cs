using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    public class Camera
    {
        private static Camera instance;
        Vector2 position, focalPoint;

        public Vector2 FocalPoint
        {
            get { return focalPoint; }
            set { focalPoint = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Matrix viewMatrix;

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }

        public static Camera Instance
        {
            get {
                if (instance == null)
                    instance = new Camera();
                return Camera.instance; }
            set { Camera.instance = value; }
        }

        public void SetFocalPoint(Vector2 focalPoint)
        {
            this.focalPoint = focalPoint;
            position = new Vector2(focalPoint.X - ScreenManager.Instance.Dimensions.X / 2,
                focalPoint.Y - ScreenManager.Instance.Dimensions.Y / 2);

            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}
