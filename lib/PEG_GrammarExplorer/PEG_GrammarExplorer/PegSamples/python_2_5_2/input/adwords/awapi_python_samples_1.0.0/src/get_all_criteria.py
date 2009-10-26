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

"""This code sample retrieves all criteria from the AdWords account that
belongs to the customer issuing the request."""

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
# campaign_service/ad_group_service/criterion_service.config.debug to 1.
# To send requests to production environment, replace "sandbox.google.com"
# with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
campaign_service = SOAPpy.SOAPProxy(namespace + '/CampaignService',
                                    header=headers)
campaign_service.config.debug = 0
ad_group_service = SOAPpy.SOAPProxy(namespace + '/AdGroupService',
                                    header=headers)
ad_group_service.config.debug = 0
criterion_service = SOAPpy.SOAPProxy(namespace + '/CriterionService',
                                     header=headers)
criterion_service.config.debug = 0

# Get all campaigns.
campaigns = campaign_service.getAllAdWordsCampaigns(0)

# Convert to a list if we get back a single object.
if len(campaigns) > 0 and not isinstance(campaigns, list):
  campaigns = [campaigns]

count = 0
for campaign in campaigns:
  # Get all ad groups.
  ad_groups = ad_group_service.getAllAdGroups(int(campaign['id']))

  # Convert to a list if we get back a single object.
  if len(ad_groups) > 0 and not isinstance(ad_groups, list):
    ad_groups = [ad_groups]

  for ad_group in ad_groups:
    # Get all criteria.
    criteria = criterion_service.getAllCriteria(
        SOAPpy.Types.untypedType(str(ad_group['id'])))

    # Convert to a list if we get back a single object.
    if len(criteria) > 0 and not isinstance(criteria, list):
      criteria = [criteria]

    for criterion in criteria:
      # Display criteria info.
      count += 1
      print 'Campaign id is "%s", ad group id is "%s", id is "%s", and ' \
          'type is "%s".' % (campaign['id'], ad_group['id'], criterion['id'],
              criterion['criterionType'])

print 'Account has %s criteria.' % (count)
