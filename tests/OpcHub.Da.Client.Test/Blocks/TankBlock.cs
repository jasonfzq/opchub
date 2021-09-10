using OpcHub.Da.Client.Attributes;
using TGS = OpcHub.Da.Client.Test.Blocks.OpcBlockConstants.Tank;
using WeightBridge = OpcHub.Da.Client.Test.Blocks.OpcBlockConstants.WeightBridge;

namespace OpcHub.Da.Client.Test.Blocks
{
    [OpcBlock(WeightBridge.SCHEMA)]
    public class WeighingReadBlock
    {
        [OpcItem(WeightBridge.Items.TRUCK_CARD_ID)]
        public string TruckCardID { get; set; }

        [OpcItem(WeightBridge.Items.DRIVER_CARD_ID)]
        public string DriverCardID { get; set; }

        [OpcItem(WeightBridge.Items.WEIGHT)]
        public decimal Weight { get; set; }
    }
    
    [OpcBlock(TGS.SCHEMA)]
    public class TankBlock
    {
        [OpcItem(TGS.Items.LIQUID_LEVEL)]
        public decimal? LiquidLevel { get; set; }

        [OpcItem(TGS.Items.LIQUID_OBSERVED_DENSITY)]
        public decimal? LiquidObservedDensity { get; set; }

        [OpcItem(TGS.Items.LIQUID_REFERENCE_DENSITY)]
        public decimal? LiquidReferenceDensity { get; set; }

        [OpcItem(TGS.Items.LIQUID_TEMPERATURE)]
        public decimal? LiquidTemperature { get; set; }

        [OpcItem(TGS.Items.LIQUID_TOV)]
        public decimal? LiquidTOV { get; set; }

        [OpcItem(TGS.Items.LIQUID_GOV)]
        public decimal? LiquidGOV { get; set; }

        [OpcItem(TGS.Items.LIQUID_GSV)]
        public decimal? LiquidGSV { get; set; }

        [OpcItem(TGS.Items.LIQUID_NSV)]
        public decimal? LiquidNSV { get; set; }

        [OpcItem(TGS.Items.LIQUID_MASS)]
        public decimal? LiquidMass { get; set; }

        [OpcItem(TGS.Items.WATER_LEVEL)]
        public decimal? WaterLevel { get; set; }

        [OpcItem(TGS.Items.WATER_VOLUME)]
        public decimal? WaterVolume { get; set; }

        [OpcItem(TGS.Items.VAPOUR_TEMPERATURE)]
        public decimal? VapourTemperature { get; set; }

        [OpcItem(TGS.Items.VAPOUR_PRESSURE)]
        public decimal? VapourPressure { get; set; }

        [OpcItem(TGS.Items.VAPOUR_MASS)]
        public decimal? VapourMass { get; set; }

        [OpcItem(TGS.Items.TOTAL_MASS)]
        public decimal? TotalMass { get; set; }
    }
}