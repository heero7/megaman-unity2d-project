using System;

[Serializable]
public class FloatReference
{
    public bool useConstant = false;
    public float constantValue;
    public FloatVariable floatVariable;
    public float Value
    {
        get => useConstant ? constantValue : floatVariable._value;
        set
        {
            if (useConstant)
            {
                constantValue = value;
            }
            else
            {
                floatVariable._value = value;
            }
        }
    }
}