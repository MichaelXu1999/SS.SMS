﻿using System;
using System.Collections.Generic;
using SS.SMS.Core.AliYun.Acs.Core.Auth;
using SS.SMS.Core.AliYun.Acs.Core.Regions.Location;
using SS.SMS.Core.AliYun.Acs.Core.Regions.Location.Model;

namespace SS.SMS.Core.AliYun.Acs.Core.Regions
{
    class RemoteEndpointsParser
    {
        private DescribeEndpointService describeEndpointService;

        public void SetDescribeEndpointService(DescribeEndpointService describeEndpointService)
        {
            this.describeEndpointService = describeEndpointService;
        }

        public static RemoteEndpointsParser InitRemoteEndpointsParser()
        {
            RemoteEndpointsParser parser = new RemoteEndpointsParser();
            parser.SetDescribeEndpointService(new DescribeEndpointService());
            return parser;
        }

        public Endpoint GetEndpoint(String regionId, String product, String locationProduct,
                                    Credential credential, LocationConfig locationConfig)
        {
            if (null == locationProduct)
            {
                return null;
            }
            Endpoint endpoint = null;

            DescribeEndpointResponse response = describeEndpointService.DescribeEndpoint(regionId, locationProduct,
                    credential, locationConfig);
            if (null == response)
            {
                return endpoint;
            }

            ISet<string> regionIds = new HashSet<String>();
            regionIds.Add(regionId);

            List<ProductDomain> productDomainList = new List<ProductDomain>();
            productDomainList.Add(new ProductDomain(product, response.Endpoint));
            endpoint = new Endpoint(response.RegionId, regionIds, productDomainList);
            return endpoint;
        }
    }
}
