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
		private Quaternion localRotation;


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

		/** localPosition */
		public float x
		{
			set 
			{ 
				localPosition.x = value;
				localPosition.y = y;
				localPosition.z = z;
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
				localPosition.x = x;
				localPosition.y = value;
				localPosition.z = z;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.y; 
			}
		}

		public float z
		{
			set 
			{ 
				localPosition.x = x;
				localPosition.y = y;
				localPosition.z = value;
				gameObject.transform.localPosition = localPosition;
			}
			
			get 
			{ 
				return gameObject.transform.localPosition.z; 
			}
		}


		/** localScale */
		public float scaleX
		{
			set 
			{ 
				localScale.x = value;
				localScale.y = scaleY;
				localScale.z = scaleZ;
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
				localScale.x = scaleX;
				localScale.y = value;
				localScale.z = scaleZ;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.y; 
			}
		}

		public float scaleZ
		{
			set 
			{ 
				localScale.x = scaleX;
				localScale.y = scaleY;
				localScale.z = value;
				gameObject.transform.localScale = localScale;
			}
			
			get 
			{ 
				return gameObject.transform.localScale.z; 
			}
		}
		

		/** localRotation */
		public float rotationX
		{
			get 
		    { 
		        return localRotation.eulerAngles.x; 
		    }
		
		    set
		    { 	
				Vector3 angles = ( transform.localRotation ).eulerAngles;
				Quaternion quaternion = Quaternion.Euler( value, angles.y, angles.z );

				transform.localRotation = quaternion;
		    }
		}

		public float rotationY
		{
			get 
		    { 
		        return localRotation.eulerAngles.y;
		    }
		
		    set
		    { 	
				Vector3 angles = ( transform.localRotation ).eulerAngles;
				Quaternion quaternion = Quaternion.Euler( angles.x, value, angles.z );

				transform.localRotation = quaternion;
		    }
		}

		public float rotationZ
		{
			get 
		    { 
		        return localRotation.eulerAngles.y; 
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
			localRotation = gameObject.transform.localRotation;
		}
	}
}