# BurstImageProcessing

This project is *experimental & unofficial*, a **proof-of-concept**.  Don't try using it in any serious capacity yet. 

## [Demo 1 - Effect Composer](/Assets/Scripts/EffectComposer.cs)

This demo demonstrates processing input from a webcam in real time. The main bottleneck in doing this is copying data from the webcam and back to a texture.  those problems haven't been solved yet, and are two big roadblocks to practical use of this approach.

The [effect composer](/Assets/Scripts/EffectComposer.cs) allows you to define the image effect in a granular , per channel way.
You can find it on the `WebcamDisplay` object in the `WebcamComposerDemo` scene.


There's three factors for each channel in the composer:
  
  1) the [Operator](/Assets/Scripts/Constants/Operators.cs), which defines the mathematical or bitwise operation to use
 
  2) the [Comparator](/Assets/Scripts/Constants/Comparators.cs), which defines the comparison to use.  One of `Greater`, `Equal`, `Less`.
 
  3) the [Operand](/Assets/Scripts/Constants/Operand.cs), which defines whether the Operator works against the pixel's own value or the threshold value.  `Self` means operate on the pixel's value, `Other` means operate on the threshold value.
  
In addition, you can also enable / disable processing for each color channel, as well as changing the value of the color threshold.

If this sounds confusing, i would encourage you to just try out the demo scene and see how the image processing changes as you change the parameters.
