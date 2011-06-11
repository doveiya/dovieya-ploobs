﻿////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: StatusBar.cs                                 //
//                                                            //
//      Version: 0.7                                          //
//                                                            //
//         Date: 11/09/2010                                   //
//                                                            //
//       Author: Tom Shane                                    //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//  Copyright (c) by Tom Shane                                //
//                                                            //
////////////////////////////////////////////////////////////////

#region //// Using /////////////

////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;
////////////////////////////////////////////////////////////////////////////

#endregion

namespace TomShane.Neoforce.Controls
{


	public class StatusBar : Control
	{

		#region //// Fields ////////////

		////////////////////////////////////////////////////////////////////////////                 
		////////////////////////////////////////////////////////////////////////////

		#endregion

		#region //// Properties ////////

		////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////

		#endregion

		#region //// Construstors //////

		////////////////////////////////////////////////////////////////////////////       
		public StatusBar(Manager manager)
			: base(manager)
		{
			Left = 0;
			Top = 0;
			Width = 64;
			Height = 24;
			CanFocus = false;
		}
		////////////////////////////////////////////////////////////////////////////

		#endregion

		#region //// Methods ///////////

		////////////////////////////////////////////////////////////////////////////
		public override void Init()
		{
			base.Init();
		}
		////////////////////////////////////////////////////////////////////////////                          

		////////////////////////////////////////////////////////////////////////////                          
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["StatusBar"]);
		}
		////////////////////////////////////////////////////////////////////////////                          

		////////////////////////////////////////////////////////////////////////////
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		////////////////////////////////////////////////////////////////////////////

		#endregion

	}

}
