using DA_Lab_1.Specifics.DTO;
using System;
using System.Collections.Generic;

namespace DA_Lab_1.Specifics.DataHandlers
{
    internal abstract class DataConverter<T1, T2> : IDataConverter where T1 : IData where T2 : IData
    {
        public Type FromType => typeof(T1);

        public Type ToType => typeof(T2);

        public List<IData> Handle(List<IData> data) => Handle(data);

        protected abstract List<T2> Handle(List<T1> data);
    }
}
