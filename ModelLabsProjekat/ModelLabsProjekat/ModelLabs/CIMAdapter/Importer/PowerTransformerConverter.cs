namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	using FTN.Common;

	/// <summary>
	/// PowerTransformerConverter has methods for populating
	/// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
	/// </summary>
	public static class PowerTransformerConverter
	{

		#region Populate ResourceDescription
		public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
		{
			if ((cimIdentifiedObject != null) && (rd != null))
			{
				if (cimIdentifiedObject.MRIDHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
				}
				if (cimIdentifiedObject.NameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
				}
				if (cimIdentifiedObject.AliasNameHasValue)
				{
					rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
				}
               
            }
		}

		public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cimPowerSystemResource, ResourceDescription rd)
		{
			if ((cimPowerSystemResource != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPowerSystemResource, rd);

				
			}
		}


		public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd);

			}
		}

		public static void PopulateConductingEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd)
		{
			if ((cimEquipment != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateEquipmentProperties(cimEquipment, rd);

				
			}
		}


		public static void PopulateSeriesCompensatorProperties(FTN.SeriesCompensator cimSeriesCompensator, ResourceDescription rd)
		{
			if ((cimSeriesCompensator != null) && (rd != null))
			{
				PowerTransformerConverter.PopulateConductingEquipmentProperties(cimSeriesCompensator, rd);

				if (cimSeriesCompensator.RHasValue)
				{
					rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_REACTANCE, cimSeriesCompensator.R));
				}
                if (cimSeriesCompensator.R0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_REACTANCE0, cimSeriesCompensator.R0));
                }
                if (cimSeriesCompensator.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_RESISTANCE, cimSeriesCompensator.X));
                }
                if (cimSeriesCompensator.X0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SERIESCOMPENSATOR_RESISTANCE0, cimSeriesCompensator.X0));
                }
            }
		}

        public static void PopulateConductorProperties(FTN.Conductor cimConductor, ResourceDescription rd)
        {
            if ((cimConductor != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateConductingEquipmentProperties(cimConductor, rd);
                if (cimConductor.LengthHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.CONDUCTOR_LENGTH, cimConductor.Length));
                }

            }
        }
        public static void PopulateDCLineSegmentProperties(FTN.DCLineSegment cimDCLineSegment, ResourceDescription rd)
        {
            if ((cimDCLineSegment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateConductorProperties(cimDCLineSegment, rd);
                

            }
        }
        public static void PopulateTerminalProperties(FTN.Terminal cimTerminal, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimTerminal != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimTerminal, rd);

                if (cimTerminal.ConductingEquipmentHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimTerminal.ConductingEquipment.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimTerminal.GetType().ToString()).Append(" rdfID = \"").Append(cimTerminal.ID);
                        report.Report.Append("\" - Failed to set reference to Conducting Equipment: rdfID \"").Append(cimTerminal.ConductingEquipment.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.TERMINAL_CONDUCTINGEQUIPMENT, gid));
                }

            }
        }
        public static void PopulatePerLengthImpedanceProperties(FTN.PerLengthImpedance cimPerLengthImpedance, ResourceDescription rd)
        {
            if ((cimPerLengthImpedance != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPerLengthImpedance, rd);


            }
        }
        public static void PopulatePerLengthSequenceImpedanceProperties(FTN.PerLengthSequenceImpedance cimPerLengthSequenceImpedance, ResourceDescription rd)
        {
            if ((cimPerLengthSequenceImpedance != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPerLengthSequenceImpedance, rd);

                if (cimPerLengthSequenceImpedance.B0chHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_SUSCEPTANCEPERLENGTH0, cimPerLengthSequenceImpedance.B0ch));
                }
                if (cimPerLengthSequenceImpedance.BchHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_SUSCEPTANCEPERLENGTH1, cimPerLengthSequenceImpedance.Bch));
                }
                if (cimPerLengthSequenceImpedance.R0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_RESISTANCEPERLENGTH0, cimPerLengthSequenceImpedance.R0));
                }
                if (cimPerLengthSequenceImpedance.RHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_RESISTANCEPERLENGTH1, cimPerLengthSequenceImpedance.R));
                }
                if (cimPerLengthSequenceImpedance.G0chHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_CONDUCTANCEPERLENGTH0, cimPerLengthSequenceImpedance.G0ch));
                }
                if (cimPerLengthSequenceImpedance.GchHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_CONDUCTANCEPERLENGTH1, cimPerLengthSequenceImpedance.Gch));
                }
                if (cimPerLengthSequenceImpedance.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_REACTANCEPERLENGTH0, cimPerLengthSequenceImpedance.X));
                }
                if (cimPerLengthSequenceImpedance.X0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLSI_RESISTANCEPERLENGTH1, cimPerLengthSequenceImpedance.X0));
                }
            }
        }
        public static void PopulatePerLengthPhaseImpedanceProperties(FTN.PerLengthPhaseImpedance cimPerLengthPhaseImpedance, ResourceDescription rd)
        {
            if ((cimPerLengthPhaseImpedance != null) && (rd != null))
            {
                PowerTransformerConverter.PopulatePerLengthImpedanceProperties(cimPerLengthPhaseImpedance, rd);
                if (cimPerLengthPhaseImpedance.ConductorCountHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PLPI_CONDUCTORCOUNT, cimPerLengthPhaseImpedance.ConductorCount));
                }

            }
        }
        public static void PopulatePhaseImpedanceDataProperties(FTN.PhaseImpedanceData cimPhaseImpedanceData, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimPhaseImpedanceData != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimPhaseImpedanceData, rd);
                if (cimPhaseImpedanceData.BHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PHID_SUSCEPTANCEPERLENGTH, cimPhaseImpedanceData.B));
                }
                if (cimPhaseImpedanceData.RHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PHID_RESISTANCEPERLENGTH, cimPhaseImpedanceData.R));
                }
                if (cimPhaseImpedanceData.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PHID_REACTANCEPERLENGTH, cimPhaseImpedanceData.X));
                }
                if (cimPhaseImpedanceData.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.PHID_SEQUENCENUMBER, cimPhaseImpedanceData.SequenceNumber));
                }
                if (cimPhaseImpedanceData.PhaseImpedanceHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimPhaseImpedanceData.PhaseImpedance.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimPhaseImpedanceData.GetType().ToString()).Append(" rdfID = \"").Append(cimPhaseImpedanceData.ID);
                        report.Report.Append("\" - Failed to set reference to Phase Impedance: rdfID \"").Append(cimPhaseImpedanceData.PhaseImpedance.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.PHID_PHASEIMPEDANCE, gid));
                }

            }
        }
        public static void PopulateACLinesSegmentProperties(FTN.ACLineSegment cimACLineSegment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cimACLineSegment != null) && (rd != null))
            {
                PowerTransformerConverter.PopulateIdentifiedObjectProperties(cimACLineSegment, rd);

                if (cimACLineSegment.B0chHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_SUSCEPTANCE0, cimACLineSegment.B0ch));
                }
                if (cimACLineSegment.BchHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_SUSCPETANCE, cimACLineSegment.Bch));
                }
                if (cimACLineSegment.R0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_RESISTANCE0, cimACLineSegment.R0));
                }
                if (cimACLineSegment.RHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_RESISTANCE, cimACLineSegment.R));
                }
                if (cimACLineSegment.G0chHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_CONDUCTANCE0, cimACLineSegment.G0ch));
                }
                if (cimACLineSegment.GchHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_CONDUCTANCE, cimACLineSegment.Gch));
                }
                if (cimACLineSegment.XHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_REACTANCE, cimACLineSegment.X));
                }
                if (cimACLineSegment.X0HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_REACTANCE0, cimACLineSegment.X0));
                }
                if (cimACLineSegment.PerLengthImpedanceHasValue)
                {
                    long gid = importHelper.GetMappedGID(cimACLineSegment.PerLengthImpedance.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cimACLineSegment.GetType().ToString()).Append(" rdfID = \"").Append(cimACLineSegment.ID);
                        report.Report.Append("\" - Failed to set reference to PerLengthPhaseImpedance: rdfID \"").Append(cimACLineSegment.PerLengthImpedance.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.ACLINESEGMENT_PERLENGTHIMPEDANCE, gid));
                }
            }
        }
        #endregion Populate ResourceDescription

        #region Enums convert
        //public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
        //{
        //	switch (phases)
        //	{
        //		case FTN.PhaseCode.A:
        //			return PhaseCode.A;
        //		case FTN.PhaseCode.AB:
        //			return PhaseCode.AB;
        //		case FTN.PhaseCode.ABC:
        //			return PhaseCode.ABC;
        //		case FTN.PhaseCode.ABCN:
        //			return PhaseCode.ABCN;
        //		case FTN.PhaseCode.ABN:
        //			return PhaseCode.ABN;
        //		case FTN.PhaseCode.AC:
        //			return PhaseCode.AC;
        //		case FTN.PhaseCode.ACN:
        //			return PhaseCode.ACN;
        //		case FTN.PhaseCode.AN:
        //			return PhaseCode.AN;
        //		case FTN.PhaseCode.B:
        //			return PhaseCode.B;
        //		case FTN.PhaseCode.BC:
        //			return PhaseCode.BC;
        //		case FTN.PhaseCode.BCN:
        //			return PhaseCode.BCN;
        //		case FTN.PhaseCode.BN:
        //			return PhaseCode.BN;
        //		case FTN.PhaseCode.C:
        //			return PhaseCode.C;
        //		case FTN.PhaseCode.CN:
        //			return PhaseCode.CN;
        //		case FTN.PhaseCode.N:
        //			return PhaseCode.N;
        //		case FTN.PhaseCode.s12N:
        //			return PhaseCode.ABN;
        //		case FTN.PhaseCode.s1N:
        //			return PhaseCode.AN;
        //		case FTN.PhaseCode.s2N:
        //			return PhaseCode.BN;
        //		default: return PhaseCode.Unknown;
        //	}
        //}

        //public static TransformerFunction GetDMSTransformerFunctionKind(FTN.TransformerFunctionKind transformerFunction)
        //{
        //	switch (transformerFunction)
        //	{
        //		case FTN.TransformerFunctionKind.voltageRegulator:
        //			return TransformerFunction.Voltreg;
        //		default:
        //			return TransformerFunction.Consumer;
        //	}
        //}

        //public static WindingType GetDMSWindingType(FTN.WindingType windingType)
        //{
        //	switch (windingType)
        //	{
        //		case FTN.WindingType.primary:
        //			return WindingType.Primary;
        //		case FTN.WindingType.secondary:
        //			return WindingType.Secondary;
        //		case FTN.WindingType.tertiary:
        //			return WindingType.Tertiary;
        //		default:
        //			return WindingType.None;
        //	}
        //}

        //public static WindingConnection GetDMSWindingConnection(FTN.WindingConnection windingConnection)
        //{
        //	switch (windingConnection)
        //	{
        //		case FTN.WindingConnection.D:
        //			return WindingConnection.D;
        //		case FTN.WindingConnection.I:
        //			return WindingConnection.I;
        //		case FTN.WindingConnection.Z:
        //			return WindingConnection.Z;
        //		case FTN.WindingConnection.Y:
        //			return WindingConnection.Y;
        //		default:
        //			return WindingConnection.Y;
        //	}
        //}
        #endregion Enums convert
    }
}
