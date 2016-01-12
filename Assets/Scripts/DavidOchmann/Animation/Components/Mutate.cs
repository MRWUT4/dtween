using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DavidOchmann.Animation
{
	[Serializable]
	public class Mutate : MonoBehaviour
	{
		private Vector3 localPosition;
		private Vector3 localScale;


		/**
		 * Override interface.
		 */

		public void Start()
		{
			initVariables();
		}


		/**
		 * Getter / Setter.
		 */

		/** Transform properties. */
		public float x
		{
			set 
			{ 
				localPosition.x = value;
				localPosition.y = y;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.x; 
			}
		}

		public float y
		{
			set 
			{ 
				localPosition.y = value;
				localPosition.x = x;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.y; 
			}
		}

		public float scaleX
		{
			set 
			{ 
				localScale.x = value;
				localScale.y = scaleY;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.x; 
			}
		}

		public float scaleY
		{
			set 
			{ 
				localScale.y = value;
				localScale.x = scaleX;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.y; 
			}
		}
		
		public float rotationZ
		{
			get 
		    { 
		    	Vector3 angles = ( transform.localRotation ).eulerAngles;
		        return angles.z; 
		    }
		
		    set
		    { 	
				Vector3 angles = ( transform.localRotation ).eulerAngles;
				Quaternion quaternion = Quaternion.Euler( angles.x, angles.y, value );

				transform.localRotation = quaternion;
		    }
		}


		/**
		 * Private interface.
		 */

		private void initVariables()
		{
			localPosition = gameObject.transform.localPosition;
			localScale = gameObject.transform.localScale;
		}
	}
}