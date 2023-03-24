namespace KDLCompiler
{
    internal class Instruction
    {
        internal Validator validator;
        private string identifier;

        public Instruction(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get => identifier; set => identifier = value; }
    }
}