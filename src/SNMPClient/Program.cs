using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;


namespace SNMPClient
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string ipAddress = "127.0.0.0"; // Replace with the target server IP address
            string communityString = "public"; // Replace with your SNMP community string
            int snmpVersion = 1; // Use SNMP version 1, 2, or 3 as appropriate
            int timeout = 5000; // Timeout in milliseconds

            List<Variable> result = GetSnmpData(ipAddress, communityString, snmpVersion, timeout);

            foreach (Variable variable in result)
            {
                Console.WriteLine($"{variable.Id}: {variable.Data}");
            }
        }

        public static List<Variable> WalkSnmpData(string ipAddress, string communityString, int snmpVersion, int timeout)
        {
            // Prepare SNMP version
            VersionCode version;
            switch (snmpVersion)
            {
                case 1:
                    version = VersionCode.V1;
                    break;
                case 2:
                    version = VersionCode.V2;
                    break;
                case 3:
                    version = VersionCode.V3;
                    break;
                default:
                    throw new ArgumentException("Invalid SNMP version.");
            }

            // Perform SNMP walk
            List<Variable> result = new List<Variable>();
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), 161);
                ObjectIdentifier rootOid = new ObjectIdentifier("1.3"); // SNMP OID root
                Messenger.Walk(version, endpoint, new OctetString(communityString), rootOid, result,timeout, WalkMode.WithinSubtree);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return result;
        }

        public static List<Variable> GetSnmpData(string ipAddress, string communityString, int snmpVersion, int timeout)
        {
            // Replace these OIDs with the ones you're interested in
            List<Variable> oids = new List<Variable>
    {
        new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")), // sysDescr
        new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")), // sysUpTime
        new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.5.0")), // sysName
    };

            // Prepare SNMP version
            VersionCode version;
            switch (snmpVersion)
            {
                case 1:
                    version = VersionCode.V1;
                    break;
                case 2:
                    version = VersionCode.V2;
                    break;
                case 3:
                    version = VersionCode.V3;
                    break;
                default:
                    throw new ArgumentException("Invalid SNMP version.");
            }

            // Perform SNMP GET request
            List<Variable> result = new List<Variable>();
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), 161);
                result.AddRange(Messenger.Get(version, endpoint, new OctetString(communityString), oids, timeout));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return result;
        }


    }
}
