namespace HULK.CodeAnalysis.Binding
{
    internal sealed class BoundPredefinedFunction
    {
        
        private BoundPredefinedFunction(string function, int argumentsCount, Type[] argumentsType, Type resultType)
        {
            Function = function;
            ArgumentsCount = argumentsCount;
            ArgumentsType = argumentsType;
            ResultType = resultType;
        }

        private static BoundPredefinedFunction[] _Functions =
        {
            new BoundPredefinedFunction( "rand", 0, new Type[] { }, typeof(double)),

            new BoundPredefinedFunction( "print", 1, new Type[] { typeof(void) }, typeof(void)),
            new BoundPredefinedFunction( "sqrt", 1, new Type[] { typeof(double) }, typeof(double)),
            new BoundPredefinedFunction( "exp", 1, new Type[] { typeof(double) }, typeof(double)),
            new BoundPredefinedFunction( "sen", 1, new Type[] { typeof(double) }, typeof(double)),
            new BoundPredefinedFunction( "cos", 1, new Type[] { typeof(double) }, typeof(double)),
            new BoundPredefinedFunction( "tan", 1, new Type[] { typeof(double) }, typeof(double)),
            new BoundPredefinedFunction( "cot", 1, new Type[] { typeof(double) }, typeof(double)),

            new BoundPredefinedFunction( "log", 2, new Type[] { typeof(double), typeof(double) }, typeof(double)),
        };

        public static BoundPredefinedFunction Bind(string function, int argumentsCount, Type[] argumentsType)
        {
            for (int i = 0; i < _Functions.Length; i++)
            {
                var isTypeEqual = false;

                if (_Functions[i].Function == function && _Functions[i].ArgumentsCount == argumentsCount)
                {
                    if(_Functions[i].Function == "rand" || _Functions[i].Function == "print")
                        return _Functions[i];

                    for(int j = 0; j < _Functions[i].ArgumentsType.Length; j++)
                    {
                        if (_Functions[i].ArgumentsType[j] != argumentsType[j] && argumentsType[j] != typeof(void))
                        {
                            Console.WriteLine($"{argumentsType[j]} ----- {_Functions[i].ArgumentsType[j]}");
                            isTypeEqual = false;
                            break;
                        }

                        isTypeEqual = true;
                    }

                    if (isTypeEqual)
                        return _Functions[i];
                }

            }
            return null;
        }
        public string Function { get; }
        public int ArgumentsCount { get; }
        public Type[] ArgumentsType { get; }
        public Type ResultType { get; }
    }
}