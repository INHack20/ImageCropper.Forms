namespace MauiTOCropViewController
{
    public enum TOCropViewCroppingStyle : ulong
    {
        Default,
        Circular
    }

    public enum TOCropViewControllerAspectRatioPreset : ulong
    {
        Original,
        Square,
        TOCropViewControllerAspectRatioPreset3x2,
        TOCropViewControllerAspectRatioPreset5x3,
        TOCropViewControllerAspectRatioPreset4x3,
        TOCropViewControllerAspectRatioPreset5x4,
        TOCropViewControllerAspectRatioPreset7x5,
        TOCropViewControllerAspectRatioPreset16x9,
        Custom
    }

    public enum TOCropViewControllerToolbarPosition : ulong
    {
        Bottom,
        Top
    }
}
