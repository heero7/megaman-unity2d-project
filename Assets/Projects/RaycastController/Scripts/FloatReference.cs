using System;

[Serializable]
public class FloatReference
{
    public bool UseConstant = false;
    public float ConstantValue;
    public FloatVariable Variable;
    public float Value
    {
        get => UseConstant ? ConstantValue : Variable.Value;
        set
        {
            if (UseConstant)
            {
                ConstantValue = value;
            }
            else
            {
                Variable.Value = value;
            }
        }
    }

    // Allows the objects of this type to be used like floats or <T> in comparisons, setting, etc.
    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}