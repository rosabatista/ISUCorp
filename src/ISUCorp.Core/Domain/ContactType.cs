using System.ComponentModel;

namespace ISUCorp.Core.Domain
{
    public enum ContactType : byte
    {
        [Description("Type One")]
        TypeOne = 1,

        [Description("Type Two")]
        TypeTwo = 2,

        [Description("Type Three")]
        TypeThree = 3,

        [Description("Type Four")]
        TypeFour = 4
    }

    public class ContactTypeItem 
    { 
        public int Value { get; set; }

        public string Name { get; set; }
    }
}
