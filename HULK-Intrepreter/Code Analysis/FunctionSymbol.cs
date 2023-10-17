namespace HULK.CodeAnalysis
{
    public sealed class FunctionSymbol
    {
        public FunctionSymbol( string name, Type type, List<VariableSymbol> parameters)
        {
            Name = name;
            Type = type;
            Parameters = parameters;
        }

        public string Name { get; }
        public Type Type { get; }
        public List<VariableSymbol> Parameters { get; }
    }

}

