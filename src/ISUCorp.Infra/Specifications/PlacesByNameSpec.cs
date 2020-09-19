using ISUCorp.Core.Domain;
using ISUCorp.Infra.Contracts;

namespace ISUCorp.Infra.Specifications
{
    public class PlacesByNameSpec : BaseSpecification<Place>
    {
        public PlacesByNameSpec(string name)
            : base(e => e.Name.ToLower() == name.Trim().ToLower())
        {

        }
    }
}
