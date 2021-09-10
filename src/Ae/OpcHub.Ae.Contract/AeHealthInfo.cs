using System;

namespace OpcHub.Ae.Contract
{
    public class AeHealthInfo //: IEquatable<AeHealthInfo>
    {
        #region Ctor
        public AeHealthInfo(
            string stationName,
            AeState state,
            AeFailureType failureType,
            string failureReason,
            DateTime timestamp)
        {
            StationName = stationName;
            State = state;
            FailureType = failureType;
            FailureReason = failureReason;
            Timestamp = timestamp;
        }
        #endregion

        #region Properties
        public string StationName { get; }

        public AeState State { get; private set; }

        public AeFailureType FailureType { get; private set; }

        public string FailureReason { get; private set; }

        public DateTime Timestamp { get; private set; }
        #endregion

        #region Methods
        public void SetNormal()
        {
            Timestamp = DateTime.Now;
            State = AeState.Normal;
            FailureType = AeFailureType.None;
            FailureReason = null;
        }

        public void SetFailed(AeFailureType failureType, string failureReason)
        {
            Timestamp = DateTime.Now;
            State = AeState.Failed;
            FailureType = failureType;
            FailureReason = failureReason;
        }

        public AeHealthInfo Clone()
        {
            return new AeHealthInfo(
                StationName,
                State,
                FailureType,
                FailureReason,
                Timestamp);
        }

        #endregion

        #region IEquatable Members
        //public bool Equals(AeHealthInfo other)
        //{
        //    if (ReferenceEquals(null, other)) return false;
        //    if (ReferenceEquals(this, other)) return true;
        //    return string.Equals(StationName, other.StationName) && 
        //           State == other.State && 
        //           FailureType == other.FailureType && 
        //           string.Equals(FailureReason, other.FailureReason);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(null, obj)) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != this.GetType()) return false;
        //    return Equals((AeHealthInfo) obj);
        //}

        //public override int GetHashCode()
        //{
        //    unchecked
        //    {
        //        var hashCode = (StationName != null ? StationName.GetHashCode() : 0);
        //        hashCode = (hashCode * 397) ^ (int) State;
        //        hashCode = (hashCode * 397) ^ (int) FailureType;
        //        hashCode = (hashCode * 397) ^ (FailureReason != null ? FailureReason.GetHashCode() : 0);
        //        return hashCode;
        //    }
        //}
        #endregion
    }
}
