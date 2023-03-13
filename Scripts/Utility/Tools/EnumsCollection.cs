namespace Pearl
{
    public enum UpdateModes { Update, FixedUpdate, LateUpdate }

    public enum ChangeModes { Time, Velocity }

    public enum InitModes { Awake, Start }

    public enum TimeType { Scaled, Unscaled }

    public enum StartFinish { Start, Finish }

    public enum TypeTranslation
    {
        Transform,
        Rigidbody2D,
        RigidBody,
    }

    public enum FuzzyBoolean { No = -1, IDoNo = 0, Yes = 1 }

    public enum StateButton { Down, Up, Press }

    public enum DeleteGameObjectEnum { Disable, Destroy }

    public enum DimensionsEnum { TwoDimension, ThreeDimension }

    public enum InformationTypeEnum { Error, Info, None, Warning }

    public enum StringTypeControl { Name, Tags, Layer, Tag }

    public enum WorldReference { World, Local }

    public enum TypeReferenceEnum { Absolute, Relative }

    public enum EnableEnum { Enable, Disable }

    public enum NumberStruct { Float, Vector2, Vector3, Vector4 }

    public enum ChangeTypeEnum { Setting, Modify }

    public enum LockEnum { Lock, Unlock }

    public enum ComparerEnum { Major, Minor, Equal }

    public enum TypePathEnum { Once, PingPong, Loop }

    public enum BlackboardTypeEnum { Graph, Local }

    public enum TypeNumber { Float, Integer }

    public enum TypeSibilling { First, SpecificIndex, Last }

    public enum PrimitiveEnum { Bool, Integer, Float, Enum, String, Vector2, Vector3 }

    public enum CompareElementsEnum { Major, Minor, Equal, EqualOrMinor, EqualOrMajor, Different }

    public enum TriggerType { Enter, Exit }

    public enum ActionEvent { Add, Remove }

    public enum TypeUnityElementEnum { GameObject, Component }

    public enum TypeParameter { Integer, Float, String, Boolean, Various }

    public enum HorizontalEnum { Left, Right }

    public enum FadeEnum
    {
        FadeIn,
        FadeOut,
    }

    public enum RoundsEnum
    {
        Ceil,
        Floor,
        Round,
    }

    public enum BoolEnum
    {
        False = 0,
        True = 1
    }
}