using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Common.Helper
{
    public interface IServiceChannel<T>
    {
        TResult Fetch<TResult>(Func<T, TResult> block);
        void Execute(Action<T> block);
    }
    public class ServiceChannel<T> : IServiceChannel<T>
    {
        private ChannelFactory<T> _channelFactory;
        public ServiceChannel(ChannelFactory<T> channelFactory)
        {
            _channelFactory = channelFactory;
        }

        public TResult Fetch<TResult>(Func<T, TResult> block)
        {
            IClientChannel channel = (IClientChannel)_channelFactory.CreateChannel();
            bool isSuccessful = false;
            try
            {
                TResult result = block((T)channel);
                channel.Close();
                isSuccessful = true;
                return result;
            }
            finally
            {
                if (!isSuccessful)
                {
                    channel.Abort();
                }
            }
        }

        public void Execute(Action<T> block)
        {
            IClientChannel channel = (IClientChannel)_channelFactory.CreateChannel();
            bool isSuccessful = false;
            try
            {
                block((T)channel);
                channel.Close();
                isSuccessful = true;
            }
            finally
            {
                if (!isSuccessful)
                {
                    channel.Abort();
                }
            }
        }

        public Task<TResult> FetchAsync<TResult>(Func<T, Task<TResult>> block)
        {
            IClientChannel channel = (IClientChannel)_channelFactory.CreateChannel();
            Task<TResult> result = block((T)channel);
            result.ContinueWith(t => ContinueFromAsync(t, channel));
            return result;
        }

        public Task ExecuteAsync(Func<T, Task> block)
        {
            IClientChannel channel = (IClientChannel)_channelFactory.CreateChannel();
            Task result = block((T)channel);
            result.ContinueWith(t => ContinueFromAsync(t, channel));
            return result;
        }


        public Task<TResult> FetchAsync<TResult>(Func<T, TResult> block)
        {
            IClientChannel channel = (IClientChannel)_channelFactory.CreateChannel();
            Task<TResult> result = Task.Factory.StartNew(() => block((T)channel));
            result.ContinueWith(t => ContinueFromAsync(t, channel));
            return result;
        }

        public Task ExecuteAsync(Action<T> block)
        {
            IClientChannel channel = (IClientChannel)_channelFactory.CreateChannel();
            Task result = Task.Factory.StartNew(() => block((T)channel));
            result.ContinueWith(t => ContinueFromAsync(t, channel));
            return result;
        }

        private void ContinueFromAsync(Task t, IClientChannel channel)
        {
            bool isSuccessful = false;

            try
            {
                if (t.IsFaulted == false)
                {
                    channel.Close();
                    isSuccessful = true;
                }
            }
            finally
            {
                if (!isSuccessful)
                {
                    channel.Abort();
                }
            }
        }
    }
}
