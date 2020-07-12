using System;

namespace ExceptionHandler.DataAccess
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ProcedureItemName : Attribute
    {
        public string Name;

        public ProcedureItemName(string name)
        {
            Name = name;
        }
    }
}
