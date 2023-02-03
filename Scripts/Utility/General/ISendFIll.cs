using System;

namespace Pearl
{
    public interface ISendFill
    {
        event Action<float> FillHandler;
    }
}
