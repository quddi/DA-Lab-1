using System;
using System.Collections.Generic;

namespace DA_Lab_1
{
    public abstract class DataConverter<T1, T2> : IDataConverter where T1 : IData where T2 : IData
    {
        public Type FromType => typeof(T1);

        public Type ToType => typeof(T2);

        public abstract Type? ParametersType { get; }

        public List<IData> Handle(List<IData> data, IDataConverterParameters? parameters)
        {
            return Handle(data.ToTemplateDataList<T1>(), parameters).ToGeneralDataList();
        }

        protected abstract List<T2> Handle(List<T1> data, IDataConverterParameters? parameters);
    }
}
