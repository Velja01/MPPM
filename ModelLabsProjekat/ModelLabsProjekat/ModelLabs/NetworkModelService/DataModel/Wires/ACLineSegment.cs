using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class ACLineSegment : Conductor
    {
        private float susceptance0;
        private float susceptance;
        private float conductance0;
        private float conductance;
        private float resistance0;
        private float resistance;
        private float reactance0;
        private float reactance;
        private long perLengthImpedance = 0;
        public ACLineSegment(long globalId) : base(globalId)
        {
        }

        public float Susceptance0 { get => susceptance0; set => susceptance0 = value; }
        public float Susceptance { get => susceptance; set => susceptance = value; }
        public float Conductance0 { get => conductance0; set => conductance0 = value; }
        public float Conductance { get => conductance; set => conductance = value; }
        public float Resistance0 { get => resistance0; set => resistance0 = value; }
        public float Resistance { get => resistance; set => resistance = value; }
        public float Reactance0 { get => reactance0; set => reactance0 = value; }
        public float Reactance { get => reactance; set => reactance = value; }
        public long PerLengthImpedance { get => perLengthImpedance; set => perLengthImpedance = value; }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            base.AddReference(referenceId, globalId);
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                ACLineSegment x = (ACLineSegment)obj;
                return ((x.susceptance0==this.susceptance0)&&
                        (x.susceptance == this.susceptance) &&
                        (x.conductance0 == this.conductance0) &&
                        (x.conductance == this.conductance) &&
                        (x.resistance0 == this.resistance0) &&
                        (x.resistance == this.resistance) &&
                        (x.reactance0 == this.reactance0) &&
                        (x.reactance == this.reactance) &&
                        (x.perLengthImpedance == this.perLengthImpedance)
                    );
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
            switch (prop.Id)
            {
                case ModelCode.ACLINESEGMENT_CONDUCTANCE0:
                    prop.SetValue(conductance0);
                    break;

                case ModelCode.ACLINESEGMENT_CONDUCTANCE:
                    prop.SetValue(conductance);
                    break;

                case ModelCode.ACLINESEGMENT_REACTANCE:
                    prop.SetValue(reactance);
                    break;

                case ModelCode.ACLINESEGMENT_REACTANCE0:
                    prop.SetValue(reactance0);
                    break;

                case ModelCode.ACLINESEGMENT_RESISTANCE0:
                    prop.SetValue(resistance0);
                    break;
                case ModelCode.ACLINESEGMENT_RESISTANCE:
                    prop.SetValue(resistance);
                    break;
                case ModelCode.ACLINESEGMENT_SUSCEPTANCE0:
                    prop.SetValue(susceptance0);
                    break;
                case ModelCode.ACLINESEGMENT_SUSCPETANCE:
                    prop.SetValue(susceptance);
                    break;
                case ModelCode.ACLINESEGMENT_PERLENGTHIMPEDANCE:
                    prop.SetValue(perLengthImpedance);
                    break;
                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (perLengthImpedance != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.ACLINESEGMENT_PERLENGTHIMPEDANCE] = new List<long>();
                references[ModelCode.ACLINESEGMENT_PERLENGTHIMPEDANCE].Add(perLengthImpedance);
            }

            base.GetReferences(references, refType);
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.ACLINESEGMENT_CONDUCTANCE:
                case ModelCode.ACLINESEGMENT_CONDUCTANCE0:
                case ModelCode.ACLINESEGMENT_REACTANCE:
                case ModelCode.ACLINESEGMENT_REACTANCE0:
                case ModelCode.ACLINESEGMENT_RESISTANCE0:
                case ModelCode.ACLINESEGMENT_RESISTANCE:
                case ModelCode.ACLINESEGMENT_SUSCPETANCE:
                case ModelCode.ACLINESEGMENT_SUSCEPTANCE0:
                case ModelCode.ACLINESEGMENT_PERLENGTHIMPEDANCE:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.ACLINESEGMENT_CONDUCTANCE:
                    conductance = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_CONDUCTANCE0:
                    conductance0 = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_REACTANCE:
                    reactance = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_REACTANCE0:
                    reactance0 = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_RESISTANCE:
                    resistance = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_RESISTANCE0:
                    resistance0 = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_SUSCPETANCE:
                    susceptance = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_SUSCEPTANCE0:
                    susceptance0 = property.AsFloat();
                    break;
                case ModelCode.ACLINESEGMENT_PERLENGTHIMPEDANCE:
                    perLengthImpedance = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}
