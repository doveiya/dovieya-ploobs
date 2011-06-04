using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PloobsEngine.SceneControl.GUI;
using Microsoft.Xna.Framework;
using PloobsEngine.Engine;
using PloobsEngine.Engine.Logger;

namespace PloobsEngine.SceneControl._2DScene
{
    public abstract class I2DScene : IScreen
    {
       #if !WINDOWS_PHONE
        public I2DScene(IGui gui = null) : base(gui) { }
#else
        public I2DScene() : base() { }
#endif

        #region properties

        private RenderTechnich2D _renderTecnic = null;        
        private bool _isFirstTimeTechnic = true;       


        /// <summary>
        /// Gets the render technics.
        /// </summary>
        public RenderTechnich2D RenderTechnic
        {
            get
            {
                return _renderTecnic;
            }            
        }

        private I2DWorld _world = null;

        /// <summary>
        /// Gets or sets the world instance.
        /// </summary>
        /// <value>
        /// The world.
        /// </value>
        public I2DWorld World
        {            
            get
            {
                return this._world;
            }
        }

        /// <summary>
        /// Function called After all the stuffs LoadContent is called
        /// </summary>
        protected override void AfterLoadContent(IContentManager manager, Engine.GraphicInfo ginfo, Engine.GraphicFactory factory)
        {
            _renderTecnic.iAfterLoadContent(manager, ginfo, factory);         
        }
       

        #endregion

        /// <summary>
        /// Update the Screen
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {            
            base.Update(gameTime);
            World.iUpdateWorld(gameTime);
        }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="render"></param>
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime,RenderHelper render)
        {
            if (_isFirstTimeTechnic == true)
            {                                
                _renderTecnic.iBeforeFirstExecution(render, this.World);                
                _isFirstTimeTechnic = false;
            }
            _renderTecnic.iExecuteTechnic(gameTime, render, this.World);           
            
        }

        /// <summary>
        /// Load content for the screen.        
        /// </summary>
        /// <param name="GraphicInfo"></param>
        /// <param name="factory"></param>
        /// <param name="contentManager"></param>
        protected override void LoadContent(GraphicInfo GraphicInfo, GraphicFactory factory ,IContentManager contentManager)
        {
            base.LoadContent(GraphicInfo, factory,contentManager);
        }

        /// <summary>
        /// Init Screen
        /// </summary>
        /// <param name="GraphicInfo">The graphic info.</param>
        /// <param name="engine"></param>
        protected override void InitScreen(GraphicInfo GraphicInfo, EngineStuff engine)
        {
            SetWorldAndRenderTechnich(out _renderTecnic, out _world);
            if (_renderTecnic == null )
            {
                    ActiveLogger.LogMessage("IScene must have a renderTechnic", LogLevel.FatalError);
                #if !WINDOWS_PHONE
                    System.Diagnostics.Debug.Fail("IScene must have a renderTechnic");
                #endif
                    throw new Exception("IScene must have a renderTechnic");
            }
            if (_world == null)
            {
                    ActiveLogger.LogMessage("World cannot be null", LogLevel.FatalError);
                #if !WINDOWS_PHONE
                    System.Diagnostics.Debug.Fail("World cannot be null");
                #endif
                    throw new Exception("World cannot be null");
            }
            
            this._world.GraphicsFactory = GraphicFactory;
            this._world.GraphicsInfo = GraphicInfo;
            this._world.ContentManager = screenManager.contentManager;
            this._world.iInitWorld();
            base.InitScreen(GraphicInfo, engine);
        }

        /// <summary>
        /// Sets the world and render technich.
        /// </summary>
        /// <param name="renderTech">The render tech.</param>
        /// <param name="world">The world.</param>
        protected abstract void SetWorldAndRenderTechnich(out RenderTechnich2D renderTech, out I2DWorld world);
        
    }

}