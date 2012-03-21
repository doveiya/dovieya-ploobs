using System;
using Microsoft.Xna.Framework.Graphics;
using PloobsEngine.Cameras;
using Microsoft.Xna.Framework;

namespace Osiris.Graphics.Terrain
{
	public class TerrainModel
	{
		private readonly int _numPatchesX;
		private readonly int _numPatchesY;
		private readonly Patch[,] _patches;
		private readonly float _tau;

		public HeightMap HeightMap { get; private set; }

		public Effect Effect { get; set; }

		internal TerrainModel(int numPatchesX, int numPatchesY, Patch[,] patches, HeightMap heightMap, float tau, Effect effect)
		{
			HeightMap = heightMap;
			_numPatchesX = numPatchesX;
			_numPatchesY = numPatchesY;
			_patches = patches;
			_tau = tau;
			Effect = effect;
		}

        public void Initialize(ICamera camera, GraphicsDevice graphicsDevice)
		{
			for (int y = 0; y < _numPatchesY; y++)
				for (int x = 0; x < _numPatchesX; x++)
					_patches[x, y].Initialize(_tau, camera, graphicsDevice);
		}

		public void Update(ICamera camera)
		{
			// TODO: Maybe don't need to update levels for invisible patches?
			// set preferred tesselation levels for each patch
			for (int y = 0; y < _numPatchesY; y++)
                for (int x = 0; x < _numPatchesX; x++)
                {
                    if (camera.BoundingFrustum.Contains(_patches[x, y].BoundingBox) != ContainmentType.Disjoint)
                    {
                        _patches[x, y].Visible = true;
                        _patches[x, y].UpdateLevelOfDetail(camera);
                    }
                    else
                    {
                        _patches[x, y].Visible = false;
                    }
                }

			// now, make sure that each patch is no more than 1 level different from its neighbours
			// we loop through all patches, and if any are changed, set a flag. continue the outer
			// loop until the "changed" flag is false for all patches
			bool bChanged;
			do
			{
				bChanged = false;

				for (int y = 0; y < _numPatchesY; y++)
				{
					for (int x = 0; x < _numPatchesX; x++)
					{
						// get the minimum level for neighbouring patches
						Patch pPatch = _patches[x, y];
						int nLevel = pPatch.ActiveLevel;
						int nLeft = pPatch.LeftActiveLevel;
						int nRight = pPatch.RightActiveLevel;
						int nTop = pPatch.TopActiveLevel;
						int nBottom = pPatch.BottomActiveLevel;

						int nMinimumNeighbouringLevel = Math.Min(Math.Min(nLeft, nRight), Math.Min(nTop, nBottom));

						if (nLevel > nMinimumNeighbouringLevel + 1)
						{
							pPatch.ActiveLevel = nMinimumNeighbouringLevel + 1;
							bChanged = true;
						}
					}
				}
			}
			while (bChanged);

			// finally, update geometry to match LOD
			for (int y = 0; y < _numPatchesY; y++)
				for (int x = 0; x < _numPatchesX; x++)
					_patches[x, y].UpdateTessellation();

			/*int numVisible = 0;
			for (int y = 0; y < _numPatchesY; y++)
				for (int x = 0; x < _numPatchesX; x++)
					if (_patches[x, y].Visible)
						numVisible++;*/
		}

        public void CleanUp()
        {
            for (int y = 0; y < _numPatchesY; y++)
                for (int x = 0; x < _numPatchesX; x++)
                    _patches[x, y].CleanUp();
        }

        public float[,] GetHeights()
        {
            float[,] map = new float[HeightMap.Width * HeightMap.HorizontalScale, HeightMap.Width * HeightMap.HorizontalScale];
            for (int x = 0; x < HeightMap.Width * HeightMap.HorizontalScale; x++)
            {
                for (int y = 0; y < HeightMap.Width * HeightMap.HorizontalScale; y++)
                {
                    map[x, y] = HeightMap[x,y];
                }
            }
            return map;
        }

        public void Draw()
		{
			// this needs to change to render closest patches first, so that we
			// make proper use of the z-index
			for (int y = 0; y < _numPatchesY; y++)
				for (int x = 0; x < _numPatchesX; x++)
					
					if (_patches[x, y].Visible)
					{
						foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
						{
							pass.Apply();
							_patches[x, y].Draw();
						}
                    }
					
		}
	}
}