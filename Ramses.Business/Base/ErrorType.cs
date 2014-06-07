using System;
using System.Collections.Generic;
using System.Linq;

namespace Ramses.Business
{
    public enum ErrorType
    {
        IntegrityConstraintDelete,
        IntegrityConstraintUpdate,
        UniqueConstraint
    }
}
