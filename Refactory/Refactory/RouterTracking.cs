﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Refactory
{
    class RouterTracking
    {

        public RouterTracking()
        {

        }

        /// <summary>
        /// TraceRoute ip or hostname
        /// </summary>
        /// <param name="ipAddressOrHostName"></param>
        /// <returns></returns>
        public string Traceroute(string ipAddressOrHostName)
        {
            IPAddress ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];
            StringBuilder traceResults = new StringBuilder();


            using (Ping pingSender = new Ping())
            {

                PingOptions pingOptions = new PingOptions();
                Stopwatch stopWatch = new Stopwatch();
                byte[] bytes = new byte[32];

                pingOptions.DontFragment = true;
                pingOptions.Ttl = 1;
                int maxHops = 30;

                traceResults.AppendLine(
                    string.Format(
                        "Tracing route to {0} over a maximum of {1} hops:",
                        ipAddress,
                        maxHops));

                traceResults.AppendLine();

                for (int i = 1; i < maxHops + 1; i++)
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                    PingReply pingReply = pingSender.Send(
                        ipAddress,
                        5000,
                        new byte[32], pingOptions);

                    stopWatch.Stop();

                    traceResults.AppendLine(
                        string.Format("{0}\t{1} ms\t{2}",
                        i,
                        stopWatch.ElapsedMilliseconds,
                        pingReply.Address));



                    if (pingReply.Status == IPStatus.Success)
                    {
                        traceResults.AppendLine();
                        traceResults.AppendLine("Trace complete."); break;
                    }

                    pingOptions.Ttl++;

                }
            }
            return traceResults.ToString();
        }


        /// <summary>
        /// Display the dhcp servers
        /// </summary>
       public string DisplayDhcpServerAddresses()
        {
            string returnText = "";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapteradapterProperties = adapter.GetIPProperties();
                IPAddressCollection addresses = adapteradapterProperties.DhcpServerAddresses;
                if (addresses.Count > 0)
                {
                    Console.WriteLine(adapter.Description);
                    foreach (IPAddress address in addresses)
                    {
                        returnText += $"Dhcp Address ............................ : {address.ToString()}\n";
                    }
                  
                }
            }

            return returnText;
        }


    }
}
