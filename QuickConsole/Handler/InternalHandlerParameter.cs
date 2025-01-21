using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickConsole.Handler
{
    internal class InternalHandlerParameter
    {
    }

    public class Parameter
    {
        private readonly Type _parameterType;
        private readonly object? _parameterValue;
        private readonly IList<object>? _parameterValueArray = [];

        public Parameter(string commandLineParam)
        {
            // try parse and set Type int string bool i []

            _parameterValue = 5;
            _parameterValueArray = [4, 5, 6];
        }

        public (Type type, object value) GetValue(uint index)
        {
            return (_parameterType, _parameterValue);
        }

        public T CastObject<T>(object input)
        {
            return (T)input;
        }

        public T ConvertObject<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        public dynamic CastFromType(object input, Type type)
        {
            dynamic changedObj = Convert.ChangeType(input, type);
            //changedObj.Meod();
            return changedObj;
        }
    }

    public class ParameterBuiler
    {
        private readonly IList<Parameter> _quickParameters = [];
        public ParameterBuiler(string[] commandLineParams)
        {
            foreach (var param in commandLineParams)
            {
                var quickParam = new Parameter(param);
                _quickParameters.Add(quickParam);
            }
        }

        public IList<Parameter> GetParameters()
        {
            return _quickParameters;
        }
    }
}
