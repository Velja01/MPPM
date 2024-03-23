using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class SeriesCompensator : ConductingEquipment
    {

        private float resistance0;
        private float resistance1;
        private float reactance0;
        private float reactance1;

        public SeriesCompensator(long globalId) : base(globalId)
        {
        }

        public float Resistance0 { get => resistance0; set => resistance0 = value; }
        public float Resistance1 { get => resistance1; set => resistance1 = value; }
        public float Reactance0 { get => reactance0; set => reactance0 = value; }
        public float Reactance1 { get => reactance1; set => reactance1 = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SeriesCompensator x = (SeriesCompensator)obj;
                return ((x.resistance0 == this.resistance0) &&
                        (x.resistance1 == this.resistance1) &&
                        (x.reactance0 == this.reactance0) &&
                        (x.reactance1 == this.reactance1));
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
                case ModelCode.SERIESCOMPENSATOR_REACTANCE0:
                    prop.SetValue(reactance0);
                    break;
                case ModelCode.SERIESCOMPENSATOR_REACTANCE:
                    prop.SetValue(reactance1);
                    break;
                case ModelCode.SERIESCOMPENSATOR_RESISTANCE0:
                    prop.SetValue(resistance0);
                    break;
                case ModelCode.SERIESCOMPENSATOR_RESISTANCE:
                    prop.SetValue(resistance1);
                    break;
                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.SERIESCOMPENSATOR_REACTANCE:
                case ModelCode.SERIESCOMPENSATOR_REACTANCE0:
                case ModelCode.SERIESCOMPENSATOR_RESISTANCE:
                case ModelCode.SERIESCOMPENSATOR_RESISTANCE0:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id) {
                case ModelCode.SERIESCOMPENSATOR_REACTANCE:
                    reactance1 = property.AsFloat();
                    break;
                case ModelCode.SERIESCOMPENSATOR_REACTANCE0:
                    reactance0 = property.AsFloat();
                    break;
                case ModelCode.SERIESCOMPENSATOR_RESISTANCE:
                    resistance1 = property.AsFloat();
                    break;
                case ModelCode.SERIESCOMPENSATOR_RESISTANCE0:
                    reactance0 = property.AsFloat();
                    break;
                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}
