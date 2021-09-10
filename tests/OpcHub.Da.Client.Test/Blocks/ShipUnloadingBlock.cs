using OpcHub.Da.Client.Attributes;
using SU = OpcHub.Da.Client.Test.Blocks.OpcBlockConstants.ShipUnloading;

namespace OpcHub.Da.Client.Test.Blocks
{
    //[OpcBlock(SU.SCHEMA, SU.BLOCK_UNLOAD)]
    public class ShipUnloadingActivityDownloadBlock
    {
        [OpcItem(SU.Items.ERROR_CODE)]
        public int ErrorCode { get; set; }

        [OpcItem(SU.Items.ORDER_NO)]
        public string OrderNo { get; set; }

        [OpcItem(SU.Items.ACTIVITY_NO)]
        public string ActivityNo { get; set; }

        [OpcItem(SU.Items.ACTIVITY_TARGET_QTY)]
        public decimal ActivityTargetQty { get; set; }

        [OpcItem(SU.Items.PRODUCT)]
        public string Product { get; set; }

        [OpcItem(SU.Items.SHIP)]
        public string Ship { get; set; }

        [OpcItem(SU.Items.JETTY)]
        public string Jetty { get; set; }

        [OpcItem(SU.Items.ADDITIVE)]
        public string Additive { get; set; }

        [OpcItem(SU.Items.ADDITIVE_TARGET_QTY)]
        public int AdditiveTargetQty { get; set; }

        [OpcItem(SU.Items.DESTINATION_TANKS)]
        public string[] Tanks { get; set; }

        [OpcItem(SU.Items.TANK_STOP_CONDITION_TYPE)]
        public int TankStopConditionType { get; set; }

        [OpcItem(SU.Items.TANK_STOP_CONDITION_VALUE)]
        public decimal TankStopConditionValue { get; set; }
    }

    //[OpcBlock(SU.SCHEMA, SU.BLOCK_UNLOAD)]
    public class ShipUnloadingEventReadBlock
    {
        [OpcItem(SU.Items.ORDER_NO)]
        public string OrderNo { get; set; }

        [OpcItem(SU.Items.ACTIVITY_NO)]
        public string ActivityNo { get; set; }
    }

    //[OpcBlock(SU.SCHEMA, SU.BLOCK_UNLOAD)]
    public class ShipUnloadingCommandBlock
    {
        public ShipUnloadingCommandBlock(int val)
        {
            MSResponseFlag = val;
        }

        [OpcItem(SU.Items.MS_RESPONSE_FLAG)]
        public int MSResponseFlag { get; }
    }

    //[OpcBlock(SU.SCHEMA, SU.BLOCK_UNLOAD)]
    public class ShipUnloadingOperationReadBlock
    {
        [OpcItem(SU.Items.ACTIVITY_ACTUAL_START_TIME)]
        public string ActivityActualStartTime { get; set; }

        [OpcItem(SU.Items.ACTIVITY_ACTUAL_STOP_TIME)]
        public string ActivityActualStopTime { get; set; }

        [OpcItem(SU.Items.TOTAL_RECEIVED_QTY_BY_METER)]
        public decimal TotalReceivedQtyByMeter { get; set; }

        [OpcItem(SU.Items.TOTAL_RECEIVED_QTY_BY_TANK)]
        public decimal TotalReceivedQtyByTank { get; set; }

        [OpcItem(SU.Items.ACTUAL_RECEIVED_QTY_OF_TANKS)]
        public decimal[] ActualReceivedQtyOfTanks { get; set; }

        [OpcItem(SU.Items.ACTUAL_ADDITIVE_INJECTED_QTY)]
        public decimal ActualAdditiveInjectedQty { get; set; }

        [OpcItem(SU.Items.METER_START_LIQUID_MASS)]
        public decimal MeterStartLiquidMass { get; set; }

        [OpcItem(SU.Items.METER_START_VAPOR_MASS)]
        public decimal MeterStartVaporMass { get; set; }

        [OpcItem(SU.Items.METER_STOP_LIQUID_MASS)]
        public decimal MeterStopLiquidMass { get; set; }

        [OpcItem(SU.Items.METER_STOP_VAPOR_MASS)]
        public decimal MeterStopVaporMass { get; set; }

        [OpcItem(SU.Items.OPERATION_STATUS_OF_TANKS)]
        public int[] OperationStatusOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_TIME_OF_TANKS)]
        public string[] OpenTimeOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_DENSITY_OF_TANKS)]
        public decimal[] OpenDensityOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_TEMPERATURE_OF_TANKS)]
        public decimal[] OpenTemperatureOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_VAPOR_TEMPERATURE_OF_TANKS)]
        public decimal[] OpenVaporTemperatureOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_LEVEL_OF_TANKS)]
        public decimal[] OpenLevelOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_TOV_OF_TANKS)]
        public decimal[] OpenTOVOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_GOV_OF_TANKS)]
        public decimal[] OpenGOVOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_GSV_OF_TANKS)]
        public decimal[] OpenGSVOfTanks { get; set; }

        [OpcItem(SU.Items.OPEN_MASS_OF_TANKS)]
        public decimal[] OpenMassOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_TIME_OF_TANKS)]
        public string[] StopTimeOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_DENSITY_OF_TANKS)]
        public decimal[] StopDensityOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_TEMPERATURE_OF_TANKS)]
        public decimal[] StopTemperatureOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_VAPOR_TEMPERATURE_OF_TANKS)]
        public decimal[] StopVaporTemperatureOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_LEVEL_OF_TANKS)]
        public decimal[] StopLevelOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_TOV_OF_TANKS)]
        public decimal[] StopTOVOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_GOV_OF_TANKS)]
        public decimal[] StopGOVOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_GSV_OF_TANKS)]
        public decimal[] StopGSVOfTanks { get; set; }

        [OpcItem(SU.Items.STOP_MASS_OF_TANKS)]
        public decimal[] StopMassOfTanks { get; set; }

        [OpcItem(SU.Items.JOB_STATUS)]
        public int JobStatus { get; set; }
    }
}