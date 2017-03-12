using System;

namespace Elurnity.Serialization
{
    public delegate void RefAction<T1, T2>(ref T1 arg1, T2 arg2);
    public delegate void RefAction<T1, T2, T3>(ref T1 arg1, T2 arg2, T3 arg3);

    public delegate T2 RefFunc<T1, T2>(ref T1 arg1);
    public delegate T3 RefFunc<T1, T2, T3>(ref T1 arg1, T2 arg2);
}
