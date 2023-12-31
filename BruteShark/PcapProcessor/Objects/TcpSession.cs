﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PcapProcessor
{
    public class TcpSession : NetworkSession<TcpPacket>
    {
        public byte[] Data { get; set; }
        public override List<TcpPacket> Packets { get; set; }

        public TcpSession()
        {
            this.Packets = new List<TcpPacket>();
            this.Protocol = "TCP";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TcpSession))
            {
                return false;
            }

            var session = obj as TcpSession;

            // Note: we need to check both directions of the conversation to 
            // determine equality.
            return (this.SourceIp == session.SourceIp &&
                    this.DestinationIp == session.DestinationIp &&
                    this.SourcePort == session.SourcePort &&
                    this.DestinationPort == session.DestinationPort) ||
                    (this.SourceIp == session.DestinationIp &&
                    this.DestinationIp == session.SourceIp &&
                    this.SourcePort == session.DestinationPort &&
                    this.DestinationPort == session.SourcePort);
        }

        public override int GetHashCode()
        {
            return this.SourceIp.GetHashCode() ^
                   this.SourcePort.GetHashCode() ^
                   this.DestinationPort.GetHashCode() ^
                   this.DestinationIp.GetHashCode();
        }

    }
}