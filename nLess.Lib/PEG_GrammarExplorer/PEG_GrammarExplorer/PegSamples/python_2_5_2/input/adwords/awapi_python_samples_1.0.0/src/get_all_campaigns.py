#!/usr/bin/python
#
# Copyright 2008, Google Inc. All Rights Reserved.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

"""This code sample retrieves information about all campaigns that belongs to
the customer issuing the request."""

import SOAPpy


# Provide AdWords login information.
email = 'INSERT_LOGIN_EMAIL_HERE'
password = 'INSERT_PASSWORD_HERE'
client_email = 'INSERT_CLIENT_LOGIN_EMAIL_HERE'
useragent = 'INSERT_COMPANY_NAME: AdWords API Python Sample Code'
developer_token = 'INSERT_DEVELOPER_TOKEN_HERE'
application_token = 'INSERT_APPLICATION_TOKEN_HERE'

# Define SOAP headers.
headers = SOAPpy.Types.headerType()
headers.email = email
headers.password = password
headers.clientEmail = client_email
headers.useragent = useragent
headers.developerToken = developer_token
headers.applicationToken = application_token

# Set up service connection. To view XML request/response, change value of
# campaign_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
campaign_service = SOAPpy.SOAPProxy(namespace + '/CampaignService',
                                    header=headers)
campaign_service.config.debug = 0

# Get all campaigns.
campaigns = campaign_service.getAllAdWordsCampaigns(0)

# Convert to a list if we get back a single object.
if len(campaigns) > 0 and not isinstance(campaigns, list):
  campaigns = [campaigns]

# Display campaign info.
for campaign in campaigns:
  print 'Campaign name is "%s" id is "%s".' % (campaign['name'], campaign['id'])
