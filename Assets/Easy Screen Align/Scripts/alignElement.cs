using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eageramoeba.DetectResize;

namespace eageramoeba.DetectResize {

	[System.Serializable]
	public class alignElement : MonoBehaviour {
		
		[SerializeField, HideInInspector]
		public Camera GUICamera;
	
		[SerializeField, HideInInspector]
		public int alignE = 4;
		[SerializeField, HideInInspector]
		public int alignO = 4;

		private float leftConstraint = 0.0f;
		private float rightConstraint = 0.0f;
		private float topConstraint = 0.0f;
		private float bottomConstraint = 0.0f;
		private float distanceZ = 10.0f;
	
		[SerializeField, HideInInspector]
		public float sideF = 0;
		[SerializeField, HideInInspector]
		public float verF = 0;

		[SerializeField, HideInInspector]
		public Vector3 alignVector;
		[SerializeField, HideInInspector]
		public Vector3 alignVectorObj;

		private Camera useCam;
	
		private bool canGo;
		private bool canGo2;
		
		[SerializeField, HideInInspector]
		public bool onResize = true;
		[SerializeField, HideInInspector]
		public bool onMove = true;

		[SerializeField, HideInInspector]
		public bool usePercentage = true;

		[SerializeField, HideInInspector]
		public bool invert = false;
		[SerializeField, HideInInspector]
		public bool invertL = false;

		[SerializeField, HideInInspector]
		public bool lockX = false;	
		[SerializeField, HideInInspector]
		public bool lockY = false;
		[SerializeField, HideInInspector]
		public bool lockZ = false;
		[SerializeField, HideInInspector]
		public float lockXFloat = 0;
		[SerializeField, HideInInspector]
		public float lockYFloat = 0;
		[SerializeField, HideInInspector]
		public float lockZFloat = 0;

		[SerializeField, HideInInspector]
		public bool lockXR = false;
		[SerializeField, HideInInspector]
		public bool lockYR = false;
		[SerializeField, HideInInspector]
		public bool lockZR = false;
		[SerializeField, HideInInspector]
		public float lockXFloatR = 0;
		[SerializeField, HideInInspector]
		public float lockYFloatR = 0;
		[SerializeField, HideInInspector]
		public float lockZFloatR = 0;
	
		[SerializeField, HideInInspector]
		public Vector3 forwardRot = Vector3.up;
		[SerializeField, HideInInspector]
		public Vector3 offsetR = Vector3.zero;

		[SerializeField, HideInInspector]
		public int lookAtMode = 0;
		[SerializeField, HideInInspector]
		public int boundsMode = 1;

		private float sideFI;
		private float verFI;
		
		private Vector3 camPos;
		private Quaternion camRot;		
		private bool camMode;
		private Transform camTrans;
		
		private Vector3 lookPos;
		private Vector3 overrideVector;
		private Vector3 sendVector;
		private Vector3 sizeVector;
		private Vector3 sizeVectorL;

		private Vector3 originalRot;

		[SerializeField, HideInInspector]
		public bool lerpTransform = false;
		[SerializeField, HideInInspector]
		public bool lerpTime = false;
		[SerializeField, HideInInspector]
		public bool lerpLookat = false;
		[SerializeField, HideInInspector]
		public float lerpStepTransform = 0.25f;
		[SerializeField, HideInInspector]
		public float lerpStepLookat = 0.25f;
		[SerializeField, HideInInspector]
		public bool lerpTolerance = false;
		[SerializeField, HideInInspector]
		public bool smoothTolerance = true;
		[SerializeField, HideInInspector]
		public float lerpToleranceF = 1;
		[SerializeField, HideInInspector]
		public float smoothToleranceF = .3f;

		[SerializeField, HideInInspector]
		public Vector2 boundsSet;
		[SerializeField, HideInInspector]
		public GameObject boundsObj;

		private float timeStep;

		private Vector3 velocity = Vector3.zero;
		private float yVelocity = 0.0F;
		private float xVelocity = 0.0F;
		private float zVelocity = 0.0F;

		private float xVal;
		private float yVal;
		private float zVal;

		private float screenP;

		private Quaternion tempToRot;

		private Vector3 centerPt;
		private Vector3 newPos;
		private Vector3 offset;
		private Vector3 tOV;
		private Vector3 offsetRT;

		private Vector3 cen = Vector3.zero;
		private Vector3 ext = Vector3.zero;
		private Vector2 min = Vector3.zero;
		private Vector2 max = Vector3.zero;

		private float minX = Mathf.Infinity;
		private float minY = Mathf.Infinity;
		private float maxX = -Mathf.Infinity;
		private float maxY = -Mathf.Infinity;
		private Bounds bounds;
		private Vector3 v3Center;
		private Vector3 v3Extents;
		private Vector3[] corners = new Vector3[8];
		private float width;
		private float height;
		private float area;
		private float percentage;

		private float screenPX;
		private float screenPY;


		void Start () {
			if (Camera.main || useCam.GetComponent<Camera>()) {
				originalRot = transform.rotation.eulerAngles;
				getLevelBounds();
			}
		}
		
		void Update () {
			if (Camera.main || useCam.GetComponent<Camera>()) {
				canGo = true;
				if (onResize) {
					canGo = detectResize.hasResized;
				}
				canGo2 = false;
				if (onMove) {
					if (camPos != camTrans.position || camRot != camTrans.rotation || useCam.orthographic != camMode) {
						canGo2 = true;
					}
				}
				if (canGo || canGo2) {
					getLevelBounds();
					if (usePercentage) {
						screenPX = Screen.width / 100;
						screenPY = Screen.height / 100;
						screenP = screenPY;
					} else {
						screenP = 1;
						screenPY = 1;
						screenPX = 1;
					}
					if (lerpTime) {
						timeStep = Time.timeScale;
					} else {
						timeStep = 1;
					}
					alignVector = Vector3.zero;

					verFI = verF * screenPY;
					sideFI = sideF * screenPX;

					alignVector.x = rightConstraint / 2;
					alignVector.y = topConstraint / 2;

					switch (boundsMode) {
						case 0:
							sizeVector = getObjSize() / 2;
							break;
						case 1:
							sizeVector = CalcScreenPercentage() / 2;
							break;
						case 2:
							sizeVector = CalcScreenPercentage() / 2;
							break;
						case 3:
							sizeVector = CalcScreenPercentage() / 2;
							break;
						case 4:
							sizeVector = CalcScreenPercentage() / 2;
							break;
					}

					switch (alignE) {
						case 8:
							alignVector.x = rightConstraint - sideFI;
							alignVector.y = bottomConstraint + verFI;
							break;
						case 7:
							alignVector.y = bottomConstraint + verFI;
							break;
						case 6:
							alignVector.x = leftConstraint + sideFI;
							alignVector.y = bottomConstraint + verFI;
							break;
						case 5:
							alignVector.x = rightConstraint - sideFI;
							break;
						case 4:
							alignVector.x = Screen.width / 2 + sideFI;
							alignVector.y = Screen.height / 2 + verFI;
							break;
						case 3:
							alignVector.x = leftConstraint + sideFI;
							break;
						case 2:
							alignVector.y = topConstraint - verFI;
							alignVector.x = rightConstraint - sideFI;
							break;
						case 1:
							alignVector.y = topConstraint - verFI;
							break;
						case 0:
							alignVector.y = topConstraint - verFI;
							alignVector.x = leftConstraint + sideFI;
							break;
						default:
							alignVector.x = Screen.width / 2 + sideFI;
							alignVector.y = Screen.height / 2 + verFI;
							break;
					}

					switch (alignO) {
						case 8:
							alignVector.x -= sizeVector.x;
							alignVector.y += sizeVector.y;
							break;
						case 7:
							alignVector.y += sizeVector.y;
							break;
						case 6:
							alignVector.x += sizeVector.x;
							alignVector.y += sizeVector.y;
							break;
						case 5:
							alignVector.x -= sizeVector.x;
							break;
						case 4:
							break;
						case 3:
							alignVector.x += sizeVector.x;
							break;
						case 2:
							alignVector.y -= sizeVector.y;
							alignVector.x -= sizeVector.x;
							break;
						case 1:
							alignVector.y -= sizeVector.y;
							break;
						case 0:
							alignVector.y -= sizeVector.y;
							alignVector.x += sizeVector.x;
							break;
						default:
							break;
					}


					if (invert) {
						alignVector.x = -alignVector.x;
					}
					if (invert) {
						alignVector.y = -alignVector.y;
					}

					overrideVector = useCam.WorldToScreenPoint(transform.position);
					if (overrideVector.y < 0) {
						overrideVector.y = 0;
					}
					if (overrideVector.x < 0) {
						overrideVector.x = 0;
					}
					lookPos = camTrans.position;
					if (invertL) {
						lookPos = 2 * transform.position - lookPos;
					}
					alignVector.z = -distanceZ;
					if (lerpTolerance) {
						centerPt = new Vector3(alignVector.x, alignVector.y, -distanceZ);
						newPos = centerPt + (overrideVector - centerPt);
						offset = newPos - centerPt;
						tOV = centerPt + Vector3.ClampMagnitude(offset, lerpToleranceF * screenP);
						if (smoothTolerance) {
							overrideVector = Vector3.Slerp(overrideVector, tOV, smoothToleranceF * timeStep);
						} else {
							overrideVector = tOV;
						}
					}
					if (lockX) {
						if (lerpTransform) {
							overrideVector.x = lockXFloat * (Screen.width / 100);
						}
						alignVector.x = lockXFloat * (Screen.width / 100);
					}
					if (lockY) {
						if (lerpTransform) {
							overrideVector.y = lockYFloat * (Screen.height / 100);
						}
						alignVector.y = lockYFloat * (Screen.height / 100);
					}
					if (lockZ) {
						if (lerpTransform) {
							overrideVector.z = lockZFloat;
						}
						alignVector.z = lockZFloat;
					}
					if (lerpTransform) {
						overrideVector = useCam.ScreenToWorldPoint(overrideVector);
						alignVector = useCam.ScreenToWorldPoint(alignVector);
						transform.position = overrideVector;
						transform.position = Vector3.Slerp(transform.position, alignVector, timeStep * lerpStepTransform);
					} else {
						alignVector = useCam.ScreenToWorldPoint(alignVector);
						transform.position = alignVector;
					}
					switch (lookAtMode) {
						case 1:
							if (lerpLookat) {
								tempToRot = Quaternion.LookRotation(lookPos - transform.position, forwardRot);
								if (lockXR) {
									xVal = lockXFloatR;
								} else {
									xVal = tempToRot.eulerAngles.x;
								}
								if (lockYR) {
									yVal = lockYFloatR;
								} else {
									yVal = tempToRot.eulerAngles.y;
								}
								if (lockZR) {
									zVal = lockZFloatR;
								} else {
									zVal = tempToRot.eulerAngles.z;
								}
								tempToRot = Quaternion.Euler(xVal, yVal, zVal);
								transform.rotation = Quaternion.Slerp(transform.rotation, tempToRot, lerpStepLookat * timeStep);
							} else {
								transform.LookAt(lookPos, forwardRot);
								if (lockXR) {
									xVal = lockXFloatR;
								} else {
									xVal = transform.eulerAngles.x;
								}
								if (lockYR) {
									yVal = lockYFloatR;
								} else {
									yVal = transform.eulerAngles.y;
								}
								if (lockZR) {
									zVal = lockZFloatR;
								} else {
									zVal = tempToRot.eulerAngles.z;
								}
								transform.rotation = Quaternion.Euler(xVal, yVal, zVal);
							}
							break;
						case 2:
							if (lerpLookat) {
								lookPos = -useCam.transform.TransformDirection(Vector3.forward).normalized;
								lookPos = transform.position + lookPos;
								if (invertL) {
									lookPos = 2 * transform.position - lookPos;
								}
								tempToRot = Quaternion.LookRotation(lookPos - transform.position, forwardRot);
								if (lockXR) {
									xVal = lockXFloatR;
								} else {
									xVal = tempToRot.eulerAngles.x;
								}
								if (lockYR) {
									yVal = lockYFloatR;
								} else {
									yVal = tempToRot.eulerAngles.y;
								}
								if (lockZR) {
									zVal = lockZFloatR;
								} else {
									zVal = -camTrans.eulerAngles.z;
								}
								tempToRot = Quaternion.Euler(xVal, yVal, zVal);
								transform.rotation = Quaternion.Slerp(transform.rotation, tempToRot, lerpStepLookat * timeStep);
							} else {
								lookPos = -useCam.transform.TransformDirection(Vector3.forward).normalized;
								lookPos = transform.position + lookPos;
								if (invertL) {
									lookPos = 2 * transform.position - lookPos;
								}
								transform.LookAt(lookPos, forwardRot);
								if (lockXR) {
									xVal = lockXFloatR;
								} else {
									xVal = transform.eulerAngles.x;
								}
								if (lockYR) {
									yVal = lockYFloatR;
								} else {
									yVal = transform.eulerAngles.y;
								}
								if (lockZR) {
									zVal = lockZFloatR;
								} else {
									zVal = -camTrans.eulerAngles.z;
								}
								transform.rotation = Quaternion.Euler(xVal, yVal, zVal);
							}
							break;
						case 0:
							break;
					}
				}
				if (onMove) {
					camPos = camTrans.position;
					camRot = camTrans.rotation;
					camMode = useCam.orthographic;
				}
			} else {
				Debug.Log("No camera present in scene");
			}
		}

		void getLevelBounds() {
			if (GUICamera) {
				useCam = GUICamera;
			} else {
				useCam = Camera.main;
				GUICamera = useCam;
			}
			camTrans = useCam.transform;
			if (!useCam.GetComponent<Camera>().orthographic) {
				distanceZ = useCam.GetComponent<Camera>().nearClipPlane;
			} else {
				distanceZ = -10;
			}
			leftConstraint = 0;
			rightConstraint = Screen.width;
			topConstraint = Screen.height;
			bottomConstraint = 0;
		}

		Vector3 getObjSize() {
			bool error = false;
			Vector3[] tARR = new Vector3[4];
			if (gameObject.GetComponent<Renderer>() || boundsObj
#if UNITY_5 || UNITY_5_3_OR_NEWER
				|| gameObject.GetComponent<RectTransform>()
#endif
				) {
				if (boundsObj) {
					if (boundsObj.GetComponent<Renderer>()) {
						cen = boundsObj.GetComponent<Renderer>().bounds.center;
						ext = boundsObj.GetComponent<Renderer>().bounds.extents;
#if UNITY_5 || UNITY_5_3_OR_NEWER
					} else if (boundsObj.GetComponent<RectTransform>()) {
						cen = boundsObj.GetComponent<RectTransform>().position;
						boundsObj.GetComponent<RectTransform>().GetWorldCorners(tARR);
						ext = boundsObj.GetComponent<RectTransform>().position-tARR[1];
#endif
					} else {
						error = true;
					}
				} else if(gameObject.GetComponent<Renderer>()) {
					cen = gameObject.GetComponent<Renderer>().bounds.center;
					ext = gameObject.GetComponent<Renderer>().bounds.extents;
#if UNITY_5 || UNITY_5_3_OR_NEWER
				} else if (gameObject.GetComponent<RectTransform>()) {
					cen = gameObject.GetComponent<RectTransform>().position;
					gameObject.GetComponent<RectTransform>().GetWorldCorners(tARR);
					ext = gameObject.GetComponent<RectTransform>().position - tARR[1];
#endif
				} else {
					error = true;
				}
				if (!error) {
					Vector2[] extentPoints = new Vector2[8]
					{
			useCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
			useCam.WorldToScreenPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
					};
					min = extentPoints[0];
					max = extentPoints[0];
					foreach (Vector2 v in extentPoints) {
						min = Vector2.Min(min, v);
						max = Vector2.Max(max, v);
					}
					return new Vector3(max.x - min.x, max.y - min.y, 0);
				}
			} else {
				error = true;
			}
			if (error) {
				Debug.Log(gameObject.name + ": Bounds not set, nothing to use");
			}
			return Vector3.zero;
		}

		Vector3 CalcScreenPercentage() {
			bool error = false;

			minX = Mathf.Infinity;
			minY = Mathf.Infinity;
			maxX = -Mathf.Infinity;
			maxY = -Mathf.Infinity;

			if (boundsMode == 4) {
				v3Center = Vector3.zero;
				v3Extents = boundsSet;
			} else if (boundsObj) {
				if (
#if UNITY_4_5 || UNITY_4_6 || UNITy_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5 || UNITY_5_3_OR_NEWER
					boundsObj.GetComponent<Collider2D>() || boundsObj.GetComponent<SpriteRenderer>() || 
#endif
					boundsObj.GetComponent<Renderer>() || boundsObj.GetComponent<MeshFilter>() || boundsObj.GetComponent<Collider>()) {
					if (boundsMode == 1) {
						if (boundsObj.GetComponent<MeshFilter>()) {
							bounds = boundsObj.GetComponent<MeshFilter>().mesh.bounds;
						} else {
							error = true;
						}
					}
					if (boundsMode == 2) {
#if UNITY_4_5 || UNITY_4_6 || UNITy_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5 || UNITY_5_3_OR_NEWER
						if (boundsObj.GetComponent<SpriteRenderer>()) {
							bounds = boundsObj.GetComponent<SpriteRenderer>().sprite.bounds;
						} else {
							error = true;
						}
#else
						Debug.Log("Sprite mode not supported below Unity 4.5");
#endif
					}
					if (boundsMode == 3) {
						if (boundsObj.GetComponent<Collider>()) {
							bounds = boundsObj.GetComponent<Collider>().bounds;
#if UNITY_4_5 || UNITY_4_6 || UNITy_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5 || UNITY_5_3_OR_NEWER
						} else if (boundsObj.GetComponent<Collider2D>()) {
							bounds = boundsObj.GetComponent<Collider2D>().bounds;
#endif
						} else {
							error = true;
						}
					}
					if (!error) {
						v3Center = bounds.center;
						v3Extents = bounds.extents;
						if (boundsMode != 3) {
							v3Extents.y = v3Extents.y * boundsObj.transform.localScale.y;
							v3Extents.x = v3Extents.x * boundsObj.transform.localScale.x;
							v3Extents.z = v3Extents.z * boundsObj.transform.localScale.z;
							v3Extents = boundsObj.transform.localRotation * v3Extents;
						}
					}
				} else {
					error = true;
				}
			} else if (gameObject) {
				if (boundsMode == 1) {
					if (GetComponent<MeshFilter>()) {
						bounds = GetComponent<MeshFilter>().mesh.bounds;
					} else {
						error = true;
					}
				}
				if (boundsMode == 2) {
#if UNITY_4_5 || UNITY_4_6 || UNITy_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5 || UNITY_5_3_OR_NEWER
					if (GetComponent<SpriteRenderer>()) {
						bounds = GetComponent<SpriteRenderer>().sprite.bounds;
					} else {
						error = true;
					}
#else
					Debug.Log("Sprite mode not supported below Unity 4.5");
#endif
				}
				if (boundsMode == 3) {
					if (GetComponent<Collider>()) {
						bounds = GetComponent<Collider>().bounds;
#if UNITY_4_5 || UNITY_4_6 || UNITy_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5 || UNITY_5_3_OR_NEWER
					} else if (GetComponent<Collider2D>()) {
						bounds = GetComponent<Collider2D>().bounds;
#endif
					} else {
						error = true;
					}
				}
				if (!error) {
					v3Center = bounds.center;
					v3Extents = bounds.extents;
					if (boundsMode != 3) {
						v3Extents.y = v3Extents.y * transform.localScale.y;
						v3Extents.x = v3Extents.x * transform.localScale.x;
						v3Extents.z = v3Extents.z * transform.localScale.z;
					}
				}
				} else {
				error = true;
			}
			if (!error) {
				if (boundsMode == 2) {
					v3Center = Vector3.zero;
				}

				corners = new Vector3[8];

				corners[0] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
				corners[1] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
				corners[2] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
				corners[3] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
				corners[4] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
				corners[5] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
				corners[6] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
				corners[7] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner


				for (int i = 0; i < corners.Length; i++) {
					Vector3 corner = corners[i];
					if (!useCam.GetComponent<Camera>().orthographic) {
						if (lockZ) {
							corner.z += lockZFloat;
						} else {
							corner.z += distanceZ;
						}
					}
					corner = camTrans.TransformPoint(corner);
					corner = useCam.WorldToScreenPoint(corner);
					if (corner.x > maxX) maxX = corner.x;
					if (corner.x < minX) minX = corner.x;
					if (corner.y > maxY) maxY = corner.y;
					if (corner.y < minY) minY = corner.y;
					minX = Mathf.Clamp(minX, 0, Screen.width);
					maxX = Mathf.Clamp(maxX, 0, Screen.width);
					minY = Mathf.Clamp(minY, 0, Screen.height);
					maxY = Mathf.Clamp(maxY, 0, Screen.height);
				}
				width = maxX - minX;
				height = maxY - minY;
				return new Vector3(width, height, 0);
			} else {
				Debug.Log(gameObject.name+": Bounds not set, nothing to use");
			}
			return Vector3.zero;
		}
	}
	
}
