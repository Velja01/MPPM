using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class DCLineSegment : Conductor
    {
        public DCLineSegment(long globalId) : base(globalId)
        {
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                DCLineSegment x = (DCLineSegment)obj;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void GetProperty(Property prop)
        {
            base.GetProperty(prop);
        }

        public override bool HasProperty(ModelCode property)
        {
            return base.HasProperty(property);
        }

        public override void SetProperty(Property property)
        {
            
            base.SetProperty(property);
        }
    }
}
