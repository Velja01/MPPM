using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class PerLengthPhaseImpedance:PerLengthImpedance
    {
        private int conductorCount;
        private List<long> phaseImpedanceData= new List<long>();

        public PerLengthPhaseImpedance(long globalId) : base(globalId)
        {
        }

        public List<long> PhaseImpedanceData { get => phaseImpedanceData; set => phaseImpedanceData = value; }
        public int ConductorCount { get => conductorCount; set => conductorCount = value; }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.PLPI_PHASEIMPEDANCEDATA:
                    phaseImpedanceData.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                PerLengthPhaseImpedance x = (PerLengthPhaseImpedance)obj;
                return ((x.conductorCount==this.conductorCount)&&
                        CompareHelper.CompareLists(x.phaseImpedanceData, this.phaseImpedanceData));
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
            switch (property.Id) {
                case ModelCode.PLPI_PHASEIMPEDANCEDATA:
                    property.SetValue(phaseImpedanceData);
                    break;
                case ModelCode.PLPI_CONDUCTORCOUNT:
                    property.SetValue(conductorCount);
                    break;
                default:
                    base.GetProperty(property);
                    break;
            }
        }
        public override bool IsReferenced
        {
            get
            {
                return phaseImpedanceData.Count > 0 || base.IsReferenced;
            }
        }
        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (phaseImpedanceData != null && phaseImpedanceData.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.PLPI_PHASEIMPEDANCEDATA] = phaseImpedanceData.GetRange(0, phaseImpedanceData.Count);
            }

            base.GetReferences(references, refType);
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.PLPI_CONDUCTORCOUNT:
                case ModelCode.PLPI_PHASEIMPEDANCEDATA:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.PLPI_PHASEIMPEDANCEDATA:

                    if (phaseImpedanceData.Contains(globalId))
                    {
                        phaseImpedanceData.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id) {
                case ModelCode.PLPI_CONDUCTORCOUNT:
                    conductorCount=property.AsInt();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}
