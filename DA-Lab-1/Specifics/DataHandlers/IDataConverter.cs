using DA_Lab_1.Specifics.DTO;
using System;
using System.Collections.Generic;

namespace DA_Lab_1.Specifics.DataHandlers
{
    interface IDataConverter
    {
        Type FromType { get; }

        Type ToType { get; }

        List<IData> Handle(List<IData> data);
    }
}
