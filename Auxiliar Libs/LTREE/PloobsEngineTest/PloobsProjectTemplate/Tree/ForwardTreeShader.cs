﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.Modelo;
using LTreesLibrary.Trees;

namespace PloobsEngine.Material
{
    /// <summary>
    /// Tree Shader, used by the Tree Material, interacts with the LTree
    /// </summary>
    public class ForwardTreeShader : IShader
    {
        /// <summary>
        /// Gets the type of the material.
        /// </summary>
        /// <value>
        /// The type of the material.
        /// </value>
        public override MaterialType MaterialType
        {
            get { return MaterialType.FORWARD; }
        }

        public override void  Draw(Microsoft.Xna.Framework.GameTime gt, SceneControl.IObject obj, SceneControl.RenderHelper render, Cameras.ICamera cam, IList<Light.ILight> lights)        
 	    {
            SimpleTree tree = (obj.Modelo as TreeModel).Tree;            
            tree.DrawTrunk(obj.WorldMatrix, cam.View, cam.Projection,true);
            tree.DrawLeaves(obj.WorldMatrix, cam.View, cam.Projection, true);            
            render.ResyncStates();
        }

        public override void DepthExtractor(Microsoft.Xna.Framework.GameTime gt, SceneControl.IObject obj, Microsoft.Xna.Framework.Matrix View, Microsoft.Xna.Framework.Matrix projection, SceneControl.RenderHelper render)
        {
            SimpleTree tree = (obj.Modelo as TreeModel).Tree;
            tree.DrawTrunk(obj.WorldMatrix, View, projection, false, true);
            render.ResyncStates();
            tree.DrawLeaves(obj.WorldMatrix, View, projection, false, true);
            render.ResyncStates();
        }         
    }
}
