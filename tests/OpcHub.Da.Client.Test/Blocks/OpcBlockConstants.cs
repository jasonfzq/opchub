using System.Collections.Generic;

namespace OpcHub.Da.Client.Test.Blocks
{
    public static class OpcBlockConstants
    {
        static OpcBlockConstants()
        {
            BlockNames = new List<string>
            {
                Gate.BLOCK_IN_GATE,
                Gate.BLOCK_OUT_GATE,
                Bay.BLOCK_BAY_01,
                Bay.BLOCK_BAY_02,
                Bay.BLOCK_BAY_03,
                ShipUnloading.BLOCK_UNLOAD
            };
        }
        
        public static IReadOnlyList<string> BlockNames { get; }

        public static class Gate
        {
            public const string SCHEMA = "Gate";
            public const string BLOCK_IN_GATE = "GATEIN01";
            public const string BLOCK_OUT_GATE = "GATEOUT01";

            //public static readonly string COMMAND_TEMPLATE = $"{0}.{Items.MS_RESPONSE_FLAG}";
            
            public static class Items
            {
                public const string TRUCK_CARD_ID = "TruckCardID";
                public const string DRIVER_CARD_ID = "DriverCardID";
                public const string MS_RESPONSE_FLAG = "MSResponseFlag";
                public const string ERROR_CODE = "ErrorCode";
                public const string ORDER_NO = "OrderNo";
                public const string PRODUCT = "Product";
                public const string TRUCK_LICENSE = "TruckLicense";
                public const string BAY_NO = "BayNo";
            }
        }


        public class WeightBridge
        {
            public const string SCHEMA = "WeightBridge";

            public static class Items
            {
                public const string TRUCK_CARD_ID = "TruckCardID";
                public const string DRIVER_CARD_ID = "DriverCardID";
                public const string MS_RESPONSE_FLAG = "MSResponseFlag";
                public const string ERROR_CODE = "ErrorCode";
                public const string WEIGHT = "Weight";
                public const string ORDER_NO = "OrderNo";
                public const string PRODUCT = "Product";
                public const string TRUCK_LICENSE = "TruckLicense";
                public const string BAY_NO = "BayNo";
                public const string WEIGHING_TYPE = "WeighingType";
            }
        }

        public static class Bay
        {
            public const string SCHEMA = "Bay";
            public const string BLOCK_BAY_01 = "TRK_LOAD";
            public const string BLOCK_BAY_02 = "TRK_LOAD";
            public const string BLOCK_BAY_03 = "TRK_LOAD";
            
            //public static readonly string COMMAND_TEMPLATE = $"{0}.{Items.MS_RESPONSE_FLAG}";

            public static class Items
            {
                public const string TRUCK_CARD_ID = "TruckCardID";
                public const string DRIVER_CARD_ID = "DriverCardID";
                public const string MS_RESPONSE_FLAG = "MSResponseFlag";
                public const string ERROR_CODE = "ErrorCode";
                public const string ORDER_NO = "OrderNo";
                public const string ACTIVITY_NO = "ActivityNo";
                public const string PRODUCT = "Product";
                public const string TRUCK_LICENSE = "TruckLicense";
                public const string DRIVER_NAME = "DriverName";
                public const string TRANSPORTER = "Transporter";
                public const string TARGET_QUANTITY = "TargetQuantity";
                public const string LOADED_QUANTITY = "LoadedQuantity";
                public const string METER_START_LIQUID_MASS = "MeterStartLiquidMass";
                public const string METER_START_VAPOR_MASS = "MeterStartVaporMass";
                public const string METER_STOP_LIQUID_MASS = "MeterStopLiquidMass";
                public const string METER_STOP_VAPOR_MASS = "MeterStopVaporMass";
                public const string START_TIME = "StartTime";
                public const string END_TIME = "EndTime";
                public const string EXPECTED_END_TIME = "ExpectedEndTime";
                public const string LOADING_RATE = "LoadingRate";
                public const string JOB_STATUS = "JobStatus";
            }
        }

        public static class ShipUnloading
        {
            public const string SCHEMA = "ShipUnloading";
            public const string BLOCK_UNLOAD = "SHIP_UNLOAD";

            public static class Items
            {
                public const string MS_RESPONSE_FLAG = "MSResponseFlag";
                public const string ERROR_CODE = "ErrorCode";
                public const string ORDER_NO = "OrderNo";
                public const string ACTIVITY_NO = "ActivityNo";
                public const string ACTIVITY_TARGET_QTY = "ActivityTargetQty";
                public const string PRODUCT = "Product";
                public const string SHIP = "Ship";
                public const string JETTY = "Jetty";
                public const string ADDITIVE = "Additive";
                public const string ADDITIVE_TARGET_QTY = "AdditiveTargetQty";
                public const string DESTINATION_TANKS = "DestinationTanks";
                public const string TANK_STOP_CONDITION_TYPE = "TankStopConditionType";
                public const string TANK_STOP_CONDITION_VALUE = "TankStopConditionValue";
                public const string ACTIVITY_ACTUAL_START_TIME = "ActivityActualStartTime";
                public const string ACTIVITY_ACTUAL_STOP_TIME = "ActivityActualStopTime";
                public const string TOTAL_RECEIVED_QTY_BY_METER = "TotalReceivedQtyByMeter";
                public const string TOTAL_RECEIVED_QTY_BY_TANK = "TotalReceivedQtyByTank";
                public const string ACTUAL_RECEIVED_QTY_OF_TANKS = "ActualReceivedQtyOfTanks";
                public const string ACTUAL_ADDITIVE_INJECTED_QTY = "ActualAdditiveInjectedQty";
                public const string METER_START_LIQUID_MASS = "MeterStartLiquidMass";
                public const string METER_START_VAPOR_MASS = "MeterStartVaporMass";
                public const string METER_STOP_LIQUID_MASS = "MeterStopLiquidMass";
                public const string METER_STOP_VAPOR_MASS = "MeterStopVaporMass";
                public const string OPERATION_STATUS_OF_TANKS = "OperationStatusOfTanks";
                public const string OPEN_TIME_OF_TANKS = "OpenTimeOfTanks";
                public const string OPEN_DENSITY_OF_TANKS = "OpenDensityOfTanks";
                public const string OPEN_TEMPERATURE_OF_TANKS = "OpenTemperatureOfTanks";
                public const string OPEN_VAPOR_TEMPERATURE_OF_TANKS = "OpenVaporTemperatureOfTanks";
                public const string OPEN_LEVEL_OF_TANKS = "OpenLevelOfTanks";
                public const string OPEN_TOV_OF_TANKS = "OpenTOVOfTanks";
                public const string OPEN_GOV_OF_TANKS = "OpenGOVOfTanks";
                public const string OPEN_GSV_OF_TANKS = "OpenGSVOfTanks";
                public const string OPEN_MASS_OF_TANKS = "OpenMassOfTanks";
                public const string STOP_TIME_OF_TANKS = "StopTimeOfTanks";
                public const string STOP_DENSITY_OF_TANKS = "StopDensityOfTanks";
                public const string STOP_TEMPERATURE_OF_TANKS = "StopTemperatureOfTanks";
                public const string STOP_VAPOR_TEMPERATURE_OF_TANKS = "StopVaporTemperatureOfTanks";
                public const string STOP_LEVEL_OF_TANKS = "StopLevelOfTanks";
                public const string STOP_TOV_OF_TANKS = "StopTOVOfTanks";
                public const string STOP_GOV_OF_TANKS = "StopGOVOfTanks";
                public const string STOP_GSV_OF_TANKS = "StopGSVOfTanks";
                public const string STOP_MASS_OF_TANKS = "StopMassOfTanks";
                public const string JOB_STATUS = "JobStatus";
            }
        }
        
        public class Tank
        {
            public const string SCHEMA = "TGS";

            public static class Items
            {
                public const string LIQUID_LEVEL = "LiquidLevel";
                public const string LIQUID_OBSERVED_DENSITY = "LiquidObservedDensity";
                public const string LIQUID_REFERENCE_DENSITY = "LiquidReferenceDensity";
                public const string LIQUID_TEMPERATURE = "LiquidTemperature";
                public const string LIQUID_TOV = "LiquidTOV";
                public const string LIQUID_GOV = "LiquidGOV";
                public const string LIQUID_GSV = "LiquidGSV";
                public const string LIQUID_NSV = "LiquidNSV";
                public const string LIQUID_MASS = "LiquidMass";
                public const string WATER_LEVEL = "WaterLevel";
                public const string WATER_VOLUME = "WaterVolume";
                public const string VAPOUR_TEMPERATURE = "VapourTemperature";
                public const string VAPOUR_PRESSURE = "VapourPressure";
                public const string VAPOUR_MASS = "VapourMass";
                public const string TOTAL_MASS = "TotalMass";
            }
        }
    }
}