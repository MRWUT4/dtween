using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DavidOchmann.Animation
{
	public enum TweenMethod{ To, From };
	public enum EaseType{ Back, Bounce, Circ, Cubic, Elastic, Expo, Linear, Quad, Quart, Quint, Sine };
	public enum EaseMethod{ EaseIn, EaseOut, EaseInOut };


	[ System.Serializable ]
	public class TweenAttributeVO
	{
		public string property;
		public float value;
	}
	

	[ System.Serializable ]
	public class TweenGameObjectEvents
	{
		public UnityEvent onStart;
		public UnityEvent onUpdate;
		public UnityEvent onComplete;
	}


	[ RequireComponent( typeof( Mutate ) ) ]
	public class TweenGameObject : MonoBehaviour
	{
		public string id;
		public bool playOnStart = false;
		public float duration = .6f;
		public TweenMethod direction;
		public EaseType easeType;
		public EaseMethod easeMethod;

		public TweenAttributeVO[] attributes;
		public TweenGameObjectEvents events;

		private DTween dTween;
		private Mutate mutate;
		private Dictionary<string, object> dictionary;


		/**
		 * Public interface.
		 */

		public void Start()
		{
			initVariables();
			initDictionary();

			if( playOnStart )
				Play();
		}

		public void FixedUpdate()
		{
			dTween.Update();
		}


		public Tween.EaseDelegate GetDelegateFromSetup()
		{
			string typeName = Enum.GetName( typeof( EaseType ), easeType );
			string methodName = Enum.GetName( typeof( EaseMethod ), easeMethod );

			Type type = Type.GetType( typeName );
			MethodInfo methodInfo = type.GetMethod( methodName, BindingFlags.Public | BindingFlags.Static );

			Tween.EaseDelegate easeDelegate = (Tween.EaseDelegate)Delegate.CreateDelegate( typeof( Tween.EaseDelegate ), methodInfo );

			return easeDelegate;
		}

		public void Play(string id = null)
		{
			if( id != null)
			{
				TweenGameObject[] tweenGameObjects = GetComponents<TweenGameObject>();

				for( int i = 0; i < tweenGameObjects.Length; ++i )
				{
				    TweenGameObject tweenGameObject = tweenGameObjects[ i ];

				    if( this.id == id )
				 		tweenGameObject.Play();   
				}
			}
			else
				initTween();
		}

		public void PlayAll()
		{
			TweenGameObject[] tweenGameObjects = GetComponents<TweenGameObject>();

			for( int i = 0; i < tweenGameObjects.Length; ++i )
			{
			    TweenGameObject tweenGameObject = tweenGameObjects[ i ];
			 	tweenGameObject.Play();   
			}
		}


		/**
		 * Private interface.
		 */

		/** Init variables. */
		private void initVariables()
		{
			dTween = new DTween();
			mutate = GetComponent<Mutate>();
			dictionary = new Dictionary<string, object>();
		}


		/** Create dictionary with attibutes elements. */
		private void initDictionary()
		{
			for( int i = 0; i < attributes.Length; ++i )
			{
			    TweenAttributeVO attribute = attributes[ i ];
			    dictionary.Add( attribute.property, attribute.value );
			}
		}


		/** Start Tween. */
		private void initTween()
		{
			Tween.EaseDelegate easeDelegate = GetDelegateFromSetup();
			Tween tween = null;

			switch( direction )
			{
				case TweenMethod.To:
					tween = dTween.To( mutate, duration, dictionary, easeDelegate );
					break;

				case TweenMethod.From:
					tween = dTween.From( mutate, duration, dictionary, easeDelegate );
					break;
			}

			tween.OnStart += dTweenOnStartHandler;
			tween.OnComplete += dTweenOnCompleteHandler;
			tween.OnUpdate += dTweenOnUpdateHandler;
		}

		private void dTweenOnStartHandler(Tween tween)
		{
			events.onStart.Invoke();
		}

		private void dTweenOnUpdateHandler(Tween tween)
		{
			events.onUpdate.Invoke();
		}

		private void dTweenOnCompleteHandler(Tween tween)
		{
			events.onComplete.Invoke();
		}
	}
}