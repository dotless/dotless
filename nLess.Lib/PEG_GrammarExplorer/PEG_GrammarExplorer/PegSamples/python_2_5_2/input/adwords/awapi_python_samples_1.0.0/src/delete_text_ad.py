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

"""This code sample deletes an existing ad given an existing ad group. To create
an ad group, you can run add_ad_group.py. To create an ad, you can run
add_text_ad.py."""

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
# ad_service.config.debug to 1. To send requests to production
# environment, replace "sandbox.google.com" with "adwords.google.com".
namespace = 'https://sandbox.google.com/api/adwords/v12'
ad_service = SOAPpy.SOAPProxy(namespace + '/AdService',
                              header=headers)
ad_service.config.debug = 0

# Create existing ad structure.
ad_group_id = long('INSERT_AD_GROUP_ID_HERE')
ad_id = long('INSERT_AD_ID_HERE')
ad = {
  'adGroupId': ad_group_id,
  'adType': SOAPpy.Types.untypedType('TextAd'),
  'id': SOAPpy.Types.untypedType(str(ad_id)),
  'status': SOAPpy.Types.untypedType('Disabled')
}

# Delete ad.
ad_service.updateAds([ad])

# Display deleted ad.
print 'Ad with id "%s" was deleted.' % (ad_id)
