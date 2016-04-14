using DavidOchmann.Animation;
using DavidOchmann.CustomEditorTools;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

namespace DavidOchmann.Animation
{
	[ CustomEditor( typeof( TweenComponentValues ) ) ]
	public class TweenComponentValuesEditor : Editor
	{
		private TweenComponentValues tweenComponentValues;

		private List<IUpdate> updateList;
		private EditorPopup editorPopupComponents;
		private EditorButton editorPopupAddProperty;
		private MonoBehaviour[] monoBehaviours;


		/**
		 * Editor interface
		 */

		public void OnEnable()
		{
		    initVariables();
			initEditorPopupComponents();
			initAddPropertyButton();
		}

		public override void OnInspectorGUI()
		{
			editorPopupComponents.Update();
			updateUpdateList();
			editorPopupAddProperty.Update();
		}


		/**
		 * Private interface.
		 */

		/** Variables. */
		private void initVariables()
		{
			updateList = new List<IUpdate>();
			tweenComponentValues = ( TweenComponentValues )target;
		}


		/** Parse every gameObject component and add them to the EditorPopup */
		private void initEditorPopupComponents()
		{
			List<string> list = new List<string>();

		    GameObject gameObject = tweenComponentValues.gameObject;
		    monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

		    for( int i = 0; i < monoBehaviours.Length; ++i )
		    {
		        MonoBehaviour monoBehaviour = monoBehaviours[ i ];
		        string name = monoBehaviour.GetType().Name;

		        list.Add( name );
		    }

		    addNumbersToDuplicateString( list );

		    editorPopupComponents = new EditorPopup( "Components", list );
		}

		private void addNumbersToDuplicateString(List<string> list)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();

			for( int i = 0; i < list.Count; ++i )
			{
				string key = list[ i ];
			    bool containsKey = dictionary.ContainsKey( key );


			    if( !containsKey )
			    	dictionary.Add( key, 0 );
			    else
					dictionary[ key ]++;


				int amount = dictionary[ key ];

				if( amount > 0 )
				{
					string name = key + "-" + amount;
					list[ i ] = name;
				}
			}

			addNumberToFirstDuplicateString( list, dictionary );
		}

		private void addNumberToFirstDuplicateString(List<string> list, Dictionary<string, int> dictionary)
		{
			for( int i = 0; i < list.Count; ++i )
			{
				string key = list[ i ];

				if( dictionary.ContainsKey( key ) )
				{
					int amount = dictionary[ key ];
					bool hasIndex = key.Contains( "-" );

					if( amount > 0 && !hasIndex )
						list[ i ] = key + "-0";
				}
			}
		}


		/** Updated Elements in updateList. */
		private void updateUpdateList()
		{
			for( int i = 0; i < updateList.Count; ++i )
			{
			    IUpdate iUpdate = updateList[ i ];
			    iUpdate.Update();
			}
		}


		/** Button that adds a new selection for the Component float properties. */
		private void initAddPropertyButton()
		{
			editorPopupAddProperty = new EditorButton( "add property" );
			editorPopupAddProperty.OnClick += editorPopupAddPropertyOnClickHandler;
		}

		private void editorPopupAddPropertyOnClickHandler(EditorButton editorButton)
		{
			MonoBehaviour monoBehaviour = monoBehaviours[ editorPopupComponents.index ];
			parseComponentPropertiesOf( monoBehaviour );
		}

		private void parseComponentPropertiesOf(MonoBehaviour monoBehaviour)
		{
			Debug.Log( monoBehaviour );

			Type type = monoBehaviour.GetType();
			PropertyInfo[] propertyInfos = type.GetProperties();

			for( int i = 0; i < propertyInfos.Length; ++i )
			{
			    PropertyInfo propertyInfo = propertyInfos[ i ];
			    Debug.Log( propertyInfo );
			}
		}
	}
}