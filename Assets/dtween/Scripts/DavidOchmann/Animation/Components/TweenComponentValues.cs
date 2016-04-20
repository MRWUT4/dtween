using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DavidOchmann.Animation
{
    [ System.Serializable ]
    public class TweenComponentValuesEvent : UnityEvent {}


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

		public string id = "easeIn";
		public bool playOnStart = true;
		public bool allowOverwrite = true;
		public bool jumpToEnd = false;
		public float duration = .6f;
		
		public PopupVO tweenMethod;
		public PopupVO easeType;
		public PopupVO easeMethod;
   
        public TweenComponentValuesEvent onStart = new TweenComponentValuesEvent();
        public TweenComponentValuesEvent onUpdate = new TweenComponentValuesEvent();
        public TweenComponentValuesEvent onComplete = new TweenComponentValuesEvent();

		public bool showSetupList = true;
		public bool showEasingList = true;
		public bool showUpdateList = true;
		public bool showOverwriteList = true;
		public bool showEventList = true;


		/**
		 * Public interface.
		 */

		/** MonoBehaviour interface. */
		public void Start()
		{
			initVariables();
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