using System;

[Serializable]
public class VariableReference<T> where T : Variable<T>
{
    public bool UseConstant = false;
    public T ConstantValue;
    public T Variable;
    public T Value
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
}