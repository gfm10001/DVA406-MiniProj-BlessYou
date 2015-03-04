using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlessYou
{
    public enum EnumCaseStatus
    {
        csNone =0,
        csUnknown=-1,
        csIsConfirmedSneeze=10,
        csIsConfirmedNoneSneeze=-10,
        csIsProposedSneeze=5,
        csIsProposedNoneSneeze=-5
    } // EnumCaseStatus
}
