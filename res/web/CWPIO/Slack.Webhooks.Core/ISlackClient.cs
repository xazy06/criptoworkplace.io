using System;
using System.Collections.Generic;

#if NET40
using System.Threading.Tasks;
using RestSharp;
#endif

namespace Slack.Webhooks
{
	public interface ISlackClient
	{
		bool Post(SlackMessage slackMessage);
		bool PostToChannels(SlackMessage message, IEnumerable<string> channels);
	}
}
