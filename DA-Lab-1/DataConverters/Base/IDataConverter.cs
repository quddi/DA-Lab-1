using DA_Lab_1.DataConverters.Base;
using DA_Lab_1.DTO.Base;
using System;
using System.Collections.Generic;

namespace DA_Lab_1.Specifics.DataHandlers
{
    interface IDataConverter
    {
        Type FromType { get; }

        Type ToType { get; }

        Type? ParametersType { get; }

        List<IData> Handle(List<IData> data, IDataConverterParameters? parameters);
    }
}
