using System;

namespace OpcHub.Ae.Contract
{
    public class AeEvent
    {
        public AeEvent(
            string source,
            string message,
            DateTime eventTime,
            DateTime stationTimeGMT,
            string stationNameOfOpcServer,
            string stationNameOfFCS)
        {
            Source = source;
            Message = message;
            EventTime = eventTime;
            StationTimeGMT = stationTimeGMT;
            StationNameOfOpcServer = stationNameOfOpcServer;
            StationNameOfFCS = stationNameOfFCS;
        }

        /// <summary>
        /// example: %PR10525S050101
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// example: JOBCON1 CP2007
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// event time (based on opc server machine time)
        /// </summary>
        public DateTime EventTime { get; }

        /// <summary>
        /// actual event generation timestamp
        /// </summary>
        public DateTime StationTimeGMT { get; }

        public string StationNameOfOpcServer { get; }

        public string StationNameOfFCS { get; }

        public bool IsSameEvent(AeEvent aeEvent)
        {
            if (aeEvent == null) return false;

            return Source == aeEvent.Source &&
                   Message == aeEvent.Message &&
                   StationTimeGMT == aeEvent.StationTimeGMT;
        }

        public AeEvent Clone()
        {
            return new AeEvent(Source, Message, EventTime, StationTimeGMT, StationNameOfOpcServer, StationNameOfFCS);
        }
    }
}
