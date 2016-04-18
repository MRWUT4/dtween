using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DavidOchmann.Animation
{
	// public enum TweenMethod{ To, From };
	// public enum EaseType{ Back, Bounce, Circ, Cubic, Elastic, Expo, Linear, Quad, Quart, Quint, Sine };
	// public enum EaseMethod{ EaseIn, EaseOut, EaseInOut };


	// public class TweenPropertyVO
	// {
	// 	public string name;
	// 	public float value;
	// }
	

	// [ System.Serializable ]
	// public class TweenComponentValuesEvents
	// {
	// 	public UnityEvent onStart;
	// 	public UnityEvent onUpdate;
	// 	public UnityEvent onComplete;
	// }


	// [ System.Serializable ]
	// public class TweenComponentValuesOverwrite
	// {
	// 	public bool overwrite = true;
	// 	public bool jumpToEnd = false;
	// }

	[ System.Serializable ]
	public class PopupVO
	{
		public string name;
		public int index;
		public List<string> list;
	}


	[ System.Serializable ]
	public class PopupFloatFieldVO
	{
		public List<string> list;
		public float value;
		public int index;
	}


	[ RequireComponent( typeof( Mutate ) ) ]
	public class TweenComponentValues : MonoBehaviour
	{
		public PopupVO popupComponentVO;
		public List<PopupFloatFieldVO> popupFloatFieldVOs;
		// public PopupFloatFieldVO popupFloatFieldVO;
  //       private static string EASING_NAMESPACE = "DavidOchmann.Animation.";

		public string id;
		public bool playOnStart = false;
		public float duration = .6f;
		
		public TweenMethod direction;
		public EaseType easeType;
		public EaseMethod easeMethod;
		// public TweenComponentValuesOverwrite overwrite;
		// public TweenPropertyVO[] attributes;
		// public TweenComponentValuesEvents events;

		// public DTween dTween;
		// private Dictionary<string, object> dictionary;


		// TODO: setup vo objects that are being passed to the wrapper objects.


		/**
		 * Public interface.
		 */

		/** MonoBehaviour interface. */
		public void Start()
		{
			initVariables();

			// Debug.Log( DavidOchmann.CustomEditorTools.PopupVO );
		}

		public void FixedUpdate()
		{
			
		}


		/**
		 * Private interface.
		 */

		/** Init variables. */
		private void initVariables()
		{
			
		}
	}
}