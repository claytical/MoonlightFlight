---Easy Screen Align README---

Hello and thankyou for purchasing this script!

'Easy Screen Align' does what it's title suggests. It is a simple script for aligning UI/Game elements to the screen via your GUI/Main camera.

If you have any issues or improvements for us, please let us know via our websites contact page - http://eageramoeba.co.uk/Contact/

--AQHAT--
We have more than just 'Easy Screen Align' on the asset store.
Our best asset is an Auto Quality solution (AQHAT) for Unity 4.5+ - http://u3d.as/CdY

--Contents--
-An Editor folder containing the custom inspector script
-A prefabs folder containing a sample implementation of the methods.
-A scenes folder containing a debug/test scene.
-A scripts folder containing alignElement.cs and detectResize.cs.
-This README file

--Quick start/example method--
1. Open a scene containing your GUI Camera and the element you want to align. It is reccomended you use a static GUI camera separate to your main camera, however this is not essential and the script will work with other configurations.
2. Insert the DetectResize prefab into your scene.
3. Add the alignElement.cs script to the element you want to align to your screen.
4. Set the GUI Camera, Anchor and spacing settings. If in Unity 4 (base) or below, you may want to use the Invert option.
5. Enjoy!

--Notes--
-This asset has been built with UI in mind but it can be respurposed or modified for other uses.
-The script works with both UI and perspective cameras.
-The script can align sprites, meshes, rect transforms, colliders and pretty much anything unity can handle.
-The script can align objects to a moving camera, but this is not reccommended due to performance concerns.

--SCRIPT DETAILS--
In order to access any variabels or fubnctions from either script you will need to include the namescape for this module - 'eageramoeba.DetectResize'

--detectResize.cs--
Monitors the screen resolution and detects whether it has changed, it then provides this information via a static variable 'detectResize.hasResized'.

Update frequency:
How frequent, in seconds, the script checks to see if the screen has resized.

--alignElement.cs--
This script requires you include detectResize.cs once within your scene.

GUI Camera:
This is the camera that the element will be aligned to

Trigger on resize:
Triggers the script to run when the screen has been resized, useful for static UI.
(Can be used in tandemn with Trigger on camera move).

Trigger on camera move:
Triggers the script to run when the GUI camera has moved, changed mode or rotated, useful for anything that requires the object to lock to a variable camera.

Use percentage:
Make all spacial x/y values use the percentage of the relevant screen side. X co-ordinates use the width and Y co-ordinates use the height.

Object Anchor:
The base anchor point for the object, it will align to the Screen Anchor based upon this point on the objects rendered image. Represented via a selection grid.

Screen Anchor:
The base anchor point on the screen, represented via a selection grid.

X Spacing:
Space the x axis from the anchor, uses percentage when enabled.

Y Spacing:
Space the y axis from the anchor, uses percentage when enabled.

Invert Align:
Invert the align vector on the x/y axis.

LookatMode:
Set the mode for lookat.
None - No lookat
Camera - Look at the camera transform.
Screen - "Face" the screen.

Lookat world up:
The world up direction.

Lookat Rotation Offset:
Offset the lookat rotation using a Vector3.

Invert Lookat:
Invert the lookat direction.

Lock x movement:
Lock the x axis, does not use percentage.

Lock y movement:
Lock the y axis, does not use percentage.

Lock z-distance:
Lock the Z axis to a number, effects only the distance from the camera. Does not use percentage.

Lock x lookat:
Locks the x rotation axis, does not use percentage.

Lock y lookat:
Locks the y rotation axis, does not use percentage.

Lock z lookat:
Locks the z rotation axis, does not use percentage.

Lerp by timescale:
Have the lerp be effected by the timescale.

Lerp Transform:
The amount to smooh/lerp the x/y/z transform. If axis is locked it will not be effected by this.

Lerp Transform Tolerance:
Radius of a sphere that the transform cannot move away from, based upon the anchor point. Uses percentage when enabled.

Smooth tolerance:
Smooth the tolerance.

Lerp lookat:
The amount to smooh/lerp the lookat. If rotation axis is locked it will not be effected by this.

Bounds Mode:
Set the mode when detecting the object bounds; Renderer, Mesh, Sprite, Collider, Override
Renderer - Use Unity's renderer.bounds property for the bounds, or the calculated bounds for UI rect transforms. Works best for 3D objects with no lookat, or Unity UI objects.
Mesh - Use the mesh's bounds (if within a bounds object, this is rotated by localRotation).
Sprite - Use the sprites bounds.
Collider - Use the colliders bounds.
Override - Use the override property in place of any object bounds.

Bounds Set:
Set the bounds value to be used when override mode is selected, does not use percentage.

Bounds Object:
The object to use for bounds data, useful when wrapping a mesh within an empty. Ensure this object has the relevant component attached.