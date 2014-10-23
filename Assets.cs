using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Toolkit;
namespace Project
{
    using SharpDX.Toolkit.Graphics;
    public class Assets
    {
        LabGame game;

        public Assets(LabGame game)
        {
            this.game = game;
        }

        // Dictionary of currently loaded models.
        // New/existing models are loaded by calling GetModel(modelName, modelMaker).
        public Dictionary<String, MyModel> modelDict = new Dictionary<String, MyModel>();

        // Load a model from the model dictionary.
        // If the model name hasn't been loaded before then modelMaker will be called to generate the model.
        public delegate MyModel ModelMaker();
        public MyModel GetModel(String modelName, ModelMaker modelMaker)
        {
            if (!modelDict.ContainsKey(modelName))
            {
                modelDict[modelName] = modelMaker();
            }
            return modelDict[modelName];
        }

        // Create a cube with one texture for all faces.
        public MyModel CreateTexturedCube(String textureName, float size)
        {
            return CreateTexturedCube(textureName, new Vector3(size, size, size));
        }

        public MyModel CreateTexturedCube(String texturePath, Vector3 size)
        {
            float dif = 1 / 6f;

            VertexPositionNormalTexture[] shapeArray = new VertexPositionNormalTexture[]{
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0, 0, -1), new Vector2(0.0f, 1.0f)), // Front
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(0, 0, -1), new Vector2(0.0f, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0, 0, -1), new Vector2(dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0, 0, -1), new Vector2(0.0f, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0, 0, -1), new Vector2(dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(0, 0, -1), new Vector2(dif, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0, 0, 1), new Vector2(3*dif, 1.0f)), // BACK
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0, 0, 1), new Vector2(2*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(0, 0, 1), new Vector2(3*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0, 0, 1), new Vector2(3*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(0, 0, 1), new Vector2(2*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0, 0, 1), new Vector2(2*dif, 0.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(0, 1, 0), new Vector2(4*dif, 1.0f)), // Top
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(0, 1, 0), new Vector2(4*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0, 1, 0), new Vector2(5*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(0, 1, 0), new Vector2(4*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(0, 1, 0), new Vector2(5*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(0, 1, 0), new Vector2(5*dif, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0, -1, 0), new Vector2(5*dif, 0.0f)), // Bottom
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(0, -1, 0), new Vector2(6*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(0, -1, 0), new Vector2(5*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(0, -1, 0), new Vector2(5*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(0, -1, 0), new Vector2(6*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(0, -1, 0), new Vector2(6*dif, 1.0f)),

            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(-1, 0, 0), new Vector2(2*dif, 1.0f)), // Left
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, 1.0f), new Vector3(-1, 0, 0), new Vector2(dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1, 0, 0), new Vector2(dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, -1.0f, -1.0f), new Vector3(-1, 0, 0), new Vector2(2*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1, 0, 0), new Vector2(dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(-1.0f, 1.0f, -1.0f), new Vector3(-1, 0, 0), new Vector2(2*dif, 0.0f)),

            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(1, 0, 0), new Vector2(3*dif, 1.0f)), // Right
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1, 0, 0), new Vector2(4*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, 1.0f), new Vector3(1, 0, 0), new Vector2(4*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, -1.0f, -1.0f), new Vector3(1, 0, 0), new Vector2(3*dif, 1.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, -1.0f), new Vector3(1, 0, 0), new Vector2(3*dif, 0.0f)),
            new VertexPositionNormalTexture(new Vector3(1.0f, 1.0f, 1.0f), new Vector3(1, 0, 0), new Vector2(4*dif, 0.0f)),
            };

            for (int i = 0; i < shapeArray.Length; i++)
            {
                shapeArray[i].Position.X *= size.X / 2;
                shapeArray[i].Position.Y *= size.Y / 2;
                shapeArray[i].Position.Z *= size.Z / 2;
            }

            return new MyModel(game, shapeArray, texturePath);
        }

        public MyModel CreateTexturedObject(String texturePath, VertexPositionNormalTexture[] shapeArray)
        {
            return new MyModel(game, shapeArray, texturePath);
        }
    }
}
