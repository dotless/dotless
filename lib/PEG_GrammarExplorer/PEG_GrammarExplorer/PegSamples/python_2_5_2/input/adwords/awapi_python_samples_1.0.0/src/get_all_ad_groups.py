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

"""This code sample retrieves information about all ad groups given an existing
campaign. To create a campaign, you can run add_campaign.py."""

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
# ad_group_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
ad_group_service = SOAPpy.SOAPProxy(namespace + '/AdGroupService',
                                    header=headers)
ad_group_service.config.debug = 0

# Get all ad groups.
campaign_id = int('INSERT_CAMPAIGN_ID_HERE')
ad_groups = ad_group_service.getAllAdGroups(campaign_id)

# Convert to a list if we get back a single object.
if len(ad_groups) > 0 and not isinstance(ad_groups, list):
  ad_groups = [ad_groups]

# Display ad group info.
print 'Campaign with id "%s" has %s ad group(s).' % \
    (campaign_id, len(ad_groups))
for ad_group in ad_groups:
  print '  Ad group name is "%s" and id is "%s".' % \
      (ad_group['name'], ad_group['id'])
