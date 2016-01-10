using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DavidOchmann.Utils.DTween
{
	public class Tween
	{
		public delegate float EaseDelegate(float t, float b, float c, float d);

		public static string KEY_DELAY = "delay";
		public static string KEY_EASE = "ease";
		public static string KEY_GAMEOBJECT = "gameObject";
		public static string KEY_FIXEDEND = "fixedEnd";
		public static string DEFAULT_EASE = "Cubic.EaseOut";
		public static string[] IGNORED_PROPERTIES = { KEY_EASE, KEY_DELAY, KEY_GAMEOBJECT };
		public static int FPS = (int)Math.Floor( 1 / Time.fixedDeltaTime );

		public object Target;
		public float Duration;
		public Dictionary<string, object> Setup;
		public bool hasCompleted;
		public bool hasStarted;

		private int frame = 0;
		private float delay;
		private EaseDelegate ease;
		private Dictionary<string, float> beginValues;


		public Tween(object target, float duration, Dictionary<string, object> setup, EaseDelegate ease)
		{
			this.Target = target;
			this.Duration = duration;
			this.Setup = setup;

			this.frame = 0;
			this.delay = setup.ContainsKey( KEY_DELAY ) ? Convert.ToSingle( setup[ KEY_DELAY ] ) : 0;
			this.ease = ease != null ? ease : Quad.EaseOut;
			
			initBeginValues();
		}


		/**
		 * Event interface.
		 */

		public delegate void OnStartEventHandler( Tween tween );
		public event OnStartEventHandler OnStart;
		
		public virtual void InvokeStart() 
		{
			if( OnStart != null ) OnStart( this );
		}

		public delegate void OnUpdateEventHandler( Tween tween );
		public event OnUpdateEventHandler OnUpdate;
		
		public virtual void InvokeUpdate() 
		{
			if( OnUpdate != null ) OnUpdate( this );
		}

		public delegate void OnCompleteEventHandler( Tween tween );
		public event OnCompleteEventHandler OnComplete;
		
		public virtual void InvokeComplete() 
		{
			if( OnComplete != null ) OnComplete( this );
		}


		/**
		 * Static interface.
		 */
	 	
		public static object GetObjectValue(object target, string property)
		{
			object value = null;
			Type type = target.GetType();

			PropertyInfo propertyInfo = type.GetProperty( property );

			if( propertyInfo != null )
			{
				value = propertyInfo.GetValue( target, null );
			}
			else
			{
				FieldInfo fieldInfo = type.GetField( property );
				value = fieldInfo.GetValue( target );
			}

			return value;
		}

	 	public static object SetObjectValue(object target, string property, object value)
	 	{
	 		Type type = target.GetType();
			PropertyInfo propertyInfo = type.GetProperty( property );

			if( propertyInfo != null ) 
			{
				propertyInfo.SetValue( target, value, null );
			}
			else
			{
				FieldInfo fieldInfo = type.GetField( property );
				fieldInfo.SetValue( target, value );
			}

			return target;
	 	}

		public static bool GetIsIgnoredProperty(string property)
		{
			for( int i = 0; i < IGNORED_PROPERTIES.Length; ++i )
			{
				if( property == IGNORED_PROPERTIES[ i ] )
					return true;
			}

			return false;
		}

		public static MethodInfo GetMethodInfo(String value)
		{
			String[] split = value.Split( new Char[] { '.' } );

			String typeName = split[ 0 ];
			String methodName = split[ 1 ];

			Type type = Type.GetType( typeName );

			MethodInfo methodInfo = type.GetMethod( methodName, BindingFlags.Public | BindingFlags.Static );

			return methodInfo;
		}


		/**
		 * Getter / Setter.
		 */

		public bool GetIsFirstFrame()
		{
			return frame == 0;
		}

		public bool GetComplete()
		{
			return frame >= GetTotalFrames();
		}

		public bool GetStart()
		{
			return frame >= GetDelayFrames();
		}

		public int GetTotalFrames()
		{
			return GetDurationFrames() + GetDelayFrames();
		}

		public int GetDurationFrames()
		{
			return GetSecondsToFrames( Duration );
		}

		public int GetDelayFrames()
		{
			return GetSecondsToFrames( delay );
		}

		public float GetTimescale()
		{
			return Duration / GetDurationFrames();
		}

		public int GetSecondsToFrames(float seconds)
		{
			return (int)Math.Ceiling( FPS * seconds );
		}

		public Dictionary<string, float> GetDictionaryAtFrame(int frame)
		{
			Dictionary<string, float> dictionary = null;

			if( GetStart() && beginValues != null )
			{
				dictionary = new Dictionary<string, float>();

				for( int i = 0; i < Setup.Count; ++i )
				{
					KeyValuePair<string, object> pair = Setup.ElementAt( i );

					string property = pair.Key;

					if( !GetIsIgnoredProperty( property ) )
					{
						float value = Convert.ToSingle( pair.Value );
						int durationFrame = frame - GetDelayFrames();

						dictionary[ property ] = float.NaN;

						float t = durationFrame * GetTimescale();
						float b = beginValues[ property ];
						float c = value - b;

						if( durationFrame < GetDurationFrames() - 1 )
							dictionary[ property ] = this.ease( t, b, c, Duration );
						else
							dictionary[ property ] = value;
					}
				}
			}

			return dictionary;
		}


		/**
		 * Public interface.
		 */

		public void Reset()
		{
			frame = 0;
			hasCompleted = false;
		}

		public void Kill()
		{
			frame = this.GetTotalFrames() - 1;
			updateCurrentFrameProperties();
		}

		public void Update()
		{
			updateCurrentFrameProperties();
		}


		/**
		 * Private interface.
		 */

		private void initBeginValues()
		{
			beginValues = new Dictionary<string, float>();

			for( int i = 0; i < Setup.Count; ++i )
			{
				KeyValuePair<string, object> pair = Setup.ElementAt( i );
				string name = pair.Key;

				if( !GetIsIgnoredProperty( name ) )
				{
					object value = GetObjectValue( Target, name );
					beginValues.Add( name, Convert.ToSingle( value ) );
				}
			}
		}

		private void updateCurrentFrameProperties()
		{
			Dictionary <string, float> dictionary = GetDictionaryAtFrame( frame );

			if( dictionary != null )
			{
				for( int i = 0; i < dictionary.Count; ++i )
				{
					KeyValuePair<string, float> pair = dictionary.ElementAt( i );
					SetObjectValue( Target, pair.Key, pair.Value );
				}
			}

			frame++;
		}
	}
}