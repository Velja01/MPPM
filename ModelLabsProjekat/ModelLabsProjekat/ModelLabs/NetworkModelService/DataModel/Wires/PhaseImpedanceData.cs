using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PhaseImpedanceData:IdentifiedObject
    {
        private float suscpetancePerLength;
        private float resistancePerLength;
        private int sequenceNumber;
        private float reactancePerLength;
        private long phaseImpedance;

        public PhaseImpedanceData(long globalId) : base(globalId)
        {
        }

        public float SuscpetancePerLength { get => suscpetancePerLength; set => suscpetancePerLength = value; }
        public float ResistancePerLength { get => resistancePerLength; set => resistancePerLength = value; }
        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }
        public float ReactancePerLength { get => reactancePerLength; set => reactancePerLength = value; }
        public long PhaseImpedance { get => phaseImpedance; set => phaseImpedance = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                PhaseImpedanceData x = (PhaseImpedanceData)obj;
                return ((x.suscpetancePerLength==this.suscpetancePerLength)&&
                        (x.resistancePerLength==this.resistancePerLength)&&
                        (x.sequenceNumber==this.sequenceNumber)&&
                        (x.reactancePerLength==this.reactancePerLength)&&
                        (x.phaseImpedance==this.phaseImpedance));
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

        public override void GetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PHID_REACTANCEPERLENGTH:
                    property.SetValue(reactancePerLength);
                    break;
                case ModelCode.PHID_RESISTANCEPERLENGTH:
                    property.SetValue(resistancePerLength);
                    break;
                case ModelCode.PHID_SEQUENCENUMBER:
                    property.SetValue(sequenceNumber);
                    break;
                case ModelCode.PHID_PHASEIMPEDANCE:
                    property.SetValue(phaseImpedance);
                    break;
                case ModelCode.PHID_SUSCEPTANCEPERLENGTH:
                    property.SetValue(suscpetancePerLength);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.PHID_SUSCEPTANCEPERLENGTH:
                case ModelCode.PHID_RESISTANCEPERLENGTH:
                case ModelCode.PHID_SEQUENCENUMBER:
                case ModelCode.PHID_REACTANCEPERLENGTH:
                case ModelCode.PHID_PHASEIMPEDANCE:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.PHID_REACTANCEPERLENGTH:
                    reactancePerLength = property.AsFloat();
                    break;
                case ModelCode.PHID_RESISTANCEPERLENGTH:
                    resistancePerLength = property.AsFloat();
                    break;
                case ModelCode.PHID_SEQUENCENUMBER:
                    sequenceNumber = property.AsInt();
                    break;
                case ModelCode.PHID_SUSCEPTANCEPERLENGTH:
                    suscpetancePerLength = property.AsFloat();
                    break;
                case ModelCode.PHID_PHASEIMPEDANCE:
                    phaseImpedance = property.AsReference();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (phaseImpedance != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.PHID_PHASEIMPEDANCE] = new List<long>();
                references[ModelCode.PHID_PHASEIMPEDANCE].Add(phaseImpedance);
            }

            base.GetReferences(references, refType);
        }
    }
}
