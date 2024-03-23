using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	/// <summary>
	/// PowerTransformerImporter
	/// </summary>
	public class PowerTransformerImporter
	{
		/// <summary> Singleton </summary>
		private static PowerTransformerImporter ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static PowerTransformerImporter Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new PowerTransformerImporter();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            
            ImportDCLineSegments();
            ImportPerLengthPhaseImpedances();
            ImportPerLengthSequenceImpedances();
            ImportPhaseImpedanceDatas();
            ImportACLineSegments();
            ImportTerminals();
            
            ImportSeriesCompensators();
            

            LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import
		private void ImportPhaseImpedanceDatas()
		{
			SortedDictionary<string, object> cimPhaseImpedanceDatas = concreteModel.GetAllObjectsOfType("FTN.PhaseImpedanceData");
			if (cimPhaseImpedanceDatas != null)
			{
				foreach (KeyValuePair<string, object> cimPhaseImpedanceDataPair in cimPhaseImpedanceDatas)
				{
					FTN.PhaseImpedanceData cimPhaseImpedanceData = cimPhaseImpedanceDataPair.Value as FTN.PhaseImpedanceData;

					ResourceDescription rd = CreatePhaseImpedanceDataResourceDescription(cimPhaseImpedanceData);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("PhaseImpedance Data ID = ").Append(cimPhaseImpedanceData.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("PhaseImpedance ID = ").Append(cimPhaseImpedanceData.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePhaseImpedanceDataResourceDescription(FTN.PhaseImpedanceData cimPhaseImpedanceData)
		{
			ResourceDescription rd = null;
			if (cimPhaseImpedanceData != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PHID, importHelper.CheckOutIndexForDMSType(DMSType.PHID));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimPhaseImpedanceData.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulatePhaseImpedanceDataProperties(cimPhaseImpedanceData, rd, importHelper, report);
			}
			return rd;
		}
		
		private void ImportPerLengthPhaseImpedances()
		{
			SortedDictionary<string, object> cimPLPIS = concreteModel.GetAllObjectsOfType("FTN.PerLengthPhaseImpedance");
			if (cimPLPIS != null)
			{
				foreach (KeyValuePair<string, object> cimPLPIPair in cimPLPIS)
				{
					FTN.PerLengthPhaseImpedance cimPLPI = cimPLPIPair.Value as FTN.PerLengthPhaseImpedance;

					ResourceDescription rd = CreatePerLengthPhaseImpedanceDescription(cimPLPI);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("PerLengthPhaseImpedance ID = ").Append(cimPLPI.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("PerLengthPhaseImpedance ID = ").Append(cimPLPI.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePerLengthPhaseImpedanceDescription(FTN.PerLengthPhaseImpedance cimPLPI)
		{
			ResourceDescription rd = null;
			if (cimPLPI != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PLPI, importHelper.CheckOutIndexForDMSType(DMSType.PLPI));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimPLPI.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulatePerLengthPhaseImpedanceProperties(cimPLPI, rd);
			}
			return rd;
		}

		private void ImportPerLengthSequenceImpedances()
		{
			SortedDictionary<string, object> cimPLSIS = concreteModel.GetAllObjectsOfType("FTN.PerLengthSequenceImpedances");
			if (cimPLSIS != null)
			{
				foreach (KeyValuePair<string, object> cimPLSIPair in cimPLSIS)
				{
					FTN.PerLengthSequenceImpedance cimPLSI = cimPLSIPair.Value as FTN.PerLengthSequenceImpedance;

					ResourceDescription rd = CreatePerLengthSequenceImpedanceDescription(cimPLSI);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("PerLengthSequenceImpedance ID = ").Append(cimPLSI.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("PerLengthSequenceImpedance ID = ").Append(cimPLSI.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreatePerLengthSequenceImpedanceDescription(FTN.PerLengthSequenceImpedance cimPerLengthSequenceImpedance)
		{
			ResourceDescription rd = null;
			if (cimPerLengthSequenceImpedance != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PLSI, importHelper.CheckOutIndexForDMSType(DMSType.PLSI));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimPerLengthSequenceImpedance.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulatePerLengthSequenceImpedanceProperties(cimPerLengthSequenceImpedance, rd);
			}
			return rd;
		}

		private void ImportTerminals()
		{
			SortedDictionary<string, object> cimTerminals = concreteModel.GetAllObjectsOfType("FTN.Terminal");
			if (cimTerminals != null)
			{
				foreach (KeyValuePair<string, object> cimTerminalPair in cimTerminals)
				{
					FTN.Terminal cimTerminal = cimTerminalPair.Value as FTN.Terminal;

					ResourceDescription rd = CreateTerminalDescription(cimTerminal);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("Terminal ID = ").Append(cimTerminal.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateTerminalDescription(FTN.Terminal cimTerminal)
		{
			ResourceDescription rd = null;
			if (cimTerminal != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.TERMINAL, importHelper.CheckOutIndexForDMSType(DMSType.TERMINAL));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimTerminal.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateTerminalProperties(cimTerminal, rd, importHelper, report);
			}
			return rd;
		}

		private void ImportACLineSegments()
		{
			SortedDictionary<string, object> cimACLineSegments = concreteModel.GetAllObjectsOfType("FTN.ACLineSegment");
			if (cimACLineSegments != null)
			{
				foreach (KeyValuePair<string, object> cimACLineSegmentPair in cimACLineSegments)
				{
					FTN.ACLineSegment cimACLinesSegment = cimACLineSegmentPair.Value as FTN.ACLineSegment;

					ResourceDescription rd = CreateACLineSegmentDescription(cimACLinesSegment);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("ACLinesSegment ID = ").Append(cimACLinesSegment.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("ACLinesSegment ID = ").Append(cimACLinesSegment.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateACLineSegmentDescription(FTN.ACLineSegment cimACLineSegment)
		{
			ResourceDescription rd = null;
			if (cimACLineSegment != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.ACLINESEGMENT, importHelper.CheckOutIndexForDMSType(DMSType.ACLINESEGMENT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimACLineSegment.ID, gid);

				////populate ResourceDescription
				PowerTransformerConverter.PopulateACLinesSegmentProperties(cimACLineSegment, rd, importHelper, report);
			}
			return rd;
		}

        private void ImportDCLineSegments()
        {
            SortedDictionary<string, object> cimDCLineSegments = concreteModel.GetAllObjectsOfType("FTN.DCLineSegment");
            if (cimDCLineSegments != null)
            {
                foreach (KeyValuePair<string, object> cimDCLineSegmentPair in cimDCLineSegments)
                {
                    FTN.DCLineSegment cimDCLinesSegment = cimDCLineSegmentPair.Value as FTN.DCLineSegment;

                    ResourceDescription rd = CreateDCLineSegmentDescription(cimDCLinesSegment);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("DCLinesSegment ID = ").Append(cimDCLinesSegment.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("DCLinesSegment ID = ").Append(cimDCLinesSegment.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateDCLineSegmentDescription(FTN.DCLineSegment cimDCLineSegment)
        {
            ResourceDescription rd = null;
            if (cimDCLineSegment != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DCLINESEGMENT, importHelper.CheckOutIndexForDMSType(DMSType.DCLINESEGMENT));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimDCLineSegment.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateDCLineSegmentProperties(cimDCLineSegment, rd);
            }
            return rd;
        }

        private void ImportSeriesCompensators()
        {
            SortedDictionary<string, object> cimSeriesCompensators = concreteModel.GetAllObjectsOfType("FTN.SeriesCompensator");
            if (cimSeriesCompensators != null)
            {
                foreach (KeyValuePair<string, object> cimSeriesCompensatorPair in cimSeriesCompensators)
                {
                    FTN.SeriesCompensator cimSeriesCompensator = cimSeriesCompensatorPair.Value as FTN.SeriesCompensator;

                    ResourceDescription rd = CreateSeriesCompensatorDescription(cimSeriesCompensator);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("SeriesCompensator ID = ").Append(cimSeriesCompensator.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("SeriesCompensator ID = ").Append(cimSeriesCompensator.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }
        private ResourceDescription CreateSeriesCompensatorDescription(FTN.SeriesCompensator cimSeriesCompensator)
        {
            ResourceDescription rd = null;
            if (cimSeriesCompensator != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SERIESCOMPENSATOR, importHelper.CheckOutIndexForDMSType(DMSType.SERIESCOMPENSATOR));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSeriesCompensator.ID, gid);

                ////populate ResourceDescription
                PowerTransformerConverter.PopulateSeriesCompensatorProperties(cimSeriesCompensator, rd);
            }
            return rd;
        }
        #endregion Import
    }
}

