using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DavidOchmann.Components
{
	[Serializable]
	public class Mutate : MonoBehaviour
	{
		public string colorComponent = "SpriteRenderer";
		private string _rotationZ;

		private Vector3 position;
		private Vector3 localScale;
		private Color _color;


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
				position.x = value;
				position.y = y;
				gameObject.transform.localPosition = position;
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
				position.y = value;
				position.x = x;
				gameObject.transform.localPosition = position;
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
			position = gameObject.transform.localPosition;
			localScale = gameObject.transform.localScale;
		}
	}
}