using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{	
	public class Terminal : IdentifiedObject
	{
		

		private long conductingEquipment=0;

        public Terminal(long globalId) : base(globalId)
        {
        }

        public long ConductingEquipment { get => conductingEquipment; set => conductingEquipment = value; }

		
		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
			{
				Terminal x = (Terminal)obj;
				return ((x.conductingEquipment==this.conductingEquipment));
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

		#region IAccess implementation

		public override bool HasProperty(ModelCode t)
		{
			switch (t)
			{
				
				case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
					return true;
				default:

					return base.HasProperty(t);
			}
		}

		public override void GetProperty(Property prop)
		{
			switch (prop.Id)
			{
				case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
					prop.SetValue(conductingEquipment);
					break;
				default:
					base.GetProperty(prop);
					break;
			}
		}

		public override void SetProperty(Property property)
		{

            switch (property.Id)
            {
                case ModelCode.TERMINAL_CONDUCTINGEQUIPMENT:
                    conductingEquipment = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }


        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (conductingEquipment != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.TERMINAL_CONDUCTINGEQUIPMENT] = new List<long>();
                references[ModelCode.TERMINAL_CONDUCTINGEQUIPMENT].Add(conductingEquipment);
            }

            base.GetReferences(references, refType);
        }

        #endregion IAccess implementation	


    }
}
