namespace OpcHub.Ae.Contract
{
    public enum AeFailureType
    {
        None = 0,

        ///// <summary>
        ///// Detected by ae.server.ServerShutdown event in "EventHub.Client"
        ///// </summary>
        //OPCServerShutdown = 1,

        /// <summary>
        /// Detected in "EventHub.Middleware"
        /// </summary>
        AeSenderDisconnected = 2,

        /// <summary>
        /// Detected by checking the ae.server.Status in "EventHub.Client"
        /// </summary>
        OPCServerNotConnected = 3,

        /// <summary>
        /// Detected by watch-dog in "EventHub.Client"
        /// </summary>
        AeNoWDTEventReceived = 4
    }
}
