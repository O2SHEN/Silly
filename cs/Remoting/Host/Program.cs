using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;
using System.Threading.Tasks;
using Shared;
using System.Collections;
using System.Runtime.Remoting.Channels.Tcp;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            // Insert .NET Remoting code.
            // Register a listening channel.
            #region programmatically configured
            #region using tcp channel
            //Hashtable props = new Hashtable();
            //props["port"] = 1234;

            ////Set up for remoting events properly
            //BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
            //serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            //TcpChannel oJobChannel = new TcpChannel(props, null, serverProv);


            ////HttpChannel oJobChannel = new HttpChannel(1234);
            //ChannelServices.RegisterChannel(oJobChannel, false);
            //// Register a well−known type.
            //RemotingConfiguration.RegisterWellKnownServiceType(
            //typeof(JobServerImpl),
            //"JobURI",
            //WellKnownObjectMode.Singleton);
            #endregion

            #region using http channel 
            Hashtable props = new Hashtable();
            props["port"] = 1234;

            //Set up for remoting events properly
            SoapServerFormatterSinkProvider serverProv = new SoapServerFormatterSinkProvider();
            serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

            HttpChannel oJobChannel = new HttpChannel(props, null, serverProv);


            //HttpChannel oJobChannel = new HttpChannel(1234);
            ChannelServices.RegisterChannel(oJobChannel, false);
            // Register a well−known type.
            RemotingConfiguration.RegisterWellKnownServiceType(
            typeof(JobServerImpl),
            "JobURI",
            WellKnownObjectMode.Singleton);
            #endregion
            #endregion

            #region config file configured
            //RemotingConfiguration.Configure("Host.exe.config");
            #endregion

            // Keep running until told to quit.
            System.Console.WriteLine("Press Enter to exit");
            // Wait for user to press the Enter key.
            System.Console.ReadLine();
        }
    }
}
