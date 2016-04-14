using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace DavidOchmann.CustomEditorTools
{
	/**
	 * Updated Interface
	 */

	public interface IUpdate
	{
		void Update();
	}


	/**
	 * EditorButton
	 */

	public class EditorButton : IUpdate
	{
		private string name;


		public EditorButton(string name)
		{
			this.name = name;
		}


		public event OnClickEventHandler OnClick;
		public delegate void OnClickEventHandler(EditorButton editorButton);
		
		protected virtual void InvokeClick() 
		{
			if( OnClick != null ) OnClick( this );
		}


		public void Update()
		{
			bool clicked = GUILayout.Button( name );

			if( clicked )
				InvokeClick();
		}
	}


	/**
	 * EditorPopup
	 */

	public class EditorPopup : IUpdate
	{
		public string name;
		public int index;
		public List<string> list;


		public EditorPopup(string name, List<string> list)
		{
			this.name = name;
			this.list = list;
		}


		/**
		 * Getter / Setter
		 */
		
		public string value
		{
			get 
		    { 
		        return list[ index ]; 
		    }
		}


		/**
		 * Public
		 */

		public void Update()
		{
			index = EditorGUILayout.Popup( name, index, list.ToArray() );
		}
	}
}