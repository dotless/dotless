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

"""This code sample creates a new website given an existing ad group. To
create an ad group, you can run add_ad_group.py."""

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

# Create new website structure.
ad_group_id = long('INSERT_AD_GROUP_ID_HERE')
website = {
  'adGroupId': ad_group_id,
  'criterionType': SOAPpy.Types.untypedType('Website'),
  'url': 'example.com'
}

# Check new website for policy violations before adding it.
language_target = {'languages': ['en']}
geo_target = {'countryTargets': {'countries': ['US']}}
errors = criterion_service.checkCriteria([website], language_target, geo_target)

# Convert to a list if we get back a single object.
if len(errors) > 0 and not isinstance(errors, list):
  errors = [errors]

# Add website if there are no policy violations.
if len(errors) == 0:
  criteria = criterion_service.addCriteria([website])

  # Convert to a list if we get back a single object.
  if len(criteria) > 0 and not isinstance(criteria, list):
    criteria = [criteria]

  # Display new website.
  for criterion in criteria:
    print 'New website with url "%s" and id "%s" was created.' % \
        (criterion['url'], criterion['id'])
else:
  print 'New website was not created due to the following policy violations:'
  for error in errors:
    print '  Detail: %s\nisExemptable: %s' % \
        (error['detail'], error['isExemptable'])
    print
