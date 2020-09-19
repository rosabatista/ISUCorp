using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contracts;

namespace ISUCorp.Infra.Specifications
{
    public class ContactsByNameSpec : BaseSpecification<Contact>
    {
        public ContactsByNameSpec(string name)
            : base(e => e.Name.ToLower() == name.Trim().ToLower())
        {

        }
    }
}
