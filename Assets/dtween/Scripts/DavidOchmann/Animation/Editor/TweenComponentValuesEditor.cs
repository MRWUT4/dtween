using DavidOchmann.Animation;
using DavidOchmann.CustomEditorTools;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace DavidOchmann.Animation
{
	[ CustomEditor( typeof( TweenComponentValues ) ) ]
	public class TweenComponentValuesEditor : Editor
	{
		private TweenComponentValues tweenComponentValues;

		// private List<IUpdate> updateList;
		private EditorPopup editorPopupComponents;
		private EditorButton editorButtonAddProperty;
		private MonoBehaviour[] monoBehaviours;
		private bool showUpdateList = true;
		

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
			EditorGUILayout.Space();

			editorPopupComponents.Update();

			EditorGUILayout.Space();
			
			updateUpdateList();
			editorButtonAddProperty.Update();
		}


		/**
		 * Private interface.
		 */

		/** Variables. */
		private void initVariables()
		{
			// updateList = new List<IUpdate>();
			tweenComponentValues = ( TweenComponentValues )target;


			/*
			PopupFloatFieldVO popupFloatFieldVO	= tweenComponentValues.popupFloatFieldVO;
			popupFloatFieldVO.list = new List<string>{ "foo", "bar" };

			PopupFloatField popupFloatField = new PopupFloatField( popupFloatFieldVO );
			updateList.Add( popupFloatField );
			//*/
		}


		/** Parse every gameObject component and add them to the EditorPopup */
		private void initEditorPopupComponents()
		{
			PopupVO popupVO = tweenComponentValues.popupComponentVO;


			List<string> list = new List<string>();

		    GameObject gameObject = tweenComponentValues.gameObject;
		    monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

		    for( int i = 0; i < monoBehaviours.Length; ++i )
		    {
		        MonoBehaviour monoBehaviour = monoBehaviours[ i ];

		        if( monoBehaviour != null )
		        {
		        	string name = monoBehaviour.GetType().Name;
		        	list.Add( name );
		        }
		    }

		    addNumbersToDuplicateString( list );


		    popupVO.name = "Component";
		    popupVO.list = list;

		    editorPopupComponents = new EditorPopup( popupVO );
		    // editorPopupComponents.OnChange += editorPopupComponentsOnChangeHandler;
		}

		// private void editorPopupComponentsOnChangeHandler(EditorPopup editorPopup)
		// {
			// updateList.Clear();
		// }


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
			Debug.Log( "updateUpdateList" );

			List<PopupFloatFieldVO> list = tweenComponentValues.popupFloatFieldVOs;

			for( int i = 0; i < list.Count; ++i )
			{
			    PopupFloatFieldVO popupFloatFieldVO = list[ i ];

			    PopupFloatField popupFloatField = new PopupFloatField( popupFloatFieldVO );
			    popupFloatField.Update();

			    // updateList.Add( popupFloatField );
			}

			// showUpdateList = EditorGUILayout.Foldout( showUpdateList, "Properties" );

			// if( showUpdateList )
			// {
			// 	for( int i = 0; i < updateList.Count; ++i )
			// 	{
			// 	    IUpdate iUpdate = updateList[ i ];
			// 	    iUpdate.Update();
			// 	}
			// }
		}


		/** Button that adds a new selection for the Component float properties. */
		private void initAddPropertyButton()
		{
			editorButtonAddProperty = new EditorButton( "add property" );
			editorButtonAddProperty.OnClick += editorButtonAddPropertyOnClickHandler;
		}

		private void editorButtonAddPropertyOnClickHandler(EditorButton editorButton)
		{
			showUpdateList = true;

			createPropertyPopup();
			// parsePopupFloatFieldVOList();
		}


		/** PopupCloseField list */
		private void createPropertyPopup()
		{
			MonoBehaviour monoBehaviour = monoBehaviours[ editorPopupComponents.index ];
			
			List<string> list = new List<string>();

			list = list.Concat( parseComponentPropertiesOf( monoBehaviour ) ).ToList();
			list = list.Concat( parseComponentFieldsOf( monoBehaviour ) ).ToList();

			

			Debug.Log( tweenComponentValues.popupFloatFieldVOs.Count );

			PopupFloatFieldVO popupFloatFieldVO = new PopupFloatFieldVO();
			popupFloatFieldVO.list = list;

			tweenComponentValues.popupFloatFieldVOs.Add( popupFloatFieldVO );
		}




		private void parsePopupFloatFieldVOList()
		{
			// List<PopupFloatFieldVO> list = tweenComponentValues.popupFloatFieldVOs;

			// for( int i = 0; i < list.Count; ++i )
			// {
			//     PopupFloatFieldVO popupFloatFieldVO = list[ i ];

			//     PopupFloatField popupFloatField = new PopupFloatField( popupFloatFieldVO );
			//     popupFloatField.Update();

			//     // updateList.Add( popupFloatField );
			// }
		}


		private void popupFloatFieldOnCloseHandler(PopupFloatField popupFloatField)
		{
			removePopupFloatFieldFromUpdateList( popupFloatField );
		}


		/** Helper functions */
		private void removePopupFloatFieldFromUpdateList(PopupFloatField popupFloatField)
		{
			// List<PopupFloatFieldVO> list = tweenComponentValues.popupFloatFieldVOs;

			// for( int i = list.Count - 1; i >= 0; --i )
			// {
			//     PopupFloatField iUpdate = list[ i ];
			 	
			//     if( popupFloatField == iUpdate )
			//     	list.RemoveAt( i );
			// }
		}

		private List<string> parseComponentPropertiesOf(MonoBehaviour monoBehaviour)
		{
			List<string> list = new List<string>();

			Type monoBehaviourType = monoBehaviour.GetType();
			PropertyInfo[] propertyInfos = monoBehaviourType.GetProperties();

			for( int i = 0; i < propertyInfos.Length; ++i )
			{
				PropertyInfo propertyInfo = propertyInfos[ i ];

				if( propertyInfo != null )
				{
					Type propertyType = propertyInfo.PropertyType;
					bool isFloat = propertyType == typeof( float );
					
					if( isFloat )
						list.Add( propertyInfo.Name );
				}
			}

			return list;
		}

		private List<string> parseComponentFieldsOf(MonoBehaviour monoBehaviour)
		{
			List<string> list = new List<string>();

			Type monoBehaviourType = monoBehaviour.GetType();
			FieldInfo[] fieldInfos = monoBehaviourType.GetFields();

			for( int i = 0; i < fieldInfos.Length; ++i )
			{
				FieldInfo fieldInfo = fieldInfos[ i ];

				if( fieldInfo != null )
				{
					Debug.Log( fieldInfo );

					Type fieldType = fieldInfo.FieldType;
					bool isFloat = fieldType == typeof( float );
					
					if( isFloat )
						list.Add( fieldInfo.Name );
				}
			}

			return list;
		}
	}
}