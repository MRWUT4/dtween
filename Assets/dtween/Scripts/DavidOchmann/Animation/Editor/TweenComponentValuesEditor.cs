using UnityEngine;
using System.Collections;
using UnityEditor;
using DavidOchmann.Animation;


namespace DavidOchmann.Animation
{
	public enum Properties{ a = 0, b = 1, c = 2 }

	[ CustomEditor( typeof( TweenComponentValues ) ) ]
	public class TweenComponentValuesEditor : Editor 
	{
		private TweenComponentValues tweenComponentValues;
		public Properties properties;
		private int selected;

		public override void OnInspectorGUI()
		{
		    tweenComponentValues = ( TweenComponentValues )target;
		    
		    initPropertiesEnumField();

		    // myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
		    // EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
		}

		private void initPropertiesEnumField()
		{
			string[] list = { "a", "b" };

			selected = EditorGUILayout.Popup( "properties", selected, list );
		}
	}
}