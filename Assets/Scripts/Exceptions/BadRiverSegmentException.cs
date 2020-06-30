using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Exceptions
{
    class BadRiverSegmentException : Exception
    {
        public BadRiverSegmentException(string segmentName) : base("Found bad river segment: " + segmentName + ". End position must have greater Z coordinate than Start position")
        {

        }
    }
}
