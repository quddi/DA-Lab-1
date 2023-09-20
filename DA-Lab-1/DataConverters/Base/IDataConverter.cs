using System;
using System.Collections.Generic;

namespace DA_Lab_1
{
    interface IDataConverter
    {
        Type FromType { get; }

        Type ToType { get; }

        Type? ParametersType { get; }

        List<IData> Handle(List<IData> data, IDataConverterParameters? parameters);
    }
}
