using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PerLengthSequenceImpedance : PerLengthImpedance
    {
        private float susceptancePerLength0;
        private float susceptancePerLength;
        private float conductancePerLength0;
        private float conductancePerLength;
        private float resistancePerLength0;
        private float resistancePerLength;
        private float reactancePerLength0;
        private float reactancePerLength;

        public PerLengthSequenceImpedance(long globalId) : base(globalId)
        {
        }

        public float SusceptancePerLength0 { get => susceptancePerLength0; set => susceptancePerLength0 = value; }
        public float SusceptancePerLength { get => susceptancePerLength; set => susceptancePerLength = value; }
        public float ConductancePerLength0 { get => conductancePerLength0; set => conductancePerLength0 = value; }
        public float ConductancePerLength { get => conductancePerLength; set => conductancePerLength = value; }
        public float ResistancePerLength0 { get => resistancePerLength0; set => resistancePerLength0 = value; }
        public float ResistancePerLength { get => resistancePerLength; set => resistancePerLength = value; }
        public float ReactancePerLength0 { get => reactancePerLength0; set => reactancePerLength0 = value; }
        public float ReactancePerLength { get => reactancePerLength; set => reactancePerLength = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                PerLengthSequenceImpedance x = (PerLengthSequenceImpedance)obj;
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

        public override Property GetProperty(ModelCode propId)
        {
            return base.GetProperty(propId);
        }

        public override void GetProperty(Property property)
        {
            base.GetProperty(property);
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
