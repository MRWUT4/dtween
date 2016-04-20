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

		private List<IUpdate> updateList;
		private EditorPopup editorPopupComponent;
		private EditorButton editorButtonAddProperty;
		private MonoBehaviour[] monoBehaviours;
		private EditorPopup tweenMethodPopup;
		private EditorPopup easeTypePopup;
		private EditorPopup easeMethodPopup;


		/**
		 * Editor interface
		 */

		public void OnEnable()
		{
		    initVariables();
		    initTweenMethod();
		    initEaseType();
		    initEaseMethod();
			initEditorPopupComponents();
			initAddPropertyButton();
		}

		public override void OnInspectorGUI()
		{
			EditorGUILayout.Space();
			editorPopupComponent.Update();

			updateSetupFields();
			updateEasingPopups();
			updatePropertyFields();
			updateEventObjectPicker();
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


		/** Setup Field functions. */
		private void updateSetupFields()
		{
			tweenComponentValues.showSetupList = EditorGUILayout.Foldout( tweenComponentValues.showSetupList, "Setup" );

			if( tweenComponentValues.showSetupList )
			{
				tweenComponentValues.id = EditorGUILayout.TextField( "ID", tweenComponentValues.id );
				tweenComponentValues.duration = EditorGUILayout.FloatField( "Duration", tweenComponentValues.duration );
				tweenComponentValues.playOnStart = EditorGUILayout.Toggle( "Play On Start", tweenComponentValues.playOnStart );


				// overwrite options
				EditorGUI.indentLevel++;

				tweenComponentValues.showOverwriteList = EditorGUILayout.Foldout( tweenComponentValues.showOverwriteList, "Overwrite" );

				if( tweenComponentValues.showOverwriteList )
				{
					tweenComponentValues.allowOverwrite = EditorGUILayout.Toggle( "Allow Overwrite", tweenComponentValues.allowOverwrite );
					tweenComponentValues.jumpToEnd = EditorGUILayout.Toggle( "Jump To End", tweenComponentValues.jumpToEnd );
				}

				EditorGUI.indentLevel--;
			}
		}


		/** Tween method functions. */
		private void initTweenMethod()
		{
			PopupVO popupVO = tweenComponentValues.tweenMethod;
			popupVO.name = "Tween Method";
			popupVO.list = new List<string>{ "To", "From" };

			tweenMethodPopup = new EditorPopup( popupVO );
		}

		private void initEaseType()
		{
			PopupVO popupVO = tweenComponentValues.easeType;
			popupVO.name = "Ease Type";
			popupVO.list = new List<string>{ "Back", "Bounce", "Circ", "Cubic", "Elastic", "Expo", "Linear", "Quad", "Quart", "Quint", "Sine" };

			easeTypePopup = new EditorPopup( popupVO );
		}

		private void initEaseMethod()
		{
			PopupVO popupVO = tweenComponentValues.easeMethod;
			popupVO.name = "Ease Method";
			popupVO.list = new List<string>{ "EaseIn", "EaseOut", "EaseInOut" };

			easeMethodPopup = new EditorPopup( popupVO );
		}

		private void updateEasingPopups()
		{
			tweenComponentValues.showEasingList = EditorGUILayout.Foldout( tweenComponentValues.showEasingList, "Easing" );

			if( tweenComponentValues.showEasingList )
			{
				tweenMethodPopup.Update();
				easeTypePopup.Update();
				easeMethodPopup.Update();
			}
		}


		private void updateEventObjectPicker()
		{
			tweenComponentValues.showEventList = EditorGUILayout.Foldout( tweenComponentValues.showEventList, "Events" );

			if( tweenComponentValues.showEventList )
			{
				serializedObject.Update();

	            EditorGUILayout.PropertyField( serializedObject.FindProperty( "onStart" ), true );
	            EditorGUILayout.PropertyField( serializedObject.FindProperty( "onUpdate" ), true );
	            EditorGUILayout.PropertyField( serializedObject.FindProperty( "onComplete" ), true );
	            
	            serializedObject.ApplyModifiedProperties();	
	        }
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

		    editorPopupComponent = new EditorPopup( popupVO );
		    editorPopupComponent.OnChange += editorPopupComponentOnChangeHandler;
		}

		private void editorPopupComponentOnChangeHandler(EditorPopup editorPopup)
		{
			tweenComponentValues.popupFloatFieldVOs.Clear();
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


		/** Button that adds a new selection for the Component float properties. */
		private void initAddPropertyButton()
		{
			editorButtonAddProperty = new EditorButton( "add property" );
			editorButtonAddProperty.OnClick += editorButtonAddPropertyOnClickHandler;
		}

		private void editorButtonAddPropertyOnClickHandler(EditorButton editorButton)
		{
			tweenComponentValues.showUpdateList = true;
			createPropertyPopup();
		}


		/** PopupCloseField list */
		private void createPropertyPopup()
		{
			MonoBehaviour monoBehaviour = monoBehaviours[ editorPopupComponent.index ];
			
			List<string> list = new List<string>();

			list = list.Concat( parseComponentPropertiesOf( monoBehaviour ) ).ToList();
			list = list.Concat( parseComponentFieldsOf( monoBehaviour ) ).ToList();

			
			PopupFloatFieldVO popupFloatFieldVO = new PopupFloatFieldVO();
			popupFloatFieldVO.list = list;

			tweenComponentValues.popupFloatFieldVOs.Add( popupFloatFieldVO );
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
					Type fieldType = fieldInfo.FieldType;
					bool isFloat = fieldType == typeof( float );
					
					if( isFloat )
						list.Add( fieldInfo.Name );
				}
			}

			return list;
		}


		/** Generate and update PopupFloatFields. */
		private void updatePropertyFields()
		{
			tweenComponentValues.showUpdateList = EditorGUILayout.Foldout( tweenComponentValues.showUpdateList, "Properties" );

			if( tweenComponentValues.showUpdateList )
			{
				List<PopupFloatFieldVO> list = tweenComponentValues.popupFloatFieldVOs;

				for( int i = 0; i < list.Count; ++i )
				{
				    PopupFloatFieldVO popupFloatFieldVO = list[ i ];

				    PopupFloatField popupFloatField = new PopupFloatField( popupFloatFieldVO );
				    popupFloatField.OnClose += popupFloatFieldOnCloseHandler;
				    popupFloatField.Update();
				}

				editorButtonAddProperty.Update();
			}
		}

		private void popupFloatFieldOnCloseHandler(PopupFloatField popupFloatField)
		{
			removePopupFloatFieldVOFromList( popupFloatField.popupFloatFieldVO );
		}

		
		private void removePopupFloatFieldVOFromList(PopupFloatFieldVO popupFloatFieldVO)
		{
			List<PopupFloatFieldVO> list = tweenComponentValues.popupFloatFieldVOs;

			for( int i = list.Count - 1; i >= 0; --i )
			{
			    PopupFloatFieldVO item = list[ i ];
			    
			    if( item == popupFloatFieldVO )
			    	list.RemoveAt( i );
			}
		}
	}
}