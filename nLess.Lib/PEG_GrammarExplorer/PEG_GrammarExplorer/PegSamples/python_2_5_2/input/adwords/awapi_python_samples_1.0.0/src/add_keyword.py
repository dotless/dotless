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

"""This code sample creates a new keyword given an existing ad group. To create
an ad group, you can run add_ad_group.py."""

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
# criterion_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
criterion_service = SOAPpy.SOAPProxy(namespace + '/CriterionService',
                                     header=headers)
criterion_service.config.debug = 0

# Create new keyword structure.
ad_group_id = long('INSERT_AD_GROUP_ID_HERE')
keyword = {
  'adGroupId': ad_group_id,
  'criterionType': SOAPpy.Types.untypedType('Keyword'),
  'text': 'mars cruise',
  'type': SOAPpy.Types.untypedType('Broad')
}

# Check new keyword for policy violations before adding it.
language_target = {'languages': ['en']}
geo_target = {'countryTargets': {'countries': ['US']}}
errors = criterion_service.checkCriteria([keyword], language_target, geo_target)

# Convert to a list if we get back a single object.
if len(errors) > 0 and not isinstance(errors, list):
  errors = [errors]

# Add keyword if there are no policy violations.
if len(errors) == 0:
  criteria = criterion_service.addCriteria([keyword])

  # Convert to a list if we get back a single object.
  if len(criteria) > 0 and not isinstance(criteria, list):
    criteria = [criteria]

  # Display new keyword.
  for criterion in criteria:
    print 'New keyword with text "%s" and id "%s" was created.' % \
        (criterion['text'], criterion['id'])
else:
  print 'New keyword was not created due to the following policy violations:'
  for error in errors:
    print '  Detail: %s\nisExemptable: %s' % \
        (error['detail'], error['isExemptable'])
    print
