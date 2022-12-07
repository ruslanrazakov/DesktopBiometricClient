using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Core.Entities
{
    public enum ResponseStatus
    {
        Ok = 0,
        EmptyApiGateString = -1,
        WrongApiGateString = -2,
        BadRequest = -3
    }
}
