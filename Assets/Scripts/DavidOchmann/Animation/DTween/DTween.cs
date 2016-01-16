using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DavidOchmann.Animation
{
	public class DTween
	{
		public bool overwrite;
		public bool isFactory;
		private List<Tween> tweenList = new List<Tween>();
		

		public DTween(bool overwrite = false)
		{
			this.overwrite = overwrite;
		}


		/**
		 * Event interface.
		 */

		public delegate void OnUpdateEventHandler( Tween tween );
		public event OnUpdateEventHandler OnUpdate;
		
		protected virtual void InvokeUpdate(Tween tween) 
		{
			if( OnUpdate != null ) OnUpdate( tween );
		}


		/**
		 * Public interface.
		 */

		public void Update()
		{
			for( int i = tweenList.Count - 1; i >= 0; --i )
			{
				Tween tween = tweenList[ i ];

				if( tween != null )
				{
					tween.Update();
					InvokeUpdate( tween );
				
					if( tween.complete )
						tweenList.RemoveAt( i );
				}
			}
		}

		public List<Tween> Add(List<Tween> list, bool overwrite = false)
		{
			if( list != null )
			{
				for( int i = 0; i < list.Count; ++i )
				{
				    Tween tween = list[ i ];
				    Add( tween, overwrite || this.overwrite );
				}
			}

			return list;
		}

		public Tween Add(Tween tween, bool overwrite = false)
		{
			if( !isFactory )
			{
				if( overwrite || this.overwrite )
				{
					for( int i = 0; i < tweenList.Count; ++i )
					{
					    Tween item = tweenList[ i ];
					    
					    if( item.target == tween.target )
					    {
					    	tweenList[ i ] = tween;
					    	return tween;
					    }
					}
				}
				
				tweenList.Add( tween );
			}

			return tween;
		}

		public void Kill(bool killTweens = true)
		{
			for( int i = tweenList.Count - 1; i >= 0; --i )
			{
			    Tween tween = tweenList[ i ];
			    
			    if( killTweens )
			 		tween.Kill();
			 		
			 	tweenList.RemoveAt( i );   
			}
		}


		/** Ease object to values. */
		public Tween To(float duration)
		{
			return To( new {}, duration, ObjectToDictionary( new {} ), Linear.EaseNone );
		}

		public Tween To(float duration, object setup)
		{
			return To( new {}, duration, ObjectToDictionary( setup ), Linear.EaseNone );
		}

		public Tween To(object target, float duration, object setup)
		{
			return To( target, duration, ObjectToDictionary( setup ), Quad.EaseOut );
		}

		public Tween To(object target, float duration, object setup, Tween.EaseDelegate ease)
		{
			return To( target, duration, ObjectToDictionary( setup ), ease );
		}

		public Tween To(object target, float duration, Dictionary<string, object> setup, Tween.EaseDelegate ease)
		{
			Tween tween = new Tween( target, duration, setup, ease );
			Add( tween );

			return tween;
		}


		/** Ease object from values to begin values. */
		public Tween From(object target, float duration, object setup)
		{
			return From( target, duration, ObjectToDictionary( setup ), Quad.EaseOut );
		}

		public Tween From(object target, float duration, object setup, Tween.EaseDelegate ease)
		{
			return From( target, duration, ObjectToDictionary( setup ), ease );
		}

		public Tween From(object target, float duration, Dictionary<string, object> setup, Tween.EaseDelegate ease)
		{	
			Dictionary<string, object> dictionaryEnd = new Dictionary<string, object>();

			for( int i = 0; i < setup.Count; ++i )
			{
				KeyValuePair<string, object> pair = setup.ElementAt( i );
				string name = pair.Key;

				if( !Tween.GetIsIgnoredProperty( name ) )
				{
					object value = Tween.GetObjectValue( target, name );
					dictionaryEnd.Add( name, value );
				}
				else
					dictionaryEnd.Add( name, pair.Value );
			}

			Tween tweenStart = new Tween( target, 0, setup, ease );
			tweenStart.Update();

			Tween tween = To( target, duration, dictionaryEnd, ease );

			return tween;
		}

		public Dictionary<string, object> ObjectToDictionary(object setup)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();

			Type type = setup.GetType();
			
			foreach( PropertyInfo propertyInfo in type.GetProperties() )
			{
				string name = propertyInfo.Name;
				object value = propertyInfo.GetValue( setup, null );
				
				dictionary.Add( name, value );
			}

			return dictionary;
		}
	}
}