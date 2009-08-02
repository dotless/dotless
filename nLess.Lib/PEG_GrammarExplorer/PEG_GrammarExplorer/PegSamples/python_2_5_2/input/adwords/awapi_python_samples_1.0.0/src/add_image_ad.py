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

"""This code sample creates a new image ad given an existing ad group. To
create an ad group, you can run add_ad_group.py."""

import base64
import urllib
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

# Create new image ad structure.
ad_group_id = long('INSERT_AD_GROUP_ID_HERE')
image = open('INSERT_IMAGE_PATH_HERE', 'r').read()
image_ad = {
  'adGroupId': ad_group_id,
  'adType': SOAPpy.Types.untypedType('ImageAd'),
  'displayUrl': 'www.example.com',
  'destinationUrl': 'http://www.example.com',
  'image': {'data': SOAPpy.Types.untypedType(base64.encodestring(image)),
            'name': SOAPpy.Types.untypedType('image_ad')}
}

# Check new ad for policy violations before adding it.
language_target = {'languages': ['en']}
geo_target = {'countryTargets': {'countries': ['US']}}
errors = ad_service.checkAds([image_ad], language_target, geo_target)

# Convert to a list if we get back a single object.
if len(errors) > 0 and not isinstance(errors, list):
  errors = [errors]

# Add image ad if there are no policy violations.
if len(errors) == 0:
  ads = ad_service.addAds([image_ad])

  # Convert to a list if we get back a single object.
  if len(ads) > 0 and not isinstance(ads, list):
    ads = [ads]

  # Display new image ad.
  for ad in ads:
    print 'New image ad with displayUrl "%s" and id "%s" was ' \
        'created.' % (ad['displayUrl'], ad['id'])
else:
  print 'New image ad was not created due to the following policy ' \
      'violations:'
  for error in errors:
    print '  Detail: %s\nisExemptable: %s' % \
        (error['detail'], error['isExemptable'])
    print
