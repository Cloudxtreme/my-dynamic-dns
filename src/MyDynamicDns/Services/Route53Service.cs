using System.Collections.Generic;
using System.Net;
using Amazon.Route53;
using Amazon.Route53.Model;

namespace MyDynamicDns.Services
{
    public static class Route53Service
    {
        public static void UpdateRoute53Record(string hostname, IPAddress newIp)
        {
            var route53Client = new AmazonRoute53Client(
                Settings.AwsAccessKeyId,
                Settings.AwsSecretAccessKey,
                Amazon.RegionEndpoint.USEast1);

            var recordSet = new ResourceRecordSet
            {
                Name = hostname,
                TTL = 60,
                Type = RRType.A,
                ResourceRecords = new List<ResourceRecord>
                {
                    new ResourceRecord {Value = newIp.ToString()}
                }
            };

            var changeBatch = new ChangeBatch
            {
                Changes = new List<Change>
                {
                    new Change
                    {
                        ResourceRecordSet = recordSet,
                        Action = ChangeAction.UPSERT
                    }
                }
            };

            var recordsetRequest = new ChangeResourceRecordSetsRequest
            {
                HostedZoneId = Settings.HostedZoneId,
                ChangeBatch = changeBatch
            };

            route53Client.ChangeResourceRecordSets(recordsetRequest);
        }
    }
}