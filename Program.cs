using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(o =>
                {
                    var logger = new SimpleLogger(o.OutputFile);
                    while (true)
                    {
                        var response = PingHost(o.PingTarget, o.Timeout, logger);
                        if (response == null)
                        {
                            return;
                        }
                        if (response.Status == IPStatus.Success)
                        {
                            logger.Info($"Ping succesful: {response.RoundtripTime}ms. roundtrip. [{response.Address}, {response.Status.ToString()}]");
                        }
                        if (response.Status != IPStatus.Success)
                        {
                            logger.Error($"Ping failed: {response.Status.ToString()}");
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                });
        }

        // https://stackoverflow.com/questions/11800958/using-ping-in-c-sharp
        private static PingReply PingHost(string nameOrAddress, int? timeout, SimpleLogger logger)
        {
            Ping pinger = null;
            PingReply result = null;

            try
            {
                pinger = new Ping();
                if (timeout == null)
                {
                    result = pinger.Send(nameOrAddress);
                } 
                else
                {
                    result = pinger.Send(nameOrAddress, timeout.Value);
                }
            }
            catch (PingException e)
            {
                logger.Fatal("Exception: " + e.ToString());
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return result;
        }
    }
}
