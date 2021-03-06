﻿/*
* ==============================================================================
*
* Filename: LazynetClientInitalizer
* Description: 
*
* Version: 1.0
* Created: 2020/5/4 0:54:13
* Compiler: Visual Studio 2010
*
* Author: Your name
* Company: Your company name
*
* ==============================================================================
*/
using DotNetty.Codecs;
using DotNetty.Codecs.Http;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Handlers.Streams;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazynet.Core.Network.Client
{
    public class LazynetClientInitalizer : ChannelInitializer<IChannel>
    {
        public LazynetSocketType SocketType { get; }
        public string WebsocketPath { get; }
        public ILazynetSocketEvent SocketEvent { get; }

        public LazynetClientInitalizer(LazynetClientConfig config, ILazynetSocketEvent socketEvent)
        {
            this.SocketType = config.SocketType;
            this.WebsocketPath = config.WebsocketPath;
            this.SocketEvent = socketEvent;
        }

        protected override void InitChannel(IChannel channel)
        {

            IChannelPipeline pipeline = channel.Pipeline;
            if (SocketType == LazynetSocketType.TcpSocket)
            {
                pipeline.AddLast(new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 4, 0, 4));
                pipeline.AddLast(new LengthFieldPrepender(4));
                pipeline.AddLast(new StringDecoder(Encoding.UTF8));
                pipeline.AddLast(new StringEncoder(Encoding.UTF8));
                pipeline.AddLast(new LazynetTSClientHandler(SocketEvent));
            }
            else if (SocketType == LazynetSocketType.Websocket)
            {
                pipeline.AddLast(new HttpServerCodec());
                pipeline.AddLast(new ChunkedWriteHandler<byte>());
                pipeline.AddLast(new HttpObjectAggregator(8192));
                pipeline.AddLast(new WebSocketServerProtocolHandler(this.WebsocketPath));
                pipeline.AddLast(new LazynetWSClientHandler(SocketEvent));
            }
        }


    }
}
