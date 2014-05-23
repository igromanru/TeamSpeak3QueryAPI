﻿using System;
using System.Diagnostics;
using System.IO;
using TeamSpeak3QueryApi.Net.Specialized;
using TeamSpeak3QueryApi.Net.Specialized.Notifications;

namespace TeamSpeak3QueryApi.Net.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            DoItRich();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        static async void DoItRich()
        {
            var loginData = File.ReadAllLines("..\\..\\..\\logindata.secret");

            var host = loginData[0].Trim();
            var user = loginData[1].Trim();
            var password = loginData[2].Trim();

            var rc = new TeamSpeakClient(host);

            await rc.Connect();

            await rc.Client.Send("login", new Parameter("client_login_name", user), new Parameter("client_login_password", password));
            await rc.Client.Send("use", new Parameter("sid", 1));
            await rc.Client.Send("whoami");
            await rc.Client.Send("servernotifyregister", new Parameter("event", "server"));//, new Parameter("id", "30"));
            await rc.Client.Send("servernotifyregister", new Parameter("event", "channel"), new Parameter("id", "30"));
            rc.Subscribe<ClientEnterView>(NotificationType.ClientEnterView, data =>
                                                                            {
                                                                                foreach (var i in data)
                                                                                    Trace.WriteLine("Client " + i.ClientNickName + " joined.");
                                                                            });
            rc.Subscribe<ClientLeftView>(NotificationType.ClientLeftView, data =>
                                                                            {
                                                                                foreach (var i in data)
                                                                                    Trace.WriteLine("Client with id " + i.ClientId + " left (kicked/banned/left).");
                                                                            });
            rc.Subscribe<ServerEdited>(NotificationType.ServerEdited, data =>
                                                                       {
                                                                           Debugger.Break();
                                                                       });
            rc.Subscribe<ChannelEdited>(NotificationType.ChannelEdited, data =>
                                                                       {
                                                                           Debugger.Break();
                                                                       });
            rc.Subscribe<ClientMoved>(NotificationType.ClientMoved, data =>
                                                                       {
                                                                           Debugger.Break();
                                                                       });

            Console.WriteLine("Done1");
        }

        /*
        static async void DoIt()
        {
            var loginData = File.ReadAllLines("..\\..\\..\\logindata.secret");

            var host = loginData[0].Trim();
            var user = loginData[1].Trim();
            var password = loginData[2].Trim();

            var cl = new QueryClient(host);

            await cl.Connect();
            await cl.Send("login", new Parameter("client_login_name", user), new Parameter("client_login_password", password));
            await cl.Send("use", new Parameter("sid", 1));
            await cl.Send("whoami");

            //await cl.Send("servernotifyregister", new[] { "event", "channel" }, new[] { "id", "24" });
            await cl.Send("servernotifyregister", new Parameter("event", "channel"), new Parameter("id", "24"));

            cl.Subscribe("clientmoved", data =>
                                        {
                                            Console.WriteLine("Some client moved!");
                                            cl.Unsubscribe("clientmoved");
                                            cl.Send("servernotifyunregister", new[] { "event", "channel" }, new[] { "id", "24" });
                                        });
            // cl.Unsubscribe("message");

            Console.WriteLine("Done1");
        }
        */
    }
}
