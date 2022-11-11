using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDataContract.Validor
{
    public class EntityValidator : AbstractValidator<EntityDataContract.EntityDto>
    {
        public EntityValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.X).InclusiveBetween(0, 999);
            RuleFor(x => x.Y).InclusiveBetween(0, 999);
        }

    }
}
